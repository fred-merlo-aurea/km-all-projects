CREATE PROCEDURE [job_PTNThirdPartyBTNExport]
as
BEGIN

	SET NOCOUNT ON

	--140	PT Third party BTN 1 of 3

	SELECT DISTINCT s.subscriptionid, s.FNAME, s.LNAME, s.TITLE, s.COMPANY, s.ADDRESS, s.MAILSTOP,
		s.CITY, s.STATE, s.ZIP, s.email
	FROM Subscriptions s WITH (nolock) 
		JOIN pubsubscriptions ps WITH (nolock) ON s.subscriptionid = ps.subscriptionid 
	WHERE ps.pubid IN ( 272, 321, 323, 375, 1 ) 
		AND ps.pubcategoryid IN ( 10 ) 
		AND ps.pubtransactionid IN (SELECT transactionid 
									FROM   [transaction] 
									WHERE  transactiongroupid IN ( 1 )) 
		AND Isnull(ps.emailstatusid, 1) = 1 
		AND s.subscriptionid IN (SELECT sfilter.subscriptionid 
									FROM subscriptions sfilter WITH (nolock) 
										JOIN pubsubscriptions ps1 WITH (nolock) ON ps1.subscriptionid = sfilter.subscriptionid 
									WHERE  ps1.pubid IN ( 272, 321, 323, 375, 1 ) 
										   AND ps1.pubcategoryid IN ( 10 ) 
										   AND ps1.pubtransactionid IN (SELECT transactionid 
																		FROM [transaction] 
																		WHERE transactiongroupid IN ( 1 )) 
										   AND Isnull(ps1.emailstatusid, 1) = 1) 
		AND
		s.SubscriptionID not in
		(
			--141	PT Third party BTN 2 of 3
			select distinct s.SubscriptionID 
			from subscriptions s with (NOLOCK) 
				join pubsubscriptions ps  with (NOLOCK)  on s.subscriptionID = ps.subscriptionID  
			where (s.CountryID in ( 2 )  OR not ((s.CountryID is null) or (s.CountryID = 2))) and  s.SubscriptionID in ( select sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) JOIN pubsubscriptions ps1 WITH (NOLOCK) ON ps1.subscriptionID = sfilter.subscriptionID where (sfilter.CountryID in ( 2 )  OR not ((sfilter.CountryID is null)  or (sfilter.CountryID = 2))) )
			union
			--142	PT Third party BTN 3 of 3
			select distinct s.SubscriptionID 
			from subscriptions s with (NOLOCK) 
				join pubsubscriptions ps  with (NOLOCK)  on s.subscriptionID = ps.subscriptionID  
			where ps.pubid in (442 )  and  s.SubscriptionID in ( select sfilter.SubscriptionID from subscriptions sfilter  with (NOLOCK) JOIN pubsubscriptions ps1 WITH (NOLOCK) ON ps1.subscriptionID = sfilter.subscriptionID where ps1.pubid in (442 )  )
		)

End