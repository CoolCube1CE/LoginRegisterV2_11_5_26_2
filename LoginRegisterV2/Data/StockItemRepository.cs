using System;
using System.Data;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>All SQL for the StockItems table, and nothing else.</summary>
    public class StockItemRepository
    {
        // ------------------------------------------------------------------
        // Lists
        // ------------------------------------------------------------------

        public DataTable GetAll()
        {
            const string sql = @"SELECT ItemId, ItemName, Category, SKU, Quantity, ReorderLevel, UnitPrice, UpdatedAt
                                 FROM StockItems
                                 ORDER BY ItemName ASC";
            return SqlRunner.Query(sql);
        }

        public DataTable GetForDropdown()
        {
            return SqlRunner.Query("SELECT ItemId, ItemName, Quantity, UnitPrice FROM StockItems ORDER BY ItemName ASC");
        }

        public DataTable GetLowStock(int top)
        {
            const string sql = @"SELECT TOP (@top) ItemName, SKU, Quantity, ReorderLevel
                                 FROM StockItems
                                 WHERE Quantity <= ReorderLevel
                                 ORDER BY (Quantity - ReorderLevel) ASC, ItemName ASC";
            return SqlRunner.Query(sql, new SqlParameter("@top", top));
        }

        // ------------------------------------------------------------------
        // Dashboard numbers
        // ------------------------------------------------------------------

        /// <summary>Returns the headline totals, or null if the query returned no row.</summary>
        public StockOverview GetOverview()
        {
            const string sql = @"SELECT
                (SELECT COUNT(*) FROM StockItems) AS TotalItems,
                (SELECT ISNULL(SUM(Quantity), 0) FROM StockItems) AS TotalUnits,
                (SELECT COUNT(*) FROM StockItems WHERE Quantity <= ReorderLevel) AS LowStockCount,
                (SELECT ISNULL(SUM(Quantity * UnitPrice), 0) FROM StockItems) AS TotalValue";

            DataTable table = SqlRunner.Query(sql);
            if (table.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = table.Rows[0];
            return new StockOverview
            {
                TotalItems = Convert.ToInt32(row["TotalItems"]),
                TotalUnits = Convert.ToInt32(row["TotalUnits"]),
                LowStockCount = Convert.ToInt32(row["LowStockCount"]),
                TotalValue = Convert.ToDecimal(row["TotalValue"])
            };
        }

        // ------------------------------------------------------------------
        // Add / edit
        // ------------------------------------------------------------------

        public void Insert(string itemName, string category, string sku, int quantity, int reorderLevel, decimal unitPrice)
        {
            const string sql = @"INSERT INTO StockItems (ItemName, Category, SKU, Quantity, ReorderLevel, UnitPrice, CreatedAt, UpdatedAt)
                                 VALUES (@name, @category, @sku, @qty, @reorder, @price, GETDATE(), GETDATE())";
            SqlRunner.Execute(sql,
                new SqlParameter("@name", itemName),
                new SqlParameter("@category", SqlRunner.OrNull(category)),
                new SqlParameter("@sku", SqlRunner.OrNull(sku)),
                new SqlParameter("@qty", quantity),
                new SqlParameter("@reorder", reorderLevel),
                new SqlParameter("@price", unitPrice));
        }

        public void Update(int itemId, string itemName, string category, string sku, int reorderLevel, decimal unitPrice)
        {
            const string sql = @"UPDATE StockItems
                                 SET ItemName = @name, Category = @category, SKU = @sku,
                                     ReorderLevel = @reorder, UnitPrice = @price, UpdatedAt = GETDATE()
                                 WHERE ItemId = @id";
            SqlRunner.Execute(sql,
                new SqlParameter("@name", itemName),
                new SqlParameter("@category", SqlRunner.OrNull(category)),
                new SqlParameter("@sku", SqlRunner.OrNull(sku)),
                new SqlParameter("@reorder", reorderLevel),
                new SqlParameter("@price", unitPrice),
                new SqlParameter("@id", itemId));
        }

        // ------------------------------------------------------------------
        // Steps that run inside a transaction opened by a service
        // ------------------------------------------------------------------

        /// <summary>Returns the item's current quantity and price, or null if the item does not exist.</summary>
        public StockLevel GetLevel(SqlConnection conn, SqlTransaction tx, int itemId)
        {
            DataTable table = SqlRunner.Query(conn, tx,
                "SELECT UnitPrice, Quantity FROM StockItems WHERE ItemId = @id",
                new SqlParameter("@id", itemId));

            if (table.Rows.Count == 0)
            {
                return null;
            }

            return new StockLevel
            {
                UnitPrice = Convert.ToDecimal(table.Rows[0]["UnitPrice"]),
                Quantity = Convert.ToInt32(table.Rows[0]["Quantity"])
            };
        }

        /// <summary>Adds delta to the item's quantity (use a negative delta to take stock out).</summary>
        public void AdjustQuantity(SqlConnection conn, SqlTransaction tx, int itemId, int delta)
        {
            SqlRunner.Execute(conn, tx,
                "UPDATE StockItems SET Quantity = Quantity + @delta, UpdatedAt = GETDATE() WHERE ItemId = @id",
                new SqlParameter("@delta", delta),
                new SqlParameter("@id", itemId));
        }

        public void Delete(SqlConnection conn, SqlTransaction tx, int itemId)
        {
            SqlRunner.Execute(conn, tx,
                "DELETE FROM StockItems WHERE ItemId = @id",
                new SqlParameter("@id", itemId));
        }
    }
}
