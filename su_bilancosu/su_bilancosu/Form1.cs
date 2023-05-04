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


namespace su_bilancosu
{
    public partial class Form1 : Form
    {
        //Bağlantı ve Sorgu değişkenleri:
        SqlConnection con = new SqlConnection("server=.; Initial Catalog=SU_BILANCOSU;Integrated Security=SSPI");
        SqlCommand cmd;
        SqlDataReader dr;

        //Kullanılacak değişkenler
       public int sayac;
       public double ortalamaSicaklik;
       public double toplamSicaklikIndisi;
       public double toplamYagis;
       public double toplamDuzeltilmisPE;
       public double toplamGercek;
       public double toplamSuNoksani;
       public double toplamSuFazlasi;
       public double toplamAkis;
       public double yagisEtkinlikIndisi;
       public string iklimTipi;
       public string sicaklikEtkinlikIndisi;
       public double PEOran;
       public string PEOranString;
       public double kuraklikIndisi;
       public double nemlilikIndisi;
       public string NK_Indisi;
        bool hata; //Eğer bütün aylarda Yağış Miktarı DüzeltilmişPE'den büyükse, Bütün Aylık Değişimler=0 ve Birikmiş Su=100 olmalı

        //Kullanılacak diziler:
        public string[] iller = new string[81];
        public int[] enlemler = new int[81];
        public double[] duzeltmeKatsayi = new double[12];
        public double[] sicaklik = new double[12];
        public double[] sicaklikIndisi = new double[12];
        public double[] yagis = new double[12];
        public double[] duzeltilmemisPE = new double[12];
        public double[] duzeltilmisPE = new double[12];
        public double[] aylikDegisim = new double[12];
        public double[] birikmisSu = new double[12];
        public double[] gercek = new double[12];
        public double[] suFazlasi = new double[12];
        public double[] suNoksani = new double[12];
        public double[] akis = new double[12];
        public double[] nemlilikOrani = new double[12];

        //TextBox dizileri:
        TextBox[] txtSicaklik = new TextBox[12];
        TextBox[] txtDuzeltmeKatsayi = new TextBox[12];
        TextBox[] txtSicaklikIndisi = new TextBox[12];
        TextBox[] txtYagis = new TextBox[12];
        TextBox[] txtDuzeltilmemisPE = new TextBox[12];
        TextBox[] txtDuzeltilmisPE = new TextBox[12];
        TextBox[] txtAylikDegisim = new TextBox[12];
        TextBox[] txtBirikmisSu = new TextBox[12];
        TextBox[] txtGercek = new TextBox[12];
        TextBox[] txtNoksan = new TextBox[12];
        TextBox[] txtFazla = new TextBox[12];
        TextBox[] txtAkis = new TextBox[12];
        TextBox[] txtNemlilikOrani = new TextBox[12];

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //İller ve Enlemler veritabanından alınıp dizilere aktarılıyor.
            //İller comboBox'ı dolduruluyor
            cmd = new SqlCommand("SELECT iller.ilAd, enlemBilgileri.enlemDegeri FROM iller INNER JOIN enlemBilgileri ON iller.id = enlemBilgileri.ilId", con);
            con.Open();
            dr = cmd.ExecuteReader();
            int sayac = 0;
            while (dr.Read())
            {
                iller[sayac] = dr["ilAd"].ToString();
                enlemler[sayac] = Convert.ToInt32(dr["enlemDegeri"]);
                sayac++;
            }
            con.Close();
            foreach (string item in iller)
            {
                cmb_iller.Items.Add(item);
            }

            //Sıcaklık ile ilgili TextBox'lar diziye aktarılıyor.
            txtSicaklik[0] = txt_sicaklik_ocak;
            txtSicaklik[1] = txt_sicaklik_subat;
            txtSicaklik[2] = txt_sicaklik_mart;
            txtSicaklik[3] = txt_sicaklik_nisan;
            txtSicaklik[4] = txt_sicaklik_mayis;
            txtSicaklik[5] = txt_sicaklik_haziran;
            txtSicaklik[6] = txt_sicaklik_temmuz;
            txtSicaklik[7] = txt_sicaklik_agustos;
            txtSicaklik[8] = txt_sicaklik_eylul;
            txtSicaklik[9] = txt_sicaklik_ekim;
            txtSicaklik[10] = txt_sicaklik_kasim;
            txtSicaklik[11] = txt_sicaklik_aralik;

            //Sıcaklık İndisi ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtSicaklikIndisi[0] = txt_sicaklik_indisi_ocak;
            txtSicaklikIndisi[1] = txt_sicaklik_indisi_subat;
            txtSicaklikIndisi[2] = txt_sicaklik_indisi_mart;
            txtSicaklikIndisi[3] = txt_sicaklik_indisi_nisan;
            txtSicaklikIndisi[4] = txt_sicaklik_indisi_mayis;
            txtSicaklikIndisi[5] = txt_sicaklik_indisi_haziran;
            txtSicaklikIndisi[6] = txt_sicaklik_indisi_temmuz;
            txtSicaklikIndisi[7] = txt_sicaklik_indisi_agustos;
            txtSicaklikIndisi[8] = txt_sicaklik_indisi_eylul;
            txtSicaklikIndisi[9] = txt_sicaklik_indisi_ekim;
            txtSicaklikIndisi[10] = txt_sicaklik_indisi_kasim;
            txtSicaklikIndisi[11] = txt_sicaklik_indisi_aralik;
            foreach (TextBox item in txtSicaklikIndisi)
            {
                item.Enabled = false;
            }

