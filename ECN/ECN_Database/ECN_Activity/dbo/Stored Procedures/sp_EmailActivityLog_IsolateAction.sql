CREATE PROCEDURE [dbo].[sp_EmailActivityLog_IsolateAction]    

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

	declare @EAID int,
			@EmailID	int,
			@BlastID	int,
			@ActionTypeCode varchar(50),
			@ActionDate	datetime,
			@ActionValue	varchar(255),
			@ActionNotes	varchar(255),
			@BlastLinkID int,
			@UnsubscribeCodeID int	,
			@SuppressedCodeID int,
			@BounceCodeID int,
			@DeleteRecord bit
			
	set @DeleteRecord = 1

	select MIN(EAID) from ecn_Activity.dbo.emailactivitylog 
	
	print (convert(varchar, getdate(), 108)) 

		DECLARE c_EAL CURSOR FORWARD_ONLY FAST_FORWARD  FOR 
		select EAID, EmailID, BlastID, ActionTypeCode, ActionDate, ActionValue, ActionNotes 
		from ecn_Activity.dbo.emailactivitylog 

		OPEN c_EAL  
	  
  		FETCH NEXT FROM c_EAL INTO @EAID,@EmailID,@BlastID,@ActionTypeCode,@ActionDate,@ActionValue,@ActionNotes 

		WHILE @@FETCH_STATUS = 0  
		BEGIN  

			set @DeleteRecord = 1
			
			IF @ActionTypeCode = 'send' or @ActionTypeCode = 'testsend' or @ActionTypeCode = 'TEXTsend' --SENDS
			Begin
				--Insert New Send Records
				INSERT dbo.BlastActivitySends(
						BlastID, EmailID, SendTime, IsOpened, IsClicked, SMTPMessage,  IsResend)
				values
						(@BlastID, @EmailID, @ActionDate, NULL,  NULL,  NULL,  0)
			End
			ELSE IF @ActionTypeCode = 'open' --OPENS
			Begin
				--Insert New Open Records
				INSERT dbo.BlastActivityOpens
						(BlastID, EmailID, OpenTime, BrowserInfo)
				VALUES	
						(@BlastID, @EmailID, @ActionDate, @ActionValue)
	
			End			
			ELSE IF @ActionTypeCode = 'resend' --RESENDS
			Begin
				INSERT dbo.BlastActivityResends
						(BlastID, EmailID, ResendTime)
				values 
						(@BlastID, @EmailID, @ActionDate)		
			End

			ELSE IF @ActionTypeCode = 'conversion'	--CONVERSION
			Begin
				INSERT dbo.BlastActivityConversion
						(BlastID, EmailID, ConversionTime, URL)
				VALUES	
						(@BlastID, @EmailID, @ActionDate, @ActionValue) 			
			End
			ELSE IF @ActionTypeCode = 'refer' --REFER
			Begin
				INSERT dbo.BlastActivityRefer
					(BlastID, EmailID, ReferTime, EmailAddress)
				VALUES	
					(@BlastID, @EmailID, @ActionDate, @ActionValue)
			End
			ELSE IF @ActionTypeCode = 'suppressed' --SUPPRESSED
			Begin
				set @SuppressedCodeID = null
				
				select @SuppressedCodeID = SuppressedCodeID from dbo.SuppressedCodes where SupressedCode = @ActionValue

				INSERT dbo.BlastActivitySuppressed( BlastID, EmailID, SuppressedCodeID)
					VALUES (@BlastID, @EmailID, @SuppressedCodeID)	
			End			
			ELSE IF @ActionTypeCode = 'click' --CLICKS
			Begin
				set @BlastLinkID = null
				
				select @BlastLinkID =  BlastLinkID  FROM  ECN5_COMMUNICATOR.dbo.BlastLinks Where  BlastID = @BlastID and LinkURL = @ActionValue 
								
				INSERT dbo.BlastActivityClicks
						(BlastID, EmailID, ClickTime, URL, BlastLinkID) 
				VALUES 
						(@BlastID, @EmailID, @ActionDate, @ActionValue, @BlastLinkID)
			End			
			ELSE IF @ActionTypeCode = 'bounce' --BOUNCES
			Begin
				set @BounceCodeID = null
				
				select @BounceCodeID = BounceCodeID from dbo.BounceCodes where BounceCode = @ActionValue
							
				--Insert New Bounce Records
					INSERT dbo.BlastActivityBounces
							(BlastID, EmailID, BounceTime, BounceCodeID, BounceMessage)
					VALUES 
							(@BlastID, @EmailID, @ActionDate, @BounceCodeID, @ActionNotes)

			End			
			ELSE IF @ActionTypeCode = 'subscribe' or @ActionTypeCode = 'MASTSUP_UNSUB' or @ActionTypeCode = 'ABUSERPT_UNSUB' or @ActionTypeCode = 'FEEDBACK_UNSUB' 	--UNSUBSCRIBES
			Begin
				set @UnsubscribeCodeID = null
				
				select @UnsubscribeCodeID = UnsubscribeCodeID from dbo.UnsubscribeCodes where UnsubscribeCode = @ActionTypeCode
							
				INSERT dbo.BlastActivityUnSubscribes(BlastID, EmailID, UnsubscribeTime, UnsubscribeCodeID, Comments)
				VALUES (@BlastID, @EmailID, @ActionDate, @UnsubscribeCodeID, @ActionNotes)
			End
			Else
			Begin
				set @DeleteRecord = 0
			End			
			
			--delete the processed record
			if @DeleteRecord  = 1
				delete from EmailActivityLog  where EAID = @EAID

			FETCH NEXT FROM c_EAL INTO @EAID,@EmailID,@BlastID,@ActionTypeCode,@ActionDate,@ActionValue,@ActionNotes 
		End

	CLOSE c_EAL  
	DEALLOCATE c_EAL 		
END
