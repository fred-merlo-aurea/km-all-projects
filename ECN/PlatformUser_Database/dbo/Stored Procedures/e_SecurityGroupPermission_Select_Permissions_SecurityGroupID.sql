CREATE PROCEDURE [dbo].[e_SecurityGroupPermission_Select_Permissions_SecurityGroupID]  
(
	@SecurityGroupID int
)
as
Begin
	declare @clientgroupID int,
			@ClientID int
			
	select @clientgroupID = ISNULL(clientgroupID,0), @ClientID = ISNULL(clientID, 0)
	from 
		SecurityGroup with (NOLOCK)
	where
		SecurityGroupID = @securityGroupID		

if @ClientID > 0
	select	sgp.*,
			s.ServiceID,
			s.ServiceCode,
			sf.ServiceFeatureID,
			sf.SFCode,
			a.AccessID,
			a.AccessCode
	 from
		securityGroup sg with (NOLOCK) join
		SecurityGroupPermission sgp with (NOLOCK) on sgp.securitygroupID = sg.securitygroupID join
		ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
		ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
		Access a with (NOLOCK) on sfam.AccessID = a.AccessID join
		[Service] s with (NOLOCK) on s.serviceID = sf.ServiceID join 
		ClientServiceFeatureMap csfm with (NOLOCK) on csfm.clientID = sg.ClientID and csfm.ServiceFeatureID = sf.ServiceFeatureID join
		ClientServiceMap csm with (NOLOCK) on csm.ClientID = sg.ClientID and csm.ServiceID = s.ServiceID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		csfm.IsEnabled = 1 and
		csm.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sf.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sgp.IsActive = 1 and
		sg.IsActive = 1
		
else if @clientgroupID > 0
	select sgp.*,
			s.ServiceID,
			s.ServiceCode,
			sf.ServiceFeatureID,
			sf.SFCode,
			a.AccessID,
			a.AccessCode
	from
		securityGroup sg with (NOLOCK) join
		SecurityGroupPermission sgp with (NOLOCK) on sgp.securitygroupID = sg.securitygroupID join
		ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
		ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
		Access a with (NOLOCK) on sfam.AccessID = a.AccessID join
		[Service] s with (NOLOCK) on s.serviceID = sf.ServiceID join 
		ClientGroupServiceFeatureMap cgsfm with (NOLOCK) on cgsfm.ClientGroupID = sg.ClientGroupID and cgsfm.ServiceFeatureID = sf.ServiceFeatureID join
		ClientGroupServiceMap cgsm with (NOLOCK) on cgsm.ClientGroupID = sg.ClientGroupID and cgsm.ServiceID = s.ServiceID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		cgsfm.IsEnabled = 1 and
		cgsm.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sf.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sgp.IsActive = 1 and
		sg.IsActive = 1
End