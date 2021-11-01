using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FedcoWalletMVCApp.Repository;
using Fedco.PHED.Agent.management.DAL;
using System.Configuration;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Fedco.PHED.Agent.management.Web.ViewModels;
using Fedco.PHED.Agent.management.Web.Controllers;
using Fedco.PHED.Agent.management.Web.Models;
using Fedco.PHED.Agent.management.ViewModels;
using Fedco.PHED.Agent.management.Models;
using Fedco.PHED.Agent.management.Web.Enum;

namespace FedcoWalletMVCApp.Controllers
{


    /// <summary>
    /// Controller class for all account related operations
    /// </summary>
    public class AccountController : Controller
    {


        [HttpGet]
        public ActionResult EditAgent(int agentId)
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
                    storedProcName = "SP_AG_GETSUPERAGENT_BY_AGENTID";
                    ViewBag.Admin = true;
                    break;
                case 2:
                    storedProcName = "SP_AG_GETAGENT_BY_AGENTID";
                    ViewBag.Admin = false;
                    break;

                case 3:
                    storedProcName = "SP_AG_GETSUBAGENT_BY_AGENTID";
                    ViewBag.Admin = false;
                    break;
                

            }
            var superAgent = dal.GetSuperAgentByID(agentId, storedProcName);
            var mapObjAgent = new FedcoWalletMapper<Agent, Fedco.PHED.Agent.management.Web.Models.AgentModel > ();
           // userview.Agent = ;
            userview.Agent = mapObjAgent.Translate(dal.GetSuperAgentByID(agentId, storedProcName));
            userview.Agent.usrType = new Fedco.PHED.Agent.management.Web.Models.UserType { Id = (int)Session["UserTypeId"] };
           
