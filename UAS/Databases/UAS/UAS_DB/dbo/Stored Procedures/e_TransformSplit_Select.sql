CREATE PROCEDURE e_TransformSplit_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformSplit With(NoLock)

END