using System;
using System.Collections.Generic;

namespace EmailChecker
{
   public interface IMxResolver
   {
      /// <summary>
      /// Resolve Mx address
      /// </summary>
      /// <param name="a_host">Host</param>
      /// <returns>List of Mx address</returns>
      Tuple<IList<string>, string> Resolve(string a_host);
   }
}