namespace EmailChecker
{
   public interface IHostNameProvider
   {
      /// <summary>
      /// Pobranie publicznej nazwy host'a
      /// </summary>
      /// <returns></returns>
      string GetHostName();
   }
}