CREATE  PROC [dbo].[e_FilterCondition_GetSortOrder] 
(
	@FilterGroupID int = NULL
)
AS 
BEGIN
	IF EXISTS (SELECT * FROM FilterCondition WITH (NOLOCK) WHERE FilterGroupID = @FilterGroupID)
    BEGIN
		SELECT MAX(ISNULL(SortOrder, 0)) + 1 FROM FilterCondition WITH (NOLOCK) WHERE FilterGroupID = @FilterGroupID
	END
	ELSE
	BEGIN
		SELECT 1
	END
END