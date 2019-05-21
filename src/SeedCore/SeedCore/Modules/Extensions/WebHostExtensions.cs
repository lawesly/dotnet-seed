﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace SeedCore.Modules
{
    public static class WebHostExtensions
    {
        public static void Run(this IWebHost host, Action<IServiceProvider> action)
        {
            using (var cts = new CancellationTokenSource())
            {
                host.Run(action, cts.Token, "Application started. Press Ctrl+C to shut down.");
            }
        }

        public static void Run(this IWebHost host, Action<IServiceProvider> action, CancellationToken token)
        {
            host.Run(action, token, shutdownMessage: null);
        }

        public static void Run(this IWebHost host, Action<IServiceProvider> action, CancellationToken token, string shutdownMessage)
        {
            using (host)
            {
                host.Start();

                var hostingEnvironment = host.Services.GetService<IHostingEnvironment>();
                var applicationLifetime = host.Services.GetService<IApplicationLifetime>();

                Console.WriteLine($"Hosting environment: {hostingEnvironment.EnvironmentName}");
                Console.WriteLine($"Content root path: {hostingEnvironment.ContentRootPath}");

                var serverAddresses = host.ServerFeatures.Get<IServerAddressesFeature>()?.Addresses;
                if (serverAddresses != null)
                {
                    foreach (var address in serverAddresses)
                    {
                        Console.WriteLine($"Now listening on: {address}");
                    }
                }

                if (!string.IsNullOrEmpty(shutdownMessage))
                {
                    Console.WriteLine(shutdownMessage);
                }

                token.Register(state =>
                {
                    ((IApplicationLifetime)state).StopApplication();
                },
                applicationLifetime);

                action(host.Services);
            }
        }
    }

}