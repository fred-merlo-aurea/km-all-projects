CREATE PROCEDURE [dbo].[e_PubSubscriptions_Select_PubID_SubscriptionID]
@PubID int,
@SubscriptionID int
AS
	select *
	from PubSubscriptions ps with(nolock)
    where SubscriptionID = @SubscriptionID and PubID = @PubID