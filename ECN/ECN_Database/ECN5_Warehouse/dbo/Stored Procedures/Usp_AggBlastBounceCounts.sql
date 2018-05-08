

CREATE Procedure Usp_AggBlastBounceCounts

AS

SET NOCOUNT ON

 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/
 
CREATE TABLE #BlastIdsUpdated  (BlastId INT)
CREATE unique clustered index Idx_TMP_blast on #BlastIdsUpdated(BlastId)
INSERT INTO #BlastIdsUpdated SELECT DISTINCT Blastid from ECN_Activity.dbo.BlastActivityBounces WITH (NOLOCK) WHERE DATEDIFF(DAY,BounceTime,GETDATE()) =1


MERGE INTO  ECN5_Warehouse.dbo.BlastBounceCounts AS Target

USING (
	SELECT 
		bab.BlastID, 
		BounceCode,
		COUNT(bab.BounceID) TotalBounces,
		COUNT(DISTINCT bab.EmailID) UniqueBounces		
	FROM 
		ECN_Activity.dbo.BlastActivityBounces bab WITH (NOLOCK) 
		JOIN ECN_Activity.dbo.BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID 
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bab.BlastID = b.BlastID
		INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
	GROUP BY 
		bab.BlastID,
		BounceCode ) AS Source (BlastId, BounceCode,TotalBounces,UniqueBounces)
ON Target.BlastID = Source.BlastID AND Target.BounceCode = Source.BounceCode

WHEN MATCHED AND target.TotalBounces < Source.TotalBounces THEN
	UPDATE SET 
		TotalBounces = Source.TotalBounces,
		UniqueBounces = Source.UniqueBounces,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		BounceCode,
		TotalBounces,
		UniqueBounces,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		BounceCode,
		TotalBounces,
		UniqueBounces,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;