CREATE PROCEDURE [dbo].[e_ServiceFeature_Select_ServiceID]
@ServiceID int
AS
	SELECT *
	FROM ServiceFeature With(NoLock)
	WHERE ServiceID = @ServiceID