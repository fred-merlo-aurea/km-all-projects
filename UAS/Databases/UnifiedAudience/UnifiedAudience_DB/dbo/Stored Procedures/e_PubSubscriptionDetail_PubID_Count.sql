CREATE PROCEDURE [dbo].[e_PubSubscriptionDetail_PubID_Count]
@ProductID int
AS
BEGIN

	SET NOCOUNT ON


	select COUNT(*) 
	from PubSubscriptionDetail psd with(nolock) 
		join PubSubscriptions ps with(nolock) on psd.PubSubscriptionID = ps.PubSubscriptionID
	where PubID = @ProductID

END