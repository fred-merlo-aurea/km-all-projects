﻿CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Delete_Single]
@DomainTrackerFieldsID int,
@UserID int
AS

update DomainTrackerFields set IsDeleted=1, UpdatedUserID=@UserID, UpdatedDate=GETDATE()
where DomainTrackerFieldsID=@DomainTrackerFieldsID

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerFields_Delete_Single] TO [ecn5]
    AS [dbo];

