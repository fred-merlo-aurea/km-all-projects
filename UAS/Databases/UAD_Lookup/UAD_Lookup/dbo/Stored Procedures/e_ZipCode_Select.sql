CREATE PROCEDURE [dbo].[e_ZipCode_Select]
AS
BEGIN

	set nocount on

	SELECT * 
	FROM ZipCode WITH(NOLOCK)

END