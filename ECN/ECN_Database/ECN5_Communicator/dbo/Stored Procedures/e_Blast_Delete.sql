CREATE  PROC [dbo].[e_Blast_Delete] 
(
	@BlastID int,
	@CustomerID int,
    @UserID int
)
AS 
BEGIN
	UPDATE Blast SET StatusCode = 'Deleted', UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CustomerID = @CustomerID AND BlastID = @BlastID
END
