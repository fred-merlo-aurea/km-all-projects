CREATE PROCEDURE [dbo].[e_TransformJoin_Select_By_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM TransformJoin With(NoLock)
	where TransformationID = @TransformationID

END