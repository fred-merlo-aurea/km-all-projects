CREATE PROCEDURE [dbo].[e_TransformationFieldMap_Delete_TransformationFieldMapID]
@TransformationFieldMapId int
AS
BEGIN

	set nocount on

	Delete from TransformationFieldMap 
	where TransformationFieldMapId = @TransformationFieldMapId;
	Select @TransformationFieldMapId;
	
END
