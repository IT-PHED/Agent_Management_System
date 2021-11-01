using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
    public class AgentModel
    {
        public int? SuperAgentId { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_]+([\s][a-zA-Z0-9_]+)*", ErrorMessage = "Invalid Name")]
        [Required(ErrorMessage = "Agent Name is mandatory.")]
        [DisplayName("Agent Name: ")]
        public string AgentName { get; set; }

        [RegularExpression(@"^[a-zA-Z0-9_]*$", ErrorMessage = "Invalid Registration Number")]
        [Required(ErrorMessage = "Agent registration number is mandatory.")]
        [DisplayName("Registration Number: ")]
        public string RegistartionNumber { get; set; }

     
        [Required(ErrorMessage = "Agent code is mandatory.")]
        [DisplayName("Agent Code: ")]
        public string AgentCode { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Boolean Status { get; set; }

        //[DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        //[DisplayName("Balance:")]
        [DisplayName("Current Balance: ")]
        public float Balance { get; set; }

        public string filePath { get; set; }

        [Range(2.00, 100.00, ErrorMessage = "Please enter a Amount between 2.00 and 100.00")]
        [DisplayName("Commission Percentage: ")]
        public float Commission { get; set; }

        [Required(ErrorMessage ="Security deposit is mandotory")]
        [DisplayName("Security Deposit")]
        public float? SecurityDeposit { get; set; }
        public int? AgentId { get; set; }
        public int? SubAgentId { get; set; }
        public UserType usrType { get; set; }
        public string ApprovalStatus { get; set; }
        public string fileDisplay { get; set; }
        public int? isAmountApproved { get; set; }
        public int? isTransferApproved { get; set; }
        public int? isReversalApproved { get; set; }
        public string email { get; set; }
        public string MobileNumber { get; set; }
        public int UserId { get; set; }

        public string Action { get; set; }

        public string isAmountApprovedStatus { get; set; }
        public string isTransferApprovedStatus { get; set; }
        public string isReversalApprovedStatus { get; set; }
        public int? CreatedById { get; set; }
        public int? ModifiedById { get; set; }
    }
}