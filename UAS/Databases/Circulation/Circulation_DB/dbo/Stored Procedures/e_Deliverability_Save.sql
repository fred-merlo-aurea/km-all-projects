CREATE PROCEDURE e_Deliverability_Save
@DeliverabilityID int,
@DeliverabilityName nvarchar(50),
@DeliverabilityCode nchar(10),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @DeliverabilityID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Deliverability
		SET 
			DeliverabilityName = @DeliverabilityName,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE DeliverabilityID = @DeliverabilityID;
		
		SELECT @DeliverabilityID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Deliverability (DeliverabilityName,DeliverabilityCode,IsActive,DateCreated,CreatedByUserID)
		VALUES(@DeliverabilityName,@DeliverabilityCode,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
