CREATE proc [dbo].[rpt_CategorySummary_Old]
(
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@CountryIDs varchar(1500),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(1000),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@WaveMail varchar(100) = ''    
)
as
BEGIN
	
	SET NOCOUNT ON

	--DECLARE @PublicationID int = 1
	--DECLARE @CategoryIDs varchar(800) = ''
	--DECLARE @CategoryCodes varchar(800) = ''
	--DECLARE @TransactionIDs varchar(800) = ''
	--DECLARE @TransactionCodes varchar(800) = ''
	--DECLARE @QsourceIDs varchar(800) = ''
	--DECLARE @StateIDs varchar(800) = ''
	--DECLARE @Regions varchar(max) = ''
	--DECLARE @CountryIDs varchar(800) = ''
	--DECLARE @Email varchar(10) = ''
	--DECLARE @Phone varchar(10) = ''
	--DECLARE @Mobile varchar(10) = ''
	--DECLARE @Fax varchar(10) = ''
	--DECLARE @ResponseIDs varchar(800) = ''
	--DECLARE @Demo7 varchar(10) = ''		
	--DECLARE @Year varchar(20) = ''
	--DECLARE @startDate varchar(10) = ''		
	--DECLARE @endDate varchar(10) = ''
	--DECLARE @AdHocXML varchar(8000) = ''	
	
	declare @pubID int	
	set @pubID = @PublicationID
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode 
	from Pubs with(nolock) 
	where PubID = @pubID

	CREATE TABLE #SubscriptionID (SubscriptionID int, Copies int)   

	INSERT INTO #SubscriptionID   
	EXEC rpt_GetSubscriptionIDs_Copies_From_Filter @PublicationID, @CategoryIDs,
	@CategoryCodes,
	@TransactionIDs,
	@TransactionCodes,
	@QsourceIDs,
	@StateIDs,
	@CountryIDs,
	@Email,
	@Phone,
	@Mobile,
	@Fax,
	@ResponseIDs,
	@Demo7,		
	@Year,
	@startDate,		
	@endDate,
	@AdHocXML

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)

	DECLARE @cat TABLE
		(
		CategoryCodeTypeID int,
		CategoryCodeID int,
		CategoryCodeTypeName varchar(100),
		CategoryCodeName varchar(100),
		CategoryCodeValue int
		)

	INSERT INTO @cat
	SELECT	DISTINCT cct.CategoryCodeTypeID, 
			cc.CategoryCodeID, 
			cct.CategoryCodeTypeName, 
			cc.CategoryCodeName,
			CategoryCodeValue
	FROM UAD_Lookup..CategoryCodeType cct with(nolock) 
		join UAD_Lookup..CategoryCode cc with(nolock) on cc.CategoryCodeTypeID = cct.CategoryCodeTypeID 
			
	BEGIN
		DECLARE @sub TABLE
		(
			scount int,
			CategoryCodeID int
		)

		INSERT INTO @sub
		SELECT	sum(sf.copies), s.PubCategoryID
		FROM PubSubscriptions  s with(nolock) 
			JOIN #SubscriptionID sf with(nolock) on sf.SubscriptionID = s.SubscriptionID
		WHERE PubID = @PublicationID
		GROUP BY PubCategoryID		

		SELECT 
			ISNULL(c.CategoryCodeTypeName, 'Invalid') as 'Category Group',
			ISNULL(convert(varchar(10), c.CategoryCodeValue) + ' - ' +  c.CategoryCodeName, 'Invalid') as 'Category',
			isnull(sum(scount),0) as Total,
			Case	when c.CategoryCodeTypeName = 'Qualified Free' then 1
					when c.CategoryCodeTypeName = 'Qualified Paid' then 3
					when c.CategoryCodeTypeName = 'NonQualified Free' then 5
					when c.CategoryCodeTypeName = 'NonQualified Paid' then 7
					else 9
					
			end as sort
		FROM @sub s 
			left join @cat c on c.CategoryCodeID = s.CategoryCodeID 		
		GROUP BY c.CategoryCodeTypeName, c.CategoryCodeName, c.CategoryCodeValue
		UNION
			SELECT 
				c.CategoryCodeTypeName as 'Category Group',
				'Total ' + c.CategoryCodeTypeName as 'Category',
				isnull(sum(scount),0) as Total,
				Case	when c.CategoryCodeTypeName = 'Qualified Free' then 2
						when c.CategoryCodeTypeName = 'Qualified Paid' then 4
						when c.CategoryCodeTypeName = 'NonQualified Free' then 6
						when c.CategoryCodeTypeName = 'NonQualified Paid' then 8
					
				end as sort
			FROM @cat c 
				left outer join @sub s on c.CategoryCodeID = s.CategoryCodeID 
		GROUP BY c.CategoryCodeTypeName
		ORDER BY sort, Category
	END

	DROP TABLE #SubscriptionID
end