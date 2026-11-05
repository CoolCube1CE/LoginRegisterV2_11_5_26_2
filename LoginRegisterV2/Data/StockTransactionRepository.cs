using System.Data;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>All SQL for the StockTransactions table, and nothing else.</summary>
    public class StockTransactionRepository
    {
        public DataTable GetAll()
        {
            const string sql = @"SELECT t.TransactionId, i.ItemName, t.TransactionType, t.Quantity,
                                        t.TransactionDate, t.PerformedBy, t.Reference, t.Notes
                                 FROM StockTransactions t
                                 INNER JOIN StockItems i ON i.ItemId = t.ItemId
                                 ORDER BY t.TransactionDate DESC, t.TransactionId DESC";
            return SqlRunner.Query(sql);
        }

        public DataTable GetRecent(int top)
        {
            const string sql = @"SELECT TOP (@top) t.TransactionId, i.ItemName, t.TransactionType, t.Quantity,
                                        t.TransactionDate, t.PerformedBy, t.Reference
                                 FROM StockTransactions t
                                 INNER JOIN StockItems i ON i.ItemId = t.ItemId
                                 ORDER BY t.TransactionDate DESC, t.TransactionId DESC";
            return SqlRunner.Query(sql, new SqlParameter("@top", top));
        }

        // Steps that run inside a transaction opened by a service

        public void Insert(SqlConnection conn, SqlTransaction tx, StockTransactionEntry entry)
        {
            const string sql = @"INSERT INTO StockTransactions (ItemId, TransactionType, Quantity, Reference, PerformedBy, TransactionDate, Notes)
                                 VALUES (@id, @type, @qty, @reference, @by, GETDATE(), @notes)";
            SqlRunner.Execute(conn, tx, sql,
                new SqlParameter("@id", entry.ItemId),
                new SqlParameter("@type", entry.Type),
                new SqlParameter("@qty", entry.Quantity),
                new SqlParameter("@reference", SqlRunner.OrNull(entry.Reference)),
                new SqlParameter("@by", SqlRunner.OrNull(entry.PerformedBy)),
                new SqlParameter("@notes", SqlRunner.OrNull(entry.Notes)));
        }

        public void DeleteByItem(SqlConnection conn, SqlTransaction tx, int itemId)
        {
            SqlRunner.Execute(conn, tx,
                "DELETE FROM StockTransactions WHERE ItemId = @id",
                new SqlParameter("@id", itemId));
        }
    }
}
