CREATE PROCEDURE [dbo].[e_IssueSplit_Clear_Issue]
@IssueID int
AS
BEGIN

	SET NOCOUNT ON
	
	--Delete FilterDetails
	--DELETE FROM IssueSplitFilterDetails 
 --   WHERE FilterId in ( SELECT FilterID From IssueSplit WHERE IssueId =@IssueID)
	
	----Delete Filter
 --   DELETE FROM IssueSplitFilter 
 --   WHERE FilterId in ( SELECT FilterID From IssueSplit WHERE IssueId =@IssueID)
    
 --   --Delete IssueSplitMapping
 --   DELETE FROM IssueSplitArchivePubSubscriptionMap 
 --   WHERE IssueSplitId in ( SELECT IssueSplitId From IssueSplit WHERE IssueId =@IssueID)
    
     --Delete IssueSplit
	DELETE FROM IssueSplit
	WHERE IssueId = @IssueID

END