using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace Seed.Environment.BackgroundTasks
{
    public interface IBackgroundTaskSettingsProvider
    {
        IChangeToken ChangeToken { get; }

        Task<BackgroundTaskSettings> GetSettingsAsync(IBackgroundTask task);
    }
}