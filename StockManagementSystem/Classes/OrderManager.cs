using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace StockManagementSystem.Classes
{
    public static class OrderManager
    {
        public static void AddOrder(Order order)
        {
            using (SqlConnection conn = DatabaseHelper.GetConnection())
            {
                conn.Open(); // THIS WAS MISSING!
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Insert Order
                    string sqlOrder = @"
                        INSERT INTO [Order] (CustomerId, OrderDate, TotalAmount, Status)
                        OUTPUT INSERTED.OrderId
                        VALUES (@CustomerId, @OrderDate, @TotalAmount, @Status)";

                    int orderId;
                    using (SqlCommand cmd = new SqlCommand(sqlOrder, conn, transaction))
                    {
                        cmd.Parameters.AddWithValue("@CustomerId",
                            order.Customer?.CustomerId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@OrderDate", order.OrderDate);
                        cmd.Parameters.AddWithValue("@TotalAmount", order.CalculateTotal());
                        cmd.Parameters.AddWithValue("@Status", order.Status ?? "Pending");

                        orderId = (int)cmd.ExecuteScalar();
                        order.OrderID = orderId; // Important: set the generated ID
                    }

                    // Insert OrderItems + Reduce Stock
                    foreach (var item in order.OrderItems)
                    {
                        // Insert item
                        string sqlItem = @"
                            INSERT INTO OrderItem (OrderId, ProductId, Quantity, UnitPrice)
                            VALUES (@OrderId, @ProductId, @Quantity, @UnitPrice)";

                        using (SqlCommand cmd = new SqlCommand(sqlItem, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@ProductId", item.Product.ProductId);
                            cmd.Parameters.AddWithValue("@Quantity", item.Quantity);
                            cmd.Parameters.AddWithValue("@UnitPrice", item.PriceAtPurchase);
                            cmd.ExecuteNonQuery();
                        }

                        // Reduce stock
                        string sqlStock = @"
                            UPDATE Product 
                            SET QuantityInStock = QuantityInStock - @Qty 
                            WHERE ProductId = @Id";

                        using (SqlCommand cmd = new SqlCommand(sqlStock, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Qty", item.Quantity);
                            cmd.Parameters.AddWithValue("@Id", item.Product.ProductId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Failed to complete order: " + ex.Message);
                }
            }
        }
        public static int GetTotalOrders()
        {
            string query = "SELECT COUNT(*) FROM [Order]";
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
