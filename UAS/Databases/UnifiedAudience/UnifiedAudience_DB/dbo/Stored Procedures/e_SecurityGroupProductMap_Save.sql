CREATE PROCEDURE [dbo].[e_SecurityGroupProductMap_Save]
@SecurityGroupProductMapID int,
@SecurityGroupID int,
@ProductID int,
@HasAccess bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	SET NOCOUNT ON

	IF @SecurityGroupProductMapID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE SecurityGroupProductMap
			SET HasAccess = @HasAccess,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE SecurityGroupProductMapID = @SecurityGroupProductMapID;
		
			SELECT @SecurityGroupProductMapID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO SecurityGroupProductMap (SecurityGroupID,ProductID,HasAccess,DateCreated,CreatedByUserID)
			VALUES(@SecurityGroupID,@ProductID,@HasAccess,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END

END
GO