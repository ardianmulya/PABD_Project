using PABD_Project.Relasi;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Button = System.Windows.Forms.Button;

namespace PABD_Project.Forms
{
    public partial class ADMIN : Form
    {
        SqlConnection conn = new SqlConnection(@"Data Source=MSI\ARDIANMULYA;Initial Catalog=final;Integrated Security=True");
        public ADMIN()
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

        private void Add_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_petugas from dbo.Petugas Where ID_petugas = " +(textBox1.Text),conn);
            SqlDataAdapter da = new SqlDataAdapter(check);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count>0)
            {
                MessageBox.Show("Data sudah ada", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (string.IsNullOrEmpty(textBox1.Text)|| string.IsNullOrEmpty(textBox2.Text)|| string.IsNullOrEmpty(textBox3.Text)|| string.IsNullOrEmpty(textBox4.Text))
            {
                MessageBox.Show("Data tidak lengkap", "Gagal", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("insert into dbo.Petugas (ID_petugas,Nama,No_TLP,Alamat)values(@ID,@nama,@no_tlp,@alamat)", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("@ID", textBox1.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@nama", textBox2.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@no_tlp", textBox3.Text.ToString()));
                cmd.Parameters.Add(new SqlParameter("@alamat", textBox4.Text.ToString()));
                cmd.ExecuteNonQuery();

                conn.Close();
                MessageBox.Show("Data Berhasil ditambahkan");
                refreshform();
                Lihatdata();
            }

        }

        private void Lihatdata()
        {
            conn.Open();
            string str = "select * from dbo.Petugas";
            SqlDataAdapter da = new SqlDataAdapter(str, conn);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            conn.Close();
        }

        private void ADMIN_Load(object sender, EventArgs e)
        {
            Lihatdata();
            LoadTheme();
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            conn.Open();
            SqlCommand check = new SqlCommand("select ID_petugas from dbo.Petugas Where ID_petugas = " + (textBox1.Text), conn);
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
                if(MessageBox.Show("Apakah anda yakin ingin menghapus data ini?","Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("delete dbo.Petugas where ID_petugas=" + (textBox1.Text) + "", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Data Berhasil dihapus");
                    refreshform();
                    Lihatdata();
                }
            }
            else
            {
                MessageBox.Show("ISI ID_petugas yang akan dihapus");
            }
        }

        private void Search_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                conn.Open();
                string str = "select * from dbo.Petugas Where ID_petugas = " + (textBox1.Text);
                SqlDataAdapter da = new SqlDataAdapter(str, conn);
                DataSet ds = new DataSet();
                da.Fill(ds);
                dataGridView1.DataSource = ds.Tables[0];
                conn.Close();
                refreshform();
            }
            else
            {
                MessageBox.Show("ISI ID_petugas yang akan dicari");
            }
        }

        private void Refresh_Click(object sender, EventArgs e)
        {
            Lihatdata();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>=0)
            {
                DataGridViewRow row = this.dataGridView1.Rows[e.RowIndex];
                textBox1.Text = row.Cells["ID_petugas"].Value.ToString();
                textBox2.Text = row.Cells["Nama"].Value.ToString();
                textBox3.Text = row.Cells["No_TLP"].Value.ToString();
                textBox4.Text = row.Cells["Alamat"].Value.ToString();
            }
        }

        private void Update_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Apakah anda yakin ingin mengupdate data ini?", "Delete Record", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("update dbo.Petugas set Nama = '"+textBox2.Text+"',No_TLP = '"+textBox3.Text+"',Alamat='"+textBox4.Text+"'where ID_petugas = " + int.Parse(textBox1.Text), conn);
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Data Berhasil diupdate");
                refreshform();
                Lihatdata();
            }
        }
    }
}
