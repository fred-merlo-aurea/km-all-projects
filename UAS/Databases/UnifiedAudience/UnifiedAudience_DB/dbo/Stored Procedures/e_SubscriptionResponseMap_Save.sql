CREATE PROCEDURE [dbo].[e_SubscriptionResponseMap_Save]
@SubscriptionID int,
@ResponseID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@ResponseOther varchar(300)
AS
BEGIN
	
	SET NOCOUNT ON
	
	IF EXISTS(Select SubscriptionID From SubscriptionResponseMap With(NoLock) Where SubscriptionID = @SubscriptionID AND ResponseID = @ResponseID) 
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SubscriptionResponseMap
			SET IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				ResponseOther = @ResponseOther
			WHERE SubscriptionID = @SubscriptionID AND ResponseID = @ResponseID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SubscriptionResponseMap (SubscriptionID,ResponseID,IsActive,DateCreated,CreatedByUserID,ResponseOther)
			VALUES(@SubscriptionID,@ResponseID,@IsActive,@DateCreated,@CreatedByUserID,@ResponseOther);SELECT @@IDENTITY;
		END

END