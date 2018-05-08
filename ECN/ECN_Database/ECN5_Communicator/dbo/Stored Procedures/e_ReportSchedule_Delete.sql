CREATE  PROC dbo.e_ReportSchedule_Delete
(
	@ReportScheduleID int,
	@UserID int
) 
AS 
BEGIN
	update ReportSchedule set IsDeleted=1, UpdatedDate= GETDATE(), UpdatedUserID= @UserID
	where ReportScheduleID= @ReportScheduleID
END