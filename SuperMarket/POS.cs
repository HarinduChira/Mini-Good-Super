using System;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http.Headers;
using System.Windows.Forms;

namespace SuperMarket
{

    public partial class POS : Form
    {
        string userName;
        string role = "Cashier";

        string conString = "Data Source=HARINDU;Initial Catalog=SuperMarket;Integrated Security=True";

        string nic, proId, proName, date, unitPrice;

        int qty;
        float newtot;
        float tempTran;

        private void btnAddtoCart_Click(object sender, EventArgs e)
        {
            proName = cmbProduct.SelectedItem.ToString();
            qty = int.Parse(txtQty.Text);

            string selectSql = "SELECT * FROM Product WHERE ProName = @SearchValue";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectSql, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", proName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            proId = reader["ProductID"].ToString();
                            unitPrice = reader["Price"].ToString();

                            float uPrice = float.Parse(unitPrice);

                            float totPrice = qty * uPrice;

                            if (updateStock(proId, qty))
                            {
                                addToCart(proId, proName, uPrice, qty, totPrice);
                            }
                            else
                            {
                                MessageBox.Show("Low Stock");
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        private void addToCart(string ProductID, string ProductName, float UnitPrice, int Quantity, float Total)
        {
            string insertSql = "INSERT INTO ShoppingCart (ProductID,ProductName,UnitPrice,Quantity,Total) VALUES (@Value1, @Value2,@Value3,@Value4,@Value5)";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(insertSql, connection))
                {
                    command.Parameters.AddWithValue("@Value1", ProductID);
                    command.Parameters.AddWithValue("@Value2", ProductName);
                    command.Parameters.AddWithValue("@Value3", UnitPrice);
                    command.Parameters.AddWithValue("@Value4", Quantity);
                    command.Parameters.AddWithValue("@Value5", Total);

                    command.ExecuteNonQuery();

                    txtQty.Clear();

                    MessageBox.Show("Added to the Cart");

                    sqlDataAdapter1.Fill(shopingDataList1);
                }
                connection.Close();
            }
        }

