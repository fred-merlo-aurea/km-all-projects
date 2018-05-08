CREATE PROCEDURE e_Filter_Save
@FilterId int,
@FilterName varchar(50),
@ProductId int,
@IsActive bit,
@BrandId int,
@ClientID int,
@FilterGroupID int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE Filter
			SET FilterName = @FilterName,
				ProductId = @ProductId,
				IsActive = @IsActive,
				BrandId = @BrandId,
				ClientID = @ClientID,
				FilterGroupID = @FilterGroupID,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterId = @FilterId;
		
			SELECT @FilterId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Filter (FilterName,ProductId,IsActive,BrandId,ClientID,FilterGroupID,DateCreated,CreatedByUserID)
			VALUES(@FilterName,@ProductId,@IsActive,@BrandId,@ClientID,@FilterGroupID,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO