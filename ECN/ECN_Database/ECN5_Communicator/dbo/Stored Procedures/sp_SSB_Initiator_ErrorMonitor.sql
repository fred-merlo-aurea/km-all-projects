CREATE PROCEDURE [dbo].[sp_SSB_Initiator_ErrorMonitor]    
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/13/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to monitor SSB components and generate notification in the event an error is detected

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-13		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/

BEGIN  

--Define Temp Table to capture errors
DECLARE @ErrorLog table(
	RecID int IDENTITY(1,1),
	ObjectName varchar(50),
	ErrorDesc varchar(100),
	ErrorValue varchar(50))

DECLARE @InitiatorDB varchar(100),
	@InitiatorTriggerName varchar(100),
	@InitiatorQueue varchar(100),
	@QueueTimeLimit int, --in minutes
	@QueueSizeLimit int, -- in messages
	@NotifyEmailAddress varchar(500),
	@ReportSQL varchar(8000),
	@ErrorCount int

--Assign variable values - these parameters can be added to SP call if additional SSB implementations are added
SELECT @InitiatorDB = 'ecn5_communicator',
	@InitiatorTriggerName = 'tr_SSB_EmailActivityLog_NewAction',
	@InitiatorQueue = 'InitiatorReportingSyncQueue',
	@QueueTimeLimit = 60,
	@QueueSizeLimit = 1000,
	@NotifyEmailAddress = 'dev@knowledgemarketing.com',
	@ReportSQL = '',
	@ErrorCount = 0
	


--Check SSB is enabled on Initiator
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT name,
		'is_broker_enabled', 
		is_broker_enabled 
	FROM sys.databases 
	WHERE name = @InitiatorDB
	AND is_broker_enabled = 0
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT


--Check Initiator Trigger is enabled
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT name,
		'is_disabled', 
		is_disabled 
	FROM sys.triggers 
	WHERE name = @InitiatorTriggerName
	AND is_disabled = 1
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT
	


--Check Initiator Queues is Enabled
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT name,
		'is_enqueue_enabled', 
		is_enqueue_enabled 
	FROM sys.service_queues 
	WHERE name IN (@InitiatorQueue)
	AND is_enqueue_enabled = 0
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT


--Check that no messages have exceeded allowable time in queue
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT 'InitiatorTransmissionQueue',
		'QueueTime',
		DATEDIFF(mi,enqueue_time,GETDATE())
	FROM sys.transmission_queue
	WHERE DATEDIFF(mi,enqueue_time,GETDATE()) > @QueueTimeLimit
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT
	

--Check that transmission queue does not exceed size limit
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT 'InitiatorTransmissionQueue',
		'QueueSize',
		SubQ.RecCount
	FROM (	SELECT COUNT(1) AS RecCount
			FROM sys.transmission_queue) SubQ
	WHERE SubQ.RecCount > @QueueSizeLimit
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT
	
	
	--Generate Email notification when error is detected
	IF @ErrorCount > 0
	BEGIN
	
		SELECT @ReportSQL = @ReportSQL + 'SELECT ''' + ObjectName + ''' AS ObjectName, ''' 
			+ ErrorDesc + ''' AS ErrorDesc, '
			+ ErrorValue + ' AS ErrorValue' 
			+ CASE WHEN RecID < @ErrorCount THEN ' UNION ALL ' ELSE '' END
		FROM @ErrorLog
		
		/** DATABASE MAIL MAY NEED TO BE CONFIGURED **/
		EXEC msdb.dbo.sp_send_dbmail
		@profile_name = 'SQLAdmin',
		@recipients = @NotifyEmailAddress,
		@importance='High',
		@body_format = 'HTML',
		@query = @ReportSQL,
		@subject = 'SSB Initiator Error Detected ',
		@attach_query_result_as_file = 1 ;	
    END

END
