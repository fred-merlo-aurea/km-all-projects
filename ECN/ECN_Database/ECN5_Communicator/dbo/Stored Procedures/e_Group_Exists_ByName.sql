CREATE PROCEDURE [dbo].[e_Group_Exists_ByName] 
	@GroupID int = NULL,
	@CustomerID int = NULL,
	@FolderID int = NULL,
	@GroupName varchar(50) = NULL
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 GroupID FROM [Groups] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND GroupID != ISNULL(@GroupID, -1) AND FolderID = @FolderID AND GroupName = @GroupName) SELECT 1 ELSE SELECT 0
END