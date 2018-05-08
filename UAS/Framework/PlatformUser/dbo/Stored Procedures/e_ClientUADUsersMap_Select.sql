CREATE PROCEDURE [dbo].[e_ClientUADUsersMap_Select]
AS
	SELECT *
	FROM ClientUADUsersMap With(NoLock)
