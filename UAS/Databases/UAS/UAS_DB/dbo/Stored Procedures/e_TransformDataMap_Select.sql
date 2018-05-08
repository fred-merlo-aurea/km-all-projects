CREATE PROCEDURE e_TransformDataMap_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformDataMap With(NoLock)

END