

CREATE PROCEDURE Usp_AggDailyBlastMobileOpens

@InputDate DATE = NULL

AS

--SET NOCOUNT ON


------------------------------------
--Aggregate Opens By Blast and Day--	
------------------------------------

DECLARE @Date Date
DECLARE @MaxOpenId INT
DECLARE @MinOpenId INT

DECLARE c1 CURSOR FOR

SELECT 
	OpenDate 
FROM 
	ECN5_Warehouse.dbo.BlastOpenRangeByDate
WHERE
	OpenDate > (SELECT ISNULL(MAX(Date),'2010-01-01') FROM ECN5_WareHouse.dbo.BlastMobileOpenByDay)	
	AND (OpenDate = @InputDate OR @InputDate IS NULL)
ORDER BY
	OpenDate

OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN


	SELECT
		@MaxOpenId =MaxOpenId ,
		@MinOpenId =MinOpenId 
	FROM 
		ECN5_Warehouse.dbo.BlastOpenRangeByDate 
	WHERE
		OpenDate = @Date
	--	AND OpenDate > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastOpenByDay)

	INSERT INTO ECN5_Warehouse.dbo.BlastMobileOpenByDay (
		BlastId,
		Date,
		TotalOpens,
		UniqueOpens
		)

	SELECT 
		bao.BlastID, 
		CONVERT(Date,OpenTime) as Date,
		COUNT(OpenID) TotalOpens,
		COUNT(DISTINCT bao.EmailID) UniqueOpens
	FROM 
		ECN_Activity.dbo.BlastActivityOpens bao WITH (NOLOCK) 
		LEFT JOIN BlastMobileOpenByDay bod WITH (NOLOCK) ON bao.BlastId = bod.BlastId AND CONVERT(Date,bao.OpenTime) = bod.Date
	WHERE
		OpenID BETWEEN @MinOpenId AND @MaxOpenId
		AND bod.BlastId IS NULL
		AND bao.PlatformID = 2
	GROUP BY 
		bao.BlastID  ,	
		CONVERT(Date,OpenTime) 
	ORDER BY 
		1,
		2

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1