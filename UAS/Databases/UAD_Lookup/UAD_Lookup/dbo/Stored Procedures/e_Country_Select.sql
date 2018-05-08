CREATE PROCEDURE e_Country_Select
AS
BEGIN

	set nocount on

	SELECT * FROM Country With(NoLock)

END