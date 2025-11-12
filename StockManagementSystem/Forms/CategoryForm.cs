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

namespace StockManagementSystem.Forms
{
    public partial class CategoryForm : Form
    {
        private readonly CategoryService _service = new CategoryService();
        public CategoryForm()
        {
            InitializeComponent();
            SetupListView();
            LoadCategories();

        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void SetupListView()
        {
            // CLEAR any old columns
            listViewCategories.Columns.Clear();

            // ADD COLUMNS WITH PROPER WIDTH
            listViewCategories.Columns.Add("No.", 60);
            listViewCategories.Columns.Add("ID", 80);
            listViewCategories.Columns.Add("Name", 200);
            listViewCategories.Columns.Add("Description", 350);

            // BEAUTIFUL STYLE
            listViewCategories.View = View.Details;
            listViewCategories.FullRowSelect = true;
            listViewCategories.GridLines = true;
            listViewCategories.Font = new Font("Segoe UI", 10F);
        }
        private void LoadCategories()
        {
            listViewCategories.Items.Clear();
            try
            {
                var categories = _service.GetAll();
                int index = 1;
                foreach (var cat in categories)
                {
                    var item = new ListViewItem(new[] {
                        index.ToString(),
                        cat.CategoryId.ToString(),
                        cat.Name,
                        cat.Description ?? ""
                    });
                    item.Tag = cat;
                    listViewCategories.Items.Add(item);
                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading categories: " + ex.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void listViewCategories_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Please enter a category name.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtName.Text.Trim();
            string desc = txtDescription.Text.Trim();

            // Check for duplicate name
            if (_service.NameExists(name))
            {
                MessageBox.Show("A category with this name already exists.", "Duplicate",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var category = new Category
            {
                Name = name,
                Description = string.IsNullOrEmpty(desc) ? null : desc
            };

            try
            {
                _service.Add(category);
                MessageBox.Show("Category added successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                txtName.Clear();
                txtDescription.Clear();
                LoadCategories();
                txtName.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding category: " + ex.Message, "Database Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtName.Clear();
            txtDescription.Clear();
            txtName.Focus();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
