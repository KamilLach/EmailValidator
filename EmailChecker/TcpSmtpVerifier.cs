using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using EmailChecker.Properties;

namespace EmailChecker
{
   /// <summary>
   /// Tcp implementation of verifier
   /// </summary>
   public class TcpSmtpVerifier : ISmtpVerifier
   {
      private readonly IHostNameProvider m_hostNameProvider;

      #region Const

      private const string CRLF = "\r\n";

      #endregion

      /// <summary>
      /// Initializes a new instance of the <see cref="T:System.Object"/> class.
      /// </summary>
      public TcpSmtpVerifier(IHostNameProvider a_hostNameProvider)
      {
         if (a_hostNameProvider == null)
         {
            throw new ArgumentNullException("a_hostNameProvider");
         }

         m_hostNameProvider = a_hostNameProvider;
      }

      #region Private methods

      /// <summary>
      /// Converts given string to byte arr.
      /// </summary>
      /// <param name="a_string">String to convert</param>
      /// <returns>Converted string</returns>
      private byte[] BytesFromString(string a_string)
      {
         return Encoding.ASCII.GetBytes(a_string);
      }

      /// <summary>
      /// Get code from response
      /// </summary>
      /// <param name="a_responseString">Response string</param>
      /// <returns>Response code</returns>
      private int GetResponseCode(string a_responseString)
      {
         return int.Parse(a_responseString.Substring(0, 3));
      }

      #endregion

      #region Implementation of ISmtpVerifier

      /// <summary>
      /// Verify
      /// </summary>
      /// <param name="a_smtpHost">Smtp host</param>
      /// <param name="a_emailAddress">Email address to veryfi</param>
      /// <returns>Status and message optional</returns>
      public Tuple<SmtpVerifyStatus, string> Verify(string a_smtpHost, string a_emailAddress)
      {
         try
         {
            string hostName = m_hostNameProvider.GetHostName();
            TcpClient smtpTest = new TcpClient();
            smtpTest.Connect(a_smtpHost, 25);
            if (!smtpTest.Connected)
            {
               return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.ServerNotExist,
                                                          Resource.UnableToConnectToSmtpServer);
            }

            using (NetworkStream ns = smtpTest.GetStream())
            {
               using (StreamReader clearTextReader = new StreamReader(ns))
               {
                  using (StreamWriter clearTextWriter = new StreamWriter(ns) { AutoFlush = true })
                  {
                     var responseConnected = clearTextReader.ReadLine();
                     if (GetResponseCode(responseConnected) != 220)
                     {
                        return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.UnableToConnect, responseConnected);
                     }

                     clearTextWriter.WriteLine(string.Format("HELO {0}", hostName));
                     var responseHello = clearTextReader.ReadLine();
                     if (GetResponseCode(responseHello) != 250)
                     {
                        return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.UnableToConnect, responseHello);
                     }

                     clearTextWriter.WriteLine(string.Format("MAIL FROM: <check@{0}>", hostName));
                     var responseMailFrom = clearTextReader.ReadLine();
                     if (GetResponseCode(responseMailFrom) != 250)
                     {
                        return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.UnableToConnect, responseMailFrom);
                     }

                     clearTextWriter.WriteLine(string.Format("RCPT TO: <{0}>", a_emailAddress));
                     var responseRcptTo = clearTextReader.ReadLine();
                     int statusCode = GetResponseCode(responseRcptTo);

                     clearTextWriter.WriteLine(string.Format("QUITE"));
                     smtpTest.Close();

                     switch (statusCode)
                     {
                        case 250:
                           return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.Ok, responseRcptTo);
                        case 550:
                           return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.UserNotExist, responseRcptTo);
                        default:
                           return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.ServerUnreliable, responseRcptTo);
                     }
                  }
               }
            }
         }
         catch (Exception exception)
         {
            return new Tuple<SmtpVerifyStatus, string>(SmtpVerifyStatus.ServerNotExist, exception.ToString());
         }
      }

      #endregion
   }
}