using Core.Models;
using DAL;

namespace WebAPI.Managers
{
    public class UserRoleManager
    {
        private readonly GenericRepository<UserRole> _userRoleRepository;

        public UserRoleManager(GenericRepository<UserRole> userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<bool> IsUserInRole(AppUser user, string role)
        {
            var userRole = await _userRoleRepository.GetByIdAsync(user.RoleId);
            return userRole.Name.Equals(role);
        }
    }
}
