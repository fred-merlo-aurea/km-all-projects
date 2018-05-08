CREATE PROCEDURE [dbo].[e_Transformation_Select_TransformationID] 
@TransformationID int
AS
BEGIN

	set nocount on

	Select * from Transformation With(NoLock)
	where TransformationID = @TransformationID and IsActive = 'true'

END