using System.Collections.Generic;
using FrameworkUAS.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IDqmQue
    {
        bool Save(DQMQue x);
        DQMQue Select(string processCode);
        List<DQMQue> Select(int clientID, bool isDemo, bool isADMS);
        List<DQMQue> Select(bool isDemo, bool isADMS, bool isQued = false);
        List<DQMQue> Select(int clientID, bool isDemo, bool isADMS, bool isQued = false);
        List<DQMQue> Select(bool isDemo, bool isADMS, bool isQued = false, bool isCompleted = false);
        bool UpdateComplete(string processCode, bool createLog = false, string msg = "", int sourceFileId = -1);
    }
}