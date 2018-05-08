CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Save]
@UserClientSecurityGroupMapID int,
@UserID int,
@ClientID int,
@SecurityGroupID int,
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@InactiveReason varchar(50) = null
AS

IF @UserClientSecurityGroupMapID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE UserClientSecurityGroupMap
		SET 
			UserID = @UserID,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			InactiveReason = @InactiveReason,
			SecurityGroupID = @SecurityGroupID,
			ClientID = @ClientID
		WHERE UserClientSecurityGroupMapID = @UserClientSecurityGroupMapID;

		SELECT @UserClientSecurityGroupMapID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		if exists (select top 1 * from UserClientSecurityGroupMap u with(nolock) where u.SecurityGroupID = @SecurityGroupID and u.UserID = @UserID and IsActive = 0 and u.ClientID = @ClientID)
		BEGIN
		--Adding this catch for "reactivating" a disabled role
			UPDATE UserClientSecurityGroupMap
			SET 
				UserID = @UserID,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				InactiveReason = @InactiveReason,
				SecurityGroupID = @SecurityGroupID
			WHERE SecurityGroupID = @SecurityGroupID and UserID = @UserID and ClientID = @ClientID
			
			select UserClientSecurityGroupMapID
			FROM UserClientSecurityGroupMap
			where SecurityGroupID = @SecurityGroupID and UserID = @UserID and ClientID = @ClientID
		END
		ELSE
		BEGIN
			INSERT INTO UserClientSecurityGroupMap (UserID,ClientID,SecurityGroupID,IsActive,DateCreated,CreatedByUserID,InactiveReason)
			VALUES(@UserID,@ClientID,@SecurityGroupID,@IsActive,@DateCreated,@CreatedByUserID,@InactiveReason);SELECT @@IDENTITY;
		END
	END