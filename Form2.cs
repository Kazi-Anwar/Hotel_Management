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
    public partial class Form2 : Form
    {
        private readonly string connectionString = ConfigurationManager.ConnectionStrings["DbCon"].ConnectionString;
        public Form2()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
            RefreshDataGridView();
        }
        public void InitializeDatabaseConnection()
        {
            string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=HotelDB;Integrated Security=True";

        }
        public void RefreshDataGridView()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT \r\n    g.Name,\r\n    g.Address,\r\n    g.Phone,\r\n    bk.Date,\r\n    bk.Room,\r\n    bk.Charge\r\nFROM \r\n    Guest g\r\nINNER JOIN \r\n    Booking bk ON g.GuestID = bk.GuestID;";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable booking = new DataTable();
                    adapter.Fill(booking);
                    dataGridView1.DataSource = booking;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            Report report = new Report();
            report.Show();
        }
    }
}
