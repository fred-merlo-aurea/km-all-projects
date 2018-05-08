CREATE PROCEDURE [dbo].[e_Client_Select_DefaultClient_AccessKey]
@AccessKey uniqueidentifier
AS
	SELECT Distinct c.*
	FROM Client c With(NoLock)
	JOIN ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	JOIN ClientGroupUserMap cgum with(nolock) on cgcm.ClientGroupID = cgum.ClientGroupID
	JOIN [User] u With(NoLock) ON u.UserID = cgum.UserID
	WHERE u.AccessKey = @AccessKey 
	AND u.IsAccessKeyValid = 'true' 
	AND u.IsActive = 'true'
	and cgcm.IsActive = 'true' 
	and cgum.IsActive = 'true' 
	and c.IsActive='true'
	and c.ClientID = u.DefaultClientID
