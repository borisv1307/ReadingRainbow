using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;  
using ReadingRainbowAPI.Models;
using System.Net;
using System.Web;
 
namespace ReadingRainbowAPI.Middleware
{
    public interface IEmailHelper
    {
        Task<bool> SendEmail(string userName, string userEmail, string confirmationLink);
        string GenerateEmailLink(Person person, string callBackUrl);
      
    }

    public class EmailHelper : IEmailHelper
    {
        private string _emailUser;  
        private string _emailSecret;

        public EmailHelper(IConfiguration config)
        {
            _emailSecret = config.GetSection("Email").GetSection("secret").Value;  
            _emailUser = config.GetSection("Email").GetSection("User").Value;  
        }
        
        public async Task<bool> SendEmail(string userName, string userEmail, string confirmationLink)
        {
            try  
            {  
                //From Address    
                string FromAddress = "readingrainbow@se691.edu";  
                string FromAdressTitle = "Reading Rainbow";  
                //To Address    
                string ToAddress = userEmail;  
                string ToAdressTitle = userName;  
                string Subject = "Confirm your email for access to Complete Reading Rainbow Sign up"; 
                string BodyContent = confirmationLink;  
  
                //Smtp Server    
                string SmtpServer = "smtp.gmail.com";
                //Smtp Port Number    
                int SmtpPortNumber = 587;  
  
                var mimeMessage = new MimeMessage();  
                mimeMessage.From.Add(new MailboxAddress  
                                        (FromAdressTitle,   
                                         FromAddress  
                                         ));  
                mimeMessage.To.Add(new MailboxAddress  
                                         (ToAdressTitle,  
                                         ToAddress  
                                         ));  
                mimeMessage.Subject = Subject; //Subject  
                mimeMessage.Body = new TextPart("Html")  
                {  
                    Text = BodyContent  
                };  
  
                using (var client = new SmtpClient())  
                {  
                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Connect(SmtpServer, SmtpPortNumber, false);  
                    client.Authenticate(  
                        _emailUser,   // Gmail name 
                        _emailSecret  //gmail password
                        );  
                   await client.SendAsync(mimeMessage);  
                    Console.WriteLine("The mail has been sent successfully !!");  
                   await client.DisconnectAsync(true);  
                }  

            }  
            catch (Exception ex)  
            {  
                Console.WriteLine($"Exception {ex} occured when sending Email");  
                return false;
            }    
            return true;
        }

        public string GenerateEmailLink(Person person, string callBackUrl)
        {
            if (string.IsNullOrEmpty(callBackUrl))
            {
                callBackUrl = "https://localhost:5001/api/email/AddPerson";
            }

            callBackUrl = callBackUrl + "/" + HttpUtility.UrlEncode(person.Token) + "/" + HttpUtility.UrlEncode(person.Name);
            
            return ($"Please confirm your account by clicking this link: <a href='{callBackUrl}'>link</a>");
        }
   }
}