            //Düzeltme Katsayısı ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtDuzeltmeKatsayi[0] = txt_duzeltme_ocak;
            txtDuzeltmeKatsayi[1] = txt_duzeltme_subat;
            txtDuzeltmeKatsayi[2] = txt_duzeltme_mart;
            txtDuzeltmeKatsayi[3] = txt_duzeltme_nisan;
            txtDuzeltmeKatsayi[4] = txt_duzeltme_mayis;
            txtDuzeltmeKatsayi[5] = txt_duzeltme_haziran;
            txtDuzeltmeKatsayi[6] = txt_duzeltme_temmuz;
            txtDuzeltmeKatsayi[7] = txt_duzeltme_agustos;
            txtDuzeltmeKatsayi[8] = txt_duzeltme_eylul;
            txtDuzeltmeKatsayi[9] = txt_duzeltme_ekim;
            txtDuzeltmeKatsayi[10] = txt_duzeltme_kasim;
            txtDuzeltmeKatsayi[11] = txt_duzeltme_aralik;
            foreach (TextBox item in txtDuzeltmeKatsayi)
            {
                item.Enabled = false;
            }

            //Yağışlar ile ilgili TextBox'lar diziye aktarılıyor.
            txtYagis[0] = txt_yagis_ocak;
            txtYagis[1] = txt_yagis_subat;
            txtYagis[2] = txt_yagis_mart;
            txtYagis[3] = txt_yagis_nisan;
            txtYagis[4] = txt_yagis_mayis;
            txtYagis[5] = txt_yagis_haziran;
            txtYagis[6] = txt_yagis_temmuz;
            txtYagis[7] = txt_yagis_agustos;
            txtYagis[8] = txt_yagis_eylul;
            txtYagis[9] = txt_yagis_ekim;
            txtYagis[10] = txt_yagis_kasim;
            txtYagis[11] = txt_yagis_aralik;

            //Düzeltilmemiş PE ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtDuzeltilmemisPE[0] = txt_duzeltilmemis_ocak;
            txtDuzeltilmemisPE[1] = txt_duzeltilmemis_subat;
            txtDuzeltilmemisPE[2] = txt_duzeltilmemis_mart;
            txtDuzeltilmemisPE[3] = txt_duzeltilmemis_nisan;
            txtDuzeltilmemisPE[4] = txt_duzeltilmemis_mayis;
            txtDuzeltilmemisPE[5] = txt_duzeltilmemis_haziran;
            txtDuzeltilmemisPE[6] = txt_duzeltilmemis_temmuz;
            txtDuzeltilmemisPE[7] = txt_duzeltilmemis_agustos;
            txtDuzeltilmemisPE[8] = txt_duzeltilmemis_eylul;
            txtDuzeltilmemisPE[9] = txt_duzeltilmemis_ekim;
            txtDuzeltilmemisPE[10] = txt_duzeltilmemis_kasim;
            txtDuzeltilmemisPE[11] = txt_duzeltilmemis_aralik;
            foreach (TextBox item in txtDuzeltilmemisPE)
            {
                item.Enabled = false;
            }

            //Düzeltilmiş PE ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtDuzeltilmisPE[0] = txt_duzeltilmis_ocak;
            txtDuzeltilmisPE[1] = txt_duzeltilmis_subat;
            txtDuzeltilmisPE[2] = txt_duzeltilmis_mart;
            txtDuzeltilmisPE[3] = txt_duzeltilmis_nisan;
            txtDuzeltilmisPE[4] = txt_duzeltilmis_mayis;
            txtDuzeltilmisPE[5] = txt_duzeltilmis_haziran;
            txtDuzeltilmisPE[6] = txt_duzeltilmis_temmuz;
            txtDuzeltilmisPE[7] = txt_duzeltilmis_agustos;
            txtDuzeltilmisPE[8] = txt_duzeltilmis_eylul;
            txtDuzeltilmisPE[9] = txt_duzeltilmis_ekim;
            txtDuzeltilmisPE[10] = txt_duzeltilmis_kasim;
            txtDuzeltilmisPE[11] = txt_duzeltilmis_aralik;
            foreach (TextBox item in txtDuzeltilmisPE)
            {
                item.Enabled = false;
            }

            //Birikmiş Suyun Aylık Değişimi ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtAylikDegisim[0] = txt_aylik_degisim_ocak;
            txtAylikDegisim[1] = txt_aylik_degisim_subat;
            txtAylikDegisim[2] = txt_aylik_degisim_mart;
            txtAylikDegisim[3] = txt_aylik_degisim_nisan;
            txtAylikDegisim[4] = txt_aylik_degisim_mayis;
            txtAylikDegisim[5] = txt_aylik_degisim_haziran;
            txtAylikDegisim[6] = txt_aylik_degisim_temmuz;
            txtAylikDegisim[7] = txt_aylik_degisim_agustos;
            txtAylikDegisim[8] = txt_aylik_degisim_eylul;
            txtAylikDegisim[9] = txt_aylik_degisim_ekim;
            txtAylikDegisim[10] = txt_aylik_degisim_kasim;
            txtAylikDegisim[11] = txt_aylik_degisim_aralik;
            foreach (TextBox item in txtAylikDegisim)
            {
                item.Enabled = false;
            }

