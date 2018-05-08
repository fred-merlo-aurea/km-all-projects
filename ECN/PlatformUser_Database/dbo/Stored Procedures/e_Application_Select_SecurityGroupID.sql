CREATE PROCEDURE [dbo].[e_Application_Select_SecurityGroupID]
@SecurityGroupID int
AS
	SELECT distinct a.* 
	FROM Application a With(NoLock)
	JOIN ApplicationSecurityGroupMap am With(NoLock) on a.ApplicationID = am.ApplicationID
	WHERE am.SecurityGroupID = @SecurityGroupID
	AND a.IsActive = 'true'
	AND am.HasAccess = 'true'
	order by a.ApplicationName
GO

