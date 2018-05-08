CREATE proc [dbo].[sp_getfiltercount2]
(
	@CustomerID int,
	@filtername varchar(100)
)
as
Begin
	set nocount on
	declare              
		@Col1 varchar(8000),        
		@Col2 varchar(8000),      
		@Filter varchar(8000),
		@groupID int,
		@filterID int,
		@mastersupplistGroupID int

	set @mastersupplistGroupID = 0
	select @mastersupplistGroupID = groupID from groups where customerID = @CustomerID and groupname = 'Master Supression'


	create table #filters (filterID int, filtername varchar(200), counts  int)

	insert into #filters
	select filterID, filtername, 0 from [FILTER] where customerID = @CustomerID and filtername like @filtername+'%'  

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
			' where EmailGroups.emailID not in (select emailID from emailgroups where groupID = ' + @mastersupplistGroupID + ') and  EmailGroups.GroupID = ' + @GroupID + ' and EmailGroups.SubscribeTypeCode = ''S''' + @Filter)        
 		 
		update #filters set counts = (select counts from #c) where filterID = @filterID

		drop table #c  
          
		FETCH NEXT FROM c_Filter INTO @filterID
	END

	CLOSE c_Filter  
	DEALLOCATE c_Filter  

	select filterID, filtername, replace(filtername,@filtername,'') + ' (' + convert(varchar,counts) + ')' as filterText from #filters order by filtername

	drop table #filters
end
