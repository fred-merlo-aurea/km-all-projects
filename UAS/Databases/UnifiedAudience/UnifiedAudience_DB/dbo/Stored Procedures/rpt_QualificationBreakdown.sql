CREATE proc [dbo].[rpt_QualificationBreakdown]
(
	@Filters TEXT ='<XML><Filters></Filters></XML>',
	@AdHocFilters TEXT = '<XML></XML>',
	@IncludeAddRemove bit = 0,
	@ProductID int,
	@IssueID int = 0
)
as
BEGIN 
	--Declare @Filters varchar(max) ='<XML><Filters><QsourceIDs>1874</QsourceIDs><ProductID>1</ProductID><StateIDs>MN</StateIDs></Filters></XML> '
	--DECLARE @AdHocFilters varchar(max) = '<XML><FilterDetail><FilterField>Qualificationdate</FilterField><SearchCondition>DateRange</SearchCondition><AdHocToField>5/31/2013</AdHocToField><AdHocFromField>6/1/2012</AdHocFromField><FilterObjectType>DateRange</FilterObjectType></FilterDetail></XML>'
	--DECLARE @IncludeAddRemove bit = 1
	--DECLARE @ProductID int = 1
	--DECLARE @IssueID int = 0
	
	IF 1=0 BEGIN
     SET FMTONLY OFF
	END

	SET NOCOUNT ON
	
	CREATE TABLE #SubscriptionID (SubscriptionID int)  
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	DECLARE @qSourceCodeID int, @qSourceTypeCodeID int
	SET @qSourceCodeID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source')
	SET @qSourceTypeCodeID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source Type')
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	DECLARE @NoResponseID int
	
	SELECT CodeID, DisplayName
	INTO #QSourceTypes
	FROM UAD_Lookup..Code WHERE CodeTypeId = @qSourceTypeCodeID
	INSERT INTO #QSourceTypes
	SELECT 'No Response'
	SET @NoResponseID = (SELECT @@IDENTITY)
	
	SELECT CategoryCodeTypeName
	INTO #CatTypes
	FROM UAD_Lookup..CategoryCodeType WHERE CategoryCodeTypeName in ('Qualified Free', 'Qualified Paid')

	DECLARE @yearTemp int,
		@startperiod varchar(10),
		@endperiod varchar(10),
		@startdateTemp datetime,
		@enddateTemp datetime
	
	SELECT @startperiod = p.YearStartDate , @endperiod = p.YearEndDate FROM Pubs p WHERE PubID = @ProductID
	
	if(exists(select DateComplete from Issue where IssueId = @issueid) and @issueid > 0)
	begin
		IF (select DateComplete from Issue where IssueId = @issueid) > convert(datetime,@startperiod + '/' + convert(varchar,year((select DateComplete from Issue where IssueId = @issueid))))
			SET @yearTemp = year((select DateComplete from Issue where IssueId = @issueid)) 
		ELSE
			SET @yearTemp = year((select DateComplete from Issue where IssueId = @issueid)) - 1
	end
	else 
	begin
		IF GETDATE() > CONVERT(DATETIME,@startperiod + '/' + CONVERT(VARCHAR,YEAR(GETDATE())))
			SET @yearTemp = YEAR(GETDATE()) 
		ELSE
			SET @yearTemp = YEAR(GETDATE()) - 1	
	end
		
	SELECT @startdateTemp = @startperiod + '/' + convert(varchar,@yearTemp)
	SELECT @endDateTemp =  dateadd(ss, -1, dateadd(yy, 1, @startdateTemp)) 
	
	IF @IssueID = 0
	BEGIN
		INSERT INTO #SubscriptionID   
		EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML  
		@Filters, @AdHocFilters, @IncludeAddRemove
		
		select QSourceID, Year, [Qual Status],  Demo7, QSourceType, QSource, sum(Copies) Copies from
		(
			SELECT ISNULL(ps.PubQSourceID, 0) as 'QSourceID', (CASE WHEN ps.Qualificationdate between @startdateTemp and @endDateTemp THEN '1 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) THEN '2 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) THEN '3 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) THEN '4 Year'
										  WHEN ps.Qualificationdate < dateadd(yy, -4,  @endDateTemp ) THEN 'Older' END) as 'Year',
				   cct.CategoryCodeTypeName as 'Qual Status', ps.Copies, cin.DisplayName as 'Demo7', ISNULL(c2.DisplayName, 'No Response') as 'QSourceType', 
				   ISNULL(c.DisplayName, 'No Response') as 'QSource'
			FROM PubSubscriptions ps with(NOLOCK) 
			JOIN #SubscriptionID sf with(NOLOCK) on sf.SubscriptionID = ps.PubSubscriptionID 
			JOIN UAD_Lookup..Code cin ON cin.CodeValue = ps.demo7 
			JOIN UAD_Lookup..CategoryCode cc with(NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID 
			JOIN UAD_Lookup..CategoryCodeType cct with(NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			LEFT JOIN UAD_Lookup..Code c with(NOLOCK) ON c.CodeId = ps.PubQSourceID AND c.CodeTypeId = @qSourceCodeID
			LEFT JOIN UAD_Lookup..Code c2 with(NOLOCK) ON c.ParentCodeId = c2.CodeId
			WHERE ps.PubID = @ProductID
			AND cin.CodeTypeId = @DeliverID
			UNION ALL
			SELECT ISNULL(c.CodeId,0), '', '',0, '', q.DisplayName as 'QSourceType', ISNULL(c.DisplayName, 'No Response') as 'QSource'
			FROM #QSourceTypes q
			LEFT JOIN UAD_Lookup..Code c ON c.ParentCodeId = q.CodeId
			UNION ALL
			SELECT 0, '', CategoryCodeTypeName, 0, '', '', ''
			FROM #CatTypes
		) x
		group by QSourceID, Year, [Qual Status],  Demo7, QSourceType, QSource

	END
	ELSE
	BEGIN
		INSERT INTO #SubscriptionID   
		EXEC rpt_GetSubscriptionIDs_Copies_From_Filter_XML  
		@Filters, @AdHocFilters, @IncludeAddRemove, 1, @IssueID
		
		select QSourceID, Year, [Qual Status],  Demo7, QSourceType, QSource, sum(Copies) Copies from
		(
			SELECT ISNULL(ps.PubQSourceID, 0) as 'QSourceID', (CASE WHEN ps.Qualificationdate between @startdateTemp and @endDateTemp THEN '1 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) THEN '2 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) THEN '3 Year'
										  WHEN ps.Qualificationdate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) THEN '4 Year'
										  WHEN ps.Qualificationdate < dateadd(yy, -4,  @endDateTemp ) THEN 'Older' END) as 'Year',
				   cct.CategoryCodeTypeName as 'Qual Status', ps.Copies, cin.DisplayName as 'Demo7', ISNULL(c2.DisplayName, 'No Response') as 'QSourceType', 
				   ISNULL(c.DisplayName, 'No Response') as 'QSource'
			FROM IssueArchiveProductSubscription ps with(NOLOCK) 
			JOIN #SubscriptionID sf with(NOLOCK) on sf.SubscriptionID = ps.PubSubscriptionID 
			JOIN UAD_Lookup..Code cin ON cin.CodeValue = ps.demo7 
			JOIN UAD_Lookup..CategoryCode cc with(NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID 
			JOIN UAD_Lookup..CategoryCodeType cct with(NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
			LEFT JOIN UAD_Lookup..Code c with(NOLOCK) ON c.CodeId = ps.PubQSourceID AND c.CodeTypeId = @qSourceCodeID
			LEFT JOIN UAD_Lookup..Code c2 with(NOLOCK) ON c.ParentCodeId = c2.CodeId
			WHERE ps.PubID = @ProductID AND ps.IssueID = @IssueID
			AND cin.CodeTypeId = @DeliverID
			UNION ALL
			SELECT ISNULL(c.CodeId,0), '', '',0, '', q.DisplayName as 'QSourceType', ISNULL(c.DisplayName, 'No Response') as 'QSource'
			FROM #QSourceTypes q
			LEFT JOIN UAD_Lookup..Code c ON c.ParentCodeId = q.CodeId
			UNION ALL
			SELECT 0, '', CategoryCodeTypeName, 0, '', '', ''
			FROM #CatTypes
		) x
		group by QSourceID, Year, [Qual Status],  Demo7, QSourceType, QSource
	END
	
	DROP TABLE #SubscriptionID
	DROP TABLE #QSourceTypes	
	DROP TABLE #CatTypes		
END

go
