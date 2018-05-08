CREATE proc [dbo].[spInsertBlastScheduleHistory] (
	@BlastID int = 0,
	@blastscheduleID int = 0,
	@Action varchar(50))
as

BEGIN
 	SET NOCOUNT ON 	
 	
 	if(@blastscheduleID = 0)
 	Begin
 		Select @blastscheduleID = BlastScheduleID from [BLAST] Where BlastID = @BlastID
 	End
 
	INSERT INTO BlastScheduleHistory
		(BlastScheduleID, SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, [Action])
		SELECT 
			@blastscheduleID, SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate, UpdatedBy, UpdatedDate, @Action
		FROM
			BlastSchedule
		WHERE
			BlastScheduleID = @blastscheduleID
	DECLARE @HistoryID int
	set @HistoryID = @@IDENTITY		
	INSERT INTO BlastScheduleDaysHistory
		(BlastScheduleHistoryID, BlastScheduleDaysID, BlastScheduleID, DayToSend, IsAmount, Total, Weeks)
		SELECT 
			@HistoryID,BlastScheduleDaysID, @blastscheduleID, DayToSend, IsAmount, Total, Weeks
		FROM
			BlastScheduleDays
		WHERE
			BlastScheduleID = @blastscheduleID
			
	SELECT @blastscheduleID
		 
END

--DECLARE @blastscheduleID int
--exec spInsertBlastSchedule '23:00:00', '2012-10-01', '2013-10-01', 'w', 4496, '5/14/2012', 4496, '5/14/2012'
--set @blastscheduleID = @@IDENTITY
--exec spInsertBlastScheduleDays @blastscheduleID, 15, 'false', 50
--select * from BlastSchedule
--select * from BlastScheduleDays

--exec spInsertBlastScheduleHistory 4, 'UPDATE'
