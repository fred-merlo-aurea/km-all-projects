CREATE PROCEDURE [dbo].[spGreatClipsUnsubscribeUpdate] 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    update GreatClips..email
	set EmailOptPref = 'U',
		DateUpdated = inn.updateddate
	from GreatClips..email e join
	(
	select distinct EmailAddress, isnull(eg.LastChanged, GETDATE()) as updateddate
	from	Emails e with (NOLOCK) join 
			[ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c with (NOLOCK) on e.CustomerID = c.customerID join
			emailgroups eg with (NOLOCK) on e.emailID = eg.emailID join 
			groups g with (NOLOCK) on g.groupID = eg.groupID
	where
			BaseChannelID = 48 and 
			c.CustomerID not in (2835, 3012) and
			(g.MasterSupression = 1 or SubscribeTypeCode = 'U')
	) inn on inn.emailaddress = e.emailaddress

END