            userview.User = new Fedco.PHED.Agent.management.Web.Models.User { EmailId = userview.Agent.email,MobileNumber = userview.Agent.MobileNumber, UserId =userview.Agent.UserId,UserTypeId =(int)Session["UserTypeId"] };
            return View(userview);
        }

        [HttpPost]
        public ActionResult EditAgent(UserViewModel userView)
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


                var mapObjuser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.AgentModel, Fedco.PHED.Agent.management.Models.Agent>();
                userView.Agent.email = userView.User.EmailId;
                userView.Agent.MobileNumber = userView.User.MobileNumber;
                userView.Agent.UserId = userView.User.UserId;
                userView.Agent.CreatedById = (int)Session["LoginUserId"];
                userView.Agent.usrType = new Fedco.PHED.Agent.management.Web.Models.UserType { Id = (int)Session["UserTypeId"] };
                var status = dal.UpdteAgent(mapObjuser.Translate(userView.Agent), "SP_AG_UPDATE_AGENT_BYAGENTID");
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
        /// <summary>
        /// Generates the account homepage with transactions, balance, cards, merchants, and utilities
        /// </summary>
        /// <param name="from">from date for the transactions to be fetched</param>
        /// <param name="from">to date for the transactions to be fetched</param>
        /// <returns></returns>
        public ActionResult Home(DateTime? from = null, DateTime? to = null)
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

                // Data access layer object
                FedcoWalletRepository repObj = new FedcoWalletRepository();
                var storedProcName = string.Empty;
                var storedProcNameAgentDetails = string.Empty;
                int agentId = 0;
                switch ((int)Session["UserTypeId"])
                {
                    case 2:
                        storedProcName = "SP_AG_GETAGENT_DET";
                        storedProcNameAgentDetails = "SP_AG_GETSUPERAGENT_BY_AGENTID";
                        agentId = Convert.ToInt32(Session["SuperAgentId"].ToString());
                        ViewBag.Admin = true;
                        Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "SUPERAGENT" });
                        break;
                    case 3:
                        storedProcName = "SP_AG_GETSUBAGENT_DET";
                        storedProcNameAgentDetails = "SP_AG_GETAGENT_BY_AGENTID";
                        agentId = Convert.ToInt32(Session["AgentId"].ToString());
                        ViewBag.Admin = false;
                        Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "AGENT" });
                        break;

                    case 4:
                        storedProcName = "SP_AG_GETSUBAGENT_DET";
                        storedProcNameAgentDetails = "SP_AG_GETSUBAGENT_BY_AGENTID";
                        agentId = Convert.ToInt32(Session["SubAgentId"].ToString());
                        ViewBag.Admin = false;
                        Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "SUBAGENT" });
                        break;
                    case 6:
                          return View("~/Views/Shared/_UserHome.cshtml"); ;
                        

                }
                var list = repObj.GetAgentsBySuperAgentId(agentId, storedProcName).Select(m => new Fedco.PHED.Agent.management.Web.Models.AgentModel
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
                    SuperAgentId = m.SuperAgentId
                }).ToList();
                ViewBag.agents = list;

                // Calculate balance points and amount approval it to the view
                var superAgent = repObj.GetSuperAgentByID(agentId, storedProcNameAgentDetails);
                ViewBag.ViewBalance = superAgent.Balance;
                ViewBag.isAmountApproved = superAgent.isAmountApproved;

                // Draw the account homepage
                return View();
            }
            catch (Exception ex)
            {
                // Show session expiration page and allow re-log

                Session["regError"] = ex.StackTrace + ":" + ex.Source + ":" + ex.InnerException + ex.Message;
                return RedirectToAction("Error", "Register");
            }
        }


        /// <summary>
        /// Draws the change password page
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangePassword()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }

        public ActionResult RequestForRecords()
        {
            return View();
        }

        /// <summary>
        /// Forgot password.
        /// </summary>
        /// <returns>View Type.</returns>
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return this.View();
        }

        /// <summary>
        /// Forgets the password.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="captchaValid">if set to <c>true</c> [captcha valid].</param>
        /// <param name="captchaErrorMessage">The captcha error message.</param>
        /// <returns>Action Result</returns>
        [HttpPost]
        public ActionResult ForgotPassword(CheckUsernameViewModel model)
        {
            if (model == null)
            {
                return null;
            }

            // this.Logger.Debug(string.Format("ForgotPassword : User {0}", model.UserName));
            if (ModelState.IsValid && model != null)
            {

                try
                {
                    AlertService alertService = new AlertService();
                    // Object for data access layer
                    FedcoWalletRepository dal = new FedcoWalletRepository();
                    // Check if the entered credentials match database records
                    var userData = dal.GetUserdetails(model.UserName);

                    if (userData == null)
                    {
                        ModelState.AddModelError(string.Empty, "Email address doesn't exist in our records.");
                        return this.View(model);
                    }
                    else
                    {
                        var guid = Guid.NewGuid().ToString();
                        var userid = dal.InsertResetcode(userData.UserId, guid);
                        var url = ConfigurationManager.AppSettings["ResetPasswordUrl"];
                        url = string.Format(url, model.UserName, guid);
                        alertService.SendMail(new EmailModel()
                        {
                            Subject = "Reset Password Request",
                            Type = MailType.ResetPassword,
                            ForgotPasswordLink = url,
                            ToAddress = userData.EmailId,
                            SupportEmail = ConfigurationManager.AppSettings["InfoEmail"],
                            SupportPhone = ConfigurationManager.AppSettings["PhoneContact"]
                        });
                        return this.View("ForgotPasswordSuccess");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Invalid User Name Please try again.";
                    ModelState.AddModelError(string.Empty, ex.Message);
                }


                ModelState.AddModelError(string.Empty, "");
            }

            return this.View(model);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <param name="email">reset password for selected user email</param>
        /// <param name="id">reset password for selected user id.</param>
        /// <returns>Reset Password View.</returns>
        [HttpGet]
        public ActionResult ResetPassword(string name, string guid)
        {
           
            var resetPasswordModel =new ResetPasswordModel();
            try
            {
                // Object for data access layer
                FedcoWalletRepository dal = new FedcoWalletRepository();
                // Check if the entered credentials match database records
                var userData = dal.GetUserdetails(name);

                resetPasswordModel =new ResetPasswordModel { Id = userData.UserId, Email = userData.EmailId, Name = userData.Name };

                if (resetPasswordModel==null)
                {
                    ModelState.AddModelError(string.Empty, "Your request for reset password has expired.Please raise a request for reset again from login page.");
                }
            }
            catch (Exception ex)
            {
                //this.Logger.Error("Error in Reset Password", ex);
                ModelState.AddModelError(string.Empty, "Your request for reset password has expired.Please raise a request for reset again from login page.");
            }

            return this.View(resetPasswordModel);
        }

        /// <summary>
        /// Resets password
        /// </summary>
        /// <param name="resetPasswordModel">ResetPasswordModel Type.</param>
        /// <returns>View Object</returns>
        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel)
        {
            if (resetPasswordModel == null)
            {
                return null;
            }

            if (ModelState.IsValid && resetPasswordModel != null)
            {
                // Object for data access layer
                FedcoWalletRepository dal = new FedcoWalletRepository();
                var mapObjUser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.ChangePasswordModel, Fedco.PHED.Agent.management.Models.ChangePassword>();
                Fedco.PHED.Agent.management.Models.ChangePassword changePasswordModel = new Fedco.PHED.Agent.management.Models.ChangePassword();
                changePasswordModel.userId = resetPasswordModel.Id;
                changePasswordModel.NewPassword = resetPasswordModel.NewPassword;
                changePasswordModel.ModifiedById = (int)Session["LoginUserId"];
                var status = dal.ResetPassword(changePasswordModel);
                if (status>0)
                {
                    return this.View("ResetPasswordSuccess");
                }

                ModelState.AddModelError(string.Empty, "Error updating your password.Please try again.");
            }

            return this.View(resetPasswordModel);
        }

        /// <summary>
        /// Push the password change to the database
        /// </summary>
        /// <param name="oldPassword">User's old password</param>
        /// <param name="newPassword">User's new password</param>
        /// <returns></returns>
        public ActionResult SaveNewPassword(Fedco.PHED.Agent.management.Web.Models.ChangePasswordModel changePassword)
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            // Data access layer object
            FedcoWalletRepository dal = new FedcoWalletRepository();
            var mapObjUser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.ChangePasswordModel, Fedco.PHED.Agent.management.Models.ChangePassword>();
            changePassword.userId = (int)Session["LoginUserId"];
            // Make the password change for the user
            var status = dal.ResetPassword(mapObjUser.Translate(changePassword));
            if (status > 0)
            {
                return RedirectToAction("Logout", "Login");
            }
            else
            {
                // Something went wrong, show status
                return View(status.ToString());
            }
            return null;
        }


        [HttpGet]
        public JsonResult UpdateUserStatus(string status, int agentid)
        {
            // Object for data access layerf
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var resultStatus = "";
            switch ((int)Session["UserTypeId"])
            {
                case 2:
                    resultStatus = dal.UpdateAgentStatus(agentid, status == "true" ? "ACTIVE" : "INACTIVE", 0, 1, 0, Session["email"].ToString(), (int)Session["LoginUserId"]);
                    break;
                case 3:
                    resultStatus = dal.UpdateAgentStatus(agentid, status == "true" ? "ACTIVE" : "INACTIVE", 0, 0, 1, Session["email"].ToString(), (int)Session["LoginUserId"]);
                    break;

                case 4:

                    break;

            }

            return new JsonResult { Data = resultStatus, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// OnActionExecuting method.
        /// </summary>
        /// <param name="filterContext">ActionExecutingContext type.</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (filterContext != null && ((System.Web.Mvc.ReflectedActionDescriptor)filterContext.ActionDescriptor).ActionName != "ForgotPassword" && ((System.Web.Mvc.ReflectedActionDescriptor)filterContext.ActionDescriptor).ActionName != "ResetPassword")
            {
                var controller = (Controller)filterContext.Controller;
                var user = Session["UserTypeId"];
                if (user == null)
                {
                    filterContext.Result = this.RedirectToAction("Index", "Login", new { area = string.Empty });
                    return;
                }

            }

            if (filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"] != null)
            {

                string privatekey = ConfigurationManager.AppSettings["RecaptchaPrivateKey"];
                //string privatekey = "6LfHPdwSAAAAAGGy3uq-0nYyBvdK-tPZHvLJxPVb";
                string response = filterContext.RequestContext.HttpContext.Request["g-recaptcha-response"];
                // this.Logger.Debug("g-recaptcha-response: " + response);
                filterContext.ActionParameters["captchaValid"] = Validate(response, privatekey);

            }
        }



        public bool Validate(string mainresponse, string privatekey)
        {

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=" + privatekey + "&response=" + mainresponse);
                //this.Logger.Debug("g-recaptcha-web request: ");
                WebResponse response = req.GetResponse();

                using (StreamReader readStream = new StreamReader(response.GetResponseStream()))
                {
                    string jsonResponse = readStream.ReadToEnd();
                    //this.Logger.Debug("g-recaptcha-web request with response: ");
                    //this.Logger.Debug("g-recaptcha-web request with response: " + jsonResponse);
                    JsonResponseObject jobj = JsonConvert.DeserializeObject<JsonResponseObject>(jsonResponse);
                    return jobj.success;
                }
            }
            catch (Exception)
            {

                return false;

            }

        }

        public class JsonResponseObject
        {
            public bool success { get; set; }

            public List<string> errorcodes { get; set; }
        }

    }
}
