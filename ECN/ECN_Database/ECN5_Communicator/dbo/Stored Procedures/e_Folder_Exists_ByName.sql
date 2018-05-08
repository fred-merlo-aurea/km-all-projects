CREATE PROCEDURE [dbo].[e_Folder_Exists_ByName] 
	@FolderID int = NULL,
	@CustomerID int = NULL,
	@ParentID int = NULL,
	@FolderName varchar(50) = NULL,
	@FolderType varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 FolderID FROM Folder WITH (NOLOCK) WHERE CustomerID = @CustomerID AND FolderID != ISNULL(@FolderID, -1) AND ParentID = @ParentID AND FolderName = @FolderName AND IsDeleted = 0 AND FolderType = @FolderType) SELECT 1 ELSE SELECT 0
END