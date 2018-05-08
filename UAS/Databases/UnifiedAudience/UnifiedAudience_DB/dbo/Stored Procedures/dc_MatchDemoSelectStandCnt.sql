CREATE PROCEDURE dc_MatchDemoSelectStandCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @stdDB varchar(300) = 'UAD_TEMP.dbo.tmpStdDemo_'+ @TableName
	declare @stdDemoSQL nvarchar(1000) = 'select @stdDemoCountDynamic = COUNT(*) from ' + @stdDB + ' WITH (NOLOCK) where SubscriptionID > 0'
	declare @stdDemoParams nvarchar(500) = '@stdDemoCountDynamic int OUTPUT'
	declare @stdDemoCount int = 0
	EXEC sp_executesql @stdDemoSQL, @stdDemoParams, @stdDemoCountDynamic = @stdDemoCount OUTPUT;

	select @stdDemoCount

END
go