using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace PD3100sifra43
{
    public partial class Form1 : Form
    {
        Mail email = new Mail();
        DataSetPuni dsPuni = new DataSetPuni();
        SqlConnection conn = new SqlConnection();
        string nfEXP = null;
        string poruka = DateTime.Now.ToString("dd.MM.yyyy H:mm:ss") + " >> Razmjena sa poreskom PD3100 prijava i odjava << \r\n";
        bool exception = false;
        public Form1()
        {
            InitializeComponent();
            
            try
            {
                conn.ConnectionString = Properties.Settings.Default.Konekcija; /*LORD*/
                //conn.ConnectionString = Properties.Settings.Default.PioRSConnectionString; /*radenko-PC*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                Log.Write("============================================================================================={0}","");
                Log.Write("     ::: Razmjena podataka PIO => PURS :::{0}", "");
                //genXML();
                impFile();
                //email.SendEmail(poruka);
                Log.Write("     ::: KRAJ :::{0}", "");
                Log.Write("============================================================================================={0}","");
                Application.Exit();
            }
            catch (Exception ex)
            {
                Log.Write("===========================================================");
                Log.Write("->::: GREŠKA ::: Generisanje XML fajla i import rezultat fajla je prekinuto: {0} ", ex.ToString());
                //MessageBox.Show(ex.Message);

            }
        }
    #region export
        public void puniDS(DataTable tabela)
        {
            try
            {
                conn.Open();
                SqlCommand komanda = new SqlCommand("likvidatura.TabelaZaExportDnevni", conn);
                komanda.CommandType = CommandType.StoredProcedure;
                SqlDataReader citac = komanda.ExecuteReader();
                gPunjac punjac = new gPunjac();
                punjac.PuniDataSet(citac, tabela);
            }
            catch (Exception ex)
            {
                exception = true;
                //Console.WriteLine(ex.Message);
                //MessageBox.Show(ex.Message);
                Log.Write("->::: GREŠKA ::: Punjenje DataSeta podacima iz tabele {0} je prekinuto: {1}!", tabela.TableName, ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Greška kod punjenja data seta " + ex.Message;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                //if (exception)
                //{
                //    email.SendEmail(poruka);
                //    this.Close();
                //}
            }
        }
        public void genXML()
        {
            string path = "C:\\PD3100RazmjenaPU\\ArchiveFolder\\Export\\";
                if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            DateTime LastMonthLastDate = DateTime.Today.AddDays(0 - DateTime.Today.Day);
            DateTime LastMonthFirstDate = LastMonthLastDate.AddDays(1 - LastMonthLastDate.Day);
            string datumS = LastMonthFirstDate.ToString("yyyy-MM-dd");
            int slanje = 1;
            string FileName = DateTime.Today.ToString("yyyyMMdd") + "_0"+slanje+"_PIORS_PURS_PD3100.xml";
            string NazivExport = FileName;
            string nf = "C:\\razmjena\\export\\" + FileName;
            if (File.Exists(nf))
            {
                slanje++;
                FileName = DateTime.Today.ToString("yyyyMMdd") + "_0" + slanje + "_PIORS_PURS_PD3100.xml";
                nf = "C:\\razmjena\\export\\" + FileName;   
            }
            int count = 0;         
            try
            {
                /*u sledecoj verziji treba ispitati da li podaci sadrze < ili > &lt i &gt */
                //string nfEXP = "C:\\ArchiveFolder\\Export\\";  
                puniDS(dsPuni.PD3100Korisnik);
                count = dsPuni.PD3100Korisnik.Rows.Count;
                if (dsPuni.PD3100Korisnik.Rows.Count != 0)
                {
                    FileStream fs = new FileStream(nf, FileMode.Append, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs);

                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>");
                    sw.WriteLine("<xml>");
                    sw.WriteLine("<JS_PODACI xmlns=\"\">");
                    sw.WriteLine("<VRSTA_PODATAKA_SIFRA>1</VRSTA_PODATAKA_SIFRA>");
                    sw.WriteLine("<VRSTA_PODATAKA>ПОДАЦИ О ОСИГУРАЊУ ЗА ПЕНЗИОНЕРА</VRSTA_PODATAKA>");
                    sw.WriteLine("<BROJ_SLOGOVA>" + count + "</BROJ_SLOGOVA>");
                        
                    for (int i = 0; i < dsPuni.PD3100Korisnik.Count; i++)
                    {
                        sw.WriteLine("<PRIJAVA xmlns=\"\">");

                        sw.WriteLine("<BARKOD>" + dsPuni.PD3100Korisnik.Rows[i]["LBO"] + "</BARKOD>");
                        sw.WriteLine("<VRSTA_PRIJAVE_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["VRSTA_PRIJAVE_SIFRA"] + "</VRSTA_PRIJAVE_SIFRA>");
                        sw.WriteLine("<VRSTA_PRIJAVE>" + dsPuni.PD3100Korisnik.Rows[i]["VRSTA_PRIJAVE"] + "</VRSTA_PRIJAVE>");
                        sw.WriteLine("<TIP_PRIJAVE_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["TIP_PRIJAVE_SIFRA"] + "</TIP_PRIJAVE_SIFRA>");
                        sw.WriteLine("<TIP_PRIJAVE>" + dsPuni.PD3100Korisnik.Rows[i]["TIP_PRIJAVE"] + "</TIP_PRIJAVE>");
                        sw.WriteLine("<DATUM_PODNOSENJA>" + dsPuni.PD3100Korisnik.Rows[i]["DATUM_PODNOSENJA"] + "</DATUM_PODNOSENJA>");
                        sw.WriteLine("<DATUM_PRIJEMA>" + dsPuni.PD3100Korisnik.Rows[i]["DATUM_PRIJEMA"] + "</DATUM_PRIJEMA>");
                            
                        sw.WriteLine("<UPLATIOC>");
                        sw.WriteLine("<JIB>" + dsPuni.PD3100Korisnik.Rows[i]["JIB"] + "</JIB>");
                        sw.WriteLine("<NAZIV>" + dsPuni.PD3100Korisnik.Rows[i]["NAZIV"] + "</NAZIV>");
                        sw.WriteLine("<ADRESA>" + dsPuni.PD3100Korisnik.Rows[i]["ADRESA"] + "</ADRESA>");
                        sw.WriteLine("<OPSTINA_SIFRA_ZS>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA_SIFRA_ZS"] + "</OPSTINA_SIFRA_ZS>");
                        sw.WriteLine("<OPSTINA_SIFRA_PU>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA_SIFRA_PU"] + "</OPSTINA_SIFRA_PU>");
                        sw.WriteLine("<OPSTINA>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA"] + "</OPSTINA>");
                        sw.WriteLine("<TELEFON>" + dsPuni.PD3100Korisnik.Rows[i]["TELEFON"] + "</TELEFON>");
                        sw.WriteLine("<EMAIL>" + dsPuni.PD3100Korisnik.Rows[i]["EMAIL"] + "</EMAIL>");

                        sw.WriteLine("<OBVEZNIK>");
                        sw.WriteLine("<JMB>" + dsPuni.PD3100Korisnik.Rows[i]["JMB"] + "</JMB>");
                        sw.WriteLine("<LIB>" + dsPuni.PD3100Korisnik.Rows[i]["LIB"] + "</LIB>");
                        sw.WriteLine("<PREZIME>" + dsPuni.PD3100Korisnik.Rows[i]["PREZIME"] + "</PREZIME>");
                        sw.WriteLine("<IME>" + dsPuni.PD3100Korisnik.Rows[i]["IME"] + "</IME>");
                        sw.WriteLine("<DJEV_PREZIME>" + dsPuni.PD3100Korisnik.Rows[i]["DJEV_PREZIME"] + "</DJEV_PREZIME>");
                        sw.WriteLine("<DATUM_RODJ>" + dsPuni.PD3100Korisnik.Rows[i]["DATUM_RODJ"] + "</DATUM_RODJ>");
                        sw.WriteLine("<POL_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["POL_SIFRA"] + "</POL_SIFRA>");
                        sw.WriteLine("<POL>" + dsPuni.PD3100Korisnik.Rows[i]["POL"] + "</POL>");
                        sw.WriteLine("<ADRESA>" + dsPuni.PD3100Korisnik.Rows[i]["ADRESA2"] + "</ADRESA>");
                        sw.WriteLine("<OPSTINA>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA3"] + "</OPSTINA>");
                        sw.WriteLine("<OPSTINA_SIFRA_ZS>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA_SIFRA_ZS4"] + "</OPSTINA_SIFRA_ZS>");
                        sw.WriteLine("<OPSTINA_SIFRA_PU>" + dsPuni.PD3100Korisnik.Rows[i]["OPSTINA_SIFRA_PU5"] + "</OPSTINA_SIFRA_PU>");
                        sw.WriteLine("<KONTAKT_ADRESA_ULICA>" + dsPuni.PD3100Korisnik.Rows[i]["KONTAKT_ADRESA_ULICA"] + "</KONTAKT_ADRESA_ULICA>");
                        sw.WriteLine("<KONTAKT_ADRESA_PBROJ>" + dsPuni.PD3100Korisnik.Rows[i]["KONTAKT_ADRESA_PBROJ"] + "</KONTAKT_ADRESA_PBROJ>");
                        sw.WriteLine("<KONTAKT_ADRESA_MJESTO>" + dsPuni.PD3100Korisnik.Rows[i]["KONTAKT_ADRESA_MJESTO"] + "</KONTAKT_ADRESA_MJESTO>");
                        sw.WriteLine("<EMAIL>" + null + "</EMAIL>");
                        sw.WriteLine("<STRUCNA_SPREMA_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["STRUCNA_SPREMA_SIFRA"] + "</STRUCNA_SPREMA_SIFRA>");
                        sw.WriteLine("<STRUCNA_SPREMA>" + dsPuni.PD3100Korisnik.Rows[i]["STRUCNA_SPREMA"] + "</STRUCNA_SPREMA>");
                        sw.WriteLine("<INVALID_PIO_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["INVALID_PIO_SIFRA"] + "</INVALID_PIO_SIFRA>");
                        sw.WriteLine("<INVALID_PIO>" + dsPuni.PD3100Korisnik.Rows[i]["INVALID_PIO"] + "</INVALID_PIO>");
                        sw.WriteLine("<INVALID_ZPR_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["INVALID_ZPR_SIFRA"] + "</INVALID_ZPR_SIFRA>");
                        sw.WriteLine("<INVALID_ZPR>" + dsPuni.PD3100Korisnik.Rows[i]["INVALID_ZPR"] + "</INVALID_ZPR>");

                        sw.WriteLine("<OSNOV_UPLATE_DOPRINOSA>");
                        sw.WriteLine("<DATUM_DOPRINOSA>" + dsPuni.PD3100Korisnik.Rows[i]["DATUM_PROMJENE"] + "</DATUM_DOPRINOSA>");
                        sw.WriteLine("<OSNOV_OBAVEZE_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["OSNOV_OBAVEZE_SIFRA"] + "</OSNOV_OBAVEZE_SIFRA>");
                        sw.WriteLine("<OSNOV_OBAVEZE>" + dsPuni.PD3100Korisnik.Rows[i]["OSNOV_OBAVEZE"] + "</OSNOV_OBAVEZE>");
                        sw.WriteLine("<RAD_VRI_SATI>" + dsPuni.PD3100Korisnik.Rows[i]["RAD_VRI_SATI"] + "</RAD_VRI_SATI>");
                        sw.WriteLine("<RAD_VRI_MINUTE>" + dsPuni.PD3100Korisnik.Rows[i]["RAD_VRI_MINUTE"] + "</RAD_VRI_MINUTE>");
                        sw.WriteLine("<ZANIMANJE_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["ZANIMANJE_SIFRA"] + "</ZANIMANJE_SIFRA>");
                        sw.WriteLine("<ZANIMANJE>" + dsPuni.PD3100Korisnik.Rows[i]["ZANIMANJE"] + "</ZANIMANJE>");
                        sw.WriteLine("<STRUCNA_SPREMA_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["STRUCNA_SPREMA_SIFRA6"] + "</STRUCNA_SPREMA_SIFRA>");
                        sw.WriteLine("<STRUCNA_SPREMA>" + dsPuni.PD3100Korisnik.Rows[i]["STRUCNA_SPREMA7"] + "</STRUCNA_SPREMA>");
                        sw.WriteLine("<DOPUNSKI_RAD_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["DOPUNSKI_RAD_SIFRA"] + "</DOPUNSKI_RAD_SIFRA>");
                        sw.WriteLine("<DOPUNSKI_RAD>" + dsPuni.PD3100Korisnik.Rows[i]["DOPUNSKI_RAD"] + "</DOPUNSKI_RAD>");
                        sw.WriteLine("<BRUTO_PLATA>" + dsPuni.PD3100Korisnik.Rows[i]["BRUTO_PLATA"] + "</BRUTO_PLATA>");
                        sw.WriteLine("<UVECANJE_RMJESTO_SIFRA>" + dsPuni.PD3100Korisnik.Rows[i]["UVECANJE_RMJESTO_SIFRA"] + "</UVECANJE_RMJESTO_SIFRA>");
                        sw.WriteLine("<UVECANJE_STEPEN>" + dsPuni.PD3100Korisnik.Rows[i]["UVECANJE_STEPEN"] + "</UVECANJE_STEPEN>");
                            
                        sw.WriteLine("</OSNOV_UPLATE_DOPRINOSA>");
                        sw.WriteLine("</OBVEZNIK>");
                        sw.WriteLine("</UPLATIOC>");
                        sw.WriteLine("</PRIJAVA>");
                    }

                    sw.WriteLine("</JS_PODACI>");
                    sw.WriteLine("</xml>");
                    sw.Close();

                    Log.Write("===========================================================");
                    Log.Write("=> Generisan je XML '{0}'. Broj slogova je: {1}", nf,count);
                    {
                        QResult hash = HASH.HashFromFile(nf);
                        FileName = hash.Message;
                        string nf1 = nf.Replace(".xml", "_KONTROLA.xml");
                        FileStream fs1 = new FileStream(nf1, FileMode.Append, FileAccess.Write);
                        StreamWriter sw1 = new StreamWriter(fs1);
                        sw1.WriteLine("<?xml version=\"1.0\"?>");
                        sw1.WriteLine("<UCIS xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">");
                        sw1.WriteLine("<KONTROLNI_PODACI>");
                        sw1.WriteLine("<MD5HASH>" + hash.Message + "</MD5HASH>");
                        sw1.WriteLine("<UPLATIOC>");
                        sw1.WriteLine("<JIB>4400411170007</JIB>");
                        sw1.WriteLine("<BROJ_OBVEZNIKA>" + count.ToString() + "</BROJ_OBVEZNIKA>");
                        sw1.WriteLine("</UPLATIOC>");
                        sw1.WriteLine("</KONTROLNI_PODACI>");
                        sw1.WriteLine("<DODATNI_PODACI>");
                        sw1.WriteLine("<UPLATIOC_DETALJ>");
                        sw1.WriteLine("<JIB>4400411170007</JIB>");
                        sw1.WriteLine("<NAZIV_OBVEZNIKA>ФОНД ПИО РС</NAZIV_OBVEZNIKA>");
                        sw1.WriteLine("<ADRESA>ЊЕГОШЕВА 28 А</ADRESA>");
                        sw1.WriteLine("<MJESTO>БИЈЕЉИНА</MJESTO>");
                        sw1.WriteLine("<OPSTINA_SIFRA_ZS>10057</OPSTINA_SIFRA_ZS>");
                        sw1.WriteLine("<OPSTINA_SIFRA_PU>005</OPSTINA_SIFRA_PU>");
                        sw1.WriteLine("<OPSTINA>БИЈЕЉИНА</OPSTINA>");
                        sw1.WriteLine("<TELEFON>055-202-937</TELEFON>");
                        sw1.WriteLine("<EMAIL>info@fondpiors.org</EMAIL>");
                        sw1.WriteLine("<DATUM>" + DateTime.Today.ToString("dd.MM.yyyy") + "</DATUM>");
                        sw1.WriteLine("<DJELATNOST_SIFRA></DJELATNOST_SIFRA>");
                        sw1.WriteLine("<DJELATNOST></DJELATNOST>");
                        sw1.WriteLine("<VRSTA_UPLATIOCA_SIFRA></VRSTA_UPLATIOCA_SIFRA>");
                        sw1.WriteLine("<VRSTA_UPLATIOCA></VRSTA_UPLATIOCA>");
                        sw1.WriteLine("</UPLATIOC_DETALJ>");
                        sw1.WriteLine("</DODATNI_PODACI>");
                        sw1.WriteLine("</UCIS>");
                        sw1.Close();

                        Log.Write("===========================================================");
                        Log.Write("=> Generisan je kontrolni XML '{0}'", nf1);
                    }

                    poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Uspješno je generisan KONTROLNI fajl";
                    poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " =>  Broj redova koji je eksportovan u XML je " + count;
                    File.Copy(nf.Replace(".xml", "_KONTROLA.xml"), path + NazivExport.Replace(".xml", "_KONTROLA.xml"));
                    Log.Write("===========================================================");
                    Log.Write("=> XML fajl je kopiran na lokaciju '{0}'", path + NazivExport.Replace(".xml", "_KONTROLA.xml"));
                    File.Copy(nf, path + NazivExport);
                    Log.Write("===========================================================");
                    Log.Write("=> XML kontrolni fajl je kopiran na lokaciju '{0}'", path + NazivExport);
                    //string appPath = Path.GetDirectoryName(Application.ExecutablePath); 
                }
            }
            catch (Exception ex)
            {
                exception = true;
                Log.Write("->::: GREŠKA ::: Generisanje XML fajla {0} je prekinuto: {1}!", nf, ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Greška kod generisanja XML-a " + ex.Message;
                //throw;--komentar poslednje sto sam komentarisao
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                if (exception)
                {
                    email.SendEmail(poruka);
                    Application.Exit();
                }
            }
        }
    #endregion
    #region import
        public void impFile()
        {
            string pathImp = "C:\\PD3100RazmjenaPU\\ArchiveFolder\\Import\\";
            if (!Directory.Exists(pathImp))
                Directory.CreateDirectory(pathImp);
            string SearchFolder = "C:\\razmjena\\import\\";
            string SearchPattern = "*PIORS_PURS_PD3100_REZULTAT.xml";
            DirectoryInfo di = new DirectoryInfo(SearchFolder);
            conn.Open();
            try
            {
                foreach (FileInfo file in di.GetFiles(SearchPattern))
                {
                    SqlCommand komanda = new SqlCommand("likvidatura.ProvjeraImportFajla", conn);
                    komanda.CommandTimeout = 0;
                    komanda.CommandType = CommandType.StoredProcedure;
                    komanda.Parameters.Add("@nazivFajla", SqlDbType.VarChar).Value = file.Name;
                    bool pogodak = (bool) komanda.ExecuteScalar();
                    //komanda.ExecuteNonQuery();
                    if (pogodak==true)
                    {
                        Log.Write("===========================================================");
                        Log.Write("=> Počinje import fajla '{0}'", file.Name);
                        ImportData(file.FullName, file.Name);
                        Log.Write("===========================================================");
                        Log.Write("=> Završen je import fajla '{0}'", file.Name);
                        //File.SetAttributes(file.FullName, FileAttributes.Hidden);
                        File.Copy(file.FullName, pathImp + file.Name);
                        Log.Write("===========================================================");
                        Log.Write("=> Fajl je kopiran na lokaciju '{0}'", pathImp + file.Name);                            
                    }
                }
            }
            catch (Exception ex)
            {
                exception = true;
                //MessageBox.Show(ex.Message);
                Log.Write("->::: GREŠKA ::: Import XML fajla je prekinuto: GREŠKA: {0}", ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Greška kod importa XML-a: " + ex.Message;
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        public void ImportData(string xmlData,string name)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlData);
            XmlNodeList xnList = doc.SelectNodes("/XML/GRESKA");
            int countList = xnList.Count;
            int countUpis = 0;
            try
            {
                foreach (XmlNode xn in xnList)
                {
                    string piobroj = "0" + xn["BARKOD"].InnerText;
                    string jmbg = xn["JMB"].InnerText;
                    int greska = Int32.Parse(xn["GRESKA_SIFRA"].InnerText);
                    int tip = 0;
                    string datumObrade = DateTime.Today.ToString("dd.MM.yyyy");
                    string nazivFajla = name;
                    string napomena = null;
                    upis(piobroj, jmbg, greska, tip, datumObrade, name, napomena);
                    countUpis++;
                }
                Log.Write("===========================================================");
                Log.Write("=> Broj slogova za import je ==> {0} <==. Importovano je ==> {1} <== slogova.", countList,countUpis);
                upisUSinisaPD3100(name.Substring(0, 11));
                //txtCount.Text = countUpis.ToString();
                //MessageBox.Show("Import je uspješno izvršen." + "\r\n" + "Broj slogova u listi za import je: " + countList + "\r\n" + "Importovano je slogova: " + countUpis);
            }

            catch (Exception ex)
            {
                exception = true;
                //MessageBox.Show(ex.Message);
                Log.Write("->::: GREŠKA ::: Import XML fajla je prekinuto: GREŠKA: {0}", ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Greška kod importa XML-a: " + ex.Message;
            }
        }
        public void upis(string parPiobr, string parJMB, int parGreska, int parTip, string parDatumObrade, string parNazivFajla, string parNapomena)
        {
            try
            {
                //conn.Open();
                SqlCommand komanda = new SqlCommand("likvidatura.upisPD3100_GRESKA", conn);
                komanda.CommandTimeout = 0;
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.Parameters.Add("@piobr", SqlDbType.VarChar).Value = parPiobr;
                komanda.Parameters.Add("@jmbg", SqlDbType.VarChar).Value = parJMB;
                komanda.Parameters.Add("@greska", SqlDbType.TinyInt).Value = parGreska;
                komanda.Parameters.Add("@tip", SqlDbType.TinyInt).Value = parTip;
                komanda.Parameters.Add("@datumObrade", SqlDbType.Date).Value = parDatumObrade;
                komanda.Parameters.Add("@nazivFajla", SqlDbType.VarChar).Value = parNazivFajla;
                komanda.Parameters.Add("@napomena", SqlDbType.VarChar).Value = parNapomena;
                komanda.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                exception = true;
                //MessageBox.Show(ex.Message);
                Log.Write("->::: GREŠKA ::: Upis grešaka je prekinut. SP 'likvidatura.upisPD3100_GRESKA'. GREŠKA: {0}", ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Upis grešaka je prekinut. SP 'likvidatura.upisPD3100_GRESKA'. GREŠKA: " + ex.Message;
            }
        }
        public void upisUSinisaPD3100(string parDatum)
        {
            try
            {
                //conn.Open();
                SqlCommand komanda = new SqlCommand("likvidatura.import2SinisaPD3100", conn);
                komanda.CommandTimeout = 0;
                komanda.CommandType = CommandType.StoredProcedure;
                komanda.Parameters.Add("@datum", SqlDbType.Char).Value = parDatum;
                komanda.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                exception = true;
                //MessageBox.Show(ex.Message);
                Log.Write("->::: GREŠKA ::: Upis grešaka je prekinut. SP 'likvidatura.import2SinisaPD3100'. GREŠKA: {0}", ex.ToString());
                poruka = poruka + "\r\n" + DateTime.Now.ToString("H:mm:ss") + " => Upis grešaka je prekinut. SP 'likvidatura.import2SinisaPD3100'. GREŠKA: " + ex.Message;
            }
        }
    #endregion
    }
}
