using System;
using System.Windows.Forms;
using StockManagementSystem.Forms;

namespace StockManagementSystem
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new OrderForm());
            Application.Run(new Form1());
        }
    }
}
