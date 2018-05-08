CREATE PROCEDURE [e_ClientCustomProcedure_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM ClientCustomProcedure With(NoLock)

END