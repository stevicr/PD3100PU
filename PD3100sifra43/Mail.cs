using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;

namespace PD3100sifra43
{
    class Mail
    {
        public void SendEmail(string poruka)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.teol.net");
                mail.From = new MailAddress("radenko.stevic@fondpiors.org");
                mail.To.Add("radenko.stevic@hotmail.com");
                mail.Subject = "Razmjena podataka sa PU RS";
                mail.Body = poruka;
                //SmtpServer.Port = 110;
                //SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.Credentials = new System.Net.NetworkCredential("radenko.stevic@fondpiors.org", "Pegla00");
                SmtpServer.EnableSsl = true;
                Log.Write("===========================================================");
                Log.Write("::: Generisanje emaila {0}", "");
                SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");

                //MailMessage mail = new MailMessage();
                //SmtpClient SmtpServer = new SmtpClient("smtp.teol.net");

                //mail.From = new MailAddress("radenko.stevic@hotmail.com");
                //mail.To.Add("radenko.stevic@fondpiors.org");
                //mail.Subject = "Test Mail";
                //mail.Body = "This is for testing SMTP mail from GMAIL";

                //SmtpServer.Credentials = new System.Net.NetworkCredential("radenko.stevic", "Pegla00");
                //SmtpServer.EnableSsl = true;

                //SmtpServer.Send(mail);
                //MessageBox.Show("mail Send");

            }
            catch (Exception ex)
            {
                Log.Write("===========================================================");
                Log.Write("->::: GREŠKA ::: Generisanje emaila je prekinuto {1}", ex.ToString());
            }
        }
    }
}
