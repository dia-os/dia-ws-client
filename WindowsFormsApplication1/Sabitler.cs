using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Net;

namespace DIAWebServerExample
{
    class Sabitler
    {
        // Sunucu icerisindeki ortak değişken ve fonksiyonlar burada tutulur.
        // Bu değişkenlere büyün sayfalardan ortak erişim olabileceği için bu class oluşturulmuştur

        // Post metodları için gerekli adresler
        // JSON isteği gönderilirken sunucu adresinin devamına modüllere özel uzantı eklenir
        public static string sisEk = "/api/v3/sis/json";
        public static string scfEk = "/api/v3/scf/json";
        public static string sunucuAdresi = "http://diademo.dia.gen.tr";

        // Login olunduktan sonra session_id verilir.
        // Bu session_id her istek gonderilirken kullanılacaktır.
        public static string session_id = "";

        // Seçili firma ve dönem kodu isteklerde sıklıkla kullanıldığı ortak alanda tutulur.
        public static long seciliFirmaKodu = -1;
        public static long seciliDonemKodu = -1;

        public static dynamic firma_sube_donem_listesi = null;      // Server'dan cekilen firma_sube_donem listesi
        public static dynamic carikartlistesi = null;               // Server'dan cekilen carikart listesi

        public static int CARI_LISTELEME_LIMIT = 100;                // Tek seferde kaç carinin liste ekranında görüneceği

        /// <summary>
        /// Server ile iletişimi kuran fonksiyon. Parametre olarak gönderilen obje JSON string'ine çevrilir.
        /// Bağlantı ayarları yapılır ve istek gönderilir. Gelen cevap yine dinamik bir yapıda geri dönderilir.
        /// </summary>
        /// <param name="obj"> json stringine çevrilecek olan obje. ExpandoObject class'ı tavsiye edilmektedir.</param>
        /// <param name="ek"> İstekle ilgili olan modulun uzantısı</param>
        /// <returns></returns>
        public static dynamic sendMessageToServer(dynamic obj, string ek)
        {
            string jsonWS = "";
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(sunucuAdresi + ek);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    jsonWS = JsonConvert.SerializeObject(obj);

                    // Debug için json stringi kontrol edilebilir. Alttaki satırın commentini kaldırdığınızda
                    // ekrana gönderilen json string'i bastırılır.
                    Console.WriteLine("json: " + jsonWS);

                    streamWriter.Write(jsonWS);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    dynamic ret = JsonConvert.DeserializeObject<ExpandoObject>(result);
                    return ret;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("sendMessageToServer --- sunucu: " + sunucuAdresi + ek + " --- json: " + jsonWS);
                Console.WriteLine(e);
            }
            return null;
        }

        /// <summary>
        /// Expando object içinde hata almadan key değeri var mı diye kontrol eder
        /// </summary>
        /// <param name="expando"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool HasAttr(ExpandoObject expando, string key)
        {
            return ((IDictionary<string, Object>)expando).ContainsKey(key);
        }
    }
}
