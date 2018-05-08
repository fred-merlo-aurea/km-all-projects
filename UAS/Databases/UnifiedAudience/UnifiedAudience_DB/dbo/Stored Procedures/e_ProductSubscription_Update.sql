CREATE PROCEDURE [dbo].[e_ProductSubscription_Update]
@PubSubscriptionID int,
@IsLocked bit,
@UserID int
AS
BEGIN

	SET NOCOUNT ON

	IF (@PubSubscriptionID) > 0
		BEGIN
			IF(@IsLocked) = 1
				BEGIN
					UPDATE PubSubscriptions
					SET IsLocked = @IsLocked,
						LockedByUserID = @UserID,
						LockDate = getdate(),
						LockDateRelease = null
					WHERE PubSubscriptionID = @PubSubscriptionID;
				END
			ELSE
				BEGIN
					UPDATE PubSubscriptions
					SET IsLocked = @IsLocked,
						LockDateRelease = getdate()
					WHERE PubSubscriptionID = @PubSubscriptionID;
				END
			SELECT @PubSubscriptionID;
		END
	ELSE
		BEGIN
			-- This part of the proc should only happen if the application crashes.  If there is a crash, all subscribers
			-- that was locked by the user will get unlocked
			IF(@UserID) > 0 AND @IsLocked = 0
				BEGIN
					UPDATE PubSubscriptions
					SET IsLocked = @IsLocked,
						LockDateRelease = getdate()
					WHERE LockedByUserID = @UserID AND IsLocked = 1
				END
		END

END