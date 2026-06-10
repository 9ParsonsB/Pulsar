namespace TestProject1;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using Pulsar;
using Pulsar.Features;

[TestFixture]
public class FileWatcherServiceTests
{
    [Test]
    public async Task HandleFileChanged_ProcessesOldestFirst()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = new PulsarConfiguration { JournalDirectory = tempDir, ProcessHistoricalJournals = true };
            var options = Options.Create(config);
            var fileHandlerService = Substitute.For<IFileHandlerService>();
            var logger = NullLogger<FileWatcherService>.Instance;

            var service = new FileWatcherService(options, fileHandlerService, logger);

            // Create files in reverse order of name
            var file1 = Path.Combine(tempDir, "Journal.2024-01-01T000000.01.log");
            var file2 = Path.Combine(tempDir, "Journal.2024-01-02T000000.01.log");
            var file3 = Path.Combine(tempDir, "Journal.2024-01-03T000000.01.log");

            File.WriteAllText(file3, "test3");
            File.WriteAllText(file2, "test2");
            File.WriteAllText(file1, "test1");

            var processedFiles = new List<string>();
            fileHandlerService.HandleFile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    processedFiles.Add((string)x[0]);
                    return Task.CompletedTask;
                });

            // Act
            await service.StartAsync(default);

            // Assert
            Assert.That(processedFiles.Count, Is.EqualTo(3));
            Assert.That(Path.GetFileName(processedFiles[0]), Is.EqualTo("Journal.2024-01-01T000000.01.log"));
            Assert.That(Path.GetFileName(processedFiles[1]), Is.EqualTo("Journal.2024-01-02T000000.01.log"));
            Assert.That(Path.GetFileName(processedFiles[2]), Is.EqualTo("Journal.2024-01-03T000000.01.log"));
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Test]
    public async Task HandleFileChanged_SkipsHistoricalByDefault()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = new PulsarConfiguration { JournalDirectory = tempDir, ProcessHistoricalJournals = false };
            var options = Options.Create(config);
            var fileHandlerService = Substitute.For<IFileHandlerService>();
            var logger = NullLogger<FileWatcherService>.Instance;

            var service = new FileWatcherService(options, fileHandlerService, logger);

            // Create multiple journals and a non-journal file
            var file1 = Path.Combine(tempDir, "Journal.2024-01-01T000000.01.log");
            var file2 = Path.Combine(tempDir, "Journal.2024-01-02T000000.01.log");
            var file3 = Path.Combine(tempDir, "Journal.2024-01-03T000000.01.log");
            var marketFile = Path.Combine(tempDir, "Market.json");

            File.WriteAllText(file1, "test1");
            File.WriteAllText(file2, "test2");
            File.WriteAllText(file3, "test3");
            File.WriteAllText(marketFile, "market");

            var processedFiles = new List<string>();
            fileHandlerService.HandleFile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    processedFiles.Add((string)x[0]);
                    return Task.CompletedTask;
                });

            // Act
            await service.StartAsync(default);

            // Assert
            Assert.That(processedFiles.Count, Is.EqualTo(3),
                "Expected the latest 2 journals and the market file to be processed");
            Assert.That(processedFiles.Any(f => Path.GetFileName(f) == "Journal.2024-01-02T000000.01.log"), Is.True);
            Assert.That(processedFiles.Any(f => Path.GetFileName(f) == "Journal.2024-01-03T000000.01.log"), Is.True);
            Assert.That(processedFiles.Any(f => Path.GetFileName(f) == "Market.json"), Is.True);
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }

    [Test]
    public async Task HandleFileChanged_ProcessesLastTwoJournalsByDefault()
    {
        // Arrange
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(tempDir);

        try
        {
            var config = new PulsarConfiguration { JournalDirectory = tempDir, ProcessHistoricalJournals = false };
            var options = Options.Create(config);
            var fileHandlerService = Substitute.For<IFileHandlerService>();
            var logger = NullLogger<FileWatcherService>.Instance;

            var service = new FileWatcherService(options, fileHandlerService, logger);

            // Create multiple journals
            var file1 = Path.Combine(tempDir, "Journal.2024-01-01T000000.01.log");
            var file2 = Path.Combine(tempDir, "Journal.2024-01-02T000000.01.log");
            var file3 = Path.Combine(tempDir, "Journal.2024-01-03T000000.01.log");
            var file4 = Path.Combine(tempDir, "Journal.2024-01-04T000000.01.log");

            File.WriteAllText(file1, "test1");
            File.WriteAllText(file2, "test2");
            File.WriteAllText(file3, "test3");
            File.WriteAllText(file4, "test4");

            var processedFiles = new List<string>();
            fileHandlerService.HandleFile(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                {
                    processedFiles.Add((string)x[0]);
                    return Task.CompletedTask;
                });

            // Act
            await service.StartAsync(default);

            // Assert
            Assert.That(processedFiles.Count, Is.EqualTo(2), "Expected only the last 2 journals to be processed");
            Assert.That(processedFiles.Any(f => Path.GetFileName(f) == "Journal.2024-01-03T000000.01.log"), Is.True);
            Assert.That(processedFiles.Any(f => Path.GetFileName(f) == "Journal.2024-01-04T000000.01.log"), Is.True);
        }
        finally
        {
            if (Directory.Exists(tempDir)) Directory.Delete(tempDir, true);
        }
    }
}