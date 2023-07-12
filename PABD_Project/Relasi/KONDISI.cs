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
    public partial class KONDISI : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MSI\ARDIANMULYA;Initial Catalog=final;Integrated Security=True");
        public KONDISI()
        {
            InitializeComponent();
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
            textBox1.Focus();
        }

        private void Lihatdata()
        {
            conn.Open();
            string str = "select * from dbo.Kondisi";
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void KONDISI_Load(object sender, EventArgs e)
        {
            LoadTheme();
            Lihatdata();
        }

        private void Refresh_Click_1(object sender, EventArgs e)
        {
            refreshform();
            Lihatdata();
        }

        private void Add_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_kondisi from dbo.Kondisi Where ID_kondisi = '" + (textBox1.Text) + "'", conn);
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
                SqlCommand cmd = new SqlCommand("insert into dbo.Kondisi (ID_kondisi,Kondisi_bus,ID_bus,ID_gudang)values(@ID,@kondisi,@bus,@gudang)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID", textBox1.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@kondisi", textBox2.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@bus", textBox3.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@gudang", textBox4.Text.ToString()));
                cmd.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Data Berhasil ditambahkan");
                refreshform();
                Lihatdata();
            }
        }

        private void Delete_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_kondisi from dbo.Kondisi Where ID_kondisi = '" + (textBox1.Text) + "'", conn);
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
                    SqlCommand cmd = new SqlCommand("delete dbo.Kondisi where ID_kondisi = '" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil dihapus");
                    refreshform();
                    Lihatdata();
                }
            }
            else
            {
                MessageBox.Show("ISI ID_kondisi yang akan dihapus");
            }
        }

        private void Search_Click_1(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_kondisi from dbo.Kondisi Where ID_kondisi = '" + (textBox1.Text) + "'", conn);
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
                string str = "select * from dbo.Kondisi Where ID_kondisi = '" + (textBox1.Text) + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
                refreshform();
            }
            else
            {
                MessageBox.Show("ISI ID_kondisi yang akan dicari");
            }
        }

        private void Update_Click_1(object sender, EventArgs e)
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
                    SqlCommand cmd = new SqlCommand("update dbo.Kondisi set Kondisi_bus = '" + textBox2.Text + "',ID_bus = '" + textBox3.Text + "',ID_gudang='" + textBox4.Text + "'where ID_kondisi = '" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil diupdate");
                    refreshform();
                    Lihatdata();
                }
            }
        }

        private void dataGridView1_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["ID_kondisi"].Value.ToString();
                textBox2.Text = row.Cells["Kondisi_bus"].Value.ToString();
                textBox3.Text = row.Cells["ID_bus"].Value.ToString();
                textBox4.Text = row.Cells["ID_gudang"].Value.ToString();
            }
        }
    }
}
