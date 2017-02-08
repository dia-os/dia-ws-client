using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Forms;

namespace DIAWebServerExample
{
    public partial class Form_CariKart : Form
    {
        
        private Form_CariKartListesi carikartlistesi;   // Bu ekranı çağıran ekran. 
                                                        // Ekran kapıtıldıktan sonra bu ekrana geri dönülecektir.

        private dynamic sehirler = null;        // Serverdan çekilen sehirler bu değişkende tutulur
        private dynamic vergiDaireleri = null;  // Serverdan çekilen vergi daireleri bu değişkende tutulur
        private dynamic carikart = null;        // Serverdan çekilen carikart bu değişkende tutulur
        private bool isYeniCariKart = true;     // Ekran değiştirme modunda mı açılmış yoksa yeni ekleme modunda mı?

        private bool islemBitti = false;

        public Form_CariKart(Form_CariKartListesi carikartlistesi, string _key_scf_carikart)
        {
            this.carikartlistesi = carikartlistesi;

            InitializeComponent();

            // Combolar hazırlanır
            initiliazeTipi();
            initiliazeTuru();
            initiliazeSubeler();
            initiliazeSehirler();
            initiliazeVergiDairesi();

            // Egerki bir _key bilgisi gonderilmiş ise, bu guncelle(değiştir) işlemi demektir.
            // Ekrandaki bilgileri güncellenecek carinin bilgileri ile dolduruyoruz
            if(_key_scf_carikart != null && _key_scf_carikart != "")
            {
                this.carikart = cari_kart_getir(_key_scf_carikart);

                // Serverdan çekilen bilgilere göre ekrandaki bilgiler doldurulur
                ekrandakiBilgileriDoldur(this.carikart);
                this.isYeniCariKart = false;
            }
            else
            {
                // Ekran yeni ekleme modunda. Otomatik olarak yeni kod istiyoruz
                btn_KodGetir_Click(null, null);
            }
        }

        /// <summary>
        /// Server ile iletişim kurarak _key bilgisi verilen carikart'ı döndürür.
        /// </summary>
        /// <param name="_key_scf_carikart"></param>
        /// <returns></returns>
        private dynamic cari_kart_getir(string _key_scf_carikart)
        {
            dynamic request = new ExpandoObject();
            request.scf_carikart_getir = new ExpandoObject();
            request.scf_carikart_getir.session_id = Sabitler.session_id;
            request.scf_carikart_getir.firma_kodu = Sabitler.seciliFirmaKodu;
            request.scf_carikart_getir.donem_kodu = Sabitler.seciliDonemKodu;
            request.scf_carikart_getir.key = _key_scf_carikart;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.scfEk);
            // Server ile iletişim başarısız
            if (response == null)
            {
                return null;
            }
            // Sesrver ile iletişim başarılı, işlem başarılı
            else if(response.code == "200")
            {
                return response.result;
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            // İletişim başarılı, işlem başarısız
            else
            {
                MessageBox.Show("Cari getirilirken bir hata oluştu. Code: " + response.code + " Message: " + response.msg);
            }
            return null;
        }

        /// <summary>
        /// Serverdan çekilen carikart objesindeki bilgilere göre ekrandaki gerekli alanları doldurur
        /// </summary>
        /// <param name="carikart"></param>
        public void ekrandakiBilgileriDoldur(dynamic carikart)
        {
            txt_Kodu.Text = carikart.carikartkodu;
            txt_Unvan.Text = carikart.unvan;
            txt_TcNo.Text = carikart.tckimlikno;
            txt_Eposta.Text = carikart.eposta;
            txt_VergiNo.Text = carikart.verginumarasi;
            txt_KisaAciklama.Text = carikart.kisaaciklama;

            if (carikart.carikayitturu == "SHS")
                cmb_Tur.Text = "Şahıs";
            else{
                cmb_Tur.Text = "Kuruluş";
            }

            cmb_Tipi.Text = carikart.carikarttipi;
            foreach(dynamic cari_kayitli_adres in carikart.m_adresler)
            {
                if(cari_kayitli_adres.anaadres == "1")
                {
                    txt_Adres.Text = cari_kayitli_adres.adres1;
                    txt_Telefon.Text = cari_kayitli_adres.telefon1;
                    txt_PostaKodu.Text = cari_kayitli_adres.postakodu;
                    if (cari_kayitli_adres._key_sis_sehirler != null && cari_kayitli_adres._key_sis_sehirler.GetType() == typeof(ExpandoObject))
                        cmb_Sehirler.Text = sehirAdGetir( "" + cari_kayitli_adres._key_sis_sehirler._key);

                    break;
                }
            }

            // _key_sis_sube bilgisi var ve bir ExpandoObject yapısında ise
            // Bu şube geçerlidir. Şube bilgilerini dolduralım
            if (carikart._key_sis_sube != null && carikart._key_sis_sube.GetType() == typeof(ExpandoObject))
                cmb_Sube.Text = subeAdiGetir(Sabitler.seciliFirmaKodu, "" + carikart._key_sis_sube._key);

        }

