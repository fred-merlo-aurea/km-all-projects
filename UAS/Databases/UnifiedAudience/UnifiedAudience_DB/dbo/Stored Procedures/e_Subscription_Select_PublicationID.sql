CREATE PROCEDURE e_Subscription_Select_PublicationID
@PublicationID int
AS
BEGIN
	
	SET NOCOUNT ON
	
	SELECT * 
	FROM ProductSubscription With(NoLock) 
	WHERE PubID = @PublicationID

END