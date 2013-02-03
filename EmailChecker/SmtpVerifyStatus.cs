namespace EmailChecker
{
   public enum SmtpVerifyStatus
   {
      /// <summary>
      /// Veryfing status
      /// </summary>
      Ok,

      /// <summary>
      /// Unable to connect to SMTP
      /// </summary>
      UnableToConnect,

      /// <summary>
      /// User not exist in server database
      /// </summary>
      UserNotExist,

      /// <summary>
      /// Able to connect but, server return code other than 250
      /// </summary>
      ServerUnreliable,

      /// <summary>
      /// Unable to connect to Stmp server
      /// </summary>
      ServerNotExist
   }
}