CREATE proc [dbo].[sp_rptQualificationBreakdown]
(
	@MagazineID int,
	@years int,  
	@Filters TEXT,
	@PrintColumns varchar(4000),
	@Download char(1),
	@OnlyCount bit = 0  ,
	@SubscriptionID  XML    
)
as
Begin 

	set nocount on
	
	declare @publicationID int
	
	set @publicationID = @MagazineID
	
	declare @PublicationCode varchar(20)
	select @PublicationCode  = Publicationcode from Publication where PublicationID = @PublicationID
	
	if len(ltrim(rtrim(@PrintColumns))) > 0 
	Begin
		set @PrintColumns  = ', ' + @PrintColumns 
	end

	create table #SubscriptionID (SubscriptionID int, copies int)  
	
	Insert into #SubscriptionID   
	 exec sp_getSubscribers_using_XMLFilters @PublicationID, @Filters, 1 

	CREATE UNIQUE CLUSTERED INDEX IX_1 on #SubscriptionID (SubscriptionID)
	
	if @download = '0'
	Begin
		declare @year int,
				@i int,
				@y int,
				@sqlstring varchar(4000),
				@startperiod varchar(10),
				@endperiod varchar(10),
				@startdate datetime,
				@enddate datetime

		select @startperiod = p.YearStartDate , @endperiod = p.YearEndDate from Publication p where PublicationID = @publicationID

		set @i = 0
		set @sqlstring = ''

		if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
			set @year = year(getdate()) 
		else
			set @year = year(getdate()) - 1

		select @startdate = @startperiod + '/' + convert(varchar,@year)
		select @endDate =  dateadd(yy, 1, @startdate) 

		while (@i < 5)
		Begin
			if @i < @years
			Begin
				set @y = @year-@i
				set @sqlstring = @sqlstring +  ' isnull(sum(Case when s.QSourceDate between ''' + convert(varchar(20), @startdate, 120) +''' and ''' + convert(varchar(20),dateadd(ss, -1,@endDate ), 120) +''' then s.copies end),0) AS Column'+ convert(varchar,@i)  + ','
				
				set @startdate = dateadd(yy, -1, @startdate) 
				set @endDate =  dateadd(yy, -1, @endDate) 
								
			end
			else if @i = 4
			Begin
				set @sqlstring = @sqlstring + ' isnull(sum(Case when s.QSourceDate < ''' + convert(varchar(20), @endDate, 120) +''' then s.copies end),0) AS Column4' + ','
			end
			else
			Begin
				set @sqlstring = @sqlstring +  ' 0 AS Column' + convert(varchar,@i) + ','
			end
			select @i = @i + 1
		end 

		exec(' select	qg.displayname as QsourceGroup, q.displayname as Qsource, isnull(Column0,0) as Column0, isnull(Column1,0) as Column1, isnull(Column2,0) as Column2, isnull(Column3,0) as Column3, isnull(Column4,0) as Column4, isnull(Qualified_Nonpaid,0) as Qualified_Nonpaid, isnull(Qualified_Paid,0) as Qualified_Paid ' + 
			 ' from dbo.QualificationSourceType qg join dbo.QualificationSource q  on qg.QSourceTypeID = Q.QSourceTypeID left outer join ' + 
			'( ' + 
				'select s.QSourceID, ' + @sqlstring + 
						' sum(case when cct.CategoryCodeTypeName = ''Qualified Free'' then s.copies end) as Qualified_Nonpaid, ' + 
						' sum(case when cct.CategoryCodeTypeName = ''Qualified Paid'' then s.copies end) as Qualified_Paid ' + 
			 ' From ' +   
				' Subscription s join ' +
				' #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join  ' +
				' Subscriber sb on s.SubscriberID = sb.subscriberID join ' +
				' Action a on a.ActionID = s.ActionID_Current join ' +
				' CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join ' +
				' CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID join ' +
				' TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID  ' +
				
			 ' Where ' +
				' s.publicationID = ' + @publicationID + 
			  ' group by  s.QSourceID ) as temp1 on temp1.QSourceID = q.QSourceID ')
			 
	end
	else
	begin
		if (@OnlyCount = 1)
		begin
			
			select 
					 DISTINCT s.subscriptionid 
			From 
					Subscription s join
					#SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID  join 
					Subscriber sb on s.SubscriberID = sb.subscriberID join
					Action a on a.ActionID = s.ActionID_Current join
					CategoryCode cc on cc.CategoryCodeID  = a.CategoryCodeID join
					CategoryCodeType cct on cct.CategoryCodeTypeID = cc.CategoryCodeTypeID join
					TransactionCode tc on tc.TransactionCodeID = a.TransactionCodeID
			where 
					PublicationID = @PublicationID			 
		end
		else
		begin
			IF OBJECT_ID('tempdb..#tmpSubscriptionID') IS NOT NULL 
				BEGIN 
						DROP TABLE #tmpSubscriptionID;
				END 
				      
				CREATE TABLE #tmpSubscriptionID (subsID int); 

				insert into #tmpSubscriptionID
							SELECT SubscriptionID.ID.value('./@SubscriptionID','INT')FROM @SubscriptionID.nodes('/Subscriptions') as SubscriptionID(ID) ;	
			
			
				exec (	' select  distinct mg.Magazine_Code as pubcode,s.SubscriberID, s.SubscriptionID, s.EMAILADDRESS, s.FNAME, s.LNAME, s.COMPANY, s.TITLE, s.ADDRESS, s.MAILSTOP, s.CITY, s.STATE, s.ZIP, s.PLUS4, s.COUNTRY, s.PHONE, s.FAX, s.website, s.Category_ID as CAT, c.Category_Name as CategoryName, s.Transaction_ID as XACT, s.Transaction_Date as XACTDate, s.FORZIP, s.COUNTY, s.QSource_ID, q.Qsource_Name + '' (''+ q.Qsource_value + '')'' as Qsource, s.QSourceDate, s.Subsrc, s.copies, spt.PriceCode, spt.Term, spt.StartIssueDate, spt.ExpireIssueDate, spt.CPRate, spt.Amount, spt.AmountPaid, spt.BalanceDue, spt.PaidDate, spt.TotalIssues, p.paymenttype, ' +
						' case  when StartIssueDate > GETDATE() then TotalIssues
								when StartIssueDate < GETDATE() and expireissuedate > getdate() then  DATEDIFF(MM, dateadd(mm,1,convert(varchar,MONTH(getdate())) + ''/01/'' + convert(varchar,Year(getdate()))), expireissuedate) + 1   
								when  expireissuedate  < getdate()  then 0    end as  isssuestogo, spt.CheckNumber,spt.CCNumber,spt.CCExpirationMonth,spt.CCEXpirationYear,spt.CCHolderName   ' +
						@PrintColumns + 
						' from Subscriptions s join #SubscriptionID sf on sf.SubscriptionID = s.SubscriptionID left outer join QSource q on q.Qsource_ID = s.Qsource_ID join Category C on s.Category_ID = C.Category_ID join ' +
						' Categorygroup cg on cg.Categorygroup_ID = C.Categorygroup_ID join [Transaction] t on s.Transaction_ID = t.Transaction_ID join ' + 
						' Publication mg on mg.magazineID = s.magazineID ' + 
						' join #tmpSubscriptionID tmp on s.subscriptionID = tmp.subsID ' +
						' left outer join SubscriberPaidTransaction spt on spt.SubscriptionID = s.subscriptionID ' +
						' left outer join PaymentsType p on  p.PaymentTypeID = spt.PaymentTypeID ' +
						' where s.magazineID = ' + @MagazineID )	--+ ' and c.CategoryGroup_ID in (1,2)  and  t.TransactionGroup_ID=1'
		end			
	end
	
	drop table #SubscriptionID 
		
end