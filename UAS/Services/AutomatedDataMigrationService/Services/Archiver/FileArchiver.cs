using System;
using System.IO;
using System.Linq;
using Core.ADMS.Events;
using Core.ADMS.Events.Subscriber;

namespace ADMS.Services.Archiver
{
    class FileArchiver : ServiceBase, IFileArchiver
    {
        private readonly FileProcessedSubscriber _fileProcessedSubscriber;

        public FileArchiver(FileProcessedSubscriber fileProcessedSubscriber)
        {
            _fileProcessedSubscriber = fileProcessedSubscriber;
            _fileProcessedSubscriber.RegisterServiceHandler(HandleFileProcessedEvent);
        }

        private void HandleFileProcessedEvent(FileProcessed eventMessage)
        {
            try
            {
                //React to the event in here.
                MoveFileFromClientRepositoryToArchiveLocation(eventMessage.ImportFile.FullName, eventMessage.Client);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".HandleFileProcessedEvent");
            }
        }

        public void MoveFileFromClientRepositoryToArchiveLocation(string fileFromRepo, KMPlatform.Entity.Client client)
        {
            try
            {
                string fileName = Path.GetFileName(fileFromRepo);
                string clientArchivedLocation = Core.ADMS.BaseDirs.getClientArchiveDir() + "\\" + client.FtpFolder + "\\";
                string copyFullFileNameToRepo = clientArchivedLocation + fileName;

                if (!String.IsNullOrEmpty(clientArchivedLocation))
                    System.IO.File.Copy(fileFromRepo, @copyFullFileNameToRepo);

                FrameworkUAS.BusinessLogic.EngineLog elWrk = new FrameworkUAS.BusinessLogic.EngineLog();
                elWrk.SaveEngineLog("FILEMOVER: " + fileName + " copied to " + clientArchivedLocation + " " + DateTime.Now.ToString(), client.ClientID, FrameworkUAS.BusinessLogic.Enums.Engine.ADMS);
            }
            catch (Exception ex)
            {
                LogError(ex, client, this.GetType().Name.ToString() + ".MoveFileFromClientRepositoryToArchiveLocation");
            }
        }

    }
}
