CREATE  PROC [dbo].[e_BlastFields_Delete] 
(
	@CustomerID int,
    @BlastID int,
    @UserID int
)
AS 
BEGIN
	UPDATE bf SET bf.IsDeleted = 1, bf.UpdatedDate = GETDATE(), bf.UpdatedUserID = @UserID
	FROM BlastFields bf
		JOIN Blast b WITH (NOLOCK) ON bf.BlastID = b.BlastID
	WHERE b.CustomerID = @CustomerID AND bf.BlastID = @BlastID
END
