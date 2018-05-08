CREATE PROCEDURE dc_NoDataCreateTable
@list varchar(max),
@TableName varchar(250),
@ProcessCode varchar(50)
AS
BEGIN

	set nocount on

	declare @sql varchar(max)
	
	set @sql = (' select ' + @list +
				' into UAD_TEMP.dbo.tmpNoData_' + @TableName +
				' from SubscriberFinal sf with(nolock)
				  left join Subscriptions s with(nolock) on sf.IGrp_No = s.IGrp_No
				  where s.IGrp_No is null
				  and sf.ProcessCode = ''' +  @ProcessCode + '''')
	exec(@sql)	

	IF (NOT EXISTS (SELECT * 
                 FROM UAD_TEMP.INFORMATION_SCHEMA.TABLES 
                 WHERE TABLE_SCHEMA = 'dbo' 
                 AND  TABLE_NAME = 'tmpNoData_' + @TableName))
		BEGIN
			declare @sqlStdDemoTableClause varchar(max) 
			set @sqlStdDemoTableClause = ' select 0 as SubscriptionID
								  into UAD_TEMP.dbo.tmpNoData_' + @TableName
			
			exec(@sqlStdDemoTableClause)
		END

END
go