            //Birikmiş Su ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtBirikmisSu[0] = txt_birikmis_su_ocak;
            txtBirikmisSu[1] = txt_birikmis_su_subat;
            txtBirikmisSu[2] = txt_birikmis_su_mart;
            txtBirikmisSu[3] = txt_birikmis_su_nisan;
            txtBirikmisSu[4] = txt_birikmis_su_mayis;
            txtBirikmisSu[5] = txt_birikmis_su_haziran;
            txtBirikmisSu[6] = txt_birikmis_su_temmuz;
            txtBirikmisSu[7] = txt_birikmis_su_agustos;
            txtBirikmisSu[8] = txt_birikmis_su_eylul;
            txtBirikmisSu[9] = txt_birikmis_su_ekim;
            txtBirikmisSu[10] = txt_birikmis_su_kasim;
            txtBirikmisSu[11] = txt_birikmis_su_aralik;
            foreach (TextBox item in txtBirikmisSu)
            {
                item.Enabled = false;
            }

            //Gerçek Evapotranspirasyon ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtGercek[0] = txt_gercek_ocak;
            txtGercek[1] = txt_gercek_subat;
            txtGercek[2] = txt_gercek_mart;
            txtGercek[3] = txt_gercek_nisan;
            txtGercek[4] = txt_gercek_mayis;
            txtGercek[5] = txt_gercek_haziran;
            txtGercek[6] = txt_gercek_temmuz;
            txtGercek[7] = txt_gercek_agustos;
            txtGercek[8] = txt_gercek_eylul;
            txtGercek[9] = txt_gercek_ekim;
            txtGercek[10] = txt_gercek_kasim;
            txtGercek[11] = txt_gercek_aralik;
            foreach (TextBox item in txtGercek)
            {
                item.Enabled = false;
            }

            //Su Noksanı ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtNoksan[0] = txt_noksan_ocak;
            txtNoksan[1] = txt_noksan_subat;
            txtNoksan[2] = txt_noksan_mart;
            txtNoksan[3] = txt_noksan_nisan;
            txtNoksan[4] = txt_noksan_mayis;
            txtNoksan[5] = txt_noksan_haziran;
            txtNoksan[6] = txt_noksan_temmuz;
            txtNoksan[7] = txt_noksan_agustos;
            txtNoksan[8] = txt_noksan_eylul;
            txtNoksan[9] = txt_noksan_ekim;
            txtNoksan[10] = txt_noksan_kasim;
            txtNoksan[11] = txt_noksan_aralik;
            foreach (TextBox item in txtNoksan)
            {
                item.Enabled = false;
            }

            //Su Fazlası ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtFazla[0] = txt_fazla_ocak;
            txtFazla[1] = txt_fazla_subat;
            txtFazla[2] = txt_fazla_mart;
            txtFazla[3] = txt_fazla_nisan;
            txtFazla[4] = txt_fazla_mayis;
            txtFazla[5] = txt_fazla_haziran;
            txtFazla[6] = txt_fazla_temmuz;
            txtFazla[7] = txt_fazla_agustos;
            txtFazla[8] = txt_fazla_eylul;
            txtFazla[9] = txt_fazla_ekim;
            txtFazla[10] = txt_fazla_kasim;
            txtFazla[11] = txt_fazla_aralik;
            foreach (TextBox item in txtFazla)
            {
                item.Enabled = false;
            }

            //Akış ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtAkis[0] = txt_akis_ocak;
            txtAkis[1] = txt_akis_subat;
            txtAkis[2] = txt_akis_mart;
            txtAkis[3] = txt_akis_nisan;
            txtAkis[4] = txt_akis_mayis;
            txtAkis[5] = txt_akis_haziran;
            txtAkis[6] = txt_akis_temmuz;
            txtAkis[7] = txt_akis_agustos;
            txtAkis[8] = txt_akis_eylul;
            txtAkis[9] = txt_akis_ekim;
            txtAkis[10] = txt_akis_kasim;
            txtAkis[11] = txt_akis_aralik;
            foreach (TextBox item in txtAkis)
            {
                item.Enabled = false;
            }

            //Nemlilik Oranı ile ilgili TextBox'lar diziye aktarılıyor ve kullanıcı girişleri engelleniyor.
            txtNemlilikOrani[0] = txt_nemlilik_ocak;
            txtNemlilikOrani[1] = txt_nemlilik_subat;
            txtNemlilikOrani[2] = txt_nemlilik_mart;
            txtNemlilikOrani[3] = txt_nemlilik_nisan;
            txtNemlilikOrani[4] = txt_nemlilik_mayis;
            txtNemlilikOrani[5] = txt_nemlilik_haziran;
            txtNemlilikOrani[6] = txt_nemlilik_temmuz;
            txtNemlilikOrani[7] = txt_nemlilik_agustos;
            txtNemlilikOrani[8] = txt_nemlilik_eylul;
            txtNemlilikOrani[9] = txt_nemlilik_ekim;
            txtNemlilikOrani[10] = txt_nemlilik_kasim;
            txtNemlilikOrani[11] = txt_nemlilik_aralik;
            foreach (TextBox item in txtNemlilikOrani)
            {
                item.Enabled = false;
            }

