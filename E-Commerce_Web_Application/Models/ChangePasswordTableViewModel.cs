using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ChangePasswordTableViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Required")]
        [Display(Name="Your Password")]
        public string YourPassword { get; set; }
        [Required(ErrorMessage = "Required")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Display(Name="Confirm Password")]
        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}