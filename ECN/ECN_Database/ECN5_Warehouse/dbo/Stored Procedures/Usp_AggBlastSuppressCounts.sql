

CREATE Procedure [dbo].[Usp_AggBlastSuppressCounts]

AS

SET NOCOUNT ON

 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/

 
CREATE TABLE #BlastIdsUpdated  (BlastId INT)
CREATE unique clustered index Idx_TMP_blast on #BlastIdsUpdated(BlastId)
INSERT INTO #BlastIdsUpdated SELECT DISTINCT Blastid from ECN_Activity.dbo.BlastActivitySuppressed WITH (NOLOCK) WHERE DATEDIFF(DAY,SuppressedTime,GETDATE()) =1


MERGE INTO  ECN5_Warehouse.dbo.BlastSuppressCounts AS Target

USING (
	SELECT 
		bas.BlastID, 
		SupressedCode, --<sic>
		COUNT(bas.SuppressId) TotalSuppressed,
		COUNT(DISTINCT bas.EmailID) UniqueSuppressed		
	FROM
		ECN_Activity.dbo.BlastActivitySuppressed bas WITH (NOLOCK) 
		INNER JOIN ECN_Activity.dbo.SuppressedCodes sc WITH (NOLOCK) ON bas.SuppressedCodeId = sc.SuppressedCodeId
		INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bas.BlastID = b.BlastID
		INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
	GROUP BY 
		bas.BlastID ,
		SupressedCode --<sic> 
		) AS Source (BlastId, SuppressedCode,TotalSuppressed,UniqueSuppressed)
ON Target.BlastID = Source.BlastID AND Target.SuppressedCode = Source.SuppressedCode

WHEN MATCHED AND target.TotalSuppressed < Source.TotalSuppressed THEN
	UPDATE SET 
		TotalSuppressed = Source.TotalSuppressed,
		UniqueSuppressed = Source.UniqueSuppressed,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		SuppressedCode,
		TotalSuppressed,
		UniqueSuppressed,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		SuppressedCode,
		TotalSuppressed,
		UniqueSuppressed,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;