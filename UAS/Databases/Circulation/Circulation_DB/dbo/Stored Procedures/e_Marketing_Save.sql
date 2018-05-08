CREATE PROCEDURE e_Marketing_Save
@MarketingID int,
@MarketingName nvarchar(50),
@MarketingCode nchar(10),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @MarketingID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Marketing
		SET 
			MarketingName = @MarketingName,
			IsActive = @IsActive,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE MarketingID = @MarketingID;
		
		SELECT @MarketingID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Marketing (MarketingName,MarketingCode,IsActive,DateCreated,CreatedByUserID)
		VALUES(@MarketingName,@MarketingCode,@IsActive,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
