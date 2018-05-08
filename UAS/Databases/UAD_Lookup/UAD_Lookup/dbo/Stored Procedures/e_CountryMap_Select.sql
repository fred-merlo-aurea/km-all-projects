CREATE PROCEDURE e_CountryMap_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM CountryMap With(NoLock)

END