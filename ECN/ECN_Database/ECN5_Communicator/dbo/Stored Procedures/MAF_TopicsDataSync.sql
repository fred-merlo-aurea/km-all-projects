CREATE PROCEDURE [dbo].[MAF_TopicsDataSync]
(

	@Database varchar(50),
	@FullSync bit = false
)
as

Begin
	
	set nocount on
	
	declare @from date	
	create table #topics (topicCode varchar(10), TopicDesc varchar(100))
	
	exec ('insert into #topics
	select mastervalue, masterdesc 
	from	
			[10.10.41.251].' + @Database + '.dbo.mastergroups mg join 
			[10.10.41.251].' + @Database + '.dbo.mastercodesheet mc on mg.mastergroupID = mc.mastergroupID
	where mg.name = ''Master_TOPICS''
	union all
	select ''0'' + mastervalue, masterdesc 
	from	
			[10.10.41.251].' + @Database + '.dbo.mastergroups mg join 
			[10.10.41.251].' + @Database + '.dbo.mastercodesheet mc on mg.mastergroupID = mc.mastergroupID
	where mg.name = ''Master_TOPICS'' and LEN(mastervalue) = 1 and ISNUMERIC(mastervalue) = 1
	order by 1')
	
	if @FullSync = 1
		set @from = dateadd(yy, -1, GETDATE() )
	Else
		set @from = dateadd(dd, -7, GETDATE() )
	
	--set @from = '4/7/2013'
		
	declare @pubID int,
			@groupID int,
			@GroupDatafieldsID int,
			@pubsubscriptionID int,
			@dt datetime,
			@i int
			
	set @dt = GETDATE()
	set @i = 0
	
	create table #tmpPubSubscriptions (PubSubscriptionID int, emailID int)
	create table #tmpTopicClick (emailID int, Actiondate date, topiccode varchar(255))
	
	CREATE INDEX IDX_tmpEALclick_emailaddress ON #tmpTopicClick(emailID)
	CREATE INDEX IDX_tmpPubSubscriptions_emailID ON #tmpPubSubscriptions(emailID)

	create table #groups (pubID int, GroupID int, GroupdatafieldsID int)
	
	exec ('insert into #groups 
			select p.pubID, pg.groupID, GroupDatafieldsID  
			from [10.10.41.251].' + @Database + '.dbo.pubs p join
					[10.10.41.251].' + @Database + '.dbo.pubgroups pg on p.pubID = pg.pubID join groupdatafields gdf on pg.groupID = gdf.groupID Where pg.GroupID > 0 and ShortName = ''Topics''')


	select * from #groups

	DECLARE c_Pubs CURSOR FOR select * from #groups

	OPEN c_Pubs  
	FETCH NEXT FROM c_Pubs INTO @pubID, @groupID, @GroupDatafieldsID
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @i = @i + 1
		
		print ('   Start : ' + convert(varchar(20), getdate() , 114))
		
		print convert(varchar(10), @i) + ' / GroupID : ' + convert(varchar(10), @groupID)
				
		exec ('insert into #tmpPubSubscriptions	
		select ps.pubsubscriptionID, e.EmailID
		from	[10.10.41.251].' + @Database + '.dbo.pubsubscriptions ps join
				[10.10.41.251].' + @Database + '.dbo.subscriptions s on ps.subscriptionID = s.subscriptionID join
				(select e.emailID, emailaddress from Emails e join EmailGroups eg on e.EmailID = eg.EmailID where eg.GroupID = ' + @groupID + ') e on s.Email  = e.EmailAddress
		Where ps.pubID = ' + @PubID + ' and EmailExists = 1 and LEN(email) > 0')
		
		print('      Subscribers : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))

		if exists (select top 1 * from #tmpPubSubscriptions)
		Begin
			insert into #tmpTopicClick
			select  edv.EmailID, edv.ModifiedDate , edv.DataValue
			from	
					 Emaildatavalues edv  with (NOLOCK)
			where 
					GroupDatafieldsID = @GroupDatafieldsID and
					edv.ModifiedDate >= @from

					
			print('      TopicClick : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
		
		
			exec ('insert into [10.10.41.251].' + @Database + '.dbo.SubscriberclickActivity (PubSubscriptionID, BlastID, ActivityDate, Link, LinkAlias, LinkSource, Linktype)
			select  s.pubsubscriptionID, 0,  ActionDate, null, tp.TopicDesc, null, null
			from	
					#tmpTopicClick t join 
					#topics tp on t.topiccode = tp.topicCode join
					#tmpPubSubscriptions s on t.emailID = s.emailID')
			
			print('      Click Insert : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
					
			delete from #tmpTopicClick
			delete from #tmpPubSubscriptions
		end
		
		print ('   End : ' + convert(varchar(20), getdate() , 114))
		print ('   ')
		FETCH NEXT FROM c_Pubs INTO @pubID, @groupID, @GroupDatafieldsID
	END

	--exec ('[10.10.41.251].' + @Database + '.dbo.spUpdateTopicsDimension')

	CLOSE c_Pubs  
	DEALLOCATE c_Pubs  

	drop table #groups 
	drop table #topics
	drop table #tmpTopicClick
	drop table #tmpPubSubscriptions
	
End
