using HalcyonApparelsMVC.Interfaces;
using HalcyonApparelsMVC.Models;
using System.Net;
using System.Net.Mail;

namespace HalcyonApparelsMVC.Services
{
    public class MailSender : IMailSender
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _config;
        public MailSender(IServiceProvider serviceProvider, IConfiguration config)
        {
            _serviceProvider = serviceProvider;
            _config = config;
        }

        //private bool SendEmail(string recepientEmail)
        private bool SendEmail(MarketingList mlist)
        {

            string HostAdd = _config.GetSection("Send")["ServerName"];
            string FromEmailid = _config.GetSection("Send")["FromEmail"];
            var mailPassword = _config.GetSection("Send")["APIKey"];
            string SMTPPort = _config.GetSection("Send")["Port"];

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid);
            mailMessage.Subject = "Grab the Offer!";

            mailMessage.IsBodyHtml = true;

            if ((mlist.Product_Type__c == "shirt") || (mlist.Product_Type__c == "Pant") || (mlist.Product_Type__c == "Jeans") || (mlist.Product_Type__c == "T-Shirt") || (mlist.Product_Type__c == "Jackets") || (mlist.Product_Type__c == "Shorts"))
            {
                mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/Watches.cshtml");

            }
            else if ((mlist.Product_Type__c == "kurthis") || (mlist.Product_Type__c == "Top") || (mlist.Product_Type__c == "Churidar") || (mlist.Product_Type__c == "Saree") || (mlist.Product_Type__c == "Skirts"))

            {
                mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/bag.cshtml");

            }
            else if( (mlist.Product_Type__c == "Frock")|| (mlist.Product_Type__c == "Dungaree"))

            {
                mailMessage.Body = System.IO.File.ReadAllText($"{Directory.GetCurrentDirectory()}/wwwroot/emails/Toys.cshtml");

            }
                {
                    mailMessage.To.Add(new MailAddress(mlist.Email));
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = HostAdd;
                smtp.EnableSsl = true;
                smtp.Port = Convert.ToInt32(SMTPPort);
                smtp.Credentials = new NetworkCredential(FromEmailid, mailPassword);
                smtp.Send(mailMessage);
             
            
            return true;
        }
       

        //public async void SendBulkMail(IEnumerable<string> recepientEmails)
        public async void SendBulkMail(List<MarketingList> mlist)
        {
            foreach (var mailid in mlist)
            {
                SendEmail(mailid);

            }
            //SendEmail(mlist);

        }
    }
}