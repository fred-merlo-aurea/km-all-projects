CREATE PROCEDURE [dbo].[e_Menu_Select_UserID_SecurityGroupID]
@UserID int,
@SecurityGroupID int,
@IsActive bit,
@HasAccess bit
AS
	SELECT m.* 
	FROM Menu m With(NoLock)
	JOIN MenuSecurityGroupMap sg With(NoLock) ON m.MenuID = sg.MenuID
	JOIN UserClientSecurityGroupMap u with(nolock) on sg.SecurityGroupID = u.SecurityGroupID
	WHERE sg.SecurityGroupID = @SecurityGroupID
	and u.UserID = @UserID
	and m.IsActive = @IsActive
	and sg.IsActive = @IsActive
	and sg.HasAccess = @HasAccess
	and u.IsActive = @IsActive
