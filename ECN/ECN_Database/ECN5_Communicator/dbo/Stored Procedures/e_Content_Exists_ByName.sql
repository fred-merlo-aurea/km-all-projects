CREATE PROCEDURE [dbo].[e_Content_Exists_ByName] 
	@ContentID int = NULL,
	@CustomerID int = NULL,
	@FolderID int = NULL,
	@ContentTitle varchar(255) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 ContentID FROM Content WITH (NOLOCK) WHERE CustomerID = @CustomerID AND ContentID != ISNULL(@ContentID, -1) AND FolderID = @FolderID AND ContentTitle = @ContentTitle AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END