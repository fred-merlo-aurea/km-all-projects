
CREATE  PROC [dbo].[e_ReportSchedule_Select_BlastId]
(
	@BlastID int
) 
AS 
BEGIN
	select * from ecn5_communicator..ReportSchedule where BlastID = @BlastID and IsDeleted = 0
END

GO

