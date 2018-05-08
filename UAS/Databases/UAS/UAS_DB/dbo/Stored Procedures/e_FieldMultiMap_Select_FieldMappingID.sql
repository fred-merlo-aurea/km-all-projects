CREATE PROCEDURE [dbo].[e_FieldMultiMap_Select_FieldMappingID]
	@FieldMappingID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FieldMultiMap WITH(NOLOCK)
	WHERE FieldMappingID = @FieldMappingID

END