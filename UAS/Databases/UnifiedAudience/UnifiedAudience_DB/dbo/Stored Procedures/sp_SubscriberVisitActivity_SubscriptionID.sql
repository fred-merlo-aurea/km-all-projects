create PROCEDURE [dbo].[sp_SubscriberVisitActivity_SubscriptionID]
(@SubscriptionID int)	
AS
BEGIN

	SET NOCOUNT ON

	select VisitActivityID, ActivityDate, DomainName, URL
	from SubscriberVisitActivity sva 
		join DomainTracking dt on sva.DomainTrackingID = dt.DomainTrackingID 
	where SubscriptionID = @SubscriptionID 
	order by ActivityDate desc

END