using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Fedco.PHED.Agent.management.Web.Models
{
    /// <summary>
    /// Used to model user information. Models registration validation and login
    /// </summary>
    public class User
    {
        public int UserId { get; set; }
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email address.")]
        [Required(ErrorMessage = "EmailId is mandatory.")]
        [DisplayName("Email Id:")]
        public string EmailId { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid Mobile Number")]
        [Required(ErrorMessage = "MobileNumber is mandatory.")]
        [DisplayName("Mobile Number")]
        public string MobileNumber { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Invalid Name")]
        [Required(ErrorMessage = "Name is mandatory.")]
        [DisplayName("Login Name:")]
        public string Name { get; set; }

        //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[$@$!%*#?&])[A-Za-z\d$@$!%*#?&]{8,20}$", ErrorMessage = "Invalid Password.")]
        //[DataType(DataType.Password)]
        //[Required(ErrorMessage = "Password is mandatory.")]
        //[DisplayName("Password")]
        public string Password { get; set; }

        public int UserTypeId { get; set; }

        public byte StatusId { get; set; }

        public string IMEI { get; set; } 

        public string IMEI2 { get; set; }

        public DateTime CreatedTimestamp { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime ModifiedTimestamp { get; set; }
       
        public string Action { get; set; }

        public string superAgentName { get; set; }
        public string agentName { get; set; }
        public string subAgentName { get; set; }

        public string isActivated { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
    }
}