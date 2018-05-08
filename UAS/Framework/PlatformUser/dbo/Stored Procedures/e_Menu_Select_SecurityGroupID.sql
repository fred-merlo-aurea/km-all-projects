CREATE PROCEDURE [dbo].[e_Menu_Select_SecurityGroupID]
@SecurityGroupID int,
@HasAccess bit
AS
SELECT m.* 
FROM Menu m With(NoLock)
JOIN MenuSecurityGroupMap sg With(NoLock) ON m.MenuID = sg.MenuID
WHERE sg.SecurityGroupID = @SecurityGroupID
AND sg.HasAccess = @HasAccess
