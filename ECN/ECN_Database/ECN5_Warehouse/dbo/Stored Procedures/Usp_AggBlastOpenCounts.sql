
CREATE Procedure Usp_AggBlastOpenCounts

AS

SET NOCOUNT ON

 

 /*****************************************/
 /* Merge statement to maintain Agg table*/
 /***************************************/
 
 --DROP TABLE #BlastIdsUpdated
CREATE TABLE #BlastIdsUpdated  (BlastId INT)
CREATE UNIQUE CLUSTERED INDEX Idx_TMP_blast on #BlastIdsUpdated(BlastId)
INSERT INTO #BlastIdsUpdated SELECT Blastid from ECN_Activity.dbo.BlastActivityOpens WHERE OpenId >= (SELECT MinOpenId FROM  ECN5_Warehouse.dbo.BlastOpenRangeByDate WHERE DATEDIFF(DAY,OpenDate,GETDATE()) =1) GROUP BY BlastId


MERGE INTO  ECN5_Warehouse.dbo.BlastOpenCounts AS Target

USING (

SELECT a.BlastId,TotalOpens, UniqueOpens
FROM (
	SELECT 
		bao.BlastID,
		COUNT(bao.OpenID) TotalOpens
	FROM
		ECN_Activity.dbo.BlastActivityOpens bao WITH (NOLOCK) 
		INNER JOIN #BlastIdsUpdated bu  WITH (NOLOCK)  on bu.BlastId = bao.BlastId
	GROUP BY 
		bao.BlastID) A JOIN 

	(SELECT 
		bao.BlastID,
		COUNT(DISTINCT bao.EmailID) UniqueOpens		
	FROM
		ECN_Activity.dbo.BlastActivityOpens bao WITH (NOLOCK) 
		INNER JOIN #BlastIdsUpdated bu  WITH (NOLOCK) on bu.BlastId = bao.BlastId
	GROUP BY 
		bao.BlastID) B ON a.blastid = b.blastid
	 ) AS Source (BlastId, TotalOpens,UniqueOpens)
ON Target.BlastID = Source.BlastID

WHEN MATCHED AND target.TotalOpens < Source.TotalOpens THEN
	UPDATE SET 
		TotalOpens = Source.TotalOpens,
		UniqueOpens = Source.UniqueOpens,
		UpdatedDate = GETDATE()

WHEN NOT MATCHED BY TARGET THEN
	INSERT (
		BlastID, 
		TotalOpens,
		UniqueOpens,
		MobileOpens,
		MobileUniqueOpens,
		CreatedDate,
		UpdatedDate
		) 
	VALUES (
		BlastID, 
		TotalOpens,
		UniqueOpens,
		0,
		0,
		GETDATE(),
		GETDATE()
		)

OUTPUT
    $action AS dml_action,
    inserted.*;

SELECT
	bao.BlastId,
	MobileOpens = ISNULL(COUNT(bao.OpenID),0) ,
	MobileUniqueOpens = ISNULL(COUNT(DISTINCT bao.EmailID),0) 
INTO 
	#MobileOpens
FROM 
	ECN_Activity.dbo.BlastActivityOpens bao WITH(NOLOCK) 
	INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bao.BlastID = b.BlastID 
	INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
WHERE
	StatusCode = 'sent' 
	AND PlatformId = 2
GROUP BY
	bao.BlastId
		

UPDATE 
	boc 
SET 
	MobileOpens = mo.MobileOpens,
	MobileUniqueOpens = mo.MobileUniqueOpens,
	UpdatedDate = GETDATE()
FROM 
	ECN5_Warehouse.dbo.BlastOpenCounts  boc
	JOIN #MobileOpens mo ON boc.BlastId = mo.BlastId 
WHERE
	mo.MobileOpens > ISNULL(boc.MobileOpens,0)
	

  
/*
WITH MobileOpens (BlastId, MobileOpens,MobileUniqueOpens)
  AS
  ( 
	SELECT
		bao.BlastId,
		MobileOpens = ISNULL(COUNT(bao.OpenID),0) ,
		MobileUniqueOpens = ISNULL(COUNT(DISTINCT bao.EmailID),0) 
	FROM 
		ECN_Activity.dbo.BlastActivityOpens bao 
	 	INNER JOIN ECN5_COMMUNICATOR..Blast b WITH(NOLOCK) ON bao.BlastID = b.BlastID 
		INNER JOIN #BlastIdsUpdated bu on bu.BlastId = b.BlastId
	WHERE
		StatusCode = 'sent' 
		AND PlatformId = 2
	GROUP BY
		bao.BlastId
 )

UPDATE 
	boc 
SET 
	MobileOpens = cte.MobileOpens,
	MobileUniqueOpens = cte.MobileUniqueOpens,
	UpdatedDate = GETDATE()
FROM 
	ECN5_Warehouse.dbo.BlastOpenCounts  boc
	JOIN (SELECT * FROM MobileOpens)cte	ON boc.BlastId = cte.BlastId 
WHERE
	cte.MobileOpens >  boc.MobileOpens
*/