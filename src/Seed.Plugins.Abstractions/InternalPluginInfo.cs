﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Seed.Plugins.Abstractions.Feature;
using System.Linq;
using Seed.Plugins.Abstractions.Descriptors;

namespace Seed.Plugins.Abstractions
{
    public class InternalPluginInfo : IPluginInfo
    {
        private readonly IFileInfo _fileInfo;
        private readonly string _path;
        private readonly IDescriptorInfo _descriptorInfo;
        private readonly IEnumerable<IFeatureInfo> _features;

        public InternalPluginInfo(string path)
        {
            _path = path;

            _fileInfo = new NotFoundFileInfo(path);
            _descriptorInfo = new NullDescriptorInfo(path);
            _features = Enumerable.Empty<IFeatureInfo>();
        }

        public string Id => _fileInfo.Name;
        public IFileInfo PluginFileInfo => _fileInfo;
        public string Path => _path;
        public IDescriptorInfo Descriptor => _descriptorInfo;
        public IEnumerable<IFeatureInfo> Features => _features;
        public bool Exists => _fileInfo.Exists && _descriptorInfo.Exists;
    }
}
