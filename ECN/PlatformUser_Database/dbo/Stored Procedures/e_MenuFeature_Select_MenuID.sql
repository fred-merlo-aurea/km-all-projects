CREATE PROCEDURE [dbo].[e_MenuFeature_Select_MenuID]
@MenuID int
AS
	select *
	from MenuFeature mf with(nolock)
	join MenuMenuFeatureMap m with(nolock) on mf.MenuFeatureID = m.MenuFeatureID
	where m.MenuID = @MenuID
GO

