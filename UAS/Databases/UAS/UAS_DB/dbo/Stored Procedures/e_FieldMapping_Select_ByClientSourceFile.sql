CREATE PROCEDURE [dbo].[e_FieldMapping_Select_ByClientSourceFile]
@ClientID int,
@FileName varchar(100)
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FieldMapping WITH(NOLOCK)
	WHERE SourceFileID =
	(
		Select SourceFileID FROM SourceFile WITH(NOLOCK)
		WHERE ClientID = @ClientID and FileName = @FileName and IsDeleted = 0
	)
	ORDER BY SourceFileID,ColumnOrder

END