        /// <summary>
        /// Ekrandaki bilgilere göre carikart oluşturur.
        /// </summary>
        /// <returns></returns>
        public dynamic ekrandakiBilgilerdenCariİstekOlustur()
        {
            // Yeni bir carikart objesi yaratıyoruz
            dynamic carikart = new ExpandoObject();

            carikart.carikartkodu = txt_Kodu.Text;
            carikart.unvan = txt_Unvan.Text;
            carikart.tckimlikno = txt_TcNo.Text;
            carikart.eposta = txt_Eposta.Text;
            carikart.verginumarasi = txt_VergiNo.Text;
            carikart.kisaaciklama = txt_KisaAciklama.Text;

            // Comboyu ayarlarken "Şahıs" ve "Kuruluş" şeklinde ayarlanmıştı
            if (cmb_Tur.Text == "Şahıs")
                carikart.carikayitturu = "SHS";
            else
                carikart.carikayitturu = "KRLS";

            // combo ayarlanırken, "AL", "AS", "ST" şeklinde ayarlanmıştı 
            carikart.carikarttipi = cmb_Tipi.Text;

            carikart._key_sis_sube = subeKeyGetir(Sabitler.seciliFirmaKodu, cmb_Sube.Text);
            carikart._key_sis_vergidairesi = vergiDairesiKeyGetir(cmb_VergiDairesi.Text);

            carikart.m_adresler = new ArrayList();

            dynamic adres = new ExpandoObject();
            // Ana adres oldugunu belirtmek için "1" yapıyoruz.
            adres.anaadres = "1";
            adres.adres1 = txt_Adres.Text;
            adres.telefon1 = txt_Telefon.Text;
            adres.postakodu = txt_PostaKodu.Text;
            adres._key_sis_sehirler = sehirKeyGetir(cmb_Sehirler.Text);

            // Ana adresin _key değerini buluyoruz. 
            // Ekrandaki bilgiler ile bu ana adresi değiştireceğiz
            if (this.carikart != null)
            {
                foreach(dynamic cari_kayitli_adres in this.carikart.m_adresler)
                {
                    if(cari_kayitli_adres.anaadres == "1")
                    {
                        adres._key = cari_kayitli_adres._key;
                        break;
                    }
                }
            }

            carikart.m_adresler.Add(adres);

            return carikart;
        }

        private long sehirKeyGetir(string sehirText)
        {
            // ismi bilinen bir sehir için _key bulunur
            // Ekleme sırasında sehir adı degil, _key kullanılır
            foreach (dynamic sehir in sehirler)
            {
                if (sehir.sehiradi == sehirText)
                {
                    return long.Parse(sehir._key);
                }
            }

            return 0;
        }

        private string sehirAdGetir(string key)
        {
            // key'i bilinen bir sehir için sehiradı bulunur
            foreach (dynamic sehir in sehirler)
            {
                if ((string)sehir._key == key)
                {
                    return sehir.sehiradi;
                }
            }

            return "";
        }

