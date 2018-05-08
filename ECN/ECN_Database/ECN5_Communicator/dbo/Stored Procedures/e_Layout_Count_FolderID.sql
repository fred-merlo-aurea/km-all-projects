CREATE  PROC [dbo].[e_Layout_Count_FolderID] 

	@FolderID int = NULL

AS 
BEGIN
	if exists ( select Top 1 LayoutID from Layout WITH (NOLOCK) where FolderID = @FolderID and IsDeleted = 0) select 1 else select 0
END