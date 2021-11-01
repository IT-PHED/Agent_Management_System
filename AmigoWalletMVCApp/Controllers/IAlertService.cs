using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fedco.PHED.Agent.management.Web.Models;

namespace Fedco.PHED.Agent.management.Web.Controllers
{
    public interface IAlertService
    {
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="emailOptions">The email options.</param>
        void SendMail(EmailModel emailOptions);

        /// <summary>
        /// Sends error mail.
        /// </summary>
        /// <param name="emailOptions">The email options.</param>
        /// <param name="message">Error message</param>
        void SendErrorMail(EmailModel emailOptions, string message);
    }
}
