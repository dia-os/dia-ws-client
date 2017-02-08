using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Forms;

namespace DIAWebServerExample
{
    public partial class Form_CariKartListesi : Form
    {
        
        // Bu şemaya göre gridview kolonları oluşturulacaktır.
        private struct GridHeaderItem
        {
            // servis aracılığıyla çekilen isimle "name" alanı aynı olmalıdır.
            public string name;
            public string headerText; // kolonun görünen ismi
            public bool visible; // kolonun görünüp görenmeyeceği
            public int columnWidth; // kolonun genişliği
        }

        // kolon listesi, initiliazeGridColumns() fonksiyonu içerisinde
        // kolonlar oluşturulacaktır.
        private List<GridHeaderItem> gridHeader = new List<GridHeaderItem>();

        public Form_CariKartListesi()
        {

            InitializeComponent();

            // Firma listesi serverden çekilip combo doldurulur
            initilizeFirmaCombo();

            // Gridview içerisindeki kolonlar ayarlanır
            initiliazeGridColumns();
        }

        private void initiliazeGridColumns()
        {
            // Carikart listeleme için kolonlar oluşturuluyor.
            // Sunucunun gönderdiği json içerisindeki key ile GridHeaderItem içerisindeki "name" alanı aynı olmalıdır.
            // Liste ekranını oluşturmak için bu fonksiyon şart değil, fakat kullanım kolaylığı sağlamaktadır.
            gridHeader.Clear();

            GridHeaderItem newGridHeader = new GridHeaderItem();
            newGridHeader.name = "_key";
            newGridHeader.headerText = "";
            newGridHeader.visible = false;
            newGridHeader.columnWidth = 0;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "unvan";
            newGridHeader.headerText = "Unvan";
            newGridHeader.visible = true;
            newGridHeader.columnWidth = 200;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "carikartkodu";
            newGridHeader.headerText = "Cari Kart Kodu";
            newGridHeader.visible = true;
            newGridHeader.columnWidth = 200;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "ozelkod1";
            newGridHeader.headerText = "Özel Kod 1";
            newGridHeader.visible = true;
            newGridHeader.columnWidth = 200;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "ozelkod2";
            newGridHeader.headerText = "Özel Kod 2";
            newGridHeader.visible = false;
            newGridHeader.columnWidth = 200;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "adres1";
            newGridHeader.headerText = "Adres";
            newGridHeader.visible = true;
            newGridHeader.columnWidth = 200;
            gridHeader.Add(newGridHeader);

            newGridHeader.name = "ceptel";
            newGridHeader.headerText = "Cep Telefonu";
            newGridHeader.visible = true;
            newGridHeader.columnWidth = 100;
            gridHeader.Add(newGridHeader);

            gbx_CariListe.Columns.Clear();
            foreach(GridHeaderItem columnItem in gridHeader)
            {

                DataGridViewColumn col = new DataGridViewTextBoxColumn();

                col.HeaderText = columnItem.headerText;
                col.Name = columnItem.name;
                col.Visible = columnItem.visible;
                col.Width = columnItem.columnWidth;

                gbx_CariListe.Columns.Add(col);
            }
        }

        private void initilizeFirmaCombo()
        {
            // Firmalar listesini server'dan alıyoruz
            dynamic request = new ExpandoObject();
            request.sis_yetkili_firma_donem_sube_depo = new ExpandoObject();
            request.sis_yetkili_firma_donem_sube_depo.session_id = Sabitler.session_id;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);

