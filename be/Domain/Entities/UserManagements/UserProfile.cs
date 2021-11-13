using Domain.Common.Contants;
using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserManagements
{
    public class UserProfile : EntityBase
    {
        [Required]
        public string UserId { get; set; }

        [StringLength(LengthContants.LENGTH_255)]
        public string Email { get; set; }

        [StringLength(LengthContants.LENGTH_100)]
        public string NickName { get; set; }

        public string PhoneNumber { get; set; }
        public string FullName { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}