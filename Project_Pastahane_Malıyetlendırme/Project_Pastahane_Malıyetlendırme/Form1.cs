using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Project_Pastahane_Malıyetlendırme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=MSI\SQLEXPRESS;Initial Catalog=DbPastahaneMalıyet;Integrated Security=True;Encrypt=True;TrustServerCertificate=True");

        void MalzemeLıste()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select* From TBLMALZEMELER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void UrunLıstesı()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select * From TBLURUNLER", baglanti);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            dataGridView1.DataSource = dt2;
        }
        void Kasa()
        {
            SqlDataAdapter da3 = new SqlDataAdapter("Select * From TBLKASA", baglanti);
            DataTable dt3 = new DataTable();
            da3.Fill(dt3);
            dataGridView1.DataSource = dt3;
        }
        void urunler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * from TBLURUNLER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbUrun.ValueMember = "URUNID";
            CmbUrun.DisplayMember = "AD";
            CmbUrun.DataSource = dt;
            baglanti.Close();
        }
        void malzemeler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From TBLMALZEMELER", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CmbMalzeme.ValueMember = "MALZEMEID";
            CmbMalzeme.DisplayMember = "AD";
            CmbMalzeme.DataSource = dt;
            baglanti.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MalzemeLıste();
            urunler();
            malzemeler();
        }

        private void BtnUrunLıstesı_Click(object sender, EventArgs e)
        {
            UrunLıstesı();
        }

        private void BtnMalzemeLıstesı_Click(object sender, EventArgs e)
        {
            MalzemeLıste();
        }

        private void BtnKasa_Click(object sender, EventArgs e)
        {
            Kasa();
        }

        private void BtnMalzemeEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLMALZEMELER (AD,STOK,FIYAT,NOTLAR) VALUES (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtMalzemeAd.Text);
            komut.Parameters.AddWithValue("@p2", decimal.Parse(TxtMalzemeStok.Text));
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtMalzemeFıyat.Text));
            komut.Parameters.AddWithValue("@p4", TxtMalzemeNotlar.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Eklendi");
            MalzemeLıste();
        }

        private void BtnUrunEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut2 = new SqlCommand("INSERT INTO TBLURUNLER (AD) VALUES (@p1)", baglanti);
            komut2.Parameters.AddWithValue("@p1", TxtUrunAd.Text);
            komut2.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürün Sisteme Eklendi");
            UrunLıstesı();
        }

        private void BtnUrunOlustur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("INSERT INTO TBLFIRIN (URUNID,MALZEMEID,MIKTAR,MALIYET) VALUES (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbUrun.SelectedValue);
            komut.Parameters.AddWithValue("@p2", CmbMalzeme.SelectedValue);
            komut.Parameters.AddWithValue("@p3", decimal.Parse(TxtUrunMıktar.Text));
            komut.Parameters.AddWithValue("@p4", decimal.Parse(TxtUrunMalıyet.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi");


            listBox1.Items.Add(CmbMalzeme.Text + "-" + TxtUrunMalıyet.Text);
        }

        private void TxtUrunMıktar_TextChanged(object sender, EventArgs e)
        {
            double malıyet;

            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select* From TBLMALZEMELER Where MALZEMEID=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", CmbMalzeme.SelectedValue);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                TxtUrunMalıyet.Text = dr[3].ToString();
            }
            baglanti.Close();
            malıyet = Convert.ToDouble(TxtUrunMalıyet.Text) / 1000 * Convert.ToDouble(TxtUrunMıktar.Text);

            TxtUrunMalıyet.Text=malıyet.ToString();
        }

        private void CmbMalzeme_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secılen = dataGridView1.SelectedCells[0].RowIndex;
            TxtUrunID.Text = dataGridView1.Rows[secılen].Cells[0].Value.ToString();
            TxtUrunAd.Text= dataGridView1.Rows[secılen].Cells[1].Value.ToString();


            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select Sum(MALIYET) From TBLFIRIN Where URUNID=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtUrunID.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while(dr.Read())
            {
                TxtMalıyetFıyat.Text = dr[0].ToString();
            }
            baglanti.Close();
        }

        private void BtnCıkıs_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