        private long subeKeyGetir(long firmaKodu, string subeAdi)
        {
            // firma_sube_donem listesi, login olduktan sonra Sabitler içerisinde kaydetilmişti.
            foreach (dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                // İlgili firma bulundu.
                if (firma_sube_donem.firmakodu == firmaKodu)
                {
                    // Firma içerisindeki şubelerde gezilir ve key bilgisi bulunur
                    if (Sabitler.HasAttr(firma_sube_donem, "subeler"))
                    {
                        foreach(dynamic sube in firma_sube_donem.subeler)
                        {
                            if (sube.subeadi == subeAdi)
                            {
                                return sube._key;
                            }
                        }
                    }
                    break;
                }
            }

            return 0;
        }

        private string subeAdiGetir(long firmaKodu, string key)
        {
            // firma_sube_donem listesi, login olduktan sonra Sabitler içerisinde set edilmişti.
            foreach (dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                // İlgili firma bulundu.
                if (firma_sube_donem.firmakodu == firmaKodu)
                {
                    // Firma içerisindeki şubelerde geizilir ve şubenin adı bulunur
                    if (Sabitler.HasAttr(firma_sube_donem, "subeler"))
                    {
                        foreach (dynamic sube in firma_sube_donem.subeler)
                        {
                            if ( ("" + sube._key) == key)
                            {
                                return sube.subeadi;
                            }
                        }
                    }
                    break;
                }
            }

            return "";
        }

        private long vergiDairesiKeyGetir(string vergiDairesiText)
        {
            // Ekranda vergi dairesini gosterdigimiz formata göre vergiDairesiText'i ayrıştılırı
            // Format için vergiDairesiTextGetir fonksiyonuna bakılabilir.
            // İlgili alanların doğruluguna göre key döndürülür

            // vergiDairesiText  ->  vergiDairesi["daire"] + "---" + vergiDairesi["kod"]
            string[] arr = vergiDairesiText.Split(new[] { "   " }, StringSplitOptions.None);

            foreach (dynamic vergiDairesi in vergiDaireleri)
            {
                if (vergiDairesi.daire == arr[0] && vergiDairesi.kod == arr[1])
                    return vergiDairesi._key;
            }

            return 0;
        }

        private string vergiDairesiTextGetir(long key)
        {
            // Key'i bilinen bir vergi dairsei ekranda gözükecek formatta ismi döndürülür.
            // Daha sonra ayrıştırma yapılırken bu format dikkate alınır (text'i bilinen bir vergi dairesinden key bulmak için)

            // vergiDairesiText -> vergiDairesi["daire"] + "---" + vergiDairesi["kod"]

            foreach (dynamic vergiDairesi in vergiDaireleri)
            {
                if(vergiDairesi._key == key)
                {
                    return vergiDairesi.daire + "---" + vergiDairesi.kod;
                }
            }

            return "";
        }

        /// <summary>
        /// Server ile iletişim kurarak vergi dairelerini getirir.
        /// </summary>
        public void initiliazeVergiDairesi()
        {
            dynamic request = new ExpandoObject();
            request.sis_vergidairesi_listele = new ExpandoObject();
            request.sis_vergidairesi_listele.firma_kodu = Sabitler.seciliFirmaKodu;
            request.sis_vergidairesi_listele.donem_kodu = Sabitler.seciliDonemKodu;
            request.sis_vergidairesi_listele.session_id = Sabitler.session_id;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);

            if(response == null)
            {
                MessageBox.Show("Vergi daireleri getirilirken, bağlantı hatası oluştu.");
                return;
            }
            else if (response.code == "200")
            {
                this.vergiDaireleri = response.result;

                List<string> vergiDaireleriCombo = new List<string>();

                foreach (dynamic vergiDairesi in response.result)
                {
                    vergiDaireleriCombo.Add(vergiDairesi.daire + "---" + vergiDairesi.kod);
                }

                cmb_VergiDairesi.Items.Clear();
                cmb_VergiDairesi.Items.AddRange(vergiDaireleriCombo.ToArray());
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            else
            {
                MessageBox.Show("Vergi daireleri getirilirken bir hata oluştu. Code: " + response.code + " Message: " + response.msg);
            }
        }

