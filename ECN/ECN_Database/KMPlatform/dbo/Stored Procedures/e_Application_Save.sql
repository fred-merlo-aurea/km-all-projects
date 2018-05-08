CREATE PROCEDURE [dbo].[e_Application_Save]
@ApplicationID int,
@ApplicationName varchar(50),
@Description varchar(500),
@ApplicationCode varchar(10),
@DefaultView varchar(250),
@IsActive bit,
@IconFullName varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@FromEmailAddress varchar(250),
@ErrorEmailAddress varchar(250)
AS

IF @ApplicationID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Application
		SET ApplicationName = @ApplicationName,
			Description = @Description,
			ApplicationCode = @ApplicationCode,
			DefaultView = @DefaultView,
			IsActive = @IsActive,
			IconFullName = @IconFullName,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			FromEmailAddress = @FromEmailAddress,
			ErrorEmailAddress = @ErrorEmailAddress
		WHERE ApplicationID = @ApplicationID;
		
		SELECT @ApplicationID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Application (ApplicationName,Description,ApplicationCode,DefaultView,IsActive,IconFullName,DateCreated,CreatedByUserID,FromEmailAddress,ErrorEmailAddress)
		VALUES(@ApplicationName,@Description,@ApplicationCode,@DefaultView,@IsActive,@IconFullName,@DateCreated,@CreatedByUserID,@FromEmailAddress,@ErrorEmailAddress);SELECT @@IDENTITY;
	END