CREATE PROCEDURE [dbo].[e_TransformSplit_Save]
@TransformSplitID int,
@TransformationID int,
@Delimiter varchar(50),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @TransformSplitID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE TransformSplit
			SET TransformationID = @TransformationID,
				  Delimiter = @Delimiter,
				  IsActive = @IsActive,
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID       
			WHERE TransformSplitID = @TransformSplitID;

			SELECT @TransformSplitID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO TransformSplit (TransformationID, Delimiter, IsActive, DateCreated, CreatedByUserID)
			VALUES(@TransformationID, @Delimiter, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
		END

END