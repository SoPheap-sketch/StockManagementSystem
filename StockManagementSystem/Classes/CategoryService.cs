using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StockManagementSystem.Classes
{
    public class CategoryService
    {
        // FIXED: public method
        public List<Category> GetAll()
        {
            var list = new List<Category>();
            string sql = "SELECT CategoryId, Name, Description FROM Category ORDER BY Name";
            DataTable dt = DatabaseHelper.ExecuteQuery(sql);  // Your method

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Category
                {
                    CategoryId = Convert.ToInt32(row["CategoryId"]),
                    Name = row["Name"]?.ToString() ?? "",
                    Description = row["Description"]?.ToString()
                });
            }
            return list;
        }

        // FIXED: public method
        public void Add(Category category)
        {
            category.Validate();

            string sql = "INSERT INTO Category (Name, Description) VALUES (@Name, @Desc)";
            var parameters = new[]
            {
                new SqlParameter("@Name", category.Name),
                new SqlParameter("@Desc", (object)category.Description ?? DBNull.Value)
            };

            DatabaseHelper.ExecuteNonQuery(sql, parameters);  // Your method
        }

        // FIXED: public method
        public bool NameExists(string name)
        {
            string sql = "SELECT COUNT(*) FROM Category WHERE Name = @Name";
            var parameters = new[] { new SqlParameter("@Name", name) };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            int count = Convert.ToInt32(dt.Rows[0][0]);
            return count > 0;
        }
    }
}