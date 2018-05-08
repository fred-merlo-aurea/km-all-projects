CREATE PROCEDURE [dbo].[e_Client_Save]
@ClientID int,
@ClientName varchar(100),
@DisplayName varchar(100),
@ClientCode varchar(15),
@ClientTestDBConnectionString varchar(255),
@ClientLiveDBConnectionString varchar(255),
@IsActive bit,
@IgnoreUnknownFiles bit,
@AccountManagerEmails varchar(500),
@ClientEmails varchar(1000),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@HasPaid bit,
@IsKMClient bit,
@ParentClientId int,
@HasChildren bit
AS

IF @ClientID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Client
		SET ClientName = @ClientName,
			DisplayName = @DisplayName,
			ClientCode = @ClientCode,
			ClientTestDBConnectionString = @ClientTestDBConnectionString,
			ClientLiveDBConnectionString = @ClientLiveDBConnectionString,
			IsActive = @IsActive,
			IgnoreUnknownFiles = @IgnoreUnknownFiles,
			AccountManagerEmails = @AccountManagerEmails,
			ClientEmails = @ClientEmails,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			HasPaid = @HasPaid,
			IsKMClient = @IsKMClient,
			ParentClientId = @ParentClientId,
			HasChildren = @HasChildren
		WHERE ClientID = @ClientID;
		
		SELECT @ClientID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Client (ClientName,DisplayName,ClientCode,ClientTestDBConnectionString,ClientLiveDBConnectionString,IsActive,IgnoreUnknownFiles,AccountManagerEmails,ClientEmails,
							DateCreated,CreatedByUserID,HasPaid,IsKMClient,ParentClientId,HasChildren)
		VALUES(@ClientName,@DisplayName,@ClientCode,@ClientTestDBConnectionString,@ClientLiveDBConnectionString,@IsActive,@IgnoreUnknownFiles,@AccountManagerEmails,@ClientEmails,
				@DateCreated,@CreatedByUserID,@HasPaid,@IsKMClient,@ParentClientId,@HasChildren);SELECT @@IDENTITY;
	END
