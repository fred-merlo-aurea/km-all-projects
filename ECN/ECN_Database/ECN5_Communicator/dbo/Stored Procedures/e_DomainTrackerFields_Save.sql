CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Save]
@DomainTrackerID int,
@GroupDatafieldsID int,
@Source varchar(100),
@SourceID varchar(100),
@UserID int
AS
insert into DomainTrackerFields(DomainTrackerID, GroupDatafieldsID, Source, SourceID, CreatedUserID, CreatedDate, IsDeleted)
values(@DomainTrackerID, @GroupDatafieldsID, @Source, @SourceID, @UserID, GETDATE(), 0);
	Select  @@IDENTITY;