CREATE PROCEDURE [dbo].[e_Blast_UpdateStatusBlastEngineID]
	@BlastID int,
	@Status varchar(50)
AS
BEGIN
	UPDATE Blast SET StatusCode = @Status , BlastEngineID = null
	WHERE BlastID = @BlastID
END
