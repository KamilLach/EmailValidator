using System;
using System.Windows.Forms;
using Autofac;

namespace EmailChecker.SampleGui
{
   static class Program
   {
      /// <summary>
      /// The main entry point for the application.
      /// </summary>
      [STAThread]
      static void Main()
      {
         IContainer container = AutofacRegistration();
         Application.EnableVisualStyles();
         Application.SetCompatibleTextRenderingDefault(false);
         Application.Run(container.Resolve<Form>());
      }

      /// <summary>
      /// Build container
      /// </summary>
      /// <returns>Container</returns>
      private static IContainer AutofacRegistration()
      {
         ContainerBuilder builder = new ContainerBuilder();
         builder.RegisterType<SmtpEmail>().AsImplementedInterfaces();
         builder.RegisterType<TcpSmtpVerifier>().AsImplementedInterfaces();
         builder.RegisterType<DnsMxResolver>().AsImplementedInterfaces();
         builder.RegisterType<DynDnsHostNameProvider>().AsImplementedInterfaces();
         builder.RegisterType<MainForm>().As<Form>();
         return builder.Build();
      }
   }
}
