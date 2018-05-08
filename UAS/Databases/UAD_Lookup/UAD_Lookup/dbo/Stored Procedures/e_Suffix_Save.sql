
CREATE PROCEDURE [dbo].[e_Suffix_Save]
@SuffixID int,
@SuffixCodeTypeID int,
@SuffixName varchar(100),
@IsActive bit,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @SuffixID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE Suffix
			SET SuffixName = @SuffixName,
				SuffixCodeTypeID = @SuffixCodeTypeID,
				IsActive = @IsActive,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE SuffixID = @SuffixID
		
			SELECT @SuffixID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO Suffix (SuffixCodeTypeID,SuffixName,IsActive,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID)
			VALUES(@SuffixCodeTypeID,@SuffixName,@IsActive,@DateCreated,@DateUpdated,@CreatedByUserID,@UpdatedByUserID);
			SELECT @@IDENTITY;
		END

END