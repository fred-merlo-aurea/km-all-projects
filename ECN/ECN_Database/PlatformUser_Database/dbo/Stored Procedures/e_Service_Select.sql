CREATE PROCEDURE [dbo].[e_Service_Select]
AS
	SELECT *
	FROM Service With(NoLock)
GO
