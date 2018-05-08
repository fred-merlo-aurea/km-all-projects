Create Procedure Usp_AggBlastSendCounts

AS

--SET NOCOUNT ON


 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/

-- Should not be needed for sends, as we only send on 1 day.
--CREATE TABLE #BlastIdsUpdated  (BlastId INT)
--CREATE UNIQUE CLUSTERED INDEX Idx_TMP_blast on #BlastIdsUpdated(BlastId)
--INSERT INTO #BlastIdsUpdated SELECT DISTINCT Blastid from ECN_Activity.dbo.BlastActivitySends WHERE SendId >= (SELECT MinSendId FROM  ECN5_Warehouse.dbo.BlastSendRangeByDate WHERE DATEDIFF(DAY,SendDate,GETDATE()) =1)


INSERT INTO  ECN5_Warehouse.dbo.BlastSendCounts 
(
	BlastID, 
	TotalSends,
	UniqueSends,
	CreatedDate,
	UpdatedDate
	) 
SELECT 
	b.BlastID, 
	SendTotal,
	COUNT(DISTINCT bao.EmailID) UniqueSends		,
	GETDATE(),
	GETDATE()
FROM
	ECN_Activity.dbo.BlastActivitySends bao WITH (NOLOCK) 
	INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bao.BlastID = b.BlastID
	LEFT JOIN ECN5_Warehouse.dbo.BlastSendCounts agg on b.BlastID = agg.BlastID
WHERE
	StatusCode = 'sent' 
	AND DATEDIFF(DAY,b.SendTime,GETDATE()) IN (1,0)
	AND Agg.BlastId is null
GROUP BY 
	b.BlastID,
	SendTotal


/*

MERGE INTO  ECN5_Warehouse.dbo.BlastSendCounts AS Target

USING (
	SELECT 
		bao.BlastID, 
		--COUNT(bao.SendID) TotalSends,
		COUNT(DISTINCT bao.EmailID) TotalSends,	
		COUNT(DISTINCT bao.EmailID) UniqueSends		
	FROM
		ECN_Activity.dbo.BlastActivitySends bao WITH (NOLOCK) 
		INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bao.BlastID = b.BlastID
		--INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
		AND DATEDIFF(DAY,b.SendTime,GETDATE()) = 0
	GROUP BY 
		bao.BlastID ) AS Source (BlastId, TotalSends,UniqueSends)
ON Target.BlastID = Source.BlastID

WHEN MATCHED AND target.TotalSends < Source.TotalSends THEN
	UPDATE SET 
		TotalSends = Source.TotalSends,
		UniqueSends = Source.UniqueSends,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		TotalSends,
		UniqueSends,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		TotalSends,
		UniqueSends,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;
    
GO

*/