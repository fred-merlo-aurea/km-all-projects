
CREATE PROCEDURE Usp_AggDailyBlastBounces

@InputDate DATE = NULL

AS

--SET NOCOUNT ON


------------------------------------
--Aggregate Bounces By Blast and Day--	
------------------------------------

DECLARE @Date Date
DECLARE @MaxBounceId INT
DECLARE @MinBounceId INT

DECLARE c1 CURSOR FOR

SELECT DISTINCT
	CONVERT(DATE,BounceTime) AS BounceDate
FROM 
	ECN_Activity.dbo.BlastActivityBounces bab WITH (NOLOCK) 
WHERE
	CONVERT(DATE,BounceTime) > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastBounceByDay)	
	AND DATEDIFF(DAY, BounceTime,GETDATE()) > 0 
	AND (CONVERT(DATE,BounceTime) = @InputDate OR @InputDate IS NULL)
ORDER BY
	BounceDate

OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
/*	SELECT 
		MIN (bounceid),
		MAX(bounceid) 
	FROM 
		ECN_ACTIVITY.dbo.blastactivitybounces 
	WHERE
		CONVERT(DATE,BounceTime) = @Date
*/

	INSERT INTO ECN5_Warehouse.dbo.BlastBounceByDay (
		BlastId,
		Date,
		BounceCode,
		TotalBounces,
		UniqueBounces
		)

	SELECT	
		bab.BlastID, 
		CONVERT(DATE,BounceTime),
		bc.BounceCode,
		COUNT(bab.BounceID), 
		COUNT(DISTINCT bab.EmailID)
	FROM 
		ECN_Activity.dbo.BlastActivityBounces bab WITH (NOLOCK) 
		JOIN ECN_Activity.dbo.BounceCodes bc WITH (NOLOCK) ON bab.BounceCodeID = bc.BounceCodeID 
		LEFT JOIN BlastBounceByDay bod WITH (NOLOCK) ON bab.BlastId = bod.BlastId AND CONVERT(Date,bab.BounceTime) = bod.Date
	WHERE
--		BounceID BETWEEN @MinBounceId AND @MaxBounceId
		CONVERT(DATE,BounceTime) = @Date
		AND bod.BlastId IS NULL

	GROUP BY 
		bab.BlastID, 
		CONVERT(DATE,BounceTime),
		bc.BounceCode
	ORDER BY 
		1,
		2

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1