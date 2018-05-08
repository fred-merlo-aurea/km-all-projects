CREATE PROCEDURE [e_Publisher_Save]
@PublisherID int,
@PublisherName varchar(50),
@PublisherCode varchar(50),
@IsActive bit,
@HasPaid bit,
@ClientID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @PublisherID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Publisher
		SET 
			PublisherName = @PublisherName,
			PublisherCode = @PublisherCode,
			IsActive = @IsActive,
			HasPaid = @HasPaid,
			ClientID = @ClientID,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PublisherID = @PublisherID;
		
		SELECT @PublisherID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Publisher (PublisherName,PublisherCode,IsActive,HasPaid,ClientID,DateCreated,CreatedByUserID)
		VALUES(@PublisherName,@PublisherCode,@IsActive,@HasPaid,@ClientID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
