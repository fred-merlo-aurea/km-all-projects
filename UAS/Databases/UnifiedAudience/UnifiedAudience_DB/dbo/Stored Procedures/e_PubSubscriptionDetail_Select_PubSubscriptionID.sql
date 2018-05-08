CREATE PROCEDURE e_PubSubscriptionDetail_Select_PubSubscriptionID
@PubSubscriptionID int
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM PubSubscriptionDetail with(nolock) 
	WHERE PubSubscriptionID = @PubSubscriptionID    
    
END
GO