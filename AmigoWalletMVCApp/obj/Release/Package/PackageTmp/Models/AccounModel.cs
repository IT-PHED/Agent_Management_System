using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fedco.PHED.Agent.management.Web.Models
{
 
    /// <summary>
    /// ChangePasswordModel class.
    /// </summary>    
    public class ChangePasswordModel
    {
        /// <summary>
        /// Gets or sets the old password.
        /// </summary>
        /// <value>The old password.</value>        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>        
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } 

        public int userId { get; set; }
    }

    /// <summary>
    /// LogOnModel class.
    /// </summary>    
    public class LogOnModel
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>The name of the user.</value>       
        [Required(ErrorMessage = "Please enter user name.")]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        /// <value>The password.</value>        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to Remember the credentials.
        /// </summary>
        /// <value><c>true</c> if [remember me]; otherwise, <c>false</c>.</value>        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// RegisterModel class.
    /// </summary>    
    public class RegisterModel
    {
        /// <summary>
        /// Gets or sets the UserName.
        /// </summary>
        /// <value>The name of the user.</value>        
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>The email.</value>        
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        /// <value>The password.</value>       
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the ConfirmPassword.
        /// </summary>
        /// <value>The confirm password.</value>       
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// Reset Password Model class.
    /// </summary>    
    public class ResetPasswordModel
    {
        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        /// <value>The new password.</value>        
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirm password.
        /// </summary>
        /// <value>The confirm password.</value>        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets Email address of the customer/contact to reset the password
        /// </summary>
        /// <value>the customer email address</value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets customer/contact ID to reset the password
        /// </summary>
        /// <value>customer/contact ID to reset the password</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets if reset password for multiple customer/contacts then this fields used to identify which user password to reset.
        /// </summary>
        /// <value>if reset password for multiple customer/contacts then this fields used to identify which user password to reset.</value>
        public bool Modify { get; set; }

        /// <summary>
        /// Gets or sets Name of the customer/contact to reset the password
        /// </summary>
        /// <value> Name of the customer/contact to reset the password</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets To identify the user type (customer/contact)
        /// </summary>
        /// <value>To identify the user type (customer/contact)</value>
        public string Type { get; set; }
    }
}
