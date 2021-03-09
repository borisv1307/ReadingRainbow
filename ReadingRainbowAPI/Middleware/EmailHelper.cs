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
        Task<bool> SendEmail(string userName, string userEmail, string body, string subject);
        string ConfirmationLinkBody(Person person, string callBackUrl);
        string ConfirmationLinkSubject();
        string ResetPasswordSubject();
        string ResetPasswordBody(Person person, string passwordText, string expirateDateText);
        string ChangePasswordSubject();
        string ChangePasswordBody(Person person);
      
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
        
        public async Task<bool> SendEmail(string userName, string userEmail, string body, string subject)
        {
            try  
            {  
                //From Address    
                string FromAddress = "readingrainbow@se691.edu";  
                string FromAdressTitle = "Reading Rainbow";  
                //To Address    
                string ToAddress = userEmail;  
                string ToAdressTitle = userName;  
                string Subject = subject; 
                string BodyContent = body;  
  
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

        public string ConfirmationLinkBody(Person person, string callBackUrl)
        {
            if (string.IsNullOrEmpty(callBackUrl))
            {
                callBackUrl = "https://localhost:5001/api/email/AddPerson";
            }

            callBackUrl = callBackUrl + "/" + HttpUtility.UrlEncode(person.Token) + "/" + HttpUtility.UrlEncode(person.Name);
            
            return ($"Please confirm your account by clicking this link: <a href='{callBackUrl}'>Email Confirmation Link</a>");
        }

        public string ConfirmationLinkSubject()
        {
            return "Confirm your email for access to Complete Reading Rainbow Sign up";
        }

        public string ResetPasswordSubject()
        {
            return "Reading Rainbow Reset Password Request";
        }
        
        public string ResetPasswordBody(Person person, string passwordText, string expirateDateText)
        {
            var resetStr = $"User's {person.Name} temporary password has been set to {passwordText}. \n" +
                $"You have {expirateDateText} hours to log into the Reading Rainbow Application using this password. \n" +
                "You will be required to change this password right after login.";
            
            return resetStr;
        }

        public string ChangePasswordSubject()
        {
            return "Password Change for Reading Rainbow Application";
        }

        public string ChangePasswordBody(Person person)
        {
            return $"User {person.Name} has just changed the password on the reading rainbow account";
        }
    }
}