        private bool updateStock(string ProductID, int quantity)
        {
            int newqty;
            int oldqty;
            int dbqty;
            bool flag = true;

            string searchSql = "SELECT Quantity FROM Store WHERE ProductID = @value";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(searchSql, connection))
                {
                    command.Parameters.AddWithValue("@value", ProductID);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            dbqty = int.Parse(reader["Quantity"].ToString());

                            oldqty = dbqty;

                            newqty = dbqty - quantity;

                            if (newqty < 0)
                            {
                                newqty = oldqty;
                                flag = false;
                            }
                            else
                            {
                                update(newqty);
                                flag = true;
                            }
                        }
                    }
                }
                connection.Close();
            }

            if (flag)
            {
                return true;
            }
            else
            {
                return false;
            }

            void update(int qty)
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    string updateSql = "UPDATE Store SET Quantity = @NewValue1 WHERE ProductID = @PrimaryKeyValue";

                    connection.Open();
                    using (SqlCommand cmd = new SqlCommand(updateSql, connection))
                    {
                        cmd.Parameters.AddWithValue("@NewValue1", qty);
                        cmd.Parameters.AddWithValue("@PrimaryKeyValue", ProductID);

                        cmd.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login login = new Login();
            login.openWindow(role, userName);
            this.Close();
        }

        private void btnClearBill_Click(object sender, EventArgs e)
        {

            reverseCart();

            clearCart();
            shopingDataList1.Clear();
            sqlDataAdapter1.Fill(shopingDataList1);

            MessageBox.Show("Bill Cleared");
        }


        private void reverseCart()
        {
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                string selectSql = "SELECT ProductID,Quantity FROM ShoppingCart";

                using (SqlCommand command = new SqlCommand(selectSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            string proId;
                            int qty;

                            while (reader.Read())
                            {
                                proId = reader["ProductID"].ToString();
                                qty = int.Parse(reader["Quantity"].ToString());

                                reverseQty(proId, qty);
                            }
                        }
                        else
                        {
                            MessageBox.Show("No Transactions");
                        }
                    }
                }
                connection.Close();
            }

            void reverseQty(string proid, int qty)
            {
                string selectStockSql = "SELECT Quantity FROM Store WHERE ProductID = @value";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using(SqlCommand command = new SqlCommand(selectStockSql, connection))
                    {
                        command.Parameters.AddWithValue("@value", proid);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int oldQty;
                            int newQty;

                            while(reader.Read())
                            {
                                oldQty = int.Parse(reader["Quantity"].ToString());

                                newQty = oldQty + qty;

                                updateReverseStock(proid, newQty);
                            }
                        }
                    }
                    connection.Close();
                }
            }

            void updateReverseStock(string proid, int newqty)
            {
                string updateStockSql = "UPDATE Store SET Quantity = @NewValue1 WHERE ProductID = @PrimaryKeyValue";

                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open ();

                    using(SqlCommand command = new SqlCommand(updateStockSql, connection))
                    {
                        command.Parameters.AddWithValue("@NewValue1", newqty);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", proid);

                        command.ExecuteNonQuery();
                    }
                    connection.Close ();
                }
            }
        }
        

        private void btnTotalBill_Click(object sender, EventArgs e)
        {
            float total;

            string totalSql = "SELECT SUM(Total) as Tot FROM ShoppingCart";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(totalSql, connection))
                {
                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            string tot = reader["Tot"].ToString();

                            total = float.Parse(tot);

                            float newTot = checkDiscount(nic, total);

                            lblBillTot.Text = newTot.ToString();

                            updateCusTot(nic, newTot);

                            MessageBox.Show("Bill Finalized" + Environment.NewLine + "The Total Bill is : " + newTot );                            

                            this.Hide();
                            POS pos = new POS(userName);
                            pos.ShowDialog();
                            this.Close();                                          
                        }
                    }
                }
                connection.Close();
            }

        }

        private float checkDiscount(string nic, float tot)
        {
            this.newtot = tot;
            this.tempTran = tot;

            string selectCustrans = "SELECT TotTrans FROM CUSTOMER WHERE NIC = @SearchValue";
            

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectCustrans, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", nic);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            float cusTrans = float.Parse(reader["TotTrans"].ToString());

                            checkRange(cusTrans);
                        }
                    }
                }
                connection.Close();
            }
            return newtot; // Return the updated total after applying discounts
        }

        private void checkRange(float custrans)
        {
            string selectDis = "SELECT DiscountID,Percentage,MinTotTrans,MaxTotTrans FROM DiscountDetails";
            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectDis, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string disID = reader["DiscountID"].ToString();
                            float percentage = float.Parse(reader["Percentage"].ToString());
                            int minTran = int.Parse(reader["MinTotTrans"].ToString());
                            int maxTran = int.Parse(reader["MaxTotTrans"].ToString());

                            if (minTran <= custrans && custrans < maxTran)
                            {
                                DialogResult result = MessageBox.Show("Customer is Eligible for Discount Code : " + disID + " of " + percentage + "%" + Environment.NewLine + "Apply Discount ?", "Discount Available", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

                                if (result == DialogResult.OK)
                                {
                                    this.newtot = tempTran * ((100 - percentage) / 100);
                                    break;
                                }
                            }
                        }
                    }
                }
                connection.Close();
            }
        }

        private void updateCusTot(String NIC , float totalTrans)
        {
            float oldVal;
            float newVal;

            string updateSql = "UPDATE Customer SET TotTrans = @NewValue1 WHERE NIC = @PrimaryKeyValue";

            string selectSql = "SELECT TotTrans FROM CUSTOMER WHERE NIC = @SearchValue";

            using(SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open() ;

                using(SqlCommand command = new SqlCommand(selectSql, connection))
                {
                    command.Parameters.AddWithValue("@SearchValue", NIC);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string dbVal = reader["TotTrans"].ToString();
                            oldVal = float.Parse(dbVal);

                            newVal = oldVal + totalTrans;

                            Update();
                        }
                    }
                }                                  
                connection.Close() ;
            }

            void Update()
            {
                using (SqlConnection connection = new SqlConnection(conString))
                {
                    connection.Open();

                    using(SqlCommand command = new SqlCommand(updateSql, connection))
                    {

                        command.Parameters.AddWithValue("@NewValue1", newVal);
                        command.Parameters.AddWithValue("@PrimaryKeyValue", NIC);

                        command.ExecuteNonQuery();
                    }
                    connection.Close();
                }
            }
        }

        public POS(string userName)
        {
            InitializeComponent();
            this.userName = userName;

            groupBox2.Enabled = false;
            groupBox3.Enabled = false;

        }

        private void addPro()
        {
            cmbProduct.Items.Clear();

            string selectSql = "SELECT ProName FROM Product";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(selectSql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string productName = reader["ProName"].ToString();
                            cmbProduct.Items.Add(productName);
                        }
                    }
                }
                connection.Close();
            }
        }

        private void clearCart()
        {
            string deleteSql = "DELETE FROM ShoppingCart";

            using (SqlConnection connection = new SqlConnection(conString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(deleteSql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            nic = txtNIC.Text;
            date = dateTimePicker.Text;


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
                            MessageBox.Show("Customer Verified");

                            lblCusId.Text = nic;
                            lblDate.Text = date;

                            groupBox2.Enabled = true;
                            groupBox3.Enabled = true;

                            addPro();

                            clearCart();
                        }
                        else
                        {
                            MessageBox.Show("Entered Customer Details is unavailable " + Environment.NewLine + "Please add the Customer details to the database");
                        }
                    }
                }
                connection.Close();
            }
        }
    }
}
