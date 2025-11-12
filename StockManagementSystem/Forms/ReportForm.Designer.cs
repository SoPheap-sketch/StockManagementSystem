namespace StockManagementSystem.Forms
{
    partial class ReportForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelProducts = new System.Windows.Forms.Panel();
            this.panelCustomers = new System.Windows.Forms.Panel();
            this.panelOrders = new System.Windows.Forms.Panel();
            this.panelRevenue = new System.Windows.Forms.Panel();
            this.lblProductsTitle = new System.Windows.Forms.Label();
            this.lblProducts = new System.Windows.Forms.Label();
            this.lblCustomersTitle = new System.Windows.Forms.Label();
            this.lblCustomers = new System.Windows.Forms.Label();
            this.lblOrders = new System.Windows.Forms.Label();
            this.lblOrdersTitle = new System.Windows.Forms.Label();
            this.lblRevenue = new System.Windows.Forms.Label();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.ChartSales = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panelProducts.SuspendLayout();
            this.panelCustomers.SuspendLayout();
            this.panelOrders.SuspendLayout();
            this.panelRevenue.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartSales)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Report Summary";
            // 
            // panelProducts
            // 
            this.panelProducts.BackColor = System.Drawing.Color.LightSkyBlue;
            this.panelProducts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelProducts.Controls.Add(this.lblProducts);
            this.panelProducts.Controls.Add(this.lblProductsTitle);
            this.panelProducts.Location = new System.Drawing.Point(20, 80);
            this.panelProducts.Name = "panelProducts";
            this.panelProducts.Size = new System.Drawing.Size(200, 120);
            this.panelProducts.TabIndex = 1;
            // 
            // panelCustomers
            // 
            this.panelCustomers.BackColor = System.Drawing.Color.LightGreen;
            this.panelCustomers.Controls.Add(this.lblCustomers);
            this.panelCustomers.Controls.Add(this.lblCustomersTitle);
            this.panelCustomers.Location = new System.Drawing.Point(240, 80);
            this.panelCustomers.Name = "panelCustomers";
            this.panelCustomers.Size = new System.Drawing.Size(200, 120);
            this.panelCustomers.TabIndex = 2;
            
            // 
            // panelOrders
            // 
            this.panelOrders.BackColor = System.Drawing.Color.LightYellow;
            this.panelOrders.Controls.Add(this.lblOrdersTitle);
            this.panelOrders.Controls.Add(this.lblOrders);
            this.panelOrders.ForeColor = System.Drawing.Color.LightYellow;
            this.panelOrders.Location = new System.Drawing.Point(455, 80);
            this.panelOrders.Name = "panelOrders";
            this.panelOrders.Size = new System.Drawing.Size(204, 120);
            this.panelOrders.TabIndex = 3;
            // 
            // panelRevenue
            // 
            this.panelRevenue.BackColor = System.Drawing.Color.LightCoral;
            this.panelRevenue.Controls.Add(this.lblRevenueTitle);
            this.panelRevenue.Controls.Add(this.lblRevenue);
            this.panelRevenue.Location = new System.Drawing.Point(670, 80);
            this.panelRevenue.Name = "panelRevenue";
            this.panelRevenue.Size = new System.Drawing.Size(210, 120);
            this.panelRevenue.TabIndex = 4;
            // 
            // lblProductsTitle
            // 
            this.lblProductsTitle.AutoSize = true;
            this.lblProductsTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProductsTitle.Location = new System.Drawing.Point(37, 19);
            this.lblProductsTitle.Name = "lblProductsTitle";
            this.lblProductsTitle.Size = new System.Drawing.Size(97, 17);
            this.lblProductsTitle.TabIndex = 0;
            this.lblProductsTitle.Text = "Total Products";
            // 
            // lblProducts
            // 
            this.lblProducts.AutoSize = true;
            this.lblProducts.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProducts.Location = new System.Drawing.Point(71, 59);
            this.lblProducts.Name = "lblProducts";
            this.lblProducts.Size = new System.Drawing.Size(28, 32);
            this.lblProducts.TabIndex = 1;
            this.lblProducts.Text = "0";
            // 
            // lblCustomersTitle
            // 
            this.lblCustomersTitle.AutoSize = true;
            this.lblCustomersTitle.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomersTitle.ForeColor = System.Drawing.Color.Black;
            this.lblCustomersTitle.Location = new System.Drawing.Point(43, 20);
            this.lblCustomersTitle.Name = "lblCustomersTitle";
            this.lblCustomersTitle.Size = new System.Drawing.Size(108, 17);
            this.lblCustomersTitle.TabIndex = 1;
            this.lblCustomersTitle.Text = "Total Customers";
            // 
            // lblCustomers
            // 
            this.lblCustomers.AutoSize = true;
            this.lblCustomers.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomers.Location = new System.Drawing.Point(85, 60);
            this.lblCustomers.Name = "lblCustomers";
            this.lblCustomers.Size = new System.Drawing.Size(28, 32);
            this.lblCustomers.TabIndex = 2;
            this.lblCustomers.Text = "0";
            // 
            // lblOrders
            // 
            this.lblOrders.AutoSize = true;
            this.lblOrders.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrders.ForeColor = System.Drawing.Color.Black;
            this.lblOrders.Location = new System.Drawing.Point(56, 20);
            this.lblOrders.Name = "lblOrders";
            this.lblOrders.Size = new System.Drawing.Size(84, 17);
            this.lblOrders.TabIndex = 1;
            this.lblOrders.Text = "Total Orders";
            // 
            // lblOrdersTitle
            // 
            this.lblOrdersTitle.AutoSize = true;
            this.lblOrdersTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOrdersTitle.ForeColor = System.Drawing.Color.Black;
            this.lblOrdersTitle.Location = new System.Drawing.Point(77, 60);
            this.lblOrdersTitle.Name = "lblOrdersTitle";
            this.lblOrdersTitle.Size = new System.Drawing.Size(28, 32);
            this.lblOrdersTitle.TabIndex = 2;
            this.lblOrdersTitle.Text = "0";
            // 
            // lblRevenue
            // 
            this.lblRevenue.AutoSize = true;
            this.lblRevenue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenue.ForeColor = System.Drawing.Color.Black;
            this.lblRevenue.Location = new System.Drawing.Point(56, 21);
            this.lblRevenue.Name = "lblRevenue";
            this.lblRevenue.Size = new System.Drawing.Size(95, 17);
            this.lblRevenue.TabIndex = 1;
            this.lblRevenue.Text = "Total Revenue";
            // 
            // lblRevenueTitle
            // 
            this.lblRevenueTitle.AutoSize = true;
            this.lblRevenueTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRevenueTitle.ForeColor = System.Drawing.Color.Black;
            this.lblRevenueTitle.Location = new System.Drawing.Point(84, 60);
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            this.lblRevenueTitle.Size = new System.Drawing.Size(28, 32);
            this.lblRevenueTitle.TabIndex = 2;
            this.lblRevenueTitle.Text = "0";
            // 
            // ChartSales
            // 
            chartArea2.Name = "ChartArea1";
            this.ChartSales.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.ChartSales.Legends.Add(legend2);
            this.ChartSales.Location = new System.Drawing.Point(20, 230);
            this.ChartSales.Name = "ChartSales";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.ChartSales.Series.Add(series2);
            this.ChartSales.Size = new System.Drawing.Size(860, 250);
            this.ChartSales.TabIndex = 5;
            this.ChartSales.Text = "chart1";
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(933, 588);
            this.Controls.Add(this.ChartSales);
            this.Controls.Add(this.panelRevenue);
            this.Controls.Add(this.panelOrders);
            this.Controls.Add(this.panelCustomers);
            this.Controls.Add(this.panelProducts);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Report Summar";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.panelProducts.ResumeLayout(false);
            this.panelProducts.PerformLayout();
            this.panelCustomers.ResumeLayout(false);
            this.panelCustomers.PerformLayout();
            this.panelOrders.ResumeLayout(false);
            this.panelOrders.PerformLayout();
            this.panelRevenue.ResumeLayout(false);
            this.panelRevenue.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChartSales)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelProducts;
        private System.Windows.Forms.Panel panelCustomers;
        private System.Windows.Forms.Panel panelOrders;
        private System.Windows.Forms.Panel panelRevenue;
        private System.Windows.Forms.Label lblProducts;
        private System.Windows.Forms.Label lblProductsTitle;
        private System.Windows.Forms.Label lblCustomers;
        private System.Windows.Forms.Label lblCustomersTitle;
        private System.Windows.Forms.Label lblOrdersTitle;
        private System.Windows.Forms.Label lblOrders;
        private System.Windows.Forms.Label lblRevenueTitle;
        private System.Windows.Forms.Label lblRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart ChartSales;
    }
}