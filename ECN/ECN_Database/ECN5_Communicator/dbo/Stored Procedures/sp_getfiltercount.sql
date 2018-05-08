CREATE proc [dbo].[sp_getfiltercount]
	@CustomerID varchar(5000)
	as
Begin
	set nocount on
	declare              
		@Col1 varchar(8000),        
		@Col2 varchar(8000),      
		@Filter varchar(8000),
		@groupID int,
		@filterID int,
		@masterlistGroupID int,
		@mastersupplistGroupID int

	if CHARINDEX(',',@customerID) = 0
	Begin

	 set @masterlistGroupID = 0
		set @mastersupplistGroupID = 0
		
		select @mastersupplistGroupID = groupID from groups where customerID = @CustomerID and groupname = 'Master Supression'

		create table #filters (filterID int, filtername varchar(200), counts  int)

		insert into #filters
		select filterID, filtername, 0 from [FILTER] where customerID = @CustomerID and filtername in ('Student','Fan','Alumni','Faculty','Parent','Membership Card')   

		DECLARE c_Filter CURSOR FOR 
		SELECT filterID from #filters
		OPEN c_Filter  

		FETCH NEXT FROM c_Filter INTO @filterID
		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			print @filterID

			set @Col1  = ''        
			set @Col2  = ''        
			set @filter = ''
			set @groupID = 0

			select @groupID = groupID, @Filter=WhereClause from [FILTER] where filterID = @FilterID

			if @filter <> ''
				set @filter = ' and (' + @filter + ') '
		        
			create table #c(counts int)            

			exec ( 'insert into #c select count(Emails.EmailID) from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
				' where EmailGroups.emailID not in (select emailID from emailgroups where groupID = ' + @mastersupplistGroupID + ') and EmailGroups.GroupID = ' + @GroupID + ' and EmailGroups.SubscribeTypeCode = ''S''' + @Filter)        
	 		 
			update #filters set counts = (select counts from #c) where filterID = @filterID

			drop table #c  
	          
			FETCH NEXT FROM c_Filter INTO @filterID
		END

		CLOSE c_Filter  
		DEALLOCATE c_Filter  
		

		select @masterlistGroupID = groupID from groups where customerID = @CustomerID and groupname = 'MasterList'

		if @masterlistGroupID> 0 
			insert into #filters
			select 0, 'All', count(emailgroupID) from emailgroups where groupID = @masterlistGroupID and SubscribeTypeCode = 'S'
					and EmailGroups.emailID not in (select emailID from emailgroups where groupID = @mastersupplistGroupID)

		select filtername, filtername + ' (' + convert(varchar,counts) + ')' as filterText from #filters order by filtername

		drop table #filters
		
	 end
	 else
	 Begin
	 exec ('select user1 as filtername, count(distinct e.emailID) as counts into #temp1 from emails e join emailgroups eg on e.EmailID = eg.EmailID join groups g on eg.groupID = g.groupID 
	 where	g.groupname=''MasterList'' and  e.CustomerID In (' + @customerID + ') and ISNULL(User1,'''') <> '''' and eg.SubscribeTypeCode = ''S'' and
			e.EmailID not in (select eg.emailID from emailgroups eg join groups g on eg.groupID = g.groupID where g.groupname=''Master Supression'' and  g.CustomerID In (' + @customerID + ')) group by User1
			
			select ''ALL''  as filtername, ''All ('' + convert(varchar,SUM(COUNTS)) + '')'' as filterText from #temp1
			UNION 
			SELECT FILTERNAME, filtername + '' ('' + convert(varchar,counts) + '')'' FROM #TEMP1 ORDER BY 1
			
			drop table #temp1
			
			')

	 End
end
