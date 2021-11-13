using Domain.Common.Contants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.UserManagements
{
    public class PasswordReset
    {
        public string Token { get; set; }
        public string Email { get; set; }
        [MinLength(LengthContants.LENGTH_8)]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsEqualPassword => NewPassword.Equals(ConfirmPassword);
    }
}
