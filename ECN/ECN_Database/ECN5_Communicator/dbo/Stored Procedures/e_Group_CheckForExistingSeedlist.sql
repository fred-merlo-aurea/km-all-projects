Create PROCEDURE [dbo].[e_Group_CheckForExistingSeedlist] 
	@GroupID int = NULL,
	@CustomerID int
AS     
BEGIN     		
	SELECT 
		case when COUNT(GroupID) > 0 then 1 else 0 end
	FROM 
		Groups WITH (NOLOCK) 
	WHERE 
		CustomerID = @CustomerID AND 
		GroupID != ISNULL(@GroupID, -1) AND 
		IsNull(IsSeedList, 0) = 1
END