CREATE PROCEDURE [dbo].[e_Service_AMS_Select_SecurityGroup]
@SecurityGroupID int
as
	declare @ClientID int
			
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
		[Service] s with (NOLOCK) on s.serviceID = csm.ServiceID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		csm.IsEnabled = 1 and
		sg.IsActive = 1