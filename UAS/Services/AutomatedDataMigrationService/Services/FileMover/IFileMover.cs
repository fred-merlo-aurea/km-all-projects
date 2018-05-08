using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.FileMover
{
    public interface IFileMover
    {
        /// <summary>
        /// Customer drops file into FTP which triggers this event
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleFileDetected(FileDetected eventMessage);
        /// <summary>
        /// HandleFileValidated - Message from Validator that it is done validating and includes validation results
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleFileValidated(FileValidated eventMessage);
        /// <summary>
        /// HandleFileValidated - Message from Validator that it is done with custom file processing
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleCustomFileProcessed(CustomFileProcessed eventMessage);
        event Action<FileMoved> FileMoved;
    }
}
