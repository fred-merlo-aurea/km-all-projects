CREATE  PROC dbo.e_Group_Count_DomainTracking
(
	@GroupID int = NULL
)
AS 
BEGIN
	if exists ( select Top 1 GroupID from DomainTracker WITH (NOLOCK) where GroupID = @GroupID and IsDeleted=0) select 1 else select 0
END