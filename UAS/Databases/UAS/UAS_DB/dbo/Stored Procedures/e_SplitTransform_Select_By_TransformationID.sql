CREATE PROCEDURE [dbo].[e_SplitTransform_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformSplitTrans With(NoLock)
	where TransformationID = @TransformationID

END
GO