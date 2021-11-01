using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fedco.PHED.Agent.management.DAL;
using Fedco.PHED.Agent.management.Web.Enum;
using Fedco.PHED.Agent.management.Web.ViewModels;
using FedcoWalletMVCApp.Repository;

namespace Fedco.PHED.Agent.management.Web.Controllers
{
    public class ReportController : Controller
    {
        // GET: Report
        public ActionResult Index()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult AccountStatement()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult CashierRegister()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult InterAccount()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult SubAgentFundingReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult DailyMonthlyReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult AgentSubAgentReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult AgentCollectionReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult FundingHistoryReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public ActionResult CommissionReport()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            return View();
        }

        public JsonResult GetSubAgents(int agentID)
        {
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GETSUBAGENTS_BY_AGENTID";
            List<Fedco.PHED.Agent.management.Models.Agent> subAgents = dal.GetagentsByID(agentID, storedProcedure);
            return new JsonResult { Data = subAgents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSuperAgents()
        {
            int agentID = 0;
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GETSUPERAGENTS";
            List<Fedco.PHED.Agent.management.Models.Agent> superAgents = dal.GetagentsByID(agentID, storedProcedure);
            return new JsonResult { Data = superAgents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAgents(int agentID)
        {
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GETAGENTS_BY_AGENTID";
            List<Fedco.PHED.Agent.management.Models.Agent> subAgents = dal.GetagentsByID(agentID, storedProcedure);
            return new JsonResult { Data = subAgents, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult ComissionReportByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "";
            var agentid = 0;

            if((superAgentId!=0 && agentId!=0 && subAgentId != 0)||(agentId != 0 && subAgentId != 0) ||(subAgentId!=0))
            {
                agentid = subAgentId;
                storedProcedure = "SP_AG_GET_SUBAGENT_COMISSION_BY_AGENTID";//SP_AG_GET_ADMIN_TRS_BY_ID
            }
            else if( (agentId != 0 && subAgentId == 0) ||(agentId!=0))
            {
                agentid = agentId;
                storedProcedure = "SP_AG_GET_AGENT_COMISSION_BY_AGENTID";//SP_AG_GET_ADMIN_TRS_BY_ID
            }
            else if(superAgentId!=0 && agentId == 0 && subAgentId == 0)
            {
                agentid = superAgentId;
                storedProcedure = "SP_AG_GET_SUPERAGENT_COMISSION_BY_AGENTID";//SP_AG_GET_ADMIN_TRS_BY_ID
            }


            if (!string.IsNullOrEmpty(storedProcedure))
            {
                List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetCommissionTransactions(storedProcedure, startDate, EndDate, agentId);
                ViewBag.userTransactions = userTransactions;
                ViewBag.UserTypeId = (int)Session["UserTypeId"];

                // return View(userTransactions);
                return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            }
            else
            {
                return null;
            }
            // return Json(new {data = userTransactions });
        }

        public ActionResult AgentSubAgentReportByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GET_AGENTSUBAGENT_FUNDING_BY_ID";
            
            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetAgentSubAgentFundingTransactions( storedProcedure, startDate, EndDate, superAgentId, agentId, subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

        public ActionResult AgentInterAccountByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GET_INTERACOUNT_BY_ID";

            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetAgenInterAccountTransactions(storedProcedure, startDate, EndDate, superAgentId, agentId, subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

        public ActionResult CashierRegisterByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GET_CASHIERREGISTER_BY_ID";

            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetCashierRegisterTransactions(storedProcedure, startDate, EndDate, superAgentId, agentId, subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

        public ActionResult AgentCollectionReportByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "SP_AG_GET_AGENTCOLLECTION_BY_ID";

            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetAgentCollectionTransactions(storedProcedure, startDate, EndDate, superAgentId, agentId, subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

        public ActionResult AccountStatementByFilters(string startDate, string EndDate, int superAgentId, int agentId, int subAgentId,string reportType)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "";
            var agentid = 0;


            switch ((int)Session["UserTypeId"])
            {

                case 1:
                    storedProcedure = "SP_AG_GET_ADMIN_TRANSACTIONS_BY_ID";//SP_AG_GET_ADMIN_TRS_BY_ID
                    agentid = (int)Session["LoginUserId"];
                    break;
                case 2:
                    storedProcedure = "SP_AG_GET_SUPERAGENT_TRANSACTIONS_BY_ID";//  SP_GET_SPRAGT_TRS_BY_ID
                    agentid = (int)Session["SuperAgentId"];
                    break;
                case 3:
                    storedProcedure = "SP_AG_GET_AGENT_TRANSACTIONS_BY_ID";//  SP_AG_GET_AGENT_TRS_BY_ID
                    agentid = (int)Session["AgentId"];
                    break;

                case 4:
                    storedProcedure = "SP_AG_GET_SUBAGENT_TRANSACTIONS_BY_ID"; //  SP_GET_SUBAGENT_TRS_BY_ID
                    agentid = (int)Session["SubAgentId"];
                    break;


            }
            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetSuperAgentTransactions(agentid, storedProcedure, startDate, EndDate, superAgentId, agentId, subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

        public ActionResult subAgentFundingtByFilters(string startDate, string EndDate, int agentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var storedProcedure = "";

            storedProcedure = "SP_AG_GET_SUBAGENT_FUNDING_BY_AGENTID";//  SP_GET_SPRAGT_TRS_BY_ID
            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetSubAgentFunding( storedProcedure, startDate, EndDate,agentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength = Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }
    }
}