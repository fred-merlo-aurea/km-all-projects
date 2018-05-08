CREATE PROCEDURE [dbo].[sp_SSB_ErrorMonitor_Target]    
	
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

DECLARE @TargetDB varchar(100),
	@TargetQueue varchar(100),
	@QueueTimeLimit int, --in minutes
	@QueueSizeLimit int, -- in messages
	@NotifyEmailAddress varchar(500),
	@ReportSQL varchar(8000),
	@ErrorCount int

--Assign variable values - these parameters can be added to SP call if additional SSB implementations are added
SELECT @TargetDB = 'ecn_Activity',
	@TargetQueue = 'TargetReportingSyncQueue',
	@QueueTimeLimit = 60,
	@QueueSizeLimit = 1000,
	@NotifyEmailAddress = 'dev@knowledgemarketing.com',
	@ReportSQL = '',
	@ErrorCount = 0


--Check SSB is enabled Target
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT name,
		'is_broker_enabled', 
		is_broker_enabled 
	FROM sys.databases 
	WHERE name = @TargetDB
	AND is_broker_enabled = 0
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT


--Check Target Queue is Enabled
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT name,
		'is_enqueue_enabled', 
		is_enqueue_enabled 
	FROM sys.service_queues 
	WHERE name = @TargetQueue
	AND is_enqueue_enabled = 0
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT
	

--Check that Target queue does not exceed size limit
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT SubQ.QueueName,
		'QueueSize',
		SubQ.RecCount
	FROM (	SELECT q.name AS QueueName,
				p.rows AS RecCount
			FROM sys.service_queues AS q
			JOIN sys.objects AS o ON o.object_id = q.object_id
			JOIN sys.objects AS i ON i.parent_object_id = q.object_id
			JOIN sys.partitions p ON p.object_id = i.object_id AND p.index_id IN(0,1)
			WHERE q.name = @TargetQueue) SubQ
	WHERE SubQ.RecCount > @QueueSizeLimit

	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT


----Check Exceptions table for SP errors
	INSERT @ErrorLog(ObjectName,
		ErrorDesc,
		ErrorValue)
	SELECT 'ExceptionTable',
		'Activity Processing Exceptions',
		SubQ.RecCount
	FROM (	SELECT COUNT(1) AS RecCount
			FROM dbo.BlastActivityExceptions	
			WHERE ExceptionDate >= DATEADD(hh,-1,GETDATE())
			OR IsCleared = 0) SubQ
	WHERE SubQ.RecCount > @QueueSizeLimit
	
	SELECT @ErrorCount = @ErrorCount + @@ROWCOUNT
	
 
 	--Generate Email notification when error is detected
	IF @ErrorCount > 0
	BEGIN
	
		SELECT @ReportSQL = @ReportSQL 
			+ 'SELECT ''' + ObjectName + ''' AS ObjectName, ''' 
			+ ErrorDesc + ''' AS ErrorDesc, '
			+ ErrorValue + ' AS ErrorValue' 
			+ CASE WHEN RecID < @ErrorCount THEN ' UNION ALL ' ELSE '' END
		FROM @ErrorLog
		
		SELECT @ReportSQL
		
		/** DATABASE MAIL MAY NEED TO BE CONFIGURED **/
		EXEC msdb.dbo.sp_send_dbmail
		@profile_name = 'SQLAdmin',
		@recipients = @NotifyEmailAddress,
		@importance='High',
		@body_format = 'HTML',
		@query = @ReportSQL,
		@subject = 'SSB Target Error Detected',
		@attach_query_result_as_file = 1 ;	
		
    END

END
