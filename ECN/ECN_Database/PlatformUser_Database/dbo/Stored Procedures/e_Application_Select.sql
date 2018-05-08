CREATE PROCEDURE [dbo].[e_Application_Select]
AS
	SELECT *
	FROM Application With(NoLock)
	order by ApplicationName
GO
