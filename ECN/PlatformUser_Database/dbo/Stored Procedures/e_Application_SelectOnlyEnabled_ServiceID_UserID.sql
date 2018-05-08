CREATE PROCEDURE [dbo].[e_Application_SelectOnlyEnabled_ServiceID_UserID]
@ServiceID int,
@UserID int
AS
	select distinct a.*
	from [Application] a with(nolock)
	join ApplicationServiceMap asm with(nolock) on a.ApplicationID = asm.ApplicationID
	join [Service] s with(nolock) on s.ServiceID = asm.ServiceID
	join [User] u on u.UserID = @UserID
	--join ApplicationSecurityGroupMap asgm on asgm.ApplicationID = a.ApplicationID
	join UserClientSecurityGroupMap ucsg with(nolock) on ucsg.UserID = u.UserID and ucsg.ClientID = u.DefaultClientID
	join ClientServiceMap csm with(nolock) on csm.ClientID = ucsg.ClientID
	where asm.ServiceID = @ServiceID
	and a.IsActive = 'true'
	and asm.IsEnabled = 'true'
	and csm.IsEnabled = 'true'
	and s.IsEnabled = 'true'
	--and asgm.HasAccess = 1
	order by a.ApplicationName