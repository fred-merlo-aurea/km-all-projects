CREATE PROCEDURE [dbo].[e_DomainTracker_Save]
@Domain varchar(200),
@GroupID int,
@CustomerID int,
@TrackerKey varchar(200),
@UserID int
AS
	insert into DomainTracker(Domain, GroupID, CustomerID, TrackerKey, CreatedUserID, CreatedDate, IsDeleted)
	values(@Domain, @GroupID,@CustomerID, @TrackerKey, @UserID, GETDATE(), 0);
	Select  @@IDENTITY