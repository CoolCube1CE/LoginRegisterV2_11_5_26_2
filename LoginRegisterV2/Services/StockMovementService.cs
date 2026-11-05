using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>
    /// The one definition of "stock moves": can this much move, and how a move
    /// is recorded (the item's quantity changes AND a history row is written).
    /// Manual stock in/out and customer orders both use it, so the rule and the
    /// message exist once.
    /// </summary>
    public class StockMovementService
    {
        private readonly StockItemRepository _items = new StockItemRepository();
        private readonly StockTransactionRepository _transactions = new StockTransactionRepository();

        /// <summary>
        /// Checks a move before it happens. Pass the item's current level (null if
        /// the item does not exist). Taking stock out is refused when there is not enough.
        /// </summary>
        public OperationResult CheckCanMove(StockLevel level, string type, int quantity)
        {
            if (level == null)
                return OperationResult.Fail("Selected item was not found.");

            if (type == StockTransactionType.Out && quantity > level.Quantity)
                return OperationResult.Fail("Not enough stock on hand. Only " + level.Quantity + " unit(s) available.");

            return OperationResult.Ok();
        }

        /// <summary>
        /// Does the move inside the caller's transaction: changes the item's quantity
        /// (stock in adds, anything else subtracts) and writes the history row.
        /// Call CheckCanMove first.
        /// </summary>
        public void Apply(SqlConnection conn, SqlTransaction tx, StockTransactionEntry entry)
        {
            int delta = entry.Type == StockTransactionType.In ? entry.Quantity : -entry.Quantity;

            _items.AdjustQuantity(conn, tx, entry.ItemId, delta);
            _transactions.Insert(conn, tx, entry);
        }
    }
}
