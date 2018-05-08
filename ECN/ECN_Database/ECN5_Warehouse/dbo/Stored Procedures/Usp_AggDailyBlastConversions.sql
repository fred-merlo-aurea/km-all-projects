
CREATE PROCEDURE Usp_AggDailyBlastConversions

@InputDate DATE = NULL

AS

--SET NOCOUNT ON

------------------------------------
--Aggregate Conversions By Blast and Day--	
------------------------------------

DECLARE @Date Date

DECLARE c1 CURSOR FOR

SELECT DISTINCT
	CONVERT(DATE,ConversionTime) AS ConversionDate
FROM 
	ECN_Activity.dbo.BlastActivityConversion bab WITH (NOLOCK) 
WHERE
	CONVERT(DATE,ConversionTime) > (SELECT MAX(Date) FROM ECN5_WareHouse.dbo.BlastConversionByDay)	
	AND DATEDIFF(DAY, ConversionTime,GETDATE()) > 0 
	AND (CONVERT(DATE,ConversionTime) = @InputDate OR @InputDate IS NULL)
ORDER BY
	ConversionDate


OPEN c1

	FETCH NEXT FROM c1
	INTO @Date

	WHILE @@FETCH_STATUS = 0
	BEGIN
	

	INSERT INTO ECN5_Warehouse.dbo.BlastConversionByDay (
		BlastId,
		Date,
		TotalConversion,
		UniqueConversion
		)

	SELECT 
		bau.BlastID, 
		CONVERT(Date,ConversionTime) ,
		COUNT(bau.ConversionID), 
		COUNT(DISTINCT bau.EmailID)
	
	FROM 
		ECN_Activity.dbo.BlastActivityConversion bau WITH (NOLOCK) 
		LEFT JOIN BlastConversionByDay bod WITH (NOLOCK) ON bau.BlastId = bod.BlastId AND CONVERT(Date,bau.ConversionTime) = bod.Date
	WHERE 
		CONVERT(DATE,ConversionTime) = @Date
		AND bod.BlastId IS NULL
	GROUP BY 
		bau.BlastID,
		CONVERT(Date,ConversionTime) 

	FETCH NEXT FROM c1
	INTO @Date 
	END

CLOSE c1
DEALLOCATE c1