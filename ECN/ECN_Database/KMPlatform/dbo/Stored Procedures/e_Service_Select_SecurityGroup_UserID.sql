CREATE PROCEDURE [dbo].[e_Service_Select_SecurityGroup_USerID]
@SecurityGroupID int,
@USerID int
as
	declare @ClientGroupID int,
			@ClientID int
			
	select @ClientGroupID = ISNULL(ClientGroupID,0), @ClientID = ISNULL(ClientID, -1)
	from 
		SecurityGroup with (NOLOCK)
	where
		SecurityGroupID = @SecurityGroupID		

if @ClientID > -1
	select	distinct s.*
	 from
		SecurityGroup sg with (NOLOCK) join
		SecurityGroupPermission sgp with (NOLOCK) on sgp.SecurityGroupID = sg.SecurityGroupID join
		ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
		ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
		[Service] s with (NOLOCK) on s.ServiceID = sf.ServiceID join 
		ClientServiceFeatureMap csfm with (NOLOCK) on csfm.ClientID = sg.ClientID and csfm.ServiceFeatureID = sf.ServiceFeatureID join
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
		
else if @ClientGroupID > 0
	select s.*
	from
		SecurityGroup sg with (NOLOCK) join
		SecurityGroupPermission sgp with (NOLOCK) on sgp.SecurityGroupID = sg.SecurityGroupID join
		ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
		ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
		[Service] s with (NOLOCK) on s.ServiceID = sf.ServiceID join 
		ClientGroupServiceFeatureMap cgsfm with (NOLOCK) on cgsfm.ClientGroupID = sg.ClientGroupID and cgsfm.ServiceFeatureID = sf.ServiceFeatureID join
		ClientGroupServiceMap cgsm with (NOLOCK) on cgsm.ClientGroupID = sg.ClientID and cgsm.ServiceID = s.ServiceID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		cgsfm.IsEnabled = 1 and
		cgsm.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sf.IsEnabled = 1 and
		sfam.IsEnabled = 1 and
		sgp.IsActive = 1 and
		sg.IsActive = 1