

CREATE  PROC [dbo].[e_ReportSchedule_Delete_BlastId]
(
	@BlastID int,
	@UserID int
) 
AS 
BEGIN
	update ReportSchedule set IsDeleted=1, UpdatedDate= GETDATE(), UpdatedUserID= @UserID
	where BlastID = @BlastID
END