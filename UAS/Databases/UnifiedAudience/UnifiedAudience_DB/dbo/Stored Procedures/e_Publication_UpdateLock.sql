CREATE PROCEDURE e_Publication_UpdateLock 
	@UserID int
AS
BEGIN

	SET NOCOUNT ON

	Update Pubs
	SET IsOpenCloseLocked = 0, DateUpdated = GETDATE()
	WHERE IsOpenCloseLocked = 1 AND UpdatedByUserID = @UserID

END