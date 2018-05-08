CREATE PROCEDURE e_ClientCustomProcedure_ExecuteSproc
@sproc varchar(250),
@srcFile int,
@ProcessCode varchar(50) = '',
@ClientId int,
@FileName varchar(50) = ''
AS
BEGIN

	set nocount on

	DECLARE @execsproc varchar(MAX)
	IF(LEN(@ProcessCode) > 0)
		begin
			SET @execsproc = 'EXEC '+@sproc + ' '+ CAST(@srcFile AS varchar(25)) + ', ' + '''' + @ProcessCode + '''' + ', ' + CAST(@ClientId as varchar(5))
		end
	else
		begin
			SET @execsproc = 'EXEC '+@sproc + ' '+ CAST(@srcFile AS varchar(25)) + ''  + ', ' + CAST(@ClientId as varchar(5))
		end

	Exec(@execsproc)

END