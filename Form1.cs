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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Veri tabanı dosya yolu ve provider nesnesinin belirlenmesi
        OleDbConnection baglantim = new OleDbConnection("Provider=Microsoft.Ace.OleDb.12.0;Data Source=data/database.accdb");
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void login_Click(object sender, EventArgs e)
        {
            if (username.Text!="" && password.Text!="" )
            {
                baglantim.Open();
                OleDbCommand sorgula = new OleDbCommand("SELECT * FROM users WHERE username='" + username.Text + "'", baglantim);
                OleDbDataReader kayitli = sorgula.ExecuteReader();
                if (kayitli.Read())
                {
                    string veri = kayitli.GetValue(2).ToString();
                    string yetki = kayitli.GetValue(3).ToString();
                    if (password.Text==veri)
                    {
                        if (yetki=="user")
                        {
                            this.Hide();
                            userpanel userpanel = new userpanel();
                            userpanel.Show();
                        }
                        if (yetki=="admin")
                        {
                            this.Hide();
                            adminpanel adminpanel = new adminpanel();
                            adminpanel.Show();
                        }

                    }
                    else
                    {
                        baglantim.Close();
                        MessageBox.Show("Şifre Yanlış !");
                        username.Clear();
                        password.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Kullanıcı Bulunamadı.");
                }
  
                baglantim.Close();
            }
            else
            {
                MessageBox.Show("Boş Alan Bırakmıyınız!");
                username.Clear();
                password.Clear();
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
