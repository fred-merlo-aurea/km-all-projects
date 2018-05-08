CREATE proc [dbo].[sp_Subscriber_MasterCodesheet_counts] 
@Queries XML, 
@masterGroup int = 0, 
@Description varchar(50) = '', 
@BrandID int = null, 
@IsRecencyView bit = false
as
Begin
       set nocount on

		--print 'start : ' + convert(varchar(100), getdate(), 109)
       
        declare @docHandle int,
                     @MasterGroupColumn varchar(100)
                     
		create table #subscriptionIDs (SubscriptionID int primary key)                     

		declare @hDoc AS INT
		declare @filterno int, @qry varchar(max),
				@linenumber int, @SelectedFilterNo varchar(800), @SelectedFilterOperation varchar(20), @SuppressedFilterNo varchar(800), @SuppressedFilterOperation varchar(20) 

		create table #tblquery  (filterno int, Query varchar(max))
		create table #tblqueryresults  (filterno int, subscriptionID int, primary key (filterno, subscriptionID))

		EXEC sp_xml_preparedocument @hDoc OUTPUT, @queries

		insert into #tblquery
		SELECT filterno, Query
		FROM OPENXML(@hDoc, 'xml/Queries/Query')
		WITH 
		(
		filterno int '@filterno',
		Query [varchar](max) '.'
		)
		
		SELECT 
				@linenumber = linenumber, 
				@SelectedFilterNo = selectedfilterno, 
				@SelectedFilterOperation = selectedfilteroperation, 
				@SuppressedFilterNo = suppressedfilterno, 
				@SuppressedFilterOperation = suppressedfilteroperation
		FROM OPENXML(@hDoc, 'xml/Results/Result')
		WITH 
		(
			linenumber int							'@linenumber',
			selectedfilterno varchar(800)			'@selectedfilterno',
			selectedfilteroperation varchar(20)		'@selectedfilteroperation',
			suppressedfilterno varchar(800)			'@suppressedfilterno',
			suppressedfilteroperation varchar(20)	'@suppressedfilteroperation'
		)

		EXEC sp_xml_removedocument @hDoc
		
		DECLARE c_queries CURSOR 
		FOR select filterno from #tblquery
			
		OPEN c_queries  
		FETCH NEXT FROM c_queries INTO @filterno

		WHILE @@FETCH_STATUS = 0  
		BEGIN  
			select @qry =  query from #tblquery with (NOLOCK) where filterno = @filterno
			--print @qry
			
			insert into #tblqueryresults  
			execute (@qry)
			
			FETCH NEXT FROM c_queries INTO @filterno
		END
		
		CLOSE c_queries  
		DEALLOCATE c_queries 

		select @qry = 'select distinct subscriptionID from ((select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SelectedFilterNo, ',', '  ' + @SelectedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') ' + 
		case when len(@suppressedfilterno) > 0 then 
			' except ' 
			+ ' (select subscriptionID from #tblqueryresults t where filterno = ' + REPLACE(@SuppressedfilterNo , ',', '  ' + @SuppressedFilterOperation + ' select subscriptionID from #tblqueryresults t where filterno = ') + ') '
		else
		'' end
		+ ') x'

		--print @qry
		
		insert into #subscriptionIDs 
		execute (@qry)
		
		drop table #tblquery
		drop table #tblqueryresults                     

		--print 'after subID : ' + convert(varchar(100), getdate(), 109)
       
       if @BrandID = 0
       Begin
              if (@IsRecencyView = 0)
              Begin
                     select sd.masterid as ID, m.masterValue as Value, m.MasterDesc as [Desc], m.MasterDesc1 as Desc1,
                           count( distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s with (nolock)
                           join subscriptiondetails sd  with (nolock) on sd.subscriptionid = s.subscriptionid 
                           join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%')
                     group by sd.masterid, m.masterValue, m.MasterDesc, m.MasterDesc1
                     union all
                     select 0, 'YYY', 'NO RESPONSES',' ', COUNT(distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s 
                     left outer join subscribermastervalues smv  with (nolock) on s.subscriptionid = smv.subscriptionid and  smv.MasterGroupID = @masterGroup
                     where  smv.MastercodesheetValues is null
                     union all
                     select -1, 'ZZZ', 'TOTAL UNIQUE SUBSCRIBERS',' ', 
                           count( distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s 
                           join subscriptiondetails sd  with (nolock) on sd.subscriptionid = s.subscriptionid 
                           join mastercodesheet m  with (nolock) on m.masterid = sd.masterid 
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%')
                     order by count desc
              End
              else
              Begin
                     select m.masterid as ID, m.masterValue as Value, m.MasterDesc as [Desc], m.MasterDesc1 as Desc1,
                           count( distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s with (nolock)
                           join vw_RecentConsensus vrc on vrc.SubscriptionID = s.SubscriptionID 
                           join subscriptiondetails sd  with (nolock) on sd.subscriptionid = s.subscriptionid 
                           join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and vrc.MasterGroupID = @masterGroup
                     group by m.masterid, m.masterValue, m.MasterDesc, m.MasterDesc1
                     union all
                     select 0, 'YYY', 'NO RESPONSES',' ',  COUNT(distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s 
                           left outer join subscribermastervalues smv  with (nolock) on s.subscriptionid = smv.subscriptionid and  smv.MasterGroupID = @masterGroup
                     where  smv.MastercodesheetValues is null 
                     union all
                     select -1, 'ZZZ', 'TOTAL UNIQUE SUBSCRIBERS',' ', 
                           count( distinct vrc.SubscriptionID) as 'Count'
                     from #subscriptionIDs s with (nolock)
                           join vw_RecentConsensus vrc on vrc.SubscriptionID = s.SubscriptionID
                           join subscriptiondetails sd  with (nolock) on sd.subscriptionid = s.subscriptionid 
                           join mastercodesheet m  with (nolock) on m.masterid = vrc.masterid 
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and vrc.MasterGroupID = @masterGroup
                     order by count desc
              End
       End
       else
       Begin
              if (@IsRecencyView = 0)
              Begin
                     select m.masterid as ID, m.masterValue as Value, m.MasterDesc as [Desc], m.MasterDesc1 as Desc1,
                           count( distinct s.SubscriptionID) as 'Count'
                     from   branddetails bd WITH (nolock)
                              join vw_BrandConsensus v on bd.BrandID = v.BrandID
                              join mastercodesheet m  with (nolock) ON m.masterid = v.masterid        
                              join #subscriptionIDs s ON s.SubscriptionID = v.SubscriptionID
                     where m.MasterGroupID =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and bd.BrandID = @BrandID 
                     group by m.masterid, m.masterValue, m.MasterDesc, m.MasterDesc1
                     union all
                     select 0, 'YYY', 'NO RESPONSES',' ', 
                           count( distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s 
                           left outer join vw_BrandConsensus v on s.subscriptionid = v.subscriptionid  and v.BrandID = @BrandID  and MasterID in (select mc.MasterID from Mastercodesheet mc  with (nolock) where MasterGroupID = @masterGroup)
                     where  v.SubscriptionID is null
                     union all
                     select -1, 'ZZZ', 'TOTAL UNIQUE SUBSCRIBERS',' ',  COUNT(distinct s.SubscriptionID)  as 'Count'
                     from   branddetails bd WITH (nolock)
                              join vw_BrandConsensus v on bd.BrandID = v.BrandID
                              join mastercodesheet m  with (nolock) ON m.masterid = v.masterid        
                              join #subscriptionIDs s ON s.SubscriptionID = v.SubscriptionID
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and bd.BrandID = @BrandID 
                     order by count desc  
              End
              else
              Begin
                     select m.masterid as ID, m.masterValue as Value, m.MasterDesc as [Desc], m.MasterDesc1 as Desc1,
                           count( distinct s.SubscriptionID) as 'Count'
                     from   branddetails bd WITH (nolock)
                              join vw_RecentBrandConsensus vrbc on bd.BrandID = vrbc.BrandID
                              join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid               
                              join #subscriptionIDs s ON s.SubscriptionID = vrbc.SubscriptionID
                     where m.MasterGroupID =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and bd.BrandID = @BrandID and vrbc.MasterGroupID = @masterGroup
                     group by m.masterid, m.masterValue, m.MasterDesc, m.MasterDesc1
                     union all
                     select 0, 'YYY', 'NO RESPONSES',' ', 
                           count( distinct s.SubscriptionID) as 'Count'
                     from #subscriptionIDs s 
                           left outer join vw_RecentBrandConsensus vrbc on s.subscriptionid = vrbc.subscriptionid  and vrbc.BrandID = @BrandID  and MasterID in (select mc.MasterID from Mastercodesheet mc  with (nolock) where MasterGroupID = @masterGroup)
                     where  vrbc.SubscriptionID is null 
                     union all
                     select -1, 'ZZZ', 'TOTAL UNIQUE SUBSCRIBERS',' ', COUNT(distinct s.SubscriptionID)  as 'Count'
                     from   branddetails bd WITH (nolock)
                              join vw_RecentBrandConsensus vrbc on bd.BrandID = vrbc.BrandID
                              join mastercodesheet m  with (nolock) ON m.masterid = vrbc.masterid               
                              join #subscriptionIDs s ON s.SubscriptionID = vrbc.SubscriptionID
                     where m.mastergroupid =  @masterGroup  and (LEN(ltrim(rtrim(@Description))) = 0 or m.masterDesc like '%' + @Description + '%') and bd.BrandID = @BrandID and vrbc.MasterGroupID = @masterGroup 
                     order by count desc               
              end
       END
       
       --print 'end : ' + convert(varchar(100), getdate(), 109)
       
       drop table #subscriptionIDs
       
end
