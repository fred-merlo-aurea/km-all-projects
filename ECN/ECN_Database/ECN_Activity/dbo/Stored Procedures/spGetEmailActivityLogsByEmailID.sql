CREATE PROCEDURE [dbo].[spGetEmailActivityLogsByEmailID]
	@EmailID int
AS
BEGIN
	SET NOCOUNT ON
	
	declare @emailActivityLog table(EAID int , EmailID int , BlastID int , ActionTypeCode varchar(500), ActionDate datetime, 
			ActionValue varchar(2048), ActionNotes varchar(500)) 

	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'send' as ActionTypeCode, SendTime as ActionDate, '' as ActionValue, '' as ActionNotes
	from BlastActivitySends with (nolock) where EmailID = @EmailID
		
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'click' as ActionTypeCode, ClickTime as ActionDate, URL as ActionValue, '' as ActionNotes
	from BlastActivityClicks with (nolock) where EmailID = @EmailID
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'bounce' as ActionTypeCode, BounceTime as ActionDate, bc.BounceCode as ActionValue, babo.BounceMessage as ActionNotes
	from BlastActivityBounces babo  with (nolock) 
		join BounceCodes bc with (nolock) on bc.BounceCodeID = babo.BounceCodeID
	where babo.EmailID = @EmailID 
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'conversion' as ActionTypeCode, ConversionTime as ActionDate, URL as ActionValue, '' as ActionNotes
	from BlastActivityConversion with (nolock) where EmailID = @EmailID 	
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'open' as ActionTypeCode, OpenTime as ActionDate, BrowserInfo as ActionValue, '' as ActionNotes
	from BlastActivityOpens with (nolock) where EmailID = @EmailID 
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'refer' as ActionTypeCode, ReferTime as ActionDate, EmailAddress as ActionValue, '' as ActionNotes
	from BlastActivityRefer with (nolock) where EmailID = @EmailID 
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, 'resend' as ActionTypeCode, ResendTime as ActionDate, '' as ActionValue, '' as ActionNotes
	from BlastActivityResends with (nolock) where EmailID = @EmailID 
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, sc.SupressedCode as ActionTypeCode, bas.SuppressedTime as ActionDate, '' as ActionValue, '' as ActionNotes
	from BlastActivitySuppressed bas with (nolock) 
		join SuppressedCodes sc with (nolock) on sc.SuppressedCodeID = bas.SuppressedCodeID where bas.EmailID = @EmailID
	
	insert into @emailActivityLog 
	select EAID, EmailID, BlastID, usc.UnsubscribeCode as ActionTypeCode, baus.UnsubscribeTime as ActionDate, baus.Comments as ActionValue, baus.Comments as ActionNotes
	from BlastActivityUnSubscribes baus with (nolock) 
		join UnsubscribeCodes usc with (nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID where baus.EmailID = @EmailID 
	
		select e.EmailID, e.EmailAddress, eal.BlastID, b.EmailSubject, eal.ActionTypeCode, eal.ActionDate, eal.ActionValue 
	from @emailActivityLog eal 
		join ecn5_communicator..Emails e with (nolock) on e.EmailID = eal.EmailID
		join ecn5_communicator..Blast b with (nolock) on b.BlastID = eal.BlastID and e.EmailID = @EmailID
	UNION
	select e.EmailID, e.EmailAddress, eal.BlastID, '', eal.ActionTypeCode, eal.ActionDate, eal.ActionValue 
	from @emailActivityLog eal 
		join ecn5_communicator..Emails e with (nolock) on e.EmailID = eal.EmailID
	WHERE eal.BlastID = 0 and eal.ActionTypeCode = 'subscribe'
	order by ActionDate desc
END