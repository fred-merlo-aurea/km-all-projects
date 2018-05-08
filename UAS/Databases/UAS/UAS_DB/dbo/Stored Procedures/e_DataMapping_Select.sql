CREATE PROCEDURE e_DataMapping_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM DataMapping With(NoLock)

END