CREATE PROCEDURE o_Get_PubSubscriptionStateData
	@Filters TEXT = '<XML><Filters><ProductID>1</ProductID></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@IssueID int = 0
AS
BEGIN

	SET NOCOUNT ON

	IF 1=0 BEGIN
    SET FMTONLY OFF
	END
	
	CREATE TABLE #SubscriptionID (PubSubscriptionID int)  
		
	IF @IssueID = 0 --Query Current Issue
		BEGIN
			INSERT INTO #SubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML  
			@Filters, @AdHocFilters
		
			SELECT ps.RegionCode, SUM(ps.Copies) as Copies
			FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			GROUP BY ps.RegionCode
		END
	ELSE --Query Archive
		BEGIN
			INSERT INTO #SubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML  
			@Filters, @AdHocFilters, 0, 1, @IssueID
		
			SELECT ps.RegionCode, SUM(ps.Copies) as Copies
			FROM IssueArchiveProductSubscription ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			GROUP BY ps.RegionCode
		END
END