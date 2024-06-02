using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class StockList : Form
    {
        string userName;
        string role;
        public StockList(string userName,string role)
        {
            InitializeComponent();

            this.userName = userName;  
            this.role = role;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.openWindow(role,userName);
            this.Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            stockList11.Clear();
            sqlDataAdapter1.Fill(stockList11);
        }
    }
}
