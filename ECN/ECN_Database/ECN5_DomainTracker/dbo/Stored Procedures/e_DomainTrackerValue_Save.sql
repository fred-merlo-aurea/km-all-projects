CREATE PROCEDURE [dbo].[e_DomainTrackerValue_Save]
@DomainTrackerActivityID int,
@DomainTrackerFieldsID int,
@Value varchar(8000),
@UserID int
AS
	insert into DomainTrackerValue (DomainTrackerActivityID, DomainTrackerFieldsID, Value,CreatedUserID, CreatedDate, IsDeleted)
	values(@DomainTrackerActivityID, @DomainTrackerFieldsID, @Value, @UserID, GETDATE(), 0)
	select @@IDENTITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerValue_Save] TO [ecn5]
    AS [dbo];

