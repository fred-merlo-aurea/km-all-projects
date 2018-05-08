
CREATE Procedure [dbo].[MAF_DomainTrackingSYNC] 
(
	@database varchar(50) ,
	@CustomerID int = 0, 
	@basechannelID int =0 
)	

AS
BEGIN
	declare --@groupID int,
			@domainTrackerID int,
			@domain varchar(50),
			@lastsyncDate datetime,
			@sql nvarchar(1000),
			@MAFdomaintrackingID int

SET NOCOUNT ON

	set @sql = N'SET @ldt = (select MAX(ActivityDate) from [10.10.41.251].' + @database + '.dbo.subscribervisitactivity)'
	execute sp_executesql @sql, N'@ldt datetime output', @ldt=@lastsyncDate output

	--select @lastsyncDate

	create table #customers (CustomerID int)

	if @customerID > 0
	BEGIN
		insert into #customers
		select @customerID
	END
	Else if @basechannelID > 0
	BEGIN
		insert into #customers
		select 
			customerID 
		from 
			ECN5_ACCOUNTS..Customer c 
		Where 
			BaseChannelID = @basechannelID
	END

--	select * from #customers

	if exists (select top 1 CustomerID from #customers)
	BEGIN

		create table #tmpDT (domaintrackingID int, domainname varchar(200))
		CREATE INDEX IDX_tmpDT_domainname ON #tmpDT(domainname)
				
		create table #tmpDTdata (domaintrackingID int, emailaddress varchar(100), URL varchar(500), TimeStamp datetime)
		CREATE INDEX IDX_tmpDTdata_emailaddress ON #tmpDTdata(emailaddress)

		exec ('
		insert into [10.10.41.251].' + @database + '.dbo.DomainTracking (domainname)
		select 
		lower(replace(replace(replace(domain,''https://'',''''), ''http://'',''''), ''/'','''')) as  Domain 
		from 
			ECN5_ACCOUNTS..Customer c 
			join ECN5_DomainTracker.dbo.DomainTracker dc on c.CustomerID = dc.CustomerID 
			join #Customers c1 on c1.CustomerID = c.CustomerID
			left outer join [10.10.41.251].' + @database + '.dbo.DomainTracking dt on dt.domainname = lower(replace(replace(replace(domain,''https://'',''''), ''http://'',''''), ''/'','''')) 
		where dt.domaintrackingID is null;')

		exec( 'insert into #tmpDT select domaintrackingID, domainname from [10.10.41.251].' + @database + '.dbo.DomainTracking')

		--select * from #tmpDT

		if exists (select top 1 * from #tmpDT)
		BEGIN

			select 
				@domainTrackerID = min( domainTrackerID ) 
			from 
				ECN5_ACCOUNTS..Customer c 
				join ECN5_DomainTracker.dbo.DomainTracker dc on c.CustomerID = dc.CustomerID 
				join #Customers c1 on c1.CustomerID = c.CustomerID

			while @domainTrackerID is not null
			BEGIN

				select 
					@customerID = customerID, 
					@domainTrackerID = DomainTrackerId,
					--@groupID = groupID, 
					@domain = lower(replace(replace(replace(domain,'https://',''), 'http://',''), '/','')) 
				from 
					ECN5_DomainTracker.dbo.DomainTracker 
				where DomainTrackerID = @domainTrackerID
			    
				select @MAFdomaintrackingID= domaintrackingID from #tmpDT where domainname = @domain
			    
			    
			  --  select * from #tmpdt
--			    select * from #tmpDTdata
			    
				insert into #tmpDTdata
				SELECT	
					@MAFdomaintrackingID,
					emails.emailaddress, 
					innertable2.URL,
					innertable2.TimeStamp 
				FROM
					emails  with (NOLOCK) 
					JOIN (SELECT InnerTable1.emailid AS tmp_EmailID, 
									DomainTrackerActivityId,
									Max([url])          AS 'URL', 
									Max([timestamp])    AS 'TimeStamp'
							 FROM   (	select EmailId,PageURL as URL,Timestamp  ,
							 DomainTrackerActivityId
										from
											ECN5_DomainTracker.dbo.DomainTrackerActivity dta WITH (NOLOCK)
											JOIN ECN5_DomainTracker.dbo.DomainTrackerUserProfile up WITH (NOLOCK) on dta.ProfileID = up.ProfileID
											JOIN ECN5_COMMUNICATOR.dbo.Emails e WITH (NOLOCK) on up.EmailAddress = e.EmailAddress
										Where 
											--GroupID = @groupID 
											dta.DomainTrackerID = @DomainTrackerId
											and (@lastsyncDate is null or dta.Timestamp > @lastsyncDate)
									 ) AS InnerTable1 
							 GROUP  BY 
								InnerTable1.emailid,
								DomainTrackerActivityId
																	 ) 
								 AS InnerTable2 ON emails.emailid = InnerTable2.tmp_emailid 
						JOIN emailgroups  with (NOLOCK) ON emailgroups.emailid = emails.emailid 
				WHERE  
					emails.customerid = @customerID 
				--	AND emailgroups.groupid = @groupID 
			    
			    
				select 
					@domainTrackerID = min( domainTrackerID ) 
				from 
					ECN5_ACCOUNTS..Customer c 
					join ECN5_DomainTracker.dbo.DomainTracker dc on c.CustomerID = dc.CustomerID 
					join #Customers c1 on c1.CustomerID = c.CustomerID
				where 
					domainTrackerID > @domainTrackerID
			END

			if exists (select top 1 domaintrackingID from #tmpDTdata)
			BEGIN
			
				select COUNT(*) from #tmpDTdata 
				--select distinct t.emailaddress from [10.10.41.251].stamatsMasterDB.dbo.Subscriptions s with (NOLOCK) join #tmpDTdata t with (NOLOCK)  on s.email = t.emailaddress order by emailaddress
				
				exec ('
				insert into [10.10.41.251].' + @database + '.dbo.SubscriberVisitActivity (SubscriptionID, DomainTrackingID, URL, ActivityDate)
				select s.subscriptionID, t.domaintrackingID, t.URL, t.TimeStamp 
				from [10.10.41.251].' + @database + '.dbo.Subscriptions s with (NOLOCK) join #tmpDTdata t with (NOLOCK)  on s.email = t.emailaddress 
				GROUP BY s.subscriptionID, t.domaintrackingID, t.URL, t.TimeStamp
				order by t.TimeStamp')
			END
		END
		
		drop table  #tmpDT
		drop table  #tmpDTdata 
		
	END

	drop table  #customers 

END

