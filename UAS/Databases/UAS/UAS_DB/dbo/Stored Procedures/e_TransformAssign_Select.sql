CREATE PROCEDURE e_TransformAssign_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformAssign With(NoLock)

END