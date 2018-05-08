
CREATE PROCEDURE [dbo].[e_Subscriber_Update]
@SubscriberID int,
@IsLocked bit,
@UserID int
AS

IF (@SubscriberID) > 0
BEGIN

	IF(@IsLocked) = 1
	BEGIN
		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockedByUserID = @UserID,
			LockDate = getdate(),
			LockDateRelease = null
		WHERE SubscriberID = @SubscriberID;
	END
	ELSE
	BEGIN
		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockDateRelease = getdate()
		WHERE SubscriberID = @SubscriberID;
	END
		SELECT @SubscriberID;

END
ELSE
BEGIN
	-- This part of the proc should only happen if the application crashes.  If there is a crash, all subscribers
	-- that was locked by the user will get unlocked
	IF(@UserID) > 0 AND @IsLocked = 0
	BEGIN

		UPDATE Subscriber
		SET IsLocked = @IsLocked,
			LockDateRelease = getdate()
		WHERE LockedByUserID = @UserID AND IsLocked = 1

	END
END

