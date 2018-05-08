
CREATE PROCEDURE [dbo].[e_Service_Select_SecurityGroup]
@SecurityGroupID int
as
	select	distinct s.*
	 from
		SecurityGroup sg with (NOLOCK) join
		SecurityGroupPermission sgp with (NOLOCK) on sgp.SecurityGroupID = sg.SecurityGroupID join
		ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID join
		ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID join
		[Service] s with (NOLOCK) on s.ServiceID = sf.ServiceID
	where
		sg.SecurityGroupID = @SecurityGroupID and
		sg.IsActive = 1 and
		sgp.IsActive = 1 and
		sfam.IsEnabled = 1 and
		sf.IsEnabled = 1