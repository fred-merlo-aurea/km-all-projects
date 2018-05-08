CREATE PROCEDURE [dbo].[spLoad_MEDTECH_TopicsDataToMD]
(
	@FullSync bit = false
)
as

Begin
	
	set nocount on
	
	declare @from date	
	declare @topics TABLE (topicCode varchar(10), TopicDesc varchar(100))
	
	insert into @topics
	select mastervalue, masterdesc 
	from	
			[10.10.41.251].medtechmasterDB.dbo.mastergroups mg join 
			[10.10.41.251].medtechmasterDB.dbo.mastercodesheet mc on mg.mastergroupID = mc.mastergroupID
	where mg.name = 'Master_TOPICS'
	union all
	select '0' + mastervalue, masterdesc 
	from	
			[10.10.41.251].medtechmasterDB.dbo.mastergroups mg join 
			[10.10.41.251].medtechmasterDB.dbo.mastercodesheet mc on mg.mastergroupID = mc.mastergroupID
	where mg.name = 'Master_TOPICS' and LEN(mastervalue) = 1 and ISNUMERIC(mastervalue) = 1
	order by 1
	
	if @FullSync = 1
		set @from = dateadd(yy, -1, GETDATE() )
	Else
		set @from = dateadd(dd, -7, GETDATE() )
		
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

	DECLARE c_Pubs CURSOR FOR 
	select p.pubID, p.groupID, GroupDatafieldsID  from [10.10.41.251].medtechmasterDB.dbo.pubs p join groupdatafields gdf on p.groupID = gdf.groupID Where p.GroupID > 0 and ShortName = 'Topics'
	
	OPEN c_Pubs  
	FETCH NEXT FROM c_Pubs INTO @pubID, @groupID, @GroupDatafieldsID
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
		set @i = @i + 1
		
		print ('   Start : ' + convert(varchar(20), getdate() , 114))
		
		print convert(varchar(10), @i) + ' / GroupID : ' + convert(varchar(10), @groupID)
				
		insert into #tmpPubSubscriptions	
		select ps.pubsubscriptionID, e.EmailID
		from	[10.10.41.251].medtechmasterDB.dbo.pubsubscriptions ps join
				[10.10.41.251].medtechmasterDB.dbo.subscriptions s on ps.subscriptionID = s.subscriptionID join
				(select e.emailID, emailaddress from Emails e join EmailGroups eg on e.EmailID = eg.EmailID where eg.GroupID = @groupID) e on s.Email  = e.EmailAddress
		Where ps.pubID =  @PubID and EmailExists = 1 and LEN(email) > 0
		
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
		
			insert into [10.10.41.251].medtechmasterDB.dbo.SubscriberclickActivity (PubSubscriptionID, BlastID, ActivityDate, Link, LinkAlias, LinkSource, Linktype)
			select  s.pubsubscriptionID, 0,  ActionDate, null, tp.TopicDesc, null, null
			from	
					#tmpTopicClick t join 
					@topics tp on t.topiccode = tp.topicCode join
					#tmpPubSubscriptions s on t.emailID = s.emailID
			
			print('      Click Insert : ' + convert(varchar(10), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate() , 114))
					
			delete from #tmpTopicClick
			delete from #tmpPubSubscriptions
		end
		
		print ('   End : ' + convert(varchar(20), getdate() , 114))
		print ('   ')
		FETCH NEXT FROM c_Pubs INTO @pubID, @groupID, @GroupDatafieldsID
	END

	CLOSE c_Pubs  
	DEALLOCATE c_Pubs  

	drop table #tmpTopicClick
	drop table #tmpPubSubscriptions
	
End
