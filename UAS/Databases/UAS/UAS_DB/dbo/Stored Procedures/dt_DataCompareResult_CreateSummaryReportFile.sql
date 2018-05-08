create procedure dt_DataCompareResult_CreateSummaryReportFile
@DataCompareResultId int,
@FileName varchar(250),
@ProcessCode varchar(50)
as
BEGIN

	set nocount on

	declare @userId int = (select q.UserId 
						   from DataCompareResultQue q with(nolock) 
							join DataCompareResult r with(nolock) on q.DataCompareResultQueId= r.DataCompareResultQueId
						   where r.DataCompareResultId = @DataCompareResultId)
	declare @dcResultQueId int = (select DataCompareResultQueId from DataCompareResult with(nolock) where DataCompareResultId = @DataCompareResultId)
						   
	update DataCompareResult
	set TotalDataCompareRecordCount = (select case when SUM(isnull(TotalRecordCount,0)) > 0 then SUM(isnull(TotalRecordCount,0)) else 0 end from DataCompareResultDetail with(nolock) where DataCompareResultId = @DataCompareResultId),
		TotalDataCompareItemCount = (select case when SUM(isnull(TotalItemCount,0)) > 0 then SUM(isnull(TotalItemCount,0)) else 0 end from DataCompareResultDetail with(nolock) where DataCompareResultId = @DataCompareResultId),
		TotalDataCompareCost = (select case when SUM(isnull(TotalCost,0)) > 0 then SUM(isnull(TotalCost,0)) else 0 end from DataCompareResultDetail with(nolock) where DataCompareResultId = @DataCompareResultId),
		DateUpdated = GETDATE(),
		UpdatedByUserID = @userId
	where DataCompareResultId = @DataCompareResultId

	update DataCompareResultQue
	set IsResultComplete = 'true',
		ResultCompleteDate = GETDATE()
	where DataCompareResultQueId = @dcResultQueId

	-- Insert into DataCompareResultDetail  ReportFile, TotalItemCount, TotalCost
	declare @dcOptId int = (select DataCompareOptionId from .DataCompareOption with(nolock) where OptionName = 'Summary Report')
    insert into DataCompareResultDetail (DataCompareResultId,DataCompareOptionId,ReportFile,TotalRecordCount,TotalItemCount,TotalCost,IsPurchased,IsBilled,DateCreated,CreatedByUserId)
    --values(@DataCompareResultId,@dcOptId,@FileName,0,0,0,'true','true',GETDATE(),1)
	select @DataCompareResultId,@dcOptId,@FileName,sum(rd.TotalRecordCount),sum(rd.TotalItemCount),sum(rd.TotalCost),'true','true',GETDATE(),1
	from DataCompareResultDetail rd with(nolock)
	where rd.DataCompareResultId = @DataCompareResultId
	group by rd.DataCompareResultId

	-----------now get the datatable
	declare @sfRecordCount int = (select SourceFileRecordCount from DataCompareResult with(nolock) where DataCompareResultQueId = @dcResultQueId)
	declare @noDataCount int = (select SourceFileNoDataCount from DataCompareResult with(nolock) where DataCompareResultQueId = @dcResultQueId)
	 
	select o.DataCompareOptionId,o.OptionName,rd.TotalRecordCount,rd.TotalItemCount,rd.TotalCost
	from DataCompareResultDetail rd with(nolock)
		join DataCompareOption o with(nolock) on rd.DataCompareOptionId = o.DataCompareOptionId
	where rd.DataCompareResultId = @DataCompareResultId
		and o.OptionName in ('Matching Profiles','Matching Demographics','Like Profiles','Like Demographics')
	union
	select 98,'No Data',@noDataCount,0,0
	union
	select 99,'Source File Record Count',@sfRecordCount,0,0
	order by o.DataCompareOptionId

END
go