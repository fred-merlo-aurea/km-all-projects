CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Select_TransformationID]
@TransformationID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM TransformationFieldMap With(NoLock)
	Where TransformationID = @TransformationID and IsActive = 'true'

END