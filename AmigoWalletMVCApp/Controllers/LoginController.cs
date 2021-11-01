using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Fedco.PHED.Agent.management.DAL;

namespace FedcoWalletMVCApp.Controllers
{
    /// <summary>
    /// Shows and handles user login
    /// </summary>
    public class LoginController : Controller
    {
        /// <summary>
        /// Show the login page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Abandon user session and redirect to landing page
            Session.Clear();
            Session.Abandon();
            System.Web.HttpContext.Current.Session.Abandon();
            SignOut();
            string[] myCookies = Request.Cookies.AllKeys;
            FormsAuthentication.SignOut();
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }

            return View();
        }

        /// <summary>
        /// Handles login process
        /// </summary>
        /// <param name="email">User's email input</param>
        /// <param name="password">User's password input</param>
        /// <returns></returns>
        ///
        [HttpPost]
        public ActionResult Login(Fedco.PHED.Agent.management.Web.Models.LogOnModel logon)
        {
            if (ModelState.IsValid)
            {
                // Create a error message variable for login failure alert
                ViewBag.ErrorMessage = null;

                // Object for data access layer
                FedcoWalletRepository dal = new FedcoWalletRepository();
                try
                {
                    // Check if the entered credentials match database records
                    var userData = dal.Login(logon.UserName, logon.Password);

                    if (userData != null)
                    {
                        // Upon success, create a session with user's emailID and direct user to account home page
                        SignIn(userData.EmailId, true, userData.UserTypeId == 1 ? "ADMIN" : userData.UserTypeId == 2 ? "SUPERAGENT" : userData.UserTypeId == 3 ? "AGENT" : userData.UserTypeId == 6 ? "AUDITOR" : "SUBAGENT");
                        Session["email"] = userData.EmailId;
                        Session["SuperAgentId"] = userData.SuperAgentID;
                        Session["AgentId"] = userData.AgentID;
                        Session["SubAgentId"] = userData.SubAgentID;
                        Session["UserTypeId"] = userData.UserTypeId;
                        Session["LoginUserId"] = userData.UserId;
                        Session["LoginUserisDefaultLogin"] = userData.isDefaultPassword;
                        ViewBag.UserType = userData.UserTypeId;

                        switch (userData.UserTypeId)
                        {
                            case 2:
                                var superAgent = dal.GetSuperAgentByID(Convert.ToInt32(Session["SuperAgentId"].ToString()), "SP_AG_GETSUPERAGENT_BY_AGENTID");
                                if (superAgent.ApprovalStatus == "PENDING")
                                {
                                    ViewBag.ErrorMessage = "Approval status is pending, Try Again";
                                    return View("Index");
                                }
                                else if (superAgent.ApprovalStatus == "REJECTED")
                                {
                                    ViewBag.ErrorMessage = "Approvar has rejected..";
                                    return View("Index");
                                }
                                else if (superAgent.Status == false)
                                {
                                    ViewBag.ErrorMessage = "Account has been De-Activated please contact administrator.";
                                    return View("Index");
                                }
                                break;
                            case 3:
                                var Agent = dal.GetSuperAgentByID(Convert.ToInt32(Session["AgentId"].ToString()), "SP_AG_GETAGENT_BY_AGENTID");
                                if (Agent.Status == false)
                                {
                                    ViewBag.ErrorMessage = "Account has been De-Activated please contact Super Agent.";
                                    return View("Index");
                                }
                                break;
                            case 4:
                                var subAgent = dal.GetSuperAgentByID(Convert.ToInt32(Session["SubAgentId"].ToString()), "SP_AG_GETSUBAGENT_BY_AGENTID");
                                if (subAgent.Status == false)
                                {
                                    ViewBag.ErrorMessage = "Account has been De-Activated please contact Agent.";
                                    return View("Index");
                                }
                                break;
                            
                                
                        }


                        if (userData.isDefaultPassword == true)
                        {
                            return RedirectToAction("ChangePassword", "Account");
                        }

                        switch (userData.UserTypeId)
                        {
                            case 1:
                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "ADMIN" });
                                return RedirectToAction("Index", "Admin");
                            case 2:

                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "SUPERAGENT" });
                                return RedirectToAction("Home", "Account");
                            case 3:


                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "AGENT" });
                                return RedirectToAction("Home", "Account");
                            case 4:
                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "SUBAGENT" });
                                return RedirectToAction("Home", "Account");
                            case 5:
                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "APPROVAR" });
                                return RedirectToAction("ApprovarSuperAgentsList", "Admin");

                            case 6:
                                Response.Cookies.Add(new System.Web.HttpCookie("UserType") { Value = "USER" });
                                return RedirectToAction("Home", "Account");

                        }


                    }
                    // Return the error message on failure
                    else
                    {
                        ViewBag.ErrorMessage = "Password/Email Combination Incorrect, Try Again";
                        return View("Index");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Invalid User Name/Password. Please try again.";
                    return View("Index");
                }
            }
            ViewBag.ErrorMessage = "Password/Email is required, Try Again";
            return View("Index");
        }

        /// <summary>
        /// Handles logout and abandon session
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            // Abandon user session and redirect to landing page
            Session.Clear();
            Session.Abandon();
            Response.Cookies.Clear();
            System.Web.HttpContext.Current.Session.Abandon();
            SignOut();
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            return RedirectToAction("Index", "Login");
        }

        /// <summary>
        /// Forms authentication login,uses login and password provided
        /// </summary>
        /// <param name="userName">The username</param>
        /// <param name="createPersistentCookie">creates persistent cookie</param>
        /// <param name="roles">The roles.</param>
        private void SignIn(string userName, bool createPersistentCookie, string roles)
        {
        
            var authTicket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.AddDays(30),
                createPersistentCookie,
                roles,
                "/");

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket));

            if (authTicket.IsPersistent)
            {
                cookie.Expires = authTicket.Expiration;
            }

           
           Response.Cookies.Add(cookie);
        }

        private void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}
