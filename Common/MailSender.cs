using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;


namespace Common
{
    /// <summary>
    /// Send email message
    /// </summary>
    public class MailSender
    {
        #region Fields

        private NetworkCredential credential;
        private MailMessage mailMessage;

        #endregion Fields

        #region Properties

        private StringBuilder ClassInfo
        {
            get
            {
                return new StringBuilder(string.Format("{0}.{1}", Assembly.GetExecutingAssembly().GetName().Name, this.GetType()));
            }
        }

        /// <summary>
        /// Port for SMTP server
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Address of the sender of the e-mail message
        /// </summary>
        public string From
        {
            get
            {
                return mailMessage.From.Address;
            }
        }

        /// <summary>
        /// Address of the recipient of the e-mail  message
        /// </summary>
        public string To
        {
            get
            {
                return mailMessage.To[0].Address;
            }
        }

        /// <summary>
        /// Subject text of the e-mail  message
        /// </summary>
        public string Subject
        {
            get
            {
                return mailMessage.Subject;
            }
        }

        /// <summary>
        /// Body of the e-mail  message
        /// </summary>
        public string Body
        {
            get
            {
                return mailMessage.Body;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialize mail message with necessary parameters
        /// </summary>
        /// <param name="from">Address of the sender of the e-mail message</param>
        /// <param name="to">Address of the recipient of the e-mail  message</param>
        /// <param name="subject">Subject text of the e-mail  message</param>
        /// <param name="body">Body of the e-mail  message</param>
        public void InitializeMailMessage(string from, string to, string subject, string body)
        {
            mailMessage = new MailMessage(from, to, subject, CreateBody(from, body));
        }

        /// <summary>
        /// Create email body
        /// </summary>
        /// <param name="from">Address of the sender of the e-mail message</param>
        /// <param name="body">Body of the e-mail  message</param>
        private string CreateBody(string from, string body)
        {
            return "\nEmail Address: " + from + "\nMessage: " + @body;
        }

        /// <summary>
        /// Set credentials to authenticate the client
        /// </summary>
        /// <param name="userName">User name associated with the credentials</param>
        /// <param name="password">Password for the user name associated with the credentials</param>
        public void SetCredential(string userName, string password)
        {
            credential = new NetworkCredential(userName, password);
        }

        /// <summary>
        /// Send mail message using specified SMTP server and Port
        /// </summary>
        /// <param name="server">Name of the host used for SMTP transactions</param>
        /// <param name="port">Port to be used on host</param>
        /// <param name="isAsync">Indicates whether sending to SMTP server is asynchronous operation</param>
        public void SendSmtp(string server, int port, bool isAsync)
        {
            var client = new SmtpClient(server);

            Port = port;
            client.Port = port;

            Debug.WriteLine("MailSender Host - Server: {0} ; Port: {1} ; IsAsync: {2}", server, port, isAsync);

            SendSmtp(client, isAsync);
        }

        /// <summary>
        /// Send mail message using Server and Port from configuration file
        /// </summary>
        /// <param name="isAsync">Indicates whether sending to SMTP server is asynchronous operation</param>
        public void SendSmtp(bool isAsync)
        {
            var client = new SmtpClient();

            SendSmtp(client, isAsync);
        }

        /// <summary>
        /// Send mail message using smtp client
        /// </summary>
        /// <param name="isAsync">Indicates whether sending to SMTP server is asynchronous operation</param>
        public void SendSmtp(SmtpClient client, bool isAsync)
        {
            client.Timeout = 5000;

            if (credential != null)
            {
                Debug.WriteLine("MailSender Credential - AccountName: {0} ; AccountPassword: {1}", credential.UserName, credential.Password);
            }
            Debug.WriteLine("MailSender Message - From: {0} ; To: {1}; Subject: {2} ; Body: {3}", mailMessage.From, mailMessage.To, mailMessage.Subject, mailMessage.Body);

            if (isAsync)
            {
                client.SendCompleted += MailDeliveryComplete;
                client.SendAsync(mailMessage, client);
                Debug.WriteLine("{0}. To: {1}. Message was sent async!", ClassInfo.Append(".SendSmtp()"), mailMessage.To);
            }
            else
            {
                try
                {
                    client.Send(mailMessage);
                    Debug.WriteLine("{0}. To: {1}. Message was sent sync successfully!", ClassInfo.Append(".SendSmtp()"), mailMessage.To);

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("{0}. Mail wasn't sent. Error : {1}", ClassInfo.Append(".SendSmtp()"), ex.Message);
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        /// <summary>
        /// Add file name to attachment collection
        /// </summary>
        /// <param name="fileName"> File path to create the attachment</param>
        public void AddAttachment(string fileName)
        {
            var data = new Attachment(fileName);
            mailMessage.Attachments.Add(data);
        }

        /// <summary>
        /// Add recipient's address
        /// </summary>
        /// <param name="mailAddress">E-mail address</param>
        public void AddRecipient(string mailAddress)
        {
            mailMessage.To.Add(new MailAddress(mailAddress));
        }

        /// <summary>
        ///  Check if the server requires the client to authenticate
        /// </summary>
        private bool IsUseDefaultCredentials()
        {
            return credential == null;
        }

        /// <summary>
        ///  Callback method when mail delivery is completed
        /// </summary>
        /// <param name="sender">The source of the event</param>
        /// <param name="e">Event data</param>
        private void MailDeliveryComplete(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            var smtpClient = (SmtpClient)e.UserState;
            var debugMessage = new StringBuilder();

            debugMessage.Append(string.Format("{0}. Async mode. To: {1}, SMTP Server: {2}. ", ClassInfo.Append(".SendSmtp()"), mailMessage.To, smtpClient.Host));

            if (e.Error != null)
            {
                debugMessage.Append(string.Format("Mail wasn't sent. Error : {0}", e.Error.Message));
            }
            else
            {
                if (e.Cancelled)
                {
                    debugMessage.Append("Sending of email is cancelled.");
                }
                else
                {
                    debugMessage.Append("Message was sent successfully!");
                }
            }

            Debug.WriteLine(debugMessage);
            smtpClient.Dispose();
        }

        public static bool ValidateEmail(string mail)
        {
            return Regex.IsMatch(mail, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        }

        #endregion Methods
    }
}
