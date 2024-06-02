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
    public partial class EmpList : Form
    {
        string userName;
        string role;
        public EmpList(string userName,string role)
        {
            InitializeComponent();
            this.userName = userName;
            this.role = role;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            empList11.Clear();
            sqlDataAdapter1.Fill(empList11);
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
