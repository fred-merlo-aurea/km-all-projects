CREATE PROCEDURE [dbo].[sp_SSB_MailActions_IsolateAction]    
	@message_body XML
	
AS
/*=============================================================================

Author:		Nathan C. Hoialmen	
Date:		02/07/2012
Req:		SSB Sync ECN_Activity (reporting) db with ECN_Communicator
Descr:		Create SP to determine which email activities need to be processed and call appropriate SP

============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Nathan C. Hoialmen			ECN Phase III 			Initial Release


==============================================================================*/

BEGIN

DECLARE @hdoc int

DECLARE @tempActions table(
	RecID int IDENTITY(1,1),
	ActionTypeCode varchar(50))

	EXEC sp_xml_preparedocument @hdoc OUTPUT, @message_body

	--Get Distinct list of ActionTypeCodes
	INSERT @tempActions(ActionTypeCode)
	SELECT DISTINCT LTRIM(RTRIM(a.ActionTypeCode))
	FROM OPENXML(@hdoc, N'/ROOT/ACTION')    
	WITH (EAID int,
		EmailID int,
		BlastID int,
		ActionTypeCode varchar(50),
		ActionDate datetime,
		ActionValue varchar(255),
		ActionNotes varchar(255),
		Processed char(10)) a 
		
	EXEC sp_xml_removedocument @hdoc		
		
	/** MAKE SP CALLS BASED ON EMAIL ACTION TYPES RECEIVED IN SSB MESSAGE **/
	
	--SENDS
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('send','testsend','TEXTsend'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivitySends @message_body
	
	
	--RESENDS
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('resend'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityResends @message_body
	
	
	--OPENS
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('open'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityOpens @message_body
	
	
	--BOUNCES
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('bounce'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityBounces @message_body
	
	
	--CLICKS
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('click'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityClicks @message_body
	
	
	--SUPPRESSED
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('suppressed'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivitySuppressed @message_body
	
	
	--UNSUBSCRIBES
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('subscribe','MASTSUP_UNSUB','ABUSERPT_UNSUB','FEEDBACK_UNSUB'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityUnsubscribes @message_body
	
	
	--CONVERSION
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('conversion'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityConversion @message_body
	
	
	--REFER
	IF EXISTS (SELECT 1 FROM @tempActions WHERE ActionTypeCode IN ('refer'))
	EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityRefer @message_body
	


END
