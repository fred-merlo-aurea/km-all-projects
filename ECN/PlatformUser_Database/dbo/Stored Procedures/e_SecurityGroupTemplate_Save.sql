CREATE PROCEDURE [dbo].[e_SecurityGroupTemplate_Save]
@SecurityGroupTemplateID int,
@SecurityGroupName varchar(50),
@ClientGroupID int,
@IsActive bit,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @SecurityGroupTemplateID > 0
	BEGIN
		UPDATE SecurityGroupTemplate
		SET SecurityGroupName = @SecurityGroupName,
			IsActive = @IsActive,
			DateUpdated = GETDATE(),
			UpdatedByUserID = @UpdatedByUserID
		WHERE SecurityGroupTemplateID = @SecurityGroupTemplateID;
		SELECT @SecurityGroupTemplateID;
	END
ELSE
	BEGIN
		INSERT INTO SecurityGroupTemplate (SecurityGroupName,IsActive,DateCreated,CreatedByUserID)
		VALUES(@SecurityGroupName,@IsActive,GETDATE(),@CreatedByUserID);
		SELECT @@IDENTITY;
	END