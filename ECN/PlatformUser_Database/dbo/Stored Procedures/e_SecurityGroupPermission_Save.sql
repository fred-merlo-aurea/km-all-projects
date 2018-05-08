CREATE PROCEDURE [dbo].[e_SecurityGroupPermission_Save]
	@SecurityGroupPermissionID int = null,
	@SecurityGroupID int,
	@ServiceFeatureAccessMapID int,
	@IsActive bit,
	@CreatedByUserID int,
	@UpdatedByUserID int
AS
BEGIN
	--DECLARE @SecurityGroupPermissionID int = 0;
	
	--SELECT @SecurityGroupPermissionID = SecurityGroupPermissionID 
	--			 FROM SecurityGroupPermission 
	--			WHERE SecurityGroupID = @SecurityGroupID
	--			  AND ServiceFeatureAccessMapID = @ServiceFeatureAccessMapID
	
	IF @SecurityGroupPermissionID is not null
	BEGIN
		UPDATE SecurityGroupPermission
		SET IsActive         = @IsActive,
			DateUpdated      = GETDATE(),
			UpdatedByUserID  = @UpdatedByUserID
		WHERE SecurityGroupPermissionID = @SecurityGroupPermissionID;
		SELECT @SecurityGroupPermissionID;
	END
	ELSE
	BEGIN
		INSERT 
		  INTO SecurityGroupPermission
			  (SecurityGroupID,ServiceFeatureAccessMapID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupID,@ServiceFeatureAccessMapID,@IsActive,GETDATE(),@CreatedByUserID);
		SELECT @@IDENTITY;
	END
END