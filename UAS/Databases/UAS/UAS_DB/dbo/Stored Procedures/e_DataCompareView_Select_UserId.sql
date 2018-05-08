create procedure e_DataCompareResult_Select_UserId
@UserId int
as
BEGIN

	set nocount on

	select v.*
	from DataCompareView v with(nolock)
	join DataCompareRun r with(nolock) on r.DcRunId = v.DcRunId
	join SourceFile s with(nolock) on s.SourceFileID = r.SourceFileId
	where s.CreatedByUserID = @UserId

END
go