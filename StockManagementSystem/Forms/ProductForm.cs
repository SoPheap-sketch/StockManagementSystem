using StockManagementSystem.Classes;
using System;
using System.Data;
using System.Windows.Forms;

namespace StockManagementSystem.Forms
{
    public partial class ProductForm : Form
    {
        private int selectedProductId = -1;

        public ProductForm()
        {
            InitializeComponent();

            // Make sure DataGridView auto-generates columns
            dgvProducts.AutoGenerateColumns = true;
            dgvProducts.BackgroundColor = System.Drawing.Color.White; // grid background
            dgvProducts.DefaultCellStyle.BackColor = System.Drawing.Color.White; // row background
            dgvProducts.DefaultCellStyle.ForeColor = System.Drawing.Color.Black; // text color
            dgvProducts.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue; // selected row
            dgvProducts.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black; // selected text
            dgvProducts.RowHeadersDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgvProducts.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            // Event handlers
            dgvProducts.CellClick += dgvProducts_CellClick;
            btnAdd.Click += btnAdd_Click;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;
            btnClear.Click += btnClear_Click;
            btnRefresh.Click += btnRefresh_Click;
            btnSearch.Click += btnSearch_Click;

            // When sorting, refresh row numbers
            dgvProducts.Sorted += (s, e) => AddRowNumbers();

            LoadCategories();
        }

        private void ProductForm_Load(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            try
            {
                dgvProducts.DataSource = ProductManager.GetAllProducts();
                CleanUpDataGridView();
                AddRowNumbers(); // Add row numbers after loading
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load products: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadCategories()
        {
            try
            {
                var dt = DatabaseHelper.ExecuteQuery("SELECT CategoryId, Name FROM Category ORDER BY Name");

                DataRow selectRow = dt.NewRow();
                selectRow["CategoryId"] = -1;
                selectRow["Name"] = "--- Please select a category ---";
                dt.Rows.InsertAt(selectRow, 0);

                cmbCategory.DataSource = dt;
                cmbCategory.DisplayMember = "Name";
                cmbCategory.ValueMember = "CategoryId";
                cmbCategory.SelectedIndex = 0;

            }catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message); 
            }
        }

        private void CleanUpDataGridView()
        {
            if (dgvProducts.Columns.Contains("ProductId"))
                dgvProducts.Columns["ProductId"].Visible = false;
            if (dgvProducts.Columns.Contains("Name"))
                dgvProducts.Columns["Name"].HeaderText = "Product Name";
            if (dgvProducts.Columns.Contains("CategoryName"))
                dgvProducts.Columns["CategoryName"].HeaderText = "Category";
            if (dgvProducts.Columns.Contains("QuantityInStock"))
                dgvProducts.Columns["QuantityInStock"].HeaderText = "Quantity";
            if (dgvProducts.Columns.Contains("Price"))
                dgvProducts.Columns["Price"].HeaderText = "Price";

            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }


        private void AddRowNumbers()
        {
            if (!dgvProducts.Columns.Contains("No"))
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "No.",
                    ReadOnly = true,
                    Width = 50
                };
                dgvProducts.Columns.Insert(0, col);
            }
            for (int i = 0; i < dgvProducts.Rows.Count; i++)
                dgvProducts.Rows[i].Cells["No"].Value = (i + 1).ToString();
        }

        // --- CRUD Operations ---
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateFields(out int qty, out decimal price)) return;
            if (cmbCategory.SelectedValue == null || (int) cmbCategory.SelectedValue <= 0)
            {
                MessageBox.Show("Please select a valid category.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = new Product
            {
                Name = txtProductName.Text.Trim(),
                CategoryId = (int)cmbCategory.SelectedValue,
                QuantityInStock = qty,
                Price = price,
                Description = txtDescription.Text.Trim() ?? ""
            };

            // Check for duplicate (same name + category)
            var existing = ProductManager.SearchProducts(product.Name);
            foreach (DataRow r in existing.Rows)
            {
                if (r["Name"].ToString().Trim().Equals(product.Name, StringComparison.OrdinalIgnoreCase) &&
                    (int)r["CategoryId"] == product.CategoryId)
                {
                    // Update quantity
                    product.ProductId = (int)r["ProductId"];
                    product.QuantityInStock += (int)r["QuantityInStock"];
                    ProductManager.UpdateProduct(product);
                    MessageBox.Show($"Product exists! Quantity updated to {product.QuantityInStock}", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearFields();
                    LoadProducts();
                    return;
                }
            }

            // Add new
            if (ProductManager.AddProduct(product) == "OK")
            {
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
                LoadProducts();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0)
            {
                MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!ValidateFields(out int qty, out decimal price)) return;
            if (cmbCategory.SelectedIndex <= 0) return;

            var product = new Product
            {
                ProductId = selectedProductId,
                Name = txtProductName.Text.Trim(),
                CategoryId = (int)cmbCategory.SelectedValue,
                QuantityInStock = qty,
                Price = price,
                Description = txtDescription?.Text.Trim() ?? ""
            };

            if (ProductManager.UpdateProduct(product))
            {
                MessageBox.Show("Product updated successfully!");
                ClearFields();
                LoadProducts();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedProductId <= 0) return;
            if (MessageBox.Show("Delete this product?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                ProductManager.DeleteProduct(selectedProductId);
                ClearFields();
                LoadProducts();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string kw = txtSearch.Text.Trim();
            dgvProducts.DataSource = string.IsNullOrEmpty(kw)
                ? ProductManager.GetAllProducts()
                : ProductManager.SearchProducts(kw);
            CleanUpDataGridView();
            AddRowNumbers();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvProducts.Rows.Count) return;

            var row = dgvProducts.Rows[e.RowIndex];
            selectedProductId = Convert.ToInt32(row.Cells["ProductId"].Value ?? -1);

            txtProductName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtDescription.Text = row.Cells["Description"].Value?.ToString() ?? "";
            txtQuantity.Text = row.Cells["QuantityInStock"].Value?.ToString() ?? "";
            txtPrice.Text = row.Cells["Price"].Value?.ToString() ?? "";

            // Select category in ComboBox
            if (row.Cells["CategoryId"].Value != null)
                cmbCategory.SelectedValue = row.Cells["CategoryId"].Value;
        }

        // --- Helpers ---
        private void ClearFields()
        {
            txtProductName.Clear();
            txtDescription?.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
            txtSearch.Clear();
            selectedProductId = -1;
            cmbCategory.SelectedIndex = 0;
        }

        private bool ValidateFields(out int quantity, out decimal price)
        {
            quantity = 0; price = 0;
            if (string.IsNullOrWhiteSpace(txtProductName.Text) ||
                string.IsNullOrWhiteSpace(txtQuantity.Text) ||
                string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Please fill all required fields.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (!int.TryParse(txtQuantity.Text, out quantity) || !decimal.TryParse(txtPrice.Text, out price))
            {
                MessageBox.Show("Quantity and Price must be numbers.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
