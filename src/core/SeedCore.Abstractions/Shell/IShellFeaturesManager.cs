using SeedCore.Addon;
using SeedCore.Addon.Features;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SeedCore.Shell
{
    public interface IShellFeaturesManager
    {
        Task<IEnumerable<IFeatureInfo>> GetEnabledFeaturesAsync();
        Task<IEnumerable<IFeatureInfo>> GetAlwaysEnabledFeaturesAsync();
        Task<IEnumerable<IFeatureInfo>> GetDisabledFeaturesAsync();
        Task<(IEnumerable<IFeatureInfo>, IEnumerable<IFeatureInfo>)> UpdateFeaturesAsync(
            IEnumerable<IFeatureInfo> featuresToDisable, IEnumerable<IFeatureInfo> featuresToEnable, bool force);
        Task<IEnumerable<IExtensionInfo>> GetEnabledExtensionsAsync();
    }
}
