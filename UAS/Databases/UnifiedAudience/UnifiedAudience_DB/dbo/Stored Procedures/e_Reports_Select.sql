CREATE PROCEDURE [dbo].[e_Reports_Select]
AS
BEGIN

	SET NOCOUNT ON

	SELECT *
	FROM Reports With(NoLock)	

END