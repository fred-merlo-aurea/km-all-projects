CREATE PROCEDURE [dbo].[e_Service_AMS_Select_SecurityGroup_UserID]
@SecurityGroupID int,
@UserID int
as
	declare @clientgroupID int,
			@ClientID int
			
	select @ClientID = ISNULL(clientID, -1)
	from 
		SecurityGroup with (NOLOCK)
	where
		SecurityGroupID = @securityGroupID		
if @ClientID > -1
	select	distinct s.*
	 from
		securityGroup sg with (NOLOCK) join
		ClientServiceMap csm with (NOLOCK) on csm.ClientID = sg.ClientID join
		[Service] s with (NOLOCK) on s.serviceID = csm.ServiceID join
		UserClientSecurityGroupMap ucsg on ucsg.ClientID = csm.ClientID and ucsg.SecurityGroupID = sg.SecurityGroupID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		csm.IsEnabled = 1 and
		sg.IsActive = 1 and
		ucsg.UserID = @UserID
		
--else if @clientgroupID > -1
--	select s.*
--	from
--		securityGroup sg with (NOLOCK) join
--		ClientGroupServiceMap cgsm with (NOLOCK) on cgsm.ClientGroupID = sg.ClientID join
--		ClientGroup cg with(NOLOCK) on cg.ClientGroupID = cgsm.ClientGroupID join
--		ClientGroupClientMap cgcm with(NOLOCK) on cgcm.ClientGroupID = cg.ClientGroupID and cgcm.ClientID = 
--		[Service] s with (NOLOCK) on s.serviceID = cgsm.ServiceID join
--		UserClientSecurityGroupMap ucsg on ucsg.ClientID = cg. and ucsg.SecurityGroupID = sg.SecurityGroupID
--	where
--		sg.SecurityGroupID = @SecurityGroupID and
--		cgsm.IsEnabled = 1 and
--		sg.IsActive = 1 and
--		s.IsEnabled = 1