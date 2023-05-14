using Bank.Core.Authorization;
using MailKit.Net.Smtp;
using MimeKit;

namespace Bank.Core.Emailer
{
    public static class EmailAction
    {
        public static bool sendEmail(string uniqueId, string recipient)
        {

            EmailClass ec = new EmailClass();
            ec.setClass();


            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Stin Bank app", ec.email));
            message.To.Add(new MailboxAddress("Recipient Name", recipient));
            message.Subject = "Test";
            message.Body = new TextPart("plain") { Text = "Your code is: " + uniqueId + ", it will expire in 10 minutes." };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, false);
                    client.Authenticate(ec.email, ec.password);

                    // Send the email
                    client.Send(message);

                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
    }
}
