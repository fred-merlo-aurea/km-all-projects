CREATE PROCEDURE [rpt_IssueSplitSummary]
@IssueID int
AS
BEGIN
	
	SET NOCOUNT ON

	SELECT IssueSplitName, IssueSplitDescription, KeyCode, IssueSplitRecords, IssueSplitCount
	FROM IssueSplit
	WHERE IssueId = @IssueID AND FilterId = 0 AND IsActive = 0

END