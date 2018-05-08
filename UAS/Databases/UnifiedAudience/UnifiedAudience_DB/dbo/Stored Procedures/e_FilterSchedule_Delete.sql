CREATE proc [dbo].[e_FilterSchedule_Delete](
@FilterScheduleID int,
@UserID int
)
as
BEGIN

	SET NOCOUNT ON

	update FilterSchedule 
	set IsDeleted = 1, UpdatedBy = @UserID, UpdatedDate=GETDATE() 
	where FilterScheduleID = @FilterScheduleID 

End