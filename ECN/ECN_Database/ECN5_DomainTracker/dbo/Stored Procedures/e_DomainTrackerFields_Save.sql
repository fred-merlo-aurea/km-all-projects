CREATE PROCEDURE [dbo].[e_DomainTrackerFields_Save]
@DomainTrackerID int,
@FieldName varchar(100),
@Source varchar(100),
@SourceID varchar(100),
@UserID int
AS
insert into DomainTrackerFields(DomainTrackerID, FieldName, Source, SourceID, CreatedUserID, CreatedDate, IsDeleted)
values(@DomainTrackerID, @FieldName, @Source, @SourceID, @UserID, GETDATE(), 0);
	Select  @@IDENTITY;

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerFields_Save] TO [ecn5]
    AS [dbo];

