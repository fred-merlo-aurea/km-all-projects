CREATE  PROC [dbo].[e_ContentFilter_Save] 
(
	@FilterID int = NULL,
	@LayoutID int = NULL,
	@SlotNumber int = NULL,
	@ContentID int = NULL,
	@GroupID int = NULL,
	@FilterName varchar(50) = NULL,
	@WhereClause varchar(8000) = NULL,
	@UserID int = NULL
)
AS 
BEGIN
	IF @FilterID is NULL or @FilterID <= 0
	BEGIN
		INSERT INTO ContentFilter
		(
			LayoutID, SlotNumber, ContentID, GroupID, FilterName, WhereClause, CreatedDate, CreatedUserID, IsDeleted
		)
		VALUES
		(
			@LayoutID, @SlotNumber, @ContentID, @GroupID, @FilterName, @WhereClause, GETDATE(), @UserID, 0
		)
		SET @FilterID = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE ContentFilter
			SET LayoutID=@LayoutID, SlotNumber=@SlotNumber,ContentID=ContentID, @GroupID=@GroupID, 
				FilterName=@FilterName, WhereClause=@WhereClause, UpdatedDate=GETDATE(), UpdatedUserID=@UserID
		WHERE
			FilterID = @FilterID
	END

	SELECT @FilterID
END