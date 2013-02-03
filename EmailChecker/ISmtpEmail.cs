using System.Net.Mail;

namespace EmailChecker
{
   /// <summary>
   /// Stmp email, manages given email's
   /// </summary>
   public interface ISmtpEmail
   {
      /// <summary>
      /// Validates given email
      /// </summary>
      /// <returns>Verify status</returns>
      SmtpEmailStatus Validate();

      /// <summary>
      /// Parse string email into MailAddress instance
      /// </summary>
      /// <returns>MailAddress instance</returns>
      MailAddress Parse();
   }
}