CREATE PROCEDURE [dbo].[e_Filter_Archive]
	@FilterID int,
	@UpdatedUserID int,
	@Archived bit
AS
	UPDATE Filter
	SET Archived = @Archived, UpdatedUserID = @UpdatedUserID, UpdatedDate = GETDATE()
	WHERE FilterID = @FilterID
