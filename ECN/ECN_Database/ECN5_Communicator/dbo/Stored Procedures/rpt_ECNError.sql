CREATE PROCEDURE [dbo].[rpt_ECNError] (@from datetime, @to datetime)
AS
BEGIN	
	set @from = @from + ' 00:00:00 '    
	set @to = @to + '  23:59:59'       
	
	SELECT COUNT(*) AS 'ErrCounts'
	FROM ecn_misc..ECNTasks 
	WHERE DateAdded between @from and @to
	GROUP BY  YEAR(DateAdded), MONTH(DateAdded) 
END
