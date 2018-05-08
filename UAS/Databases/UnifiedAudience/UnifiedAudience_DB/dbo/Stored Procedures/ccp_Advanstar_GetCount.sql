CREATE PROCEDURE [dbo].[ccp_Advanstar_GetCount]
@tableName varchar(250)
AS
BEGIN

	set nocount on

	DECLARE @sqlCommand varchar(MAX)
	SET @sqlCommand = 'SELECT COUNT(*) FROM '+ CAST(@tableName AS varchar(250)) + ' WITH(Nolock)'

	Exec(@sqlCommand)

END
GO