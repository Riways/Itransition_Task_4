using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Task_4.Models
{
    public class UserModel : IdentityUser
    {
        public DateTime RegistrationDate { get; set; }
        public DateTime LastVisitDate { get; set; }
        public bool isBlocked { get; set; }
    }
}
