CREATE PROCEDURE [dbo].[GetBlastInfo]
	@BlastID int
AS
BEGIN
	SELECT b.BlastID, b.SendTime,
	CASE
		WHEN bp.Period = 1 THEN 'Daily' 	 
		WHEN bp.Period = 7 or bp.period = 14 THEN 'Weekly'
		WHEN bp.Period = 0 THEN 'Monthly' 
	END AS 'BlastType', 
	YEAR(sendtime) AS 'BlastYear',    
	MONTH(sendtime) AS 'BlastMonth', 
	DAY(sendtime) AS 'BlastDay', 
	bp.Period FROM [BLAST] b WITH (NOLOCK) join BlastPlans bp WITH (NOLOCK) ON b.BlastID = bp.BlastID 
	WHERE b.BlastID = @BlastID 	
END