using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserManagements.UserProfile.Dtos
{
    public class DetailsDto
    {
        public string LoginId { get; set; }
        public string UserId { get; set; }
        public string NickName { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> RolePermission { get; set; }
    }
}
