CREATE  PROC [dbo].[e_Filter_UpdateWhere_FilterID] 
(
	@UserID int = NULL,
    @FilterID int = NULL,
    @WhereClause text = NULL
)
AS 
BEGIN
	Update Filter SET WhereClause = @WhereClause, UpdatedDate = GETDATE(), UpdatedUserID = @UserID WHERE FilterID = @FilterID
END
