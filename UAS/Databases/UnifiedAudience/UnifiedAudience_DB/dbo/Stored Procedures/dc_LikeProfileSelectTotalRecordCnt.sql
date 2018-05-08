CREATE PROCEDURE dc_LikeProfileSelectTotalRecordCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @totalRecordCount int = 0;
	declare @countSQL nvarchar(max) = 'select @recCount = COUNT(*) from UAD_TEMP.dbo.tmpStdProf_'+ @TableName + ' s  WITH (NOLOCK) full join UAD_TEMP.dbo.tmpPremProf_'+ @TableName + ' p WITH (NOLOCK) on s.SubscriptionID = p.SubscriptionID'
	declare @countParams nvarchar(500) = '@recCount int OUTPUT'
	EXEC sp_executesql @countSQL, @countParams, @recCount = @totalRecordCount OUTPUT

	select @totalRecordCount

END
go