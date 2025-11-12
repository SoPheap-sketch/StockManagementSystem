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
    public partial class UserForm : Form
    {
        private int? selectedUserId = null;

        public UserForm()
        {
            InitializeComponent();
            SetupForm();
            LoadUsers();
        }
        private void SetupForm()
        {
            this.Text = "Manage Users";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);

            // Buttons
            btnAdd.BackColor = Color.ForestGreen;
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;

            btnEdit.BackColor = Color.RoyalBlue;
            btnEdit.ForeColor = Color.White;
            btnEdit.FlatStyle = FlatStyle.Flat;

            btnDelete.BackColor = Color.Crimson;
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;

            btnClear.BackColor = Color.Gray;
            btnClear.ForeColor = Color.White;
            btnClear.FlatStyle = FlatStyle.Flat;

            // DataGridView
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.MultiSelect = false;
            dgvUsers.ReadOnly = true;
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.BackgroundColor = Color.White;
            dgvUsers.GridColor = Color.LightGray;
            dgvUsers.CellClick += dgvUsers_CellClick;
            dgvUsers.CellFormatting += dgvUsers_CellFormatting;

            // Status label
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Gray;
            lblStatus.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
        }
        private void LoadUsers()
        {
            try
            {
                string sql = @"
            SELECT UserId, Username, Role, IsActive 
            FROM [User] 
            ORDER BY Username";

                DataTable dt = DatabaseHelper.ExecuteQuery(sql);
                dgvUsers.DataSource = dt;

                if (dgvUsers.Columns.Contains("UserId"))
                    dgvUsers.Columns["UserId"].Visible = false;

                if (dgvUsers.Columns.Contains("Username")) dgvUsers.Columns["Username"].HeaderText = "Username";
                if (dgvUsers.Columns.Contains("Role")) dgvUsers.Columns["Role"].HeaderText = "Role";
                if (dgvUsers.Columns.Contains("IsActive"))
                {
                    dgvUsers.Columns["IsActive"].HeaderText = "Status";
                    dgvUsers.Columns["IsActive"].Width = 80;
                    dgvUsers.Columns["IsActive"].ReadOnly = true; // THIS FIXES THE ERROR!
                }

                AddRowNumbers();
                dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading users: " + ex.Message);
            }
        }

        private void AddRowNumbers()
        {
            if (!dgvUsers.Columns.Contains("No"))
            {
                var col = new DataGridViewTextBoxColumn
                {
                    Name = "No",
                    HeaderText = "No.",
                    Width = 50,
                    ReadOnly = true
                };
                dgvUsers.Columns.Insert(0, col);
            }

            for (int i = 0; i < dgvUsers.Rows.Count; i++)
            {
                dgvUsers.Rows[i].Cells["No"].Value = (i + 1).ToString();
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!Validate()) return;

            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string role = cmbRole.Text;
            bool active = chkIsActive.Checked;

            if (UsernameExists(username))
            {
                MessageBox.Show("Username already exists!", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string sql = @"
                    INSERT INTO [User] (Username, PasswordHash, Role, IsActive, AuthenticatedPassword)
                    VALUES (@u, 'hash', @r, @a, @p)";

                var parameters = new[]
                {
                    new SqlParameter("@u", username),
                    new SqlParameter("@r", role),
                    new SqlParameter("@a", active),
                    new SqlParameter("@p", password)
                };

                DatabaseHelper.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (!selectedUserId.HasValue)
            {
                MessageBox.Show("Please select a user to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Validate()) return;

            try
            {
                string passwordPart = string.IsNullOrEmpty(txtPassword.Text) ? "" : ", AuthenticatedPassword = @p";
                string sql = $@"
                    UPDATE [User] SET
                        Username = @u,
                        Role = @r,
                        IsActive = @a {passwordPart}
                    WHERE UserId = @id";

                var parameters = new[]
                {
                    new SqlParameter("@u", txtUsername.Text.Trim()),
                    new SqlParameter("@r", cmbRole.Text),
                    new SqlParameter("@a", chkIsActive.Checked),
                    new SqlParameter("@id", selectedUserId.Value)
                };

                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    Array.Resize(ref parameters, parameters.Length + 1);
                    parameters[parameters.Length - 1] = new SqlParameter("@p", txtPassword.Text);
                }

                DatabaseHelper.ExecuteNonQuery(sql, parameters);
                MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!selectedUserId.HasValue)
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Delete this user?\nThis cannot be undone.", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                try
                {
                    string sql = "DELETE FROM [User] WHERE UserId = @id";
                    DatabaseHelper.ExecuteNonQuery(sql, new[] { new SqlParameter("@id", selectedUserId.Value) });
                    MessageBox.Show("User deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearForm();
                    LoadUsers();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void ClearForm()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            cmbRole.SelectedIndex = -1;
            chkIsActive.Checked = true;
            selectedUserId = null;
            txtSearch.Clear();
            lblStatus.Text = "Ready";
            lblStatus.ForeColor = Color.Gray;
        }

        private bool Validate()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Username is required!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }
            if (selectedUserId == null && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new user!", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(cmbRole.Text))
            {
                MessageBox.Show("Please select a role.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool UsernameExists(string username)
        {
            string sql = @"
        SELECT COUNT(*) 
        FROM [User] 
        WHERE Username = @u" +
                (selectedUserId.HasValue ? " AND UserId <> @id" : "");

            var parameters = selectedUserId.HasValue
                ? new[]
                  {
              new SqlParameter("@u", username),
              new SqlParameter("@id", selectedUserId.Value)
                  }
                : new[] { new SqlParameter("@u", username) };

            // CORRECT: Use ExecuteScalar for COUNT(*)
            object result = DatabaseHelper.ExecuteScalar(sql, parameters);

            return Convert.ToInt32(result) > 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string kw = txtSearch.Text.Trim();
            string sql = string.IsNullOrEmpty(kw)
                ? "SELECT UserId, Username, Role, IsActive FROM [User] ORDER BY Username"
                : "SELECT UserId, Username, Role, IsActive FROM [User] WHERE Username LIKE @kw ORDER BY Username";

            var parameters = string.IsNullOrEmpty(kw)
                ? null
                : new[] { new SqlParameter("@kw", "%" + kw + "%") };

            DataTable dt = DatabaseHelper.ExecuteQuery(sql, parameters);
            dgvUsers.DataSource = dt;
            LoadUsers(); // Re-apply formatting
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listViewUsers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listViewUsers_DoubleClick(object sender, EventArgs e)
        {

        }

        private void dgvUsers_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= dgvUsers.Rows.Count) return;

            var row = dgvUsers.Rows[e.RowIndex];
            selectedUserId = Convert.ToInt32(row.Cells["UserId"].Value);

            txtUsername.Text = row.Cells["Username"].Value?.ToString() ?? "";
            txtPassword.Text = ""; // Never show password
            cmbRole.Text = row.Cells["Role"].Value?.ToString() ?? "";

            // SAFE BOOLEAN READ
            if (row.Cells["IsActive"].Value != null && row.Cells["IsActive"].Value != DBNull.Value)
                chkIsActive.Checked = Convert.ToBoolean(row.Cells["IsActive"].Value);
            else
                chkIsActive.Checked = false;

            lblStatus.Text = $"Selected: {txtUsername.Text}";
            lblStatus.ForeColor = Color.DarkBlue;
        }

        private void dgvUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //if (dgvUsers.Columns[e.ColumnIndex].Name == "IsActive" &&
            //        e.Value != null &&
            //        e.Value != DBNull.Value)
            //{
            //    //bool active = Convert.ToBoolean(e.Value);
            //    e.Value = active ? "Active" : "Inactive";
            //    e.CellStyle.ForeColor = active ? Color.Green : Color.Red;
            //    e.FormattingApplied = true;
            //}
        }

        private void UserForm_Load(object sender, EventArgs e)
        {

        }
    }
}
