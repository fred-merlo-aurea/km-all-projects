CREATE PROCEDURE [dbo].[e_ServiceFeature_Select_ServiceFeatureID]
@ServiceFeatureID int
AS
	SELECT *
	FROM ServiceFeature With(NoLock)
	WHERE ServiceFeatureID = @ServiceFeatureID
