CREATE SERVICE [//ECNActivity/ReportingSync/TargetService]
    AUTHORIZATION [dbo]
    ON QUEUE [dbo].[TargetReportingSyncQueue]
    ([//ECN/ReportingSync/Contract]);

