CREATE proc [dbo].[rpt_DemoXQualification_Old]
(
	@ReportID int,  
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
	@Download bit,
	@WaveMail varchar(100) = ''      
)
as
	--DECLARE @ReportID int = 1523,  
	--@PublicationID int = 1,
	--@CategoryIDs varchar(800) = '',
	--@CategoryCodes varchar(800) = '',
	--@TransactionIDs varchar(800) = '',
	--@TransactionCodes varchar(800) = '',
	--@QsourceIDs varchar(800) = '',
	--@StateIDs varchar(800) = '',
	--@Regions varchar(max) = '',
	--@Mobile varchar(10) = '',
	--@CountryIDs varchar(800) = '',
	--@Email varchar(10) = '',
	--@Phone varchar(10) = '',
	--@Fax varchar(10) = '',
	--@ResponseIDs varchar(800) = '',
	--@Demo7 varchar(10) = '',		
	--@Year varchar(20) = '',
	--@startDate varchar(30) = '',--'01/01/2015',		
	--@endDate varchar(30) = '',--'06/16/2015',
	--@AdHocXML varchar(8000) = '', 
	--@PrintColumns varchar(4000) = '',   
	--@Download bit = 0 
	
BEGIN
	
	SET NOCOUNT ON
	
	declare @Row varchar(50),  						
		@sqlstring varchar(4000),
		@startperiod varchar(30),
		@ProductCode varchar(20)			
			
	if len(ltrim(rtrim(@PrintColumns))) > 0 
		Begin
			set @PrintColumns  = ', ' + @PrintColumns 
		end	

	create table #SubscriptionID (SubscriptionID int, copies int)  
	
	select @Row = Row 
	from Reports 
	Where ReportID = @ReportID
			
	select @ProductCode = PubCode 
	from Pubs 
	where PubID = @PublicationID			
	
	select @startperiod = YearStartDate 
	from Pubs 
	where PubID = @PublicationID
	
	print @startdate
	print  @enddate
	
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
	
	if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
		set @year = year(getdate()) 
	else
		set @year = year(getdate()) - 1	

	if len(@startDate) = 0
		set @startdate = ltrim(rtrim(@startperiod)) + '/' + convert(varchar,@year)
		
	if len(@endDate) = 0
		set @endDate = convert(varchar(10), dateadd(ss, -1, dateadd(yy, 1, convert(datetime,@startdate))), 101)

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	if @download = '0'
		Begin
		
			declare @media table (demo7 char(1), DisplayName varchar(10))
		
			insert @media values ('A', 'Print')
			insert @media values ('B', 'Digital')
			insert @media values ('C', 'Unique');	
		
			WITH report_CTE (CodeSheetID, Responsedesc, DisplayOrder,
				[Print 1 yr],[Digital 1 yr], [Unique 1 yr],
				[Print 2 yr],[Digital 2 yr], [Unique 2 yr],
				[Print 3 yr],[Digital 3 yr], [Unique 3 yr],
				[Print 4 yr],[Digital 4 yr], [Unique 4 yr],
				[Print 4+ yr],[Digital 4+ yr],[Unique 4+ yr]	
			)
			as
			(
				SELECT	inn1.CodeSheetID, inn1.Responsedesc, inn1.DisplayOrder,
						ISNULL(inn1.[Print 1 yr], 0),  ISNULL(inn1.[Digital 1 yr], 0),  ISNULL(inn1.[Unique 1 yr], 0),
						ISNULL(inn1.[Print 2 yr], 0),  ISNULL(inn1.[Digital 2 yr], 0),  ISNULL(inn1.[Unique 2 yr], 0),
						ISNULL(inn1.[Print 3 yr], 0),  ISNULL(inn1.[Digital 3 yr], 0),  ISNULL(inn1.[Unique 3 yr], 0),
						ISNULL(inn1.[Print 4 yr], 0),  ISNULL(inn1.[Digital 4 yr], 0),  ISNULL(inn1.[Unique 4 yr], 0),
						ISNULL(inn1.[Print 4+ yr], 0), ISNULL(inn1.[Digital 4+ yr], 0), ISNULL(inn1.[Unique 4+ yr], 0)		
				 from
				--(select isnull(CodeSheetID, 0) as CodeSheetID, isnull(Responsevalue + ' - ' +  Responsedesc, 'ZZ - NO RESPONSE') as Responsedesc from CodeSheet where PubID = @PublicationID and ResponseGroup = @Row) inn2
				--left outer join 
				--(
					(select	r.CodeSheetID, Responsevalue + ' - ' +  Responsedesc as Responsedesc, r.DisplayOrder,
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 1 yr',
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 1 yr',
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 1 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 4 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 4 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 4 yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 4+ yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 4+ yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 4+ yr'
				From PubSubscriptions s 
					join #SubscriptionID s1 on s.SubscriptionID = s1.SubscriptionID 
					join PubSubscriptionDetail sd on sd.PubSubscriptionID = s.PubSubscriptionID 
					join CodeSheet r on sd.CodeSheetID = r.CodeSheetID and r.ResponseGroup = @Row and r.PubID = @PublicationID
				Where s.PubID = @PublicationID
				group by  r.CodeSheetID, r.Responsedesc, r.Responsevalue, r.DisplayOrder
				UNION ALL
					(select	0 as CodeSheetID,'ZZ - NO RESPONSE' as Responsedesc, 99 as DisplayOrder,
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 1 yr',
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 1 yr',
						isnull(SUM(case when (s.qualificationDate between @startdate and  @endDate) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 1 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -1, @startdate) and DATEADD(yy, -1, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 2 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -2, @startdate) and DATEADD(yy, -2, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 3 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 4 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 4 yr',
						isnull(SUM(case when (s.qualificationDate between DATEADD(yy, -3, @startdate) and DATEADD(yy, -3, @endDate)) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 4 yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='A' or Demo7='C') then s1.copies end),0) as 'Print 4+ yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='B' or Demo7='C') then s1.copies end),0) as 'Digital 4+ yr',
						isnull(SUM(case when (s.qualificationDate <= DATEADD(yy, -4, @endDate) or s.qualificationDate is null) and (Demo7='A' or Demo7='B' or Demo7='C') then s1.copies end),0) as 'Unique 4+ yr'
				From PubSubscriptions s 
					join #SubscriptionID s1 on s.SubscriptionID = s1.SubscriptionID
				WHERE s.PubID = @PublicationID
					   AND NOT EXISTS 
					   (
							  SELECT 1 
							  FROM 
									 PubSubscriptionDetail psd 
									 JOIN CodeSheet cs ON cs.CodeSheetID = psd.CodesheetID 
							  WHERE
									 cs.ResponseGroup = @Row
									 AND psd.PubSubscriptionID = s.PubSubscriptionID)
					  )
				) inn1--) inn1 on inn1.CodeSheetID = inn2.CodeSheetID 
			)
			select Responsedesc,
					[Print 1 yr],[Digital 1 yr], [Unique 1 yr],
					[Print 2 yr],[Digital 2 yr], [Unique 2 yr],
					[Print 3 yr],[Digital 3 yr], [Unique 3 yr],
					[Print 4 yr],[Digital 4 yr], [Unique 4 yr],
					[Print 4+ yr],[Digital 4+ yr],[Unique 4+ yr] ,
					[Print 1 yr] + [Print 2 yr] + [Print 3 yr] + [Print 4 yr] + [Print 4+ yr] "Total Print",
					[Digital 1 yr] + [Digital 2 yr] + [Digital 3 yr] + [Digital 4 yr] + [Digital 4+ yr]  "Total Digital",
					[Unique 1 yr] + [Unique 2 yr] + [Unique 3 yr] + [Unique 4 yr] + [Unique 4+ yr] "Total Unique",
					1 as sort
			from report_CTE
			union
			select 'Total' as Responsdesec,
					SUM([Print 1 yr]), SUM([Digital 1 yr]),  SUM([Unique 1 yr]),
					SUM([Print 2 yr]),  SUM([Digital 2 yr]),  SUM([Unique 2 yr]),
					SUM([Print 3 yr]),  SUM([Digital 3 yr]),  SUM([Unique 3 yr]),
					SUM([Print 4 yr]),  SUM([Digital 4 yr]),  SUM([Unique 4 yr]),
					SUM([Print 4+ yr]), SUM([Digital 4+ yr]), SUM([Unique 4+ yr]) ,
					SUM([Print 1 yr] + [Print 2 yr] + [Print 3 yr] + [Print 4 yr] + [Print 4+ yr]) "Total Print",
					SUM([Digital 1 yr] + [Digital 2 yr] + [Digital 3 yr] + [Digital 4 yr] + [Digital 4+ yr])  "Total Digital",
					SUM([Unique 1 yr] + [Unique 2 yr] + [Unique 3 yr] + [Unique 4 yr] + [Unique 4+ yr]) "Total Unique",
					2 as sort
			from report_CTE
			order by sort	

		end

	drop table #SubscriptionID 
			
end