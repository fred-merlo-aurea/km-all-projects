CREATE proc [dbo].[spDataRefreshPart6]
as
BEGIN
	Update Subscriptions 
	set PubIDs = x.PubIDs
	from Subscriptions 
		join(SELECT SubscriptionID,
			STUFF((
					SELECT ', ' + convert(varchar(10), pubID)  
					FROM PubSubscriptions 
					WHERE SubscriptionID = s.SubscriptionID
					FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
					,1,2,'') AS PubIDs
			FROM Subscriptions s
			GROUP BY SubscriptionID) x on Subscriptions.SubscriptionID = x.SubscriptionID
	
	update subscriptions
	set EmailExists = (case when ltrim(rtrim(isnull(email,''))) <> '' then 1 else 0 end),
		FaxExists = (case when ltrim(rtrim(isnull(Fax,''))) <> '' then 1 else 0 end),
		PhoneExists = (case when ltrim(rtrim(isnull(PHONE,''))) <> '' then 1 else 0 end)

END