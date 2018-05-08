CREATE PROCEDURE e_FilterGroup_Save
@FilterGroupId int,
@FilterId int,
@SortOrder int,
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN

	set nocount on

	IF @FilterGroupId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE FilterGroup
			SET FilterId = @FilterId,
				SortOrder = @SortOrder,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			WHERE FilterGroupId = @FilterGroupId;
		
			SELECT @FilterGroupId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO FilterGroup (FilterId,SortOrder,DateCreated,CreatedByUserID)
			VALUES(@FilterId,@SortOrder,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
		END
END
GO