	
CREATE PROC Usp_AggDailyBlastSuppress

AS

TRUNCATE TABLE 	ECN5_Warehouse.dbo.BlastSuppress

INSERT INTO ECN5_Warehouse.dbo.BlastSuppress

SELECT 
	basu.BlastID, 
	COUNT(basu.SuppressID), 
	COUNT(DISTINCT basu.EmailID)
FROM 
	ECN_Activity.dbo.BlastActivitySuppressed basu WITH (NOLOCK) 
GROUP BY 
	basu.BlastID


INSERT INTO ECN5_Warehouse.dbo.BlastSuppress
		
SELECT 
	b.BlastID, 
	0,
	0
FROM 
	ecn5_communicator.dbo.Blast	b WITH (NOLOCK)
WHERE 
	StatusCode = 'sent' 
	and TestBlast='N'
	and b.BlastID NOT IN (Select BlastId from ECN5_Warehouse.dbo.BlastSuppress)
	and b.BlastID NOT IN (Select BlastId from ECN_ACTIVITY.dbo.BlastActivitySuppressed)
	AND DATEDIFF(DAY,SendTime,GETDATE()) > 0