CREATE PROCEDURE [dbo].[e_TransformationFieldMultiMap_Delete_ByFieldMultiMapID]
	@FieldMultiMapID int
AS
BEGIN

	set nocount on

	DELETE TransformationFieldMultiMap WHERE FieldMultiMapID = @FieldMultiMapID

END