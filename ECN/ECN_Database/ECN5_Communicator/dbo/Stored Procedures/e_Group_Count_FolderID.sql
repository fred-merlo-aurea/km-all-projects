CREATE  PROC [dbo].[e_Group_Count_FolderID] 
(
	@FolderID int = NULL
)
AS 
BEGIN
	if exists ( select Top 1 GroupID from [Groups] WITH (NOLOCK) where FolderID = @FolderID) select 1 else select 0
END