CREATE  PROCEDURE [dbo].[MovedToActivity_sp_getBlastBounceTypes] (@blastID int)
as
Begin
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getBlastBounceTypes', GETDATE())
	SELECT DISTINCT ActionValue
	FROM EmailActivityLog 
	WHERE BlastID = @blastID AND ActionTypeCode = 'bounce' 
	GROUP BY ActionValue
End
