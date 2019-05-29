using Microsoft.AspNetCore.Identity;
using SeedCore.Data;
using SeedCore.MemberShip.Security.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SeedCore.MemberShip.Services
{
    public class UserStore :
        IUserStore<IUser>,
        IUserRoleStore<IUser>,
        IUserPasswordStore<IUser>,
        IUserEmailStore<IUser>,
        IUserSecurityStampStore<IUser>,
        IUserLoginStore<IUser>
    {
        private readonly IDbContext _context;
        private readonly IRoleService _roleService;
        private readonly ILookupNormalizer _keyNormalizer;

        public UserStore(IDbContext context,
            IRoleService roleService,
            ILookupNormalizer keyNormalizer)
        {
            _context = context;
            _roleService = roleService;
            _keyNormalizer = keyNormalizer;
        }

        public Task AddLoginAsync(IUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task AddToRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public Task<IUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedEmailAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetPasswordHashAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetUserNameAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<IUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public string NormalizeKey(string key)
        {
            return _keyNormalizer == null ? key : _keyNormalizer.Normalize(key);
        }

        public Task RemoveFromRoleAsync(IUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(IUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailAsync(IUser user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(IUser user, bool confirmed, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(IUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(IUser user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetPasswordHashAsync(IUser user, string passwordHash, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetSecurityStampAsync(IUser user, string stamp, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetUserNameAsync(IUser user, string userName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(IUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
