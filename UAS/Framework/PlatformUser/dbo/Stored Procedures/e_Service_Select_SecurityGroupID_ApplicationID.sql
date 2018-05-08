CREATE PROCEDURE [dbo].[e_Service_Select_SecurityGroupID_ApplicationID]
@SecurityGroupID int,
@ApplicationID int
AS
	SELECT s.*
	FROM Service s With(NoLock)		
		JOIN SecurityGroupServicMap sgsm with(nolock) on s.ServiceID = sgsm.ServiceID
		JOIN SecurityGroup sg with(nolock) on sgsm.SecurityGroupID = sg.SecurityGroupID 
		join ApplicationServiceMap asm with(nolock) on sgsm.ServiceID = asm.ServiceID
		JOIN Application a with(nolock) on asm.ApplicationID = a.ApplicationID
		JOIN ApplicationSecurityGroupMap asgm with(nolock) on a.ApplicationID = asgm.ApplicationID and sg.SecurityGroupID = asgm.SecurityGroupID
	WHERE sg.SecurityGroupID= @SecurityGroupID
		AND sgsm.IsEnabled = 1 
		AND sg.IsActive = 1 
		AND asm.IsEnabled = 1
		AND a.IsActive = 1
		AND asgm.HasAccess = 1
