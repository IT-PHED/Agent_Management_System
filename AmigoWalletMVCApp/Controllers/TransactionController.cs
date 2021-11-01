using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fedco.PHED.Agent.management.DAL;
using Fedco.PHED.Agent.management.Models;
using Fedco.PHED.Agent.management.ViewModels;
using Fedco.PHED.Agent.management.Web.Enum;
using Fedco.PHED.Agent.management.Web.ViewModels;
using FedcoWalletMVCApp.Repository;

namespace Fedco.PHED.Agent.management.Controllers
{
    /// <summary>
    /// Deprecated transfer controller, refer to AccountController
    /// </summary>
    public class TransactionController : Controller
    {
        [HttpGet]
        public ActionResult TransferToAgent(string agentcode, float Balance)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            TransferViewModel transferView = new TransferViewModel();
            ViewBag.AgentBalance = Balance;
            var mapObjAgent = new FedcoWalletMapper<Fedco.PHED.Agent.management.Models.Agent, Fedco.PHED.Agent.management.Web.Models.AgentModel>();

            var storedProcName = string.Empty;
            switch ((int)Session["UserTypeId"])
            {
                case 1:
                    storedProcName = "SP_AG_GETSUPERAGENT_BY_AGENTCODE";//  SP_AG_GETSPG_BY_AGENTCODE

                    ViewBag.Admin = true;
                    break;
                case 2:
                    storedProcName = "SP_AG_GETAGENT_BY_AGENTCODE";
                    ViewBag.Admin = false;
                    break;

                case 3:
                    storedProcName = "SP_AG_GETSUBAGENT_BY_AGENTCODE";
                    ViewBag.Admin = false;
                    break;

            }

            transferView.Agent = mapObjAgent.Translate(dal.GetAgentByCode(agentcode, storedProcName));
            return View(transferView);
        }

        [HttpGet]
        public ActionResult AgentIMEI(int agentId)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            UserViewModel userview = new UserViewModel();


            var storedProcName = string.Empty;
            switch ((int)Session["UserTypeId"])
            {
                case 1:
                    storedProcName = "SP_AG_GETSUPERAGENTIMEI_BY_AGENTID";
                    ViewBag.Admin = true;
                    break;
                case 2:
                    storedProcName = "SP_AG_GETAGENTIMEI_BY_AGENTID";
                    ViewBag.Admin = false;
                    break;

                case 3:
                    storedProcName = "SP_AG_GETSUBAGENTIMEI_BY_AGENTID";
                    ViewBag.Admin = false;
                    break;

            }
            var mapObjUser = new FedcoWalletMapper< User, Fedco.PHED.Agent.management.Web.Models.User>();

