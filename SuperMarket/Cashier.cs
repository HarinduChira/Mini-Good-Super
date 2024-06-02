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
    public partial class Cashier : Form
    {
        string userName;
        
        public Cashier(string user)
        {
            InitializeComponent();

            userName = user;

            lblDisplay.Text = user;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void btnCusReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusReg cusReg = new CusReg(userName);
            cusReg.ShowDialog();
            this.Close();
        }

        private void btnSale_Click(object sender, EventArgs e)
        {
            this.Hide();
            POS pOS = new POS(userName);
            pOS.ShowDialog();
            this.Close();
        }
    }
}
