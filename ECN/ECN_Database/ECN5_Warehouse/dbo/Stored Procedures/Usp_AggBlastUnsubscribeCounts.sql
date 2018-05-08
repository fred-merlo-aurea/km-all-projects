

CREATE Procedure Usp_AggBlastUnsubscribeCounts

AS

SET NOCOUNT ON

 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/
 
CREATE TABLE #BlastIdsUpdated  (BlastId INT)
CREATE unique clustered index Idx_TMP_blast on #BlastIdsUpdated(BlastId)
INSERT INTO #BlastIdsUpdated SELECT DISTINCT Blastid from ECN_Activity.dbo.BlastActivityUnSubscribes WITH (NOLOCK) WHERE DATEDIFF(DAY,UnsubscribeTime,GETDATE()) <2

MERGE INTO  ECN5_Warehouse.dbo.BlastUnsubscribeCounts AS Target

USING (
	SELECT 
		bau.BlastID, 
		UnsubscribeCode,
		COUNT(bau.UnsubscribeId) TotalUnsubscribed,
		COUNT(DISTINCT bau.EmailID) UniqueUnsubscribed		
	FROM 
		ECN_Activity.dbo.BlastActivityUnSubscribes bau WITH (NOLOCK) 
		JOIN ECN_Activity.dbo.UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
		JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bau.BlastID = b.BlastID
		INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
	GROUP BY 
		bau.BlastID,
		UnsubscribeCode ) AS Source (BlastId, UnsubscribeCode,TotalUnsubscribed,UniqueUnsubscribed)
ON Target.BlastID = Source.BlastID AND Target.UnsubscribeCode = Source.UnsubscribeCode

WHEN MATCHED AND target.TotalUnsubscribed < Source.TotalUnsubscribed THEN
	UPDATE SET 
		TotalUnsubscribed = Source.TotalUnsubscribed,
		UniqueUnsubscribed = Source.UniqueUnsubscribed,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		UnsubscribeCode,
		TotalUnsubscribed,
		UniqueUnsubscribed,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		UnsubscribeCode,
		TotalUnsubscribed,
		UniqueUnsubscribed,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;