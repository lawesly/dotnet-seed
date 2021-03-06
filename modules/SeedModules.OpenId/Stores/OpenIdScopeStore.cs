﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using OpenIddict.EntityFrameworkCore;
using Seed.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace SeedModules.OpenId.Stores
{
    public class OpenIdScopeStore : OpenIddictScopeStore<DbContext>
    {
        public OpenIdScopeStore(
            IDbContext context,
            IMemoryCache cache,
            IOptionsMonitor<OpenIddictEntityFrameworkCoreOptions> optionsMonitor)
            : base(cache, context.Context, optionsMonitor)
        {
        }
    }
}
