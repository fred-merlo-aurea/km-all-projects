CREATE Proc [dbo].[sp_CIRC_Import_Results_2]
(
	@PROCESSCODE VARCHAR(50)
)
as
BEGIN
	
	SET NOCOUNT ON

	DECLARE 
		@circpubID int,
		@yearTemp int,
		@startperiod varchar(10),
		@endperiod varchar(10),
		@startdateTemp datetime,
		@enddateTemp datetime,
		@dupeCount int = 0,
		@InvalidCount int  = 0,
		@IgnoreCount int  = 0
	
	Declare @PUBCODE VARCHAR(100) 
	
	select Distinct @PUBCODE = PUBCODE
	from SubscriberFinal with (NOLOCK)
	Where Processcode = @PROCESSCODE and Ignore = 0	
			
			
	SELECT @startperiod = p.YearStartDate , @endperiod = p.YearEndDate 
	FROM Pubs p 
	WHERE PubcODE = @PUBCODE
	
	select @circpubID = PublicationID 
	from Circulation..Publication 
	where PublicationCode = @PUBCODE

	select @dupeCount = COUNT(SubscriberArchiveID)
	from SubscriberArchive with (NOLOCK)
	Where Processcode = @PROCESSCODE
			
	select @InvalidCount = COUNT(SubscriberInvalidID)
	from SubscriberInvalid with (NOLOCK)
	Where Processcode = @PROCESSCODE
			
	select @IgnoreCount = COUNT(SubscriberFinalID)
	from SubscriberFinal with (NOLOCK)
	Where Processcode = @PROCESSCODE and Ignore = 1						

	IF getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
		SET @yearTemp = year(getdate()) 
	ELSE
		SET @yearTemp = year(getdate()) - 1

	SELECT @startdateTemp = @startperiod + '/' + convert(varchar,@yearTemp)
	SELECT @endDateTemp =  dateadd(ss, -1, dateadd(yy, 1, @startdateTemp)) 

	select  ps.pubID, p.pubcode, ps.pubsubscriptionID, sequenceID, ps.FirstName, ps.LastName, ps.Qualificationdate,
			c.CodeValue as Qsource, 
			v.CategoryCodeValue, v.CategoryCodeTypeName, v.TransactionCodeValue, v.TransactionCodeTypeName
	into #tmp_ProcessedRecords									 
	from SubscriberFinal sf with (NOLOCK) 
		join Subscriptions s with (NOLOCK) on sf.IGrp_No = s.IGRP_NO 
		join PubSubscriptions ps with (NOLOCK) on ps.SubscriptionID = s.SubscriptionID 
		join Pubs p with (NOLOCK) on p.PubID = ps.PubID and p.PubCode = sf.PubCode 
		left outer join UAD_Lookup..vw_Action v on v.CategoryCodeID = ps.PubCategoryID and v.TransactionCodeID = ps.PubTransactionID and ActionTypeID = 1 
		left outer join UAD_Lookup..Code c on c.CodeId = ps.PubQSourceID and CodeTypeId  = 19
	where processcode = @PROCESSCODE and IsUpdatedInLive = 1 and Ignore = 0
		
	select t.pubID, t.pubcode, t.pubsubscriptionID, t.sequenceID, t.FirstName, t.LastName, 
		t.Qsource, t.Qualificationdate Current_QDate, 
		(CASE 
				WHEN t.Qualificationdate between @startdateTemp and @endDateTemp THEN '1 Year'
				WHEN t.Qualificationdate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) THEN '2 Year'
				WHEN t.Qualificationdate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) THEN '3 Year'
				WHEN t.Qualificationdate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) THEN '4 Year'
				WHEN t.Qualificationdate < dateadd(yy, -4,  @endDateTemp ) THEN 'Older' 
		END) as 'Year', 
		t.CategoryCodeValue Current_CAT, 
		t.TransactionCodeValue Current_XACT, 
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then historyQsource else circ.SQSource end as	Previous_QSource,
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then historyQDAte else circ.QSourceDate end as	Previous_Qdate,
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then 
				(CASE 
						WHEN y.HistoryQDate between @startdateTemp and @endDateTemp THEN '1 Year'
						WHEN y.HistoryQDate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) THEN '2 Year'
						WHEN y.HistoryQDate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) THEN '3 Year'
						WHEN y.HistoryQDate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) THEN '4 Year'
						WHEN y.HistoryQDate < dateadd(yy, -4,  @endDateTemp ) THEN 'Older' 
				END)
			 else 
				(CASE 
						WHEN circ.QSourceDate between @startdateTemp and @endDateTemp THEN '1 Year'
						WHEN circ.QSourceDate between dateadd(yy, -1, @startdateTemp) and dateadd(yy, -1,  @endDateTemp ) THEN '2 Year'
						WHEN circ.QSourceDate between dateadd(yy, -2, @startdateTemp) and dateadd(yy, -2,  @endDateTemp ) THEN '3 Year'
						WHEN circ.QSourceDate between dateadd(yy, -3, @startdateTemp) and dateadd(yy, -3,  @endDateTemp ) THEN '4 Year'
						WHEN circ.QSourceDate < dateadd(yy, -4,  @endDateTemp ) THEN 'Older' 
				END)	 
			 end as	Previous_Year,
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then history_CAT else circ.CategoryCodeValue end as	Previous_CAT,
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then history_XACT else circ.TransactionCodeValue end as	Previous_XACT,
		case when historyQDAte is not null and historyQDAte >= circ.QSourceDate then history_TransactionCodeTypeName else Circ.TransactionCodeTypeName end as	Previous_TransactionCodeTypeName
	into #tmp_Results 	
	from #tmp_ProcessedRecords T
		LEFT OUTER JOIN
		(		
				select	s.SequenceID, s.QSourceDate, 
						a.CategoryCodeValue, a.CategoryCodeTypeName, a.TransactionCodeValue, a.TransactionCodeTypeName, c.CodeValue as SQSource
				From 
						CIRCULATION..SubSCRIPTION s with (NOLOCK) join 
						CIRCULATION..Subscriber sr with (NOLOCK) on s.subscriberID = sr.subscriberID join
						UAD_Lookup..vw_Action a on s.ActionID_Current = a.ActionID left outer join
						UAD_Lookup..Code c on c.CodeId = s.QSourceID and CodeTypeId  = 19
				where publicationID = @circpubID) circ ON 
		T.SequenceID = circ.SequenceID 
		LEFT outer join
		(
			select * from 
			(	
				select hs.SequenceID, hs.QualificationDate historyQDAte, c.CodeValue as historyQsource, v.CategoryCodeValue history_CAT, v.TransactionCodeValue history_XACT, v.TransactionCodeTypeName history_TransactionCodeTypeName, 
				ROW_NUMBER() over (partition by hs.sequenceID order  by hs.QualificationDate desc) as rown
				from HistorySubscription hs with (NOLOCK) join #tmp_ProcessedRecords t on hs.pubsubscriptionID = t.pubsubscriptionID  left outer join
				UAD_Lookup..vw_Action v with (NOLOCK) on v.CategoryCodeID = hs.PubCategoryID and v.TransactionCodeID = hs.PubTransactionID and ActionTypeID = 1 left outer join
				UAD_Lookup..Code c with (NOLOCK) on c.CodeId = hs.PubQSourceID and CodeTypeId  = 19
			where hs.QualificationDate <> t.Qualificationdate
			) h 
			where rown = 1
		)	Y on T.SequenceID = Y.SequenceID

	--select 
	--		Isnull(Previous_Year, 'New') as 'ChangedFrom', 
	--		YEAR,
	--		COUNT(PubsubscriptionID)  as Counts
	--from #tmp_Results
	--where Previous_TransactionCodeTypeName is null or Previous_TransactionCodeTypeName not like '%InActive%'
	--group by 
	--		Isnull(Previous_Year, 'New'), 
	--		YEAR,
	--		Previous_TransactionCodeTypeName
	--union 
	--select 
	--		'Inactive', 
	--		'1 year',
	--		COUNT(PubsubscriptionID)
	--from #tmp_Results
	--		where Previous_TransactionCodeTypeName like '%InActive%'
	--union
	--select 'Dupes', '', @dupeCount
	--union
	--select 'Invalid', '', @InvalidCount
	--union
	--select 'Ignored', '', @IgnoreCount
	--order by 2 desc, 1

	select Isnull(Previous_Year, 'New') 'ChangedFrom', 
		YEAR, 
		Previous_cat as Previous_Categorycode, 
		Previous_XACT as Previous_TransactionCode,
		Previous_Qsource as Previous_QSource,
		Current_cat as Current_CategoryCode,
		Current_xact as Current_TransactionCode,
		COUNT(PubsubscriptionID) as Counts
	from #tmp_Results
	group by Isnull(Previous_Year, 'New'), 
		YEAR, 
		Previous_cat,
		Previous_XACT, 
		Previous_Qsource,
		Current_cat,
		Current_xact
	order by 1,5,4,6,7

	drop table #tmp_ProcessedRecords

	drop table #tmp_Results

End