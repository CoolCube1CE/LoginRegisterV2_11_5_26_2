using System.Data;

namespace LoginRegisterV2
{
    /// <summary>
    /// Customer orders: placing one (which reserves the stock) and changing its
    /// status. The stock-out rule and the history row come from StockMovementService.
    /// </summary>
    public class OrderService
    {
        private readonly StockItemRepository _items = new StockItemRepository();
        private readonly OrderRepository _orders = new OrderRepository();
        private readonly StockMovementService _movements = new StockMovementService();

        public DataTable GetAllOrders()
        {
            return _orders.GetAll();
        }

        public OperationResult PlaceOrder(OrderInput input)
        {
            if (!input.Quantity.HasValue || input.Quantity.Value <= 0)
                return OperationResult.Fail("Quantity must be a valid number greater than zero.");

            if (string.IsNullOrWhiteSpace(input.CustomerName))
                return OperationResult.Fail("Customer name is required.");

            int quantity = input.Quantity.Value;

            return SqlRunner.InTransaction((conn, tx) =>
            {
                StockLevel level = _items.GetLevel(conn, tx, input.ItemId);

                // Checked BEFORE the order row is inserted, so a refused order never uses up an order number.
                OperationResult check = _movements.CheckCanMove(level, StockTransactionType.Out, quantity);
                if (!check.Success) return check;

                int orderId = _orders.Insert(conn, tx, new NewOrder
                {
                    CustomerName = input.CustomerName.Trim(),
                    ItemId = input.ItemId,
                    Quantity = quantity,
                    UnitPrice = level.UnitPrice,
                    PlacedBy = input.PlacedBy
                });

                _movements.Apply(conn, tx, new StockTransactionEntry
                {
                    ItemId = input.ItemId,
                    Type = StockTransactionType.Out,
                    Quantity = quantity,
                    Reference = "Order #" + orderId,
                    PerformedBy = input.PlacedBy,
                    Notes = "Reserved for customer order"
                });

                return OperationResult.Ok();
            });
        }

        public void UpdateStatus(int orderId, string status)
        {
            _orders.UpdateStatus(orderId, status);
        }
    }
}
