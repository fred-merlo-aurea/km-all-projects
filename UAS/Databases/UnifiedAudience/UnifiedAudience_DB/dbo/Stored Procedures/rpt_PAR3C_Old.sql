CREATE proc [dbo].[rpt_PAR3C_Old]
(    
	@ReportID int,
	@ProductID int, 
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
	@WaveMail varchar(100) = ''
)    
as     
	--DECLARE 
	--@ReportID int = 1523,
	--@ProductID int = 1, 
	--@CategoryIDs varchar(800),
	--@CategoryCodes varchar(800),
	--@TransactionIDs varchar(800),
	--@TransactionCodes varchar(800),
	--@QsourceIDs varchar(800),
	--@StateIDs varchar(800),
	--@Regions varchar(max),
	--@CountryIDs varchar(1500),
	--@Email varchar(10),
	--@Phone varchar(10),
	--@Mobile varchar(10),
	--@Fax varchar(10),
	--@ResponseIDs varchar(1000),
	--@Demo7 varchar(50),		
	--@Year varchar(20),
	--@startDate varchar(10),		
	--@endDate varchar(10),
	--@AdHocXML varchar(8000)

Begin
	
	declare	@MagazineID int,  
			@count int

	SET NOCOUNT ON

	set @count = 0

	create table #SubscriptionID (SubscriptionID int, copies int)  
	select @MagazineID = ProductID  
	from reports 
	where reportID = @reportID  
  
	declare @magazineCode varchar(20)
	select @magazineCode  = PubCode 
	from Pubs 
	where PubID = @ProductID
	
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_Copies_From_Filter 
		@ProductID, 
		@CategoryIDs,
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
	
	DECLARE @Par3CID int = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Par3C')
	DECLARE @NoResponse int
	
	SELECT CodeID as responseID, DisplayName as responsevalue
	INTO #responseID
	FROM UAD_Lookup..Code
	WHERE CodeTypeId = @Par3CID

	insert into #responseID values ('ZZ. No Response') SELECT @NoResponse = @@IDENTITY

	declare @total int
	
	select @total = SUM(s.Copies) 
	FROM PubSubscriptions ps 
		JOIN #SubscriptionID s ON s.SubscriptionID = ps.SubscriptionID 
	WHERE PubID = @ProductID

	select ISNULL(r.responsevalue, 'ZZ. No Response'),
		isnull(SUM(case when c.CategoryCodeTypeID = 1 then s.COPIES end),0) as 'Qualified Non Paid',        
		isnull(SUM(case when c.CategoryCodeTypeID = 3 then s.COPIES end),0) as 'Qualified Paid',
		isnull(SUM(case when c.CategoryCodeTypeID in (1,3) then s.COPIES end),0) as 'Total Qualified',
		convert(varchar(100),((SUM(case when c.CategoryCodeTypeID in (1,3) then s.Copies else 0 end) * 100.0)/
		(@total) * 1.0)) + '%' as 'Percent'
	From #SubscriptionID sf 
		join PubSubscriptions s on sf.SubscriptionID = s.SubscriptionID and PubID = @ProductID 
		left join UAD_Lookup..CategoryCode c on c.CategoryCodeID = s.PubCategoryID 
		right outer join #responseID r on r.responseID = ISNULL(NULLIF(s.Par3CID, ''),@NoResponse)
	group by r.responsevalue
	order by r.responsevalue

	drop table #SubscriptionID
	drop table #responseID

End