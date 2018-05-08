
CREATE PROCEDURE [dbo].[e_Subscription_Update_QDate]
@SubscriptionID int,
@QSourceDate datetime,
@UpdatedByUserID int
AS

IF @SubscriptionID > 0
	BEGIN					
		UPDATE Subscription
		SET 
			DateCreated = @QSourceDate,
			DateUpdated = GETDATE(),
			UpdatedByUserID = @UpdatedByUserID
		WHERE SubscriptionID = @SubscriptionID;
		
		SELECT @SubscriptionID;
	END



