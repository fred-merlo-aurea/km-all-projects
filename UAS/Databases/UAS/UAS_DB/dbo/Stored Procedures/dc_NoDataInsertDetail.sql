CREATE PROCEDURE dc_NoDataInsertDetail
@dcResultQueId int,
@DataCompareResultId int,
@ndCount int,
@FileName varchar(250)
AS
BEGIN

	set nocount on

	declare @noDataId int = (select DataCompareOptionId from DataCompareOption with(nolock) where OptionName = 'No Data')
	insert into DataCompareResultDetail (DataCompareResultId,DataCompareOptionId,ReportFile,TotalRecordCount,TotalItemCount,TotalCost,IsPurchased,IsBilled,DateCreated,CreatedByUserId)
    values(@DataCompareResultId,@noDataId,@FileName,@ndCount,0,0,'false','false',GETDATE(),1)

END
go