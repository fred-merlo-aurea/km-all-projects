CREATE PROCEDURE [dbo].[e_User_Select_ClientGroupID_ServiceCode]
@ClientGroupID int,
@ServiceCode varchar(100)
AS
	select 
		distinct u.*
	from 
		[User] u with(nolock)
		JOIN UserClientSecurityGroupMap uc with(nolock) on u.UserID = uc.UserID
		JOIN ClientGroupClientMap cgcm on uc.ClientID = cgcm.ClientID
		JOIN ClientGroupServiceMap cgsm with(nolock) on cgsm.ClientGroupID = cgcm.ClientGroupID
		JOIN ClientServiceMap csm with (NOLOCK) on csm.ClientID = uc.ClientID
		JOIN SecurityGroupPermission sgp with (NOLOCK) on sgp.securitygroupID = uc.securitygroupID 
		JOIN ServiceFeatureAccessMap sfam with (NOLOCK) on sfam.ServiceFeatureAccessMapID = sgp.ServiceFeatureAccessMapID 
		JOIN ServiceFeature sf with (NOLOCK) on sf.ServiceFeatureID = sfam.ServiceFeatureID
		JOIN Access a with (NOLOCK) on sfam.AccessID = a.AccessID
		JOIN [Service] s with (NOLOCK) on s.serviceID = sf.ServiceID and s.ServiceID = cgsm.ServiceID and s.ServiceID = csm.ServiceID
	where 
		cgcm.ClientGroupID = @ClientgroupID 
		AND s.ServiceCode = @ServiceCode
		AND u.Status <> 'DISABLED'  
		AND	u.IsActive = 1
		AND	uc.IsActive = 1  
		AND cgcm.IsActive = 1
		AND csm.IsEnabled = 1
		AND sgp.IsActive = 1
		AND sfam.IsEnabled = 1
		AND sf.IsEnabled = 1
		AND s.IsEnabled = 1
order by u.LastName,u.FirstName
