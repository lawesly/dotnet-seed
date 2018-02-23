﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Seed.Data;
using Seed.Environment.Caching;
using Seed.Security;
using Seed.Security.Services;
using SeedModules.Admin.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SeedModules.Admin.Roles
{
    public class RoleStore : IRoleClaimStore<IRole>, IRoleProvider
    {
        private const string Key = "RolesManager.Roles";

        readonly IDbContext _dbContext;
        readonly ISignal _signal;
        readonly IMemoryCache _memoryCache;
        readonly IServiceProvider _serviceProvider;

        public RoleStore(
            IDbContext dbContext,
            ISignal signal,
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _signal = signal;
            _memoryCache = memoryCache;
            _serviceProvider = serviceProvider;
        }

        public Task AddClaimAsync(IRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            ((Role)role).RoleClaims.Add(new RoleClaim { ClaimType = claim.Type, ClaimValue = claim.Value });

            return Task.CompletedTask;
        }

        public Task<IdentityResult> CreateAsync(IRole role, CancellationToken cancellationToken)
        {
            _dbContext.Set<Role>().Add((Role)role);
            _dbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(IRole role, CancellationToken cancellationToken)
        {
            var set = _dbContext.Set<Role>();
            var oldrole = set.Find(((Role)role).Id);
            set.Remove(oldrole);
            _dbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {

        }

        public Task<IRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            var role = _dbContext.Set<Role>().Find(Convert.ToInt32(roleId));
            return Task.FromResult<IRole>(role);
        }

        public Task<IRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            var exrole = _dbContext.Set<Role>().FirstOrDefault(e => e.NormalizedRolename == normalizedRoleName);
            return Task.FromResult<IRole>(exrole);
        }

        public Task<IList<Claim>> GetClaimsAsync(IRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            return Task.FromResult<IList<Claim>>(((Role)role).RoleClaims.Select(x => x.ToClaim()).ToList());
        }

        public Task<string> GetNormalizedRoleNameAsync(IRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(((Role)role).NormalizedRolename);
        }

        public Task<string> GetRoleIdAsync(IRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(((Role)role).Id.ToString());
        }

        public Task<string> GetRoleNameAsync(IRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Rolename);
        }

        public Task<IEnumerable<string>> GetRoleNamesAsync()
        {
            var result = _dbContext.Set<Role>().Select(e => e.Rolename).ToArray();
            return Task.FromResult<IEnumerable<string>>(result);
        }

        public Task RemoveClaimAsync(IRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (role == null)
            {
                throw new ArgumentNullException(nameof(role));
            }

            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            ((Role)role).RoleClaims.RemoveAll(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value);

            return Task.CompletedTask;
        }

        public Task SetNormalizedRoleNameAsync(IRole role, string normalizedName, CancellationToken cancellationToken)
        {
            ((Role)role).NormalizedRolename = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IRole role, string roleName, CancellationToken cancellationToken)
        {
            ((Role)role).Rolename = roleName;
            return Task.CompletedTask;
        }

        public Task<IdentityResult> UpdateAsync(IRole role, CancellationToken cancellationToken)
        {
            _dbContext.Context.Update((Role)role);
            _dbContext.SaveChanges();
            return Task.FromResult(IdentityResult.Success);
        }
    }
}