
CREATE PROCEDURE [dbo].[e_User_Save]
@UserID int,
@DefaultClientGroupID int,
@DefaultClientID int,
@FirstName varchar(50),
@LastName varchar(50),
@UserName varchar(50),
@Password varchar(250),
@Salt varchar(250),
@EmailAddress varchar(250),
@IsActive bit,
@AccessKey uniqueidentifier,
@IsAccessKeyValid bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@Phone varchar(20) = null,
@Status varchar(20) = 'Disabled',
@IsKMStaff bit = 0,
@IsPlatformAdministrator bit = 0,
@IsReadOnly bit = 0
AS

IF @UserID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE [User]
		SET 
			DefaultClientGroupID = @DefaultClientGroupID,
			DefaultClientID = @DefaultClientID,
			FirstName = @FirstName,
			LastName = @LastName,
			UserName = @UserName,
			Password = @Password,
			Salt = @Salt,
			EmailAddress = @EmailAddress,
			IsActive = @IsActive,
			AccessKey = @AccessKey,
			IsAccessKeyValid = @IsAccessKeyValid,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			Phone = @Phone,
			Status = @Status,
			IsKMStaff = @IsKMStaff,
			IsPlatformAdministrator = @IsPlatformAdministrator,
			IsReadOnly = @IsReadOnly
		WHERE UserID = @UserID;
		
		SELECT @UserID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO [User] (DefaultClientGroupID,DefaultClientID,FirstName,LastName,UserName,Password,Salt,EmailAddress,IsActive,AccessKey,IsAccessKeyValid,DateCreated,CreatedByUserID,Phone,Status,IsKMStaff,IsPlatformAdministrator,IsReadOnly)
		VALUES(@DefaultClientGroupID,@DefaultClientID,@FirstName,@LastName,@UserName,@Password,@Salt,@EmailAddress,@IsActive,@AccessKey,@IsAccessKeyValid,@DateCreated,@CreatedByUserID, @Phone,@Status, @IsKMStaff, @IsPlatformAdministrator,@IsReadOnly);SELECT @@IDENTITY;
	END