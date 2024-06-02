using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class Report : Form
    {
        string userName, role;

        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.openWindow(role, userName);
            this.Close();
        }

        public Report(string userName,string role)
        {
            InitializeComponent();

            this.userName = userName;
            this.role = role;

            loadDetails();
        }

        private void loadDetails()
        {
            lblName.Text = userName;
            lblDate.Text = DateTime.Now.ToString();

            loadTotalSale();

            loadEmpCount();

            loadCusCount();

            loadProCount();

            loadDisCount();
        }

        private void loadTotalSale()
        {
            string totalSql = "SELECT SUM(TotTrans) as TotalSale FROM Customer";

            using(SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(totalSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    { 
                        reader.Read();

                        string totalSale = reader["TotalSale"].ToString();

                        lblTotal.Text = "Rs." + totalSale;

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        private void loadEmpCount()
        {
            string countSql = "SELECT Count(NIC) as EmpCount FROM Employee";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(countSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        string EmpCount = reader["EmpCount"].ToString();

                        lblEmpCount.Text = EmpCount ;

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        private void loadProCount()
        {
            string countSql = "SELECT Count(ProductID) as ProCount FROM Product";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(countSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        string ProCount = reader["ProCount"].ToString();

                        lblProCount.Text = ProCount;

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        private void loadCusCount()
        {
            string countSql = "SELECT Count(NIC) as CusCount FROM Customer";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(countSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        string CusCount = reader["CusCount"].ToString();

                        lblCusCount.Text = CusCount;

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }

        private void loadDisCount()
        {
            string countSql = "SELECT Count(DiscountID) as DisCount FROM DiscountDetails";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(countSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        string DisCount = reader["DisCount"].ToString();

                        lblDisCount.Text = DisCount;

                        reader.Close();
                    }
                }
                connection.Close();
            }
        }
    }
}
