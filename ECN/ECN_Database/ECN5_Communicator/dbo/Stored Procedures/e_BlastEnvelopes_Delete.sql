create  PROC [dbo].[e_BlastEnvelopes_Delete] 
(
	@CustomerID int,
    @BlastEnvelopeID int,
    @UserID int
)
AS 
BEGIN
	UPDATE BlastEnvelopes SET IsDeleted = 1, UpdatedDate = GETDATE(), UpdatedUserID = @UserID
	WHERE CustomerID = @CustomerID AND BlastEnvelopeID = @BlastEnvelopeID
END