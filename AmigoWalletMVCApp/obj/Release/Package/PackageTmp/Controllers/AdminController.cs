using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fedco.PHED.Agent.management.DAL;
using Fedco.PHED.Agent.management.Models;
using FedcoWalletMVCApp.Repository;
using Fedco.PHED.Agent.management.ViewModels;

namespace Fedco.PHED.Agent.management.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["UsertypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if(Session["LoginUserisDefaultLogin"]!=null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            FedcoWalletRepository dal = new FedcoWalletRepository();

            var list = dal.GetSuperAgents().Select(m => new Fedco.PHED.Agent.management.Web.Models.AgentModel
            { AgentId = m.AgentId,
              AgentName = m.AgentName,
              Balance = m.Balance,
              AgentCode = m.AgentCode,
              Commission = m.Commission,
              CreatedBy = m.CreatedBy,
              CreatedDate = m.CreatedDate,
              ModifiedBy = m.ModifiedBy,
              ModifiedDate = m.ModifiedDate,
              RegistartionNumber = m.RegistartionNumber,
              Status = m.Status,
              SubAgentId = m.SubAgentId,
                isAmountApproved = m.isAmountApproved,
                isTransferApproved = m.isTransferApproved,
                isReversalApproved = m.isReversalApproved,
                SuperAgentId = m.SuperAgentId, 
              ApprovalStatus =m.ApprovalStatus,
              SecurityDeposit = m.SecurityDeposit,
              filePath = m.filepath}).ToList();
            ViewBag.agents = list;
            return View(list);
        }

        public ActionResult ApprovarsList()
        {
            if (Session["UsertypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var list = dal.GetApprovarList().Select(m => new Fedco.PHED.Agent.management.Web.Models.User
            {
                UserId = m.UserId,
                Name = m.Name,
                EmailId = m.EmailId
            }).ToList();
            ViewBag.approvars = list;
            return View(list);
        }

        //GET: Super Agents for approvar
        public ActionResult ApprovarSuperAgentsList()
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

            var list = dal.GetSuperAgents().Select(m => new Fedco.PHED.Agent.management.Web.Models.AgentModel
            {
                AgentId = m.AgentId,
                AgentName = m.AgentName,
                Balance = m.Balance,
                AgentCode = m.AgentCode,
                Commission = m.Commission,
                CreatedBy = m.CreatedBy,
                CreatedDate = m.CreatedDate,
                ModifiedBy = m.ModifiedBy,
                ModifiedDate = m.ModifiedDate,
                RegistartionNumber = m.RegistartionNumber,
                Status = m.Status,
                SubAgentId = m.SubAgentId,
                SuperAgentId = m.SuperAgentId,
                ApprovalStatus = m.ApprovalStatus,
                SecurityDeposit = m.SecurityDeposit,
                isAmountApproved = m.isAmountApproved,
                isTransferApproved =m.isTransferApproved,
                isReversalApproved = m.isReversalApproved,
                filePath = m.filepath,
                fileDisplay = m.filepath != "" ? m.filepath.Split('\\')[m.filepath.Split('\\').Length - 1] : ""
            }).ToList();
            ViewBag.agents = list;
            return View(list);
        }

        public ActionResult ApprovarTransactionsSuperAgentsList()
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

            var list = dal.GetSuperAgentsTransactionsForApprove().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserTransaction
            {
                Id = m.Id,
                AgentId = m.AgentId,
                Reason =m.Reason,
                TransactionAmount = m.TransactionAmount,
                CommissionAmount = m.CommissionAmount,
                Commission = m.Commission,
                AgentCode = m.AgentCode,
                AgentName = m.AgentName

            }).ToList();
            ViewBag.transactions = list;
            return View(list);

        }

        public ActionResult Approve(int AgentId, int isApproveAccess,int isApproveTransfer, int isApproveReversal)
        {
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var status = dal.SuperAgentApproveOrReject(AgentId, isApproveAccess, isApproveTransfer, isApproveReversal, Session["email"].ToString());
            return RedirectToAction("ApprovarSuperAgentsList", "Admin");
        }

        public ActionResult Reject(int AgentId, int isApproveAccess, int isApproveTransfer, int isApproveReversal)
        {
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var status = dal.SuperAgentApproveOrReject(AgentId, isApproveAccess, isApproveTransfer, isApproveReversal, Session["email"].ToString());
            return RedirectToAction("ApprovarSuperAgentsList", "Admin");
        }



        [HttpGet]
        public ActionResult ApproveTransaction(int Id, int isApproveAmount)
        {
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var resultStatus = dal.ApproveTransaction(Id, isApproveAmount, Session["email"].ToString());
            if (resultStatus > 0)
            {
                return RedirectToAction("ApprovarTransactionsSuperAgentsList", "Admin");
            }
            else
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }



        [HttpGet]
        public JsonResult UpdateUserStatus(string status, int agentid)
        {
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var resultStatus = dal.UpdateAgentStatus(agentid, status == "true" ? "ACTIVE" : "INACTIVE", 1, 0, 0, Session["email"].ToString(), (int)Session["LoginUserId"]);
            return new JsonResult { Data = resultStatus, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        /// <summary>
        /// Download portable document downloads portable document file.
        /// </summary>
        /// <param name="fileName">specifies file name.</param>
        public void Download(string fileName)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(fileName);
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Type", "application/octet-stream");
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", string.Format("attachment;filename=\"{0}\"", fileName.Split('.')[0].Split('\\')[fileName.Split('.')[0].Split('\\').Length - 1] + "." + fileName.Split('.')[1]));
            System.Web.HttpContext.Current.Response.BinaryWrite(bytes);
            System.Web.HttpContext.Current.Response.End();
        }
    }
}