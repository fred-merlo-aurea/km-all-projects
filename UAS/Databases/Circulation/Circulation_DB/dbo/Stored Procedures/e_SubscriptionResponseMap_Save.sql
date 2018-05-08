CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Save]
@SubscriptionID int,
@CodeSheetID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ResponseOther varchar(300)
AS

IF EXISTS(Select SubscriptionID From SubscriptionResponseMap With(NoLock) Where SubscriptionID = @SubscriptionID AND ResponseID = @CodeSheetID) 
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE SubscriptionResponseMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			ResponseOther = @ResponseOther
		WHERE SubscriptionID = @SubscriptionID AND ResponseID = @CodeSheetID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
		VALUES(@SubscriptionID,@CodeSheetID,@IsActive,@DateCreated,@CreatedByUserID,@ResponseOther);SELECT @@IDENTITY;
	END
