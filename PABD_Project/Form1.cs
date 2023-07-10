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

namespace PABD_Project
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {
            string username, password;
            username = Username.Text;
            password = Password.Text;
            SqlConnection conn = new SqlConnection(@"Data Source=MSI\ARDIANMULYA;Initial Catalog=final;Integrated Security=True");
            try
            {
                string strkoneksi = "data source = MSI\\ARDIANMULYA;initial catalog = final;user ID = {0}; password = {1}";
                conn = new SqlConnection(string.Format(strkoneksi, username, password));
                conn.Open();

                MainMenu MM = new MainMenu();
                MM.Show();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("Invalid Login");
                Username.Clear();
                Password.Clear();
                Username.Focus();
            }
            finally
            {
                conn.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Password_TextChanged(object sender, EventArgs e)
        {

        }

        private void Username_TextChanged(object sender, EventArgs e)
        {

        }

        private void Clear_Click(object sender, EventArgs e)
        {
            Username.Clear();
            Password.Clear();
            Username.Focus();
        }
    }
}
