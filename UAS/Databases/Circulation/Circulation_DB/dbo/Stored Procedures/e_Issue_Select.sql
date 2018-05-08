CREATE PROCEDURE [dbo].[e_Issue_Select]
AS
	SELECT * FROM Issue With(NoLock)
