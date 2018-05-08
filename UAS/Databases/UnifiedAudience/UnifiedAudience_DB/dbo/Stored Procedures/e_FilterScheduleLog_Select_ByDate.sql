CREATE PROCEDURE [dbo].[e_FilterScheduleLog_Select_ByDate]
@dt date
AS
BEGIN

	SET NOCOUNT ON
	
	Select 
		f.Name as 'Filter Name' ,
		case when fs.ExportTypeID = 1 then 'FTP' when fs.ExportTypeID = 2 then 'ECN' when fs.ExportTypeID = 4 then 'Marketo' end as Destination,
		fsl.DownloadCount as 'Total Records Exported'
	from FilterScheduleLog fsl with(nolock) 
		join FilterSchedule fs  with(nolock)  on fs.FilterScheduleID = fsl.FilterScheduleID
		join Filters f with(nolock)  on f.FilterID = fs.FilterID
	where fsl.StartDate >= dateadd(d, datediff(d, 0, @dt)-1, 0) and fsl.StartDate < dateadd(d, datediff(d, 0, @dt), 0)

End
