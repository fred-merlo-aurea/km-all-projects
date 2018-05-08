create PROCEDURE [dbo].[e_SubscriberVisitActivity_Select_BySubscriptionID]
@SubscriptionID int	
AS
BEGIN
	SET NOCOUNT ON;
	
	select 
		VisitActivityID, 
		ActivityDate, 
		DomainName, 
		URL
	from 
		SubscriberVisitActivity sva with(nolock) join 
		DomainTracking dt with(nolock) on sva.DomainTrackingID = dt.DomainTrackingID 
	where 
		SubscriptionID = @SubscriptionID 
	order by ActivityDate desc
END