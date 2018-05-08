create procedure e_DataCompareView_Select_SourceFileId
@SourceFileId int
as
BEGIN

	set nocount on

	select v.*
	from DataCompareView v with(nolock)
	join DataCompareRun r with(nolock) on r.DcRunId = v.DcRunId
	where r.SourceFileId = @SourceFileId

END
go