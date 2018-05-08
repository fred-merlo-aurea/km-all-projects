CREATE PROCEDURE dc_MatchProfileSelectStandCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @stdDB varchar(300) = 'UAD_TEMP.dbo.tmpStdProf_'+ @TableName
	declare @stdProfSQL nvarchar(1000) = 'select @stdProfCountDynamic = COUNT(*) from ' + @stdDB + ' WITH (NOLOCK)'
	declare @stdProfParams nvarchar(500) = '@stdProfCountDynamic int OUTPUT'
	declare @stdProfCount int = 0
	EXEC sp_executesql @stdProfSQL, @stdProfParams, @stdProfCountDynamic = @stdProfCount OUTPUT;

	select @stdProfCount

END
go