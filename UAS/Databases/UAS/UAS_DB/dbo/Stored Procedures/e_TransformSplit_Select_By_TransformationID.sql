CREATE PROCEDURE [dbo].[e_TransformSplit_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformSplit With(NoLock)
	where TransformationID = @TransformationID

END