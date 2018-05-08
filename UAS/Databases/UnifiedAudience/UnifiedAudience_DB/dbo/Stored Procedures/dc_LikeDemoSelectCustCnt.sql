CREATE PROCEDURE dc_LikeDemoSelectCustCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @custDB varchar(300) = 'UAD_TEMP.dbo.tmpCustomDemo_'+ @TableName
	declare @custDemoSQL nvarchar(1000) = 'select @custDemoCountDynamic = COUNT(*) from ' + @custDB + ' WITH (NOLOCK) where SubscriptionID > 0'
	declare @custDemoParams nvarchar(500) = '@custDemoCountDynamic int OUTPUT'
	declare @custDemoCount int = 0
	EXEC sp_executesql @custDemoSQL, @custDemoParams, @custDemoCountDynamic = @custDemoCount OUTPUT

	select @custDemoCount

END
go