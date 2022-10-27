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
    public partial class adminpanel : Form
    {
        public adminpanel()
        {
            InitializeComponent();
        }
        //Veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=data/database.accdb");
        private void orderList()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter order_list = new OleDbDataAdapter
                    ("SELECT orderbarcode AS[BARKOD], ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM orders Order By ordername ASC", baglantim);
                DataSet memory = new DataSet();
                order_list.Fill(memory);
                dataGridView1.DataSource = memory.Tables[0];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }

        //barkodsuz ürün listele
        private void noidorderList()
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter order_list = new OleDbDataAdapter
                    ("SELECT ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM noidorders Order By ordername ASC", baglantim);
                DataSet memory = new DataSet();
                order_list.Fill(memory);
                dataGridView2.DataSource = memory.Tables[0];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }
        private void adminpanel_Load(object sender, EventArgs e)
        {
            orderList();
            
            birimBox.Items.Add("Seçiniz...");
            birimBox.Items.Add("Adet");
            birimBox.Items.Add("Gram");
            birimBox.Items.Add("Kilogram");
            birimBox.Items.Add("Litre");
            birimBox.Items.Add("Mililitre");
            birimBox.SelectedIndex = 0;

            yontemBox.Items.Add("Seçiniz...");
            yontemBox.Items.Add("Ürün Adı");
            yontemBox.Items.Add("Barkod No");
            yontemBox.SelectedIndex = 0;

            noidorderList();

            notunit.Items.Add("Seçiniz...");
            notunit.Items.Add("Adet");
            notunit.Items.Add("Gram");
            notunit.Items.Add("Kilogram");
            notunit.Items.Add("Litre");
            notunit.Items.Add("Mililitre");
            notunit.SelectedIndex = 0;
        }

        private void adminpanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void ekle_Click(object sender, EventArgs e)
        {
            bool kayitdurum = false;

            if (barcodeTxt.Text!="" && ordernameTxt.Text!="" && stokTxt.Text!="" && birimBox.SelectedItem.ToString() != "Seçiniz..." && alimTxt.Text!="" && satimTxt.Text!="")
            {
                // Barkod numarasını veritabanından sorguluyoruz
                baglantim.Open();
                OleDbCommand sorgu = new OleDbCommand("SELECT * FROM orders WHERE orderbarcode='" + barcodeTxt.Text + "'", baglantim);
                OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayitdurum = true;
                    baglantim.Close();
                    break;
                }

                if (kayitdurum==false)
                {
                    try
                    {
                      OleDbCommand orderSave = new OleDbCommand("INSERT INTO orders (ordername,orderbarcode,orderunit,orderstock,orderbuy,ordersale) VALUES('"
                      + ordernameTxt.Text + "','" + barcodeTxt.Text + "','" + birimBox.SelectedItem.ToString() + "','" + stokTxt.Text + "','" +
                      alimTxt.Text + "','" + satimTxt.Text + "')", baglantim);
                        orderSave.ExecuteNonQuery();
                        //MessageBox.Show("Ürün Başarı İle Kaydedildi !", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        baglantim.Close();
                        orderList();
                        birimBox.SelectedIndex = 0;
                        barcodeTxt.Clear();
                        ordernameTxt.Clear();
                        stokTxt.Clear();
                        alimTxt.Clear();
                        satimTxt.Clear();
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Daha Önceden Kayıtlı Barkod!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Boş alanları doldurunuz!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            barcodeTxt.Text=dataGridView1.SelectedCells[0].Value.ToString();
            ordernameTxt.Text= dataGridView1.SelectedCells[1].Value.ToString();
            birimBox.SelectedItem= dataGridView1.SelectedCells[2].Value.ToString();
            stokTxt.Text= dataGridView1.SelectedCells[3].Value.ToString();
            alimTxt.Text= dataGridView1.SelectedCells[4].Value.ToString();
            satimTxt.Text= dataGridView1.SelectedCells[5].Value.ToString();

        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            birimBox.SelectedIndex = 0;
            barcodeTxt.Clear();
            ordernameTxt.Clear();
            stokTxt.Clear();
            alimTxt.Clear();
            satimTxt.Clear();
        }

        private void sil_Click(object sender, EventArgs e)
        {
            if (barcodeTxt.Text == "")
            {
                MessageBox.Show("Ürün seçiniz!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                DialogResult sor = new DialogResult();
                sor = MessageBox.Show("[ "+barcodeTxt.Text+" ]  " + ordernameTxt.Text+" isimli ürün kalıcı olarak silinecektir! Onaylıyor musunuz?", "Ürün Sil !", MessageBoxButtons.YesNo);
                if (sor == DialogResult.Yes)
                {
                    baglantim.Open();
                    OleDbCommand orderdelete = new OleDbCommand("DELETE FROM orders WHERE orderbarcode='" + barcodeTxt.Text + "'", baglantim);
                    orderdelete.ExecuteNonQuery();
                    MessageBox.Show(barcodeTxt.Text + " adlı Ürün kaydı silindi!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();

                    orderList();
                    birimBox.SelectedIndex = 0;
                    barcodeTxt.Clear();
                    ordernameTxt.Clear();
                    stokTxt.Clear();
                    alimTxt.Clear();
                    satimTxt.Clear();


                }
                else { MessageBox.Show("Ürün silme işlemi iptal edildi!"); }
            }
        }

        private void guncelle_Click(object sender, EventArgs e)
        {
            if (barcodeTxt.Text != "" && ordernameTxt.Text != "" && stokTxt.Text != "" && birimBox.SelectedItem.ToString() != "Seçiniz..." && alimTxt.Text != "" && satimTxt.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand urunguncelle = new OleDbCommand("UPDATE  orders SET ordername='" + ordernameTxt.Text + "',orderbarcode='" + barcodeTxt.Text + "',orderunit='" + birimBox.SelectedItem.ToString() +
                                    "',orderstock='" + stokTxt.Text + "',orderbuy='" + alimTxt.Text + "',ordersale='" + satimTxt.Text + "' WHERE orderbarcode='" + barcodeTxt.Text + "'", baglantim);
                    urunguncelle.ExecuteNonQuery();
                    baglantim.Close();
                    orderList();
                    birimBox.SelectedIndex = 0;
                    barcodeTxt.Clear();
                    ordernameTxt.Clear();
                    stokTxt.Clear();
                    alimTxt.Clear();
                    satimTxt.Clear();
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
            else
            {
                MessageBox.Show("Boş alanlar var!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bul_Click(object sender, EventArgs e)
        {
            string yontem = yontemBox.SelectedItem.ToString();
            if (yontem!= "Seçiniz...")
            {
                if (yontem== "Barkod No")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbDataAdapter order_list = new OleDbDataAdapter
                            ("SELECT orderbarcode AS[BARKOD], ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM orders WHERE orderbarcode LIKE '%"+araTxt.Text.ToUpper()+"%' ORDER BY ordername ASC", baglantim);
                        DataSet memory = new DataSet();
                        order_list.Fill(memory);
                        dataGridView1.DataSource = memory.Tables[0];
                        baglantim.Close();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                if (yontem == "Ürün Adı")
                {
                    try
                    {
                        baglantim.Open();
                        OleDbDataAdapter order_list = new OleDbDataAdapter
                            ("SELECT orderbarcode AS[BARKOD], ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM orders WHERE ordername LIKE '%" + araTxt.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);
                        DataSet memory = new DataSet();
                        order_list.Fill(memory);
                        dataGridView1.DataSource = memory.Tables[0];
                        baglantim.Close();
                    }
                    catch (Exception hatamsj)
                    {
                        MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
            }
            else
            {
                MessageBox.Show("Arama Yöntemi Seçiniz !", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            orderList();
        }

        private void notclear_Click(object sender, EventArgs e)
        {
            notunit.SelectedIndex = 0;
            notordername.Clear();
            notstock.Clear();
            notbuy.Clear();
            notsale.Clear();
        }

        private void notrefresh_Click(object sender, EventArgs e)
        {
            noidorderList();
        }

        private void notadd_Click(object sender, EventArgs e)
        {
            bool kayit = false;

            if (notordername.Text != "" && notstock.Text != "" && notunit.SelectedItem.ToString() != "Seçiniz..." && notbuy.Text != "" && notsale.Text != "")
            {
                // Barkod numarasını veritabanından sorguluyoruz
                baglantim.Open();
                OleDbCommand sorgu = new OleDbCommand("SELECT * FROM noidorders WHERE ordername='" + notordername.Text + "'", baglantim);
                OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                while (kayitokuma.Read())
                {
                    kayit = true;
                    baglantim.Close();
                    break;
                }

                if (kayit == false)
                {
                    try
                    {
                        OleDbCommand orderSave = new OleDbCommand("INSERT INTO noidorders (ordername,orderunit,orderstock,orderbuy,ordersale) VALUES('"
                        + notordername.Text + "','" + notunit.SelectedItem.ToString()+ "','" + notstock.Text + "','" + notbuy.Text + "','" +
                        notsale.Text + "')", baglantim);
                        orderSave.ExecuteNonQuery();

                        baglantim.Close();

                        noidorderList();
                        notunit.SelectedIndex = 0;
                        notordername.Clear();
                        notstock.Clear();
                        notbuy.Clear();
                        notsale.Clear(); 
                    }
                    catch (Exception msg)
                    {
                        MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        baglantim.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Daha Önceden Kayıtlı Ürün!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Boş alanları doldurunuz!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void notupdate_Click(object sender, EventArgs e)
        {
            if (notordername.Text != "" && notstock.Text != "" && notunit.SelectedItem.ToString() != "Seçiniz..." && notbuy.Text != "" && notsale.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand urunguncelle = new OleDbCommand("UPDATE  noidorders SET ordername='" + notordername.Text + "',orderunit='" + notunit.SelectedItem.ToString() +
                                    "',orderstock='" + notstock.Text + "',orderbuy='" + notbuy.Text + "',ordersale='" + notsale.Text + "' WHERE ordername='" + notordername.Text + "'", baglantim);
                    urunguncelle.ExecuteNonQuery();
                    baglantim.Close();

                    noidorderList();
                    notunit.SelectedIndex = 0;
                    notordername.Clear();
                    notstock.Clear();
                    notbuy.Clear();
                    notsale.Clear();
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                }
            }
            else
            {
                MessageBox.Show("Boş alanlar var!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            notordername.Text = dataGridView2.SelectedCells[0].Value.ToString();
            notunit.SelectedItem = dataGridView2.SelectedCells[1].Value.ToString();
            notstock.Text = dataGridView2.SelectedCells[2].Value.ToString();
            notbuy.Text = dataGridView2.SelectedCells[3].Value.ToString();
            notsale.Text = dataGridView2.SelectedCells[4].Value.ToString();
        }

        private void notdelete_Click(object sender, EventArgs e)
        {
            if (notordername.Text == "")
            {
                MessageBox.Show("Ürün seçiniz!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            else
            {
                DialogResult sor = new DialogResult();
                sor = MessageBox.Show("[ " + notordername.Text + " ]   isimli ürün kalıcı olarak silinecektir! Onaylıyor musunuz?", "Ürün Sil !", MessageBoxButtons.YesNo);
                if (sor == DialogResult.Yes)
                {
                    baglantim.Open();
                    OleDbCommand sirketsil = new OleDbCommand("DELETE FROM noidorders WHERE ordername='" + notordername.Text + "'", baglantim);
                    sirketsil.ExecuteNonQuery();
                    MessageBox.Show(notordername.Text + " adlı Ürün kaydı silindi!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    baglantim.Close();

                    noidorderList();
                    notunit.SelectedIndex = 0;
                    notordername.Clear();
                    notstock.Clear();
                    notbuy.Clear();
                    notsale.Clear();
                }
                else { MessageBox.Show("Ürün silme işlemi iptal edildi!"); }
            }
        }

        private void notsearch_Click(object sender, EventArgs e)
        {
            try
            {
                baglantim.Open();
                OleDbDataAdapter order_list = new OleDbDataAdapter
                    ("SELECT ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM noidorders WHERE ordername LIKE '%" + notsearchbox.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);
                DataSet memory = new DataSet();
                order_list.Fill(memory);
                dataGridView2.DataSource = memory.Tables[0];
                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }

        private void sec_Click(object sender, EventArgs e)
        {
            try
            {
                baglantim.Open();
                string tarih = dateTimePicker1.Text;
                if (tarih[1] == '.')
                {
                    tarih = "0" + tarih;
                }

                OleDbDataAdapter order_list = new OleDbDataAdapter
                    ("SELECT ordername AS[ÜRÜN],total AS[ADET],price AS[FİYAT],saledate AS[TARİH] FROM sales WHERE saledate='"+ tarih + "'", baglantim);
                DataSet memory = new DataSet();
                order_list.Fill(memory);
                dataGridView3.DataSource = memory.Tables[0];

                OleDbCommand sorgu = new OleDbCommand("SELECT * FROM sales WHERE saledate='" + tarih + "'", baglantim);
                OleDbDataReader gunokuma = sorgu.ExecuteReader();
                float daysale = 0;
                while (gunokuma.Read())
                {
                    daysale += float.Parse(gunokuma.GetValue(3).ToString());
                }
                daytotal.Text = daysale.ToString() + " ₺";




                baglantim.Close();
            }
            catch (Exception hatamsj)
            {
                MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
            }
        }

        private void addbarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand sorgu = new OleDbCommand("SELECT * FROM orders WHERE orderbarcode='" + addbarcode.Text + "'", baglantim);
                    OleDbDataReader kayitokuma = sorgu.ExecuteReader();
                    if (kayitokuma.Read())
                    {
                        ordername.Text = kayitokuma.GetValue(1).ToString();
                        oldstock.Text = kayitokuma.GetValue(4).ToString();
                        oldprice.Text = kayitokuma.GetValue(5).ToString();
                        saleprice.Text = kayitokuma.GetValue(6).ToString();

                        baglantim.Close();
                    }
                    else
                    {
                        baglantim.Close();
                        MessageBox.Show("Böyle bir ürün bulunamadı", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception hata)
                {
                    baglantim.Close();
                    MessageBox.Show(hata.ToString(), "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ordername.Text = "";
            oldstock.Text = "";
            oldprice.Text = "";
            saleprice.Text = "";
            addbarcode.Clear();
            givestock.Clear();
            giveprice.Clear();
            newstock.Text = "";
            newgiveprice.Text = "";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (givestock.Text!="" && giveprice.Text!="")
            {

            
            float a = float.Parse(oldstock.Text) * float.Parse(oldprice.Text); // Eski toplam fiyat
            float b = float.Parse(givestock.Text) * float.Parse(giveprice.Text); // Yeni toplam fiyat

            int stok = Int32.Parse(oldstock.Text) + Int32.Parse(givestock.Text); // Toplam stok

            double c = (a + b) / stok; // Eski ve yeni ortalama birim fiyat
            c = Math.Round(c, 2);

            newstock.Text = stok.ToString();
            newgiveprice.Text = c.ToString();
            }
            else
            {
                MessageBox.Show("Boş alanlar var!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (newstock.Text != "" && newgiveprice.Text != "" && addbarcode.Text != "")
            {
                try
                {
                    baglantim.Open();
                    OleDbCommand stokguncelle = new OleDbCommand("UPDATE  orders SET orderstock='" + newstock.Text + "',orderbuy='" + newgiveprice.Text + "' WHERE orderbarcode='" + addbarcode.Text + "'", baglantim);
                    stokguncelle.ExecuteNonQuery();
                    baglantim.Close();
                    ordername.Text = "";
                    oldstock.Text = "";
                    oldprice.Text = "";
                    saleprice.Text = "";
                    addbarcode.Clear();
                    givestock.Clear();
                    giveprice.Clear();
                    newstock.Text = "";
                    newgiveprice.Text = "";

                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    baglantim.Close();
                    ordername.Text = "";
                    oldstock.Text = "";
                    oldprice.Text = "";
                    saleprice.Text = "";
                    addbarcode.Clear();
                    givestock.Clear();
                    giveprice.Clear();
                    newstock.Text = "";
                    newgiveprice.Text = "";
                }
            }
            else
            {
                MessageBox.Show("Boş alanlar var!", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                baglantim.Close();
                ordername.Text = "";
                oldstock.Text = "";
                oldprice.Text = "";
                saleprice.Text = "";
                addbarcode.Clear();
                givestock.Clear();
                giveprice.Clear();
                newstock.Text = "";
                newgiveprice.Text = "";
            }


        }

        private void araTxt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string yontem = yontemBox.SelectedItem.ToString();
                if (yontem != "Seçiniz...")
                {
                    if (yontem == "Barkod No")
                    {
                        try
                        {
                            baglantim.Open();
                            OleDbDataAdapter order_list = new OleDbDataAdapter
                                ("SELECT orderbarcode AS[BARKOD], ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM orders WHERE orderbarcode LIKE '%" + araTxt.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);
                            DataSet memory = new DataSet();
                            order_list.Fill(memory);
                            dataGridView1.DataSource = memory.Tables[0];
                            baglantim.Close();
                        }
                        catch (Exception hatamsj)
                        {
                            MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            baglantim.Close();
                        }
                    }
                    if (yontem == "Ürün Adı")
                    {
                        try
                        {
                            baglantim.Open();
                            OleDbDataAdapter order_list = new OleDbDataAdapter
                                ("SELECT orderbarcode AS[BARKOD], ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM orders WHERE ordername LIKE '%" + araTxt.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);
                            DataSet memory = new DataSet();
                            order_list.Fill(memory);
                            dataGridView1.DataSource = memory.Tables[0];
                            baglantim.Close();
                        }
                        catch (Exception hatamsj)
                        {
                            MessageBox.Show(hatamsj.Message, "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            baglantim.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Arama Yöntemi Seçiniz !", "Akçin Market", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }



        }

        private void notsearchbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    baglantim.Open();
                    OleDbDataAdapter order_list = new OleDbDataAdapter
                        ("SELECT ordername AS[ÜRÜN],orderunit AS[BİRİM],orderstock AS[STOK],orderbuy AS[ALIŞ FİYATI],ordersale AS[SATIŞ FİYATI] FROM noidorders WHERE ordername LIKE '%" + notsearchbox.Text.ToUpper() + "%' ORDER BY ordername ASC", baglantim);
                    DataSet memory = new DataSet();
                    order_list.Fill(memory);
                    dataGridView2.DataSource = memory.Tables[0];
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
}
