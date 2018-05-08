CREATE PROCEDURE [dbo].[e_SecurityGroupBrandMap_Save]
@SecurityGroupBrandMapID int,
@SecurityGroupID int,
@BrandID int,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF @SecurityGroupBrandMapID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SecurityGroupBrandMap
			SET HasAccess = @HasAccess,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE SecurityGroupBrandMapID = @SecurityGroupBrandMapID;
		
			SELECT @SecurityGroupBrandMapID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SecurityGroupBrandMap (SecurityGroupID,BrandID,HasAccess,DateCreated,CreatedByUserID)
			VALUES(@SecurityGroupID,@BrandID,@HasAccess,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO