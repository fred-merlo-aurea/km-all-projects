

Create Procedure Usp_AggBlastClickCounts

AS

SET NOCOUNT ON

 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/

CREATE TABLE #BlastIdsUpdated  (BlastId INT)
CREATE UNIQUE CLUSTERED INDEX Idx_TMP_blast on #BlastIdsUpdated(BlastId)
INSERT INTO #BlastIdsUpdated SELECT DISTINCT Blastid from ECN_Activity.dbo.BlastActivityClicks WHERE ClickId >= (SELECT MinClickId FROM  ECN5_Warehouse.dbo.BlastClickRangeByDate WHERE DATEDIFF(DAY,ClickDate,GETDATE()) =1)


MERGE INTO  ECN5_Warehouse.dbo.BlastClickCounts AS Target

USING (
	SELECT 
		bao.BlastID, 
		LEFT(URL,896) as URL,
		COUNT(bao.ClickID) TotalClicks,
		COUNT(DISTINCT bao.EmailID) UniqueClicks		
	FROM
		ECN_Activity.dbo.BlastActivityClicks bao WITH (NOLOCK) 
		INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bao.BlastID = b.BlastID
		INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
	GROUP BY 
		bao.BlastID,
		LEFT(URL,896)  ) AS Source (BlastId,URL, TotalClicks,UniqueClicks)
ON Target.BlastID = Source.BlastID AND Target.URL = Source.URL

WHEN MATCHED AND target.TotalClicks < Source.TotalClicks THEN
	UPDATE SET 
		TotalClicks = Source.TotalClicks,
		UniqueClicks = Source.UniqueClicks,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		URL,
		TotalClicks,
		UniqueClicks,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		URL,
		TotalClicks,
		UniqueClicks,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;