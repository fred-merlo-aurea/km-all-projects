create PROCEDURE [dbo].[sp_SubscriberVistActivity]
(@SubscriptionID int)	
AS
BEGIN
	set nocount on

	select 
		VisitActivityID, 
		ActivityDate, 
		DomainName, 
		URL
	from 
		SubscriberVisitActivity sva with (nolock) join 
		DomainTracking dt with (nolock) on sva.DomainTrackingID = dt.DomainTrackingID 
	where 
		SubscriptionID = @SubscriptionID 
	order by ActivityDate desc
END