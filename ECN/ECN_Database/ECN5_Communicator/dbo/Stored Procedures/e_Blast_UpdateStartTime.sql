CREATE  PROC [dbo].[e_Blast_UpdateStartTime] 
(
	@BlastID int
)
AS 
BEGIN
	UPDATE Blast SET StartTime = GETDATE()
	WHERE BlastID = @BlastID
END