            // Hata oluştu
            if(response == null)
            {
                MessageBox.Show("Bağlantı hatası oluştu. Lütfen daha sonra tekrar deneyiniz.");
                return;
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            else if(response.code != "200")
            {
                MessageBox.Show("Firmalar getirilirken bir hata oluştu. Code: " + response.code + " Message: " + response.msg);
                return;
            }

            // Firma listesini daha sonrada kullanabilmek için kaydediyoruz
            Sabitler.firma_sube_donem_listesi = response.result;

            // Firma combo'sunda gözükecek olan firma adları
            List<string> firmaAdlari = new List<string>();

            // Her bir firma_sube_donem sonucu içerisinden firmaadi  alani getirilir.
            foreach(dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                firmaAdlari.Add(firma_sube_donem.firmaadi);
            }

            // combo hazırlanır
            cmb_Firma.Items.Clear();
            cmb_Firma.Items.AddRange(firmaAdlari.ToArray());
        }

        private void btn_Kapat_Click(object sender, EventArgs e)
        {
            // Programın kapatılması için
            Environment.Exit(-1);
        }

        private void cmb_Firma_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Firma seçimi değiştikten sonra 
            // şubeler firmaya göre tekrardan ayarlanır.

            List<string> firmaDonemAdlari = new List<string>();

            // firma_donem_sube listesi içerisinde gezilir. Firma bulunur ve dönemleri getirilir.
            foreach(dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                // Combodaki isim ile ilgili olan firma_sube_donem bulunur
                if(firma_sube_donem.firmaadi == cmb_Firma.Text)
                {
                    // dönemler adları hazırlanır
                    // donem adları gorunendonem, baslangictarihi, bitistarihi seklinde hazırlanır
                    // aralarına 3'er boşluk bırakılır daha sonra boşluklara göre parse edileceklerdir.

                    if (Sabitler.HasAttr(firma_sube_donem, "donemler"))
                    {
                        foreach (dynamic donem in firma_sube_donem.donemler)
                        {
                            string donemAdi = donem.gorunendonemkodu;
                            donemAdi += "   ";
                            donemAdi += donem.baslangictarihi;
                            donemAdi += "   ";
                            donemAdi += donem.bitistarihi;
                            firmaDonemAdlari.Add(donemAdi);
                        }
                    }
                }
            }

            cmb_Donem.Items.Clear();
            cmb_Donem.Items.AddRange(firmaDonemAdlari.ToArray());

        }

        
        public void btn_Getir_Click(object sender, EventArgs e)
        {

            // Firma ve donem seçili olmalıdır.
            if(cmb_Firma.Text == "" || cmb_Donem.Text == "")
            {
                MessageBox.Show("Lütfen firma ve dönem seçiniz.");
                return;
            }

            long firmaKodu = -1;
            long donemKodu = -1;

            // firma_sube_donem listesi üzerinde gezilir
            // Firmakodu ve donemkodu getirilir.
            foreach(dynamic firma_sube_donem in Sabitler.firma_sube_donem_listesi)
            {
                // Firma bulundu
                if (firma_sube_donem.firmaadi == cmb_Firma.Text)
                {
                    firmaKodu = firma_sube_donem.firmakodu;

                    // donem kombosu içerisindeki text gorunendonem, baslangictarihi, bitistarihi 
                    // degerleri arasına 3'er boşluk bırakılmış şekilde hazırlanmıştı.

                    // combo içerisindeki text split edilir
                    // arr[0] -> gorunendonemkodu, arr[1] -> baslangictarihi, arr[2] -> bitistarihi
                    string[] arr = cmb_Donem.Text.Split(new[] { "   " }, StringSplitOptions.None);

                    // Firma içerisindeki dönemlerde gezilir.
                    if (Sabitler.HasAttr(firma_sube_donem, "donemler"))
                    {
                        foreach (dynamic donem in firma_sube_donem.donemler)
                        {
                            if (donem.gorunendonemkodu == arr[0]
                            && donem.baslangictarihi == arr[1]
                            && donem.bitistarihi == arr[2])
                            {
                                // Dönem bulundu
                                donemKodu = donem.donemkodu;
                                break;
                            }
                        }
                    }
                }
            }

            // seçilen firma ve doneme gore cariler serverdan getirilir.
            dynamic request = new ExpandoObject();
            request.scf_carikart_listele = new ExpandoObject();
            request.scf_carikart_listele.session_id = Sabitler.session_id;
            request.scf_carikart_listele.firma_kodu = firmaKodu;
            request.scf_carikart_listele.donem_kodu = donemKodu;
            request.scf_carikart_listele.limit = Sabitler.CARI_LISTELEME_LIMIT;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.scfEk);

