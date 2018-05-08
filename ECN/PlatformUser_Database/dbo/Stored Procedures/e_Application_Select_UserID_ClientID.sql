CREATE PROCEDURE [e_Application_Select_UserID_ClientID]
@UserID int,
@ClientID int
AS
	SELECT distinct a.* 
	FROM Application a With(NoLock)
	JOIN ApplicationSecurityGroupMap am With(NoLock) on a.ApplicationID = am.ApplicationID
	JOIN ClientGroupSecurityGroupMap cgm with(nolock) on am.SecurityGroupID = cgm.SecurityGroupID
	JOIN ClientGroupClientMap cgcm with(nolock) on cgcm.ClientGroupID = cgm.ClientGroupID 
	JOIN ClientGroupUserMap cgu with(nolock) on cgm.ClientGroupID = cgu.ClientGroupID
	JOIN [User] u With(NoLock) on cgu.UserID = u.UserID
	JOIN UserClientSecurityGroupMap ucsgm with(nolock) on ucsgm.SecurityGroupID = cgm.SecurityGroupID and u.UserID = ucsgm.UserID
	WHERE 
		ucsgm.ClientID = @ClientID 
		and cgcm.ClientID = @ClientID
		and u.UserID = @UserID
		AND a.IsActive = 'true'
		AND am.HasAccess = 'true'
		AND cgm.IsActive = 'true'
		AND cgu.IsActive = 'true'
		AND cgcm.IsActive = 'true'
		AND u.IsActive = 'true'
	order by a.ApplicationName