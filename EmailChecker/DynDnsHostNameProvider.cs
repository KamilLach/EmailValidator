using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace EmailChecker
{
   public class DynDnsHostNameProvider : IHostNameProvider
   {
      private string m_hostName;

      #region Implementation of IHostNameProvider

      /// <summary>
      /// Pobranie publicznej nazwy host'a
      /// </summary>
      /// <returns></returns>
      public string GetHostName()
      {
         if (!string.IsNullOrEmpty(m_hostName))
         {
            return m_hostName;
         }

         try
         {
            using (WebClient webClient = new WebClient())
            {
               var data = webClient.OpenRead("http://checkip.dyndns.org");
               using (StreamReader reader = new StreamReader(data))
               {
                  Regex regex =
                     new Regex(
                        @"(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])");
                  var match = regex.Match(reader.ReadToEnd());
                  if (match.Success)
                  {
                     var hostEntry = Dns.GetHostEntry(match.Value);
                     m_hostName = hostEntry.HostName;
                     return m_hostName;
                  }
               }
            }
         } 
         catch
         {
            return "email-veryfier.org";
         }

         return string.Empty;
      }

      #endregion
   }
}