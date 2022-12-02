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

namespace FitnessCenterOtomasyonu
{
    public partial class OdemeDGVV : Form
    {
        public OdemeDGVV()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Win10\Documents\SporDb.mdf;Integrated Security=True;Connect Timeout=30");

        public static DataTable DataSource { get; private set; }

        private void FillName()
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select uAdSoyad from UyeTbl", baglanti);
            SqlDataReader rdr;
            rdr = komut.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("UAdSoyad", typeof(string));
            dt.Load(rdr);
            AdSoyadCb.ValueMember = "UAdSoyad";
            AdSoyadCb.DataSource = dt;
            baglanti.Close();
        }

        private void AdFiltrele()
        {
            baglanti.Open();
            string query = "select*from OdemeTbl where OUye='" + AraTb.Text + "'";
            SqlDataAdapter sda = new SqlDataAdapter(query, baglanti);
            SqlCommandBuilder builder = new SqlCommandBuilder();
            var DS = new DataSet();
            sda.Fill(DS);
            odemeDGV.DataSource = DS.Tables[0];
            baglanti.Close();
        }
        private void uyeler()
        {
            baglanti.Open();
            string query = "select*from OdemeTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, baglanti);
            SqlCommandBuilder builder = new SqlCommandBuilder();
            var DS = new DataSet();
            sda.Fill(DS);
            odemeDGV.DataSource = DS.Tables[0];
            baglanti.Close();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            AdSoyadCb.Text = "";
            OdemeTb.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AnaSayfa anaSayfa = new AnaSayfa();
            anaSayfa.Show();
            this.Show();
        }

        private void Odeme_Load(object sender, EventArgs e)
        {
            FillName();
            uyeler();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (AdSoyadCb.Text == "" || OdemeTb.Text == "")
            {
                MessageBox.Show("Eksik Bilgi");
            }
            else
            {
                string odemeperiyot = Periyot.Value.Month.ToString() + Periyot.Value.ToString();
                baglanti.Open();
                SqlDataAdapter sda = new SqlDataAdapter("select count(*) from OdemeTbl where OUye='" + AdSoyadCb.SelectedValue.ToString() + "'and OAy='" + odemeperiyot + "'", baglanti);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                if (dt.Rows[0][0].ToString() == "1")
                {
                    MessageBox.Show("Zaten ödeme yapıldı.");
                }
                else
                {
                    string query = "insert into OdemeTbl values ('" + odemeperiyot + "','" + AdSoyadCb.SelectedValue.ToString() + "'," + OdemeTb.Text + ")";
                    SqlCommand komut = new SqlCommand(query, baglanti);
                    komut.ExecuteNonQuery();
                    MessageBox.Show("Tutar başarıyla ödendi.");
                }
                baglanti.Close();
                uyeler();
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdFiltrele();
            AraTb.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            uyeler();
        }

        private void AraTb_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
