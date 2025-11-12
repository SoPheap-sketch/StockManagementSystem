using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StockManagementSystem.Classes;

namespace StockManagementSystem.Forms
{
    public partial class OrderForm : Form
    {
        private int selectedOrderId = -1;
        private List<OrderItem> currentItems = new List<OrderItem>();
        public OrderForm()
        {
            InitializeComponent();
            InitializeForm();
        }

        private void InitializeForm()
        {
            LoadCustomers();
            LoadProducts();
            SetupGrids();
            RefreshOrderList();
            UpdateTotal();
            lblOrderDate.Text = DateTime.Now.ToString("MMMM dd, yyyy - hh:mm tt");
        }
        private void RefreshItemsGrid()
        {
            dgvOrderItems.Rows.Clear();
            foreach (var item in currentItems)
            {
                decimal subtotal = item.CalculateSubtotal();
                dgvOrderItems.Rows.Add(
                    item.Product.Name,
                    item.Quantity,
                    item.PriceAtPurchase.ToString("C2"),
                    subtotal.ToString("C2")
                );
            }
        }
        private void RefreshOrderList()
        {
            dgvOrder.Rows.Clear();
            try
            {
                string sql = @"
                    SELECT o.OrderId, ISNULL(c.Name, 'Walk-in') AS CustomerName,
                           o.TotalAmount, o.Status, o.OrderDate
                    FROM [Order] o
                    LEFT JOIN Customer c ON o.CustomerId = c.CustomerId
                    ORDER BY o.OrderDate DESC";

                var dt = DatabaseHelper.ExecuteQuery(sql);
                foreach (DataRow r in dt.Rows)
                {
                    dgvOrder.Rows.Add(
                        r["OrderId"],
                        r["CustomerName"],
                        decimal.Parse(r["TotalAmount"].ToString()).ToString("C2"),
                        r["Status"],
                        DateTime.Parse(r["OrderDate"].ToString()).ToString("MMM dd, yyyy HH:mm")
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message);
            }
        }

        private void UpdateTotal()
        {
            decimal total = currentItems.Sum(x => x.CalculateSubtotal());
            txtTotal.Text = total.ToString("C2");
        }
        private void LoadCustomers()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery(@"
                    SELECT CustomerId, Name FROM Customer 
                    UNION ALL 
                    SELECT NULL, '--- Walk-in Customer ---' 
                    ORDER BY Name");

                cmbCustomer.DataSource = dt;
                cmbCustomer.DisplayMember = "Name";
                cmbCustomer.ValueMember = "CustomerId";
                cmbCustomer.SelectedIndex = dt.Rows.Count - 1; // Walk-in default
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message);
            }
        }

