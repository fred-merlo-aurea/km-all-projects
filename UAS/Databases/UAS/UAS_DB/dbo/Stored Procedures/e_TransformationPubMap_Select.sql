CREATE PROCEDURE e_TransformationPubMap_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformationPubMap With(NoLock)

END