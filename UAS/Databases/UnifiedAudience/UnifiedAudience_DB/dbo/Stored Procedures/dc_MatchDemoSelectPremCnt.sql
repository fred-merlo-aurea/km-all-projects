CREATE PROCEDURE dc_MatchDemoSelectPremCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @premDB varchar(300) = 'UAD_TEMP.dbo.tmpPremDemo_'+ @TableName
	declare @premDemoSQL nvarchar(1000) = 'select @premDemoCountDynamic = COUNT(*) from ' + @premDB + ' WITH (NOLOCK) where SubscriptionID > 0'
	declare @premDemoParams nvarchar(500) = '@premDemoCountDynamic int OUTPUT'
	declare @premDemoCount int = 0
	EXEC sp_executesql @premDemoSQL, @premDemoParams, @premDemoCountDynamic = @premDemoCount OUTPUT

	select @premDemoCount

END
go