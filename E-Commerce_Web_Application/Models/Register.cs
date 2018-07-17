using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class Register
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^[a-zA-Z]\w+|[0-9][0-9_]*[a-zA-Z]+\w*$", ErrorMessage = "Invalid UserName")]
        [Display(Name="User Name:")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name="First Name:")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name="Last Name:")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name="password:")]
        public string Password { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,15}$", ErrorMessage = "Invalid Input")]
        [Compare("Password",ErrorMessage="Password Not Match")]
        [Display(Name="Confirm password:")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        [EmailAddress(ErrorMessage = "Email Address")]
        [Display(Name="Email Id:")]
        public string EmailId { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name="Gender:")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Required")]
        public string VerificationToken { get; set; }

    }
}