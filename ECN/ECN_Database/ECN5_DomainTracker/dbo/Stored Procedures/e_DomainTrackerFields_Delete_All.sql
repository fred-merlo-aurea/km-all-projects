CREATE  PROCEDURE [dbo].[e_DomainTrackerFields_Delete_All]
@DomainTrackerID int,
@UserID int
AS

update DomainTrackerFields set IsDeleted = 1, UpdatedDate=GETDATE(), UpdatedUserID=@UserID
from DomainTrackerFields
inner join DomainTracker  on
DomainTrackerFields.DomainTrackerID = DomainTracker.DomainTrackerID
where DomainTracker.DomainTrackerID=@DomainTrackerID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerFields_Delete_All] TO [ecn5]
    AS [dbo];

