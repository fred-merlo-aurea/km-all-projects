CREATE PROCEDURE [dbo].[e_SecurityGroup_Save]
@SecurityGroupID int,
@SecurityGroupName varchar(50),
@ClientGroupID int,
@ClientID int,
@IsActive bit,
@AdministrativeLevel varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @SecurityGroupID > 0
	BEGIN
		SET @DateUpdated = GETDATE();
		UPDATE SecurityGroup
		SET SecurityGroupName = @SecurityGroupName,
			ClientID = @ClientID,
			ClientGroupID = @ClientGroupID,
			IsActive = @IsActive,
			AdministrativeLevel = @AdministrativeLevel,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE SecurityGroupID = @SecurityGroupID;
		SELECT @SecurityGroupID;			
	END
ELSE
	BEGIN
		SET @DateCreated = GETDATE();
		INSERT INTO SecurityGroup (SecurityGroupName,ClientGroupID,ClientID,IsActive,AdministrativeLevel,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupName,@ClientGroupID,@ClientID,@IsActive,@AdministrativeLevel,@DateCreated,@CreatedByUserID);
		SELECT @@IDENTITY;
	END
