using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
    /// <summary>
    /// Enumeration for Mail Types
    /// </summary>
    public enum MailType
    {
        ResetPassword = 1,
        NewAgentEmail =2
    }
    public class EmailModel
    {
        /// <summary>
        /// Gets or sets from address.
        /// </summary>
        /// <value>
        /// From address.
        /// </value>
        public string FromAddress { get; set; }

        /// <summary>
        /// Gets or sets to address.
        /// </summary>
        /// <value>
        /// To address.
        /// </value>
        public string ToAddress { get; set; }

        /// <summary>
        /// Gets or sets the CC address.
        /// </summary>
        /// <value>
        /// The CC address.
        /// </value>
        public string CCAddress { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The mail type.
        /// </value>
        public MailType Type { get; set; }

        /// <summary>
        /// Gets or sets the forgot password link.
        /// </summary>
        /// <value>
        /// The forgot password link.
        /// </value>
        public string ForgotPasswordLink { get; set; }

        public string UserName { get; set; }

        public string SITEURL { get; set; }

        public string PASSWORD { get; set; }

        /// <summary>
        /// Gets or sets the support phone.
        /// </summary>
        /// <value>
        /// The support phone.
        /// </value>
        public string SupportPhone { get; set; }

        /// <summary>
        /// Gets or sets the support email.
        /// </summary>
        /// <value>
        /// The support email.
        /// </value>
        public string SupportEmail { get; set; }
    }
}