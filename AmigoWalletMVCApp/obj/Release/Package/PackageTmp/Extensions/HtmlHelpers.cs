using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Fedco.PHED.Agent.management.Web.Extensions
{
    public static class HtmlHelpers
    {
        public static HtmlString reCaptcha(this HtmlHelper helper)
        {
            StringBuilder sb = new StringBuilder();
            string publickey = ConfigurationManager.AppSettings["ReCaptchaPublicKey"];
            sb.AppendLine("<script type=\"text/javascript\" src='https://www.google.com/recaptcha/api.js'></script>");
            sb.AppendLine("");
            sb.AppendLine("<div class=\"g-recaptcha\" data-sitekey=\"" + "6LeSALcUAAAAAPo3a9UV1obK9JFZj6cuOPodkaWP" + "\"></div>");
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}