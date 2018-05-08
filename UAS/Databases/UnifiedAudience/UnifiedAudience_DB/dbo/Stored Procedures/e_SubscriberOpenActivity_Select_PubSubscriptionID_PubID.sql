CREATE PROCEDURE [dbo].[e_SubscriberOpenActivity_Select_PubSubscriptionID_PubID]
@PubSubscriptionID int,
@PubID int
AS
BEGIN

	SET NOCOUNT ON

	select *
	from PubSubscriptions ps with (nolock) 
		join SubscriberOpenActivity so with (nolock) on so.PubSubscriptionID = ps.PubSubscriptionID 
		left outer join Blast bl with (nolock) on so.BlastID = bl.BlastID
	where ps.PubSubscriptionID = @PubSubscriptionID  and ps.PubID = @PubID

END