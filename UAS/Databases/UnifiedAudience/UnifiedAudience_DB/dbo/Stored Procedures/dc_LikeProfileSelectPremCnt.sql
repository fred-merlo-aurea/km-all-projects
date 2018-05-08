CREATE PROCEDURE dc_LikeProfileSelectPremCnt
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @premDB varchar(300) = 'UAD_TEMP.dbo.tmpPremProf_'+ @TableName
	declare @premProfSQL nvarchar(1000) = 'select @premProfCountDynamic = COUNT(*) from ' + @premDB + ' WITH (NOLOCK)'
	declare @premProfParams nvarchar(500) = '@premProfCountDynamic int OUTPUT'
	declare @premProfCount int = 0
	EXEC sp_executesql @premProfSQL, @premProfParams, @premProfCountDynamic = @premProfCount OUTPUT

	select @premProfCount

END
go