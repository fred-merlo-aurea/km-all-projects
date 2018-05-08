CREATE PROCEDURE [dbo].[job_ADMS_Remove_By_ProcessCode]
	@ProcessCode varchar(50)	
AS	
BEGIN   

	SET NOCOUNT ON 

	if (DB_NAME() not like '%TradePress%' 
		AND DB_NAME() not like '%MTG%' 
		AND DB_NAME() not like '%France%')
		BEGIN

			declare @subs Table (subscriptionID int, pubID int)
			declare @ps Table (PubSubscriptionID int)

			insert into @subs
			select distinct  s.SubscriptionID, p.pubID
			from subscriberfinal sf with(nolock) 
				join Subscriptions s with(nolock) on sf.igrp_no = s.igrp_no 
				join pubs p on p.pubcode = sf.pubcode
			where ProcessCode = @processCode and isupdatedinlive = 1

			insert into @ps
			select distinct ps.PubSubscriptionID 
			from PubSubscriptions ps with(nolock) 
				join @subs s on ps.SubscriptionID = s.subscriptionID and ps.PubID = s.pubID

			delete 
			from SubscriberClickActivity 
			where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

			delete 
			from SubscriberOpenActivity 
			where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

			delete 
			from SubscriberTopicActivity 
			where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

			delete 
			from PubSubscriptionDetail 
			where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

			delete 
			from PubSubscriptions 
			where PubSubscriptionID in (select ps.PubSubscriptionID from @ps ps)

			declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)
                  
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
			Delete 
			from SubscriptionDetails
			Where SubscriptionID in (select distinct s.SubscriptionID from @subs s) and
				MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge with(nolock))

			insert into SubscriptionDetails (SubscriptionID, MasterID)
			select distinct psd.SubscriptionID, cmb.masterID 
			from @subs t 
				join PubSubscriptionDetail psd on t.subscriptionID  = psd.SubscriptionID
				join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
				left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
			where sd.sdID is null       

			Delete from SubscriberMasterValues
			Where SubscriptionID in (select SubscriptionID from @subs s) 

			insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
			SELECT 
				MasterGroupID, [SubscriptionID] , 
				STUFF((
					SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
					FROM [dbo].[SubscriptionDetails] sd1  with (NOLOCK)  join Mastercodesheet mc1  with (NOLOCK) on sd1.MasterID = mc1.MasterID  
					WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
					FOR XML PATH (''))
				,1,1,'') AS CombinedValues
			FROM 
				(
				SELECT distinct sd.SubscriptionID, mc.MasterGroupID
				FROM @subs t 
					join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
					join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID                    
				)
			Results
			GROUP BY [SubscriptionID] , MasterGroupID
			order by SubscriptionID   

			delete SubscriberDemographicInvalid  
			where SORecordIdentifier in (select so.SORecordIdentifier from SubscriberOriginal so with(nolock) where ProcessCode = @processCode)

			delete SubscriberDemographicArchive 
			where SARecordIdentifier in (select sa.SARecordIdentifier from  SubscriberArchive sa with(nolock) where ProcessCode = @processCode)

			delete SubscriberDemographicFinal 
			where SFRecordIdentifier in (select sf.SFRecordIdentifier from  SubscriberFinal sf with(nolock) where ProcessCode = @processCode)

			delete SubscriberDemographicTransformed 
			where STRecordIdentifier in (select st.STRecordIdentifier from SubscriberTransformed st with(nolock) where ProcessCode = @processCode)

			delete SubscriberDemographicOriginal 
			where SORecordIdentifier in (select so.SORecordIdentifier from SubscriberOriginal so with(nolock) where ProcessCode = @processCode)

			delete SubscriberInvalid 
			where ProcessCode = @processCode

			delete SubscriberArchive 
			where ProcessCode = @processCode

			delete SubscriberFinal 
			where ProcessCode = @processCode

			delete SubscriberTransformed 
			where  ProcessCode = @processCode

			delete SubscriberOriginal 
			where  ProcessCode = @processCode

		END

END