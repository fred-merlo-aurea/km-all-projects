CREATE PROCEDURE e_DeliverabilityMap_Save
@DeliverabilityID int,
@PublicationID int,
@IsAvailable bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF NOT EXISTS(SELECT DeliverabilityID FROM DeliverabilityMap With(NoLock) WHERE DeliverabilityID = @DeliverabilityID AND PublicationID = @PublicationID)
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO DeliverabilityMap (DeliverabilityID,PublicationID,IsAvailable,DateCreated,CreatedByUserID)
		VALUES(@DeliverabilityID,@PublicationID,@IsAvailable,@DateCreated,@CreatedByUserID);
	END
ELSE
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE DeliverabilityMap
		SET 
			IsAvailable = @IsAvailable,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		 WHERE DeliverabilityID = @DeliverabilityID AND PublicationID = @PublicationID;
	END
