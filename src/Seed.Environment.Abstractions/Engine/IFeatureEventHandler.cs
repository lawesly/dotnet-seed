using Seed.Environment.Plugins.Features;

namespace Seed.Environment.Engine
{
    public interface IFeatureEventHandler
    {
        void Installing(IFeatureInfo feature);

        void Installed(IFeatureInfo feature);

        void Enabling(IFeatureInfo feature);

        void Enabled(IFeatureInfo feature);

        void Disabling(IFeatureInfo feature);

        void Disabled(IFeatureInfo feature);

        void Uninstalling(IFeatureInfo feature);

        void Uninstalled(IFeatureInfo feature);
    }
}