        /// <summary>
        /// Server ile iletişim kurarak şehirleri getirir.
        /// </summary>
        public void initiliazeSehirler()
        {
            dynamic request = new ExpandoObject();
            request.sis_sehirler_listele = new ExpandoObject();
            request.sis_sehirler_listele.firma_kodu = Sabitler.seciliFirmaKodu;
            request.sis_sehirler_listele.donem_kodu = Sabitler.seciliDonemKodu;
            request.sis_sehirler_listele.session_id = Sabitler.session_id;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);

            if (response == null)
            {
                MessageBox.Show("Sehirler getirilirken, bağlantı hatası oluştu.");
                return;
            }
            else if (response.code == "200")
            {

                this.sehirler = response.result;

                List<string> sehirAdlariCombo = new List<string>();

                foreach (dynamic sehir in response.result)
                {
                    sehirAdlariCombo.Add(sehir.sehiradi);
                }

                cmb_Sehirler.Items.Clear();
                cmb_Sehirler.Items.AddRange(sehirAdlariCombo.ToArray());
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            else
            {
                MessageBox.Show("Sehir adları getirilirken bir hata oluştu. Code: " + response.code + " Message: " + response.msg);
            }
        }

        public void initiliazeTipi()
        {
            cmb_Tipi.Items.Clear();
            cmb_Tipi.Items.AddRange(new String[] { "AL", "AS", "ST" });
        }

        public void initiliazeTuru()
        {
            cmb_Tur.Items.Clear();
            cmb_Tur.Items.AddRange(new String[] { "Şahıs", "Kuruluş" });
        }


        /// <summary>
        /// Seçili firma bilgisine göre firmanın şubelerini listeler
        /// </summary>
        public void initiliazeSubeler()
        {
            List<string> subeAdlari = new List<string>();

            foreach(dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                // İlgili firma bulundu.
                if (firma_sube_donem.firmakodu == Sabitler.seciliFirmaKodu)
                {

                    if (Sabitler.HasAttr(firma_sube_donem, "subeler"))
                    {
                        foreach(dynamic sube in firma_sube_donem.subeler)
                        {
                            subeAdlari.Add(sube.subeadi);
                        }
                        break;
                    }
                }
            }

            cmb_Sube.Items.Clear();
            cmb_Sube.Items.AddRange(subeAdlari.ToArray());
        }

        /// <summary>
        /// Server ile iletişim kurarak carikart için yeni carikodu üretir.
        /// </summary>
        private void btn_KodGetir_Click(object sender, EventArgs e)
        {
            dynamic request = new ExpandoObject();
            request.sis_numara_getir = new ExpandoObject();
            request.sis_numara_getir.session_id = Sabitler.session_id;
            request.sis_numara_getir.firma_kodu = Sabitler.seciliFirmaKodu;
            request.sis_numara_getir.donem_kodu = Sabitler.seciliDonemKodu;
            request.sis_numara_getir.table_name = "scf_carikart";
            request.sis_numara_getir.column_name = "carikartkodu";
            request.sis_numara_getir.template_type = "CARIKART_KODU";
            request.sis_numara_getir.statu = 1;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);
            if(response == null)
            {
                MessageBox.Show("Cariler getirilirken, bağlantı hatası oluştu.");
                return;
            }
            else if (response.code == "200")
            {
                txt_Kodu.Text = response.result.kod;
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            else
            {
                MessageBox.Show("Yeni kod getirilirken bir hata oluştu. Code: " + response.code + " Message: " + response.msg);
                return;
            }
        }

        private void btn_Vazgec_Click(object sender, EventArgs e)
        {
            // Form kapatılmadan önce carikartlistesi gösterilir.
            this.carikartlistesi.Show();
            this.islemBitti = true;
            this.Close();
        }

