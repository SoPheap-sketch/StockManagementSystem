using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using StockManagementSystem.Classes;

namespace StockManagementSystem.Forms
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {
            LoadSummaryData();
        }

        private void LoadSummaryData()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.GetConnection())
                {
                    conn.Open();

                    // ✅ Total Products
                    SqlCommand cmdProducts = new SqlCommand("SELECT COUNT(*) FROM Products", conn);
                    int totalProducts = Convert.ToInt32(cmdProducts.ExecuteScalar());

                    // ✅ Total Customers
                    SqlCommand cmdCustomers = new SqlCommand("SELECT COUNT(*) FROM Customers", conn);
                    int totalCustomers = Convert.ToInt32(cmdCustomers.ExecuteScalar());

                    // ✅ Total Orders
                    SqlCommand cmdOrders = new SqlCommand("SELECT COUNT(*) FROM Orders", conn);
                    int totalOrders = Convert.ToInt32(cmdOrders.ExecuteScalar());

                    // ✅ Total Revenue
                    SqlCommand cmdRevenue = new SqlCommand("SELECT ISNULL(SUM(TotalAmount), 0) FROM Orders", conn);
                    decimal totalRevenue = Convert.ToDecimal(cmdRevenue.ExecuteScalar());

                    // ✅ Display results
                    lblProducts.Text = totalProducts.ToString();
                    lblCustomers.Text = totalCustomers.ToString();
                    lblOrders.Text = totalOrders.ToString();
                    lblRevenue.Text = "$" + totalRevenue.ToString("N2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading report data: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
