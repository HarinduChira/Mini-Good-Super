using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class SignUp : Form
    {
        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string nic, userName, pass, passCon;

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.ShowDialog();
            this.Close();
        }

        public SignUp()
        {
            InitializeComponent();
        }

        private void btnSignIN_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            userName = txtUserName.Text;
            pass = txtPass.Text;
            passCon = txtPassCon.Text;

            if (pass == passCon)
            {              
                string insertSql = "INSERT INTO UserStore (NIC,UserName,Password) VALUES (@Value1, @Value2,@value3)";

                string selectSql = "SELECT * FROM Employee WHERE NIC = @SearchValue";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchValue", nic);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                reader.Close();

                                using (SqlCommand cmd = new SqlCommand(insertSql, connection))
                                {
                                    cmd.Parameters.AddWithValue("@Value1", nic);
                                    cmd.Parameters.AddWithValue("@Value2", userName);
                                    cmd.Parameters.AddWithValue("@Value3", pass);

                                    cmd.ExecuteNonQuery();
                                }

                                MessageBox.Show("Signed Successfully");

                                txtNIC.Clear();
                                txtUserName.Clear();
                                txtPass.Clear();
                                txtPassCon.Clear();
                            }
                            else
                            {
                                MessageBox.Show("Entered NIC IS not Available");
                            }
                        }
                    }
                    connection.Close();
                }
            }
            else
            {
                MessageBox.Show("Password Mismatch");
            }          
        }
    }
}
