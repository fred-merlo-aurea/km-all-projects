CREATE PROCEDURE [dbo].[e_FieldMultiMap_Select_ByFieldMultiMapID]
	@FieldMultiMapID int
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FieldMultiMap WITH(NOLOCK)
	WHERE FieldMultiMapID = @FieldMultiMapID

END