CREATE proc [dbo].[spInsertBlastSchedule] (
	@SchedTime varchar(25),
	@SchedStartDate varchar(25),
	@SchedEndDate varchar(25),
	@Period varchar(1),
	@CreatedBy int,
	@SplitType varchar(1) = ''
	)
as

BEGIN
 	SET NOCOUNT ON
 
	INSERT INTO BlastSchedule
		(SchedTime, SchedStartDate, SchedEndDate, Period, CreatedBy, CreatedDate, SplitType)
		VALUES
		(@SchedTime, @SchedStartDate, @SchedEndDate, @Period, @CreatedBy, GETDATE(), @SplitType)
	SELECT @@IDENTITY 
END

--exec spInsertBlastSchedule '23:00:00', '2012-10-01', '2013-10-01', 'w', 4496, '5/14/2012', 4496, '5/14/2012'
--select * from BlastSchedule
