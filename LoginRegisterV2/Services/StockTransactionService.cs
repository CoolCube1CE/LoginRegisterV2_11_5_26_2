using System.Data;

namespace LoginRegisterV2
{
    /// <summary>
    /// Stock moving in or out by hand. The rules for a move live in
    /// StockMovementService; this class decides what a MANUAL move looks like
    /// (its reference label) and runs it as one all-or-nothing transaction.
    /// </summary>
    public class StockTransactionService
    {
        private readonly StockItemRepository _items = new StockItemRepository();
        private readonly StockTransactionRepository _transactions = new StockTransactionRepository();
        private readonly StockMovementService _movements = new StockMovementService();

        public DataTable GetAllTransactions()
        {
            return _transactions.GetAll();
        }

        public OperationResult RecordManual(StockMovementInput input)
        {
            if (!input.Quantity.HasValue || input.Quantity.Value <= 0)
                return OperationResult.Fail("Quantity must be a valid number greater than zero.");

            int quantity = input.Quantity.Value;
            bool isStockIn = input.Type == StockTransactionType.In;

            return SqlRunner.InTransaction((conn, tx) =>
            {
                StockLevel level = _items.GetLevel(conn, tx, input.ItemId);

                OperationResult check = _movements.CheckCanMove(level, input.Type, quantity);
                if (!check.Success) return check;

                _movements.Apply(conn, tx, new StockTransactionEntry
                {
                    ItemId = input.ItemId,
                    Type = input.Type,
                    Quantity = quantity,
                    Reference = "Manual " + (isStockIn ? "Restock" : "Adjustment"),
                    PerformedBy = input.PerformedBy,
                    Notes = input.Notes
                });

                return OperationResult.Ok();
            });
        }
    }
}
