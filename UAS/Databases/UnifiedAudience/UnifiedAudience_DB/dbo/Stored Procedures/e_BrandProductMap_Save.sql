CREATE PROCEDURE [dbo].[e_BrandProductMap_Save]
@BrandProductMapID int,
@ProductID int,
@BrandID int,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @BrandProductMapID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE BrandProductMap
			SET 
				HasAccess = @HasAccess,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE BrandProductMapID = @BrandProductMapID;

			SELECT @BrandProductMapID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO BrandProductMap (BrandID,ProductID,HasAccess,DateCreated,CreatedByUserID)
			VALUES(@BrandID,@ProductID,@HasAccess,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END