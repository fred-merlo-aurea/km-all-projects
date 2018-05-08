CREATE PROCEDURE [dbo].[e_TransformAssign_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformAssign With(NoLock)
	where TransformationID = @TransformationID

END