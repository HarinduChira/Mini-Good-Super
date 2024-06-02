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
    public partial class Login : Form
    {

        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string userName, pass, role, dbpass, dbrole, dbNic, dbName;

        public Login()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            userName = txtUserName.Text;
            pass = txtPass.Text;
            role = cmbRole.SelectedItem.ToString();

            string checkUserPass = "SELECT * FROM UserStore WHERE UserName = @SearchValue";            

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using(SqlCommand command = new SqlCommand(checkUserPass,connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", userName);

                    using(SqlDataReader reader = command.ExecuteReader() )
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                dbNic = reader["NIC"].ToString();
                                dbpass = reader["Password"].ToString();

                                if(dbpass == pass)
                                {
                                    if(checkRole(dbNic,role))
                                    {
                                        openWindow(role,dbName);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Password is Invalid");
                                }                                
                            }
                        }
                        else
                        {
                            MessageBox.Show("User Name is Invalid");
                        }
                    }
                }
            }
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.ShowDialog();
            this.Close();
        }

        private bool checkRole(string dbNic,string role)
        {
            string checkRole = "SELECT * FROM Employee WHERE NIC = @SearchValue";

            using(SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using(SqlCommand command = new SqlCommand(checkRole,connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", dbNic);

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.HasRows)
                        {
                            while(reader.Read())
                            {
                                dbrole = reader["Role"].ToString();
                                dbName = reader["Name"].ToString();
                            }
                        }
                        else
                        {
                            MessageBox.Show("NIC is Invalid");
                        }
                    }
                }

                connection.Close();
            }

            if (dbrole == role)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Role is Invalid");
                return false;
            }
        }

        public void openWindow(string role,string user)
        {
            switch (role)
            {
                case "Manager":
                    this.Hide();
                    Manager manager = new Manager(user);
                    manager.ShowDialog();
                    this.Close();                    
                    break;

                case "Cashier":
                    this.Hide();
                    Cashier cashier = new Cashier(user);
                    cashier.ShowDialog();
                    this.Close();
                    break;

                case "StoreKeeper":
                    this.Hide();
                    StoreKeeper storeKeeper = new StoreKeeper(user);
                    storeKeeper.ShowDialog();
                    this.Close();
                    break;

                case "HR":
                    this.Hide();
                    HR hR = new HR(user);
                    hR.ShowDialog();
                    this.Close();
                    break;

                case "Sales":
                    this.Hide();
                    Sales sales = new Sales(user);
                    sales.ShowDialog();
                    this.Close();
                    break;
            }
        }
    }
}
