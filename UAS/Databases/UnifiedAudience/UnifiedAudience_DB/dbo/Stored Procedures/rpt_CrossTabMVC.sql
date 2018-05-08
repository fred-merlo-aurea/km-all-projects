CREATE PROCEDURE [dbo].[rpt_CrossTabMVC]
@Queries VARCHAR(MAX),  
@ProductID int,
@Row varchar(100),
@Col varchar(100),
@IncludeAddRemove bit,
@IssueID INT = 0,
@IncludeReportGroup BIT = 0
AS
BEGIN
	
	SET NOCOUNT ON

	IF 1=0 
	BEGIN
		SET FMTONLY OFF
	END
   
	--DECLARE @ProductID int = 6
	--DECLARE @IssueID int = 0
	--DECLARE @Row varchar(100) = 'BUSINESS'
	--DECLARE @Col varchar(100) = 'FUNCTION'
	--DECLARE @IncludeAddRemove bit = 0
	--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>6</ProductID><CategoryCode>101,102,103,115,116,117,118,119,120,121,122</CategoryCode><TransactionType>1,3</TransactionType></Filters></XML>'
	--DECLARE @AdHocFilters varchar(max) = '<XML></XML>'
	--DECLARE @IncludeReportGroup bit = 0

	CREATE TABLE #Tmp1 (PubSubscriptionID int, Copies int, Demo7 varchar(20), Row varchar(500), DisplayOrder int, CodeSheetID varchar(20), DemoID int, GroupDisplay varchar(250),
						GroupDisplayOrder int, ResponseGroupID int)

	create nonclustered index IX_tmp1__PubSubscriptionID on #Tmp1 (PubSubscriptionID);
										
	CREATE TABLE #Tmp2 (PubSubscriptionID int, Copies int, Demo7 varchar(20), Col varchar(500), DisplayOrder int, CodeSheetID varchar(20), DemoID int, GroupDisplay varchar(250),
						GroupDisplayOrder int, ResponseGroupID int)
					
	create nonclustered index IX_tmp2__PubSubscriptionID on #Tmp1 (PubSubscriptionID);

	CREATE TABLE #Subscriptions (PubSubscriptionID int PRIMARY KEY)
	INSERT INTO #Subscriptions  
	EXEC (@Queries) 

	DECLARE @RowID int = 0
	DECLARE @ColID int = 0
	DECLARE @ResponseGroupRowID int = 0
	DECLARE @ResponseGroupColID int = 0
	DECLARE @Groups varchar(max) = ''
	DECLARE @ResponsesRow varchar(max) = ''
	DECLARE @ResponsesCol varchar(max) = ''
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	DECLARE @ResponseFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Response Group')
	DECLARE @ProfileFieldID int = (SELECT CodeId FROM UAD_Lookup..Code c JOIN UAD_Lookup..CodeType ct ON c.CodeTypeId = ct.CodeTypeId WHERE ct.CodeTypeName = 'Dimension' AND c.CodeName = 'Profile Field')
	DECLARE @MaxDisplayOrder int = (SELECT ISNULL(MAX(DisplayOrder) + 1,0) FROM ReportGroups)

	SELECT @Groups = STUFF( (SELECT ',' + '''' + ResponseGroupName + '''' FROM ResponseGroups WHERE PubID = @ProductID for XML PATH('')), 1,1,'')

	IF CHARINDEX(@Row, @Groups) > 0
	BEGIN
		SET @ResponseGroupRowID = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @Row AND PubID = @ProductID)
		SET @RowID = @ResponseFieldID
			
		SELECT c.Responsevalue + '. ' + c.Responsedesc as 'Responsedesc', c.DisplayOrder, rpts.DisplayName, rpts.DisplayOrder as 'GroupDisplayOrder'
		INTO #TmpRow
		FROM CodeSheet c 
			JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID 
			LEFT JOIN ReportGroups rpts ON rpts.ReportGroupID = c.ReportGroupID 
		WHERE rg.PubID = @ProductID AND rg.DisplayName = @Row
			
		INSERT INTO #TmpRow
		SELECT 'ZZ. No Response', 1000, '', 0
	END
	ELSE
	BEGIN
		SET @RowID = @ProfileFieldID
	END

	IF CHARINDEX(@Col, @Groups) > 0
	BEGIN
		SET @ResponseGroupColID = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @Col AND PubID = @ProductID)
		SET @ColID = @ResponseFieldID
			
		SELECT c.Responsevalue + '. ' + c.Responsedesc as 'Responsedesc', c.DisplayOrder, rpts.DisplayName, rpts.DisplayOrder as 'GroupDisplayOrder'
		INTO #TmpCol
		FROM CodeSheet c 
			JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID 
			LEFT JOIN ReportGroups rpts ON rpts.ReportGroupID = c.ReportGroupID 
		WHERE rg.PubID = @ProductID AND rg.DisplayName = @Col
			
		INSERT INTO #TmpCol
		SELECT 'ZZ. No Response', 1000, '', 0
	END
	ELSE
	BEGIN
		SET @ColID = @ProfileFieldID
	END

	IF @IssueID = 0 --Query Current Issue
	BEGIN
		

		IF @RowID = @ResponseFieldID AND @ColID = @ResponseFieldID
		BEGIN
			INSERT INTO #Tmp1
			SELECT	ps.PubSubscriptionID, 
					ps.Copies, 
					c.DisplayName as 'Demo7', 
					ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') as 'Row', 
					ISNULL(cs.DisplayOrder,100) as 'DisplayOrder', ISNULL(CONVERT(varchar(50),cs.CodeSheetID), 'ZZ') as CodeSheetID, c.CodeId as 'DemoID', 
					CASE WHEN @IncludeReportGroup= 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END as 'GroupDisplay', 
					CASE WHEN @IncludeReportGroup= 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END as 'GroupDisplayOrder',
					ISNULL(cs.ResponseGroupID, '' + CONVERT(VARCHAR(50),@ResponseGroupRowID) + '') as ResponseGroupID
			FROM 
					PubSubscriptions ps WITH (NOLOCK)
					JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
					JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeId = @DeliverID
					LEFT JOIN 
					(
						SELECT psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID, rg.ResponseGroupID 
						FROM #Subscriptions s WITH (NOLOCK)
						JOIN PubSubscriptionDetail psd WITH (NOLOCK) ON s.PubSubscriptionID = psd.PubSubscriptionID
						JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID 
						JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID 
						WHERE rg.ResponseGroupName = @Row AND rg.PubID = @ProductID 
					) cs ON cs.PubSubscriptionID = s.PubSubscriptionID
					JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID
			WHERE 
					ps.PubID =  @ProductID
		
			INSERT INTO #Tmp2
			SELECT 
					ps.PubSubscriptionID, 
					ps.Copies, 
					c.DisplayName as 'Demo7', 
					ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') as 'Col', 
					ISNULL(cs.DisplayOrder,100) as 'DisplayOrder', ISNULL(CONVERT(varchar(50),cs.CodeSheetID), 'ZZ') as CodeSheetID, c.CodeId as 'DemoID', 
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END as 'GroupDisplay', 
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END as 'GroupDisplayOrder',
					ISNULL(cs.ResponseGroupID, @ResponseGroupColID) as ResponseGroupID
			FROM 
					PubSubscriptions ps WITH (NOLOCK)
					JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
					JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 and c.CodeTypeId = @DeliverID
					LEFT JOIN 
					(
						SELECT psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID, rg.ResponseGroupID 
						FROM #Subscriptions s WITH (NOLOCK)
						JOIN PubSubscriptionDetail psd WITH (NOLOCK) ON s.PubSubscriptionID = psd.PubSubscriptionID
						JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID 
						JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID 
						WHERE rg.ResponseGroupName = @Col AND rg.PubID = @ProductID
					) cs ON cs.PubSubscriptionID = s.PubSubscriptionID
					JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID
			WHERE 
					ps.PubID = @ProductID

		END
	END
	ELSE --Query Archive
	BEGIN
		

		IF @RowID = @ResponseFieldID AND @ColID = @ResponseFieldID
			BEGIN
				INSERT INTO #Tmp1
				SELECT
					ps.PubSubscriptionID,
					ps.Copies,
					c.DisplayName AS 'Demo7',
					ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Row',
					ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',
					ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,
					c.CodeId AS 'DemoID',
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END AS 'GroupDisplay',
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END AS 'GroupDisplayOrder',
					ISNULL(cs.ResponseGroupID, @ResponseGroupRowID) AS ResponseGroupID
				FROM IssueArchiveProductSubscription ps WITH (NOLOCK)
				JOIN #Subscriptions s WITH (NOLOCK)
					ON s.PubSubscriptionID = ps.PubsubscriptionID
				JOIN UAD_Lookup..Code c WITH (NOLOCK)
					ON c.CodeValue = ps.demo7
					AND c.CodeTypeId = @DeliverID
				LEFT JOIN 
				(
					SELECT
						psd.PubSubscriptionID,cs.ReportGroupID,cs.Responsevalue,cs.Responsedesc,cs.DisplayOrder,cs.CodeSheetID,rg.ResponseGroupID,psd.IssueArchiveSubscriptionID
						FROM #Subscriptions s WITH (NOLOCK)
					JOIN IssueArchiveProductSubscription ias WITH (NOLOCK)
						ON s.PubSubscriptionID = ias.PubsubscriptionID
						AND IssueID = @IssueID
					JOIN IssueArchiveProductSubscriptionDetail psd WITH (NOLOCK)
						ON psd.IssueArchiveSubscriptionId = ias.IssueArchiveSubscriptionId
					JOIN CodeSheet cs
						ON cs.CodeSheetID = psd.CodesheetID
					JOIN ResponseGroups rg WITH (NOLOCK)
						ON rg.ResponseGroupID = cs.ResponseGroupID
					WHERE rg.ResponseGroupName = @Row
					AND rg.PubID = @ProductID
				) cs
					ON cs.IssueArchiveSubscriptionID = ps.IssueArchiveSubscriptionID
				JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK)
					ON cc.CategoryCodeID = ps.PubCategoryID
				JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK)
					ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
				LEFT JOIN ReportGroups rpts WITH (NOLOCK)
					ON rpts.ReportGroupID = cs.ReportGroupID
				WHERE ps.IssueID = @IssueID
		
				INSERT INTO #Tmp2
				SELECT
					ps.PubSubscriptionID,
					ps.Copies,
					c.DisplayName AS 'Demo7',
					ISNULL(cs.Responsevalue + '. ' + cs.Responsedesc, 'ZZ. No Response') AS 'Col',
					ISNULL(cs.DisplayOrder, 100) AS 'DisplayOrder',
					ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS CodeSheetID,
					c.CodeId AS 'DemoID',
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END AS 'GroupDisplay',
					CASE WHEN @IncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, @MaxDisplayOrder) ELSE 0 END AS 'GroupDisplayOrder',
					ISNULL(cs.ResponseGroupID, @ResponseGroupColID) AS ResponseGroupID
				FROM IssueArchiveProductSubscription ps WITH (NOLOCK)
				JOIN #Subscriptions s WITH (NOLOCK)
					ON s.PubSubscriptionID = ps.PubsubscriptionID
				JOIN UAD_Lookup..Code c WITH (NOLOCK)
					ON c.CodeValue = ps.demo7
					AND c.CodeTypeId = @DeliverID
				LEFT JOIN 
				(
					SELECT
						psd.PubSubscriptionID,cs.ReportGroupID,cs.Responsevalue,cs.Responsedesc,cs.DisplayOrder,cs.CodeSheetID,rg.ResponseGroupID,psd.IssueArchiveSubscriptionID
					FROM #Subscriptions s WITH (NOLOCK)
					JOIN IssueArchiveProductSubscription ias WITH (NOLOCK)
						ON s.PubSubscriptionID = ias.PubsubscriptionID
						AND IssueID = @IssueID
					JOIN IssueArchiveProductSubscriptionDetail psd WITH (NOLOCK)
						ON psd.IssueArchiveSubscriptionId = ias.IssueArchiveSubscriptionId
					JOIN CodeSheet cs WITH (NOLOCK)
						ON cs.CodeSheetID = psd.CodesheetID
					JOIN ResponseGroups rg WITH (NOLOCK)
						ON rg.ResponseGroupID = cs.ResponseGroupID
					WHERE rg.ResponseGroupName = @Col
					AND rg.PubID = @ProductID
				) cs
					ON cs.IssueArchiveSubscriptionID = ps.IssueArchiveSubscriptionID
				JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK)
					ON cc.CategoryCodeID = ps.PubCategoryID
				JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK)
					ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
				LEFT JOIN ReportGroups rpts WITH (NOLOCK)
					ON rpts.ReportGroupID = cs.ReportGroupID
				WHERE ps.IssueID = @IssueID
														
			END
	END

	select codesheetID, isnull(count(distinct PubSubscriptionID),0) as colTotalUniqueRespondents
	into #tmpColUniqueCounts
	from #Tmp2
	group by codesheetID
	
	declare @colgrandTotalUniqueRespondents int
	
	select @colgrandTotalUniqueRespondents = isnull(count(distinct PubSubscriptionID),0)  from #Tmp1 -- SUM(colTotalUniqueRespondents) from #tmpColUniqueCounts

	SELECT 
		t1.Row, 
		t2.Col as 'Column', 
		sum(t1.Copies) Copies, 
		count(distinct t1.PubSubscriptionID) as RecordCount, 
		colTotalUniqueRespondents,
		@colgrandTotalUniqueRespondents colgrandTotalUniqueRespondents,
		t1.Demo7, 
		t1.DisplayOrder as RowDisplayOrder, 
		t2.DisplayOrder as ColDisplayOrder, 
		ISNULL(t1.ResponseGroupID, 0) as 'RowID', 
		ISNULL(t2.ResponseGroupID, 0) as 'ColID', 
		ISNULL(CONVERT(varchar(50),t1.CodeSheetID), 'ZZ') as 'RowCodeSheetID', 
		ISNULL(CONVERT(varchar(50),t2.CodeSheetID), 'ZZ') as 'ColCodeSheetID',
		t1.GroupDisplay, 
		t1.GroupDisplayOrder, 
		t2.GroupDisplay as 'ColGroupDisplay', 
		t2.GroupDisplayOrder as 'ColGroupDisplayOrder' 
