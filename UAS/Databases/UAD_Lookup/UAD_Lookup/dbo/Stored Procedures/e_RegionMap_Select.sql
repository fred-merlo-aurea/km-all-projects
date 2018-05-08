CREATE PROCEDURE e_RegionMap_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM RegionMap With(NoLock)

END