CREATE PROCEDURE [dbo].[e_Group_Select_FolderID]  
@FolderID int
AS
	SELECT * FROM [Groups] WITH(NOLOCK) WHERE FolderID = @FolderID
