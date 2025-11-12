using System;
using System.Data;
using System.Data.SqlClient;

namespace StockManagementSystem.Classes
{
    public static class ProductManager
    {
        private const string SelectAllSql = @"
            SELECT p.ProductId, p.Name, p.Price, p.QuantityInStock, p.Description, 
                   p.CategoryId, c.Name AS CategoryName
            FROM Product p
            LEFT JOIN Category c ON p.CategoryId = c.CategoryId";

        // -----------------------------------------------------------------
        public static DataTable GetAllProducts()
        {
            return DatabaseHelper.ExecuteQuery(SelectAllSql);
        }

        // -----------------------------------------------------------------
        public static DataTable SearchProducts(string keyword)
        {
            string sql = SelectAllSql + " WHERE p.Name LIKE @kw OR c.Name LIKE @kw";
            var p = new[] { new SqlParameter("@kw", $"%{keyword}%") };
            return DatabaseHelper.ExecuteQuery(sql, p);
        }

        // -----------------------------------------------------------------
        public static string AddProduct(Product product)
        {
            try
            {
                string sql = @"
                    INSERT INTO Product (Name, Price, QuantityInStock, Description, CategoryId)
                    VALUES (@Name, @Price, @Qty, @Desc, @CatId)";
                var p = new[]
                {
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@Qty", product.QuantityInStock),
                    new SqlParameter("@Desc", product.Description ?? (object)DBNull.Value),
                    new SqlParameter("@CatId", product.CategoryId)
                };
                DatabaseHelper.ExecuteNonQuery(sql, p);
                return "OK";
            }
            catch (Exception ex) { return ex.Message; }
        }

        // -----------------------------------------------------------------
        public static bool UpdateProduct(Product product)
        {
            try
            {
                string sql = @"
                    UPDATE Product 
                    SET Name=@Name, Price=@Price, QuantityInStock=@Qty, 
                        Description=@Desc, CategoryId=@CatId
                    WHERE ProductId=@Id";
                var p = new[]
                {
                    new SqlParameter("@Id", product.ProductId),
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Price", product.Price),
                    new SqlParameter("@Qty", product.QuantityInStock),
                    new SqlParameter("@Desc", product.Description ?? (object)DBNull.Value),
                    new SqlParameter("@CatId", product.CategoryId)
                };
                return DatabaseHelper.ExecuteNonQuery(sql, p) > 0;
            }
            catch { return false; }
        }

        // -----------------------------------------------------------------
        public static bool DeleteProduct(int productId)
        {
            try
            {
                string sql = "DELETE FROM Product WHERE ProductId=@Id";
                var p = new[] { new SqlParameter("@Id", productId) };
                return DatabaseHelper.ExecuteNonQuery(sql, p) > 0;
            }
            catch { return false; }
        }
        public static int GetTotalProducts()
        {
            string query = "SELECT COUNT(*) FROM Product";
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
