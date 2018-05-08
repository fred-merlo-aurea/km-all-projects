using System;
using System.Collections.Generic;
using Core.ADMS.Events;
using FrameworkUAD.Entity;
using FrameworkUAD_Lookup;
using FrameworkUAS.Entity;
using KMPlatform.Entity;
using UasObject = FrameworkUAS.Object;

namespace UAS.UnitTests.Interfaces
{
    public interface IServiceBase
    {
        Client client { get; set; }
        Dictionary<int, string> clientPubCodes { get; set; }
        EngineLog engineLog { get; set; }
        bool logTranDetail { get; set; }

        void ConsoleMessage(string message, string processCode = "", bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0);
        void ConsoleMessage(AdmsLog admsLog, string message, string processCode = "", bool createLog = true, int sourceFileId = 0, int updatedByUserId = 0);
        bool ExecutionPointExist(Enums.ExecutionPointType ept, SourceFile sourceFile);
        HashSet<UasObject.RuleSet> GetCurrentDateSpecificRuleSet(HashSet<UasObject.RuleSet> ruleSets);
        DateTime? GetQDate(SourceFile sourceFile);
        HashSet<UasObject.RuleSet> GetRuleSetsByExecutionPoint(Enums.ExecutionPointType ept, SourceFile sourceFile);
        HashSet<SubscriberTransformed> GetValidEmail(HashSet<SubscriberTransformed> listPubCodeValid, SourceFile sourceFile);
        bool HasCurrentDateSpecificRuleSet(HashSet<UasObject.RuleSet> ruleSets);
        bool HasDateSpecificRuleSet(HashSet<UasObject.RuleSet> ruleSets);
        void LogError(Exception ex, Client client, string msg, bool removeThread = true, bool removeQue = true);
        void LogError(Exception ex, Client client, string msg, AdmsLog admsLog, bool removeThread = true, bool removeQue = true);
        void SaveEngineLog(string msg);
        void SetPropertiesNull();
        void SetStaticProperties(FileDetected fd);
        void UnhandledExceptionLog(object sender, UnhandledExceptionEventArgs e);
    }
}