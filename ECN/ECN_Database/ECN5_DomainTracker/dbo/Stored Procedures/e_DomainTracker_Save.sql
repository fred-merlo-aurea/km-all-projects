CREATE PROCEDURE [dbo].[e_DomainTracker_Save]
@Domain varchar(200),
@BaseChannelID int,
@TrackerKey varchar(200),
@UserID int
AS
	insert into DomainTracker(Domain,  BaseChannelID, TrackerKey, CreatedUserID, CreatedDate, IsDeleted)
	values(@Domain, @BaseChannelID, @TrackerKey, @UserID, GETDATE(), 0);
	Select  @@IDENTITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTracker_Save] TO [ecn5]
    AS [dbo];

