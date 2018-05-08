CREATE PROCEDURE [dbo].[spGetBlastInfo]
	@BlastID int
AS
BEGIN
	SELECT b.BlastID, b.SendTime,
	CASE
		WHEN bp.Period IS NULL THEN 'Once' 
		WHEN bp.Period = 1 THEN 'Daily' 
		WHEN bp.Period = 0 THEN 'Monthly'	 
		WHEN CONVERT(int,bp.Period) % 7 = 0 THEN 'Weekly' 		 
	END AS 'BlastType', 
	YEAR(sendtime) AS 'BlastYear',    
	MONTH(sendtime) AS 'BlastMonth', 
	DAY(sendtime) AS 'BlastDay', 
	 case when DATEPART(HOUR,sendtime) > 12 then (DATEPART(HOUR,sendtime) - 12) else DATEPART(HOUR,sendtime)  end as 'BlastHour',  
    DATEPART(MINUTE,sendtime) as 'BlastMinute', 
    DATEPART(SECOND,sendtime) as 'BlastSecond',  
    SUBSTRING(CONVERT(varchar(20), sendtime, 22), 18, 3) as 'AMPM',
    DATEPART(W, sendTime) as 'DayOfWeek',  
	bp.Period FROM [BLAST] b left join BlastPlans bp ON b.BlastID = bp.BlastID
	WHERE b.BlastID = @BlastID 	
END
