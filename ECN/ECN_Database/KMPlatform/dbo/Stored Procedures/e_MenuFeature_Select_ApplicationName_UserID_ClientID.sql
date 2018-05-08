CREATE PROCEDURE [dbo].[e_MenuFeature_Select_ApplicationName_UserID_ClientID]
@ApplicationName varchar(50),
@UserID int,
@ClientID int,
@IsActive bit = 1,
@HasAccess bit = 1,
@IsEnabled bit = 1
AS
	SELECT clperms.* 
	  FROM ApplicationSecurityGroupClientGroupServiceFeatureMap clperms With(NoLock) 
	  JOIN Application a (NOLOCK)ON a.ApplicationID = clperms.ApplicationID
	  JOIN SecurityGroup sg (NOLOCK)ON sg.SecurityGroupID = clperms.SecurityGroupID
	  JOIN ClientGroup cg (NOLOCK)ON cg.ClientGroupID = clperms.ClientGroupID
	  JOIN ClientGroupClientMap cgcm (NOLOCK)ON cgcm.ClientGroupID = clperms.ClientGroupID
	  JOIN Client c (NOLOCK)ON c.ClientID = cgcm.ClientID
	  JOIN UserClientSecurityGroupMap ucsgm(NOLOCK)ON ucsgm.ClientID = c.ClientID AND ucsgm.SecurityGroupID = clperms.SecurityGroupID
	  JOIN [User] u (NOLOCK)ON u.UserID = ucsgm.UserID
	  JOIN ServiceFeature sf (NOLOCK)ON sf.ServiceFeatureID = clperms.ServiceFeatureID
	  JOIN Service s (NOLOCK)ON s.ServiceID = clperms.ServiceID
	  JOIN ApplicationSecurityGroupMap asg (NOLOCK)ON asg.ApplicationID = clperms.ApplicationID      AND asg.SecurityGroupID = clperms.ApplicationID
	  JOIN SecurityGroupServicMap sgs (NOLOCK) ON sgs.SecurityGroupID = clperms.SecurityGroupID        and sgs.ServiceID       = clperms.ServiceID
	  JOIN ClientGroupSecurityGroupMap cgsg (NOLOCK) ON cgsg.SecurityGroupID = clperms.SecurityGroupID AND cgsg.ClientGroupID = clperms.ClientGroupID
	 WHERE clperms.HasAccess = @HasAccess 
	   and a.IsActive     = @IsActive
	   and sg.IsActive    = @IsActive
	   AND cg.IsActive = @IsActive
	   AND cgcm.IsActive = @IsActive
	   and c.IsActive     = @IsActive
	   and u.IsActive     = @IsActive
	   and ucsgm.IsActive = @IsActive
	   and sf.IsEnabled   = @IsEnabled
	   and s.IsEnabled    = @IsEnabled
	   and asg.HasAccess  = @HasAccess
	   and sgs.IsEnabled  = @IsEnabled
	   and cgsg.IsActive  = @IsActive
	   
	   AND u.UserID   = @UserID 
	   AND c.ClientID = @ClientID
	   AND a.ApplicationName = @ApplicationName
    -- AND clperms.SecurityGroupID = @SecurityGroupID --AND clperms.ClientGroupID   = @ClientGroupID --AND 	       
           ;
/*	SELECT mf.* 
	FROM MenuFeature mf With(NoLock)
	JOIN SecurityGroup sg with(Nolock) on sg.SecurityGroupID = @SecurityGroupID
	JOIN MenuFeatureSecurityGroupMap mfsg with(nolock) on mf.MenuFeatureID = mfsg.MenuFeatureID and mfsg.SecurityGroupID = @SecurityGroupID
	JOIN ApplicationSecurityGroupMap asgm with(nolock) on asgm.SecurityGroupID = @SecurityGroupID
	JOIN Application a With(NoLock) ON a.ApplicationID = asgm.ApplicationID
	JOIN UserClientSecurityGroupMap u with(nolock) on asgm.SecurityGroupID = u.SecurityGroupID
	WHERE asgm.SecurityGroupID = @SecurityGroupID
	and u.UserID = @UserID
	and mf.IsActive = @IsActive
	and mfsg.IsActive = @IsActive
	and asgm.HasAccess = @HasAccess
	and u.IsActive = @IsActive
	and sg.IsActive = @IsActive
	and a.ApplicationName = @ApplicationName
*/