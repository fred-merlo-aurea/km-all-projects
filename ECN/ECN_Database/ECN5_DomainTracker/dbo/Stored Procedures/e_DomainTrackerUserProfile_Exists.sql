CREATE PROCEDURE [dbo].[e_DomainTrackerUserProfile_Exists]
@BaseChannelID int,
@EmailAddress varchar(400)
AS
	if exists (select top 1 dt.ProfileID FROM DomainTrackerUserProfile dt WITH (NOLOCK) 
	WHERE dt.BaseChannelID = @BaseChannelID and dt.EmailAddress = @EmailAddress and dt.IsDeleted = 0)
    select 1 else select 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerUserProfile_Exists] TO [ecn5]
    AS [dbo];

