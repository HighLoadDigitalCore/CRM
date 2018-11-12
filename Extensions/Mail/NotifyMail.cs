using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Trading.Extensions.Mail
{
    public class NotifyMail
    {

        private static IConfig _config;

        public static IConfig Config
        {
            get
            {
                if (_config == null)
                {
                    _config = new Config();

                }
                return _config;
            }
        }

        private static MailSender _mailSender = new MailSender();


        public static string SendNotify(string templateName, string email,
            Func<string, string> subject,
            Func<string, string> body)
        {
            var template = Config.MailTemplates.FirstOrDefault(p => string.Compare(p.Name, templateName, true) == 0);
            if (template != null)
            {
                var result = _mailSender.SendMail(email,
                    subject.Invoke(template.Subject),
                    body.Invoke(template.Template));
                return result;
            }
            else
            {
                return "Can't find template (" + templateName + ")";
            }
        }
    }
}