using System.Net.Mail;

namespace EmailChecker
{
   public class SmtpEmail : ISmtpEmail
   {
      private readonly string m_email;

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Object"/> class.
      /// </summary>
      public SmtpEmail(string a_email)
      {
         m_email = a_email;
      }

      #region Implementation of ISmtpEmail

      /// <summary>
      /// Validates given email
      /// </summary>
      /// <returns>Verify status</returns>
      public SmtpEmailStatus Validate()
      {
         try
         {
            new MailAddress(m_email);
            return SmtpEmailStatus.Ok;
         }
         catch
         {
            return SmtpEmailStatus.BadEmail;
         }
      }

      /// <summary>
      /// Parse string email into MailAddress instance
      /// </summary>
      /// <returns>MailAddress instance</returns>
      public MailAddress Parse()
      {
         return new MailAddress(m_email);
      }

      #endregion
   }
}