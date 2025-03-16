using Microsoft.Extensions.Logging;
using System.Diagnostics;
using TreeMiner;
using XstReader;

internal class Main(ILogger<Main> logger) 
{


    public async Task RunAsync(string pstPath, CancellationToken cancellationToken = default)
    {

        var pstName = new FileInfo(pstPath).Name;
        logger.LogInformation("Reading {pst}", pstName);

        var stopwatch = new Stopwatch();
        stopwatch.Start();

        try
        {

            using var xstFile = new XstFile(pstPath);
            XstFolder root = xstFile.RootFolder;

            var xstMiner = new GenericTreeMiner<XstArtifact, XstElement, XstMessage, XstFolder>();

            var exceptionAggregate = new List<Exception>();


            var artifacts = xstMiner.GetArtifacts(
                xstMiner.GetRootArtifact(xstFile.RootFolder),
                getArtifacts: (xstFolder) => xstFolder.GetFolders().Concat(xstFolder.GetMessages() as IEnumerable<XstElement>),
                exceptionAggregate: exceptionAggregate);

            foreach (var artifact in artifacts)
            {
                if (artifact.Info is XstFolder folder)
                {
                    logger.LogInformation("{Id} {Parent}\t{Name}", artifact.Id, artifact.ParentId, folder.DisplayName);
                }

                if (artifact.Info is XstMessage message)
                {
                    //foreach (var attachment in message.Attachments)
                    //{
                    //    attachment.SaveToStream(new MemoryStream());
                    //}

                    logger.LogInformation("{Id} {Parent}\t{Subject}", artifact.Id, artifact.ParentId, message.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, $"{pstName} failed.");
        }

        stopwatch.Stop();

        logger.LogInformation("Mailbox {pst} read in {elapsed}", pstName, stopwatch.Elapsed);


        await Task.CompletedTask;
    }
}
