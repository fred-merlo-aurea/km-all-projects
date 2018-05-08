CREATE PROCEDURE e_Subscriber_Delete_SubscriberID
	@SubscriberID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM Subscription WHERE SubscriberID = @SubscriberID
	DELETE FROM Subscriber WHERE SubscriberID = @SubscriberID
    
END

