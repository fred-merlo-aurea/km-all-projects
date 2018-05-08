CREATE PROCEDURE [dbo].[rpt_SubFields]
(
	@Filters TEXT = '<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@Demo varchar(100),
	@IssueID int = 0
)
AS
BEGIN
	
	SET NOCOUNT ON
	
	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>1</ProductID></Filters></XML>',
	--@AdHocFilters varchar(max) = '<XML></XML>',
	--@Demo varchar(100) = 'DEMO7',
	--@IssueID int = 11
	
	IF 1=0 BEGIN
     SET FMTONLY OFF
	END
	
	CREATE TABLE #Subscriptions (PubSubscriptionID int)
	DECLARE @DeliverID int
	SET @DeliverID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	
	IF @IssueID = 0 --Query the Current Issue
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters
		
			IF @Demo = 'DEMO7'
			BEGIN
				SELECT ISNULL(c.DisplayName, 'No Response') as DisplayName, SUM(ps.Copies) as Copies, Count(*) as RecordCount, ISNULL(c.CodeId, 0) as 'DemoID' 
				FROM PubSubscriptions ps
					JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
				GROUP BY c.DisplayName, c.CodeId
				ORDER BY c.DisplayName DESC
			END
		END
	ELSE --Query the Archive
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters, 0, 1, @IssueID
		
			IF @Demo = 'DEMO7'
				BEGIN
					SELECT ISNULL(c.DisplayName, 'No Response') as DisplayName, SUM(ps.Copies) as Copies, Count(*) as RecordCount, ISNULL(c.CodeId, 0) as 'DemoID' 
					FROM IssueArchiveProductSubscription ps
						JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
						LEFT JOIN UAD_Lookup..Code c ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
					WHERE ps.IssueID = @IssueID
					GROUP BY c.DisplayName, c.CodeId
					ORDER BY c.DisplayName DESC
				END
		END
	DROP TABLE #Subscriptions

END