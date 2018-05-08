CREATE proc [dbo].[rpt_MediaBreakdown]
(
	@PublicationID int,
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@Regions varchar(max),
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
	@PrintColumns varchar(4000),
	@Download char(1)  
)
AS
BEGIN
	
	SET NOCOUNT ON

	--DECLARE
	--@PublicationID int = 1,
	--@CategoryIDs varchar(800)= '',
	--@CategoryCodes varchar(800) = '',
	--@TransactionIDs varchar(800)= '',
	--@TransactionCodes varchar(800) = '',
	--@QsourceIDs varchar(800) = '',
	--@StateIDs varchar(800) = '',
	--@Regions varchar(max) = '',
	--@CountryIDs varchar(1500) = '',
	--@Email varchar(10) = '',
	--@Phone varchar(10) = '',
	--@Mobile varchar(10) = '',
	--@Fax varchar(10) = '',
	--@ResponseIDs varchar(1000) = '',
	--@Demo7 varchar(50) = '',		
	--@Year varchar(20) = '',
	--@startDate varchar(10) = '',		
	--@endDate varchar(10) = '',
	--@AdHocXML varchar(8000) = '<XML></XML>', 
	--@PrintColumns varchar(4000),
	--@Download char(1)  

	BEGIN --SET UP

		create table #SubscriptionID (PubSubscriptionID int)  
	
		Insert into #SubscriptionID   
		exec rpt_GetSubscriptionIDs_From_Filter @PublicationID, @CategoryIDs,
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

		CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (PubSubscriptionID)

	END

	BEGIN

		SELECT CodeValue, CodeName
		INTO #Tmp
		FROM UAD_Lookup..Code
		WHERE CodeTypeId = (Select CodeTypeId FROM UAD_Lookup..CodeType ct WHERE ct.CodeTypeName = 'Deliver')

		INSERT INTO #Tmp (CodeValue, CodeName) VALUES('Z', 'No Response')

		Select ((CASE WHEN CodeValue <> 'Z' THEN CodeValue + '.' + CodeName + ' Edition' ELSE CodeValue + '.' + CodeName END)) as 'Description', IsNull(INN.ICount, 0) as 'Count', ISNULL(INN.IPercent, 0) as 'Percent'
		from #Tmp c
			full outer join
			(
			Select
				ISNULL(NULLIF(Demo7, ''), 'Z') as Demo7,
				COUNT(ps.PubSubscriptionID) as ICount,
				convert(varchar(250),(Count(ps.PubSubscriptionID) * 100.0 / (
				Select Count(ps.PubSubscriptionID) FROM PubSubscriptions ps JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID)) * 1.0
				) + '%' as 'IPercent'
			FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
			GROUP BY Demo7
			) as INN
		on c.CodeValue = INN.demo7
		ORDER BY 1

		DROP TABLE #Tmp
		DROP TABLE #SubscriptionID

	END

END