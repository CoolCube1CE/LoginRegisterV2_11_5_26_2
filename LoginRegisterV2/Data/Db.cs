using System.Configuration;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>
    /// The one place that knows how to reach the database.
    /// Every repository asks this class for a connection instead of reading
    /// the connection string itself.
    /// </summary>
    public static class Db
    {
        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["LoginRegisterDB"].ConnectionString; }
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
