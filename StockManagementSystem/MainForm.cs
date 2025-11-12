using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManagementSystem.Classes;
using StockManagementSystem.Forms;

namespace StockManagementSystem
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Stock Management System - Dashboard";
            LoadDashboardCounts();
            StyleDashboard();
        }
        private void LoadDashboardCounts()
        {
            try
            {
                int products = ProductManager.GetTotalProducts();
                int orders = OrderManager.GetTotalOrders();
                int customers = CustomerManager.GetTotalCustomers();

                lblProducts.Text = products.ToString();
                lblOrders.Text = orders.ToString();
                lblCustomer.Text = customers.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading dashboard: " + ex.Message);
                lblProducts.Text = "0";
                lblOrders.Text = "0";
                lblCustomer.Text = "0";
            }
        }

        private void StyleDashboard()
        {
            // Make labels big and bold
            Font bigFont = new Font("Segoe UI", 24F, FontStyle.Bold);
            Font mediumFont = new Font("Segoe UI", 12F, FontStyle.Regular);

            lblProducts.Font = bigFont;
            lblProducts.ForeColor = Color.DarkOrange;
            lblProducts.TextAlign = ContentAlignment.MiddleCenter;

            lblOrders.Font = bigFont;
            lblOrders.ForeColor = Color.DarkOrange;
            lblOrders.TextAlign = ContentAlignment.MiddleCenter;

            lblCustomer.Font = bigFont;
            lblCustomer.ForeColor = Color.DarkOrange;
            lblCustomer.TextAlign = ContentAlignment.MiddleCenter;

            // Optional: Add small labels below
            Label lblProdText = new Label { Text = "Total Products", Font = mediumFont, ForeColor = Color.Gray, TextAlign = ContentAlignment.MiddleCenter };
            Label lblOrderText = new Label { Text = "Total Orders", Font = mediumFont, ForeColor = Color.Gray, TextAlign = ContentAlignment.MiddleCenter };
            Label lblCustText = new Label { Text = "Total Customers", Font = mediumFont, ForeColor = Color.Gray, TextAlign = ContentAlignment.MiddleCenter };

            // Adjust location based on your lblProducts, etc.
            // Example (adjust as needed):
            lblProdText.Location = new Point(lblProducts.Left, lblProducts.Bottom + 5);
            lblOrderText.Location = new Point(lblOrders.Left, lblOrders.Bottom + 5);
            lblCustText.Location = new Point(lblCustomer.Left, lblCustomer.Bottom + 5);

            this.Controls.Add(lblProdText);
            this.Controls.Add(lblOrderText);
            this.Controls.Add(lblCustText);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this one is btnManageCategories
            new CategoryForm().ShowDialog();
            LoadDashboardCounts();
        }

        private void btnManageProducts_Click(object sender, EventArgs e)
        {
            new ProductForm().ShowDialog();
            LoadDashboardCounts();
        }

        private void btnManageCustomers_Click(object sender, EventArgs e)
        {
            new CustomerForm().ShowDialog();
            LoadDashboardCounts();
        }

        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            new UserForm().ShowDialog();
            LoadDashboardCounts();
        }

        private void btnManageOrders_Click(object sender, EventArgs e)
        {
            new OrderForm().ShowDialog();
            LoadDashboardCounts();
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblCustomer_Click(object sender, EventArgs e)
        {

        }

        private void lblOrders_Click(object sender, EventArgs e)
        {

        }

        private void lblProducts_Click(object sender, EventArgs e)
        {

        }
    }
}
