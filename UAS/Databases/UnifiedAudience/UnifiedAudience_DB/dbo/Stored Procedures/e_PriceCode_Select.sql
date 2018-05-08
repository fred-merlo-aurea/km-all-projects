CREATE PROCEDURE e_PriceCode_Select
AS
BEGIN

	set nocount on

	SELECT * FROM PriceCode With(NoLock)

END