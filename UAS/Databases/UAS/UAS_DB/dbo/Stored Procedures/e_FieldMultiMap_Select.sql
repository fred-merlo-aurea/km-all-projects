CREATE PROCEDURE [dbo].[e_FieldMultiMap_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM FieldMultiMap WITH(NOLOCK)

END