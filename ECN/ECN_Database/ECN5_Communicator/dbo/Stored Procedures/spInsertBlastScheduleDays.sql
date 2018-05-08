CREATE proc [dbo].[spInsertBlastScheduleDays] (
	@blastscheduleID int = null,
	@DayToSend int = null,
	@IsAmount bit = null,
	@Total int = null,
	@Weeks int = null)
as

BEGIN
 	SET NOCOUNT ON
 
	INSERT INTO BlastScheduleDays
		(BlastScheduleID, DayToSend, IsAmount, Total, Weeks)
		VALUES
		(@blastscheduleID, @DayToSend, @IsAmount, @Total, @Weeks)
	SELECT @@IDENTITY 
END
