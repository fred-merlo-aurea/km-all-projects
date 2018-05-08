CREATE PROCEDURE [dbo].[e_ClientGroupServiceFeatureMap_Select_ClientGroupID_ServiceID]
@ClientGroupID int,
@ServiceID int
AS
	SELECT *
	FROM ClientGroupServiceFeatureMap With(NoLock)
	WHERE ClientGroupID = @ClientGroupID 
	AND ServiceID = @ServiceID
