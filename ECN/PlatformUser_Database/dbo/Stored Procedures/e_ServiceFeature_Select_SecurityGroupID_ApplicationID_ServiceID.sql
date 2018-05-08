
-- TODO

CREATE PROCEDURE [dbo].[e_ServiceFeature_Select_SecurityGroupID_ApplicationID_ServiceID]
@SecurityGroupID int,
@ApplicationID int,
@ServiceID int
AS
	SELECT s.*
	FROM ServiceFeature s With(NoLock)
		JOIN SecurityGroupServiceFeatureApplicationMap cgsfa with(nolock) on s.ServiceFeatureID = cgsfa.ServiceFeatureID
		--JOIN SecurityGroupServicMap sgsm with(nolock) on cgsfa.SecurityGroupID = sgsm.SecurityGroupID and cgsfa.ServiceID = sgsm.ServiceID
		JOIN SecurityGroup sg with(nolock) on cgsfa.SecurityGroupID = sg.SecurityGroupID 
		--join ApplicationServiceMap asm with(nolock) on cgsfa.ServiceID = asm.ServiceID
		--JOIN Application a with(nolock) on asm.ApplicationID = a.ApplicationID
		--JOIN ApplicationSecurityGroupMap asgm with(nolock) on a.ApplicationID = asgm.ApplicationID and sg.SecurityGroupID = asgm.SecurityGroupID
	WHERE sg.SecurityGroupID= @SecurityGroupID
		AND cgsfa.IsActive = 1 
		AND sg.IsActive = 1 
		--AND asm.IsEnabled = 1
		--AND a.IsActive = 1
		--AND asgm.HasAccess = 1
		--AND sgsm.IsEnabled = 1