CREATE PROCEDURE e_PaidBillTo_Select_SubscriptionID
@PubSubscriptionID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM PaidBillTo With(NoLock)
	WHERE PubSubscriptionID = @PubSubscriptionID

END