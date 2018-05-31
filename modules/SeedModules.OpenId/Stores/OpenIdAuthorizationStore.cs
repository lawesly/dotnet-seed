﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OpenIddict.EntityFrameworkCore;
using Seed.Data;

namespace SeedModules.OpenId.Stores
{
    public class OpenIdAuthorizationStore : OpenIddictAuthorizationStore<DbContext>
    {
        public OpenIdAuthorizationStore(IDbContext context, IMemoryCache cache) : base(context.Context, cache)
        {
        }
    }
}
