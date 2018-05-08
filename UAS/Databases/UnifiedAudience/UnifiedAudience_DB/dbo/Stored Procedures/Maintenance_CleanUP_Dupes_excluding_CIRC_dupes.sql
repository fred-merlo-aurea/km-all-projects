CREATE procedure [dbo].[Maintenance_CleanUP_Dupes_excluding_CIRC_dupes]
as
Begin

	set nocount on

	declare @loopcounter int = 1,
			@distinctcount int

	declare @tmp table (SubscriptionID int UNIQUE CLUSTERED (SubscriptionID))	
	create table #tmp_dupes (ID int, loop int, 	KeepSubscriptionID int, DupeSubscriptionID int)
	
	CREATE INDEX IDX_tmp_dupes_KeepSubscriptionID ON #tmp_dupes(KeepSubscriptionID)
	CREATE INDEX IDX_tmp_dupes_DupeSubscriptionID ON #tmp_dupes(DupeSubscriptionID)

	create table #curtable (KeepSubscriptionID int, DupeSubscriptionID int, rnk int)	
	CREATE INDEX IDX_curtable_KeepSubscriptionID ON #curtable(KeepSubscriptionID)

	create table #delSubscriptionIDs (SubscriptionID int)	
	CREATE INDEX IDX_Users_UserName ON #delSubscriptionIDs(SubscriptionID)

	if exists (select * from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'tmp_duplicates' AND TABLE_SCHEMA = 'dbo')
	Begin
		delete from tmp_duplicates;
	End
	Else
	Begin
		Create table tmp_duplicates (ID int, matchno int, SubscriptionID int, igrp_no uniqueidentifier, fname varchar(100), lname varchar(100), title varchar(255), company varchar(255), 
		address varchar(255), city varchar(50), state varchar(50), zip varchar(50), country varchar(255), email varchar(255), phone varchar(50), score int, priority int, lastchanged datetime)
		
		CREATE NONCLUSTERED INDEX IX_tmp_duplicates_matchno_ID ON dbo.tmp_duplicates
		(
		matchno, ID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

		CREATE NONCLUSTERED INDEX IX_tmp_duplicates_SubscriptionID ON dbo.tmp_duplicates
		(
		SubscriptionID
		) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

	End
		
	if (DB_NAME() <> 'SOURCEMEDIAMASTERDB' and DB_NAME() <> 'HAPPYDAYMEDIAMASTERDB' and DB_NAME() <> 'MEISTERQUARTERLYMASTERDB')
	BEGIN
	
		while @loopcounter <= 11
		Begin

			delete from #curtable

			if @loopcounter = 1 -- MATCH ON COMLETE FNAME,LNAME,COMPANY, ADDRESS, CITY, STATE, ZIP, PHONE, EMAIL
			Begin
				insert into #curtable
				select  minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
					select isnull(FNAME,'') FNAME, isnull(LNAME,'') LNAME, isnull(COMPANY,'') COMPANY, isnull(ADDRESS,'') ADDRESS, isnull(CITY,'')CITY, isnull(State,'')State, 
					isnull(ZIP,'') ZIP, isnull(PHONE,'') PHONE, isnull(EMAIL,'') EMAIL, COUNT(subscriptionID) as dupecount, MIN(subscriptionID) as minsubscriptionID
					from Subscriptions with (NOLOCK)
					group by
					isnull(FNAME,''), isnull(LNAME,''), isnull(COMPANY,''), isnull(ADDRESS,''), isnull(CITY,''), isnull(State,''), isnull(ZIP,''), isnull(PHONE,''), isnull(EMAIL,'')
					having COUNT(subscriptionID) > 1
				)
				x 
				join subscriptions s on isnull(s.FNAME,'') = x.FNAME and 
							isnull(s.LNAME,'') = x.LNAME and 
							isnull(s.COMPANY,'') = x.COMPANY and 
							isnull(s.ADDRESS,'') = x.ADDRESS and 
							isnull(s.CITY,'') = x.CITY and 
							isnull(s.State,'') = x.State and 
							isnull(s.ZIP,'') = x.ZIP and 
							isnull(s.PHONE,'') = x.PHONE and 
							isnull(s.EMAIL,'') = x.EMAIL 		
				order by minsubscriptionID
						
			End
			else if @loopcounter = 2 -- COMLETE FNAME,LNAME,ADDRESS
			Begin
				insert into #curtable
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select ISNULL(fname,'') fname, ISNULL(lname,'') lname,ISNULL("ADDRESS",'') "ADDRESS",ZIP,CountryID, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where  ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
				group by FNAME,LNAME,"ADDRESS",ZIP,CountryID
				having COUNT(*) > 1 
				)
				x 
				join subscriptions s with (NOLOCK) on 
							isnull(s.FNAME,'') = isnull(x.FNAME,'') and 
							isnull(s.LNAME,'') = isnull(x.LNAME,'') and 
							isnull(s.ADDRESS,'') = isnull(x.ADDRESS,'') and 
							s.ZIP = x.ZIP and 
							ISNULL(x.CountryID,0) = ISNULL(S.CountryID,0)
				order by minsubscriptionID		
			End
			else if @loopcounter = 3 -- MATCH FIELDS FNAME,LNAME,ADDRESS
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select left(fname,3) fname,left(lname,6) lname,left("ADDRESS",15) "ADDRESS",ZIP, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions  WITH (nolock) 
				where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL("ADDRESS",'') <> ''
				group by left(fname,3),left(lname,6),left("ADDRESS",15),ZIP
				having COUNT(*) > 1
				)
				x 
				join subscriptions s WITH (nolock)  on 
							left(S.fname,3) = isnull(x.FNAME,'') and 
							left(S.lname,6) = isnull(x.LNAME,'') and 
							left(S.ADDRESS,15) = isnull(x.ADDRESS,'') and 
							s.ZIP = x.ZIP 
				order by minsubscriptionID		
			End
			else if @loopcounter = 4 -- COMPLETE FNAME,LNAME,EMAIL
			Begin
				insert into #curtable	
				select distinct minsubscriptionID, s1.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL( s.email,'') Email, COUNT(distinct s.SubscriptionID) as counts, MIN(s.SubscriptionID ) as minsubscriptionID
				from Subscriptions s WITH (nolock)
				where	ISNULL( s.email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
				group by FNAME,LNAME, s.EMAIL
				having COUNT(distinct s.SubscriptionID)  > 1
				)
				x 
				join subscriptions s1 WITH (nolock)   on 
							s1.FNAME = x.FNAME and 
							s1.LNAME = x.LNAME and 
							s1.EMAIL = x.EMAIL
				where ISNULL(s1.email,'') <> '' and ISNULL(s1.fname,'') <> '' and ISNULL(s1.lname,'') <> ''
				order by minsubscriptionID		
			end
			else if @loopcounter = 5 -- COMPLETE FNAME,LNAME,EMAIL
			Begin
				insert into #curtable	
				select distinct minsubscriptionID, s1.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(ps.email,'') Email, COUNT(distinct s.SubscriptionID) as counts, MIN(s.SubscriptionID ) as minsubscriptionID
				from Subscriptions s WITH (nolock) join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID
				where ISNULL(ps.email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
				group by FNAME,LNAME,ps.EMAIL
				having COUNT(distinct s.SubscriptionID)  > 1
				)
				x 
				join subscriptions s1 WITH (nolock)  join PubSubscriptions ps1 on s1.SubscriptionID = ps1.SubscriptionID on 
							s1.FNAME = x.FNAME and 
							s1.LNAME = x.LNAME and 
							s1.EMAIL = x.EMAIL
				where ISNULL(s1.email,'') <> '' and ISNULL(s1.fname,'') <> '' and ISNULL(s1.lname,'') <> ''
				order by minsubscriptionID		
			End
			else if @loopcounter = 6 -- COMPLETE FNAME,LNAME,COMPANY, zip
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select fname,lname,company, zip, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(zip,'') <> ''
				group by FNAME,LNAME,company,zip
				having COUNT(*) > 1
				)
				x 
				join subscriptions s WITH (nolock) on 
							s.FNAME = x.FNAME and 
							s.LNAME = x.LNAME and 
							s.company = x.company  and
							s.ZIP = x.ZIP
				where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' and ISNULL(s.ZIP,'') <> ''
				order by minsubscriptionID			
			End			
			else if @loopcounter = 7 -- MATCH FIELDS FNAME,LNAME,COMPANY, zip
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select left(fname,3) fname ,left(lname,6) lname,left(company,8) company, zip, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where ISNULL(company,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''  and ISNULL(zip,'') <> ''
				group by left(fname,3),left(lname,6),left(company,8), zip
				having COUNT(*) > 1
				)
				x 
				join subscriptions s WITH (nolock) on 
							LEFT(s.FNAME ,3) = x.FNAME and 
							LEFT(s.LNAME,6)  = x.LNAME and 
							LEFT(s.company,8) = x.company  and
							s.ZIP = x.ZIP
				where ISNULL(s.company,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> '' and ISNULL(s.ZIP,'') <> ''
				order by minsubscriptionID		
			End			
			else if @loopcounter = 8 -- MATCH FIELDS FNAME,LNAME,EMAIL
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select LEFT(fname,3) fname,LEFT(lname,6) lname,Email, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where ISNULL(email,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
				group by LEFT(FNAME,3),LEFT(LNAME,6),EMAIL
				having COUNT(*) > 1
				)
				x 
				join subscriptions s WITH (nolock) on 
							LEFT(s.FNAME ,3) = x.FNAME and 
							LEFT(s.LNAME,6)  = x.LNAME and 
							s.EMAIL = x.EMAIL
				where ISNULL(s.email,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
				order by minsubscriptionID		
			End			
			else if @loopcounter = 9 -- COMPLETE FNAME,LNAME,PHONE
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select ISNULL(fname,'') fname, ISNULL(lname,'') lname, ISNULL(phone,'') phone, COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
				group by FNAME,LNAME,phone
				having COUNT(*) > 1
				)
				x 
				join subscriptions s WITH (nolock) on 
							ISNULL(s.FNAME,'')  = x.FNAME and 
							ISNULL(s.LNAME,'')   = x.LNAME and 
							ISNULL(s.phone,'')  = x.phone
				where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
				order by minsubscriptionID		
			End			
			else if @loopcounter = 10 -- MATCH FIELDS FNAME,LNAME,PHONE
			Begin
				insert into #curtable	
				select minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
				select left(fname,3) fname,left(lname,6) lname,phone , COUNT(subscriptionID) as counts, MIN(subscriptionID) as minsubscriptionID
				from Subscriptions with (NOLOCK)
				where ISNULL(phone,'') <> '' and ISNULL(fname,'') <> '' and ISNULL(lname,'') <> ''
				group by left(fname,3),left(lname,6),phone
				having COUNT(*) > 1 -- 16924 dupes
				)
				x 
				join subscriptions s WITH (nolock) on   
							left(ISNULL(s.FNAME,''),3)  = x.FNAME and 
							left(ISNULL(s.LNAME,''),6)  = x.LNAME and 
							ISNULL(s.phone,'')  = x.phone
				where ISNULL(s.phone,'') <> '' and ISNULL(s.fname,'') <> '' and ISNULL(s.lname,'') <> ''
				order by minsubscriptionID		
			End			
			else if @loopcounter = 11 -- EMAIL ONLY PROFILES MATCHING WITH ANOTHER PROFILE WITH FNAME/LNAME MASTER EMAIL ONLY
			Begin
				insert into #curtable	
				select  minsubscriptionID, s.subscriptionID, DENSE_RANK() OVER (order BY minsubscriptionID) AS RowNumber
				from
				(
					select fname,lname,email, SubscriptionID as minsubscriptionID from Subscriptions with (NOLOCK) 
					where ISNULL(fname,'') <> '' and ISNULL(lname,'') <> '' and ISNULL(email,'') <> ''
				)
				x 
				join subscriptions s WITH (nolock) on 
							s.EMAIL = x.email
				where ISNULL(s.fname,'') = '' and ISNULL(s.lname,'') = '' and ISNULL(s.email,'') <> ''
				order by minsubscriptionID		
			End	
			
			
			select @distinctcount = COUNT(distinct KeepSubscriptionID) from #curtable
			
			--print '===================================================================='
			--print '================== Loop condtion : ' + Convert(varchar(100), @loopcounter) + ' / '+ convert(varchar(20), getdate(), 114) + '==================='
			--print '================== Dupes : ' + Convert(varchar(100), @distinctcount) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='
			--print '===================================================================='
			
			insert into #tmp_dupes 
			select rnk, @loopcounter as loop, KeepSubscriptionID, DupeSubscriptionID from #curtable
			where 
					DupeSubscriptionID not in (select DupeSubscriptionID from #tmp_dupes)

			set @loopcounter = @loopcounter + 1

		End

		insert into tmp_duplicates
		select Id, loop as [match#], s.SubscriptionID, s.IGRP_NO, s.FNAME, s.LNAME, s.TITLE, s.COMPANY, s.ADDRESS, s.CITY, s.STATE, s.ZIP, s.COUNTRY, s.EMAIL, s.phone, 
		case when len(isnull(s.fname,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.lname,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.title,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.company,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.address,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.city,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.state,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.zip,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.email,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.phone,'')) > 0 then 1 else 0 end as score, 0 as PRIORITY, isnull(s.DateUpdated, s.DateCreated) as lastchanged --, 'keep' as comments
		from #tmp_dupes t with (NOLOCK) join subscriptions s with (NOLOCK) on t.KeepSubscriptionID = s.subscriptionID
		where KeepSubscriptionID <> DupeSubscriptionID
		union
		select Id, loop as [match#],  s.SubscriptionID, s.IGRP_NO, s.FNAME, s.LNAME, s.TITLE, s.COMPANY, s.ADDRESS, s.CITY, s.STATE, s.ZIP, s.COUNTRY, s.EMAIL, s.phone, 
		case when len(isnull(s.fname,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.lname,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.title,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.company,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.address,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.city,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.state,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.zip,'')) > 0 then 1 else 0 end +
		case when len(isnull(s.email,'')) > 0 then 10 else 0 end +
		case when len(isnull(s.phone,'')) > 0 then 1 else 0 end as score, 0 as PRIORITY, isnull(s.DateUpdated, s.DateCreated) as lastchanged --, 'keep' as comments
		 from #tmp_dupes t with (NOLOCK) join subscriptions s with (NOLOCK) on t.DupeSubscriptionID = s.subscriptionID
		where KeepSubscriptionID <> DupeSubscriptionID
		order by loop, ID desc

		print '================== Total Dupes : ' + Convert(varchar(100), @@ROWCOUNT) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='

		delete td
		 from tmp_duplicates td 
		join
		(
		select distinct ID, matchno --, p.PubID, p.PubCode, isnull(p.IsCirc,0) isCirc, count(distinct s.SubscriptionID) as counts
		from 
				tmp_duplicates t with (NOLOCK) join 
				Subscriptions s with (NOLOCK) on t.SubscriptionID = s.SubscriptionID join
				PubSubscriptions ps with (NOLOCK) on ps.SubscriptionID = s.SubscriptionID join
				pubs p with (NOLOCK) on p.PubID = ps.PubID
		where
				isCirc = 1		
		group by
				ID, matchno, p.PubID, p.PubCode, isnull(p.IsCirc,0)	
		having 
				count(distinct s.SubscriptionID) > 1	
		) x on td.matchno = x.matchno and td.ID = x.ID

		print '================== CIRC Dupes : ' + Convert(varchar(100), @@ROWCOUNT) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='

		declare @totaldupes int
		select @totaldupes = (COUNT(*) OVER ()) from tmp_duplicates
		group by ID, matchno

		print '==================' 
		print '==================  Total Dupes to Delete : ' + Convert(varchar(100), @totaldupes) + ' / '+ convert(varchar(20), getdate(), 114) +  '==================='
		print '================== '

		update tmp_duplicates
		set Priority = x.Priority
		from tmp_duplicates t join
			 (
				select matchno, ID, subscriptionID,
				case 
						when Matchno = 1	then (ROW_NUMBER() over (partition by matchno,ID  order by lastchanged asc))  
						when matchno = 11	then (ROW_NUMBER() over (partition by matchno,ID  order by score asc))
						else					 (ROW_NUMBER() over (partition by matchno,ID  order by lastchanged asc) )
				end as priority
				from tmp_duplicates with (NOLOCK)
			)
				x on x.matchno = t.matchno and x.ID = t.ID and x.SubscriptionID = t.SubscriptionID

		/*********************************************/
		/***************Merge logic*******************/
		/*********************************************/
		declare @Matchno int, 
				@ID int,
				@KeepSubscriptionID int,
				@KeepPubsubscriptionID int,
				@pubID int,
				@pubsubscriptionID int,
				@codesheetID int,
				@rebuildconcensus bit = 0,
				@i int = 1

		DECLARE c_Subscriptions CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY
		FOR 
		select distinct  matchno, ID from tmp_duplicates with (NOLOCK)
		--where matchno >= 6
		order by matchno, ID

		OPEN c_Subscriptions  
				
					
		BEGIN TRY

			set ANSI_WARNINGS  OFF	
					
				FETCH NEXT FROM c_Subscriptions INTO @Matchno, @ID

				WHILE @@FETCH_STATUS = 0  
				BEGIN  
					set @KeepSubscriptionID =  0
								
					set @KeepSubscriptionID = (select top 1 SubscriptionID from tmp_Duplicates with (NOLOCK) where matchno = @matchno and ID = @ID order by priority desc)
							
					set @KeepPubsubscriptionID = 0
					set @codesheetID = 0
					set @pubID = 0
					set @pubsubscriptionID = 0
					
					delete from @tmp

					insert into @tmp
					select SubscriptionID 
					from tmp_Duplicates with (NOLOCK)
					where matchno = @matchno and ID = @ID
					
					declare @subIDnotexists int,
							@dupCircPubexists int
					
					set @subIDnotexists  = 0
					set	@dupCircPubexists  = 0
					
					select @subIDnotexists =  COUNT(t.subscriptionID)
					from
						@tmp t left outer join subscriptions s with (NOLOCK) on t.subscriptionID = s.subscriptionID
					where
						s.subscriptionID is null
					
					--exclude if there is duplicate CIRC product for merge subscriber.
					select  @dupCircPubexists= count(distinct ps.SubscriptionID) 
					from 
							@tmp t join 
							PubSubscriptions ps with (NOLOCK) on ps.SubscriptionID = t.SubscriptionID join
							pubs p with (NOLOCK) on p.PubID = ps.PubID
					where
							isnull(p.IsCirc,0)	 = 1
					group by
							p.PubID, p.PubCode, isnull(p.IsCirc,0)	
					having count(distinct t.SubscriptionID)  > 1
						
					if (@subIDnotexists > 0)
					Begin
						declare @ii int = 0
						--print ''
						--print (convert(varchar,@i) + ' / @matchno : ' + convert(varchar(100),@matchno) + ' / @ID : ' + convert(varchar(100),@ID) + ' / SubscriptionID not exists: ' + ' / ' + convert(varchar(20), getdate(), 114))
					End
					else if @dupCircPubexists > 0
					Begin
						print ''
						print (convert(varchar,@i) + ' / @matchno : ' + convert(varchar(100),@matchno) + ' / @ID : ' + convert(varchar(100),@ID) + ' / CIRC Dupes : ' + ' / ' + convert(varchar(20), getdate(), 114))
					End
					else
					Begin
					
						print (convert(varchar,@i) + ' Dupe Cleanup / @matchno : ' + convert(varchar(100),@matchno) + ' / @ID : ' + convert(varchar(100),@ID) + ' / SubscriptionID : ' + convert(varchar(100),@KeepSubscriptionID) + ' / ' + convert(varchar(20), getdate(), 114)) 
						
						if exists (select top 1 pubID from PubSubscriptions ps  WITH (NOLOCK) join @tmp t on ps.SubscriptionID = t.SubscriptionID group by PubID having COUNT(pubsubscriptionID) > 1)
						Begin
									
							DECLARE c_PubSubscriptions CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY
							FOR 
							select ps.pubID, ps.PubSubscriptionID 
							from PubSubscriptions ps  with (NOLOCK) join  @tmp t on t.subscriptionID = ps.subscriptionID
							where ps.SubscriptionID <> @KeepSubscriptionID and
							PubID in (
									select pubID 
									from PubSubscriptions ps with (NOLOCK) 
									where SubscriptionID in (select SubscriptionID from @tmp) 
									group by pubID having COUNT(*) > 1
									)
							order by ps.PubID, ps.PubSubscriptionID
							
							OPEN c_PubSubscriptions  
							FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
							
							WHILE @@FETCH_STATUS = 0  
							BEGIN  
								--print ('      Duplicate PubSubscriptions / PubID : ' + convert(varchar(100),@pubID) + ' / @pubsubscriptionID : ' + convert(varchar(100),@pubsubscriptionID) + ' / '+ convert(varchar(20), getdate(), 114)	)		
								
								set @KeepPubsubscriptionID = 0
								
								select @KeepPubsubscriptionID = PubSubscriptionID 
								from PubSubscriptions with (NOLOCK)
								where PubID = @pubID and SubscriptionID = @KeepSubscriptionID
								
								if (@KeepPubsubscriptionID = 0)
								Begin
								
									--print 'Insert KeepPubsubscriptionID : '
								
									insert into PubSubscriptions (SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email)
									select @KeepSubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email
									from PubSubscriptions  with (NOLOCK) 
									where pubsubscriptionID = @pubsubscriptionID
									
									set @KeepPubsubscriptionID = @@IDENTITY
									
									--print 'New KeepPubsubscriptionID : ' + convert(varchar(100),@KeepPubsubscriptionID) 
								End
								
								if (Select Count(*) from PubSubscriptionsExtension where PubsubscriptionID = @KeepPubsubscriptionID) = 0
								Begin
									-- There is no Pub Extension record, move the one that will be deleted to the PubSubID we are keeping.
									-- print 'Change PubsubscriptionID in PubsubscriptionsExtension to value we are keeping'
									UPDATE PubsubscriptionsExtension set PubsubscriptionID = @KeepPubsubscriptionID
										where pubsubscriptionID = @PubsubscriptionID
								End
								
								/********* pubSubscriptiondetail ***********/
								DECLARE c_PubSubscriptiondetail CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY
								FOR 
								select CodesheetID from PubSubscriptionDetail  WITH (NOLOCK)
									where PubSubscriptionID in (@pubsubscriptionID, @KeepPubsubscriptionID)
									group by  CodesheetID
									having COUNT(pubsubscriptiondetailID) > 1
								
								--print (' c_PubSubscriptiondetail / ' + convert(varchar(20), getdate(), 114)	)
									
								OPEN c_PubSubscriptiondetail  
								FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
								
								WHILE @@FETCH_STATUS = 0  
								BEGIN  
									--print ('              delete from PubSubscriptionDetail / ' + convert(varchar(20), getdate(), 114)	)
									delete from PubSubscriptionDetail
									where PubSubscriptionID = @pubsubscriptionID and CodesheetID = @codesheetID
								
									FETCH NEXT FROM c_PubSubscriptiondetail INTO @codesheetID
								END

								CLOSE c_PubSubscriptiondetail  
								DEALLOCATE c_PubSubscriptiondetail  
								/********* pubSubscriptiondetail ***********/

								if exists (select top 1 1 from PubSubscriptionDetail  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									update PubSubscriptionDetail
									set PubSubscriptionID = @KeepPubsubscriptionID
									where PubSubscriptionID = @pubsubscriptionID
								end
								
								delete from PaidBillTo where PubSubscriptionID = @pubsubscriptionID
								
								if exists (select top 1 1 from SubscriberAddKillDetail with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from SubscriberAddKillDetail where PubSubscriptionID = @pubsubscriptionID
								end
								
								delete from SubscriptionPaid where PubSubscriptionID = @pubsubscriptionID
								delete from WaveMailingDetail where PubSubscriptionID = @pubsubscriptionID
								delete from WaveMailSubscriber where PubSubscriptionID = @pubsubscriptionID
								
								if exists (select top 1 1 from IssueArchiveProductSubscriptionDetail  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from IssueArchiveProductSubscriptionDetail where PubSubscriptionID = @pubsubscriptionID
								end
								
								delete from History where PubSubscriptionID = @pubsubscriptionID
								delete from HistoryMarketingMap where PubSubscriptionID = @pubsubscriptionID
								delete from HistoryPaid where PubSubscriptionID = @pubsubscriptionID
								delete from HistoryPaidBillTo where PubSubscriptionID = @pubsubscriptionID
								
								if exists (select top 1 1 from HistoryResponseMap  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from HistoryResponseMap where PubSubscriptionID = @pubsubscriptionID
								end
								
								delete from HistorySubscription where PubSubscriptionID = @pubsubscriptionID
								delete from IssueArchiveProductSubscription where PubSubscriptionID = @pubsubscriptionID
								delete from IssueCloseSubGenMap where PubSubscriptionID = @pubsubscriptionID


								delete from PubSubscriptionsExtension where PubSubscriptionID = @pubsubscriptionID
								
								if exists (select top 1 1 from SubscriberTopicActivity  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from SubscriberTopicActivity where PubSubscriptionID = @pubsubscriptionID
								end
								
								if exists (select top 1 1 from SubscriberOpenActivity  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from SubscriberOpenActivity where PubSubscriptionID = @pubsubscriptionID
								end
								
								if exists (select top 1 1 from SubscriberClickActivity  with (NOLOCK) where PubSubscriptionID = @pubsubscriptionID)
								Begin
									delete from SubscriberClickActivity  where PubSubscriptionID = @pubsubscriptionID
								end
								
								delete from PubSubscriptions where  PubSubscriptionID = @pubsubscriptionID --and PubID = @pubID
								
								FETCH NEXT FROM c_PubSubscriptions INTO @pubID, @pubsubscriptionID
								
							END

							CLOSE c_PubSubscriptions  
							DEALLOCATE c_PubSubscriptions  
						End
						
						if exists (select top 1 1 from History with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							update History  set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						if exists (select top 1 1 from HistoryResponseMap with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							update HistoryResponseMap set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						if exists (select top 1 1 from HistorySubscription with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							update HistorySubscription set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						--if exists (select top 1 1 from IssueArchiveProductSubscription with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						--Begin
						--	update IssueArchiveProductSubscription set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						--end
						
						--if exists (select top 1 1 from IssueArchiveProductSubscriptionDetail with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						--Begin
						--	update IssueArchiveProductSubscriptionDetail set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						--end
						
						if exists (select top 1 1 from WaveMailingDetail with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							update WaveMailingDetail set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						if exists (select top 1 1 from PubSubscriptionDetail with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							update PubSubscriptionDetail set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						update PubSubscriptions set SubscriptionID = @KeepSubscriptionID where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						
						delete from BrandScore  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						delete from CampaignFilterDetails  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						
						if exists (select top 1 1 from SubscriberClickActivity with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							delete from SubscriberClickActivity  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						if exists (select top 1 1 from SubscriberOpenActivity with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							delete from SubscriberOpenActivity  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						if exists (select top 1 1 from SubscriberTopicActivity with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							delete from SubscriberTopicActivity  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						if exists (select top 1 1 from SubscriberVisitActivity with (NOLOCK) where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID))
						Begin
							delete from SubscriberVisitActivity  where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						end
						
						delete from SubscriberMasterValues where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						delete from SubscriptionsExtension where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						delete from SubscriptionDetails where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						delete from Subscriptions_DQM where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
						delete from Subscriptions where SubscriptionID in  (select t.SubscriptionID from @tmp t where t.SubscriptionID <> @KeepSubscriptionID) 
					
					end	
					set @i = @i + 1
						
						
					FETCH NEXT FROM c_Subscriptions INTO @Matchno, @ID

				END

			set ANSI_WARNINGS  ON

			---------------------------------------------------------------------
			-- FIX IGRP_NO Mismatch between Subscriptions and Pubsubscriptions
			---------------------------------------------------------------------		    
			update ps set ps.igrp_no = s.igrp_no
			from PubSubscriptions ps 
			join Subscriptions s WITH (NOLOCK) on s.SubscriptionID = ps.SubscriptionID
			where ps.IGrp_No is null and s.IGRP_NO is not null

			update ps set ps.igrp_no = s.igrp_no
			from PubSubscriptions ps 
			join Subscriptions s WITH (NOLOCK) on s.SubscriptionID = ps.SubscriptionID
			where ps.IGrp_No <> s.IGRP_NO and ps.IGrp_No is not null and s.IGRP_NO is not null

			-----------------------------------------------------------------------------
			-- FIX Subscriptions datecreated using the originating pubID's datecreated
			-----------------------------------------------------------------------------   
			update s set s.DateCreated = ps.DateCreated
			from PubSubscriptions ps 
			join Subscriptions s on s.SubscriptionID = ps.SubscriptionID and s.OriginatedPubID = ps.PubID
			where s.SubscriptionID in (select t.subscriptionID from tmp_duplicates t)

		END TRY
			
		BEGIN CATCH

			DECLARE @ErrorMessage NVARCHAR(4000);
			DECLARE @ErrorSeverity INT;
			DECLARE @ErrorState INT;

			SELECT 
				@ErrorMessage	= ERROR_MESSAGE(),
				@ErrorSeverity	= ERROR_SEVERITY(),
				@ErrorState		= ERROR_STATE();

			-- Use RAISERROR inside the CATCH block to return error
			-- information about the original error that caused
			-- execution to jump to the CATCH block.
			RAISERROR (@ErrorMessage,  16, 1);
			PRINT ('************************************');
			PRINT ('ERROR : ' + @ErrorMessage);
			PRINT ('************************************');

		END CATCH;

		
		CLOSE c_Subscriptions  
		DEALLOCATE c_Subscriptions  
	END


	drop table #curtable
	drop table #delSubscriptionIDs
	drop table #tmp_dupes
	
END


