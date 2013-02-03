using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Windows.Forms;
using EmailChecker.SampleGui.Properties;

namespace EmailChecker.SampleGui
{
   public partial class MainForm : Form
   {
      #region Fields

      private readonly ISmtpVerifier m_smtpVerifier;
      private readonly Func<string, ISmtpEmail> m_smtpEmail;
      private readonly IMxResolver m_mxResolver;

      #endregion

      #region Constructors

      public MainForm()
      {
         InitializeComponent();
      }

      public MainForm(ISmtpVerifier a_smtpVerifier, Func<string, ISmtpEmail> a_smtpEmail, IMxResolver a_mxResolver)
         : this()
      {
         m_smtpVerifier = a_smtpVerifier;
         m_smtpEmail = a_smtpEmail;
         m_mxResolver = a_mxResolver;
      }

      #endregion

      #region Event handlers

      private void OnCheckEmailClick(object a_sender, EventArgs a_e)
      {
         var smtpEmail = m_smtpEmail.Invoke(textBox1.Text);
         if (smtpEmail.Validate() != SmtpEmailStatus.Ok)
         {
            MessageBox.Show(Resources.EmailIsNotValid, Resources.Info, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
         }

         MailAddress mailAddress = smtpEmail.Parse();
         Tuple<IList<string>, string> result = m_mxResolver.Resolve(mailAddress.Host);

         foreach (string host in result.Item1)
         {
            Tuple<SmtpVerifyStatus, string> status
               = m_smtpVerifier.Verify(host, mailAddress.Address);
            if (status.Item1 == SmtpVerifyStatus.Ok)
            {
               MessageBox.Show(Resources.EmailIsValid, Resources.Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
               return;
            }

            if (status.Item1 == SmtpVerifyStatus.UserNotExist)
            {
               MessageBox.Show(string.Format(Resources.EmailIsNotValidDetails, Environment.NewLine, status.Item2),
                               Resources.Info, MessageBoxButtons.OK, MessageBoxIcon.Warning);
               return;
            }
         }

         MessageBox.Show(Resources.EmailIsNotValid, Resources.Info, MessageBoxButtons.OK, MessageBoxIcon.Warning);
      }

      #endregion
   }
}
