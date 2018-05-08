CREATE PROCEDURE [dbo].[e_TransformationPubMap_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformationPubMap With(NoLock)
	where TransformationID = @TransformationID

END