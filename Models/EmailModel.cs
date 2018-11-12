using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;
using System.Web.Mail;
using Trading.Extensions;
using Trading.Extensions.Helpers;
using MailMessage = System.Net.Mail.MailMessage;

namespace Trading.Models
{
    public class MailSender
    {
        public static bool SendEmail(
            string pGmailEmail,
            string pGmailPassword,
            string pTo,
            string pSubject,
            string pBody,
            System.Web.Mail.MailFormat pFormat,
            string pAttachmentPath)
        {
            try
            {
                System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserver",
                                  SiteSetting.Get<string>("SMTP"));
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserverport",
                    SiteSetting.Get<Int32>("SMTPPort"));
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/sendusing",
                                  "2");
                //sendusing: cdoSendUsingPort, value 2, for sending the message using 
                //the network.

                //smtpauthenticate: Specifies the mechanism used when authenticating 
                //to an SMTP 
                //service over the network. Possible values are:
                //- cdoAnonymous, value 0. Do not authenticate.
                //- cdoBasic, value 1. Use basic clear-text authentication. 
                //When using this option you have to provide the user name and password 
                //through the sendusername and sendpassword fields.
                //- cdoNTLM, value 2. The current process security context is used to 
                // authenticate with the service.
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //Use 0 for anonymous
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendusername",
                    pGmailEmail);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendpassword",
                     pGmailPassword);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpusessl",
                     SiteSetting.Get<bool>("SMTPSSL") ? "true" : "false");
                myMail.From = pGmailEmail;
                myMail.To = pTo;
                myMail.Subject = pSubject;
                myMail.BodyFormat = pFormat;
                myMail.Body = pBody;
                if (pAttachmentPath.Trim() != "")
                {
                    MailAttachment MyAttachment =
                            new MailAttachment(pAttachmentPath);
                    myMail.Attachments.Add(MyAttachment);
                    myMail.Priority = System.Web.Mail.MailPriority.Normal;
                }

                System.Web.Mail.SmtpMail.SmtpServer = SiteSetting.Get<string>("SMTP") + ":" + SiteSetting.Get<Int32>("SMTPPort");
                System.Web.Mail.SmtpMail.Send(myMail);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class Email
    {
        protected string MailTo { get; set; }
        protected string MailFrom { get; set; }
        protected string MailSubject { get; set; }
        protected string MailBody { get; set; }
        public List<string> Attachments { get; set; }
        public List<KeyValuePair<string, MemoryStream>> MemoryAttachments { get; set; }

        public Email To(string to)
        {
            MailTo = to;
            return this;
        }
        public Email From(string from)
        {
            MailFrom = from;
            return this;
        }
        public Email WithSubject(string subject)
        {
            MailSubject = subject;
            return this;
        }
        public Email WithBody(string body)
        {
            MailBody = body;
            return this;
        }
        /// <summary>
        /// Добавляет вложения
        /// </summary>
        /// <param name="attach">Список вложений, каждое вложение = ПОЛНЫЙ ПУТЬ ФИЗИЧЕСКОМУ ФАЙЛУ НА ДИСКЕ, НЕ URL!</param>
        /// <returns></returns>
        public Email WithAttachments(List<string> attach)
        {
            Attachments = attach;
            return this;
        }
        /// <summary>
        /// Добавляет вложения
        /// </summary>
        /// <param name="attach">Список пар Название-MemoryStream</param>
        /// <returns></returns>
        public Email WithAttachments(List<KeyValuePair<string, MemoryStream>> attach)
        {
            MemoryAttachments = attach;
            return this;
        }

        public string Send()
        {
            string msg;
            Send(out msg);
            return msg;
        }
        public bool Send(out string result)
        {
            string message;
            var success = Send(MailTo, MailFrom, MailSubject, MailBody, Attachments ?? new List<string>(),
                               MemoryAttachments ?? new List<KeyValuePair<string, MemoryStream>>(), out message);
            result = message;
            return success;
        }

        protected static bool Send(string sendTo, string sendFrom, string sendSubject, string sendMessage, List<string> attachments, List<KeyValuePair<string, MemoryStream>> memAttaches, out string result)
        {
            try
            {


             /*   MailSender.SendEmail(SiteSetting.Get<string>("SMTPLogin"), SiteSetting.Get<string>("SMTPPass"), sendTo,
                    sendSubject, sendMessage, MailFormat.Html, "");
                result = "";
                return true;*/

                bool bTest = sendTo.IsMailAdress();
                if (bTest == false)
                {
                    result = "Неправильно указан адрес: " + sendTo;
                    return false;
                }

                var message = new MailMessage(
                   sendFrom,
                   sendTo,
                   sendSubject,
                   sendMessage);


                message.IsBodyHtml = true;
                message.BodyEncoding = Encoding.UTF8;
                foreach (string attach in attachments)
                {
                    var attached = new Attachment(attach, MediaTypeNames.Application.Octet);
                    //attached.NameEncoding = Encoding.UTF8;
                    message.Attachments.Add(attached);
                }

                foreach (var pair in memAttaches)
                {
                    pair.Value.Position = 0;
                    var attached = new Attachment(pair.Value, pair.Key, MIMETypeWrapper.GetMIME(Path.GetExtension(pair.Key).Substring(1)));
                    //attached.NameEncoding = Encoding.UTF8;
                    message.Attachments.Add(attached);
                }


                // create smtp client at mail server location

                var client = new SmtpClient(SiteSetting.Get<string>("SMTP"));

                if (SiteSetting.Get<Int32>("SMTPPort") > 0)
                    client.Port = SiteSetting.Get<Int32>("SMTPPort");

                if (SiteSetting.Get<string>("SMTPLogin").IsNullOrEmpty() || SiteSetting.Get<string>("SMTPPass").IsNullOrEmpty())
                {
                    client.UseDefaultCredentials = true;
                }
                else
                {
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(SiteSetting.Get<string>("SMTPLogin"), SiteSetting.Get<string>("SMTPPass"));
                }
                client.EnableSsl = SiteSetting.Get<bool>("SMTPSSL");
                /*client.DeliveryMethod = SmtpDeliveryMethod.Network;*/

                client.Send(message);
                result = "";
                return true;

            }

            catch (Exception ex)
            {
                result = ex.Message;
                return false;

            }
        }
    }

}