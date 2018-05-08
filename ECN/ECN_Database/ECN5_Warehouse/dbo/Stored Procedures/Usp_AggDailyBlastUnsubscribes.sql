
CREATE PROCEDURE Usp_AggDailyBlastUnsubscribes

@InputDate DATE = NULL

AS

--SET NOCOUNT ON

------------------------------------
--Aggregate Unsubscribes By Blast and Day--	
------------------------------------

DECLARE @Date Date
DECLARE @MaxUnsubscribeId INT
DECLARE @MinUnsubscribeId INT

DECLARE c1 CURSOR FOR

SELECT DISTINCT
	CONVERT(DATE,UnsubscribeTime) AS UnsubscribeDate
FROM 
	ECN_Activity.dbo.BlastActivityUnsubscribes bab WITH (NOLOCK) 
WHERE
	CONVERT(DATE,UnsubscribeTime) > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastUnsubscribeByDay)	
	AND DATEDIFF(DAY, UnsubscribeTime,GETDATE()) > 0 
	AND (CONVERT(DATE,UnsubscribeTime) = @InputDate OR @InputDate IS NULL)
ORDER BY
	UnsubscribeDate


OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	INSERT INTO ECN5_Warehouse.dbo.BlastUnsubscribeByDay (
		BlastId,
		Date,
		UnsubscribeCode,
		TotalUnsubscribe,
		UniqueUnsubscribe
		)

	SELECT 
		bau.BlastID, 
		CONVERT(Date,UnsubscribeTime) ,
		UnsubscribeCode,
		COUNT(bau.UnsubscribeID), 
		COUNT(DISTINCT bau.EmailID)
	
	FROM 
		ECN_Activity.dbo.BlastActivityUnSubscribes bau WITH (NOLOCK) 
		JOIN ECN_Activity.dbo.UnsubscribeCodes uc WITH (NOLOCK) on bau.UnsubscribeCodeID = uc.UnsubscribeCodeID 
		LEFT JOIN BlastBounceByDay bod WITH (NOLOCK) ON bau.BlastId = bod.BlastId AND CONVERT(Date,bau.UnsubscribeTime) = bod.Date
	WHERE 
		CONVERT(DATE,UnsubscribeTime) = @Date
		AND bod.BlastId IS NULL
	GROUP BY 
		bau.BlastID,
		UnsubscribeCode,
		CONVERT(Date,UnsubscribeTime) 


	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1