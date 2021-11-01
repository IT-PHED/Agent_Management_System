using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    public class CheckUsernameViewModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required(ErrorMessage = "Please enter username")]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }


    }
}