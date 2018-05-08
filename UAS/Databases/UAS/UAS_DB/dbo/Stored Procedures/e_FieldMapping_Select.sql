CREATE PROCEDURE e_FieldMapping_Select
AS
BEGIN

	set nocount on

	SELECT *
	FROM FieldMapping With(NoLock)
	ORDER BY SourceFileID,ColumnOrder

END