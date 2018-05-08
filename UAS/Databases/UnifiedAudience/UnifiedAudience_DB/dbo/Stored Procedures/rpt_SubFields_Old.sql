CREATE proc [dbo].[rpt_SubFields_Old]
(    
	@ReportID int,  
	@PublicationID varchar(800),
	@CategoryIDs varchar(800),
	@CategoryCodes varchar(800),
	@TransactionIDs varchar(800),
	@TransactionCodes varchar(800),
	@QsourceIDs varchar(800),
	@StateIDs varchar(800),
	@CountryIDs varchar(800),
	@Email varchar(10),
	@Phone varchar(10),
	@Mobile varchar(10),
	@Fax varchar(10),
	@ResponseIDs varchar(800),
	@Demo7 varchar(50),		
	@Year varchar(20),
	@startDate varchar(10),		
	@endDate varchar(10),
	@AdHocXML varchar(8000),
	@WaveMail varchar(100) = ''
)    
AS     
BEGIN
	
	SET NOCOUNT ON

	declare	@MagazineID int,  
			@count int,
			@row varchar(100)

	SET @count = 0

	create table #SubscriptionID (PubSubscriptionID int)  
	select @MagazineID = ProductID, @row = [ROW]  
	from reports 
	where reportID = @reportID 
  
	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode 
	from Pubs 
	where PubID = @MagazineID

	INSERT INTO #SubscriptionID   
	EXEC rpt_GetSubscriptionIDs_From_Filter @PublicationID, @CategoryIDs,
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

	CREATE TABLE #responseID (responseID VARCHAR(10), responsevalue varchar(100)) 
	
	IF @row = 'DEMO7'
		BEGIN
			INSERT INTO #responseID values ('A', 'A. Print Edition')
			INSERT INTO #responseID values ('B', 'B. Digital Edition')
			INSERT INTO #responseID values ('C', 'C. Both')
			INSERT INTO #responseID values ('O', 'O. Opt Out')
			INSERT INTO #responseID values ('Z', 'Z. No Response')
		END
	IF @row = 'DEMO3' or @row = 'DEMO4' or @row = 'DEMO5' 
		BEGIN
			INSERT INTO #responseID values ('Y', 'Yes')
			INSERT INTO #responseID values ('N', 'No')
		END
	IF @row = 'DEMO31' or @row = 'DEMO32' or @row = 'DEMO33' or @row = 'DEMO34' or @row = 'DEMO35' 
		BEGIN
			INSERT INTO #responseID values ('1', 'Yes')
			INSERT INTO #responseID values ('0', 'No')
		END
	
	IF @row = 'DEMO7'
		BEGIN
			EXEC('
			WITH INN (Description, ICount, IPercent)
			AS
			(
				Select ISNULL(r.responsevalue, ''Z. No Response'') as Description,
					SUM(ps.Copies) as ICount,
					convert(varchar(250),(SUM(ps.Copies) * 100.0 / (
					Select SUM(ps.Copies) FROM PubSubscriptions ps JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID)) * 1.0
					) + ''%'' as ''IPercent''
				FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				LEFT JOIN #responseID r ON r.responseID = ps.' + @row + '
				GROUP BY r.responsevalue
			) 
			Select INN.Description, IsNull(INN.ICount, 0) as ''Count'', ISNULL(INN.IPercent, 0) as ''Percent'' FROM INN ORDER BY INN.Description')
		END
	ELSE IF @row = 'DEMO31' or @row = 'DEMO32' or @row = 'DEMO33' or @row = 'DEMO34' or @row = 'DEMO35' or @row = 'DEMO3' or @row = 'DEMO4' or @row = 'DEMO5' 
		BEGIN
			EXEC('
			WITH INN (Description, ICount, IPercent)
			AS
			(
				Select r.responsevalue as Description,
					SUM(ps.Copies) as ICount,
					convert(varchar(250),(SUM(ps.Copies) * 100.0 / (
					Select SUM(ps.Copies) FROM PubSubscriptions ps JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID)) * 1.0
					) + ''%'' as ''IPercent''
				FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN Subscriptions s2 ON s2.SubscriptionID = ps.SubscriptionID
				JOIN #responseID r ON r.responseID = s2.' + @row + '
				GROUP BY r.responsevalue
			) 
			Select INN.Description, IsNull(INN.ICount, 0) as ''Count'', ISNULL(INN.IPercent, 0) as ''Percent'' FROM INN')
		END
	ELSE
		BEGIN
			EXEC('
			WITH INN (Description, ICount, IPercent)
			AS
			(
				Select ps.' + @row + ',
					SUM(ps.Copies) as ICount,
					convert(varchar(250),(SUM(ps.Copies) * 100.0 / (
					Select SUM(ps.Copies) FROM PubSubscriptions ps JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID)) * 1.0
					) + ''%'' as ''IPercent''
				FROM PubSubscriptions ps
				JOIN #SubscriptionID s ON s.PubSubscriptionID = ps.PubSubscriptionID
				JOIN Subscriptions s2 ON s2.SubscriptionID = ps.SubscriptionID
				GROUP BY ps.' + @row + '
			) 
			Select INN.Description, IsNull(INN.ICount, 0) as ''Count'', ISNULL(INN.IPercent, 0) as ''Percent'' FROM INN')
		END
	
	drop table #SubscriptionID 
	drop table #responseID

End