CREATE PROCEDURE [dbo].[e_SecurityGroupServiceMap_Save]
@SecurityGroupServicMap int,
@SecurityGroupID int,
@ServiceID int,
@IsEnabled bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN
	DECLARE @UpdateID int = (SELECT m.SecurityGroupServicMap FROM SecurityGroupServicMap m WHERE m.SecurityGroupID = @SecurityGroupID AND m.ServiceID = @ServiceID);
	IF @UpdateID IS NOT NULL AND @UpdateID > 0
	BEGIN
		SET @DateUpdated = GETDATE();
		UPDATE SecurityGroupServicMap
		SET 
			IsEnabled = @IsEnabled,
			DateUpdatd = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE SecurityGroupServicMap = @UpdateID;
		SELECT @UpdateID;
	END
ELSE
	BEGIN
  		SET @DateCreated = GETDATE();
		INSERT INTO SecurityGroupServicMap (SecurityGroupID,ServiceID,IsEnabled,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupID,@ServiceID,@IsEnabled,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
END