CREATE PROCEDURE [dbo].[e_DataCompareDownload_Select_UserId]
@UserId int
AS
	begin
		set nocount on
		select d.*
		from DataCompareDownload d with(nolock)
		join DataCompareView v with(nolock) on d.DcViewId = v.DcViewId
		join DataCompareRun r with(nolock) on v.DcRunId = r.DcRunId
		join SourceFile s with(nolock) on r.SourceFileId = s.SourceFileID
		where s.CreatedByUserID = @UserId
	end
go

