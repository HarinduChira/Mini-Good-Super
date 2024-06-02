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
    public partial class HR : Form
    {
        string userName;
        string role = "HR";
        public HR(string user)
        {
            InitializeComponent();

            userName = user;

            lblDisplay.Text =  user;            
        }

        private void btnEmpReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            EmpReg empReg = new EmpReg(userName);
            empReg.ShowDialog();
            this.Close();
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
    }
}
