



------------------------------------------------
-- spLoadActivityDataToMD
-- 
--	Updated 2/3/2014 MK Commented Out Topic code sync section.  This code has been replaced by an SSIS Package
--	Updated 4/15/2014 MK Commented Out Domain Tracking and Suppression sync section.  This code has been replaced by an SSIS Package
--
------------------------------------------------

CREATE proc [dbo].[spLoadActivityDataToMD]
(
	@Database varchar(50) ,
	@FullSync bit = false
)
as
Begin
	
	set nocount on
	
	declare @from date, 
			@to datetime

	   
	DECLARE 
        @ErrorMessage    NVARCHAR(4000),
        @ErrorNumber     INT,
        @ErrorSeverity   INT,
        @ErrorState      INT,
        @ErrorLine       INT,
        @ErrorProcedure  NVARCHAR(200);

	set @to = CONVERT(varchar(10), dateadd(dd, -1 * DATEPART(weekday,getdate()), getdate() ), 101) + ' 23:59:59'


	-- Check Sunday - during weekdays; check if the activitytable is 0; if 0 - run Fully Sync
	if @FullSync <> 1
	Begin
		declare @ActivityCount int
		set @ActivityCount = 0

		DECLARE @SQLString nvarchar(500);
		DECLARE @ParmDefinition nvarchar(500);
	
		set @SQLString = 'select @ActivityCountOUT = SUM(counts) from 
		(
		select isnull(count(OPenActivityID),0) as counts from [10.10.41.251].' + @Database + '.dbo.SubscriberOpenActivity
		union all
		select isnull(count(ClickActivityID),0) from [10.10.41.251].' + @Database + '.dbo.SubscriberClickActivity
		) inn'

		SET @ParmDefinition = N'@ActivityCountOUT nvarchar(25) OUTPUT';

		EXECUTE sp_executesql @SQLString, @ParmDefinition, @ActivityCountOUT = @ActivityCount OUTPUT;	

		select @ActivityCount

		if @ActivityCount = 0
			set @FullSync = 1
		else
		Begin
			if DATEPART(weekday,GETDATE()) <> 1
				RETURN
		End
	End
	
	if @FullSync = 1
		set @from = dateadd(yy, -1, @to )
	Else
		set @from = dateadd(dd, -6, @to )

	select @from ,@To, @FullSync
	
	if @FullSync = 1
	Begin
		exec ('delete from [10.10.41.251].' + @Database + '.dbo.SubscriberOpenActivity')	
		exec ('delete from [10.10.41.251].' + @Database + '.dbo.SubscriberClickActivity')	
		Print 'Starting Full Sync'
	End
	
	declare @groupID int,
			@PubID int,
			@pubsubscriptionID int,
			@Emailaddress varchar(100),
			@dt datetime,
			@i int,
			@TotalSubscribersinGroup int,
			@EmailID int,
			@SubscriberCount int
			
	set @dt = GETDATE()
	set @i = 0
	
	create table #tmpPubSubscriptions (PubSubscriptionID int, emailID int)
	create table #tmpEALopen (blastID int, emailID int, Actiondate date)
	create table #tmpEALclick (blastID int, emailID int, Actiondate date, link varchar(2048), linkAlias varchar(100), linkSource varchar(100), linkType varchar(100))
	
	CREATE INDEX IDX_tmpEALopen_emailID ON #tmpEALopen(emailID)
	CREATE INDEX IDX_tmpEALclick_emailID ON #tmpEALclick(emailID)
	
	CREATE INDEX IDX_tmpPubSubscriptions_emailID ON #tmpPubSubscriptions(emailID)
	
	/*
	select count(*) from [10.10.41.251].canonmasterdb_TEST.dbo.SubscriberOpenActivity
	select count(*) from [10.10.41.251].canonmasterdb_TEST.dbo.SubscriberClickActivity
	delete from [10.10.41.251].canonmasterdb_TEST.dbo.SubscriberOpenActivity
	delete from [10.10.41.251].canonmasterdb_TEST.dbo.SubscriberClickActivity
	*/
	
	Create table #pubs (GroupID int, PubID int, SubscriberCount int)
		

		
		exec ('insert into #pubs
		select pg.groupID, p.PubID, COUNT(s.subscriptionID)
		from	[10.10.41.251].' + @Database + '.dbo.pubs p with (NOLOCK) join
				[10.10.41.251].' + @Database + '.dbo.pubgroups pg with (NOLOCK) on p.pubID = pg.pubID join
				[10.10.41.251].' + @Database + '.dbo.pubsubscriptions ps with (NOLOCK) on p.pubiD = ps.pubID join
				[10.10.41.251].' + @Database + '.dbo.subscriptions s with (NOLOCK) on ps.subscriptionID = s.subscriptionID 
		Where pg.GroupID > 0 
		group by pg.groupID, p.PubID
		order by 3 asc')

		
		DECLARE c_Pubs CURSOR FOR 
		select groupID, pubID, subscriberCount from #pubs
		
		
		OPEN c_Pubs  
		FETCH NEXT FROM c_Pubs INTO @groupID, @PubID, @SubscriberCount

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
		
			BEGIN TRY		
				--set @i = @i + 1
				
				--print ('   Start : ' + convert(varchar(20), getdate() , 114))
				
				--print convert(varchar(10), @i) + ' / GroupID : ' + convert(varchar(10), @groupID) + ' / Count :' + convert(varchar(10), @SubscriberCount)
				
				delete from #tmpEALopen
				delete from #tmpEALclick
				delete from #tmpPubSubscriptions				
										
				exec ('
				insert into #tmpPubSubscriptions	
				select ps.pubsubscriptionID, e.emailID
				from	[10.10.41.251].' + @Database + '.dbo.pubsubscriptions ps with (NOLOCK) join
						[10.10.41.251].' + @Database + '.dbo.subscriptions s with (NOLOCK) on ps.subscriptionID = s.subscriptionID join
						(select e.emailID, emailaddress from Emails e  with (NOLOCK) join EmailGroups eg with (NOLOCK) on e.EmailID = eg.EmailID where eg.GroupID = ' + @groupID + ') e on s.Email  = e.EmailAddress
				Where ps.pubID = ' + @PubID + ' and EmailExists = 1 and LEN(s.email) > 0 ' )
				
				--print('      Subscribers : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
				
				if exists (select top 1 * from #tmpPubSubscriptions)
				Begin		
					insert into #tmpEALopen
					select b.BlastID, bo.emailID, max(bo.OpenTime)
					from	
							[BLAST] b with (NOLOCK) join
							ECN_ACTIVITY..BlastActivityOpens bo with (NOLOCK) on bo.BlastID = b.BlastID 
					where 
							groupID = @groupID and 
							sendtime between @from and @to and 
							testblast='N' and 
							statuscode='sent' 
					group by b.BlastID, bo.emailID
									
					--print('      opens : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))

					insert into #tmpEALclick
					select  b.BlastID, bc.emailID,  max(bc.ClickTime), bc.URL, substring(la.Alias,1,99), lon.LinkOwnerName, co.CodeDisplay 
					from	
							 [BLAST] b with (NOLOCK) join
							 ECN_ACTIVITY..BlastActivityClicks bc with (NOLOCK) on bc.BlastID = b.BlastID  join
							 [LAYOUT] l with (NOLOCK) on l.layoutID = b.layoutID left outer join
							 Content c on (c.contentID = ContentSlot1 or c.contentID = ContentSlot2 or c.contentID = ContentSlot3 or c.contentID = ContentSlot4 or c.contentID = ContentSlot5 or c.contentID = ContentSlot6 or c.contentID = ContentSlot7 or c.contentID = ContentSlot8 or c.contentID = ContentSlot9 ) left outer join
							 linkalias la with (NOLOCK) on bc.URL = la.link and la.contentID = c.contentID left outer join 
							 linkownerindex lon with (NOLOCK) on lon.LinkOwnerIndexID = la.LinkOwnerID left outer join  
							 Code co with (NOLOCK) on CodeType = 'LinkType' AND co.CustomerID = b.customerID and co.codeID = la.LinkTypeID 
					where 
							groupID = @groupID and 
							sendtime between @from and @to and  
							testblast='N' and 
							statuscode='sent'
					group by b.BlastID, bc.emailID,  bc.URL, la.Alias, lon.LinkOwnerName, co.CodeDisplay 
					
					--print('      clicks : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
				
					exec ('insert into [10.10.41.251].' + @Database + '.dbo.SubscriberOpenActivity (PubSubscriptionID, BlastID, ActivityDate)
					select  s.pubsubscriptionID, BlastID, ActionDate
					from #tmpEALopen t  with (NOLOCK)  join #tmpPubSubscriptions s  with (NOLOCK)  on t.emailID = s.emailID')
					
					--print('      Open Insert : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
				
					exec ('insert into [10.10.41.251].' + @Database + '.dbo.SubscriberclickActivity (PubSubscriptionID, BlastID, ActivityDate, Link, LinkAlias, LinkSource, Linktype)
					select  s.pubsubscriptionID, BlastID,  ActionDate, t.link, t.linkAlias, t.linkSource,t.linkType
					from #tmpEALclick t  with (NOLOCK)  join #tmpPubSubscriptions s  with (NOLOCK)  on t.emailID = s.emailID')
					
					--print('      Click Insert : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))

				end
				
				--print ('   End : ' + convert(varchar(20), getdate() , 114))
				--print ('   ')
			
			END TRY
			BEGIN CATCH

				-- Assign variables to error-handling functions that 
				-- capture information for RAISERROR.
				SELECT 
					@ErrorNumber = ERROR_NUMBER(),
					@ErrorSeverity = ERROR_SEVERITY(),
					@ErrorState = ERROR_STATE(),
					@ErrorLine = ERROR_LINE(),
					@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

				-- Build the message string that will contain original
				-- error information.
				SELECT @ErrorMessage = 
					N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
						'Message: '+ ERROR_MESSAGE();

				Print @errormessage
			        
			END CATCH
	
			FETCH NEXT FROM c_Pubs INTO @groupID, @PubID, @SubscriberCount
		END

		CLOSE c_Pubs  
		DEALLOCATE c_Pubs  

	

		drop table #pubs	
		drop table #tmpEALopen
		drop table #tmpEALclick
		drop table #tmpPubSubscriptions

--The Following Code as been replaced by an SSIS Package. MK 04/15/2014
/*	BEGIN TRY		
		if DATEPART(weekday,GETDATE()) = 1 or @FullSync = 1
		Begin
			if @Database = 'AdvanstarMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC AdvanstarMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 65
			End
			else if @Database = 'ATHBMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC ATHBMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 15
			End				
			else if @Database = 'canonmasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC canonmasterDB')
				
				exec MAF_MasterSuppressSYNC @Database, 42
			End
			--else if @Database = 'medtechmasterDB'
			--Begin
			--	print('      exec MAF_MasterSuppressSYNC medtechmasterDB')
			--	exec MAF_MasterSuppressSYNC @Database, 55
			--End	
			else if @Database = 'NASFTMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC NASFTMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 71
			End	
			else if @Database = 'StamatsMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC StamatsMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 73
				exec MAF_MasterSuppressSYNC @Database, 74
				exec MAF_DomainTrackingSYNC @Database, 0, 73
				exec MAF_DomainTrackingSYNC @Database, 0, 74
			End	
			else if @Database = 'ScrantonMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC ScrantonMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 75
				exec MAF_DomainTrackingSYNC @Database, 0, 75
			End				
			else if @Database = 'UPIMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC UPIMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 63
			End	
			else if @Database = 'WattMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC WattMasterMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 34
			End	
			else if @Database = 'TenMissionsMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC TenMissionsMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 0,   2635
				exec MAF_DomainTrackingSYNC @Database, 2635, 0
			End
			else if @Database = 'VCASTMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC VCASTMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 77
			End	
			else if @Database = 'FranceMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC FranceMasterDB')
				exec MAF_MasterSuppressSYNC @Database, 80
			End	
			else if @Database = 'MeisterMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC MeisterMasterDB')
				exec MAF_MasterSuppressSYNC @Database, @basechannelid = 82
			End	
			else if @Database = 'LebharMasterDB'
			Begin
				print('      exec MAF_MasterSuppressSYNC LebharMasterDB')
				exec MAF_MasterSuppressSYNC @Database ,@CustomerId = 3565
			End	


	
--The Following Code as been replaced by an SSIS Package. MK 02/03/2014
--			if @Database = 'medtechmasterDB' or @Database = 'wattmasterDB' or @Database = 'AdvanstarMasterDB' or @Database = 'ATHBMasterDB' or @Database = 'StamatsMasterDB' 
--			Begin
--				print('      exec MAF_TopicsDataSync')
--				
--				exec MAF_TopicsDataSync @Database, @FullSync
--			End
--			
		End

	END TRY
	BEGIN CATCH
    
		SELECT 
			@ErrorNumber = ERROR_NUMBER(),
			@ErrorSeverity = ERROR_SEVERITY(),
			@ErrorState = ERROR_STATE(),
			@ErrorLine = ERROR_LINE(),
			@ErrorProcedure = ISNULL(ERROR_PROCEDURE(), '-');

		-- Build the message string that will contain original
		-- error information.
		SELECT @ErrorMessage = 
			N'Error %d, Level %d, State %d, Procedure %s, Line %d, ' + 
				'Message: '+ ERROR_MESSAGE();

		Print @errormessage
	        
	END CATCH		
*/
	--exec ('exec [10.10.41.251].' + @Database + '.dbo.spUpdateScore')	
End





