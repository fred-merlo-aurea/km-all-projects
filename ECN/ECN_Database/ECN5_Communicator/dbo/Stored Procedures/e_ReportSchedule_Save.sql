CREATE  PROC [dbo].[e_ReportSchedule_Save]
(
	@ReportScheduleID int,
	@CustomerID int,
	@ReportID int, 
	@StartTime varchar(100),
	@StartDate varchar(100),
	@EndDate varchar(100),
	@ScheduleType varchar(100),
	@RecurrenceType varchar(100),
	@RunSunday bit,
	@RunMonday bit,
	@RunTuesday bit,
	@RunWednesday bit,
	@RunThursday bit,
	@RunFriday bit,
	@RunSaturday bit,
	@MonthScheduleDay int, 
	@MonthLastDay bit,
	@FromEmail varchar(1500), 
	@FromName varchar(200), 
	@EmailSubject varchar(500), 
	@ToEmail varchar(2000),
	@ReportParameters varchar(2000),
	@UserID int,
	@BlastID int = null,
	@ExportFormat varchar(50)
) 
AS 
BEGIN
	if @ReportScheduleID < 0
	BEGIN
		insert into ReportSchedule
		(CustomerID, ReportID, StartTime, StartDate, EndDate, ScheduleType, RecurrenceType, RunSunday,
		RunMonday, RunTuesday, RunWednesday, RunThursday, RunFriday, RunSaturday, MonthScheduleDay, MonthLastDay, FromEmail, 
		FromName, EmailSubject, ToEmail, ReportParameters,CreatedDate, CreatedUserID, IsDeleted,BlastID, ExportFormat)
	values
		(@CustomerID, @ReportID, @StartTime, @StartDate, @EndDate, @ScheduleType, @RecurrenceType, @RunSunday,
		@RunMonday, @RunTuesday, @RunWednesday, @RunThursday, @RunFriday, @RunSaturday, @MonthScheduleDay, @MonthLastDay, @FromEmail,
		@FromName, @EmailSubject, @ToEmail, @ReportParameters,GETDATE(), @UserID, 0,@BlastID, @ExportFormat)
	SET @ReportScheduleID = @@IDENTITY
	END
	else
	BEGIN
		update ReportSchedule
		set 
		CustomerID= @CustomerID, ReportID= @ReportID, StartTime= @StartTime, StartDate=@StartDate, EndDate=@EndDate, ScheduleType=@ScheduleType,
		RecurrenceType= @RecurrenceType, RunSunday= @RunSunday, RunMonday= @RunMonday, RunTuesday= @RunTuesday, RunWednesday= @RunWednesday, RunThursday= @RunThursday,
		RunFriday= @RunFriday, RunSaturday= @RunSaturday, MonthScheduleDay= @MonthScheduleDay, MonthLastDay = @MonthLastDay, FromEmail= @FromEmail, 
		FromName= @FromName, EmailSubject = @EmailSubject, ToEmail = @ToEmail, UpdatedUserID= @UserID, UpdatedDate= GETDATE(),ReportParameters=@ReportParameters,BlastID=@BlastID, ExportFormat = @ExportFormat
		where ReportScheduleID=@ReportScheduleID
	END
	
	select @ReportScheduleID
END