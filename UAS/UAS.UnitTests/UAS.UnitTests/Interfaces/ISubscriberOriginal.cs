using System;
using System.Collections.Generic;
using KMPlatform.Object;

using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface ISubscriberOriginal
    {
        void FormatData(FrameworkUADEntity.SubscriberOriginal x);
        int Save(FrameworkUADEntity.SubscriberOriginal x, ClientConnections client);
        bool SaveBulkInsert(List<FrameworkUADEntity.SubscriberOriginal> list, ClientConnections client);
        bool SaveBulkSqlInsert(List<FrameworkUADEntity.SubscriberOriginal> list, ClientConnections client);
        bool SaveBulkUpdate(List<FrameworkUADEntity.SubscriberOriginal> list, ClientConnections client);
        List<FrameworkUADEntity.SubscriberOriginal> Select(ClientConnections client);
        List<FrameworkUADEntity.SubscriberOriginal> Select(string processCode, ClientConnections client);
        List<FrameworkUADEntity.SubscriberOriginal> Select(int sourceFileID, ClientConnections client);
        List<FrameworkUADEntity.SubscriberOriginal> Select(string processCode, int sourceFileID, ClientConnections client);
        List<FrameworkUADEntity.SubscriberOriginal> SelectForFileAudit(string processCode, int sourceFileID, DateTime? startDate, DateTime? endDate, ClientConnections client);
    }
}