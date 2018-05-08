CREATE PROCEDURE [dbo].[spGetRevenueConversionData] (
	@BlastID int,
	@Type varchar (10)
)
AS
BEGIN
	DECLARE @query varchar(8000)
	
	IF @Type = 'simple'
		BEGIN
			SELECT 0 AS 'TOTALREVENUE'  -- need to add column to calculate revenue
			FROM BlastActivityConversion WHERE BlastID = @BlastID
		END 
	ELSE 
		BEGIN
			SELECT e.EmailID, e.EmailAddress, COUNT(e.EmailID) AS 'TOTALTRANSACTIONS', 0 AS 'TOTALREVENUE'
		  	FROM BlastActivityConversion bac with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON e.EmailID = bac.EmailID 			
			AND bac.BlastID =  @BlastID
			GROUP BY e.EmailID, e.EmailAddress
		END
END
