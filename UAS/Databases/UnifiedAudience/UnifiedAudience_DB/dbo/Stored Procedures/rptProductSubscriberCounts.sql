CREATE proc dbo.rptProductSubscriberCounts
 @pubTypeID int = 0
as
BEGIN
	
	SET NOCOUNT ON

	select pubcounts, Count(subscriptionID)
	from
	(
		select subscriptionID, COUNT(distinct ps.pubID) as pubcounts 
		from PubSubscriptions ps 
			join Pubs p on ps.PubID = p.pubID 
		where (@pubTypeID = 0 or p.PubTypeID = @pubTypeID)		
		group by subscriptionID
	) inn 
	group by pubcounts
	order by 1

End