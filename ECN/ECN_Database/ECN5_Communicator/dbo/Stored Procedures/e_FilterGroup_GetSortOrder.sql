CREATE  PROC [dbo].[e_FilterGroup_GetSortOrder] 
(
	@FilterID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT * FROM FilterGroup WITH (NOLOCK) WHERE FilterID = @FilterID)
    BEGIN
		SELECT MAX(ISNULL(SortOrder, 0)) + 1 FROM FilterGroup WITH (NOLOCK) WHERE FilterID = @FilterID
	END
	ELSE
	BEGIN
		SELECT 1
	END
END