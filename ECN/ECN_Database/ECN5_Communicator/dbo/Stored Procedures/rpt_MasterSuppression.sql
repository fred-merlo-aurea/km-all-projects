CREATE PROCEDURE [dbo].[rpt_MasterSuppression]
	@GroupID int, 	
	@From datetime, 
	@To datetime
AS
BEGIN
	SET @from = @from + ' 00:00:00 '    
	SET @to = @to + '  23:59:59' 
	
	SELECT e.EmailID, e.EmailAddress, CONVERT(VARCHAR(19), eg.CreatedOn, 120) as 'CreatedOn' FROM Emails e JOIN EmailGroups eg ON e.EmailID = eg.EmailID 
	WHERE eg.groupID = @GroupID AND eg.CreatedOn between @From and @To
	ORDER BY CreatedOn
END
