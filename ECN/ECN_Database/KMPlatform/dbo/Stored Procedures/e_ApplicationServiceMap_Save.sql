CREATE PROCEDURE [dbo].[e_ApplicationServiceMap_Save]
@ApplicationServiceMapID int,
@ApplicationID int,
@ServiceID int,
@IsEnabled bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN
	DECLARE @UpdateID int = (SELECT ApplicationServiceMapID FROM ApplicationServiceMap m WHERE m.ApplicationID = @ApplicationID AND m.ServiceID = @ServiceID);
	IF @UpdateID IS NOT NULL AND @UpdateID > 0
	BEGIN
		SET @DateUpdated = GETDATE();
		UPDATE ApplicationServiceMap
		SET 
			IsEnabled = @IsEnabled,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ApplicationServiceMapID = @UpdateID;
		SELECT @UpdateID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ApplicationServiceMap (ApplicationID,ServiceID,IsEnabled,DateCreated,CreatedByUserID)
		VALUES(@ApplicationID,@ServiceID,@IsEnabled,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
END