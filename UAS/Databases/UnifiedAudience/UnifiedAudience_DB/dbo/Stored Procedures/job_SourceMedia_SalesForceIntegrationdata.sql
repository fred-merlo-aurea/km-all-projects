create proc [dbo].[job_SourceMedia_SalesForceIntegrationdata] (@PubID int)
as
Begin

	declare @dt date

	select @dt = DATEADD(dd, -1, Getdate())

	select Ps.SubscriptionID 
	from 
			pubsubscriptions ps join 
			Subscriptionpaid sp on ps.PubSubscriptionID = sp.PubSubscriptionID join 
			Pubs p on p.pubID = ps.PubID
	where 
			ps.PubID = @PubID and
			convert(date, ps.DateCreated) = @dt and
			isnull(sp.Amount,0) = 0
	Order by
			P.PubCode		
			
End			
