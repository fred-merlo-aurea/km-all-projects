CREATE PROCEDURE [dbo].[e_UserClientSecurityGroupMap_Select_SecurityGroupID]
@SecurityGroupID int
AS
	select *
	from UserClientSecurityGroupMap with(nolock)
	where SecurityGroupID = @SecurityGroupID