CREATE proc [dbo].[spUpdateTopicsDimension]
as
BEGIN
	
	SET NOCOUNT ON

	declare @mastergroupID int

	select @mastergroupID = mastergroupID 
	from MasterGroups 
	where Name = 'master_topics'

	insert into SubscriptionDetails (SubscriptionID, MasterID)
	select inn.subscriptionID, inn.masterID
	from 
	(
		select distinct ps.SubscriptionID, MasterID
		from SubscriberClickActivity s 
			join Mastercodesheet m on s.LinkAlias = m.MasterDesc 
			join PubSubscriptions ps on ps.PubSubscriptionID = s.PubSubscriptionID
		where m.MasterGroupID = @mastergroupID and LEN(ltrim(rtrim(isnull(link,'')))) = 0
	) inn 
		left outer join SubscriptionDetails sd on inn.SubscriptionID = sd.SubscriptionID and inn.MasterID = sd.MasterID
	where sd.sdID is null
	
	delete 
	from SubscriberClickActivity 
	where LEN(ltrim(rtrim(isnull(link,'')))) = 0
		
End