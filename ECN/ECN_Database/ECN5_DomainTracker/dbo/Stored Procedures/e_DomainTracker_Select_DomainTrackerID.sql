CREATE PROCEDURE [dbo].[e_DomainTracker_Select_DomainTrackerID]
@DomainTrackerID int
AS
	SELECT * FROM DomainTracker cd WITH (NOLOCK) 
	WHERE cd.DomainTrackerID=@DomainTrackerID and cd.IsDeleted=0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTracker_Select_DomainTrackerID] TO [ecn5]
    AS [dbo];

