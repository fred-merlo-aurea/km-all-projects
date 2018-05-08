CREATE PROCEDURE [dbo].[e_SplitTransform_Save]
@SplitTransformID int,
@TransformationID int,
@SplitBeforeID int,
@DataMapID int,
@SplitAfterID int,
@Column varchar(200),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @SplitTransformID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE TransformSplitTrans
			SET TransformationID = @TransformationID,
				  SplitBeforeID = @SplitBeforeID,
				  DataMapID = @DataMapID,
				  SplitAfterID = @SplitAfterID,
				  [Column] = @Column,
				  IsActive = @IsActive,
				  DateUpdated = @DateUpdated,
				  UpdatedByUserID = @UpdatedByUserID
			WHERE SplitTransformID = @SplitTransformID;

			SELECT @SplitTransformID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO TransformSplitTrans (TransformationID, SplitBeforeID, DataMapID, SplitAfterID, [Column], IsActive, DateCreated, CreatedByUserID)
			VALUES(@TransformationID, @SplitBeforeID, @DataMapID, @SplitAfterID, @Column, @IsActive, @DateCreated, @CreatedByUserID);SELECT @@IDENTITY;
		END
END
GO