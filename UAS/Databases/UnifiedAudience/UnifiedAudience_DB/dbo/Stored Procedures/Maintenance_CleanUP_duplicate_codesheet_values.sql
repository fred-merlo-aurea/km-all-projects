CREATE PROC Maintenance_CleanUP_duplicate_codesheet_values
as
Begin

	set nocount on
	
	declare @mastergroupID int,
			@deletecount int = 0,
			@insertcount int = 0
	
	--print 'start : ' + convert(varchar(100), getdate(), 109)
	
	declare @tmpWithPubcodes table (PubSubscriptionID int  NOT NULL PRIMARY KEY)
			
	declare @rebuildconsensusSubscriptionID table (SubscriptionID int)

	select @mastergroupID = mastergroupID from mastergroups mg where name = 'master_pubcodes' or displayname = 'PUBCODES' or displayname = 'PUBCODE'

	delete from PubSubscriptionDetail where PubSubscriptionID is null

	select PubSubscriptionID, SubscriptionID, CodesheetID, max(PubSubscriptionDetailID) as PubSubscriptionDetailID, count(PubSubscriptionDetailID) as counts
	into #tmp1
	from PubSubscriptionDetail with (nolock)
	group by PubSubscriptionID, SubscriptionID, CodesheetID 
	having COUNT(PubSubscriptionDetailID) > 1
	order by 5 desc
	
	--print '1 : ' + convert(varchar(100), getdate(), 109)

	select ps.PubSubscriptionDetailID , ps.SubscriptionID
	into #tmp2 
	from 
		#tmp1 t join 
		PubSubscriptionDetail ps with (nolock) on t.PubSubscriptionID = ps.PubSubscriptionID and t.SubscriptionID = ps.SubscriptionID and t.CodesheetID = ps.CodesheetID and ps.PubSubscriptionDetailID <> t.PubSubscriptionDetailID

	--print '2 : ' + convert(varchar(100), getdate(), 109)
	
	delete from PubSubscriptionDetail where PubSubscriptionDetailID in (select t.PubSubscriptionDetailID from #tmp2 t)

	select @deletecount = @@ROWCOUNT

	--print '3 : ' + convert(varchar(100), getdate(), 109)
	
	Insert into @rebuildconsensusSubscriptionID 
	select SubscriptionID from #tmp2 

	--print '4 : ' + convert(varchar(100), getdate(), 109)
	Insert into @tmpWithPubcodes
	select distinct PubSubscriptionID 
	from PubSubscriptionDetail psd 
	where CodesheetID  in (select distinct v.codesheetID from vw_Mapping V where ResponseGroupName = 'pubcode')
	
	--print '5 : ' + convert(varchar(100), getdate(), 109)

	declare @pubsubscriptionID int, @SubscriptionID int, @codesheetID int, @Qdate datetime

	select ps.PubSubscriptionID, ps.SubscriptionID, v.CodeSheetID, ps.Qualificationdate
	into #tmp3
	from 
			PubSubscriptions ps  join
			vw_Mapping v on ps.PubID = v.PubID and ResponseGroupName = 'pubcode' left outer join 
			@tmpWithPubcodes t on ps.PubSubscriptionID = t.PubSubscriptionID
	where
			t.PubSubscriptionID is null
			
	select @insertcount = @@ROWCOUNT

	--print '6 : ' + convert(varchar(100), getdate(), 109)
	
	Insert into @rebuildconsensusSubscriptionID 
	select SubscriptionID from #tmp3 
	
	--print '7 : ' + convert(varchar(100), getdate(), 109)

	DECLARE c_SUB CURSOR STATIC READ_ONLY FORWARD_ONLY FOR select * from #tmp3
	OPEN c_SUB    
	  
	FETCH NEXT FROM c_SUB INTO @pubsubscriptionID,  @SubscriptionID,  @codesheetID, @Qdate
	WHILE @@FETCH_STATUS = 0    
	BEGIN     
		
		insert into PubSubscriptionDetail values (@pubsubscriptionID,  @SubscriptionID,  @codesheetID, @Qdate, null, null, null, null )
		
		FETCH NEXT FROM c_SUB INTO @pubsubscriptionID,  @SubscriptionID,  @codesheetID, @Qdate  
	End    
	     
	CLOSE c_SUB    
	DEALLOCATE c_SUB 

	if @insertcount > 0 or @deletecount > 0
	BEGIN

		delete sd 
		from 
				SubscriptionDetails sd join 
				CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID 
		where 
			cmb.masterID in (select mc.masterID from mastercodesheet mc where mc.mastergroupID = @mastergroupID) and
			sd.SubscriptionID in (select SubscriptionID from @rebuildconsensusSubscriptionID)

		insert into subscriptiondetails
		select distinct subscriptionID, cb.masterID 
		from	
				PubSubscriptionDetail psd with (NOLOCK) join 
				CodeSheet_Mastercodesheet_Bridge cb  with (NOLOCK)  on psd.CodeSheetID = cb.CodeSheetID
		where
				cb.masterID in (select masterID from mastercodesheet mc where mc.mastergroupID = @mastergroupID) and
				SubscriptionID in (select SubscriptionID from @rebuildconsensusSubscriptionID)

		delete from dbo.SubscriberMasterValues where mastergroupID = @mastergroupID and
			SubscriptionID in (select SubscriptionID from @rebuildconsensusSubscriptionID)

		insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
		SELECT 
		  MasterGroupID, [SubscriptionID] , 
		  LEFT
		  (
			STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1 with (NOLOCK) join Mastercodesheet mc1 with (NOLOCK) on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH (''))
			,1,1,''), 8000
		  ) AS CombinedValues
		FROM 
			(
				SELECT distinct sd.SubscriptionID, mg.MasterGroupID
				FROM 
						[dbo].[SubscriptionDetails] sd with (NOLOCK) join 
						Mastercodesheet mc with (NOLOCK)on sd.MasterID = mc.MasterID join 
						MasterGroups mg with (NOLOCK) on mg.MasterGroupID = mc.MasterGroupID and
						SubscriptionID in (select SubscriptionID from @rebuildconsensusSubscriptionID)
			  where mc.mastergroupID = @mastergroupID
			)
		 Results
		GROUP BY [SubscriptionID] , MasterGroupID
		order by SubscriptionID		
	END

	drop table #tmp2
	drop table #tmp1
	drop table #tmp3
	
	--print 'end : ' + convert(varchar(100), getdate(), 109)
END