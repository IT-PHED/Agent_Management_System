using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fedco.PHED.Agent.management.Web.Models;

namespace Fedco.PHED.Agent.management.Web.ViewModels
{
    public class ApprovarUserModel
    {
        public List<UserType> userTypes { get; set; }
        public User User { get; set; }
    }
}