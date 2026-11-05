using System;
using System.Data.SqlClient;

namespace LoginRegisterV2
{
    /// <summary>
    /// All SQL for the "admin" table (the accounts table), and nothing else.
    /// No validation, no hashing, no session, no UI: just reading and writing rows.
    /// </summary>
    public class UserRepository
    {
        public bool EmailExists(string email)
        {
            using (SqlConnection conn = Db.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM admin WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        /// <summary>Returns the stored hash and access type for this email, or null if there is no such account.</summary>
        public UserCredentials FindCredentials(string email)
        {
            using (SqlConnection conn = Db.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT password, accesstype FROM admin WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        return null;
                    }

                    return new UserCredentials
                    {
                        Email = email,
                        PasswordHash = reader["password"].ToString(),
                        AccessType = reader["accesstype"].ToString()
                    };
                }
            }
        }

        /// <summary>Returns the stored password hash, or null if there is no such account.</summary>
        public string GetPasswordHash(string email)
        {
            using (SqlConnection conn = Db.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT password FROM admin WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@email", email);
                conn.Open();

                object result = cmd.ExecuteScalar();
                return result == null ? null : result.ToString();
            }
        }

        public void Insert(UserAccount account)
        {
            const string sql =
                "INSERT INTO admin (fullname, birthdate, address, email, number, accesstype, password) " +
                "VALUES (@fullname, @birthdate, @address, @email, @number, @accesstype, @password)";

            using (SqlConnection conn = Db.CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@fullname", account.FullName);
                cmd.Parameters.AddWithValue("@birthdate", account.BirthDate);
                cmd.Parameters.AddWithValue("@address", account.Address);
                cmd.Parameters.AddWithValue("@email", account.Email);
                cmd.Parameters.AddWithValue("@number", account.Number);
                cmd.Parameters.AddWithValue("@accesstype", account.AccessType);
                cmd.Parameters.AddWithValue("@password", account.PasswordHash);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Returns true if a row was updated, false if no account has this email.</summary>
        public bool UpdatePasswordHash(string email, string newHash)
        {
            using (SqlConnection conn = Db.CreateConnection())
            using (SqlCommand cmd = new SqlCommand("UPDATE admin SET password = @password WHERE email = @email", conn))
            {
                cmd.Parameters.AddWithValue("@password", newHash);
                cmd.Parameters.AddWithValue("@email", email);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}
