
CREATE PROCEDURE [dbo].[e_TransformDataMap_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformDataMap With(NoLock)
	where TransformationID = @TransformationID

END