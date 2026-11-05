using System;
using System.Data;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>
    /// The plumbing every repository shares: open a connection, run a command,
    /// clean up. It holds no SQL text and no business rules.
    /// </summary>
    public static class SqlRunner
    {
        // ------------------------------------------------------------------
        // One-off commands (they open and close their own connection)
        // ------------------------------------------------------------------

        public static DataTable Query(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = Db.CreateConnection())
            {
                conn.Open();
                return Query(conn, null, sql, parameters);
            }
        }

        public static void Execute(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = Db.CreateConnection())
            {
                conn.Open();
                Execute(conn, null, sql, parameters);
            }
        }

        // ------------------------------------------------------------------
        // Commands that join a connection/transaction someone else opened
        // ------------------------------------------------------------------

        public static DataTable Query(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] parameters)
        {
            DataTable table = new DataTable();
            using (SqlCommand cmd = CreateCommand(conn, tx, sql, parameters))
            using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
            {
                adapter.Fill(table);
            }
            return table;
        }

        public static int Execute(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = CreateCommand(conn, tx, sql, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(SqlConnection conn, SqlTransaction tx, string sql, params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = CreateCommand(conn, tx, sql, parameters))
            {
                return cmd.ExecuteScalar();
            }
        }

        // ------------------------------------------------------------------
        // Transactions
        // ------------------------------------------------------------------

        /// <summary>
        /// Runs several commands as one all-or-nothing unit. The work is committed
        /// if it returns a successful result, and rolled back if it returns a
        /// failed result or throws (the exception is then re-thrown).
        /// </summary>
        public static OperationResult InTransaction(Func<SqlConnection, SqlTransaction, OperationResult> work)
        {
            using (SqlConnection conn = Db.CreateConnection())
            {
                conn.Open();
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        OperationResult result = work(conn, tx);

                        if (result.Success)
                        {
                            tx.Commit();
                        }
                        else
                        {
                            tx.Rollback();
                        }

                        return result;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        // ------------------------------------------------------------------
        // Small helpers
        // ------------------------------------------------------------------

        /// <summary>A parameter value that becomes SQL NULL when the .NET value is null.</summary>
        public static object OrNull(object value)
        {
            return value ?? DBNull.Value;
        }

        private static SqlCommand CreateCommand(SqlConnection conn, SqlTransaction tx, string sql, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, conn, tx);
            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }
    }
}
