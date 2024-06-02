using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace SuperMarket
{
    public partial class EmpReg : Form
    {
        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string nic, name, contactNO, address, email, role, oldnic;

        string userName;

        int salary;

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            HR hr = new HR(userName);
            hr.ShowDialog();                
            this.Close();
        }

        public EmpReg(string userName)
        {
            InitializeComponent();

            btnEdit.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;

            this.userName = userName;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            name = txtName.Text;
            contactNO = txtContact.Text;
            address = txtAddress.Text;
            email = txtEmail.Text;
            role = cmbRole.SelectedItem.ToString();
            salary = int.Parse(txtSalary.Text);

            
            string insertSql = "INSERT INTO Employee (NIC,Name,ContactNo,Address,Email,Salary,Role) VALUES (@Value1, @Value2,@Value3,@Value4,@Value5,@Value6,@Value7)";


            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {           
                    command.Parameters.AddWithValue("@Value1", nic);
                    command.Parameters.AddWithValue("@Value2", name);
                    command.Parameters.AddWithValue("@Value3", contactNO);
                    command.Parameters.AddWithValue("@Value4", address);
                    command.Parameters.AddWithValue("@Value5", email);
                    command.Parameters.AddWithValue("@Value6", salary);
                    command.Parameters.AddWithValue("@Value7", role);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Employee Details Added");
            txtNIC.Clear();
            txtName.Clear();
            txtContact.Clear();
            txtAddress.Clear();
            txtEmail.Clear();
            txtSalary.Clear();
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
                                 while (reader.Read())
                                 {
                                    txtNIC.Text = reader["NIC"].ToString();
                                    txtName.Text = reader["Name"].ToString();
                                    txtContact.Text = reader["ContactNo"].ToString();
                                    txtAddress.Text = reader["Address"].ToString();
                                    txtEmail.Text = reader["Email"].ToString();
                                    txtSalary.Text = reader["Salary"].ToString();
                                    cmbRole.Text = reader["Role"].ToString();

                                    MessageBox.Show("Employee Details Available");

                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                    txtName.Enabled = false;
                                    txtContact.Enabled = false;
                                    txtAddress.Enabled = false;
                                    txtEmail.Enabled = false;
                                    txtSalary.Enabled = false;
                                    cmbRole.Enabled = false;
                                 }                                 
                            }
                            else {
                                MessageBox.Show("Entered NIC is not Available");
                            }                            
                        }
                    }
                    connection.Close();                 
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = false;

            txtName.Enabled = true;
            txtContact.Enabled = true;
            txtAddress.Enabled = true;
            txtEmail.Enabled =  true;
            txtSalary.Enabled = true;
            cmbRole.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            name = txtName.Text;
            contactNO = txtContact.Text;
            address = txtAddress.Text;
            email = txtEmail.Text;
            role = cmbRole.SelectedItem.ToString();
            salary = int.Parse(txtSalary.Text);

            string updateSql = "UPDATE Employee SET NIC = @NewValue1, Name = @NewValue2 , ContactNo = @NewValue3 , Address = @NewValue4 , Email = @NewValue5 , Salary = @NewValue6 , Role = @NewValue7 WHERE NIC = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@NewValue1", nic);
                    command.Parameters.AddWithValue("@NewValue2", name);
                    command.Parameters.AddWithValue("@NewValue3", contactNO);
                    command.Parameters.AddWithValue("@NewValue4", address);
                    command.Parameters.AddWithValue("@NewValue5", email);
                    command.Parameters.AddWithValue("@NewValue6", salary);
                    command.Parameters.AddWithValue("@NewValue7", role);
                    command.Parameters.AddWithValue("@PrimaryKeyValue", oldnic);

                    // Execute the SQL command
                    command.ExecuteNonQuery();

                    updateUserStore();

                    MessageBox.Show("Updated Successfully");

                    txtNIC.Clear();
                    txtName.Clear();
                    txtContact.Clear();
                    txtAddress.Clear();
                    txtEmail.Clear();
                    txtSalary.Clear();

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;

                    oldnic = "";
                }
                connection.Close();
            }

            void updateUserStore()
            {
                string updateSqlUserStore = "UPDATE UserStore SET NIC = @NewValue1 WHERE NIC = @PrimaryKeyValue";

                using(SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using(SqlCommand command = new SqlCommand(updateSqlUserStore, connection))
                    {
                        command.Parameters.AddWithValue("@NewValue1", nic);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", oldnic);

                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;

            string deleteSql = "DELETE FROM Employee WHERE NIC = @PrimaryKeyValue";

            using(SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@PrimaryKeyValue", nic);

                    command.ExecuteNonQuery();

                    deleteUserStoreDetails(nic);

                    MessageBox.Show("Employee Details Removed");

                    txtNIC.Clear();
                    txtName.Clear();
                    txtContact.Clear();
                    txtAddress.Clear();
                    txtEmail.Clear();
                    txtSalary.Clear();

                    txtName.Enabled = true;
                    txtContact.Enabled = true;
                    txtAddress.Enabled = true;
                    txtEmail.Enabled = true;
                    txtSalary.Enabled = true;
                    cmbRole.Enabled = true;

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;

                }
                connection.Close() ;
            }

            void deleteUserStoreDetails(string Nic)
            {
                string deleteSqlUserStore = "DELETE FROM UserStore WHERE NIC = @PrimaryKeyValue";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(deleteSqlUserStore, connection))
                    {
                        command.Parameters.AddWithValue("@PrimaryKeyValue", Nic);

                        command.ExecuteNonQuery();
                    }

                    connection.Close();
                }
            }
        }
    }
}
