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
using System.Xml.Linq;

namespace SuperMarket
{
    public partial class ProReg : Form
    {
        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string proID, proName, oldProID;

        string userName;

        float price;

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
                                while (reader.Read())
                                {
                                    txtPID.Text = reader["ProductID"].ToString();
                                    txtProName.Text = reader["ProName"].ToString();
                                    txtPrice.Text = reader["Price"].ToString();

                                    MessageBox.Show("Product Details Available");

                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                    txtProName.Enabled = false;
                                    txtPrice.Enabled = false;
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

            txtProName.Enabled = true;
            txtPrice.Enabled = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;
            proName = txtProName.Text;
            price = float.Parse(txtPrice.Text);

            string updateSql = "UPDATE Product SET ProductID = @NewValue1, ProName = @NewValue2 , Price = @NewValue3 WHERE ProductID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@NewValue1", proID);
                    command.Parameters.AddWithValue("@NewValue2", proName);
                    command.Parameters.AddWithValue("@NewValue3", price);
                    command.Parameters.AddWithValue("@PrimaryKeyValue", oldProID);

                    // Execute the SQL command
                    command.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    txtPID.Clear();
                    txtProName.Clear();
                    txtPrice.Clear();

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

            string deleteSql = "DELETE FROM Product WHERE ProductID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@PrimaryKeyValue", proID);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Product Details Removed");

                    txtPID.Clear();
                    txtProName.Clear();
                    txtPrice.Clear();

                    txtPID.Enabled = true;
                    txtProName.Enabled = true;
                    txtPrice.Enabled = true;

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

        public ProReg(string userName)
        {
            InitializeComponent();

            this.userName = userName;

            btnEdit.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            proID = txtPID.Text;
            proName = txtProName.Text;
            price = float.Parse(txtPrice.Text);

            string insertSql = "INSERT INTO Product (ProductID,ProName,Price) VALUES (@Value1, @Value2,@Value3)";


            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Value1", proID);
                    command.Parameters.AddWithValue("@Value2", proName);
                    command.Parameters.AddWithValue("@Value3", price);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            MessageBox.Show("Product Details Added");
            
            txtPID.Clear();
            txtProName.Clear();
            txtPrice.Clear();
        }


    }
}
