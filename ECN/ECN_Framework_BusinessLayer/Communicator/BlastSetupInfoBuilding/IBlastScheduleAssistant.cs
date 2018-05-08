using Entities = ECN_Framework_Entities.Communicator;

namespace ECN_Framework_BusinessLayer.Communicator.BlastSetupInfoBuilding
{
    interface IBlastScheduleAssistant
    {
        Entities.BlastSetupInfo PrepareDailySetupInfo();
        Entities.BlastSetupInfo PrepareMonthlySetupInfo();
        Entities.BlastSetupInfo PrepareOneTimeSetupInfo();
        Entities.BlastSetupInfo PrepareWeeklySetupInfo();
    }
}