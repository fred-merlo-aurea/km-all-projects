CREATE PROCEDURE [dbo].[rpt_DemoXQualification]
	@ProductID int,
	@Row varchar(100),
	@Filters TEXT = '<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@IssueID int = 0,
	@IncludeReportGroup bit = 0
AS
BEGIN

	SET NOCOUNT ON
   	
   	
	IF 1=0 
		BEGIN
     SET FMTONLY OFF
   END

--DECLARE @ProductID int = 6
--DECLARE @Row varchar(100) = 'BUSINESS'
--DECLARE @IncludeAddRemove bit = 0
--DECLARE @Filters varchar(max) = '<XML><Filters><ProductID>6</ProductID></Filters></XML>'
--DECLARE @ADHocFilters varchar(max) = '<XML></XML>'
--DECLARE @IssueID int = 8
--DECLARE @IncludeReportGroup bit = 0

DECLARE
	@LocalProductID int=@ProductID,
	@LocalRow varchar(100)=@Row,
	@LocalFilters varchar(max) = @Filters,
	@LocalAdHocFilters varchar(max) = @AdHocFilters,
	@LocalIssueID int =@IssueID,
	@LocalIncludeReportGroup bit = @IncludeReportGroup

CREATE TABLE #Subscriptions (PubSubscriptionID int PRIMARY KEY)

