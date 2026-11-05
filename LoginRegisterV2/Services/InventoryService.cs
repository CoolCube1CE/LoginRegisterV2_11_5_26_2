using System.Data;

namespace LoginRegisterV2
{
    /// <summary>
    /// What can be done to the list of stock items: view, add, edit, delete.
    /// (Changing quantities is StockTransactionService's job, and orders are OrderService's.)
    /// </summary>
    public class InventoryService
    {
        private readonly StockItemRepository _items = new StockItemRepository();
        private readonly StockTransactionRepository _transactions = new StockTransactionRepository();
        private readonly OrderRepository _orders = new OrderRepository();
        private readonly StockItemValidator _validator = new StockItemValidator();

        public DataTable GetAllItems()
        {
            return _items.GetAll();
        }

        public DataTable GetItemsForDropdown()
        {
            return _items.GetForDropdown();
        }

        public OperationResult AddItem(StockItemInput input)
        {
            OperationResult check = _validator.ValidateNew(input);
            if (!check.Success) return check;

            _items.Insert(
                Clean(input.Name), Clean(input.Category), Clean(input.Sku),
                input.Quantity.Value, input.ReorderLevel.Value, input.UnitPrice.Value);

            return OperationResult.Ok();
        }

        public OperationResult UpdateItem(int itemId, StockItemInput input)
        {
            OperationResult check = _validator.ValidateEdit(input);
            if (!check.Success) return check;

            _items.Update(
                itemId, Clean(input.Name), Clean(input.Category), Clean(input.Sku),
                input.ReorderLevel.Value, input.UnitPrice.Value);

            return OperationResult.Ok();
        }

        /// <summary>
        /// Deletes the item together with its transactions and orders (they point at it),
        /// all or nothing.
        /// </summary>
        public void DeleteItem(int itemId)
        {
            SqlRunner.InTransaction((conn, tx) =>
            {
                _transactions.DeleteByItem(conn, tx, itemId);
                _orders.DeleteByItem(conn, tx, itemId);
                _items.Delete(conn, tx, itemId);
                return OperationResult.Ok();
            });
        }

        private static string Clean(string text)
        {
            return text == null ? null : text.Trim();
        }
    }
}
