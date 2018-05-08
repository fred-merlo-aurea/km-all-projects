CREATE PROCEDURE [dbo].[job_ADMS_Remove_By_PubCode]
	@PubCode varchar(100)	
AS	 
BEGIN   

	SET NOCOUNT ON 
	Declare @PubID	int

	
	Select @PubID = (Select PubID from Pubs where PubCode = @PubCode)
	
	
	declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)
	create table #tblMissingIgrpno (SubscriptionID int, pubsubscriptionID int, Igrp_no uniqueidentifier)
	if (DB_NAME() like '%TradePress%' 
		AND DB_NAME() like '%MTG%' 
		AND DB_NAME() like '%France%')
		begin
			if exists (select top 1 1 from Pubs p with (NOLOCK) where p.PubCode = @PubCode and isnull(p.IsCirc,0) = 0)
				BEGIN
					Insert into #tblMissingIgrpno       
					SELECT s.SubscriptionID, ps.PubSubscriptionID, s.IGRP_NO   
					FROM PubSubscriptions ps  WITH (nolock) 
						join Subscriptions s  WITH (nolock) on ps.subscriptionID = s.SubscriptionID 
						join Pubs p  WITH (nolock) on ps.PubID = p.pubID 
					where p.PubCode = @pubcode

					select COUNT(*) 
					from #tblMissingIgrpno

					delete 
					from History 
					where PublicationID = @PubID
            
					delete 
					from HistoryResponseMap 
					where HistorySubscriptionID in (select HistorySubscriptionID from HistorySubscription where PubID  = @PubID)
					
					delete 
					from HistorySubscription 
					where PubID  = @PubID
      
					delete 
					from Batch 
					where PublicationID  = @PubID

					--delete 
					--from History 
					--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
		
					--delete 
					--from HistoryResponseMap 
					--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
		
					--delete 
					--from HistorySubscription 
					--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
		
					--delete 
					--from Batch 
					--where PublicationID  = @PubID

					delete 
					from SubscriberClickActivity
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from SubscriberOpenActivity 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from SubscriberTopicActivity 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from PubSubscriptionDetail 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from PubSubscriptionsExtension 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from SubscriberAddKillDetail 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					delete 
					from PubSubscriptions 
					where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

					insert into @tblDeleteSubscriptionIDs
					select distinct s.subscriptionID 
					from Subscriptions s  WITH (nolock) 
						left outer join PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
					where ps.SubscriptionID is null

					if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)  
						Begin                         
							  delete 
							  from BrandScore  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from CampaignFilterDetails  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriberClickActivity  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriberOpenActivity  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriberTopicActivity  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriberVisitActivity  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriberMasterValues  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriptionsExtension  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from PubSubscriptionDetail  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from PubSubscriptions  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from SubscriptionDetails  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

							  delete 
							  from Subscriptions  
							  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
						End

					-- repopulate concensus
					Delete from SubscriptionDetails
					Where SubscriptionID in (select distinct SubscriptionID from #tblMissingIgrpno) and
						MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge with(nolock))

					insert into SubscriptionDetails (SubscriptionID, MasterID)
					select distinct psd.SubscriptionID, cmb.masterID 
					from #tblMissingIgrpno t 
						join PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
						join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
						left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
					where sd.sdID is null       

					Delete 
					from SubscriberMasterValues
					Where SubscriptionID in (select SubscriptionID from #tblMissingIgrpno) 

					insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
					SELECT MasterGroupID, [SubscriptionID] , 
						STUFF((
							SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
							FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  
							join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
							WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
							FOR XML PATH (''))
						,1,1,'') AS CombinedValues
					FROM 
						(
						SELECT distinct sd.SubscriptionID, mc.MasterGroupID
						FROM #tblMissingIgrpno t 
							join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
							join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
						)
					Results
					GROUP BY [SubscriptionID] , MasterGroupID
					order by SubscriptionID    

					drop table #tblMissingIgrpno
				END
		END
	else
		begin
				Insert into #tblMissingIgrpno       
				SELECT s.SubscriptionID, ps.PubSubscriptionID, s.IGRP_NO   
				FROM PubSubscriptions ps  WITH (nolock) 
					join Subscriptions s  WITH (nolock) on ps.subscriptionID = s.SubscriptionID 
					join Pubs p  WITH (nolock) on ps.PubID = p.pubID 
				where p.PubCode = @pubcode

				select COUNT(*) 
				from #tblMissingIgrpno

				delete 
				from History 
				where PublicationID = @PubID
            
				delete 
				from HistoryResponseMap 
				where HistorySubscriptionID in (select HistorySubscriptionID from HistorySubscription where PubID  = @PubID)
					
				delete 
				from HistorySubscription 
				where PubID  = @PubID
      
				delete 
				from Batch 
				where PublicationID  = @PubID

				--delete 
				--from History 
				--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	
				--delete 
				--from HistoryResponseMap 
				--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	
				--delete 
				--from HistorySubscription 
				--where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
	
				--delete 
				--from Batch 
				--where PublicationID  = @PubID

				delete
				from SubscriberClickActivity 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from SubscriberOpenActivity 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from SubscriberTopicActivity 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from PubSubscriptionDetail
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from PubSubscriptionsExtension 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from SubscriberAddKillDetail 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				delete 
				from PubSubscriptions 
				where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)

				insert into @tblDeleteSubscriptionIDs
				select distinct s.subscriptionID 
				from Subscriptions s  WITH (nolock) 
					left outer join PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
				where ps.SubscriptionID is null

				if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)  
					Begin                         
						  delete 
						  from BrandScore  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from CampaignFilterDetails  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriberClickActivity  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriberOpenActivity  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriberTopicActivity  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriberVisitActivity  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriberMasterValues  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriptionsExtension  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from PubSubscriptionDetail  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from PubSubscriptions  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from SubscriptionDetails  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

						  delete 
						  from Subscriptions  
						  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
					End

				-- repopulate concensus
				Delete from SubscriptionDetails
				Where SubscriptionID in (select distinct SubscriptionID from #tblMissingIgrpno) and
					MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge with(nolock))

				insert into SubscriptionDetails (SubscriptionID, MasterID)
				select distinct psd.SubscriptionID, cmb.masterID 
				from #tblMissingIgrpno t 
					join PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
					join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
					left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
				where sd.sdID is null       

				Delete 
				from SubscriberMasterValues
				Where SubscriptionID in (select SubscriptionID from #tblMissingIgrpno) 

				insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
				SELECT 
					MasterGroupID, [SubscriptionID] , 
					STUFF((
						SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
						FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  
							join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
						WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
						FOR XML PATH (''))
					,1,1,'') AS CombinedValues
				FROM 
					(
					SELECT distinct sd.SubscriptionID, mc.MasterGroupID
					FROM #tblMissingIgrpno t 
						join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
						join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
					)
				Results
				GROUP BY [SubscriptionID] , MasterGroupID
				order by SubscriptionID    

				drop table #tblMissingIgrpno
			end

END