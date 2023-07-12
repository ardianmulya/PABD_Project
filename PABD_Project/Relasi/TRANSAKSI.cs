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

namespace PABD_Project.Relasi
{
    public partial class TRANSAKSI : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MSI\ARDIANMULYA;Initial Catalog=final;Integrated Security=True");
        public TRANSAKSI()
        {
            InitializeComponent();
        }

        private void TRANSAKSI_Load(object sender, EventArgs e)
        {
            LoadTheme();
            Lihatdata();
        }

        private void LoadTheme()
        {
            foreach (Control btns in this.Controls)
            {
                if (btns.GetType() == typeof(Button))
                {
                    Button btn = (Button)btns;
                    btn.BackColor = ThemeColor.PrimaryColor;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
            }
        }

        private void refreshform()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox1.Focus();
        }

        private void Lihatdata()
        {
            conn.Open();
            string str = "select * from dbo.Transaksi";
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            refreshform();
            Lihatdata();
        }

        private void Add_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_transaksi from dbo.Transaksi Where ID_transaksi = '" + (textBox1.Text) + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(check);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("Data sudah ada", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Data tidak lengkap", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into dbo.Transaksi (ID_transaksi,Harga,Jumlah_sewa,Total_harga,tanggal_sewa,ID_penyewa,ID_bus)values(@ID,@harga,@jumlah,@total,@tanggal,@penyewa,@bus)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID", textBox1.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@harga", Convert.ToInt32(textBox2.Text)));
                cmd.Parameters.Add(new SqlParameter("@jumlah", Convert.ToInt32(textBox3.Text)));
                cmd.Parameters.Add(new SqlParameter("@total", Convert.ToInt32(textBox4.Text)));
                cmd.Parameters.Add(new SqlParameter("@tanggal", dateTimePicker1.Value));
                cmd.Parameters.Add(new SqlParameter("@penyewa", textBox6.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@bus", textBox7.Text.ToString()));
                cmd.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Data Berhasil ditambahkan");
                refreshform();
                Lihatdata();
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_transaksi from dbo.Transaksi Where ID_transaksi = '" + (textBox1.Text) + "'", conn);
            SqlDataAdapter da = new SqlDataAdapter(check);
            DataTable dt = new DataTable();
            da.Fill(dt);
            conn.Close();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Data tidak ada", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text != "")
            {
                if (MessageBox.Show("Apakah anda yakin ingin menghapus data ini?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete dbo.Transaksi where ID_transaksi = '" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil dihapus");
                    refreshform();
                    Lihatdata();
                }
            }
            else
            {
                MessageBox.Show("ISI ID_transaksi yang akan dihapus");
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_transaksi from dbo.Transaksi Where ID_transaksi = '" + (textBox1.Text) + "'", conn);
            SqlDataAdapter sda = new SqlDataAdapter(check);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            conn.Close();
            if (dt.Rows.Count == 0)
            {
                MessageBox.Show("Data tidak ada", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (textBox1.Text != "")
            {
                conn.Open();
                string str = "select * from dbo.Transaksi Where ID_transaksi = '" + (textBox1.Text) + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
                refreshform();
            }
            else
            {
                MessageBox.Show("ISI ID_transaksi yang akan dicari");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["ID_transaksi"].Value.ToString();
                textBox2.Text = row.Cells["Harga"].Value.ToString();
                textBox3.Text = row.Cells["Jumlah_sewa"].Value.ToString();
                textBox4.Text = row.Cells["Total_harga"].Value.ToString();
                dateTimePicker1.Text = row.Cells["tanggal_sewa"].Value.ToString();
                textBox6.Text = row.Cells["ID_penyewa"].Value.ToString();
                textBox7.Text = row.Cells["ID_bus"].Value.ToString();
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Data tidak lengkap", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Apakah anda yakin ingin mengupdate data ini?", "Update Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update dbo.Transaksi set Harga = '" + textBox2.Text + "',Jumlah_sewa = '" + textBox3.Text + "',Total_harga='" + textBox4.Text + "',tanggal_sewa = '"+dateTimePicker1.Value+"',ID_penyewa = '"+textBox6.Text+"',ID_bus = '"+textBox7.Text+"'where ID_kondisi = '" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil diupdate");
                    refreshform();
                    Lihatdata();
                }
            }
        }
    }
}