declare @colgrandTotalUniqueRespondents int
DECLARE @Groups varchar(max) = ''
DECLARE @ResponseGroupRowID int = 0
DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
DECLARE @Year varchar(50) = '', @startDate varchar(50) = '', @endDate varchar(50) = '', @startPeriod varchar(50) = ''
SELECT @Groups = STUFF( (SELECT ',' + '''' + ResponseGroupName + '''' FROM ResponseGroups WHERE PubID = @LocalProductID for XML PATH('')), 1,1,'')
SELECT @startperiod = YearStartDate FROM Pubs WHERE PubID = @LocalProductID

	if(exists(select DateComplete from Issue where IssueId = @LocalIssueID) and @LocalIssueID > 0)
	begin
		IF (select DateComplete from Issue where IssueId = @LocalIssueID) > convert(datetime,@startperiod + '/' + convert(varchar,year((select DateComplete from Issue where IssueId = @LocalIssueID))))
			SET @Year = year((select DateComplete from Issue where IssueId = @LocalIssueID)) 
		ELSE
			SET @Year = year((select DateComplete from Issue where IssueId = @LocalIssueID)) - 1
	end
	else 
	begin
		IF GETDATE() > CONVERT(DATETIME,@startperiod + '/' + CONVERT(VARCHAR,YEAR(GETDATE())))
			SET @Year = YEAR(GETDATE()) 
		ELSE
			SET @Year = YEAR(GETDATE()) - 1	
	end
	
SET @startDate = LTRIM(RTRIM(@startperiod)) + '/' + CONVERT(VARCHAR,@Year)
SET @endDate = CONVERT(VARCHAR(10), DATEADD(ss, -1, DATEADD(yy, 1, CONVERT(DATETIME,@startdate))), 101)

SET @ResponseGroupRowID = (SELECT ResponseGroupID FROM ResponseGroups WHERE ResponseGroupName = @LocalRow AND PubID = @LocalProductID)
SELECT Responsedesc, c.DisplayOrder, ISNULL(rpts.DisplayName, '') as 'DisplayName', ISNULL(rpts.DisplayOrder, 0) as 'GroupDisplayOrder', CONVERT(varchar(50), CodeSheetID) as CodeSheetID
INTO #TmpRow
	FROM CodeSheet c 
		JOIN ResponseGroups rg ON rg.ResponseGroupID = c.ResponseGroupID 
		LEFT JOIN ReportGroups rpts ON rpts.ReportGroupID = c.ReportGroupID 
	WHERE rg.PubID = @LocalProductID AND rg.DisplayName = @LocalRow
INSERT INTO #TmpRow
SELECT 'No Answer', 1000, '', 0, 'ZZ'

IF @LocalIssueID = 0 --Query Current Issue
BEGIN
	INSERT INTO #Subscriptions
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @LocalFilters, @LocalAdHocFilters
	
	
		select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)  
		from PubSubscriptions ps WITH (NOLOCK)
			JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID
			JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
			JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		where
			ps.pubID = @LocalProductID

		SELECT
			c.CodeName AS Demo7, (CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END) AS 'Year', isnull(count(distinct s.PubSubscriptionID),0) as colTotalUniqueRespondents
			into #tmpColUniqueCounts
		FROM 
			PubSubscriptions ps WITH (NOLOCK)
			JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID
			JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
			JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		where
			ps.pubID = @LocalProductID
		group by c.CodeName, (CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END)
	
	IF CHARINDEX(@LocalRow, @Groups) > 0
	BEGIN	
	
	select * into #tmp1 from (
			SELECT
				SUM(ps.Copies) Copies,
				c.CodeName AS Demo7,
				ISNULL(cs.Responsedesc, 'No Answer') AS 'Row',
				ISNULL(cs.DisplayOrder, 1000) AS RowDisplayOrder,
				COUNT(DISTINCT ps.PubSubscriptionID) AS RecordCount,
				(CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END) AS 'Year',
				c.CodeID AS 'DemoID',
				@ResponseGroupRowID ResponseGroupID,
				ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS 'CodeSheetID',
				CASE WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END AS 'GroupDisplay',
				CASE WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, 0) ELSE 0
				END AS GroupDisplayOrder
			FROM 
				PubSubscriptions ps WITH (NOLOCK)
				JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
				JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
				LEFT JOIN 
				(
					SELECT psd.PubSubscriptionID,cs.ReportGroupID,cs.Responsevalue,cs.Responsedesc,cs.DisplayOrder,cs.CodeSheetID,rg.ResponseGroupID
					FROM 
						#Subscriptions s WITH (NOLOCK)
						JOIN PubSubscriptionDetail psd WITH (NOLOCK) ON s.PubSubscriptionID = psd.PubSubscriptionID
						JOIN CodeSheet cs WITH (NOLOCK) ON cs.CodeSheetID = psd.CodesheetID
						JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID
					WHERE 
						rg.ResponseGroupName = @LocalRow AND rg.PubID = @LocalProductID
				) cs
					ON cs.PubSubscriptionID = s.PubSubscriptionID
				JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
				JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK)  ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
				LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID
			WHERE 
				ps.PubID = @LocalProductID
			GROUP BY	
				c.CodeName,
				ISNULL(cs.Responsedesc, 'No Answer'),
				ISNULL(cs.DisplayOrder, 1000),
				(CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END),
				c.CodeID,
				ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),
				CASE WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '') ELSE '' END,
				CASE WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, 0) ELSE 0 END
			UNION ALL SELECT 0, '', Responsedesc, DisplayOrder, 0, '', 0,  @ResponseGroupRowID , CONVERT(varchar(50), CodeSheetID), DisplayName, GroupDisplayOrder FROM #TmpRow
		) as tmp
		
		END
	ELSE
	BEGIN
		EXEC('select * into #tmp1 from (SELECT sum(ps.Copies) Copies, c.CodeName as Demo7, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalRow + ']), ''''), ''0''), ''No Response'') as ''Row'', 
			0 as RowDisplayOrder, count(distinct ps.PubSubscriptionID) as RecordCount,
			(CASE WHEN ps.qualificationDate between ''' + @startDate + ''' and ''' + @endDate + ''' THEN ''1''
			WHEN ps.qualificationDate between DATEADD(yy, -1, ''' + @startDate + ''') and DATEADD(yy, -1, ''' + @endDate + ''') THEN ''2''
			WHEN ps.qualificationDate between DATEADD(yy, -2, ''' + @startDate + ''') and DATEADD(yy, -2, ''' + @endDate + ''') THEN ''3''
			WHEN ps.qualificationDate between DATEADD(yy, -3, ''' + @startDate + ''') and DATEADD(yy, -3, ''' + @endDate + ''') THEN ''4''
			WHEN ps.qualificationDate <= DATEADD(yy, -4, ''' + @endDate + ''') or ISNULL(NULLIF(ps.QualificationDate, ''''), '''') = '''' THEN ''4+'' END) as ''Year'', c.CodeID as ''DemoID''
		FROM PubSubscriptions ps WITH (NOLOCK)
		JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_LookUp..Code c WITH (NOLOCK) ON c.CodeValue = ps.Demo7 
		WHERE ps.PubID = ' + @LocalProductID +  ' and c.CodeTypeID = ' + @DeliverID +
		' group by 
			c.CodeName, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalRow + ']), ''''), ''0''), ''No Response''), 
			(CASE WHEN ps.qualificationDate between ''' + @startDate + ''' and ''' + @endDate + ''' THEN ''1''
			WHEN ps.qualificationDate between DATEADD(yy, -1, ''' + @startDate + ''') and DATEADD(yy, -1, ''' + @endDate + ''') THEN ''2''
			WHEN ps.qualificationDate between DATEADD(yy, -2, ''' + @startDate + ''') and DATEADD(yy, -2, ''' + @endDate + ''') THEN ''3''
			WHEN ps.qualificationDate between DATEADD(yy, -3, ''' + @startDate + ''') and DATEADD(yy, -3, ''' + @endDate + ''') THEN ''4''
			WHEN ps.qualificationDate <= DATEADD(yy, -4, ''' + @endDate + ''') or ISNULL(NULLIF(ps.QualificationDate, ''''), '''') = '''' THEN ''4+'' END), c.CodeID ) t'
		)	
	END
	
	select distinct t.*,cuc.colTotalUniqueRespondents,Demo7Total, @colgrandTotalUniqueRespondents as colgrandTotalUniqueRespondents
		from #tmp1 t
			join #tmpColUniqueCounts cuc on t.[year] = cuc.[year] and t.Demo7 = cuc.demo7
			join (select demo7,SUM(colTotalUniqueRespondents) as Demo7Total from #tmpColUniqueCounts group by demo7) d on d.demo7 = t.Demo7 and cuc.Demo7 = d.Demo7

END
ELSE
BEGIN
	INSERT INTO #Subscriptions
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML @LocalFilters, @LocalAdHocFilters, 0, 1, @LocalIssueID


		select @colgrandTotalUniqueRespondents = isnull(count(distinct ps.PubSubscriptionID),0)  
		from IssueArchiveProductSubscription ps WITH (NOLOCK)
			JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID
			JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
			JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		where
			ps.pubID = @LocalProductID

		SELECT
			c.CodeName AS Demo7, (CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END) AS 'Year', isnull(count(distinct s.PubSubscriptionID),0) as colTotalUniqueRespondents
			into #tmpColUniqueCounts2
		FROM 
			IssueArchiveProductSubscription ps WITH (NOLOCK)
			JOIN #Subscriptions s WITH (NOLOCK) ON ps.PubSubscriptionID = s.PubSubscriptionID
			JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
			JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
			JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		where
			ps.pubID = @LocalProductID
		group by c.CodeName, (CASE
					WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
					WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
					WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
						ps.qualificationDate IS NULL THEN '4+'
				END)
				
	IF CHARINDEX(@LocalRow, @Groups) > 0
	BEGIN	
		select * into #tmp2 from (SELECT
			SUM(ps.Copies) Copies,
			c.CodeName AS Demo7,
			ISNULL(cs.Responsedesc, 'No Answer') AS 'Row',
			ISNULL(cs.DisplayOrder, 1000) AS RowDisplayOrder,
			COUNT(DISTINCT ps.PubSubscriptionID) AS RecordCount,
			(CASE
				WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
				WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
				WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
				WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
				WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
					ps.qualificationDate IS NULL THEN '4+'
			END) AS 'Year',
			c.CodeID AS 'DemoID',
			@ResponseGroupRowID AS ResponseGroupID,
			ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ') AS 'CodeSheetID',
			CASE
				WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '')
				ELSE ''
			END AS 'GroupDisplay',
			CASE
				WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, 0)
				ELSE 0
			END AS GroupDisplayOrder
		FROM 
		IssueArchiveProductSubscription ps WITH (NOLOCK)
		JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubsubscriptionID
		JOIN UAD_Lookup..Code c WITH (NOLOCK) ON c.CodeValue = ps.demo7 AND c.CodeTypeId = @DeliverID
		LEFT JOIN 
		(
			SELECT
				psd.PubSubscriptionID, cs.ReportGroupID, cs.Responsevalue, cs.Responsedesc, cs.DisplayOrder, cs.CodeSheetID, rg.ResponseGroupID
			FROM 
				#Subscriptions s WITH (NOLOCK)
				JOIN IssueArchiveProductSubscription ias WITH (NOLOCK) ON s.PubSubscriptionID = ias.PubsubscriptionID AND IssueID = @LocalIssueID
				JOIN IssueArchiveProductSubscriptionDetail psd WITH (NOLOCK) ON psd.IssueArchiveSubscriptionId = ias.IssueArchiveSubscriptionId
				JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID
				JOIN ResponseGroups rg WITH (NOLOCK) ON rg.ResponseGroupID = cs.ResponseGroupID
			WHERE 
				rg.ResponseGroupName = @LocalRow AND rg.PubID = @LocalProductID
		) cs
			ON cs.PubSubscriptionID = s.PubSubscriptionID
		JOIN UAD_Lookup..CategoryCode cc WITH (NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
		JOIN UAD_Lookup..CategoryCodeType cct WITH (NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
		LEFT JOIN ReportGroups rpts WITH (NOLOCK) ON rpts.ReportGroupID = cs.ReportGroupID
		WHERE 
			ps.IssueID = @LocalIssueID
		GROUP BY	c.CodeName,
					ISNULL(cs.Responsedesc, 'No Answer'),
					ISNULL(cs.DisplayOrder, 1000),
					(CASE
						WHEN ps.qualificationDate BETWEEN @startDate AND @endDate THEN '1'
						WHEN ps.qualificationDate BETWEEN DATEADD(yy, -1, @startDate) AND DATEADD(yy, -1, @endDate) THEN '2'
						WHEN ps.qualificationDate BETWEEN DATEADD(yy, -2, @startDate) AND DATEADD(yy, -2, @endDate) THEN '3'
						WHEN ps.qualificationDate BETWEEN DATEADD(yy, -3, @startDate) AND DATEADD(yy, -3, @endDate) THEN '4'
						WHEN ps.qualificationDate <= DATEADD(yy, -4, @endDate) OR
							ps.qualificationDate IS NULL THEN '4+'
					END),
					c.CodeID,
					ISNULL(CONVERT(varchar(50), cs.CodeSheetID), 'ZZ'),
					CASE
						WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayName, '')
						ELSE ''
					END,
					CASE
						WHEN @LocalIncludeReportGroup = 1 THEN ISNULL(rpts.DisplayOrder, 0)
						ELSE 0
					END

		UNION ALL SELECT 0, '', Responsedesc, DisplayOrder, 0, '', 0,  @ResponseGroupRowID, CONVERT(varchar(50), CodeSheetID), DisplayName, GroupDisplayOrder FROM #TmpRow) t
	END
	ELSE
	BEGIN
		EXEC('select * into #tmp2 from (SELECT SUM(ps.Copies) Copies, c.CodeName as Demo7, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalRow + ']), ''''), ''0''), ''No Response'') as ''Row'', 
			0 as RowDisplayOrder, COUNT(DISTINCT ps.PubSubscriptionID) as RecordCount,
			(CASE WHEN ps.qualificationDate between ''' + @startDate + ''' and ''' + @endDate + ''' THEN ''1''
			WHEN ps.qualificationDate between DATEADD(yy, -1, ''' + @startDate + ''') and DATEADD(yy, -1, ''' + @endDate + ''') THEN ''2''
			WHEN ps.qualificationDate between DATEADD(yy, -2, ''' + @startDate + ''') and DATEADD(yy, -2, ''' + @endDate + ''') THEN ''3''
			WHEN ps.qualificationDate between DATEADD(yy, -3, ''' + @startDate + ''') and DATEADD(yy, -3, ''' + @endDate + ''') THEN ''4''
			WHEN ps.qualificationDate <= DATEADD(yy, -4, ''' + @endDate + ''') or ISNULL(NULLIF(ps.QualificationDate, ''''), '''') = '''' THEN ''4+'' END) as ''Year'', c.CodeID as ''DemoID''
		FROM IssueArchiveProductSubscription ps WITH (NOLOCK)
		JOIN #Subscriptions s WITH (NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID
		JOIN UAD_LookUp..Code c WITH (NOLOCK) ON c.CodeValue = ps.Demo7 
		WHERE ps.PubID = ' + @LocalProductID +  ' and c.CodeTypeID = ' + @DeliverID	+ ' AND ps.IssueID = ' + @LocalIssueID + 
		' group by
				c.CodeName, ISNULL(NULLIF(NULLIF(CONVERT(VARCHAR(500),ps.[' + @LocalRow + ']), ''''), ''0''), ''No Response''), 
			(CASE WHEN ps.qualificationDate between ''' + @startDate + ''' and ''' + @endDate + ''' THEN ''1''
			WHEN ps.qualificationDate between DATEADD(yy, -1, ''' + @startDate + ''') and DATEADD(yy, -1, ''' + @endDate + ''') THEN ''2''
			WHEN ps.qualificationDate between DATEADD(yy, -2, ''' + @startDate + ''') and DATEADD(yy, -2, ''' + @endDate + ''') THEN ''3''
			WHEN ps.qualificationDate between DATEADD(yy, -3, ''' + @startDate + ''') and DATEADD(yy, -3, ''' + @endDate + ''') THEN ''4''
			WHEN ps.qualificationDate <= DATEADD(yy, -4, ''' + @endDate + ''') or ISNULL(NULLIF(ps.QualificationDate, ''''), '''') = '''' THEN ''4+'' END) , c.CodeID ) t ')
	END
		select distinct t.*,cuc.colTotalUniqueRespondents,Demo7Total, @colgrandTotalUniqueRespondents as colgrandTotalUniqueRespondents
		from #tmp2 t
			join #tmpColUniqueCounts2 cuc on t.[year] = cuc.[year] and t.Demo7 = cuc.demo7
			join (select demo7,SUM(colTotalUniqueRespondents) as Demo7Total from #tmpColUniqueCounts2 group by demo7) d on d.demo7 = t.Demo7 and cuc.Demo7 = d.Demo7

END

IF OBJECT_ID('tempdb..#TmpRow') IS NOT NULL DROP TABLE #TmpRow
IF OBJECT_ID('tempdb..#Subscriptions') IS NOT NULL  DROP TABLE #Subscriptions
IF OBJECT_ID('tempdb..#tmpColUniqueCounts') IS NOT NULL  DROP TABLE #tmpColUniqueCounts
IF OBJECT_ID('tempdb..#tmp1') IS NOT NULL  DROP TABLE #tmp1
IF OBJECT_ID('tempdb..#tmp2') IS NOT NULL  DROP TABLE #tmp2
IF OBJECT_ID('tempdb..#tmpColUniqueCounts2') IS NOT NULL  DROP TABLE #tmpColUniqueCounts2


END


GO


