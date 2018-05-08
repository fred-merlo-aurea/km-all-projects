CREATE PROCEDURE [dbo].[e_Client_Select_DefaultClient_AccessKey]
@AccessKey uniqueidentifier
AS
	SELECT Distinct c.*
	FROM Client c With(NoLock)
	JOIN ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	--JOIN UserClientSecurityGroupMap ucsgm with(nolock) on ucsgm.ClientID = cgcm.ClientID
	--JOIN [User] u With(NoLock) ON u.UserID = ucsgm.UserID
	JOIN [User] u With(NoLock) ON u.DefaultClientID = c.ClientID
	WHERE u.AccessKey = @AccessKey
	AND u.IsAccessKeyValid = 'true' 
	AND u.IsActive = 'true'
	and cgcm.IsActive = 'true' 
	--and ucsgm.IsActive = 'true' 
	and c.IsActive='true'
	and c.ClientID = u.DefaultClientID