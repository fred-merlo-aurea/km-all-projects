CREATE PROCEDURE e_ClientCustomProcedure_ExecuteSproc_Xml
@sproc varchar(250),
@xml xml
as
BEGIN

	set nocount on

	DECLARE @execsproc varchar(MAX)
	SET @execsproc = 'EXEC '+@sproc + '''' + cast(@xml as varchar(max)) + '''' 
	Exec(@execsproc)

END
go