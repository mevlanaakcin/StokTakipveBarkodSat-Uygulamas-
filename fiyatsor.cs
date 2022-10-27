using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using System.Data.OleDb; // Access bağlantısı kurabilmek için.

namespace Barkod
{
    public partial class fiyatsor : Form
    {
        public fiyatsor()
        {
            InitializeComponent();
        }
        //Veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=data/database.accdb");

        private void barcodeTxt_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void barcodeTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                baglantim.Open();
                OleDbCommand sorgu = new OleDbCommand("SELECT * FROM orders WHERE orderbarcode='" + barcodeTxt.Text + "'", baglantim);
                OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                if (kayitokuma.Read())
                {

                    ordername.Text = kayitokuma.GetValue(1).ToString();
                    price.Text = kayitokuma.GetValue(6).ToString() + " ₺";
                    barcodeTxt.Clear();
                    baglantim.Close();
                }
                else
                {
                    ordername.Text = "Böyle Bir barkod numaralı ürün Yok !";
                    price.Text = "0,00 ₺";
                    barcodeTxt.Clear();
                    baglantim.Close();
                    MessageBox.Show("Böyle bir ürün bulunamadı", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
