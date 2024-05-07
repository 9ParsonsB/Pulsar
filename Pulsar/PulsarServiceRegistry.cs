using System.Diagnostics.CodeAnalysis;
using Lamar;
using Pulsar.Features;
using Pulsar.Features.ModulesInfo;
using Pulsar.Features.Journal;

namespace Pulsar;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
public class PulsarServiceRegistry : ServiceRegistry
{
    public PulsarServiceRegistry()
    {
        For<IFileHandlerService>().Use<FileHandlerService>();
        For<IStatusService>().Use<StatusService>();
        For<IModulesInfoService>().Use<ModulesInfoService>();
        For<IJournalService>().Use<JournalService>();
    }
}