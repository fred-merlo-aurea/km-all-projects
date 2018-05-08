CREATE PROCEDURE [dbo].[e_FilterScheduleLog_Save]
@FilterScheduleID int,
@StartDate date,
@StartTime varchar(8),
@FileName varchar(50),
@DownloadCount int
AS
Begin

	SET NOCOUNT ON
	
	insert into FilterScheduleLog (FilterScheduleID, StartDate, StartTime, [FileName], DownloadCount) 
	values (@FilterScheduleID, @StartDate, @StartTime, @FileName, @DownloadCount)

	Select @@IDENTITY;	
End