        private void btn_Kaydet_Click(object sender, EventArgs e)
        {
            if (txt_Kodu.Text == null)
            {
                MessageBox.Show("Bilgiler eksik. Carikodu");
                return;
            }
            if (cmb_Tur.Text == null)
            {
                MessageBox.Show("Bilgiler eksik. Tür");
                return;
            }

            dynamic result = null;
            // Yeni carikart ekleme modunda isek
            if (this.isYeniCariKart )
            {
                result = yeniCarikartEkle();
            }
            // Carikart güncelleme modunda isek
            else
            {
                result = carikartGuncelle();
            }

            this.islemBitti = true;
            // Server ile iletişimde hata oluştu
            if (result == null)
            {
                MessageBox.Show("Carikart eklerken server ile bağlantı hatası oluştu.");
            }
            // İşlem başarılı
            else if (result.code == "200")
            {
                MessageBox.Show("Carikart ekleme/güncelleme başarılı.");

                // Carikartlar tekrardan listelenir, ekleme formu kapatılır
                carikartlistesi.btn_Getir_Click(null, null);

                // Form kapatılmadan önce carikartlistesi gösterilir.
                carikartlistesi.Show();
                this.Close();
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (result.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + result.code + " Message: " + result.msg);
            }
            // İşlem başarısız
            else
            {
                MessageBox.Show("Carikart eklerken bir hata oluştu. Code: " + result.code + " Message: " + result.msg);
            }
        }


        /// <summary>
        /// Server ile iletişim kurarak ekrandaki bilgilere göre carikart'ı günceller.
        /// </summary>
        private dynamic carikartGuncelle()
        {
            // Carikart güncellemesi eklemesi yapılır
            dynamic request = new ExpandoObject();
            request.scf_carikart_guncelle = new ExpandoObject();
            request.scf_carikart_guncelle.session_id = Sabitler.session_id;
            request.scf_carikart_guncelle.firma_kodu = Sabitler.seciliFirmaKodu;
            request.scf_carikart_guncelle.donem_kodu = Sabitler.seciliDonemKodu;
            request.scf_carikart_guncelle.kart = ekrandakiBilgilerdenCariİstekOlustur();
            request.scf_carikart_guncelle.kart._key = this.carikart._key;

            // Kart içerisinde olmazsa hata aldığımız alanlar.
            // Bu alandaki bilgileri hata durumuna göre default değerler verebiliriz.

            if (this.carikart._key_sis_ozelkod2 != null && this.carikart._key_sis_ozelkod2.GetType() == typeof(ExpandoObject))
                // Bu alanın daha iyi anlaşılabilmesi için carikart getir servis fonksiyonun döndüğü değerlere bakılmalıdır.
                request.scf_carikart_guncelle.kart._key_sis_ozelkod2 = this.carikart._key_sis_ozelkod2._key;
            else
                request.scf_carikart_guncelle.kart._key_sis_ozelkod2 = 0;

            request.scf_carikart_guncelle.kart.efaturasenaryosu = this.carikart.efaturasenaryosu;

            string jsonWS = JsonConvert.SerializeObject(request);

            // Oluşan json örenğini test etmek için alttaki satırın comemnti kaldırılabilir
            // Console.WriteLine("json: " + jsonWS);
            
            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.scfEk);
            return response;
        }

        /// <summary>
        /// Server ile iletişim kurarak ekrandaki bilgilere göre yeni carikart ekler.
        /// </summary>
        private dynamic yeniCarikartEkle()
        {
            
            // Carikart eklemesi yapılır
            dynamic request = new ExpandoObject();
            request.scf_carikart_ekle = new ExpandoObject();
            request.scf_carikart_ekle.session_id = Sabitler.session_id;
            request.scf_carikart_ekle.firma_kodu = Sabitler.seciliFirmaKodu;
            request.scf_carikart_ekle.donem_kodu = Sabitler.seciliDonemKodu;
            request.scf_carikart_ekle.kart = ekrandakiBilgilerdenCariİstekOlustur();

            // Kart içerisinde olmazsa hata aldığımız alanlar.
            // Bu alandaki bilgileri hata durumuna göre default değerler verebiliriz.
            request.scf_carikart_ekle.kart._key_sis_ozelkod2 = 0;
            request.scf_carikart_ekle.kart.efaturasenaryosu = "";

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.scfEk);
            return response;
        }

        private void Form_CariKart_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.islemBitti == false)
                btn_Vazgec_Click(null, null);
        }
    }
}