        private void LoadProducts()
        {
            try
            {
                var dt = ProductManager.GetAllProducts();
                cmbProduct.DataSource = dt.Copy();
                cmbProduct.DisplayMember = "Name";
                cmbProduct.ValueMember = "ProductId";

                // Auto-fill price when product changes
                cmbProduct.SelectedIndexChanged += (s, e) =>
                {
                    if (cmbProduct.SelectedValue != null && cmbProduct.SelectedIndex >= 0)
                    {
                        var row = ((DataRowView)cmbProduct.SelectedItem).Row;
                        txtPrice.Text = row["Price"].ToString();
                    }
                };
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message);
            }
        }

        private void SetupGrids()
        {
            // Order Items Grid
            dgvOrderItems.Columns.Clear();
            dgvOrderItems.Columns.Add("Product", "Product");
            dgvOrderItems.Columns.Add("Qty", "Qty");
            dgvOrderItems.Columns.Add("Price", "Price");
            dgvOrderItems.Columns.Add("Subtotal", "Subtotal");
            dgvOrderItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrderItems.AllowUserToAddRows = false;

            // Orders List Grid
            dgvOrder.Columns.Clear();
            dgvOrder.Columns.Add("OrderId", "Order ID");
            dgvOrder.Columns.Add("Customer", "Customer");
            dgvOrder.Columns.Add("Total", "Total");
            dgvOrder.Columns.Add("Status", "Status");
            dgvOrder.Columns.Add("Date", "Date");
            dgvOrder.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOrder.ReadOnly = true;
            dgvOrder.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gbAddItem_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void lblOrderDate_Click(object sender, EventArgs e)
        {

        }

        private void cmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();
            foreach (DataGridViewRow row in dgvOrder.Rows)
            {
                bool match = row.Cells["Customer"].Value?.ToString().ToLower().Contains(keyword) == true ||
                             row.Cells["OrderId"].Value?.ToString().Contains(keyword) == true;
                row.Visible = string.IsNullOrEmpty(keyword) || match;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void numQuantity_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (cmbProduct.SelectedValue == null || numQuantity.Value < 1)
            {
                MessageBox.Show("Please select a product and quantity.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price.", "Invalid Price", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int productId = (int)cmbProduct.SelectedValue;
            string productName = cmbProduct.Text;

            var existing = currentItems.FirstOrDefault(x => x.Product.ProductId == productId);
            if (existing != null)
            {
                existing.Quantity += (int)numQuantity.Value;
            }
            else
            {
                currentItems.Add(new OrderItem
                {
                    Product = new Product { ProductId = productId, Name = productName },
                    Quantity = (int)numQuantity.Value,
                    PriceAtPurchase = price
                });
            }

            RefreshItemsGrid();
            UpdateTotal();
            numQuantity.Value = 1;
            txtPrice.Clear();
            cmbProduct.SelectedIndex = -1;
            cmbProduct.Focus();
        }

        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (dgvOrderItems.CurrentRow != null)
            {
                int index = dgvOrderItems.CurrentRow.Index;
                currentItems.RemoveAt(index);
                RefreshItemsGrid();
                UpdateTotal();
            }
        }

        private void dgvOrderItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCompleteOrder_Click(object sender, EventArgs e)
        {
            if (currentItems.Count == 0)
            {
                MessageBox.Show("Please add at least one item.", "Empty Order", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int? customerId = cmbCustomer.SelectedValue is DBNull ? (int?)null : (int)cmbCustomer.SelectedValue;

            var order = new Order
            {
                Customer = customerId.HasValue ? new Customer { CustomerId = customerId.Value } : null,
                OrderItems = currentItems.ToList(),
                Status = "Completed",
                OrderDate = DateTime.Now
            };

            try
            {
                OrderManager.AddOrder(order);
                MessageBox.Show($"Order #{order.OrderID} completed successfully!\nTotal: {txtTotal.Text}",
                    "Order Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearAll();
                RefreshOrderList();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Order Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ClearAll()
        {
            currentItems.Clear();
            RefreshItemsGrid();
            UpdateTotal();
            selectedOrderId = -1;
            numQuantity.Value = 1;
            txtPrice.Clear();
            cmbProduct.SelectedIndex = -1;
            cmbCustomer.SelectedIndex = cmbCustomer.Items.Count - 1;
        }
        private void btnCancelOrder_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Cancel this order?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ClearAll();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshOrderList();
        }

        private void txtTotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgvOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            selectedOrderId = (int)dgvOrder.Rows[e.RowIndex].Cells["OrderId"].Value;

            // Load items for this order
            LoadOrderItems(selectedOrderId);
        }
        private void LoadOrderItems(int orderId)
        {
            currentItems.Clear();
            RefreshItemsGrid();
            UpdateTotal();

            try
            {
                string sql = @"
            SELECT p.Name, oi.Quantity, oi.UnitPrice
            FROM OrderItem oi
            JOIN Product p ON oi.ProductId = p.ProductId
            WHERE oi.OrderId = @OrderId";

                var parameters = new[] { new SqlParameter("@OrderId", orderId) };
                var dt = DatabaseHelper.ExecuteQuery(sql, parameters);

                foreach (DataRow r in dt.Rows)
                {
                    currentItems.Add(new OrderItem
                    {
                        Product = new Product { Name = r["Name"].ToString() },
                        Quantity = (int)r["Quantity"],
                        PriceAtPurchase = (decimal)r["UnitPrice"]
                    });
                }

                RefreshItemsGrid();
                UpdateTotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading order items: " + ex.Message);
            }
        }

    }
}
