CREATE PROCEDURE e_Region_Select
AS
BEGIN

	set nocount on

	SELECT * FROM Region With(NoLock)

END