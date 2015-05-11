using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PD3100sifra43
{
    public class HASH
    {
        //public static data_layer.QResult checkHash(string fileName)
        //{
        //    data_layer.QResult res = new data_layer.QResult();
        //    string controlFile = fileName.Replace(".xml", "_KONTROLA.xml");
        //    XmlDocument doc = new XmlDocument();
        //    try
        //    {
        //        doc.Load(controlFile);
        //        res = HashFromFile(fileName);
        //        if (res.ID == -1)
        //        {
        //            throw new Exception(res.Message);
        //        }
        //        if (doc.DocumentElement.SelectSingleNode("KONTROLNI_PODACI/MD5HASH").InnerText != res.Message)
        //        {
        //            res.ID = -1;
        //            res.Message = "HASH fajla sa podacima nije ispravan!";
        //        }
        //        else
        //        {
        //            res.ID = 1;
        //            res.Message = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ID = -1;
        //        res.Message = ex.Message;
        //    }

        //    return res;
        //}
        public static QResult HashFromFile(string fileName)
        {
            QResult res = new QResult();
            if (!File.Exists(Path.GetFullPath(fileName)))
            {
                res.Message = String.Format("Fajl sa imenom '{0}' ne postoji!", fileName);
                res.ID = -1;
            }
            string txt = File.ReadAllText(Path.GetFullPath(fileName));
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(txt);
            bs = x.ComputeHash(bs);
            System.Text.StringBuilder s = new System.Text.StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            res.Message = password;
            res.ID = 1;

            return res;
        }
    }
}
