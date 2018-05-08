CREATE  PROCEDURE [dbo].[spGetBlastBounceTypes] (@blastID int)
as
Begin
	SELECT DISTINCT bc.BounceCode
	FROM BlastActivityBounces bab join BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID 
	WHERE BlastID = @blastID
	GROUP BY bc.BounceCode
End
