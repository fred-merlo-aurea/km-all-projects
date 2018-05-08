CREATE proc [dbo].[rpt_QualificationBreakdown_Old]
(
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
	@PrintColumns varchar(4000),
	@Download char(1),
	@WaveMail varchar(100) = ''  
)
as
Begin 
	/* TESTING */
	--Declare @ProductID int = 1,
	--@CategoryIDs varchar(800) = '',
	--@CategoryCodes varchar(800) = '',
	--@TransactionIDs varchar(800) = '',
	--@TransactionCodes varchar(800) = '',
	--@QsourceIDs varchar(800) = '',
	--@StateIDs varchar(800) = '',
	--@Regions varchar(max) = '',
	--@CountryIDs varchar(800) = '',
	--@Email varchar(10) = '',
	--@Phone varchar(10) = '',
	--@Fax varchar(10) = '',
	--@Mobile varchar(10) = '',
	--@ResponseIDs varchar(800) = '',
	--@Demo7 varchar(10) = '',		
	--@Year varchar(20) = '',
	--@startDate varchar(10) = '',		
	--@endDate varchar(10) = '',
	--@AdHocXML varchar(8000) = '', 
	--@years int = 0, 
	--@PrintColumns varchar(4000) = '',   
	--@Download bit = 0   


	SET NOCOUNT ON
	
	declare @PublicationID int
	declare @GetSubscriberIDs bit = 0
	
	set @PublicationID = @ProductID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = PubCode 
	from Pubs 
	where PubID = @ProductID
	
	create table #SubscriptionID (SubscriptionID int, Copies int)  
		
	Insert into #SubscriptionID   
	exec rpt_GetSubscriptionIDs_Copies_From_Filter  
		@PublicationID, 
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
	
	DECLARE @qSourceCodeTypeID int
	set @qSourceCodeTypeID = (SELECT CodeTypeID FROM UAD_Lookup..CodeType WHERE CodeTypeName = 'Qualification Source')
	
	SELECT CodeId, CodeName, DisplayName, CodeValue, ParentCodeId, CodeTypeId
	INTO #QSource
	FROM UAD_Lookup..Code
	WHERE CodeTypeId = @qSourceCodeTypeID
	
	DECLARE @id int	
	
	INSERT INTO #QSource (CodeName, DisplayName, CodeValue, ParentCodeId, CodeTypeId)
	VALUES('No Response', 'ZZ. No Response', 'ZZ', 0, @qSourceCodeTypeID) SELECT @id = @@IDENTITY
	
	if @download = '0'
		Begin
			declare @yearTemp int,
					@i int,
					@y int,
					@sqlstring varchar(4000),
					@startperiod varchar(10),
					@endperiod varchar(10),
					@startdateTemp datetime,
					@enddateTemp datetime

			select @startperiod = p.YearStartDate , @endperiod = p.YearEndDate from Pubs p where PubID = @PublicationID

			set @i = 0
			set @sqlstring = ''

			if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
				set @yearTemp = year(getdate()) 
			else
				set @yearTemp = year(getdate()) - 1

			select @startdateTemp = @startperiod + '/' + convert(varchar,@yearTemp)
			select @endDateTemp =  dateadd(ss, -1, dateadd(yy, 1, @startdateTemp) ) 
		
			print @startdateTemp
			print @endDateTemp
		
			select	ISNULL(q2.DisplayName, 'No Response') AS QsourceGroup, 
					ISNULL(q.displayname, 'ZZ. No Response') as Qsource, isnull(Column0,0) as "1 Year",
					isnull(Column1,0) as "2 Year", 
					isnull(Column2,0) as "3 Year", 
					isnull(Column3,0) as "4 Year", 
					isnull(Column4,0) as Older, 
					isnull(Qualified_Nonpaid,0) as "Qualified Non Paid", 
					isnull(Qualified_Paid,0) as "Qualified Paid" ,
					isnull(Column0,0) + isnull(Column1,0) + isnull(Column2,0) + isnull(Column3,0) + isnull(Column4,0) as "Total"
			from #QSource q 
				left join UAD_Lookup..Code q2 on q2.CodeId = q.ParentCodeId 
				left join UAD_Lookup..CodeType qst with (NOLOCK) on q.CodeTypeID = qst.CodeTypeID 
				left outer join  --QualificationSourceType
					(  
						select	(CASE WHEN ISNULL(PubQSourceID, 0) in (0,-1,'') THEN 1891 ELSE PubQSourceID END) as PubQSourceID, 
							isnull(sum(Case when ps.Qualificationdate between @startdateTemp and @endDateTemp then ps.copies end),0) AS Column0, 
							isnull(sum(Case when ps.Qualificationdate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) then ps.copies end),0) AS Column1, 
							isnull(sum(Case when ps.Qualificationdate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) then ps.copies end),0) AS Column2, 
							isnull(sum(Case when ps.Qualificationdate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) then ps.copies end),0) AS Column3, 
							isnull(sum(Case when ps.Qualificationdate < dateadd(yy, -4,  @endDateTemp ) then ps.copies end),0) AS Column4, 
							sum(case when cct.CategoryCodeTypeName = 'QUALIFIED FREE' then ps.copies end) as Qualified_Nonpaid,  
							sum(case when cct.CategoryCodeTypeName = 'QUALIFIED PAID' then ps.copies end) as Qualified_Paid  
						From #SubscriptionID s with (NOLOCK) 
							join PubSubscriptions ps with (NOLOCK) on s.SubscriptionID = ps.SubscriptionID 
							left join UAD_Lookup..categorycode cc  with (NOLOCK) ON cc.categorycodeid = ps.PubCategoryID	
							left JOIN UAD_Lookup..categorycodetype cct with (NOLOCK) ON cct.categorycodetypeid = cc.categorycodetypeid		
						Where ps.PubID =  @PublicationID  
						group by (CASE WHEN ISNULL(PubQSourceID, 0) in (0,-1,'') THEN 1891 ELSE PubQSourceID END)
					) as temp1 on temp1.PubQSourceID = q.CodeId 
			where qst.CodeTypeId = @qSourceCodeTypeID
			Order by 1, 2	
		end
	
	drop table #SubscriptionID 	
	drop table #QSource		

end