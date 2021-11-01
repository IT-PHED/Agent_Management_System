using Fedco.PHED.Agent.management.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fedco.PHED.Agent.management.Web.Controllers
{
    public class AuditController : Controller
    {
        // GET: Audit
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUserAuditReport(string startDate, string EndDate)
        {
            FedcoWalletRepository dal = new FedcoWalletRepository();
            List<Fedco.PHED.Agent.management.Models.User> users = dal.GetUserMasterAudit();
            return new JsonResult { Data = users, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAuditReport(string startDate,string EndDate,int superAgentId,int agentId,int subAgentId,int reportType)
        {
          
            FedcoWalletRepository dal = new FedcoWalletRepository();
            List<Fedco.PHED.Agent.management.Models.Agent> agents = new List<management.Models.Agent>();
            switch (reportType)
            {
                
                case 1:
                    agents = dal.GetAgentsAuditAgentId(superAgentId, "SP_AG_GET_SUPERAGENT_AUDIT_BY_ID");                    
                    break;
                case 3:
                    agents = dal.GetAgentsAuditAgentId(agentId, "SP_AG_GET_SUPERAGENT_AUDIT_BY_ID");                 
                    break;
                case 4:
                    agents = dal.GetAgentsAuditAgentId(subAgentId, "SP_AG_GET_SUPERAGENT_AUDIT_BY_ID");
                    break;
            }

            return new JsonResult { Data = agents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}