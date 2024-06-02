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
    public partial class Sales : Form
    {
        string userName;
        string role = "Sales";

        public Sales(string user)
        {
            InitializeComponent();

            userName = user;

            lblUser.Text = userName;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void btnDisReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            DisReg disReg = new DisReg(userName,role);
            disReg.ShowDialog();
            this.Close();
        }

        private void btnDisView_Click(object sender, EventArgs e)
        {
            this.Hide();
            DisList disList = new DisList(userName, role);
            disList.ShowDialog();
            this.Close();
        }
    }
}