            //RichTextBox'daki yazılar ortalanıyor ve kullanıcı girişi engelleniyor.
            rtxt_sonuc.SelectionAlignment = HorizontalAlignment.Center;
            rtxt_sonuc.Enabled = false;

            //Diğer TextBox'lara kullanıcı girişleri engelleniyor.
            txt_enlem.Enabled = false;
            txt_ortalama_sicaklik.Enabled = false;
            txt_toplam_sicaklik_indisi.Enabled = false;
            txt_toplam_duzeltilmisPE.Enabled = false;
            txt_toplam_yagis.Enabled = false;
            txt_toplam_gercek.Enabled = false;
            txt_toplam_noksan.Enabled = false;
            txt_toplam_fazla.Enabled = false;
            txt_toplam_akis.Enabled = false;
        }

        private void cmb_iller_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Seçilen ile göre enlem bilgisi TextBox'a aktarılıyor.
            int enlem = enlemler[cmb_iller.SelectedIndex];
            txt_enlem.Text = enlem.ToString();

            //Seçilen ilin enlem bilgisine göre Düzeltme Katsayısı bilgileri TextBox'lara aktarılıyor.
            cmd = new SqlCommand("SELECT ocak, subat, mart, nisan, mayis, haziran, temmuz, agustos, eylul, ekim, kasim, aralik FROM GUNES_KATSAYI WHERE(sira = @sira)", con);
            cmd.Parameters.AddWithValue("@sira", enlem);
            con.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                duzeltmeKatsayi[0] = Convert.ToDouble(dr["ocak"]);
                duzeltmeKatsayi[1] = Convert.ToDouble(dr["subat"]);
                duzeltmeKatsayi[2] = Convert.ToDouble(dr["mart"]);
                duzeltmeKatsayi[3] = Convert.ToDouble(dr["nisan"]);
                duzeltmeKatsayi[4] = Convert.ToDouble(dr["mayis"]);
                duzeltmeKatsayi[5] = Convert.ToDouble(dr["haziran"]);
                duzeltmeKatsayi[6] = Convert.ToDouble(dr["temmuz"]);
                duzeltmeKatsayi[7] = Convert.ToDouble(dr["agustos"]);
                duzeltmeKatsayi[8] = Convert.ToDouble(dr["eylul"]);
                duzeltmeKatsayi[9] = Convert.ToDouble(dr["ekim"]);
                duzeltmeKatsayi[10] = Convert.ToDouble(dr["kasim"]);
                duzeltmeKatsayi[11] = Convert.ToDouble(dr["aralik"]);
            }
            con.Close();
            sayac = 0;
            foreach (double item in duzeltmeKatsayi)
            {
                txtDuzeltmeKatsayi[sayac].Text = item.ToString();
                sayac++;
            }
        }

        private void btn_sicaklik_indis_hesaplama_Click(object sender, EventArgs e)
        {
            //Girilen sıcaklıklar diziye aktarılıyor.
            sayac = 0;
            foreach (TextBox item in txtSicaklik)
            {
                sicaklik[sayac] = Convert.ToDouble(item.Text);
                sayac++;
            }

            //Ortalama Sıcaklık hesaplanıyor ve TextBox'a aktarılıyor.
            ortalamaSicaklik = 0;
            foreach (double item in sicaklik)
            {
                ortalamaSicaklik += item;
            }
            ortalamaSicaklik = Math.Round((ortalamaSicaklik / 12), 2, MidpointRounding.AwayFromZero);
            txt_ortalama_sicaklik.Text = ortalamaSicaklik.ToString();

            //Sıcaklıklara göre ayların Sıcaklık İndisleri ile Toplamları hesaplanıyor ve TextBox'lara aktarılıyor.
            sayac = 0;
            toplamSicaklikIndisi = 0;
            foreach (double item in sicaklik)
            {
                if (item <= 0)
                {
                    sicaklikIndisi[sayac] = 0;
                }
                else
                {
                    sicaklikIndisi[sayac] = Math.Round(Math.Pow((item / 5), 1.514), 2, MidpointRounding.AwayFromZero);
                    toplamSicaklikIndisi += sicaklikIndisi[sayac];
                }
                sayac++;
            }
            sayac = 0;
            foreach (double item in sicaklikIndisi)
            {
                txtSicaklikIndisi[sayac].Text = item.ToString();
                sayac++;
            }
            txt_toplam_sicaklik_indisi.Text = toplamSicaklikIndisi.ToString();
        }

        private void btn_yagis_hesaplama_Click(object sender, EventArgs e)
        {
            //Girilen yağışlar diziye aktarılıyor.
            sayac = 0;
            foreach (TextBox item in txtYagis)
            {
                yagis[sayac] = Convert.ToDouble(item.Text);
                sayac++;
            }

            //Toplam Yağış hesaplanıyor ve TextBox'a aktarılıyor.
            toplamYagis = 0;
            foreach (double item in yagis)
            {
                toplamYagis += item;
            }
            txt_toplam_yagis.Text = toplamYagis.ToString();

            //DüzeltilmemişPE, DüzeltilmişPE ile DüzeltilmişPE Toplamı hesaplanıyor ve TextBox'lara aktarılıyor
            toplamDuzeltilmisPE = 0;
            double p = toplamSicaklikIndisi;
            double a = ((6.7510 * Math.Pow(10, -7) * Math.Pow(p, 3)) - (7.7110 * Math.Pow(10, -5) * Math.Pow(p, 2)) + (1.791210 * Math.Pow(10, -2) * p) + 0.49239);
            sayac = 0;
            foreach (double item in sicaklik)
            {
                if (item <= 0)
                {
                    duzeltilmemisPE[sayac] = 0;
                    duzeltilmisPE[sayac] = 0;
                }
                else
                {
                    duzeltilmemisPE[sayac] = Math.Round(16 * Math.Pow(((10 * item) / p), a), 2, MidpointRounding.AwayFromZero);
                    duzeltilmisPE[sayac] = Math.Round((duzeltilmemisPE[sayac] * duzeltmeKatsayi[sayac]), 2, MidpointRounding.AwayFromZero);
                    toplamDuzeltilmisPE += duzeltilmisPE[sayac];
                }
                sayac++;
            }
            sayac = 0;
            foreach (double item in duzeltilmemisPE)
            {
                txtDuzeltilmemisPE[sayac].Text = item.ToString();
                sayac++;
            }
            sayac = 0;
            foreach (double item in duzeltilmisPE)
            {
                txtDuzeltilmisPE[sayac].Text = item.ToString();
                sayac++;
            }
            txt_toplam_duzeltilmisPE.Text = toplamDuzeltilmisPE.ToString();

            //Birikmiş Su, Birikmiş Suyun Aylık Değişimi, Gerçek Evapotranspirasyon, Su Noksanı, Su Fazlası ve Akış
            //hesaplanıyor ve TextBox'lara aktarılıyor.

            //Bütün aylarda Yağış Miktarı'nın DüzeltilmişPE'den büyük olup olmadığı kontrol ediliyor.
            hata = false;
            sayac = 0;
            int sayac2 = 0;
            foreach (double item in yagis)
            {
                if (duzeltilmisPE[sayac]<item)
                {
                    sayac2++;
                }
                if (sayac2==11)
                {
                    hata = true;
                }
                sayac++;
            }

            //Hesaplamalar Eylül ayından başladığı için olaylar 2 döngüde kodlanıyor.
            //İlk döngü Eylül ayından Aralık ayına kadar çalışıyor.
            double eskiAkis = 0;
            double eskiDepo = 0;
            double depo = 0;
            for (int i = 8; i < 12; i++)
            {
                //Eğer Yağış Miktari, Düzeltilmiş Potansiyel Evapotranspirasyon'dan büyükse
                //Gerçek Evapotranspirasyon, üzeltilmiş Potansiyel Evapotranspirasyon'a eşittir.
                //Birikmiş Suyun Aylık Değişimi ise Yağış Miktarı'ndan Gerçek Evapotranspirasyon çıkarılarak bulunuyor.
                //Birikmiş Su, Aylık Değişim Miktarı kadar değişime uğruyor.
                //Depo 100'ün üzerine çıksa bile Birikmiş Su Miktarı 100'ü geçemez.
                //Bu nedenle Eğer Depo 100'ü aşarsa, 100 değerine sabitleniyor.
                //Ve Eski Depo'daki Birikmiş Su Miktarı 100'den çıkarılarak Aylık Birikmiş Su Miktarı'nın hatasız bulunması sağlanıyor.
                if (yagis[i] > duzeltilmisPE[i])
                {
                    //Eğer yapılan kontrol sonucu bütün aylarda Yağış Miktarı'nın DüzeltilmişPE'den büyükse,
                    //hesaplamaların doğru sonucu vermesi için değişiklikler yapılıyor.
                    if (hata==true) 
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = duzeltilmisPE[i];
                    aylikDegisim[i] = Math.Round(yagis[i] - gercek[i], 2);
                    depo += aylikDegisim[i];
                    birikmisSu[i] = depo;
                    if (birikmisSu[i] >= 100)
                    {
                        suFazlasi[i] = birikmisSu[i] - 100;
                        suNoksani[i] = 0;
                        akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                        eskiAkis = suFazlasi[i] / 2;

                        depo = 100;
                        birikmisSu[i] = depo;
                        aylikDegisim[i] = depo - eskiDepo;
                    }
                    else
                    {
                        suFazlasi[i] = 0;
                        suNoksani[i] = 0;
                        akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                        eskiAkis = suFazlasi[i] / 2;
                    }
                    eskiDepo = depo;
                }
                //Eğer Yağış Miktarı, Düzeltilmiş Potansiyel Evapotranspiyon'dan küçükse fakat
                //Depo'daki Birikmiş Su Miktarı ile Yağış Miktarı, Düzeltilmiş Potansiyel Evapotranspirasyon'u karşılayabiliyor ise
                //Gerçek Evapotranspirasyon, Düzeltilmiş Potansiyel Evapotranspirasyon'a eşittir.
                //Birikmiş Suyun Aylık Değişimi, aynı şekilde hesaplanıyor.
                //Birikmiş Su, Aylık Değişim Miktarı kadar değişime uğruyor.
                //Böyle bir durumda Depo'nun 0'ın altına düşme ihtimali bulunmuyor.
                else if (depo + yagis[i] > duzeltilmisPE[i])
                {
                    if (hata == true)
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = duzeltilmisPE[i];
                    aylikDegisim[i] = Math.Round(yagis[i] - gercek[i], 2);
                    depo += aylikDegisim[i];
                    birikmisSu[i] = depo;
                    suFazlasi[i] = 0;
                    suNoksani[i] = 0;
                    akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                    eskiAkis = suFazlasi[i] / 2;
                }
                //Eğer Yağış Miktarı, Düzeltilmiş Potansiyel Evapotranspiyon'dan küçükse
                //Ve Depo'daki Birikmiş Su Miktarı ile Yağış Miktarı, Düzeltilmiş Evapotranspirasyon'u karşılayamıyor ise
                //Gerçek Evapotranspirasyon, Depo'daki Birikmiş Su Miktarı ile Yağış Miktarı'nın toplamına eşittir.
                //Depo'daki bütün Birikmiş Su kullanılacağı için Birikmiş Suyun Aylık Değişimi,
                //Depo'daki Birikmiş Su Miktarının negatif halidir,
                //Ve Birikmiş Su Miktarı sıfırlanır.
                else
                {
                    if (hata == true)
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = depo + yagis[i];
                    aylikDegisim[i] = Math.Round((depo * -1), 2);
                    depo = 0;
                    birikmisSu[i] = depo;
                    suFazlasi[i] = 0;
                    suNoksani[i] = duzeltilmisPE[i] - gercek[i];
                    akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                    eskiAkis = suFazlasi[i] / 2;
                }
                txtAylikDegisim[i].Text = Math.Round(aylikDegisim[i], 2).ToString();
                txtBirikmisSu[i].Text = Math.Round(birikmisSu[i], 2).ToString();
                txtGercek[i].Text = Math.Round(gercek[i], 2).ToString();
                txtNoksan[i].Text = Math.Round(suNoksani[i], 2).ToString();
                txtFazla[i].Text = Math.Round(suFazlasi[i], 2).ToString();
                txtAkis[i].Text = Math.Round(akis[i], 2, MidpointRounding.AwayFromZero).ToString();
            }
            
            //İkinci döngü Ocak ayından Ağustos ayına kadar çalışıyor.
            //İlk döngüyle aynı işlevi görüyor.
            for (int i = 0; i < 8; i++)
            {
                if (yagis[i] > duzeltilmisPE[i])
                {
                    if (hata == true)
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = duzeltilmisPE[i];
                    aylikDegisim[i] = Math.Round(yagis[i] - gercek[i], 2);
                    depo += aylikDegisim[i];
                    birikmisSu[i] = depo;
                    if (birikmisSu[i] >= 100)
                    {
                        suFazlasi[i] = birikmisSu[i] - 100;
                        suNoksani[i] = 0;
                        akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                        eskiAkis = suFazlasi[i] / 2;

                        depo = 100;
                        birikmisSu[i] = depo;
                        aylikDegisim[i] = depo - eskiDepo;
                    }
                    else
                    {
                        suFazlasi[i] = 0;
                        suNoksani[i] = 0;
                        akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                        eskiAkis = suFazlasi[i] / 2;
                    }
                    eskiDepo = depo;
                }
                else if (depo + yagis[i] > duzeltilmisPE[i])
                {
                    if (hata == true)
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = duzeltilmisPE[i];
                    aylikDegisim[i] = Math.Round(yagis[i] - gercek[i], 2);
                    depo += aylikDegisim[i];
                    birikmisSu[i] = depo;
                    suFazlasi[i] = 0;
                    suNoksani[i] = 0;
                    akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                    eskiAkis = suFazlasi[i] / 2;
                }
                else
                {
                    if (hata == true)
                    {
                        depo = 100;
                        eskiDepo = 100;
                    }
                    gercek[i] = depo + yagis[i];
                    aylikDegisim[i] = Math.Round((depo * -1), 2);
                    depo = 0;
                    birikmisSu[i] = depo;
                    suFazlasi[i] = 0;
                    suNoksani[i] = duzeltilmisPE[i] - gercek[i];
                    akis[i] = (suFazlasi[i] / 2) + eskiAkis;
                    eskiAkis = suFazlasi[i] / 2;
                }
                txtAylikDegisim[i].Text = Math.Round(aylikDegisim[i], 2).ToString();
                txtBirikmisSu[i].Text = Math.Round(birikmisSu[i], 2).ToString();
                txtGercek[i].Text = Math.Round(gercek[i], 2).ToString();
                txtNoksan[i].Text = Math.Round(suNoksani[i], 2).ToString();
                txtFazla[i].Text = Math.Round(suFazlasi[i], 2).ToString();
                txtAkis[i].Text = Math.Round(akis[i], 2, MidpointRounding.AwayFromZero).ToString();
            }

            if (hata==true)
            {
                akis[8] = Math.Round(((suFazlasi[8] / 2) + (suFazlasi[7] / 2)), 2, MidpointRounding.AwayFromZero);
                txtAkis[8].Text = Math.Round(akis[8], 2, MidpointRounding.AwayFromZero).ToString();
            }

            //Toplam Gerçek Evapotranspirasyon hesaplanıyor ve TextBox'a aktarılıyor.
            toplamGercek = 0;
            foreach (double item in gercek)
            {
                toplamGercek += item;
            }
            txt_toplam_gercek.Text = toplamGercek.ToString();

            //Toplam Su Noksanı hesaplanıyor ve TextBox'a aktarılıyor.
            toplamSuNoksani = 0;
            foreach (double item in suNoksani)
            {
                toplamSuNoksani += item;
            }
            txt_toplam_noksan.Text = toplamSuNoksani.ToString();

            //Toplam Su Fazlası hesaplanıyor ve TextBox'a aktarılıyor.
            toplamSuFazlasi = 0;
            foreach (double item in suFazlasi)
            {
                toplamSuFazlasi += item;
            }
            txt_toplam_fazla.Text = toplamSuFazlasi.ToString();

            //Toplam Akış hesaplanıyor ve TextBox'a aktarılıyor.
            toplamAkis = 0;
            foreach (double item in akis)
            {
                toplamAkis += item;
            }
            txt_toplam_akis.Text = toplamAkis.ToString();

            //Nemlilik Oranı hesaplanıyor ve TextBox'lara aktarılıyor.
            sayac = 0;
            foreach (double item in yagis)
            {
                if (sicaklik[sayac] <= 0)
                {
                    nemlilikOrani[sayac] = 0;
                }
                else
                {
                    nemlilikOrani[sayac] = Math.Round(((item - duzeltilmisPE[sayac]) / duzeltilmisPE[sayac]), 2, MidpointRounding.AwayFromZero);
                }
                sayac++;
            }
            sayac = 0;
            foreach (double item in nemlilikOrani)
            {
                txtNemlilikOrani[sayac].Text = item.ToString();
                sayac++;
            }

            // ----------------İklim Tipi Hesaplama----------------
            //Yağış Etkinliği İndisi hesaplanıyor ve RichTextBox'a aktarılıyor.
            yagisEtkinlikIndisi = Math.Round((((100 * toplamSuFazlasi) - (60 * toplamSuNoksani)) / toplamDuzeltilmisPE), 2, MidpointRounding.AwayFromZero);
            if (yagisEtkinlikIndisi >= 100)
            {
                iklimTipi = "A çok nemli";
            }
            else if (yagisEtkinlikIndisi >= 80)
            {
                iklimTipi = "B4 Nemli";
            }
            else if (yagisEtkinlikIndisi >= 60)
            {
                iklimTipi = "B3 Nemli";
            }
            else if (yagisEtkinlikIndisi >= 40)
            {
                iklimTipi = "B2 Nemli";
            }
            else if (yagisEtkinlikIndisi >= 20)
            {
                iklimTipi = "B1 Nemli";
            }
            else if (yagisEtkinlikIndisi >= 0)
            {
                iklimTipi = "C2 Yarı Nemli";
            }
            else if (yagisEtkinlikIndisi >= -20)
            {
                iklimTipi = "C1 Kurak-Az Nemli";
            }
            else if (yagisEtkinlikIndisi >= -40)
            {
                iklimTipi = "D Yarı Kurak";
            }
            else
            {
                iklimTipi = "E Kurak(Çöl)";
            }
            rtxt_sonuc.Text += cmb_iller.Text + " İstasyonunun İklim Tipi\n" + iklimTipi + ",\n";

            //Sıcaklık Etkinlik İndisi hesaplanıyor ve RichTextBox'a aktarılıyor.
            if (toplamDuzeltilmisPE >= 1140)
            {
                sicaklikEtkinlikIndisi = "A' Megatermal(Yüksek Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 997)
            {
                sicaklikEtkinlikIndisi = "B'4 Mezotermal(Orta Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 855)
            {
                sicaklikEtkinlikIndisi = "B'3 Mezotermal(Orta Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 712)
            {
                sicaklikEtkinlikIndisi = "B'2 Mezotermal(Orta Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 570)
            {
                sicaklikEtkinlikIndisi = "B'1 Mezotermal(Orta Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 427)
            {
                sicaklikEtkinlikIndisi = "C'2 Mikrotermal(Düşük Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 285)
            {
                sicaklikEtkinlikIndisi = "C'1 Mikrotermal(Düşük Sıcaklıklardaki İklimler)";
            }
            else if (toplamDuzeltilmisPE >= 142)
            {
                sicaklikEtkinlikIndisi = "D Tundra(Çok Düşük Sıcaklıklardaki İklimler)";
            }
            else
            {
                sicaklikEtkinlikIndisi = "E Don(Çok Düşük Sıcaklıklardaki İklimler)";
            }
            rtxt_sonuc.Text += sicaklikEtkinlikIndisi + ",\n";

            //Yağış Rejimine Göre Ortaya Konan İndisler
            if (yagisEtkinlikIndisi >= 0)
            {
                kuraklikIndisi = Math.Round(((toplamSuNoksani * 100) / toplamDuzeltilmisPE), 2, MidpointRounding.AwayFromZero);
                if (kuraklikIndisi <= 16.7)
                {
                    NK_Indisi = "r derecesinde Su Noksanı olmayan veya pek az olan";
                }
                else if (kuraklikIndisi<=33.3)
                {
                    NK_Indisi = "s derecesinde Su Noksanı yaz mevsiminde ve orta derecede olan";
                }
                else
                {
                    NK_Indisi = "s2 derecesinde Su Noksanı yaz mevsiminde ve çok kuvvetli olan";
                }
            }
            else
            {
                nemlilikIndisi = Math.Round(((toplamSuFazlasi * 100) / toplamDuzeltilmisPE), 2, MidpointRounding.AwayFromZero);
                if (nemlilikIndisi<=10)
                {
                    NK_Indisi = "d derecesinde Su Fazlası olmayan veya pek az olan";
                }
                else if (nemlilikIndisi<=20)
                {
                    NK_Indisi = "s derecesinde Su Fazlası kış mevsiminde ve orta derecede olan";
                }
                else
                {
                    NK_Indisi = "s2 derecesinde Su Fazlası kış mevsiminde ve çok kuvvetli olan";
                }
            }
            rtxt_sonuc.Text += NK_Indisi + ",\n";

            //PE'nin 3 yaz ayına oranı hesaplanıyor ve RichTextBox'a aktarılıyor.
            PEOran = ((duzeltilmisPE[5] + duzeltilmisPE[6] + duzeltilmisPE[7]) * 100) / toplamDuzeltilmisPE;
            if (PEOran >= 88)
            {
                PEOranString = "d' Tam Karasal(Kontinental)";
            }
            else if (PEOran > 76.3)
            {
                PEOranString = "c'1 Karasal Etkili";
            }
            else if (PEOran > 68)
            {
                PEOranString = "c'2 Karasal Etkili";
            }
            else if (PEOran > 61.6)
            {
                PEOranString = "b'1 Karasal Etkili";
            }
            else if (PEOran > 56.3)
            {
                PEOranString = "b'2 Denizsel Etkili";
            }
            else if (PEOran > 51.9)
            {
                PEOranString = "b'3 Denizsel Etkili";
            }
            else if (PEOran > 48)
            {
                PEOranString = "b'4 Denizsel Etkili";
            }
            else
            {
                PEOranString = "a Tam Denizsel(oseanik)";
            }
            rtxt_sonuc.Text += PEOranString + " iklimine sahiptir.";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt_sicaklik_ocak.Text = "8,5";
            txt_sicaklik_subat.Text = "9,2";
            txt_sicaklik_mart.Text = "11,1";
            txt_sicaklik_nisan.Text = "15,4";
            txt_sicaklik_mayis.Text = "20,4";
            txt_sicaklik_haziran.Text = "25";
            txt_sicaklik_temmuz.Text = "27,5";
            txt_sicaklik_agustos.Text = "26,7";
            txt_sicaklik_eylul.Text = "22,9";
            txt_sicaklik_ekim.Text = "18,5";
            txt_sicaklik_kasim.Text = "14,2";
            txt_sicaklik_aralik.Text = "10,2";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            txt_yagis_ocak.Text = "132,1";
            txt_yagis_subat.Text = "96,6";
            txt_yagis_mart.Text = "68,8";
            txt_yagis_nisan.Text = "46,6";
            txt_yagis_mayis.Text = "28,3";
            txt_yagis_haziran.Text = "8,1";
            txt_yagis_temmuz.Text = "2,8";
            txt_yagis_agustos.Text = "2";
            txt_yagis_eylul.Text = "11,2";
            txt_yagis_ekim.Text = "40,2";
            txt_yagis_kasim.Text = "85,7";
            txt_yagis_aralik.Text = "154,6";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            txt_sicaklik_ocak.Text = "7";
            txt_sicaklik_subat.Text = "6,7";
            txt_sicaklik_mart.Text = "7,8";
            txt_sicaklik_nisan.Text = "11,1";
            txt_sicaklik_mayis.Text = "15,8";
            txt_sicaklik_haziran.Text = "19,7";
            txt_sicaklik_temmuz.Text = "22,2";
            txt_sicaklik_agustos.Text = "22,6";
            txt_sicaklik_eylul.Text = "19,5";
            txt_sicaklik_ekim.Text = "16,1";
            txt_sicaklik_kasim.Text = "12,6";
            txt_sicaklik_aralik.Text = "9,1";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            txt_yagis_ocak.Text = "240,9";
            txt_yagis_subat.Text = "208,9";
            txt_yagis_mart.Text = "181,9";
            txt_yagis_nisan.Text = "104,3";
            txt_yagis_mayis.Text = "93,8";
            txt_yagis_haziran.Text = "131,1";
            txt_yagis_temmuz.Text = "156,3";
            txt_yagis_agustos.Text = "201,4";
            txt_yagis_eylul.Text = "265,5";
            txt_yagis_ekim.Text = "278,2";
            txt_yagis_kasim.Text = "249,8";
            txt_yagis_aralik.Text = "245,2";
        }

        private void txt_sicaklik_ocak_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-' ;
        }

        private void txt_sicaklik_subat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_mart_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_nisan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_mayis_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_haziran_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_temmuz_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_agustos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_eylul_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_ekim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_kasim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_sicaklik_aralik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_ocak_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_subat_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_mart_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_nisan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_mayis_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_haziran_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_temmuz_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_agustos_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_eylul_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_ekim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_kasim_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }

        private void txt_yagis_aralik_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',' && e.KeyChar != '-';
        }
        //Buton tıklandığı zaman;
        //Form2 sayfasının bağlantısını yapıyoruz.
        //Form2 sayfasına yagis,gercek ve dpe değerlerini gönderiyoruz.
        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            for (int i = 0; i < 12; i++)
            {
                form2.yagis[i] = yagis[i];
                form2.gercek[i] = gercek[i];
                form2.dpe[i] = duzeltilmisPE[i];
                
            }
                form2.Show();
        }
    }
}
