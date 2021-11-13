using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.UserManagements
{
    public class UserPermission
    {
        [Key]
        [Required]
        public string UserId { get; set; }

        public bool IsFullPermission { get; set; }
    }
}