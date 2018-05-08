CREATE PROCEDURE e_TransformJoin_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformJoin With(NoLock)

END