CREATE PROCEDURE [dbo].[rpt_PAR3CMVC]
	@Queries VARCHAR(MAX),
	@IssueID int = 0
AS
BEGIN
	
	SET NOCOUNT ON

	--Declare @Filters varchar(max) ='<XML><Filters><ProductID>1</ProductID></Filters></XML>'
	
	IF 1=0 
		BEGIN
			SET FMTONLY OFF
		END
	
	DECLARE @DeliverID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Deliver')
	DECLARE @Par3CID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Par3c')
	
	SELECT DisplayName
	INTO #Demos
	FROM UAD_Lookup..Code 
	WHERE CodeTypeId = @DeliverID	
	
	SELECT DisplayName as 'Par3C'
	INTO #Par3C
	FROM UAD_Lookup..Code 
	WHERE CodeTypeId = @Par3CID

	SET NOCOUNT ON
	
	CREATE TABLE #SubscriptionID (PubSubscriptionID int)  
	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (PubSubscriptionID)
	INSERT INTO #SubscriptionID
	EXEC (@Queries) 
		
	IF @IssueID = 0 --Query Current Issue
		BEGIN
			
			select Par3C, Par3CID, CategoryCodeTypeName,  Demo7, DemoID, [Category Type], sum(Copies) Copies from
			(

				SELECT ISNULL(c2.DisplayName, 'ZZ. No Response') as 'Par3C', ISNULL(c2.CodeId, 0) as 'Par3CID', cct.CategoryCodeTypeName, ps.Copies, c.DisplayName as Demo7, c.CodeId as 'DemoID',
				(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'
				FROM PubSubscriptions ps with(NOLOCK)
					JOIN #SubscriptionID s with(NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Code c2 with(NOLOCK) ON c2.CodeID = ps.Par3CID AND c2.CodeTypeId = @Par3CID
					JOIN UAD_Lookup..CategoryCode cc with(NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct with(NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c with(NOLOCK) ON c.CodeValue = ps.demo7
				WHERE c.CodeTypeId = @DeliverID AND c2.CodeTypeId = @Par3CID
				UNION ALL
					SELECT '', 0, '', 0, DisplayName, 0, ''	
					FROM #Demos
				UNION ALL
					SELECT Par3C, 0, '', 0, '', 0, '' 
					FROM #Par3C
			) x 
			group by Par3C, Par3CID, CategoryCodeTypeName,  Demo7, DemoID, [Category Type]
		END
	ELSE
		BEGIN
			
			select Par3C, Par3CID, CategoryCodeTypeName,  Demo7, DemoID, [Category Type], sum(Copies) Copies from
			(
				SELECT ISNULL(c2.DisplayName, 'ZZ. No Response') as 'Par3C', ISNULL(c2.CodeId, 0) as 'Par3CID', cct.CategoryCodeTypeName, ps.Copies, c.DisplayName as Demo7, c.CodeId as 'DemoID',
				(CASE WHEN cct.CategoryCodeTypeName LIKE 'Qualified%' THEN 'Qualified' ELSE 'Non Qualified' END) as 'Category Type'
				FROM IssueArchiveProductSubscription ps with(NOLOCK)
					JOIN #SubscriptionID s with(NOLOCK) ON s.PubSubscriptionID = ps.PubSubscriptionID
					LEFT JOIN UAD_Lookup..Code c2 with(NOLOCK) ON c2.CodeID = ps.Par3CID AND c2.CodeTypeId = @Par3CID
					JOIN UAD_Lookup..CategoryCode cc with(NOLOCK) ON cc.CategoryCodeID = ps.PubCategoryID
					JOIN UAD_Lookup..CategoryCodeType cct with(NOLOCK) ON cct.CategoryCodeTypeID = cc.CategoryCodeTypeID
					JOIN UAD_Lookup..Code c with(NOLOCK) ON c.CodeValue = ps.demo7
				WHERE c.CodeTypeId = @DeliverID AND ps.IssueID = @IssueID AND c2.CodeTypeId = @Par3CID
				UNION ALL
					SELECT '', 0, '', 0, DisplayName, 0, '' 
					FROM #Demos
				UNION ALL
					SELECT Par3C, 0, '', 0, '', 0, '' 
					FROM #Par3C
			)
			x
			group by Par3C, Par3CID, CategoryCodeTypeName,  Demo7, DemoID, [Category Type]
		END
	
	DROP TABLE #SubscriptionID
	DROP TABLE #Demos
	DROP TABLE #Par3C
END

