CREATE PROCEDURE [e_FieldMapping_Select_BySourceFileID]
@SourceFileID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FieldMapping With(NoLock)
	WHERE SourceFileID = @SourceFileID
	ORDER BY SourceFileID,ColumnOrder

END