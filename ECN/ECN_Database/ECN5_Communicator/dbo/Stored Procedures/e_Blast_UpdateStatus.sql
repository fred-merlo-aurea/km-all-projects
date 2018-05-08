CREATE  PROC [dbo].[e_Blast_UpdateStatus] 
(
	@BlastID int,
	@Status varchar(50)
)
AS 
BEGIN
	UPDATE Blast SET StatusCode = @Status
	WHERE BlastID = @BlastID
END