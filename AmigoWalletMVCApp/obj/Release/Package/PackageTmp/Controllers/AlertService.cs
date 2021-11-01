using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Reflection;
using Fedco.PHED.Agent.management.Web.Models;

namespace Fedco.PHED.Agent.management.Web.Controllers
{
    public class AlertService : IAlertService
    {
        public void SendErrorMail(EmailModel emailOptions, string message)
        {
            if (emailOptions != null)
            {
                var mail = new MailMessage
                {
                    From =
                        new MailAddress(
                        ConfigurationManager.AppSettings["PortalErrorFromAddress"],
                        ConfigurationManager.AppSettings["PortalErrorDisplayName"])
                };

                if (!string.IsNullOrEmpty(emailOptions.CCAddress))
                {
                    string[] emailAddresses = emailOptions.CCAddress.Split(',');
                    foreach (string address in emailAddresses)
                    {
                        mail.CC.Add(new MailAddress(address));
                    }
                }

                mail.Subject = emailOptions.Subject;
                mail.To.Add(new MailAddress(emailOptions.ToAddress));
                mail.IsBodyHtml = true;
                mail.Body = message;
                var smtp = new SmtpClient();
                smtp.Send(mail);
            }
        }

        public void SendMail(EmailModel emailOptions)
        {
            if (emailOptions != null)
            {
                var emailTemplate = GetMailBodyOfTemplate(emailOptions.Type + ".cshtml");
                if (emailTemplate == null)
                {
                    throw new Exception("Email template not found.");
                }

                foreach (PropertyInfo p in typeof(EmailModel).GetProperties())
                {
                    var propertyValue = p.GetValue(emailOptions, null);
                    var displayValue = string.Empty;
                    if (p.PropertyType.Name == typeof(DateTime).Name)
                    {
                        if (Convert.ToDateTime(propertyValue) != DateTime.MinValue)
                        {
                            displayValue = Convert.ToDateTime(propertyValue).ToString("dd/MM/yyyy");
                        }
                    }
                    else
                    {
                        if (propertyValue != null)
                        {
                            displayValue = propertyValue.ToString();
                        }
                    }

                    emailTemplate = emailTemplate.Replace("<%" + p.Name + "%>", displayValue);
                }

                var mail = new MailMessage
                {
                    From =
                                       new MailAddress(
                                       ConfigurationManager.AppSettings["PortalFromAddress"],
                                       ConfigurationManager.AppSettings["PortalDisplayName"])
                };

                if (!string.IsNullOrEmpty(emailOptions.CCAddress))
                {
                    string[] emailAddresses = emailOptions.CCAddress.Split(',');
                    foreach (string address in emailAddresses)
                    {
                        mail.CC.Add(new MailAddress(address));
                    }
                }

                mail.Subject = emailOptions.Subject;
                mail.To.Add(new MailAddress(emailOptions.ToAddress));
                mail.IsBodyHtml = true;
                mail.Body = emailTemplate;
                var smtp = new SmtpClient();
                smtp.Send(mail);
            }
        }

        /// <summary>
        /// read the text in template file and return it as a string
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>electronic mail file body</returns>
        private static string ReadFileFrom(string templateName)
        {
            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/Views/Emails/" + templateName);
            var body = File.ReadAllText(filePath);
            return body;
        }

        /// <summary>
        /// Gets the template body, cache it and return the text
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns>electronic mail body template</returns>
        private static string GetMailBodyOfTemplate(string templateName)
        {
            var cacheKey = string.Concat("mailTemplate:", templateName);
            var body = (string)System.Web.HttpContext.Current.Cache[cacheKey];
            if (string.IsNullOrEmpty(body))
            {
                body = ReadFileFrom(templateName);
                if (!string.IsNullOrEmpty(body))
                {
                    System.Web.HttpContext.Current.Cache.Insert(cacheKey, body, null, DateTime.Now.AddHours(1), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }

            return body;
        }
    }
}