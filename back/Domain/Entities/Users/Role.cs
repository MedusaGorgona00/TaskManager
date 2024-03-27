using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Users
{
    public class Role : IdentityRole
    {
        public UserRoleEnum RoleEnum => Name.ToEnum<UserRoleEnum>();
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
