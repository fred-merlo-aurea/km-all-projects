CREATE PROCEDURE [dbo].[e_GetGroups_User_Customer] 
	@CustomerID int, 
	@userID int
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
		Groups g WITH(NOLOCK) JOIN usergroups ug WITH(NOLOCK) ON ug.GroupID = g.GroupID 
	WHERE 
		UserID = @userID AND CustomerID = @CustomerID
END
