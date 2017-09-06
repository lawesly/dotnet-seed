﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Seed.Plugins.Abstractions.Feature
{
    public class CompiledFeatureEntry : FeatureEntry
    {
        public CompiledFeatureEntry(IFeatureInfo featureInfo, IEnumerable<Type> exports)
        {
            FeatureInfo = featureInfo;
            Exports = exports;
        }
    }
}
