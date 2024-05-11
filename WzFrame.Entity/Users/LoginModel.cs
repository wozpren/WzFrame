using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WzFrame.Entity.Users
{
    public class LoginInput
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Captcha { get; set; } = string.Empty;

        [Required]
        public string CaptchaId { get; set; } = string.Empty;

        [Required]
        public bool RememberMe { get; set; }
    }

    public class RegisterInput
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Captcha { get; set; } = string.Empty;

        [Required]
        public string CaptchaId { get; set; } = string.Empty;
    }

    public class RegisterInputApi
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Captcha { get; set; } = string.Empty;

        [Required]
        public string CaptchaId { get; set; } = string.Empty;
    }

    public class LoginInputApi
    {
        [Required]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Captcha { get; set; } = string.Empty;

        [Required]
        public string CaptchaId { get; set; } = string.Empty;

        [Required]
        public bool RememberMe { get; set; }
    }
}
