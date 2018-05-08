CREATE PROCEDURE [dbo].[rpt_ECNError] (@from datetime, @to datetime)
AS
BEGIN	
	SELECT CONVERT(VARCHAR, MONTH(DateAdded)) +'/'+ CONVERT(VARCHAR, YEAR(DateAdded)) AS 'Month/Year',COUNT(*) AS 'ErrCounts'
	FROM ecn_misc..ECNTasks 
	WHERE DateAdded between @from and @to
	GROUP BY  YEAR(DateAdded), MONTH(DateAdded) 
END
