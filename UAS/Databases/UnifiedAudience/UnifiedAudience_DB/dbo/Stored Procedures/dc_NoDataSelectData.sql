CREATE PROCEDURE dc_NoDataSelectData
@TableName varchar(250)
AS
BEGIN

	set nocount on

	declare @return varchar(2000) = 'select * from UAD_TEMP.dbo.tmpNoData_'+ @TableName 
    exec(@return);
    
    declare @stdDrop varchar(1000) = 'drop table UAD_TEMP.dbo.tmpNoData_'+ @TableName
	exec(@stdDrop)

END
go