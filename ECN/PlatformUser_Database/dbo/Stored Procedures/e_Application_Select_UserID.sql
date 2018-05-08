CREATE PROCEDURE [dbo].[e_Application_Select_UserID]
@UserID int
AS
	SELECT distinct a.* 
	FROM Application a With(NoLock)
	JOIN ApplicationSecurityGroupMap am With(NoLock) on a.ApplicationID = am.ApplicationID
	JOIN ClientGroupSecurityGroupMap cgm with(nolock) on am.SecurityGroupID = cgm.SecurityGroupID
	JOIN ClientGroupUserMap cgu with(nolock) on cgm.ClientGroupID = cgu.ClientGroupID
	JOIN [User] u With(NoLock) on cgu.UserID = u.UserID
	JOIN UserClientSecurityGroupMap ucsgm with(nolock) on ucsgm.SecurityGroupID = cgm.SecurityGroupID and u.UserID = ucsgm.UserID
	WHERE u.UserID = @UserID
	AND a.IsActive = 'true'
	AND am.HasAccess = 'true'
	AND cgm.IsActive = 'true'
	AND cgu.IsActive = 'true'
	AND u.IsActive = 'true'
	order by a.ApplicationName
GO


