CREATE PROCEDURE [dbo].[e_MenuFeature_SelectOnlyEnabled_MenuID]
@MenuID int
AS
	select *
	from MenuFeature mf with(nolock)
	join MenuMenuFeatureMap m with(nolock) on mf.MenuFeatureID = m.MenuFeatureID
	and mf.IsActive = 'true'
	where m.MenuID = @MenuID