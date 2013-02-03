using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using JHSoftware;

namespace EmailChecker
{
   public class DnsMxResolver : IMxResolver
   {
      #region Implementation of IMxResolver

      /// <summary>
      /// Resolve Mx address
      /// </summary>
      /// <param name="a_host">Host</param>
      /// <returns>List of Mx address</returns>
      public Tuple<IList<string>, string> Resolve(string a_host)
      {
         IList<string> results = new List<string>();
         DnsClient.MXHost[] mxCollection;
         try
         {
            mxCollection = DnsClient.LookupMX(a_host);
         } 
         catch (Exception exception)
         {
            return new Tuple<IList<string>, string>(results, exception.ToString());
         }

         foreach (var row in mxCollection.OrderBy(a_c => a_c.Preference))
         {
            if (row.IPAddresses != null)
            {
               foreach (IPAddress ipAddress in row.IPAddresses)
               {
                  results.Add(ipAddress.ToString());
               }
            }
         }

         return new Tuple<IList<string>, string>(results, string.Empty);
      }

      #endregion
   }
}