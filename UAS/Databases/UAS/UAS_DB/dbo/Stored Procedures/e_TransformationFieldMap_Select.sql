CREATE PROCEDURE e_TransformationFieldMap_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformationFieldMap With(NoLock)
	Where IsActive = 'true'

END