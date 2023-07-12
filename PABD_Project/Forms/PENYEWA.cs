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

namespace PABD_Project.Forms
{
    public partial class PENYEWA : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MSI\ARDIANMULYA;Initial Catalog=final;Integrated Security=True");
        public PENYEWA()
        {
            InitializeComponent();
        }

        private void PENYEWA_Load(object sender, EventArgs e)
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
            textBox5.Clear();
            textBox1.Focus();
        }

        private void Lihatdata()
        {
            conn.Open();
            string str = "select * from dbo.Penyewa";
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
            try
            {
                conn.Open();
                SqlCommand check = new SqlCommand("select ID_penyewa from dbo.Penyewa Where ID_penyewa = '" + (textBox1.Text) + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter(check);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Data sudah ada", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
                {
                    MessageBox.Show("Data tidak lengkap", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("insert into dbo.Penyewa (ID_penyewa,Nama_penyewa,Jaminan,No_TLP,usia)values(@ID,@nama,@jaminan,@tlp,@usia)", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add(new SqlParameter("@ID", textBox1.Text.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@nama", textBox2.Text.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@jaminan", textBox3.Text.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@tlp", textBox4.Text.ToString()));
                    cmd.Parameters.Add(new SqlParameter("@usia", Convert.ToInt32(textBox5.Text)));
                    cmd.ExecuteNonQuery();

                    conn.Close();
                    MessageBox.Show("Data Berhasil ditambahkan");
                    refreshform();
                    Lihatdata();
                }
            }
            catch(SqlException ex)
            {
                MessageBox.Show("Nomor telepon Harus 12 digit", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_penyewa from dbo.Penyewa Where ID_penyewa = '" + (textBox1.Text) + "'", conn);
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
                    SqlCommand cmd = new SqlCommand("delete dbo.Penyewa where ID_penyewa='" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil dihapus");
                    refreshform();
                    Lihatdata();
                }
            }
            else
            {
                MessageBox.Show("ISI ID_penyewa yang akan dihapus");
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_penyewa from dbo.Penyewa Where ID_penyewa = '" + (textBox1.Text) + "'", conn);
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
                string str = "select * from dbo.Penyewa Where ID_penyewa = '" + (textBox1.Text) + "'";
                SqlDataAdapter da = new SqlDataAdapter(str, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
                refreshform();
            }
            else
            {
                MessageBox.Show("ISI ID_penyewa yang akan dicari");
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text))
            {
                MessageBox.Show("Data tidak lengkap", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Apakah anda yakin ingin mengupdate data ini?", "Update Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("update dbo.Penyewa set Nama_penyewa = '" + textBox2.Text + "',Jaminan = '" + textBox3.Text + "',No_TLP='" + textBox4.Text + "',usia =" + textBox5.Text + "where ID_penyewa = '" + (textBox1.Text) + "'", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil diupdate");
                    refreshform();
                    Lihatdata();
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["ID_penyewa"].Value.ToString();
                textBox2.Text = row.Cells["Nama_penyewa"].Value.ToString();
                textBox3.Text = row.Cells["Jaminan"].Value.ToString();
                textBox4.Text = row.Cells["No_TLP"].Value.ToString();
                textBox5.Text = row.Cells["usia"].Value.ToString();
            }
        }
    }
}
