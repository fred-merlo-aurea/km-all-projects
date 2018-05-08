-- =============================================
-- Author:		Rohit Pooserla
-- Create date: 9/18/2012
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertSMSActivity]
(
	@xmldata xml,
	@blastID int,
	@IsWelcomeMsg bit
)AS
BEGIN
	SET NOCOUNT ON;
	declare @dt datetime;
	set @dt=GETDATE();
	
	declare @SMSActivityData TABLE (EmailID int);	
			
	insert into @SMSActivityData
	SELECT BlastValues.SMS.value('./@EmailID','INT')
	FROM @xmldata.nodes('/Blast') as BlastValues(SMS) ;

	if(@IsWelcomeMsg=1)
	BEGIN
		Insert into SMSActivityLog (EmailID, BlastID, SendStatus,IsWelcomeMsg, SendTime)
		Select  sms.EmailID, @blastID, 'sent', @IsWelcomeMsg, @dt
		from @SMSActivityData sms
	END
	else
	begin
		Insert into SMSActivityLog (EmailID, BlastID, SendStatus,IsWelcomeMsg, SendTime)
		Select  sms.EmailID, @blastID, 'pending', @IsWelcomeMsg, @dt
		from @SMSActivityData sms
	end
	
	--WRITE CODE TO UPDATE EMAILS TABLE IF WE HAVE OPT OUT RECORDS
END
