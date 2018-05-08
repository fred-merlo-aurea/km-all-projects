CREATE PROCEDURE [dbo].[e_MenuFeatureSecurityGroupMap_Select_MenuFeatureID]
@MenuFeatureID int
AS
	select *
	from MenuFeatureSecurityGroupMap
	where MenuFeatureID = @MenuFeatureID
