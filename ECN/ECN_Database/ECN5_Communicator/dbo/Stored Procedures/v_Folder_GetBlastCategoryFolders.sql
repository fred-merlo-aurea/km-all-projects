CREATE PROCEDURE [dbo].[v_Folder_GetBlastCategoryFolders]  
(
	@CustomerID int
) AS 
BEGIN
	SELECT 
		FolderID, FolderName 
	FROM 
		[Folder]f WITH (NOLOCK)
		JOIN [Code] c WITH (NOLOCK) ON f.FolderName = c.CodeDisplay 
	WHERE
		f.CustomerID = @CustomerID AND 
		c.CustomerID = @CustomerID AND 
		c.CodeType = 'BlastCategory'
END