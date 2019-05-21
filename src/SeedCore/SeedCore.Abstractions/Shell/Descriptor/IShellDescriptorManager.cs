﻿using SeedCore.Shell.Descriptor.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedCore.Shell.Descriptor
{
    public interface IShellDescriptorManager
    {
        Task<ShellDescriptor> GetShellDescriptorAsync();

        Task UpdateShellDescriptorAsync(
            int priorSerialNumber,
            IEnumerable<ShellFeature> enabledFeatures,
            IEnumerable<ShellParameter> parameters);
    }
}