
CREATE PROCEDURE [dbo].[e_ClientGroup_Save]
@ClientGroupID int,
@ClientGroupName varchar(50),
@ClientGroupDescription varchar(250),
@Color varchar(50) = 'transparent',
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @ClientGroupID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE ClientGroup
		SET 
			ClientGroupName = @ClientGroupName,
			ClientGroupDescription = @ClientGroupDescription,
			Color = @Color,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE ClientGroupID = @ClientGroupID;
		
		SELECT @ClientGroupID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO ClientGroup (ClientGroupName,ClientGroupDescription,Color,IsActive,DateCreated,CreatedByUserID)
		VALUES(@ClientGroupName,@ClientGroupDescription,@Color,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END