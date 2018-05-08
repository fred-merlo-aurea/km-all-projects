CREATE PROCEDURE [dbo].[e_Layout_Exists_ByName] 
	@LayoutID int = NULL,
	@CustomerID int = NULL,
	@FolderID int = NULL,
	@LayoutName varchar(200) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 LayoutID FROM Layout WITH (NOLOCK) WHERE CustomerID = @CustomerID AND LayoutID != ISNULL(@LayoutID, -1) AND FolderID = @FolderID AND LayoutName = @LayoutName AND IsDeleted = 0) SELECT 1 ELSE SELECT 0
END