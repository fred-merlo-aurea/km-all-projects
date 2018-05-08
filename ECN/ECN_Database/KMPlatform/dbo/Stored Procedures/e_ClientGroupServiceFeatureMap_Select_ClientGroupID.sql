CREATE PROCEDURE [dbo].[e_ClientGroupServiceFeatureMap_Select_ClientGroupID]
@ClientGroupID int
AS
	SELECT c.*
	FROM ClientGroupServiceFeatureMap c With(NoLock) 
	WHERE ClientGroupID = @ClientGroupID