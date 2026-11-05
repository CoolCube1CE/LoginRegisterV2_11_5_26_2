using System;
using System.Data;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>All SQL for the Orders table, and nothing else.</summary>
    public class OrderRepository
    {
        public DataTable GetAll()
        {
            const string sql = @"SELECT o.OrderId, o.CustomerName, i.ItemName, o.Quantity, o.UnitPrice,
                                        o.TotalAmount, o.Status, o.OrderDate, o.PlacedBy
                                 FROM Orders o
                                 INNER JOIN StockItems i ON i.ItemId = o.ItemId
                                 ORDER BY o.OrderDate DESC, o.OrderId DESC";
            return SqlRunner.Query(sql);
        }

        public DataTable GetRecent(int top)
        {
            const string sql = @"SELECT TOP (@top) o.OrderId, o.CustomerName, i.ItemName, o.Quantity,
                                        o.TotalAmount, o.Status, o.OrderDate
                                 FROM Orders o
                                 INNER JOIN StockItems i ON i.ItemId = o.ItemId
                                 ORDER BY o.OrderDate DESC, o.OrderId DESC";
            return SqlRunner.Query(sql, new SqlParameter("@top", top));
        }

        public void UpdateStatus(int orderId, string status)
        {
            SqlRunner.Execute(
                "UPDATE Orders SET Status = @status WHERE OrderId = @id",
                new SqlParameter("@status", status),
                new SqlParameter("@id", orderId));
        }

        // Steps that run inside a transaction opened by a service

        /// <summary>Inserts a new Pending order and returns its new OrderId.</summary>
        public int Insert(SqlConnection conn, SqlTransaction tx, NewOrder order)
        {
            const string sql = @"INSERT INTO Orders (CustomerName, ItemId, Quantity, UnitPrice, Status, OrderDate, PlacedBy)
                                 OUTPUT INSERTED.OrderId
                                 VALUES (@customer, @item, @qty, @price, @status, GETDATE(), @by)";

            object newId = SqlRunner.Scalar(conn, tx, sql,
                new SqlParameter("@customer", order.CustomerName),
                new SqlParameter("@item", order.ItemId),
                new SqlParameter("@qty", order.Quantity),
                new SqlParameter("@price", order.UnitPrice),
                new SqlParameter("@status", OrderStatus.Pending),
                new SqlParameter("@by", SqlRunner.OrNull(order.PlacedBy)));

            return Convert.ToInt32(newId);
        }

        public void DeleteByItem(SqlConnection conn, SqlTransaction tx, int itemId)
        {
            SqlRunner.Execute(conn, tx,
                "DELETE FROM Orders WHERE ItemId = @id",
                new SqlParameter("@id", itemId));
        }
    }
}
