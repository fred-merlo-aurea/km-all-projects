using System;
using System.Linq;
using Core.ADMS.Events;
using System.ServiceModel;

namespace ADMS.Services.Validator
{
    [ServiceContract]
    public interface IValidator
    {
        /// <summary>
        /// File has been moved to Client Repository(Repo)
        /// </summary>
        /// <param name="eventMessage"></param>
        void HandleFileMoved(FileMoved eventMessage);
        event Action<FileValidated> FileValidated;
        event Action<CustomFileProcessed> CustomFileProcessed;

        //[OperationContract(Name="AddSubscriber")]
        //FrameworkUAD.ServiceResponse.Response<FrameworkUAD.ServiceResponse.AddSubscriber> AddSubscriber(KMPlatform.Entity.Client client, FrameworkUAD.Object.SaveSubscriber subscription);

        [OperationContract(Name = "SaveSubscriber")]
        FrameworkUAD.ServiceResponse.SavedSubscriber SaveSubscriber(KMPlatform.Entity.Client client, FrameworkUAD.Object.SaveSubscriber subscription);
    }
    
}
