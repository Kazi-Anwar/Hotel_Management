using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Hotel_Management
{
    public partial class Form1 : Form
    {
        readonly string cs = ConfigurationManager.ConnectionStrings["DbCon"].ConnectionString;
        SqlConnection Con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        public Form1()
        {
            InitializeComponent();
            InitializeTable();
        }

        private DataTable dt = new DataTable();
        private void InitializeTable()
        {
            dt.Columns.Add("Room", typeof(string));
            dt.Columns.Add("Date", typeof(DateTime));
            dt.Columns.Add("Charge", typeof(decimal));
        }

        public int InsertGuest(string Name, string Address, string Phone)
        {
            int guestID = 0;
            string query = "INSERT INTO Guest (Name, Address, Phone) VALUES (@Name, @Address, @Phone); SELECT SCOPE_IDENTITY();";

            Con = new SqlConnection(cs);
            cmd = new SqlCommand(query, Con);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Address", Address);
            cmd.Parameters.AddWithValue("@Phone", Phone);
            try
            {
                Con.Open();
                guestID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting order: " + ex.Message);
            }
            return guestID;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox2.Text == " " || textBox3.Text == " " || textBox4.Text == "")
                {
                    MessageBox.Show(" Missing Information");
                }
                else
                {
                    dt.Rows.Add(comboBox2.SelectedItem.ToString(), dateTimePicker1.Value.Date, textBox5.Text);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime Date = dateTimePicker1.Value.Date;
                string name = textBox2.Text.Trim();
                string address = textBox3.Text.Trim();
                string phone = textBox4.Text.Trim();
                int guestID = InsertGuest(name, address, phone);

                foreach (DataRow row in dt.Rows)
                {
                    string room = (string)row["Room"];
                    DateTime date = (DateTime)row["Date"];
                    decimal charge = (decimal)row["Charge"];


                    InsertBooking(guestID, room, date, charge);
                }
                MessageBox.Show("confirmed successfully.");
                dt.Clear();
            }
            catch
            {
                MessageBox.Show("Error");
            }
        }
        private void InsertBooking(int guestID, string room, DateTime date, decimal charge)
        {
            string query = "INSERT INTO Booking (GuestID, Room, Date, Charge) " +
                          " VALUES (@GuestID, @Room,@date, @Charge); ";

            Con = new SqlConnection(cs);
            cmd = new SqlCommand(query, Con);

            cmd.Parameters.AddWithValue("@GuestID", guestID);
            cmd.Parameters.AddWithValue("@Room", room);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@Charge", charge);

            try
            {
                Con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error inserting order detail: " + ex.Message);
            }
        }

        private void but_view_Click(object sender, EventArgs e)
        {
            Form2 obj = new Form2();
            obj.Show();
            this.Hide();
        }
    }
}
