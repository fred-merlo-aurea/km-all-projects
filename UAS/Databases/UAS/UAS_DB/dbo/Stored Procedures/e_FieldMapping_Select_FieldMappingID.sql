CREATE PROCEDURE e_FieldMapping_Select_FieldMappingID
@FieldMappingID int
AS
BEGIN

	set nocount on

	SELECT *
	FROM FieldMapping With(NoLock)
	WHERE FieldMappingID = @FieldMappingID
	ORDER BY SourceFileID,ColumnOrder

END