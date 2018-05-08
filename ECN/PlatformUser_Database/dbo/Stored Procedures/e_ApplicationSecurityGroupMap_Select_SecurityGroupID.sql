CREATE PROCEDURE [e_ApplicationSecurityGroupMap_Select_SecurityGroupID]
@SecurityGroupID int
AS
	SELECT * FROM ApplicationSecurityGroupMap With(NoLock) WHERE SecurityGroupID = @SecurityGroupID

