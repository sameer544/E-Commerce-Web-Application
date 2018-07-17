using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Required")]
        [Display(Name="User Name")]
        [RegularExpression(@"(^[^\s\,]+$)", ErrorMessage = "Worng Input Enter UserName Or Gmail Id")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Required")]
        public string Password { get; set; }
    }
}