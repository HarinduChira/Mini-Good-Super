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
    public partial class Manager : Form
    {
        string userName;
        string role = "Manager";
        public Manager(string user)
        {
            InitializeComponent();

            userName = user;

            lblDisplay.Text =  user;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void btnEmpList_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmpList empList = new EmpList(userName,role);
            empList.ShowDialog();              
            this.Close();
        }

        private void btnInvList_Click(object sender, EventArgs e)
        {
            this.Hide();
            StockList stkList = new StockList(userName,role);
            stkList.ShowDialog();
            this.Close();
        }

        private void btnProList_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProList proList = new ProList(userName, role);
            proList.ShowDialog();
            this.Close();
        }

        private void btnCusList_Click(object sender, EventArgs e)
        {
            this.Hide();
            CusList cusList = new CusList(userName,role);
            cusList.ShowDialog();
            this.Close();
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Report report = new Report(userName, role);
            report.ShowDialog();
            this.Close();
        }
    }
}
