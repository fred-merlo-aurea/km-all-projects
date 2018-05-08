CREATE PROCEDURE [dbo].[spUpdateBounceScore] 
(
	@EmailID INT,
	@EAID INT
)
AS
BEGIN
	SET NOCOUNT ON

	BEGIN TRAN
		UPDATE Emails SET BounceScore = 0 WHERE EmailID = @EmailID
		IF (@@ERROR <> 0) GOTO ERR_HANDLER
		UPDATE EmailActivityLog SET Processed = 'Y' WHERE EAID = @EAID
		IF (@@ERROR <> 0) GOTO ERR_HANDLER
	COMMIT TRAN
	--print 'success'
	RETURN 0
	
	ERR_HANDLER:
	ROLLBACK TRAN
	--print 'error'
	RETURN -1
END
