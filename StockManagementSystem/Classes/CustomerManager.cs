using System;
using System.Data;
using System.Data.SqlClient;

namespace StockManagementSystem.Classes
{
    public static class CustomerManager
    {
        // FIXED: Table name is "Customer" NOT "Customers"
        public static DataTable GetAllCustomers()
        {
            DataTable dt = new DataTable();
            string query = "SELECT CustomerId, Name, Address, Phone, Email FROM Customer ORDER BY Name";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error loading customers: " + ex.Message);
            }
            return dt;
        }

        public static DataTable GetCustomerByName(string name)
        {
            DataTable dt = new DataTable();
            string query = "SELECT CustomerId, Name, Address, Phone, Email FROM Customer WHERE Name LIKE @Name ORDER BY Name";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    cmd.Parameters.AddWithValue("@Name", "%" + name + "%");
                    conn.Open();
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error searching customers: " + ex.Message);
            }
            return dt;
        }

        public static int AddCustomer(string name, string address, string phone, string email)
        {
            string query = @"
                INSERT INTO Customer (Name, Address, Phone, Email) 
                OUTPUT INSERTED.CustomerId 
                VALUES (@Name, @Address, @Phone, @Email)";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);

                    conn.Open();
                    return (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error adding customer: " + ex.Message);
            }
        }

        public static void UpdateCustomer(int customerId, string name, string address, string phone, string email)
        {
            string query = @"
                UPDATE Customer 
                SET Name = @Name, Address = @Address, Phone = @Phone, Email = @Email 
                WHERE CustomerId = @CustomerId";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@Phone", (object)phone ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating customer: " + ex.Message);
            }
        }

        public static void DeleteCustomer(int customerId)
        {
            string query = "DELETE FROM Customer WHERE CustomerId = @CustomerId";

            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CustomerId", customerId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting customer: " + ex.Message);
            }
        }

        public static int GetTotalCustomers()
        {
            string query = "SELECT COUNT(*) FROM Customer";
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        return (int)cmd.ExecuteScalar();
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
