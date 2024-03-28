using Microsoft.AspNetCore.Identity;
using Domain.Common.Enums;


namespace Domain.Entities.Users
{
    public class Role : IdentityRole
    {
        /// <summary>
        /// Role as Enum
        /// </summary>
        //public UserRoleEnum RoleEnum => Name.ToEnum<UserRoleEnum>();
        //  public UserRoleEnum RoleEnum => Name.ToEnum<UserRoleEnum>();
        public UserRoleEnum RoleEnum => (UserRoleEnum)Enum.Parse(typeof(UserRoleEnum), Name, true);
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
