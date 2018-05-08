CREATE PROCEDURE [dbo].[e_Issue_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT * 
	FROM Issue With(NoLock)

END