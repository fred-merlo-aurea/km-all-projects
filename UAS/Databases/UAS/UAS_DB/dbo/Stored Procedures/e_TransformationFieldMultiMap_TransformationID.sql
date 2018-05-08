CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_TransformationID]
	@TransformationID int
AS
BEGIN

	set nocount on

	SELECT * FROM TransformationFieldMultiMap With(NoLock) WHERE TransformationID = @TransformationID

END