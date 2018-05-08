CREATE PROCEDURE [dbo].[e_Group_Select_UserID_CustomerID] 
	@CustomerID int, 
	@UserID int
AS     
BEGIN     		
	SELECT 
	g.GroupID,
	CustomerID,
	FolderID,
	GroupName,
	GroupDescription,
	OwnerTypeCode,
	MasterSupression,
	PublicFolder,
	OptinHTML,
	OptinFields,
	AllowUDFHistory,
	IsSeedList
	FROM 
		[Groups] g WITH(NOLOCK) JOIN usergroups ug WITH(NOLOCK) ON ug.GroupID = g.GroupID 
	WHERE 
		UserID = @UserID AND CustomerID = @CustomerID and ug.IsDeleted = 0
END
