CREATE PROCEDURE [dbo].[e_Client_Select_Accesskey]
@Accesskey uniqueidentifier
AS
	SELECT c.*
	FROM Client c With(NoLock)
	JOIN ClientGroupClientMap cgcm with(nolock) on c.ClientID = cgcm.ClientID
	JOIN ClientGroupUserMap cgum with(nolock) on cgcm.ClientGroupID = cgum.ClientGroupID
	JOIN [User] u With(NoLock) ON u.UserID = cgum.UserID
	WHERE u.Accesskey = @Accesskey 
	AND u.IsAccesskeyValid = 'true' 
	AND u.IsActive = 'true'
	and cgcm.IsActive = 'true' 
	and cgum.IsActive = 'true' 
	and c.IsActive='true'
GO