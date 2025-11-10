using System.Net.Mail;
using System.Net;

namespace RestaurantPMS.Service
{
	public class EmailSender : IEmailSender
	{
		public Task SendEmailAsync(string mails, string pw, string email, string subject, string message)
		{


			// Set up the SMTP client
			SmtpClient smtp = new SmtpClient("smtp.gmail.com");
			smtp.Port = 587;
			smtp.EnableSsl = true;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential(mails, pw);
			// Send the email



			return smtp.SendMailAsync(
		   new MailMessage(from: mails,
							to: email,
						   subject,
						   message
						   ));



		}



	}
}
