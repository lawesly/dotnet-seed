using System.IO;
using Microsoft.Extensions.FileProviders;
using SeedModules.Project.Models;

namespace SeedModules.Project.Services
{
    public interface IProjectReader
    {
        ProjectDescriptor ReadDescriptor(Stream stream);

        ProjectDescriptor ReadDescriptor(IFileInfo fileInfo);
    }
}