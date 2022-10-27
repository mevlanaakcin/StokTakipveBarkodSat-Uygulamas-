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
    public partial class userpanel : Form
    {
        public userpanel()
        {
            InitializeComponent();
        }
        //Veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=data/database.accdb");
        private void userpanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        void barkodsuz()
        {
            int i = 0;
            baglantim.Open();
            OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE orderunit='Adet' ORDER BY ordername ASC", baglantim);

            OleDbDataReader kayitokuma = sorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                dataGridView2.Rows.Add(i, kayitokuma.GetValue(1).ToString(), "1", kayitokuma.GetValue(2).ToString(), kayitokuma.GetValue(5).ToString() + " ₺");

                i++;

            }
            baglantim.Close();
        }
        void kilo()
        {
            kiloBox.Items.Add("Seçiniz...");
            int j = 0;
            baglantim.Open();
            OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE orderunit='Kilogram' ORDER BY ordername ASC", baglantim);

            OleDbDataReader kayitokuma = sorgu.ExecuteReader();
            while (kayitokuma.Read())
            {
                kiloBox.Items.Add(kayitokuma.GetValue(1).ToString());
                j++;

            }
            baglantim.Close();
            kiloBox.SelectedIndex = 0;
        }

        int sira = 1;
        float bakiye = 0;
        int orders = 0;
        float alinan = 0;
        private void userpanel_Load(object sender, EventArgs e)
        {
            total.Text = bakiye.ToString() + " ₺";
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "SIRA";
            dataGridView1.Columns[1].Name = "ÜRÜN";
            dataGridView1.Columns[2].Name = "MİKTAR";
            dataGridView1.Columns[3].Name = "BİRİM";
            dataGridView1.Columns[4].Name = "FİYAT";

            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 390;
            dataGridView1.Columns[2].Width = 50;
            dataGridView1.Columns[3].Width = 60;
            dataGridView1.Columns[4].Width = 60;

            /*** NORMAL BUTON EKLEME ***/
            DataGridViewButtonColumn rmvBtn = new DataGridViewButtonColumn();
            //Kolon Başlığı
            rmvBtn.HeaderText = "İŞLEM";
            // Butonun Text
            rmvBtn.Text = "SİL";
            // Butonda Text Kullanılmasını aktifleştirme
            rmvBtn.UseColumnTextForButtonValue = true;
            // Buton çerçeve rengi
            rmvBtn.DefaultCellStyle.BackColor = Color.Blue;
            // Buton seçiliykenki çerçeve rengi
            rmvBtn.DefaultCellStyle.SelectionBackColor = Color.Red;
            // Butonun genişiliği
            rmvBtn.Width = 40;
            // DataGridView e ekleme
            dataGridView1.Columns.Add(rmvBtn);



            dataGridView2.ColumnCount = 5;
            dataGridView2.Columns[0].Name = "SIRA";
            dataGridView2.Columns[1].Name = "ÜRÜN";
            dataGridView2.Columns[2].Name = "MİKTAR";
            dataGridView2.Columns[3].Name = "BİRİM";
            dataGridView2.Columns[4].Name = "FİYAT";

            dataGridView2.Columns[0].Width = 30;
            dataGridView2.Columns[1].Width = 200;
            dataGridView2.Columns[2].Width = 60;
            dataGridView2.Columns[3].Width = 60;
            dataGridView2.Columns[4].Width = 60;

            /*** NORMAL BUTON EKLEME ***/
            DataGridViewButtonColumn addBtn = new DataGridViewButtonColumn();
            //Kolon Başlığı
            addBtn.HeaderText = "İŞLEM";
            // Butonun Text
            addBtn.Text = "EKLE";
            // Butonda Text Kullanılmasını aktifleştirme
            addBtn.UseColumnTextForButtonValue = true;
            // Buton çerçeve rengi
            addBtn.DefaultCellStyle.BackColor = Color.Blue;
            // Buton seçiliykenki çerçeve rengi
            addBtn.DefaultCellStyle.SelectionBackColor = Color.Red;
            // Butonun genişiliği
            addBtn.Width = 120;
            // DataGridView e ekleme
            dataGridView2.Columns.Add(addBtn);

            barkodsuz();

            kilo();
        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.RowCount > 0)
            {
                if (e.ColumnIndex == 5)
                {
                    int satir = dataGridView1.CurrentCell.RowIndex;
                    if (satir > -1)
                    {
                        DialogResult sor = new DialogResult();
                        sor = MessageBox.Show("[ " + dataGridView1.CurrentRow.Cells[1].Value.ToString() + " ] isimli ürün listeden kaldırılıcaktır! Onaylıyor musunuz?", "Ürün Kaldır !", MessageBoxButtons.YesNo);
                        if (sor == DialogResult.Yes)
                        {

                            string fiyat = dataGridView1.CurrentRow.Cells[4].Value.ToString();
                            char[] charsToTrim = { ' ', '₺' };
                            fiyat = fiyat.Trim(charsToTrim);

                            bakiye -= float.Parse(fiyat);
                            total.Text = bakiye.ToString() + " ₺";
                            orders--;
                            totalorder.Text = orders.ToString() + " Adet";

                            dataGridView1.Rows.RemoveAt(satir);
                            dataGridView1.Refresh();
                            if (dataGridView1.RowCount.ToString() == "0")
                            {
                                bakiye = 0;
                                sira = 0;
                                orders = 0;
                                total.Text = bakiye.ToString() + " ₺";
                            }

                        }
                        else { MessageBox.Show("Ürün kaldırma işlemi iptal edildi!"); }

                    }

                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            fiyatsor fiyatsor = new fiyatsor();
            fiyatsor.Show();
        }

        private void iptal_Click(object sender, EventArgs e)
        {
            DialogResult sor = new DialogResult();
            sor = MessageBox.Show("Tüm SEPET kaldırılıcaktır! Onaylıyor musunuz?", "Tüm SEPETİ Kaldır !", MessageBoxButtons.YesNo);
            if (sor == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                total.Text = "0,00 ₺";
                totalorder.Text = "0 Adet";
                txtodenen.Clear();
                paraustu.Text = "0,00 ₺";
                alinan = 0;
                sira = 0;
                bakiye = 0;
                orders = 0;

            }
            else { MessageBox.Show("Tüm Sepeti kaldırma işlemi iptal edildi!"); }

        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.RowCount > 0)
            {
                if (e.ColumnIndex == 5)
                {

                    dataGridView1.Rows.Add(sira, dataGridView2.CurrentRow.Cells[1].Value.ToString(),
                        dataGridView2.CurrentRow.Cells[2].Value.ToString(), dataGridView2.CurrentRow.Cells[3].Value.ToString(),
                        dataGridView2.CurrentRow.Cells[4].Value.ToString(), dataGridView2.CurrentRow.Cells[5].Value.ToString());

                    string fiyat = dataGridView2.CurrentRow.Cells[4].Value.ToString();
                    char[] charsToTrim = { ' ', '₺' };
                    fiyat = fiyat.Trim(charsToTrim);

                    bakiye += float.Parse(fiyat);
                    total.Text = bakiye.ToString() + " ₺";
                    orders++;
                    totalorder.Text = orders.ToString() + " Adet";

                    sira++;

                }
            }
        }

        private void onay_Click(object sender, EventArgs e)
        {
            DialogResult sor = new DialogResult();
            sor = MessageBox.Show("[ " + total.Text + " ] tutarındaki sepeti Onaylıyor musunuz?", "Sepet Onayla !", MessageBoxButtons.YesNo);
            if (sor == DialogResult.Yes)
            {
                ;
                baglantim.Open();
                DateTime dat = DateTime.Now.Date;
                string tarih = dat.ToString().TrimEnd('0', ':');

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string orderprice = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    orderprice = orderprice.TrimEnd(' ', '₺');
                    try
                    {
                        OleDbCommand saleSave = new OleDbCommand("INSERT INTO sales (ordername,total,price,saledate) VALUES('"
                        + dataGridView1.Rows[i].Cells[1].Value.ToString() + "','" + dataGridView1.Rows[i].Cells[2].Value.ToString() + "','"
                        + orderprice + "','" + tarih + "')", baglantim);
                        saleSave.ExecuteNonQuery();


                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }

                dataGridView1.Rows.Clear();
                total.Text = "0,00 ₺";
                totalorder.Text = "0 Adet";
                txtodenen.Clear();
                paraustu.Text = "0,00 ₺";
                alinan = 0;
                sira = 0;
                bakiye = 0;
                orders = 0;

                baglantim.Close();

            }
            else { MessageBox.Show("Sepet Onayı iptal edildi!"); }
        }

        private void dayclose_Click(object sender, EventArgs e)
        {
            DateTime time = DateTime.Now.Date;
            string tarih = time.ToString().TrimEnd('0', ':');

            baglantim.Open();
            OleDbCommand sorgu = new OleDbCommand("SELECT * FROM sales WHERE saledate='" + tarih + "'", baglantim);

            OleDbDataReader kayitokuma = sorgu.ExecuteReader();
            float totalsale = 0;
            while (kayitokuma.Read())
            {
                totalsale += float.Parse(kayitokuma.GetValue(3).ToString());
            }
            baglantim.Close();
            MessageBox.Show("Bugün yapılan toplam satış [ " + totalsale + " ₺] ");

        }

        private void button1_Click(object sender, EventArgs e)
        {
          /*  DialogResult sor = new DialogResult();
            sor = MessageBox.Show("Tüm SEPET İADE olarak alıncaktır! Onaylıyor musunuz?", "Tüm SEPETİ İADE AL !", MessageBoxButtons.YesNo);
            if (sor == DialogResult.Yes)
            {

                baglantim.Open();

                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    string orderprice = dataGridView1.Rows[i].Cells[4].Value.ToString();
                    orderprice = orderprice.TrimEnd(' ', '₺');
                    try
                    {

                        OleDbCommand iade = new OleDbCommand("DELETE FROM sales WHERE ordername='" + dataGridView1.Rows[i].Cells[1].Value.ToString() +
                            "' AND price='" + orderprice + "'", baglantim);
                        iade.ExecuteNonQuery();

                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }

                dataGridView1.Rows.Clear();
                total.Text = "0,00 ₺";
                totalorder.Text = "0 Adet";
                sira = 0;
                bakiye = 0;

                baglantim.Close();
                MessageBox.Show("Tüm Sepet Başarı ile İADE Alındı!");

            }
            else { MessageBox.Show("Tüm Sepeti İADE Alma işlemi iptal edildi!"); }*/
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
                    dataGridView1.Rows.Add(sira, kayitokuma.GetValue(1).ToString(), "1", kayitokuma.GetValue(3).ToString(), kayitokuma.GetValue(6).ToString() + " ₺");
                    bakiye += float.Parse(kayitokuma.GetValue(6).ToString());
                    orders++;
                    total.Text = bakiye.ToString() + " ₺";
                    totalorder.Text = orders.ToString() + " Adet";


                    baglantim.Close();
                    barcodeTxt.Clear();
                    sira++;
                }
                else
                {
                    barcodeTxt.Clear();
                    baglantim.Close();
                    MessageBox.Show("Böyle bir ürün bulunamadı", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void kiloBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kiloBox.SelectedItem.ToString() != "Seçiniz...")
            {
                baglantim.Open();
                OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE orderunit='Kilogram' AND ordername='" + kiloBox.SelectedItem.ToString() + "'", baglantim);

                OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                if (kayitokuma.Read())
                {
                    kgtl.Text = kayitokuma.GetValue(5).ToString();
                    kilotxt.Clear();
                    totallbl.Text = "0,00 ₺";
                }
                baglantim.Close();
                

            }
        }

        private void sepeteklebtn_Click(object sender, EventArgs e)
        {
            if (kiloBox.SelectedItem.ToString() != "Seçiniz..." && kilotxt.Text.ToString() != "0,00 ₺" && totallbl.Text != "0,00 ₺")
            {


                dataGridView1.Rows.Add(sira, kiloBox.SelectedItem.ToString(),
                kilotxt.Text.ToString() + " Gr.", "Kilogram", totallbl.Text + " ₺");
                bakiye += float.Parse(totallbl.Text.ToString());
                total.Text = bakiye.ToString() + " ₺";
                orders++;
                totalorder.Text = orders.ToString() + " Adet";

                sira++;

                kiloBox.SelectedIndex = 0;
                kgtl.Text = "0,00 ₺";
                kilotxt.Clear();
                totallbl.Text = "0,00 ₺";
            }
            else
            {
                MessageBox.Show("Gram ile satış hesbı hatalı! KONTROL EDİNİZ . . .", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                kilotxt.Clear();
                totallbl.Text = "0,00 ₺";
            }
        }


        private void calculating_Click(object sender, EventArgs e)
        {
            if (kiloBox.SelectedItem.ToString() != "Seçiniz..." && kilotxt.Text.ToString() != "0,00 ₺" && kilotxt.Text.ToString() != "")
            {
                float sonuc = (float.Parse(kgtl.Text.ToString()) / 1000) * float.Parse(kilotxt.Text.ToString());
                totallbl.Text = sonuc.ToString();
            }
            else
            {
                MessageBox.Show("Gram ile satış hesabı hatalı! KONTROL EDİNİZ . . .", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                kiloBox.SelectedIndex = 0;
                kgtl.Text = "0,00 ₺";
                kilotxt.Clear();
                totallbl.Text = "0,00 ₺";
            }
        }

        private void barkodsuzarabtn_Click(object sender, EventArgs e)
        {
            if (barkodsuzaratxtBox.Text!="")
            {
                int i = 0;
                try
                {
                    dataGridView2.Rows.Clear();
                    baglantim.Open();
                    OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE orderunit='Adet' AND ordername LIKE '%" + barkodsuzaratxtBox.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);

                    OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                    while (kayitokuma.Read())
                    {
                        dataGridView2.Rows.Add(i, kayitokuma.GetValue(1).ToString(),"1", kayitokuma.GetValue(2).ToString(),
                                kayitokuma.GetValue(5).ToString());
                        i++;

                    }
                    baglantim.Close();
                }
                catch (Exception hatamsj)
                {
                    MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
        }

        private void refreshbtn_Click(object sender, EventArgs e)
        {
            barkodsuzaratxtBox.Clear();
            dataGridView2.Rows.Clear();
            barkodsuz();
        }

        private void btn200_Click(object sender, EventArgs e)
        {
            alinan += 200;
            txtodenen.Text = alinan.ToString();
        }

        private void btnclearodenen_Click(object sender, EventArgs e)
        {
            txtodenen.Clear();
            paraustu.Text = "0,00 ₺";
            alinan = 0;
        }

        private void btn100_Click(object sender, EventArgs e)
        {
            alinan += 100;
            txtodenen.Text = alinan.ToString();
        }

        private void btn50_Click(object sender, EventArgs e)
        {
            alinan += 50;
            txtodenen.Text = alinan.ToString();
        }

        private void btn20_Click(object sender, EventArgs e)
        {
            alinan += 20;
            txtodenen.Text = alinan.ToString();
        }

        private void btn10_Click(object sender, EventArgs e)
        {
            alinan += 10;
            txtodenen.Text = alinan.ToString();
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            alinan += 5;
            txtodenen.Text = alinan.ToString();
        }

        private void txtodenen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string a = total.Text.ToString();
                a = a.TrimEnd(' ', '₺');
                float sonuc= float.Parse(txtodenen.Text.ToString()) - float.Parse(a);
                paraustu.Text = sonuc.ToString() + " ₺";
            }
            }

        private void barkodsuzaratxtBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter){
                if (barkodsuzaratxtBox.Text != "")
                {
                    int i = 0;
                    try
                    {
                        dataGridView2.Rows.Clear();
                        baglantim.Open();
                        OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE orderunit='Adet' AND ordername LIKE '%" + barkodsuzaratxtBox.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);

                        OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                        while (kayitokuma.Read())
                        {
                            dataGridView2.Rows.Add(i, kayitokuma.GetValue(1).ToString(), "1", kayitokuma.GetValue(2).ToString(),
                                    kayitokuma.GetValue(5).ToString());
                            i++;

                        }
                        baglantim.Close();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
            }
        }

        private void yenisepet_Click(object sender, EventArgs e)
        {
            userpanel userpanel = new userpanel();
            userpanel.StartPosition = FormStartPosition.CenterParent;
            userpanel.Show();
        }
    }
}

