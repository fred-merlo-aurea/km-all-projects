CREATE PROCEDURE [dbo].[e_DomainTracker_Select_DomainTrackerID]
@DomainTrackerID int
AS
	SELECT * FROM DomainTracker cd WITH (NOLOCK) 
	WHERE cd.DomainTrackerID=@DomainTrackerID and cd.IsDeleted=0