CREATE PROCEDURE [dbo].[MovedToActivity_sp_getRevenueConversionData] (
	@BlastID int,
	@Type varchar (10)
)
AS
BEGIN
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_getRevenueConversionData', GETDATE())
	DECLARE @query varchar(8000)
	
	IF @Type = 'simple'
		BEGIN
			SELECT SUM(CONVERT(DECIMAL(6,2),ActionValue)) AS 'TOTALREVENUE'
			FROM EmailActivityLog
			WHERE ActionTypeCode='conversionRevenue'
			AND BlastID = @BlastID
		END 
	ELSE 
		BEGIN
			SELECT eal.EmailID, e.EmailAddress, COUNT(eal.EmailID) AS 'TOTALTRANSACTIONS', SUM(CONVERT(DECIMAL(6,2),eal.ActionValue)) AS 'TOTALREVENUE'
		  	FROM EmailActivityLog eal JOIN Emails e ON e.EmailID = eal.EmailID
			WHERE eal.ActionTypeCode='conversionRevenue'
			AND eal.BlastID =  @BlastID
			GROUP BY eal.EmailID, e.EmailAddress
		END
END
