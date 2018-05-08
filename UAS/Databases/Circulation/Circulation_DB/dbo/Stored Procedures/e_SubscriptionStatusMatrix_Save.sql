CREATE PROCEDURE e_SubscriptionStatusMatrix_Save
@StatusMatrixID int,
@SubscriptionStatusID int,
@CategoryCodeID int,
@TransactionCodeID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @StatusMatrixID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE SubscriptionStatusMatrix
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE StatusMatrixID = @StatusMatrixID;
		
		SELECT @StatusMatrixID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriptionStatusMatrix (SubscriptionStatusID,CategoryCodeID,TransactionCodeID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@SubscriptionStatusID,@CategoryCodeID,@TransactionCodeID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
