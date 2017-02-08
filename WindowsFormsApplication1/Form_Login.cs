using System;
using System.Windows.Forms;
using System.Dynamic;

namespace DIAWebServerExample
{
    public partial class Form_Login : Form
    {
        public Form_Login()
        {
            InitializeComponent();

            // Sifre alaninda * karakteri cikmasi icin textBox ayarlamasi yapilir.
            txt_Sifre.PasswordChar = '*';

            txt_KullaniciKodu.Text = "";
            txt_Sifre.Text = "";
            txt_Sunucu.Text = Sabitler.sunucuAdresi;
        }

        private void btn_Baglan_Click(object sender, EventArgs e)
        {
            // Login requesti için gerekli objeyi oluşturuyoruz
            dynamic request = new ExpandoObject();
            request.login = new ExpandoObject();
            request.login.username = this.txt_KullaniciKodu.Text;
            request.login.password = this.txt_Sifre.Text;
            request.login.disconnect_same_user = "True";
            request.login.lang = "tr";
            Sabitler.sunucuAdresi = this.txt_Sunucu.Text;

            // sendMessageToServer fonksiyonu dinamik objenin json'a çevrilmesi ve
            // verilen ek adrese göre json'un server'a gönderilmesinden sorumludur.
            // Gelen cevap ExpandoObject classindandir.
            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);

            // sonucun null olması server ile iletişimde hata olduğu anlamına gelmektedir.
            if(response == null)
            {
                MessageBox.Show("Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
            }
            // Login basarili oldugunda yapilacak islemler.
            else if (response.code == "200")
            {
                Console.WriteLine("Login başarılı oldu. session_id: " + response.msg);
                
                // Session id kaydedilir. Daha sonraki server ile iletişimde
                // bu id gonderilecektir.
                Sabitler.session_id = response.msg;


                // Login sayfasi kapatilir
                this.Hide();

                // Cari Kart sayfasi acilir
                Form_CariKartListesi form_ck = new Form_CariKartListesi();
                form_ck.Show();
                
            }
            // Login basarisiz oldugunda yapilacak islemler.
            else
            {
                MessageBox.Show("Giriş yaparken bir hata oluştu. Code: " + response.code +" Message: " + response.msg);
            }
        }

        private void btn_Vazgec_Click(object sender, EventArgs e)
        {
            // Uygulamanın kapatılması için
            Environment.Exit(1);
        }

        private void Form_Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }
    }
}
