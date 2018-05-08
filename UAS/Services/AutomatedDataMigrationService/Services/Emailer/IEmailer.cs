using System;
using System.Linq;
using Core.ADMS.Events;

namespace ADMS.Services.Emailer
{
    public interface IEmailer
    {
        /// <summary>
        /// Once file has been processed emailer invoked and sends out summary of validation to the Customer
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleFileProcessed(FileProcessed eventMessage);
        void HandleCustomFileProcessed(CustomFileProcessed eventMessage);
        
    }
}
