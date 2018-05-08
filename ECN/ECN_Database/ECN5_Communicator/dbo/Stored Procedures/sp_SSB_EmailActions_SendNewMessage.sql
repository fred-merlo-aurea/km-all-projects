CREATE PROCEDURE [dbo].[sp_SSB_EmailActions_SendNewMessage]    
	@message_body XML
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to generate SSB message to Service on ecn_Activity (reporting) db for new email actions

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/
BEGIN 
BEGIN TRANSACTION 

    EXEC dbo.sp_SSB_CreateConversation
	    @Source ='//ECNCommunicator/ReportingSync/InitiatorService'
	    ,@Destination = '//ECNActivity/ReportingSync/TargetService'
	    ,@Contract = '//ECN/ReportingSync/Contract'
	    ,@MessageType = '//ECN/ReportingSync/RequestMessage'
	    ,@MessageBody = @message_body
	    
COMMIT TRANSACTION;
	
END