FROM	
		#Tmp1 t1 WITH (NOLOCK)
		JOIN #Tmp2 t2 WITH (NOLOCK) ON t1.PubSubscriptionID = t2.PubSubscriptionID 
		left outer join #tmpColUniqueCounts tc with (NOLOCK) on t2.CodeSheetID = tc.CodeSheetID
group by 
		t1.Row,
		t2.Col,
		t1.Demo7, 
		colTotalUniqueRespondents,
		t1.DisplayOrder, 
		t2.DisplayOrder, 
		ISNULL(t1.ResponseGroupID, 0), 
		ISNULL(t2.ResponseGroupID, 0),
		ISNULL(CONVERT(varchar(50),t1.CodeSheetID), 'ZZ'), 
		ISNULL(CONVERT(varchar(50),t2.CodeSheetID), 'ZZ'),
		t1.GroupDisplay, 
		t1.GroupDisplayOrder, 
		t2.GroupDisplay, 
		t2.GroupDisplayOrder

	IF OBJECT_ID('tempdb..#TmpRow') IS NOT NULL DROP TABLE #TmpRow
	IF OBJECT_ID('tempdb..#TmpCol') IS NOT NULL  DROP TABLE #TmpCol
	IF OBJECT_ID('tempdb..#Subscriptions') IS NOT NULL  DROP TABLE #Subscriptions
	IF OBJECT_ID('tempdb..#Tmp1') IS NOT NULL  DROP TABLE #Tmp1
	IF OBJECT_ID('tempdb..#Tmp2') IS NOT NULL  DROP TABLE #Tmp2
	IF OBJECT_ID('tempdb..#tmpColUniqueCounts') IS NOT NULL  DROP TABLE #tmpColUniqueCounts

	
	
END

