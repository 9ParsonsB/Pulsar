using System.Diagnostics.CodeAnalysis;
using Lamar;
using Pulsar.Features;
using Pulsar.Features.ModulesInfo;

namespace Pulsar;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class PulsarServiceRegistry : ServiceRegistry
{
    public PulsarServiceRegistry()
    {
        For<IFileHandlerService>().Use<FileHandlerService>();
        For<IStatusService>().Use<StatusService>();
        For<IModulesInfoService>().Use<ModulesInfoService>();
    }
}