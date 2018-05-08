CREATE proc [dbo].[spUpdateBlastSchedule] (
	@blastscheduleID int,
	@SchedTime varchar(25),
	@SchedStartDate varchar(25),
	@SchedEndDate varchar(25),
	@Period varchar(1),
	@UpdatedBy int)
as

BEGIN
 	SET NOCOUNT ON
 
	UPDATE 
		BlastSchedule
	SET
		SchedTime = @SchedTime, SchedStartDate = @SchedStartDate, Period = @Period, UpdatedBy = @UpdatedBy, UpdatedDate = GETDATE()
	WHERE
		BlastScheduleID = @blastscheduleID
END

--exec spInsertBlastSchedule '23:00:00', '2012-10-01', '2013-10-01', 'w', 4496, '5/14/2012', 4496, '5/14/2012'
--select * from BlastSchedule
