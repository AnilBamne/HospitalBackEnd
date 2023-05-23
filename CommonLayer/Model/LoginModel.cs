using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CommonLayer.Model
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please enter valid email"), MaxLength(30)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please enter valid password"), MaxLength(30)]
        public string Password { get; set; }
    }
}
