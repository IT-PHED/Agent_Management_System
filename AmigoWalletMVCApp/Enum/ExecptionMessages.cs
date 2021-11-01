using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Fedco.PHED.Agent.management.Web.Enum
{
  public enum ExecptionMessages
  {
    [Description("User/Agent already Exists")]
    uniqueContraint =0001,

    [Description("Insufficient Wallet Balance")]
    Insufficient=20001

  }
}
