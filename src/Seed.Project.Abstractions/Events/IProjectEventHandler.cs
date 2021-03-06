﻿using Seed.Project.Models;
using System.Threading.Tasks;

namespace Seed.Project.Events
{
    public interface IProjectEventHandler
    {
        Task ExecutingAsync(string executionId, ProjectDescriptor descriptor);

        Task StepExecutingAsync(ProjectExecutionContext context);

        Task StepExecutedAsync(ProjectExecutionContext context);

        Task ExecutedAsync(string executionId, ProjectDescriptor descriptor);

        Task ExecutionFailedAsync(string executionId, ProjectDescriptor descriptor);
    }
}
