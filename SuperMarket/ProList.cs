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
    public partial class ProList : Form
    {
        string userName;
        string role;
        public ProList(string userName, string role)
        {
            InitializeComponent();

            this.userName = userName;
            this.role = role;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            proList11.Clear();
            sqlDataAdapter1.Fill(proList11);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.openWindow(role, userName);
            this.Close();
        }
    }
}
