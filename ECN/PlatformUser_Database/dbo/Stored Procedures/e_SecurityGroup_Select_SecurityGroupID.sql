CREATE PROCEDURE [dbo].[e_SecurityGroup_Select_SecurityGroupID]
	@SecurityGroupID int
AS
	SELECT * FROM SecurityGroup With(NoLock)
	WHERE SecurityGroupID = @SecurityGroupID