            userview.User = mapObjUser.Translate(dal.GetAgentIMEIByCode(agentId, storedProcName));
            return View(userview);
        }

        [HttpGet]
        public ActionResult ReversalFromAgent(string agentcode, float Balance)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            TransferViewModel transferView = new TransferViewModel();
            ViewBag.AgentBalance = Balance;
            var mapObjAgent = new FedcoWalletMapper<Fedco.PHED.Agent.management.Models.Agent, Fedco.PHED.Agent.management.Web.Models.AgentModel>();

            var storedProcName = string.Empty;
            switch ((int)Session["UserTypeId"])
            {
                case 1:
                    storedProcName = "SP_AG_GETSUPERAGENT_BY_AGENTCODE";//  SP_AG_GETSPG_BY_AGENTCODE

                    ViewBag.Admin = true;
                    break;
                case 2:
                    storedProcName = "SP_AG_GETAGENT_BY_AGENTCODE";
                    ViewBag.Admin = false;
                    break;

                case 3:
                    storedProcName = "SP_AG_GETSUBAGENT_BY_AGENTCODE";
                    ViewBag.Admin = false;
                    break;

            }

            transferView.Agent = mapObjAgent.Translate(dal.GetAgentByCode(agentcode, storedProcName));
            return View(transferView);
        }

        [HttpPost]
        public ActionResult ReversalFromAgent(TransferViewModel transferView)
        {
            try
            {
                if (Session["UserTypeId"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
                {
                    return RedirectToAction("ChangePassword", "Account");
                }
                // Object for data access layer
                FedcoWalletRepository dal = new FedcoWalletRepository();
                transferView.AgentWallet = new Web.Models.AgentWallet();
                transferView.AgentWallet.AgentID = transferView.Agent.AgentId.Value;
                transferView.AgentWallet.TransactionType = 0;
                transferView.AgentWallet.Reason = "PHED WALLET CREDIT REVERSAL";
                transferView.AgentWallet.OpeningBalance = 0;
                transferView.AgentWallet.TransactionBalance = transferView.TransferAmount;
                transferView.AgentWallet.Commission = transferView.Agent.Commission;
                transferView.AgentWallet.CommissionAmount = ((transferView.TransferAmount) / 100 * transferView.Agent.Commission);
                transferView.AgentWallet.CreatedBy = Session["email"].ToString();
                //transferView.AgentWallet.CreatedById = (Int32)Session["LoginUserId"];
                transferView.AgentWallet.ConsumerNumber = "";
                transferView.AgentWallet.ReceiptNumber = "";
                var storedProcName = string.Empty;
                switch ((int)Session["UserTypeId"])
                {
                    case 1:
                        storedProcName = "SP_AG_SUPERAGENT_UPDATEWALLET_TEMP";//  SP_AG_SPERAGT_UPDTEWLT_TEMP

                        ViewBag.Admin = true;
                        break;
                    case 2:
                        storedProcName = "SP_AG_AGENT_UPDATEWALLET";
                        ViewBag.Admin = false;
                        break;

                    case 3:
                        storedProcName = "SP_AG_SUBAGENT_UPDATEWALLET";
                        ViewBag.Admin = false;
                        break;

                }
                var mapObjAgentWallet = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.AgentWallet, Fedco.PHED.Agent.management.Models.AgentWallet>();
                var balance = dal.CreateSuperAgentWallet(mapObjAgentWallet.Translate(transferView.AgentWallet), storedProcName);
                switch ((int)Session["UserTypeId"])
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Home", "Account");

                    case 3:
                        return RedirectToAction("Home", "Account");
                    case 4:
                        return RedirectToAction("TransactionList", "Transaction");

                }
                return null;
            }
            catch (Exception ex)
            {

                // For debugging exceptions
                if (ex.ToString().Contains("0001"))
                {
                    Session["regError"] = ExecptionMessages.uniqueContraint;
                }
                else if (ex.ToString().Contains("20001"))
                {
                    Session["regError"] = ExecptionMessages.Insufficient;
                }
                else
                {
                    Session["regError"] = ex.ToString();
                }

                return RedirectToAction("Error", "Register");
            }

        }

        [HttpPost]
        public ActionResult AgentIMEI(UserViewModel userView)
        {
            try
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

                userView.User.CreatedById = (int)Session["LoginUserId"];
                var mapObjuser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.User, Fedco.PHED.Agent.management.Models.User>();
                var status = dal.UpdteAgentIMEI(mapObjuser.Translate(userView.User), "SP_AG_UPDATE_IMEI_BYUSERID");
                switch ((int)Session["UserTypeId"])
                {
                    case 1: 
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Home", "Account");

                    case 3:
                        return RedirectToAction("Home", "Account");
                    case 4:
                        return RedirectToAction("TransactionList", "Transaction");

                }
                return null;
            }
            catch (Exception ex)
            {

                // For debugging exceptions
                if (ex.ToString().Contains("0001"))
                {
                    Session["regError"] = ExecptionMessages.uniqueContraint;
                }
                else if (ex.ToString().Contains("20001"))
                {
                    Session["regError"] = ExecptionMessages.Insufficient;
                }
                else
                {
                    Session["regError"] = ex.ToString();
                }

                return RedirectToAction("Error", "Register");
            }
        }

        [HttpPost]
        public ActionResult TransferToAgent(TransferViewModel transferView)
        {
            try
            {
                if (Session["UserTypeId"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }
                if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
                {
                    return RedirectToAction("ChangePassword", "Account");
                }
                // Object for data access layer
                FedcoWalletRepository dal = new FedcoWalletRepository();
                transferView.AgentWallet = new Web.Models.AgentWallet();
                transferView.AgentWallet.AgentID = transferView.Agent.AgentId.Value;
                transferView.AgentWallet.TransactionType = 1;
                transferView.AgentWallet.Reason = "PHED WALLET CREDIT";
                transferView.AgentWallet.OpeningBalance = 0;
                transferView.AgentWallet.TransactionBalance = transferView.TransferAmount;
                transferView.AgentWallet.Commission = transferView.Agent.Commission;
                transferView.AgentWallet.CommissionAmount = ((transferView.TransferAmount) / 100 * transferView.Agent.Commission);
                transferView.AgentWallet.CreatedBy = Session["email"].ToString();               
                transferView.AgentWallet.ConsumerNumber = "";
                transferView.AgentWallet.ReceiptNumber = "";
                var storedProcName = string.Empty;
                switch ((int)Session["UserTypeId"])
                {
                    case 1:
                        storedProcName = "SP_AG_SUPERAGENT_UPDATEWALLET_TEMP";

                        ViewBag.Admin = true;
                        break;
                    case 2:
                        storedProcName = "SP_AG_AGENT_UPDATEWALLET";
                        ViewBag.Admin = false;
                        break;

                    case 3:
                        storedProcName = "SP_AG_SUBAGENT_UPDATEWALLET";
                        ViewBag.Admin = false;
                        break;

                }
                var mapObjAgentWallet = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.AgentWallet, Fedco.PHED.Agent.management.Models.AgentWallet>();
                var balance = dal.CreateSuperAgentWallet(mapObjAgentWallet.Translate(transferView.AgentWallet), storedProcName);
                switch ((int)Session["UserTypeId"])
                {
                    case 1:
                        return RedirectToAction("Index", "Admin");
                    case 2:
                        return RedirectToAction("Home", "Account");

                    case 3:
                        return RedirectToAction("Home", "Account");
                    case 4:
                        return RedirectToAction("TransactionList", "Transaction");

                }
                return null;
            }
            catch (Exception ex)
            {

                // For debugging exceptions
                if (ex.ToString().Contains("0001"))
                {
                    Session["regError"] = ExecptionMessages.uniqueContraint;
                }
                else if (ex.ToString().Contains("20001"))
                {
                    Session["regError"] = ExecptionMessages.Insufficient;
                }
                else
                {
                    Session["regError"] = ex.ToString();
                }

                return RedirectToAction("Error", "Register");
            }

        }

        [HttpGet]
        public ActionResult TransactionList()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            ViewBag.UserTypeId = Convert.ToInt32(Session["UserTypeId"]);
            return View();
            //return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           // return Json(new {data = userTransactions });
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


        public ActionResult TransactionListByFilters(string startDate,string EndDate,int superAgentId,int agentId,int subAgentId)
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
            List<Fedco.PHED.Agent.management.Models.UserTransaction> userTransactions = dal.GetSuperAgentTransactions(agentid, storedProcedure, startDate,EndDate,superAgentId,agentId,subAgentId);
            ViewBag.userTransactions = userTransactions;
            ViewBag.UserTypeId = (int)Session["UserTypeId"];

            // return View(userTransactions);
            return new JsonResult { Data = userTransactions, JsonRequestBehavior = JsonRequestBehavior.AllowGet, MaxJsonLength =Int32.MaxValue };
            // return Json(new {data = userTransactions });
        }

    }
}