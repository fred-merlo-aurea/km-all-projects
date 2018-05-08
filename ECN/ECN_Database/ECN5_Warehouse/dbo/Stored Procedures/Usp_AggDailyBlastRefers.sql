
CREATE PROCEDURE Usp_AggDailyBlastRefers

@InputDate DATE = NULL

AS

--SET NOCOUNT ON

------------------------------------
--Aggregate Refers By Blast and Day--	
------------------------------------

DECLARE @Date Date

DECLARE c1 CURSOR FOR

SELECT DISTINCT
	CONVERT(DATE,ReferTime) AS ReferDate
FROM 
	ECN_Activity.dbo.BlastActivityRefer bab WITH (NOLOCK) 
WHERE
	CONVERT(DATE,ReferTime) > (SELECT ISNULL(MAX(Date),'2010-01-01') FROM ECN5_WareHouse.dbo.BlastRefersByDay)	
	AND DATEDIFF(DAY, ReferTime,GETDATE()) > 0 
	AND (CONVERT(DATE,ReferTime) = @InputDate OR @InputDate IS NULL)
ORDER BY
	ReferDate


OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	INSERT INTO ECN5_Warehouse.dbo.BlastRefersByDay (
		BlastId,
		Date,
		TotalRefers,
		UniqueRefers
		)

	SELECT 
		bau.BlastID, 
		CONVERT(Date,ReferTime) ,
		COUNT(bau.ReferID), 
		COUNT(DISTINCT bau.EmailID)
	
	FROM 
		ECN_Activity.dbo.BlastActivityRefer bau WITH (NOLOCK) 
		LEFT JOIN BlastRefersByDay bod WITH (NOLOCK) ON bau.BlastId = bod.BlastId AND CONVERT(Date,bau.ReferTime) = bod.Date
	WHERE 
		CONVERT(DATE,ReferTime) = @Date
		AND bod.BlastId IS NULL
	GROUP BY 
		bau.BlastID,
		CONVERT(Date,ReferTime) 

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1