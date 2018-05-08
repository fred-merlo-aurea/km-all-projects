CREATE PROCEDURE dc_NoDataSelectCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @ndDB varchar(300) = 'UAD_TEMP.dbo.tmpNoData_'+ @TableName
	declare @ndSQL nvarchar(1000) = 'select @ndCountDynamic = COUNT(*) from ' + @ndDB + ' WITH (NOLOCK)'
	declare @ndParams nvarchar(500) = '@ndCountDynamic int OUTPUT'
	declare @ndCount int = 0
	EXEC sp_executesql @ndSQL, @ndParams, @ndCountDynamic = @ndCount OUTPUT;

	select @ndCount

END
go