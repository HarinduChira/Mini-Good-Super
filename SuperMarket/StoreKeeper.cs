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
    public partial class StoreKeeper : Form
    {
        string userName;
        string role = "StoreKeeper";
        public StoreKeeper(string user)
        {
            InitializeComponent();
            userName = user;

            lblDisplay.Text =  user;
        }

        private void btnProReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProReg proReg = new ProReg(userName);
            proReg.ShowDialog();
            this.Close();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        private void btnStore_Click(object sender, EventArgs e)
        {
            this.Hide();
            Store store = new Store(userName);
            store.ShowDialog();
            this.Close();
        }

        private void btnViewProList_Click(object sender, EventArgs e)
        {
            this.Hide();
            ProList proList = new ProList(userName,role);
            proList.ShowDialog();
            this.Close();
        }

        private void btnViewStore_Click(object sender, EventArgs e)
        {
            this.Hide();
            StockList stckList = new StockList(userName,role);
            stckList.ShowDialog();
            this.Close();
        }
    }
}
