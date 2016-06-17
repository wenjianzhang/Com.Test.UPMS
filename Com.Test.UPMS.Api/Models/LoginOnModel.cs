﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Com.Test.UPMS.Api.Models
{
    public class LoginOnModel
    {
        public int UserId { get; set; }

        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "下次自动登陆")]
        public bool RememberMe { get; set; }
    }
}