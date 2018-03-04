﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SeedModules.Admin.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeedModules.Admin.Users.Services
{
    public class UserService : IUserService
    {
        readonly UserManager<IUser> _userManager;
        readonly ILookupNormalizer _keyNormalizer;
        readonly IOptions<IdentityOptions> _identityOptions;

        public UserService(
            UserManager<IUser> userManager,
            ILookupNormalizer keyNormalizer,
            IOptions<IdentityOptions> identityOptions)
        {
            _userManager = userManager;
            _keyNormalizer = keyNormalizer;
            _identityOptions = identityOptions;
        }

        public async Task<bool> ChangePasswordAsync(IUser user, string currentPassword, string newPassword, Action<string, string> reportError)
        {
            var identityResult = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            if (!identityResult.Succeeded)
            {
                throw new Exception("修改不成功");
            }

            return identityResult.Succeeded;
        }

        public async Task<IUser> CreateUserAsync(string username, string email, string[] roleNames, string password, Action<string, string> reportError)
        {
            if (await _userManager.FindByEmailAsync(email) != null)
            {
                throw new Exception("Email 重复");
            }

            var user = new User
            {
                Username = username,
                Email = email,
                Roles = roleNames.Select(e => new UserRole()
                {
                    Role = new Role(e)
                    {
                        NormalizedRolename = _keyNormalizer.Normalize(e)
                    }
                }).ToList()
            };

            var identityResult = await _userManager.CreateAsync(user, password);

            if (!identityResult.Succeeded)
            {
                throw new Exception("创建不成功");
            }

            return user;
        }

        public Task<IUser> GetAuthenticatedUserAsync(ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                return Task.FromResult<IUser>(null);
            }

            return _userManager.GetUserAsync(principal);
        }
    }
}
