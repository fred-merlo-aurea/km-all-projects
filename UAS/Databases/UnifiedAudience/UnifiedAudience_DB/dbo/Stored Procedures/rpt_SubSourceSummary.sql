CREATE PROCEDURE [dbo].[rpt_SubSourceSummary]
	@Filters TEXT = '<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@IssueID int = 0,
	@IncludeAddRemove bit = 0
AS
BEGIN	
	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>',
	--@AdHocFilters varchar(max) = '<XML></XML>',
	--@UseArchive bit = 1, @IssueID int = 11
	
	IF 1=0 BEGIN
     SET FMTONLY OFF
	END
	
	SET NOCOUNT ON
	
	CREATE TABLE #SubscriptionID (PubSubscriptionID int)  
		
	IF @IssueID = 0
		BEGIN
			INSERT INTO #SubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML
			@Filters, @AdHocFilters, @IncludeAddRemove
		
			SELECT SubscriberSourceCode as 'SUBSRC', SUM(Copies) as Copies, 1 as Sort
			FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			WHERE SubscriberSourceCode <> '' AND SubscriberSourceCode is not null
			GROUP BY SubscriberSourceCode
			UNION 
				SELECT ISNULL(NULLIF(SubscriberSourceCode, ''), 'No Response') as 'SUBSRC', SUM(Copies) as Copies, 2 as Sort
				FROM PubSubscriptions ps
					JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				WHERE SubscriberSourceCode = '' OR SubscriberSourceCode is null
				GROUP BY ISNULL(NULLIF(SubscriberSourceCode, ''), 'No Response')
			ORDER BY Sort ASC, SUBSRC ASC
		END
	ELSE
		BEGIN
			INSERT INTO #SubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML
			@Filters, @AdHocFilters, @IncludeAddRemove, 1, @IssueID
		
			SELECT SubscriberSourceCode as 'SUBSRC', SUM(Copies) as Copies, 1 as Sort
			FROM IssueArchiveProductSubscription ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			WHERE SubscriberSourceCode <> '' AND SubscriberSourceCode is not null
				AND ps.IssueID = @IssueID
			GROUP BY SubscriberSourceCode
			UNION 
				SELECT ISNULL(NULLIF(SubscriberSourceCode, ''), 'No Response') as 'SUBSRC', SUM(Copies) as Copies, 2 as Sort
				FROM IssueArchiveProductSubscription ps
					JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				WHERE (SubscriberSourceCode = '' OR SubscriberSourceCode is null) AND IssueID = @IssueID
				GROUP BY ISNULL(NULLIF(SubscriberSourceCode, ''), 'No Response')
			ORDER BY Sort ASC, SUBSRC ASC
		END
	
	DROP TABLE #SubscriptionID
END
