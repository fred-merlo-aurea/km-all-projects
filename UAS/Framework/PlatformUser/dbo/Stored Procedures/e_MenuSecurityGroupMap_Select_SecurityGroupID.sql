CREATE PROCEDURE [dbo].[e_MenuSecurityGroupMap_Select_SecurityGroupID]
@SecurityGroupID int
AS
	SELECT * FROM MenuSecurityGroupMap With(NoLock) WHERE SecurityGroupID = @SecurityGroupID
