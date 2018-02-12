﻿using Microsoft.AspNetCore.Authorization;
using Seed.Security;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Seed.Security.Permissions;
using System.Security.Claims;
using SeedModules.Admin.Domain;
using System.Linq;

namespace SeedModules.Admin.Roles
{
    public class RolesPermissionsHandler : AuthorizationHandler<PermissionRequirement>
    {
        readonly RoleManager<IRole> _roleManager;

        public RolesPermissionsHandler(RoleManager<IRole> roleManager)
        {
            _roleManager = roleManager;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.HasSucceeded)
            {
                return;
            }

            var grantingNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            PermissionNames(requirement.Permission, grantingNames);

            var rolesToExamine = new List<string> { "Anonymous" };

            if (context.User.Identity.IsAuthenticated)
            {
                rolesToExamine.Add("Authenticated");
                foreach (var claim in context.User.Claims)
                {
                    if (claim.Type == ClaimTypes.Role)
                    {
                        rolesToExamine.Add(claim.Value);
                    }
                }
            }

            foreach (var roleName in rolesToExamine)
            {
                var role = await _roleManager.FindByNameAsync(roleName);

                if (role != null)
                {
                    foreach (var claim in ((Role)role).RoleClaims)
                    {
                        if (!String.Equals(claim.ClaimType, Permission.ClaimType, StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        string permissionName = claim.ClaimValue;

                        if (grantingNames.Contains(permissionName))
                        {
                            context.Succeed(requirement);
                            return;
                        }
                    }
                }
            }
        }

        private static void PermissionNames(Permission permission, HashSet<string> stack)
        {
            stack.Add(permission.Name);

            if (permission.IncludeBy != null && permission.IncludeBy.Any())
            {
                foreach (var includeBy in permission.IncludeBy)
                {
                    if (stack.Contains(includeBy.Name))
                    {
                        continue;
                    }

                    PermissionNames(includeBy, stack);
                }
            }

            stack.Add(StandardPermissions.SiteOwner.Name);
        }
    }
}
