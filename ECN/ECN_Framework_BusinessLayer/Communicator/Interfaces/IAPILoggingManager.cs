namespace ECN_Framework_BusinessLayer.Communicator.Interfaces
{
    public interface IAPILoggingManager
    {
        int Insert(ECN_Framework_Entities.Communicator.APILogging log);
        void UpdateLog(int apiLogId, int? logId);
    }
}