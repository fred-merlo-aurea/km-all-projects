using System.Collections.Generic;
using FrameworkUAD.BusinessLogic;
using KMPlatform.Object;

using FrameworkUADEntity = FrameworkUAD.Entity;

namespace UAS.UnitTests.Interfaces
{
    public interface IEmailStatus
    {
        List<FrameworkUADEntity.EmailStatus> Select(ClientConnections client);
        FrameworkUADEntity.EmailStatus Select(Enums.EmailStatus status, ClientConnections client);
    }
}