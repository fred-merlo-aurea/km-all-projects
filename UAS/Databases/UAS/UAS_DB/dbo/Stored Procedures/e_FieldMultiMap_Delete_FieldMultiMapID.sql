CREATE PROCEDURE [dbo].[e_FieldMultiMap_Delete_FieldMultiMapID]
	@FieldMultiMapID int
AS
BEGIN

	set nocount on

	DELETE FieldMultiMap WHERE FieldMultiMapID = @FieldMultiMapID

END