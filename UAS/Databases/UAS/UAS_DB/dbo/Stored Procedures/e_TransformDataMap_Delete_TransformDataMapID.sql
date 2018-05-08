CREATE PROCEDURE [dbo].[e_TransformDataMap_Delete_TransformDataMapID]
@TransformDataMapID int
AS	
BEGIN

	set nocount on

	Delete from TransformDataMap
	where TransformDataMapID = @TransformDataMapID;
	Select @TransformDataMapID;
	
END