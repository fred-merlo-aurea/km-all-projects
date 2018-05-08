CREATE PROCEDURE dbo.e_SubscriberOpenActivity_Insert
@PubSubscriptionID int, 
@BlastID int, 
@ActivityDate date
AS
BEGIN

	SET NOCOUNT ON

	IF NOT EXISTS(Select OpenActivityID From SubscriberOpenActivity with(nolock) WHERE PubSubscriptionID = @PubSubscriptionID AND ActivityDate = @ActivityDate AND @PubSubscriptionID > 0)
		BEGIN
			INSERT INTO SubscriberOpenActivity (PubSubscriptionID, BlastID, ActivityDate)
			VALUES(@PubSubscriptionID,@BlastID,@ActivityDate)
		END

END