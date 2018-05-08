--------------------------------------
-- 2014-07-22 MTK, Added URL
--
--
--------------------------------------


CREATE PROCEDURE Usp_AggDailyBlastClicks

@InputDate DATE = NULL

AS

--SET NOCOUNT ON


------------------------------------
--Aggregate Clicks By Blast and Day--	
------------------------------------

DECLARE @Date Date
DECLARE @MaxClickId INT
DECLARE @MinClickId INT

DECLARE c1 CURSOR FOR

SELECT 
	ClickDate 
FROM 
	ECN5_Warehouse.dbo.BlastClickRangeByDate
WHERE
	ClickDate > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastClickByDay)	
	AND (ClickDate = @InputDate OR @InputDate IS NULL)
ORDER BY
	ClickDate

OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN


	SELECT
		@MaxClickId =MaxClickId ,
		@MinClickId =MinClickId 
	FROM 
		ECN5_Warehouse.dbo.BlastClickRangeByDate 
	WHERE
		ClickDate = @Date
	--	AND ClickDate > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastClickByDay)

	INSERT INTO ECN5_Warehouse.dbo.BlastClickByDay (
		BlastId,
		URL,
		Date,
		TotalClicks,
		UniqueClicks
		)

	SELECT 
		bao.BlastID, 
		bao.URL,
		CONVERT(Date,ClickTime) as Date,
		COUNT(ClickID) TotalClicks,
		COUNT(DISTINCT bao.EmailID) UniqueClicks
	FROM 
		ECN_Activity.dbo.BlastActivityClicks bao WITH (NOLOCK) 
		LEFT JOIN BlastClickByDay bod WITH (NOLOCK) ON bao.BlastId = bod.BlastId AND CONVERT(Date,bao.ClickTime) = bod.Date
	WHERE
		ClickID BETWEEN @MinClickId AND @MaxClickId
		AND bod.BlastId IS NULL
	GROUP BY 
		bao.BlastID  ,	
		bao.URL,
		CONVERT(Date,ClickTime) 
	ORDER BY 
		1,
		2

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1
