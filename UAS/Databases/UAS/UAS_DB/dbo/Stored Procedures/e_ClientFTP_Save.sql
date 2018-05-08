CREATE PROCEDURE e_ClientFTP_Save
@FTPID int,
@ClientID int,
@Server varchar(100),
@UserName varchar(100),
@Password varchar(100),
@Folder varchar(100),
@IsDeleted bit,
@IsExternal bit,
@IsActive bit,
@FTPConnectionValidated bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on
	IF @FTPID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE ClientFTP
			SET ClientID = @ClientID,
				Server = @Server,
				UserName = @UserName,
				Password = @Password,
				Folder = @Folder,
				IsDeleted = @IsDeleted,
				IsExternal = @IsExternal,
				IsActive = @IsActive,
				FTPConnectionValidated = @FTPConnectionValidated,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FTPID = @FTPID;
		
			SELECT @FTPID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO ClientFTP (ClientID,Server,UserName,Password,Folder,IsDeleted,IsExternal,IsActive,FTPConnectionValidated,DateCreated,CreatedByUserID)
			VALUES(@ClientID,@Server,@UserName,@Password,@Folder,@IsDeleted,@IsExternal,@IsActive,@FTPConnectionValidated,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END