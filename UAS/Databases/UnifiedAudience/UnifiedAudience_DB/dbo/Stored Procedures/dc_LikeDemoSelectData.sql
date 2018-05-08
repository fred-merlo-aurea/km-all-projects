CREATE PROCEDURE dc_LikeDemoSelectData
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @return varchar(2000) = 'select * from UAD_TEMP.dbo.tmpStdDemo_'+ @TableName + ' union all select * from UAD_TEMP.dbo.tmpPremDemo_'+ @TableName + ' union all select * from UAD_TEMP.dbo.tmpCustomDemo_'+ @TableName
    exec(@return);
    
    declare @stdDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpStdDemo_'+ @TableName
	exec(@stdDrop)
	declare @premDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpPremDemo_'+ @TableName
	exec(@premDrop)	
	declare @customDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpCustomDemo_'+ @TableName
	exec(@customDrop)	

END
go