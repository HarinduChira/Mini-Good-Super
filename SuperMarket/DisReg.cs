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
    public partial class DisReg : Form
    {
        string role, userName;

        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string disID,oldDisID;

        float disPercentage;

        int minTotTrans, maxTotTrans;

        public DisReg(string userName,string role)
        {
            InitializeComponent();

            this.role = role;
            this.userName = userName;    
            
            btnEdit.Visible = false;
            btnDelete.Visible = false;
            btnUpdate.Visible = false;
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            btnUpdate.Visible = true;
            btnDelete.Visible = false;

            txtDisPer.Enabled = true;
            txtMaxTot.Enabled = true;
            txtMinTot.Enabled = true;

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            disID = txtDisID.Text;
            disPercentage = float.Parse(txtDisPer.Text);
            minTotTrans = int.Parse(txtMinTot.Text);
            maxTotTrans = int.Parse(txtMaxTot.Text);


            string updateSql = "UPDATE DiscountDetails SET DiscountID = @NewValue1, Percentage = @NewValue2 , MinTotTrans = @NewValue3 , MaxTotTrans = @NewValue4  WHERE DiscountID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(updateSql, connection))
                {
                    command.Parameters.AddWithValue("@NewValue1", disID);
                    command.Parameters.AddWithValue("@NewValue2", disPercentage);
                    command.Parameters.AddWithValue("@NewValue3", minTotTrans);
                    command.Parameters.AddWithValue("@NewValue4", maxTotTrans);

                    command.Parameters.AddWithValue("@PrimaryKeyValue", oldDisID);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Updated Successfully");

                    txtDisID.Clear();
                    txtDisPer.Clear();
                    txtMinTot.Clear();
                    txtMaxTot.Clear();

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;

                    oldDisID = "";
                }
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            disID = txtDisID.Text;

            string deleteSql = "DELETE FROM DiscountDetails WHERE DiscountID = @PrimaryKeyValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@PrimaryKeyValue", disID);

                    command.ExecuteNonQuery();

                    MessageBox.Show("Discount Details Removed");

                    txtDisID.Clear();
                    txtDisPer.Clear();
                    txtMaxTot.Clear();
                    txtMinTot.Clear();

                    txtDisPer.Enabled = true;
                    txtMaxTot.Enabled = true;
                    txtMinTot.Enabled = true;

                    btnEdit.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                }
                connection.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            disID = txtDisID.Text;

            oldDisID = disID;

            if (disID == null)
            {
                MessageBox.Show("Please enter the Discount ID to Search");
            }
            else
            {
                string selectSql = "SELECT * FROM DiscountDetails WHERE DiscountID = @SearchValue";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchValue", disID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    txtDisID.Text = reader["DiscountID"].ToString();
                                    txtDisPer.Text = reader["Percentage"].ToString();
                                    txtMinTot.Text = reader["MinTotTrans"].ToString();
                                    txtMaxTot.Text = reader["MaxTotTrans"].ToString();

                                    MessageBox.Show("Discount Details Available");

                                    btnEdit.Visible = true;
                                    btnDelete.Visible = true;

                                    txtDisPer.Enabled = false;
                                    txtMinTot.Enabled = false;
                                    txtMaxTot.Enabled = false;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Entered Discount ID is not Available");
                            }
                        }
                    }
                    connection.Close();
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            disID = txtDisID.Text;
            disPercentage = float.Parse(txtDisPer.Text);
            minTotTrans = int.Parse(txtMinTot.Text);
            maxTotTrans = int.Parse(txtMaxTot.Text);

            string insertSql = "INSERT INTO DiscountDetails (DiscountID,Percentage,MinTotTrans,MaxTotTrans) VALUES (@Value1, @Value2,@Value3,@Value4)";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Value1", disID);
                    command.Parameters.AddWithValue("@Value2", disPercentage);
                    command.Parameters.AddWithValue("@Value3", minTotTrans);
                    command.Parameters.AddWithValue("@Value4", maxTotTrans);


                    command.ExecuteNonQuery();
                }
                connection.Close();
            }

            MessageBox.Show("Discount Details Added Successfully");

            txtDisID.Clear();
            txtDisPer.Clear();
            txtMinTot.Clear();
            txtMaxTot.Clear();
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
