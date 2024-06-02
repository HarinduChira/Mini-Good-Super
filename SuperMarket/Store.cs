using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperMarket
{
    public partial class Store : Form
    {
        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string proID, oldProID;

        string userName;

        int qty;


        public Store(string userName)
        {
            InitializeComponent();

            this.userName = userName;

            btnEdit.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;

            oldProID = proID;

            if (proID == null)
            {
                MessageBox.Show("Please enter the Product ID to Search");
            }
            else
            {
                string selectSql = "SELECT * FROM Store WHERE ProductID = @SearchValue";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchValue", proID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    txtPID.Text = reader["ProductID"].ToString();
                                    txtQuantity.Text = reader["Quantity"].ToString();

                                    MessageBox.Show("Product Details Available");

                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                    txtQuantity.Enabled = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Entered Product ID is not Available");
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

            txtQuantity.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;
            qty = int.Parse(txtQuantity.Text);

            string updateSql = "UPDATE Store SET ProductID = @NewValue1, Quantity = @NewValue2 WHERE ProductID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@NewValue1", proID);
                    command.Parameters.AddWithValue("@NewValue2", qty);
                    command.Parameters.AddWithValue("@PrimaryKeyValue", oldProID);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    txtPID.Clear();
                    txtQuantity.Clear();

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;

                    oldProID = "";
                }
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;

            string deleteSql = "DELETE FROM Store WHERE ProductID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@PrimaryKeyValue", proID);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Product Details Removed");

                    txtPID.Clear();
                    txtQuantity.Clear();

                    txtPID.Enabled = true;
                    txtQuantity.Enabled = true;

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
            StoreKeeper storeKeeper = new StoreKeeper(userName);
            storeKeeper.ShowDialog();
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;

            qty = int.Parse(txtQuantity.Text);

            string insertSql = "INSERT INTO Store (ProductID,Quantity) VALUES (@Value1, @Value2)";

            string selectSql = "SELECT * FROM Product WHERE ProductID = @SearchValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectSql, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", proID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {      
                            reader.Close();

                            using(SqlCommand cmd = new SqlCommand(insertSql, connection))
                            {
                                cmd.Parameters.AddWithValue("@Value1", proID);
                                cmd.Parameters.AddWithValue("@Value2", qty);

                                cmd.ExecuteNonQuery();
                            }

                            MessageBox.Show("Product Quantity Added");

                            txtPID.Clear();
                            txtQuantity.Clear();
                        }
                        else
                        {
                            MessageBox.Show("Entered Product ID is not Available");
                        }                                                               
                    }
                }
                connection.Close();
            }
        }
    }
}
