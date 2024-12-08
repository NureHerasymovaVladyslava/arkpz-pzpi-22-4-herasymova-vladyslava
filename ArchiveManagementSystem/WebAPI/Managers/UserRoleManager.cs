using Core.Models;
using DAL;

namespace WebAPI.Managers
{
    public class UserRoleManager
    {
        public const string RoleAdmin = "Admin";
        public const string RoleUser = "User";
        public const string RoleManager = "Manager";
        public const string RoleDatabaseAdmin = "DatabaseAdmin";

        private readonly GenericRepository<UserRole> _userRoleRepository;

        public UserRoleManager(GenericRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<bool> IsUserInRoles(AppUser user, string[] roles)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(user.RoleId);
            foreach (var role in roles)
            {
                if (role.Equals(userRole))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
