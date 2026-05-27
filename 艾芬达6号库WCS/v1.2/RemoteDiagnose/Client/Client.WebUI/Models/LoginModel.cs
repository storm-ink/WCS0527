using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Client
{
    public class LoginModel
    {
        [Display(Name="用户名")]
        [Required] 
        public string UserName { get; set; }

        [Required]
        [Display(Name = "密码")]
        public string Password { get; set; }


    }

  
}