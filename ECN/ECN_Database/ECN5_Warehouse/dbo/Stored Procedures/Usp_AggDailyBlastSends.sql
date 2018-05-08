
CREATE PROCEDURE Usp_AggDailyBlastSends

@InputDate DATE = NULL

AS

--SET NOCOUNT ON


------------------------------------
--Aggregate Sends By Blast and Day--	
------------------------------------

DECLARE @Date Date
DECLARE @MaxSendId INT
DECLARE @MinSendId INT

DECLARE c1 CURSOR FOR

SELECT 
	SendDate 
FROM 
	ECN5_Warehouse.dbo.BlastSendRangeByDate
WHERE
	SendDate > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastSendByDay)	
	AND (SendDate = @InputDate OR @InputDate IS NULL)
ORDER BY
	SendDate

OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	
	SELECT
		@MaxSendId =MaxSendId ,
		@MinSendId =MinSendId 
	FROM 
		ECN5_Warehouse.dbo.BlastSendRangeByDate 
	WHERE
		SendDate = @Date

	INSERT INTO ECN5_Warehouse.dbo.BlastSendByDay (
		BlastId,
		Date,
		TotalSends,
		UniqueSends
		)

	SELECT 
		bao.BlastID, 
		CONVERT(Date,SendTime) as Date,
		COUNT(SendID) TotalSends,
		COUNT(DISTINCT bao.EmailID) UniqueSends
	FROM 
		ECN_Activity.dbo.BlastActivitySends bao WITH (NOLOCK) 
		LEFT JOIN BlastSendByDay bod WITH (NOLOCK) ON bao.BlastId = bod.BlastId AND CONVERT(Date,bao.SendTime) = bod.Date
	WHERE
		SendID BETWEEN @MinSendId AND @MaxSendId
	--	AND bod.BlastId IS NULL
	GROUP BY 
		bao.BlastID  ,	
		CONVERT(Date,SendTime) 
	ORDER BY 
		1,
		2

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1