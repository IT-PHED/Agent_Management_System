using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
    public class AgentWallet
    {
        public int AgentID { get; set; }

        public int TransactionType { get; set; }

        public string Reason { get; set; }

        public float OpeningBalance { get; set; }

        public float TransactionBalance { get; set; }

        public float ClosingBalance { get; set; }

        public float CommissionAmount { get; set; }

        public float Commission { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedDateTime { get; set; }

        public DateTime TransactionDateTime { get; set; }

        public string ConsumerNumber { get; set; }

        public string ReceiptNumber { get; set; }

        public int? CreatedById { get; set; }
    
        public int? ModifiedById { get; set; }
    }
}