using StockManagementSystem.Classes;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace StockManagementSystem.Forms
{
    public partial class CustomerForm : Form
    {
        
        private int? selectedCustomerId = null;

        public CustomerForm()
        {
            InitializeComponent();
            SetupForm();
            LoadCustomers();
        }

        private void SetupForm()
        {
            this.Text = "Manage Customers";
            this.StartPosition = FormStartPosition.CenterScreen;

            // Style buttons
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;

            btnEdit.BackColor = Color.RoyalBlue;
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;

            btnDelete.BackColor = Color.Crimson;
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;

            btnSearch.BackColor = Color.DarkOrange;
            btnSearch.ForeColor = Color.White;
            btnSearch.FlatStyle = FlatStyle.Flat;

            // DataGridView
            dgvCustomers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvCustomers.MultiSelect = false;
            dgvCustomers.ReadOnly = true;
            dgvCustomers.AllowUserToAddRows = false;
            dgvCustomers.BackgroundColor = Color.White;
            dgvCustomers.GridColor = Color.LightGray;
            dgvCustomers.CellClick += dgvCustomers_CellClick;

            // Add Back button (if not in designer)
            if (!this.Controls.Contains(btnBack))
            {
                Button btnBack = new Button
                {
                    Text = "Back",
                    BackColor = Color.Navy,
                    ForeColor = Color.White,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    Size = new Size(100, 40),
                    Location = new Point(this.ClientSize.Width - 120, 20)
                };
                btnBack.Click += (s, e) => this.Close();
                this.Controls.Add(btnBack);
            }
        }

        // --- Event Handler for Form Load ---
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadCustomers();
        }
        private void LoadCustomers()
        {
            try
            {
                DataTable dt = CustomerManager.GetAllCustomers();
                dgvCustomers.DataSource = dt;

                // Hide ID column
                if (dgvCustomers.Columns.Contains("CustomerId"))
                    dgvCustomers.Columns["CustomerId"].Visible = false;

                // Add row numbers
                AddRowNumbers();

                // Column headers
                if (dgvCustomers.Columns.Contains("Name")) dgvCustomers.Columns["Name"].HeaderText = "Customer Name";
                if (dgvCustomers.Columns.Contains("Address")) dgvCustomers.Columns["Address"].HeaderText = "Address";
                if (dgvCustomers.Columns.Contains("Phone")) dgvCustomers.Columns["Phone"].HeaderText = "Phone";
                if (dgvCustomers.Columns.Contains("Email")) dgvCustomers.Columns["Email"].HeaderText = "Email";

                dgvCustomers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load customers: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddRowNumbers()
        {
            if (!dgvCustomers.Columns.Contains("No"))
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "No.",
                    Width = 60,
                    ReadOnly = true
                };
                dgvCustomers.Columns.Insert(0, col);
            }

            for (int i = 0; i < dgvCustomers.Rows.Count; i++)
            {
                dgvCustomers.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }

        private void ClearInputs()
        {
            txtName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            txtSearch.Clear();
            selectedCustomerId = null;
            lblStatus.Text = "No customer selected";
            lblStatus.ForeColor = Color.Gray;
        }


        private void dgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvCustomers.Rows.Count) return;

            DataGridViewRow row = dgvCustomers.Rows[e.RowIndex];
            selectedCustomerId = Convert.ToInt32(row.Cells["CustomerId"].Value);

            txtName.Text = row.Cells["Name"].Value?.ToString() ?? "";
            txtAddress.Text = row.Cells["Address"].Value?.ToString() ?? "";
            txtPhone.Text = row.Cells["Phone"].Value?.ToString() ?? "";
            txtEmail.Text = row.Cells["Email"].Value?.ToString() ?? "";

            lblStatus.Text = $"Selected: {txtName.Text}";
            lblStatus.ForeColor = Color.DarkGreen;
        }

        // --- Button Click Handlers ---

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Customer name is required!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Address is required!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtAddress.Focus();
                return;
            }

            try
            {
                CustomerManager.AddCustomer(name, address, phone, email);
                MessageBox.Show("Customer added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                MessageBox.Show("Please select a customer to edit.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();
            string address = txtAddress.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string email = txtEmail.Text.Trim();

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Name and Address are required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                CustomerManager.UpdateCustomer(selectedCustomerId.Value, name, address, phone, email);
                MessageBox.Show("Customer updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearInputs();
                LoadCustomers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!selectedCustomerId.HasValue)
            {
                MessageBox.Show("Please select a customer to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this customer?\nThis cannot be undone.", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    CustomerManager.DeleteCustomer(selectedCustomerId.Value);
                    MessageBox.Show("Customer deleted!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearInputs();
                    LoadCustomers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Cannot delete: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text.Trim();
            try
            {
                DataTable dt = string.IsNullOrEmpty(keyword)
                    ? CustomerManager.GetAllCustomers()
                    : CustomerManager.GetCustomerByName(keyword);

                dgvCustomers.DataSource = dt;
                AddRowNumbers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Search error: " + ex.Message);
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtEmail.Clear();
            txtPhone.Clear();
            txtAddress.Clear();
        }
    }
}