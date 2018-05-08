CREATE PROCEDURE [dbo].[e_SubscriberClickActivity_Select_PubSubscriptionID_PubID]
@PubSubscriptionID int,
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from PubSubscriptions ps with (nolock) 
		join SubscriberClickActivity sc with (nolock) on sc.PubSubscriptionID = ps.PubSubscriptionID 
		left outer join Blast bl with (nolock) on sc.BlastID = bl.BlastID
	where ps.PubSubscriptionID = @PubSubscriptionID  and ps.PubID = @PubID

END