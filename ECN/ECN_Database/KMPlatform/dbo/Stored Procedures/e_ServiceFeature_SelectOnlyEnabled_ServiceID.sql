CREATE PROCEDURE [dbo].[e_ServiceFeature_SelectOnlyEnabled_ServiceID]
@ServiceID int
AS
	SELECT *
	FROM ServiceFeature With(NoLock)
	WHERE ServiceID = @ServiceID
	and IsEnabled = 'true'