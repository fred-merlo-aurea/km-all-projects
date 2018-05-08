CREATE PROCEDURE [dbo].[spGreatClipsEmailDateUpdate]
AS
BEGIN
	SET NOCOUNT ON;
	
	declare @dt datetime

	select @dt = convert(varchar(10), DATEADD(dd, -1, getdate()), 101)

	update GreatClips..email
	set EmailSentDate = inn.EmailDateStamp,
		DateUpdated = GETDATE()
	from GreatClips..email e join
	(select EmailAddress, max(ActionDate) as EmailDateStamp
	from Emails e with (NOLOCK) join [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (NOLOCK) on e.CustomerID = c.customerID 
	join
	emailactivitylog eal with (NOLOCK) on e.emailID = eal.emailID and eal.actiontypecode= 'send'
	where
			BaseChannelID = 48 and 
			c.CustomerID not in (2835, 3012) and
			eal.actiontypecode= 'send' 
			and ActionDate >=  @dt 
	group by emailaddress
	) inn on inn.emailaddress = e.emailaddress
			

END
