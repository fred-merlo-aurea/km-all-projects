CREATE PROCEDURE [dbo].[e_ClientGroupServiceMap_Select_ClientGroupID]
@ClientGroupID int
AS
	SELECT *
	FROM ClientGroupServiceMap With(NoLock)
	WHERE ClientGroupID = @ClientGroupID
