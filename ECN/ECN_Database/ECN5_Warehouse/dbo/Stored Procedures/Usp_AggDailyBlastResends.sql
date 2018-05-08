
CREATE PROCEDURE Usp_AggDailyBlastResends

@InputDate DATE = NULL

AS

--SET NOCOUNT ON

------------------------------------
--Aggregate Resends By Blast and Day--	
------------------------------------

DECLARE @Date Date

DECLARE c1 CURSOR FOR

SELECT DISTINCT
	CONVERT(DATE,ResendTime) AS ResendDate
FROM 
	ECN_Activity.dbo.BlastActivityResends bab WITH (NOLOCK) 
WHERE
	CONVERT(DATE,ResendTime) > (SELECT ISNULL(MAX(Date),'2010-01-01') FROM ECN5_WareHouse.dbo.BlastResendByDay)	
	AND DATEDIFF(DAY, ResendTime,GETDATE()) > 0 
	AND (CONVERT(DATE,ResendTime) = @InputDate OR @InputDate IS NULL)
ORDER BY
	ResendDate


OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	INSERT INTO ECN5_Warehouse.dbo.BlastResendByDay (
		BlastId,
		Date,
		TotalResends,
		UniqueResends
		)

	SELECT 
		bau.BlastID, 
		CONVERT(Date,ResendTime) ,
		COUNT(bau.ResendID), 
		COUNT(DISTINCT bau.EmailID)
	
	FROM 
		ECN_Activity.dbo.BlastActivityResends bau WITH (NOLOCK) 
		LEFT JOIN BlastResendByDay bod WITH (NOLOCK) ON bau.BlastId = bod.BlastId AND CONVERT(Date,bau.ResendTime) = bod.Date
	WHERE 
		CONVERT(DATE,ResendTime) = @Date
		AND bod.BlastId IS NULL
	GROUP BY 
		bau.BlastID,
		CONVERT(Date,ResendTime) 

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1