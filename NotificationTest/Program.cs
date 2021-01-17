using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Mail;

namespace NotificationTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\n*******SampleApp*******\n\t-Login-");
            while(true){
                Console.WriteLine("Username(E-mail): "); //enter e-mail for sample app login
                string email = Console.ReadLine();
                Console.WriteLine("Password: "); //any password for this sample app (12345)
                var password = Console.ReadLine();

                if(email == "xxxxxx@xxxx.com" && password == "12345"){ //e-mail and password registered with the this app (enter your e-mail)
                    Console.WriteLine($"\nWelcome {email} to SampleApp.");
                    SendMail(email); //send notification
                    break;
                }
                else{
                    Console.WriteLine("Username or password is wrong! Try again...");
                }
            }
    
            Console.Write("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        public static void SendMail(string email){
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com"; //or smtp.live.com (microsoft)
            smtp.Port = 587;

            smtp.Credentials = new NetworkCredential(
            "sample@gmail.com", "Password"); //send mail from this gmail (or live) account
            smtp.EnableSsl = true;

            MailAddress to = new MailAddress(email); //account (your e-mail) to be notified
            MailAddress from = new MailAddress("sample@gmail.com");

            MailMessage mail = new MailMessage(from, to);

            // Set subject and encoding
            mail.Subject = "Login Alert⚠";
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            
            // Get geolocation json format from ipinfo.io
            IpInfo ipInfo = new IpInfo();
            string info = new WebClient().DownloadString("https://ipinfo.io/");
            ipInfo = JsonConvert.DeserializeObject<IpInfo>(info);

            var date = DateTime.Now;

            // Set body-message and encoding
            mail.Body = "<b>Was that you?</b><br>You have logged into the Sample App with "
                        + email + " account on " + date.ToShortDateString() + " at " + date.ToShortTimeString()
                        + ".<br><b>IP: </b>" + ipInfo.Ip + "<br><b>Location: </b>" + ipInfo.City + "/" + ipInfo.Region + ", " + ipInfo.Country
                        + "<br><b>Timezone: </b>" + ipInfo.Timezone;

            mail.BodyEncoding = System.Text.Encoding.UTF8;
            // text or html
            mail.IsBodyHtml = true;

            //Console.WriteLine("Sending email...");
            smtp.Send(mail);
        }
    }
}
