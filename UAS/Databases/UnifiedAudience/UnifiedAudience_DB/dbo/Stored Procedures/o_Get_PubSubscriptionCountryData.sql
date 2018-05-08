CREATE PROCEDURE [dbo].[o_Get_PubSubscriptionCountryData]
	@Filters TEXT = '<XML><Filters></Filters></XML>',
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
		
			SELECT ISNULL(c.ShortName, 'No Response') as Country, SUM(ps.Copies) as Copies
			FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN UAD_Lookup..Country c ON c.CountryID = ps.CountryID
			GROUP BY ISNULL(c.ShortName, 'No Response')
		END
	ELSE --Query Archive
		BEGIN
			INSERT INTO #SubscriptionID   
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML  
			@Filters, @AdHocFilters, 0, 1, @IssueID
		
			SELECT ISNULL(c.ShortName, 'No Response') as Country, SUM(ps.Copies) as Copies
			FROM IssueArchiveProductSubscription ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN UAD_Lookup..Country c ON c.CountryID = ps.CountryID
			GROUP BY ISNULL(c.ShortName, 'No Response')
		END
END