            if (response == null)
            {
                MessageBox.Show("Cariler getirilirken, bağlantı hatası oluştu.");
                return;
            }
            // sonuc başarılı, firmakodu ve donemkodu daha sonraki islemlerde kullanılmasına karşı kaydedilir
            else if(response.code == "200")
            {

                // Daha sonra firma ve donem koduna hızlı erişebilmek için
                // Sabitler içerisinde firmaKodu ve donemKodu kaydedilir.
                Sabitler.seciliFirmaKodu = firmaKodu;
                Sabitler.seciliDonemKodu = donemKodu;
                Sabitler.carikartlistesi = response.result;
                // cariler listelenir.
                carileriListele(response.result);
            }
            // Yeterli kontör bulunmadığında gelen uyarı.
            else if (response.code == "406")
            {
                MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
            }
            else
            {
                MessageBox.Show("Cariler getirilirken bir hata oluştu. Code: " + response.code +" Message: " + response.msg);
                return;
            }
        }
        
        /// <summary>
        /// Cari kartları ekranda listeleyen fonksiyon
        /// </summary>
        /// <param name="cariliste"> List<ExpandoObject> </param>
        private void carileriListele(dynamic cariliste)
        {

            // listelemeden önce var olan gbx satırları silinir.
            gbx_CariListe.Rows.Clear();


            // Cariler listelenir
            foreach (dynamic cari in cariliste)
            {
                // bir satır, kolon sayısı kadar string içermektedir.
                string[] row = new string[gbx_CariListe.ColumnCount];

                // Her bir kolon için bir satırdaki değeri, cari üzerinden çekilir
                for (int i = 0; i < gridHeader.Count; i++)
                {
                    // Grid içerisindeki kolonları oluştururken json içerisindeki key alan ile aynı 
                    // olacak şekilde name alanını belirlemiştik.
                    GridHeaderItem item = gridHeader[i];
                    row[i] = (string)((IDictionary<string, Object>)cari)[item.name];
                }


                // satır eklemesi yapılır
                try
                {
                    gbx_CariListe.Rows.Add(row);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        
        private void btn_Sil_Click(object sender, EventArgs e)
        {
            // seçilen satır
            DataGridViewSelectedRowCollection rows = gbx_CariListe.SelectedRows;

            if(rows.Count == 0)
            {
                MessageBox.Show("Lütfen önce bir Cari seçiniz.");
                return;
            }

            // Tablo üzerinde multi row select kapalı oldugu için, tek satır seçildiğinden eminiz
            DataGridViewRow row = rows[0];

            // gridview'de kolonlar oluşturulurken, _key için gizli bir kolon oluşturulmuştu.
            // Bu kolon sayesinde silinecek carikart'in _key'i bilinir.
            string _key_scf_carikart = (string)row.Cells["_key"].Value;
            string unvan = (string)row.Cells["unvan"].Value;

            DialogResult dialogResult = MessageBox.Show(unvan +" ünvanlı carikart kaydını silmek istediğinize emin misiniz?", "Uyarı", MessageBoxButtons.YesNo);
            if(dialogResult == DialogResult.Yes)
            {
                // silme fonksiyonuna _key gonderilir
                dynamic request = new ExpandoObject();
                request.scf_carikart_sil = new ExpandoObject();
                request.scf_carikart_sil.session_id = Sabitler.session_id;
                request.scf_carikart_sil.firma_kodu = Sabitler.seciliFirmaKodu;
                request.scf_carikart_sil.donem_kodu = Sabitler.seciliDonemKodu;
                request.scf_carikart_sil.key = _key_scf_carikart;

                dynamic response = Sabitler.sendMessageToServer(request, Sabitler.scfEk);

                // Server ile iletişim hatası
                if(response == null)
                {
                    MessageBox.Show("Silme işlemi gerçekleştirilirken bir hata oluştu.\nLütfen daha sonra tekrar deneyiniz.");
                }
                // silme işlemi başarılı
                // gridview içerisinden silinmek istenen satır silinir.
                // Sabitler içerisindeki carikart dictionary'sinden silinir.
                else if (response.code == "200")
                {
                    MessageBox.Show("Silme işlemi başarılı.");
                    gbx_CariListe.Rows.Remove(row);

                    // Sunucu başarı ile silme işlemini yaptı. Tablo üzerinden de biz siliyoruz.
                    for (int i = 0; i < Sabitler.carikartlistesi.Count; i++)
                    {
                        if ((string)Sabitler.carikartlistesi[i]._key == _key_scf_carikart)
                        {
                            Sabitler.carikartlistesi.RemoveAt(i);
                            break;
                        }
                    }
                }
                // Yeterli kontör bulunmadığında gelen uyarı.
                else if (response.code == "406")
                {
                    MessageBox.Show("Yeterli kontörünüz bulunmamaktadır. Code: " + response.code + " Message: " + response.msg);
                }
                // Server ile iletişim başarılı. İşlem başarısız.
                else
                {
                    MessageBox.Show("Silme işlemi gerçekleştirilirken bir hata oluştu.\n"
                         + "Code: " + response.code +" Message: " + response.msg);
                }
            }
        }

        private void btn_Ekle_Click(object sender, EventArgs e)
        {

            // Daha önce getir butonuna tıklanmamışsa, sabitler içerisindeki firmakodu girilmemiş demektir.
            if (Sabitler.seciliFirmaKodu == -1)
            {
                MessageBox.Show("Ekleme yapmadan önce carileri getirmeniz gerekmektedir.");
                return;
            }

            // Ekleme formu açılır.
            // Yeni ekleme modunda olduğu için cari kartın _key bilgisi boş gönderilir.
            Form_CariKart form_carikart = new Form_CariKart(this, null);
            this.Hide();
            form_carikart.Show();
        }

        private void btn_Degistir_Click(object sender, EventArgs e)
        {
            
            DataGridViewSelectedRowCollection rows = gbx_CariListe.SelectedRows;

            if (rows.Count == 0)
            {
                MessageBox.Show("Lütfen önce bir Cari seçiniz.");
                return;
            }

            // seçilen rowdaki carikart'ın key değeri tablodan alınır.
            DataGridViewRow row = rows[0];
            string _key_scf_carikart = (string) row.Cells["_key"].Value;

            // Değiştirme modunda açılırken, seçili carinin _key bilgisi de gönderilir.
            Form_CariKart form_carikart = new Form_CariKart(this, _key_scf_carikart);
            this.Hide();
            form_carikart.Show();
        }

        private void Form_CariKartListesi_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(1);
        }

        private void bakiyeSorgula_Click(object sender, EventArgs e)
        {
            dynamic request = new ExpandoObject();
            request.sis_kontor_sorgula = new ExpandoObject();
            request.sis_kontor_sorgula.session_id = Sabitler.session_id;

            dynamic response = Sabitler.sendMessageToServer(request, Sabitler.sisEk);

            // Server ile iletişim hatası
            if (response == null)
            {
                MessageBox.Show("İşlem gerçekleştirilirken bir hata oluştu");
            }
            // silme işlemi başarılı
            // gridview içerisinden silinmek istenen satır silinir.
            // Sabitler içerisindeki carikart dictionary'sinden silinir.
            else if (response.code == "200")
            {
                MessageBox.Show("Kontör Sayısı: " + response.result._kontorsayisi);
            }
            else
            {
                MessageBox.Show("İşlem gerçekleştirilirken bir hata oluştu.\n"
                     + "Code: " + response.code + " Message: " + response.msg);
            }
        }
    }
}
