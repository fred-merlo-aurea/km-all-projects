CREATE PROCEDURE [dbo].[e_ClientGroupServiceMap_Select]
AS
	SELECT *
	FROM ClientGroupServiceMap With(NoLock)
	ORDER BY ClientGroupID