using Fedco.PHED.Agent.management.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.ViewModels
{
    public class TransferViewModel
    {
        public AgentModel Agent { get; set; }

        [Range(0.00, 1000000000000000000.00, ErrorMessage = "Please enter a Amount between 0.00 and 100.00")]
        [DisplayName("Amount: ")]
        public float TransferAmount { get; set; }

        public AgentWallet AgentWallet { get; set; }

    }
}