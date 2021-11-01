using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
    /// <summary>
    /// Models each user transaction. Used for transfering funds, 
    /// </summary>
    public class UserTransaction
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public int AgentId { get; set; }
        public string AgentCode { get; set; }
        public string AgentName { get; set; }
        public int TransactionType { get; set; }
        public float OpeningBalance { get; set; }
        public float TransactionAmount { get; set; }
        public float ClosingBalance { get; set; }
        public float CommissionAmount { get; set; }
        public float Commission { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public string ConsumerNumber { get; set; }
        public string ReceiptNumber { get; set; }
        public string Reason { get; set; }
    }
}