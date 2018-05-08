CREATE PROCEDURE [dbo].[e_Application_SelectOnlyEnabled_ServiceID_UserID]
@ServiceID int,
@UserID int
AS
	select distinct a.*
	from [Application] a with(nolock)
	join ApplicationServiceMap asm with(nolock) on a.ApplicationID = asm.ApplicationID
	join [User] u on u.UserID = @UserID
	join ApplicationSecurityGroupMap asgm on asgm.ApplicationID = a.ApplicationID
	join UserClientSecurityGroupMap ucsg on ucsg.UserID = u.UserID and ucsg.SecurityGroupID = asgm.SecurityGroupID and ucsg.ClientID = u.DefaultClientID
	where asm.ServiceID = @ServiceID
	and a.IsActive = 'true'
	and asm.IsEnabled = 'true'
	and asgm.HasAccess = 1
	order by a.ApplicationName
