using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Commerce_Web_Application.Models
{
    public class ViewUserInfoModel
    {

        //[Required(ErrorMessage = "Required")]
        //[RegularExpression(@"^[^\s\,]+$", ErrorMessage = "User Name Can't Have Spaces")]
        public string UserName { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string FirstName { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string LastName { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Required")]
        public string ConfirmPassword { get; set; }
        //[Required(ErrorMessage = "Required")]
        //[EmailAddress(ErrorMessage = "Email Address")]
        public string EmailId { get; set; }
        public string Type { get; set; }
        public string Gender { get; set; }
        public string CheckedIs { get; set; }
    }
}