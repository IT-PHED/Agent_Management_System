using Fedco.PHED.Agent.management.DAL;
using Fedco.PHED.Agent.management.Models;
using Fedco.PHED.Agent.management.ViewModels;
using Fedco.PHED.Agent.management.Web.Controllers;
using Fedco.PHED.Agent.management.Web.Enum;
using Fedco.PHED.Agent.management.Web.Models;
using Fedco.PHED.Agent.management.Web.ViewModels;
using FedcoWalletMVCApp.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FedcoWalletMVCApp.Controllers
{
    /// <summary>
    /// Allows user registration
    /// </summary>
    public class RegisterController : Controller
    {

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult SuperAgentRegister()
        {
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            var userViewModel = new Fedco.PHED.Agent.management.ViewModels.UserViewModel();
            userViewModel.Agent = new Fedco.PHED.Agent.management.Web.Models.AgentModel();
            userViewModel.User = new Fedco.PHED.Agent.management.Web.Models.User();
            FedcoWalletRepository dal = new FedcoWalletRepository();
            // Calculate balance points and pass it to the view
            var list = new List<Fedco.PHED.Agent.management.Web.Models.UserType>();

            switch ((int)Session["UserTypeId"])
            {
                case 1:
                    list = dal.GetAllUserTypes().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserType { Id = m.Id, userType = m.userType }).Where(T => T.userType == "SUPER AGENT").ToList();
                    ViewBag.ViewBalance = 0;
                    break;
                case 2:
                    list = dal.GetAllUserTypes().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserType { Id = m.Id, userType = m.userType }).Where(T => T.userType == "AGENT").ToList();
                    userViewModel.Agent.SecurityDeposit = 0;
                    ViewBag.ViewBalance = dal.GetSuperAgentByID(Convert.ToInt32(Session["SuperAgentId"].ToString()), "SP_AG_GETSUPERAGENT_BY_AGENTID").Balance;
                    break;
                case 3:
                    list = list = dal.GetAllUserTypes().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserType { Id = m.Id, userType = m.userType }).Where(T => T.userType == "SUB AGENT").ToList();
                    userViewModel.Agent.SecurityDeposit = 0;
                    ViewBag.ViewBalance = dal.GetSuperAgentByID(Convert.ToInt32(Session["AgentId"].ToString()), "SP_AG_GETAGENT_BY_AGENTID").Balance;
                    break;
                case 4:
                    list = list = dal.GetAllUserTypes().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserType { Id = m.Id, userType = m.userType }).Where(T => T.userType == "SUB AGENT").ToList();
                    userViewModel.Agent.SecurityDeposit = 0;
                    ViewBag.ViewBalance = dal.GetSuperAgentByID(Convert.ToInt32(Session["SubAgentId"].ToString()), "SP_AG_GETSUBAGENT_BY_AGENTID").Balance;
                    break;
            }
            //Get latest Agent Code
            var AgentCode = dal.GetLatestAgentCode("SP_AG_GETLATEST_SUPERAGENT");
            userViewModel.Agent.AgentCode = "PHED-" + String.Format("{0:00000}", Convert.ToUInt32(AgentCode.Substring(AgentCode.IndexOf('-') + 1)) + 1);
            userViewModel.userTypes = list;
            userViewModel.User.UserTypeId = (int)Session["UserTypeId"];

            return View(userViewModel);
        }

        [Authorize]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult RegisterApprovar()
        {
           
            if (Session["UserTypeId"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
            {
                return RedirectToAction("ChangePassword", "Account");
            }

            var approvarViewModel = new Fedco.PHED.Agent.management.Web.ViewModels.ApprovarUserModel();
            approvarViewModel.User = new Fedco.PHED.Agent.management.Web.Models.User();
            FedcoWalletRepository dal = new FedcoWalletRepository();
            // Calculate balance points and pass it to the view
            var list = new List<Fedco.PHED.Agent.management.Web.Models.UserType>();
            switch ((int)Session["UserTypeId"])
            {
                case 1:
                    list = dal.GetAllUserTypes().Select(m => new Fedco.PHED.Agent.management.Web.Models.UserType { Id = m.Id, userType = m.userType }).Where(T => T.userType == "APPROVAR" ||  T.userType == "USER").ToList();                    

                    break;
            }
            
                approvarViewModel.userTypes = list;
                approvarViewModel.User.UserTypeId = (int)Session["UserTypeId"];
            

            return View(approvarViewModel);
        }

        public ActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ValidateUserName(string userName)
        {
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var userNameCount = dal.ValidateUserName(userName);
            return new JsonResult { Data = userNameCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult ValidateMobile(string mobile)
        {
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var usermobileCount = dal.ValidateUserMobile(mobile);
            return new JsonResult { Data = usermobileCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public JsonResult ValidateEmail(string email)
        {
            // Object for data access layer
            FedcoWalletRepository dal = new FedcoWalletRepository();
            //validate user name exists
            var useremailCount = dal.ValidateUserEmail(email);
            return new JsonResult { Data = useremailCount, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// Show the registration form page
        /// </summary>
        /// <returns></returns>
        /// <summary>
        /// Register the user to the database with the entered user info
        /// </summary>
        /// <param name="user">User model object with user information</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register/RegisterUser/")]
        public ActionResult RegisterUser(UserViewModel userModel)
        {
            try
            {
                AlertService alertService = new AlertService();
                if (Session["UserTypeId"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }

                if (Session["LoginUserisDefaultLogin"] != null && (Boolean)Session["LoginUserisDefaultLogin"] == true)
                {
                    return RedirectToAction("ChangePassword", "Account");
                }

                if ((int)Session["UserTypeId"] != 1 && userModel.Agent.SecurityDeposit == null)
                {
                    userModel.Agent.SecurityDeposit = 0;
                }

                if (ModelState.IsValid)
                {

                    // Object for data access layer
                    FedcoWalletRepository dal = new FedcoWalletRepository();
                   
                    foreach (string fileName in Request.Files)
                    {
                        if (fileName == "file")
                        {
                            HttpPostedFileBase file = Request.Files[fileName];
                            if (!Directory.Exists(Server.MapPath("~/uploads/")))
                            {
                                Directory.CreateDirectory("~/uploads/");
                            }
                            var path = Path.Combine(Server.MapPath("~/uploads/"), file.FileName);
                            userModel.Agent.filePath = path;
                            file.SaveAs(path);
                        }
                    }
                    // Default values for agent creation
                    userModel.Agent.Status = true;
                    userModel.Agent.CreatedDate = System.DateTime.Now;
                    userModel.Agent.CreatedBy = Session["email"].ToString();
                    userModel.Agent.CreatedById = (int)Session["LoginUserId"];
                    userModel.User.CreatedById = (int)Session["LoginUserId"];
                    userModel.User.CreatedTimestamp = System.DateTime.Now;
                    userModel.User.CreatedBy = Session["email"].ToString();
                    userModel.User.Password = "phed123";
                    // Maps User from model to DAL
                    var mapObjAgent = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.AgentModel, Agent>();
                    var mapObjUser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.User, Fedco.PHED.Agent.management.Models.User>();
                    var storedProcName = string.Empty;
                    var agentId = 0;

                    switch ((int)Session["UserTypeId"])
                    {
                        case 1:
                            storedProcName = "sp_AG_Create_Superagent";
                            agentId = dal.CreateSuperAgent(mapObjAgent.Translate(userModel.Agent), storedProcName, null, null);
                            break;
                        case 2:
                            storedProcName = "sp_AG_Create_agent";
                            agentId = dal.CreateSuperAgent(mapObjAgent.Translate(userModel.Agent), storedProcName, (int)Session["SuperAgentId"], null);
                            break;

                        case 3:
                            storedProcName = "sp_AG_Create_SUBagent";
                            agentId = dal.CreateSuperAgent(mapObjAgent.Translate(userModel.Agent), storedProcName, null, (int)Session["AgentId"]);
                            break;

                    }


                    if (agentId > 0)
                    {
                        var status1 = 0;
                        switch ((int)Session["UserTypeId"])
                        {
                            case 1:
                                status1 = dal.CreateUserDetails_SuperAgent(mapObjUser.Translate(userModel.User), (int)agentId, 0, 0);

                                break;
                            case 2:
                                status1 = dal.CreateUserDetails_SuperAgent(mapObjUser.Translate(userModel.User), 0, (int)agentId, 0);

                                break;

                            case 3:
                                status1 = dal.CreateUserDetails_SuperAgent(mapObjUser.Translate(userModel.User), 0, 0, (int)agentId);

                                break;

                        }


                        switch ((int)Session["UserTypeId"])
                        {
                            case 1:
                                ViewBag.Message = "Super agent:" + userModel.Agent.AgentName + " was created successfully.";
                                break;

                            case 2:
                                ViewBag.Message = "Agent:" + userModel.Agent.AgentName + " was created successfully.";
                                break;

                            case 3:
                                ViewBag.Message = "Sub agent:" + userModel.Agent.AgentName + " was created successfully.";
                                break;

                        }
                        var url = ConfigurationManager.AppSettings["appURL"];
                        alertService.SendMail(new EmailModel()
                        {
                            Subject = "New Agent Creation",
                            Type = MailType.NewAgentEmail,
                            SITEURL = url,
                            UserName = userModel.User.Name,
                            ToAddress = userModel.User.EmailId,
                            PASSWORD = userModel.User.Password,
                           // ToAddress = "perumala1215@gmail.com",
                            SupportEmail = ConfigurationManager.AppSettings["InfoEmail"],
                            SupportPhone = ConfigurationManager.AppSettings["PhoneContact"]
                        });
                        return View();
                    }
                    else
                    {
                        Session["AgentId"] = agentId;
                        Session["regError"] = "Something went wrong. Please try again.";
                        return RedirectToAction("Error", "Register");
                    }
                }
                else
                {
                    return this.View(userModel);
                }
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
            return null;
        }

        /// <summary>
        /// Register the user to the database with the entered user info
        /// </summary>
        /// <param name="user">User model object with user information</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register/RegisterApprovar/")]
        public ActionResult RegisterApprovar(ApprovarUserModel approvarModel)
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



                if (ModelState.IsValid)
                {

                    // Object for data access layer
                    FedcoWalletRepository dal = new FedcoWalletRepository();

                    // Default values for approvar creation
                    approvarModel.User.CreatedTimestamp = System.DateTime.Now;
                    approvarModel.User.CreatedBy = Session["email"].ToString();
                    approvarModel.User.CreatedById = (int)Session["LoginUserId"];
                    approvarModel.User.Password = "phed123";
                    // Maps User from model to DAL
                    var mapObjUser = new FedcoWalletMapper<Fedco.PHED.Agent.management.Web.Models.User, Fedco.PHED.Agent.management.Models.User>();

                    var status = 0;
                    status = dal.CreateUserDetails_SuperAgent(mapObjUser.Translate(approvarModel.User), 0, 0, 0);
                    if (status > 0)
                    {
                        ViewBag.Message = "Approvar:" + approvarModel.User.Name + " was created successfully.";
                    }
                    else
                    {
                        Session["regError"] = "Something went wrong. Please try again.";
                        return RedirectToAction("Error", "Register");
                    }
                    return RedirectToAction("ApprovarsList", "Admin");
                }
                else
                {
                    return this.View(approvarModel);
                }
            }
            catch (Exception ex)
            {
                // For debugging exceptions
                if (ex.ToString().Contains("0001"))
                {
                    Session["regError"] = ExecptionMessages.uniqueContraint;
                }

                else
                {
                    Session["regError"] = ex.ToString();
                }
                return RedirectToAction("Error", "Register");
            }
        }
    }
}
