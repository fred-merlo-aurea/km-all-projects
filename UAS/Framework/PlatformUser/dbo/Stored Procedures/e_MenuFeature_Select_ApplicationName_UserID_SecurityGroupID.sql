CREATE PROCEDURE [dbo].[e_MenuFeature_Select_ApplicationName_UserID_SecurityGroupID]
@ApplicationName varchar(50),
@UserID int,
@SecurityGroupID int,
@IsActive bit = 1,
@HasAccess bit = 1
AS
	SELECT mf.* 
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
