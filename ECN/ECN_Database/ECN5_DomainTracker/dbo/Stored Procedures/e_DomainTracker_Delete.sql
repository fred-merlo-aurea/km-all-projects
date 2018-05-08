﻿CREATE PROCEDURE [dbo].[e_DomainTracker_Delete]
@DomainTrackerID int,
@UserID int
AS
	update DomainTracker set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE()
	where DomainTrackerID=@DomainTrackerID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTracker_Delete] TO [ecn5]
    AS [dbo];
