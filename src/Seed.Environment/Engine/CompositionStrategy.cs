﻿using Seed.Environment.Abstractions;
using Seed.Environment.Abstractions.Engine;
using Seed.Environment.Abstractions.Engine.Descriptors;
using Seed.Modules.Abstractions;
using Seed.Plugins.Abstractions;
using Seed.Plugins.Abstractions.Feature;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Environment.Engine
{
    public class CompositionStrategy : ICompositionStrategy
    {
        readonly IPluginManager _pluginManager;
        readonly ITypeFeatureProvider _typeFeatureProvider;

        public CompositionStrategy(
            IPluginManager pluginManager,
            ITypeFeatureProvider typeFeatureProvider)
        {
            _pluginManager = pluginManager;
            _typeFeatureProvider = typeFeatureProvider;
        }

        public async Task<EngineSchema> ComposeAsync(EngineSettings settings, EngineDescriptor descriptor)
        {
            var featureNames = descriptor.Features.Select(x => x.Name).ToArray();

            var features = await _pluginManager.GetFeaturesAsync(featureNames);

            var entries = new Dictionary<Type, FeatureEntry>();

            foreach (var feature in features)
            {
                foreach (var exportedType in feature.Exports)
                {
                    var requiredFeatures = RequireFeaturesAttribute.GetRequiredFeatureNamesForType(exportedType);

                    if (requiredFeatures.All(x => featureNames.Contains(x)))
                    {
                        entries.Add(exportedType, feature);
                    }
                }
            }

            var result = new EngineSchema
            {
                Settings = settings,
                Descriptor = descriptor,
                Dependencies = entries
            };

            return result;
        }
    }
}