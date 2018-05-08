CREATE PROCEDURE [dbo].[e_Menu_Select_ApplicationName_UserID_SecurityGroupID]
@ApplicationName varchar(50),
@UserID int,
@SecurityGroupID int,
@IsActive bit,
@HasAccess bit,
@IsServiceFeature bit = 0
AS
	SELECT m.* 
	FROM Menu m With(NoLock)
	JOIN Application a With(NoLock) ON a.ApplicationID = m.ApplicationID
	JOIN MenuSecurityGroupMap sg With(NoLock) ON m.MenuID = sg.MenuID
	JOIN UserClientSecurityGroupMap u with(nolock) on sg.SecurityGroupID = u.SecurityGroupID
	WHERE sg.SecurityGroupID = @SecurityGroupID
	and u.UserID = @UserID
	and m.IsActive = @IsActive
	and sg.IsActive = @IsActive
	and sg.HasAccess = @HasAccess
	and u.IsActive = @IsActive
	and a.ApplicationName = @ApplicationName
