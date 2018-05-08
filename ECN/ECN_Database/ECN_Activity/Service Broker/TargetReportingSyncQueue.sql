CREATE QUEUE [dbo].[TargetReportingSyncQueue]
    WITH ACTIVATION (STATUS = ON, PROCEDURE_NAME = [dbo].[sp_SSB_MailActions_ProcessQueue], MAX_QUEUE_READERS = 1, EXECUTE AS OWNER);

