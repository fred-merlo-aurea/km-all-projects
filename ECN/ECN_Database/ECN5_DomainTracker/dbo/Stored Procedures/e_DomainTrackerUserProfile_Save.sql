CREATE PROCEDURE [dbo].[e_DomainTrackerUserProfile_Save]
@EmailAddress varchar(400),
@BaseChannelID int,
@UserID int,
@IsKnown bit
AS
	insert into DomainTrackerUserProfile (EmailAddress, BaseChannelID, CreatedUserID, CreatedDate, IsDeleted, IsKnown)
	values(@EmailAddress, @BaseChannelID, @UserID, GETDATE(), 0, @IsKnown)
	select @@IDENTITY

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerUserProfile_Save] TO [ecn5]
    AS [dbo];

