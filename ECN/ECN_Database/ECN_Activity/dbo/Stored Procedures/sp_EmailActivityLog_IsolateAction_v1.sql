CREATE PROCEDURE [dbo].[sp_EmailActivityLog_IsolateAction_v1]    

AS
/*=============================================================================
Author:		Sunil Theenathayalu
Date:		02/07/2012
Descr:		Write EmailActivityLog to Activity tables
============================================================================
                             Revision History

Date			Name						Requirement				Change Summary
2012-02-07		Sunil Theenathayalu			ECN Phase III 			Initial Release
==============================================================================*/

BEGIN

	set nocount on

	declare @message_body XML,
			@EAID int,
			@ActionTypeCode varchar(50)
			

select MIN(EAID) from ecn_Activity.dbo.emailactivitylog 
	print (convert(varchar, getdate(), 108)) 

		DECLARE c_EAL CURSOR FORWARD_ONLY FAST_FORWARD  FOR 
		select top 10000000 EAID, ActionTypeCode from ecn_Activity.dbo.emailactivitylog 
		order by EAID asc

		OPEN c_EAL  
	  
  		FETCH NEXT FROM c_EAL INTO @EAID, @ActionTypeCode

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			if (@ActionTypeCode = 'click')
			Begin
				SET @message_body = (SELECT eal.*,
											l.BlastLinkID   
										FROM emailactivitylog eal
										LEFT JOIN ECN5_COMMUNICATOR.dbo.BlastLinks l
										ON eal.BlastID = l.BlastID  
										AND eal.ActionValue = l.LinkURL
										where EAID = @EAID
										FOR XML RAW ('ACTION'), ROOT('ROOT'))  
			End
			Else
			Begin
				SET @message_body = (SELECT eal.*,
											null as BlastLinkID   
										FROM emailactivitylog eal
										where EAID = @EAID
										FOR XML RAW ('ACTION'), ROOT('ROOT'))  	
			End
			
			--print (convert(varchar(8000), @message_body))
				
			/** MAKE SP CALLS BASED ON EMAIL ACTION TYPES RECEIVED IN SSB MESSAGE **/

			IF @ActionTypeCode = 'send' or @ActionTypeCode = 'testsend' or @ActionTypeCode = 'TEXTsend' --SENDS
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivitySends @message_body
			ELSE IF @ActionTypeCode = 'resend' --RESENDS
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityResends @message_body
			ELSE IF @ActionTypeCode = 'open' --OPENS
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityOpens @message_body
			ELSE IF @ActionTypeCode = 'bounce' --BOUNCES
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityBounces @message_body
			ELSE IF @ActionTypeCode = 'click' --CLICKS
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityClicks @message_body
			ELSE IF @ActionTypeCode = 'suppressed' --SUPPRESSED
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivitySuppressed @message_body
			ELSE IF @ActionTypeCode = 'subscribe' or @ActionTypeCode = 'MASTSUP_UNSUB' or @ActionTypeCode = 'ABUSERPT_UNSUB' or @ActionTypeCode = 'FEEDBACK_UNSUB' 	--UNSUBSCRIBES
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityUnsubscribes @message_body
			ELSE IF @ActionTypeCode = 'conversion'	--CONVERSION
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityConversion @message_body
			ELSE IF @ActionTypeCode = 'refer' --REFER
				EXEC dbo.sp_SSB_ProcessEmailAction_BlastActivityRefer @message_body

			delete from EmailActivityLog  where EAID = @EAID

			FETCH NEXT FROM c_EAL INTO @EAID, @ActionTypeCode
		End

	CLOSE c_EAL  
	DEALLOCATE c_EAL 		


END
