CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Save]
@UserClientSecurityGroupMapID int,
@UserID int,
@ClientID int,
@SecurityGroupID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @UserClientSecurityGroupMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE UserClientSecurityGroupMap
		SET 
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE UserClientSecurityGroupMapID = @UserClientSecurityGroupMapID;

		SELECT @UserClientSecurityGroupMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO UserClientSecurityGroupMap (UserID,ClientID,SecurityGroupID,IsActive,DateCreated,CreatedByUserID)
		VALUES(@UserID,@ClientID,@SecurityGroupID,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
