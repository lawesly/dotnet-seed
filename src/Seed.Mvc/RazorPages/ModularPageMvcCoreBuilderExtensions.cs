﻿using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Mvc.RazorPages
{
    public static class ModularPageMvcCoreBuilderExtensions
    {
        public static IMvcCoreBuilder AddModularRazorPages(this IMvcCoreBuilder builder)
        {
            builder.AddRazorPages();

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<RazorPagesOptions>, ModularPageRazorPagesOptionsSetup>());

            builder.Services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPageApplicationModelProvider, ModularPageApplicationModelProvider>());

            return builder;
        }
    }
}
