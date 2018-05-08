CREATE PROCEDURE [dbo].[e_Content_Select_FolderID]   
@FolderID int
AS
	SELECT * FROM Content WITH (NOLOCK) WHERE FolderID = @FolderID and IsDeleted = 0