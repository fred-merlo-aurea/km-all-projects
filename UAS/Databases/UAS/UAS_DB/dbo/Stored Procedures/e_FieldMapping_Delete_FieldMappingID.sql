CREATE PROCEDURE [dbo].[e_FieldMapping_Delete_FieldMappingID]
@FieldMappingID int
AS
BEGIN

	set nocount on

	DELETE FROM FieldMapping
	WHERE FieldMappingID = @FieldMappingID;
	Select 1

END