CREATE PROCEDURE [dbo].[sp_SSB_MailActions_ProcessQueue]    
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP process email activity messages in the SSB queue

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/

BEGIN  

DECLARE @message_body as XML    
DECLARE @Handle_ID as uniqueidentifier    
  

	/*We process this within a never ending loop so that we can drain the 
	queue on each run instead of having to launch the procedure one for each message.*/  
	  
	WHILE 1=1
	BEGIN        
		WAITFOR (
			RECEIVE TOP (1) @Handle_ID = conversation_handle,
				@message_body=cast(message_body as xml)                
			FROM [TargetReportingSyncQueue]
			), TIMEOUT 1000 
			/*If no message is received then we wait for 1 second.  
			If no message arrives in that time continue to the next command which will 
			break us out of the procedure so that we are not running the procedure for 
			no reason wasting CPU and memory resources.*/        
			
			IF (@@ROWCOUNT = 0)            
			BREAK    
		    
		--END CONVERSATION @Handle_ID WITH CLEANUP
		/*By ending the conversation we are telling SQL01 that we have removed the message 
		from the queue and that it no longer needs to remember that the message exists.*/        
	
		IF @message_body IS NOT NULL 
		/*Checking just in case.  We don’t want to bother trying to process a message if it is null.*/        
		BEGIN            
			EXEC dbo.sp_SSB_MailActions_IsolateAction @message_body = @message_body                   
		END
	END
END
