using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Net;

namespace SuperMarket
{

    public partial class CusReg : Form
    {
        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string nic, name, contactNO, oldnic;

        string userName;

        float totTrans;

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = false;

            txtName.Enabled = true;
            txtContact.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            name = txtName.Text;
            contactNO = txtContact.Text;

            string updateSql = "UPDATE Customer SET NIC = @NewValue1, Name = @NewValue2 , ContactNo = @NewValue3  WHERE NIC = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@NewValue1", nic);
                    command.Parameters.AddWithValue("@NewValue2", name);
                    command.Parameters.AddWithValue("@NewValue3", contactNO);
                    command.Parameters.AddWithValue("@PrimaryKeyValue", oldnic);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    txtNIC.Clear();
                    txtName.Clear();
                    txtContact.Clear();

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;

                    oldnic = "";
                }
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;

            string deleteSql = "DELETE FROM Customer WHERE NIC = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@PrimaryKeyValue", nic);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Customer Details Removed");

                    txtNIC.Clear();
                    txtName.Clear();
                    txtContact.Clear();

                    txtName.Enabled = true;
                    txtContact.Enabled = true;

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                }
                connection.Close();
            }
        }




        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Cashier cashier = new Cashier(userName);
            cashier.ShowDialog();
            this.Close();   
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;

            oldnic = nic;

            if (nic == null)
            {
                MessageBox.Show("Please enter the NIC to Search");
            }
            else
            {
                string selectSql = "SELECT * FROM Customer WHERE NIC = @SearchValue";

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
                                while (reader.Read())
                                {
                                    txtNIC.Text = reader["NIC"].ToString();
                                    txtName.Text = reader["Name"].ToString();
                                    txtContact.Text = reader["ContactNo"].ToString();

                                    MessageBox.Show("Customer Details Available");

                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                    txtName.Enabled = false;
                                    txtContact.Enabled = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Entered NIC is not Available");
                            }                            
                        }
                    }
                    connection.Close();
                }
            }
        }

        public CusReg(string user)
        {
            InitializeComponent();

            this.userName = user;

            btnEdit.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            name = txtName.Text;
            contactNO = txtContact.Text;

            totTrans = 0;
            


            string insertSql = "INSERT INTO Customer (NIC,Name,ContactNo,TotTrans) VALUES (@Value1, @Value2,@Value3,@Value4)";


            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Value1", nic);
                    command.Parameters.AddWithValue("@Value2", name);
                    command.Parameters.AddWithValue("@Value3", contactNO);
                    command.Parameters.AddWithValue("@Value4", totTrans);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Customer Added Successfully");
            txtNIC.Clear();
            txtName.Clear();
            txtContact.Clear();
        }
    }
}
