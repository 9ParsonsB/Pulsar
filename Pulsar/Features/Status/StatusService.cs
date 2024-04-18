namespace Pulsar.Features.Status;

public class StatusService : IStatusService
{
    public void HandleFile(string fileInfo)
    {
        throw new NotImplementedException();
    }
}

public interface IStatusService : IJournalHandler
{
}

public interface IJournalHandler
{
    void HandleFile(string fileInfo);
}