CREATE PROCEDURE [dbo].[e_DomainTrackerUserProfile_GetByEmailAddress]

@EmailAddress varchar(400),
@BaseChannelID int

AS
	SET NOCOUNT ON
	
	SELECT 
		* 
	FROM 
		DomainTrackerUserProfile WITH(NOLOCK) 
	WHERE 
		EmailAddress=@EmailAddress 
		AND BaseChannelID=@BaseChannelID
GO

GRANT EXECUTE
    ON OBJECT::[dbo].[e_DomainTrackerUserProfile_GetByEmailAddress] TO [ecn5]
    AS [dbo];

