CREATE PROCEDURE [dbo].[e_SubscriptionPaid_Select_SubscriptionID]
	@PubSubscriptionID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	Select * 
	FROM SubscriptionPaid with(nolock)
	WHERE PubSubscriptionID = @PubSubscriptionID

END