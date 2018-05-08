create procedure job_GetFileProcessingStats
@ProcessDate date
as
BEGIN   

	SET NOCOUNT ON 

	select COUNT(SourceFileId) as 'FileCount',
		SUM(ProfileCount) as 'ProfileCount',
		SUM(DemographicCount) as 'DemographicCount',
		ProcessDate
	from FileProcessingStat with(nolock)
	where ProcessDate = @ProcessDate
	group by ProcessDate

END
go