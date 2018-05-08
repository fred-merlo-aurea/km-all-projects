CREATE PROCEDURE [dbo].[e_MenuFeatureSecurityGroupMap_Select_SecurityGroupID]
@SecurityGroupID int
AS
	select *
	from MenuFeatureSecurityGroupMap
	where SecurityGroupID = @SecurityGroupID