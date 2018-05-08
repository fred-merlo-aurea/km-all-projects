CREATE PROCEDURE [dbo].[e_Client_Select]
AS
	SELECT * 
	FROM Client With(NoLock)