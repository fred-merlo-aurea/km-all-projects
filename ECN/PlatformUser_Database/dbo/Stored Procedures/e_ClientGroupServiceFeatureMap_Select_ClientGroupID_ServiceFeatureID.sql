CREATE PROCEDURE [dbo].[e_ClientGroupServiceFeatureMap_Select_ClientGroupID_ServiceFeatureID]
@ClientGroupID int,
@ServiceFeatureID int
AS
	SELECT *
	FROM ClientGroupServiceFeatureMap With(NoLock)
	WHERE ClientGroupID = @ClientGroupID 
	AND ServiceFeatureID = @ServiceFeatureID
GO