CREATE PROCEDURE [dbo].[e_GlobalMasterSuppressionList_Select_EmailAddress]   
@EmailAddress varchar(100) = NULL
AS
	DECLARE @SQLSelect varchar(1000)
	set @SQLSelect = 'SELECT * FROM GlobalMasterSuppressionList WITH (NOLOCK) WHERE EmailAddress like ''%' + @EmailAddress + '%'' and IsDeleted = 0'
	EXEC (@SQLSelect)