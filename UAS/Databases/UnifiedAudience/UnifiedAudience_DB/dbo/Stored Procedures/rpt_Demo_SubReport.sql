CREATE PROCEDURE [dbo].[rpt_Demo_SubReport]
@ProductID int,
@Row varchar(100),
@IncludeAddRemove bit,
@Filters TEXT = '<XML><Filters></Filters></XML>',
@AdHocFilters TEXT = '<XML></XML>',
@IssueID int
AS
BEGIN
	
	SET NOCOUNT ON

	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
   
	--DECLARE @ProductID int = 3
	--DECLARE @Row varchar(100) = 'BUSINESS'
	--DECLARE @IncludeAddRemove bit = 0
	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>3</ProductID><CategoryCode>101,102,103,115,116,117,118,119,120,121,122</CategoryCode><TransactionType>1,3</TransactionType></Filters></XML>'
	----SET @Filters = REPLACE(@Filters, '<ProductID>1</ProductID>', '<ProductID>1</ProductID><Responses>43_' + @CodeSheetID + '</Responses>')
	--DECLARE @Demo7 varchar(100) = 'Print'
	--DECLARE @AdHocFilters varchar(max) = '<XML></XML>'
	--DECLARE @IssueID int = 7

	CREATE TABLE #Subscriptions (PubSubscriptionID int PRIMARY KEY)

	DECLARE @RowID int = 0
	DECLARE @ResponseGroupRowID int = 0
	DECLARE @Groups varchar(max) = ''
	DECLARE @ResponsesRow varchar(max) = ''
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	DECLARE @ResponseFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Response Group')
	DECLARE @ProfileFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Profile Field')

	SELECT @Groups = STUFF( (SELECT ',' + '''' + ResponseGroupName + '''' FROM ResponseGroups WHERE PubID = @ProductID for XML PATH('')), 1,1,'')

	IF CHARINDEX(@Row, @Groups) > 0
		BEGIN
			SET @ResponseGroupRowID = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @Row AND PubID = @ProductID)
			SET @RowID = @ResponseFieldID
			SELECT Responsedesc, c.DisplayOrder
			INTO #TmpRow
			FROM CodeSheet c 
				JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID 
			WHERE rg.PubID = @ProductID AND rg.ResponseGroupName = @Row
		END
	ELSE
		BEGIN
			SET @RowID = @ProfileFieldID
		END

	DECLARE @executeString varchar(max) = ''

	IF @IssueID = 0 --Query Current Issue
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters, @IncludeAddRemove
			IF @RowID = @ResponseFieldID
				BEGIN
					Set @executeString =
					'SELECT DISTINCT ps.PubSubscriptionID, ps.Copies, c.CodeName, ISNULL(cs.DisplayOrder,1000) as DisplayOrder, ISNULL(cs.Responsedesc, ''No Response'') as ''Row'', 
					(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType
					FROM PubSubscriptions ps WITH (NOLOCK)
					JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
					JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeId = ' + convert(varchar(50),@DeliverID) + 
					'LEFT JOIN (
						SELECT psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID, rg.ResponseGroupID FROM PubSubscriptionDetail psd WITH (NOLOCK)
						JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID 
						JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID WHERE rg.ResponseGroupName = ''' + @Row + ''' AND rg.PubID = ' + convert(varchar(50),@ProductID) +
					') cs ON cs.PubSubscriptionID = s.PubSubscriptionID
					JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID) + ' and c.CodeTypeID = ' + CONVERT(varchar(50),@DeliverID)
				END
			ELSE
				BEGIN
					SET @executeString =
						'SELECT ps.PubSubscriptionID, ps.Copies, c.CodeName as Demo7, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @Row + ']), ''''), ''0''), ''No Response'') as ''Row'', 
							(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType,
							0 as RowDisplayOrder, 0 as ColDisplayOrder
						FROM PubSubscriptions ps 
						JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
						JOIN UAD_LookUp..Code c ON c.CodeValue = ps.Demo7
						JOIN UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
						JOIN UAD_LookUp..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
						WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID) +  ' and c.CodeTypeID = ' + CONVERT(varchar(50),@DeliverID)
				END
		END
	ELSE --Query Archive
		BEGIN
			INSERT INTO #Subscriptions
			EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @Filters, @AdHocFilters, @IncludeAddRemove, 1, @IssueID
			IF @RowID = @ResponseFieldID
				BEGIN
					Set @executeString =
					'SELECT DISTINCT ps.PubSubscriptionID, ps.Copies, c.CodeName, ISNULL(cs.DisplayOrder,1000) as DisplayOrder, ISNULL(cs.Responsedesc, ''No Response'') as ''Row'', 
					(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType
					FROM IssueArchiveProductSubscription ps WITH (NOLOCK)
					JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
					JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeId = ' + convert(varchar(50),@DeliverID) + 
					'LEFT JOIN (
						SELECT psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID, rg.ResponseGroupID, psd.IssueArchiveSubscriptionID 
						FROM  #Subscriptions s WITH (NOLOCK)
						JOIN IssueArchiveProductSubscription ias with (NOLOCK) on s.PubSubscriptionID = ias.PubsubscriptionID and IssueID = ' + convert(varchar(50), @IssueID) + '
						JOIN IssueArchiveProductSubscriptionDetail psd with (NOLOCK)  on psd.IssueArchiveSubscriptionId = ias.IssueArchiveSubscriptionId
						JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID 
						JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID WHERE rg.ResponseGroupName = ''' + @Row + ''' AND rg.PubID = ' + convert(varchar(50),@ProductID) +
					') cs ON cs.IssueArchiveSubscriptionID = ps.IssueArchiveSubscriptionID 
					JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID) + ' and c.CodeTypeID = ' + CONVERT(varchar(50),@DeliverID) + ' AND ps.IssueID = ' + CONVERT(varchar(50),@IssueID) 
				END
			ELSE
				BEGIN
					SET @executeString =
						'SELECT ps.PubSubscriptionID, ps.Copies, c.CodeName as Demo7, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @Row + ']), ''''), ''0''), ''No Response'') as ''Row'', 
							(CASE WHEN cct.CategoryCodeTypeName in (''Qualified Free'', ''Qualified Paid'') THEN ''Qualified'' ELSE ''NonQualified'' END) as CategoryType,
							0 as RowDisplayOrder, 0 as ColDisplayOrder
						FROM IssueArchiveProductSubscription ps 
						JOIN #Subscriptions s ON s.PubSubscriptionID = ps.PubSubscriptionID
						JOIN UAD_LookUp..Code c ON c.CodeValue = ps.Demo7
						JOIN UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = ps.PubCategoryID
						JOIN UAD_LookUp..CategoryCodeType cct ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
						WHERE ps.PubID = ' + CONVERT(varchar(50),@ProductID) +  ' and c.CodeTypeID = ' + CONVERT(varchar(50),@DeliverID)
				END
		END

	IF @ProductID > 0
		BEGIN
			exec(@executeString)	
			--print @executeString	
		END

	IF OBJECT_ID('tempdb..#TmpRow') IS NOT NULL DROP TABLE #TmpRow
	IF OBJECT_ID('tempdb..#TmpCol') IS NOT NULL  DROP TABLE #TmpCol
	IF OBJECT_ID('tempdb..#Subscriptions') IS NOT NULL  DROP TABLE #Subscriptions

END