using System;

namespace EmailChecker
{
   public interface ISmtpVerifier
   {
      /// <summary>
      /// Verify
      /// </summary>
      /// <param name="a_smtpHost">Smtp host</param>
      /// <param name="a_emailAddress">Email address to veryfi</param>
      /// <returns>Status and message optional</returns>
      Tuple<SmtpVerifyStatus, string> Verify(string a_smtpHost, string a_emailAddress);
   }
}