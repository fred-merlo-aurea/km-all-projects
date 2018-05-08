CREATE PROCEDURE [dbo].[e_TransformJoin_Save]
@TransformJoinID int,
@TransformationID int,
@ColumnsToJoin varchar(500),
@Delimiter varchar(50),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @TransformJoinID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE TransformJoin
			SET TransformationID = @TransformationID,
				  ColumnsToJoin = @ColumnsToJoin,
				  Delimiter = @Delimiter,
				  IsActive = @IsActive,
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID
			WHERE TransformJoinID = @TransformJoinID;

			SELECT @TransformJoinID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO TransformJoin (TransformationID, ColumnsToJoin, Delimiter, IsActive, DateCreated, CreatedByUserID)
			VALUES(@TransformationID, @ColumnsToJoin, @Delimiter, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
		END

END