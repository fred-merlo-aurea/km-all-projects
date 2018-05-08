CREATE PROCEDURE [dbo].[sp_MergeSubscribers]
@SubscriptionIDToKeep  int,
@SubscriptionIDToRemove int,
@PubSubscriptionIDToKeep XML,
@PubSubscriptionIDToRemove XML,
@UserID int = null
as
BEGIN

	SET NOCOUNT ON

	IF OBJECT_ID('tempdb..#TempPubSubscriptionIDToKeep') IS NOT NULL 
		BEGIN 
				DROP TABLE #TempPubSubscriptionIDToKeep;
		END 
		      
	CREATE TABLE #TempPubSubscriptionIDToKeep (PubSubscriptionID int); 
		
	insert into #TempPubSubscriptionIDToKeep	
		select T.C.value('.', 'int')
		from @PubSubscriptionIDToKeep.nodes('/PubSubscriptionIDToKeep/PubSubscriptionID') as T(C);
					

	IF OBJECT_ID('tempdb..#TempPubSubscriptionIDToRemove') IS NOT NULL 
		BEGIN 
			DROP TABLE #TempPubSubscriptionIDToRemove;
		END 
		      
	CREATE TABLE #TempPubSubscriptionIDToRemove (PubSubscriptionID int); 
		
	insert into #TempPubSubscriptionIDToRemove	
		select T.C.value('.', 'int') 
		from @PubSubscriptionIDToRemove.nodes('/PubSubscriptionIDToRemove/PubSubscriptionID') as T(C);

	BEGIN TRY

		BEGIN TRANSACTION;
    
		set ANSI_WARNINGS  OFF					
		
		Insert into Log_MergeSubscriber (SubscriptionIDToKeep,SubscriptionIDToRemove,PubSubscriptionIDToKeep,PubSubscriptionIDToRemove,DateCreated,CreatedByUserID)
		values (@SubscriptionIDToKeep , @SubscriptionIDToRemove , convert(varchar(max), @PubSubscriptionIDToKeep), convert(varchar(max), @PubSubscriptionIDToRemove), GETDATE(), @UserID)

		delete 
		from PaidBillTo 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from SubscriberAddKillDetail
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from SubscriptionPaid 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from WaveMailingDetail 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from WaveMailSubscriber 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from IssueArchiveProductSubscriptionDetail 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from History 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from HistoryMarketingMap 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from HistoryPaid 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from HistoryPaidBillTo 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from HistoryResponseMap 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from HistorySubscription 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from IssueArchiveProductSubscription 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from IssueCloseSubGenMap 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from PubSubscriptionsExtension 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from SubscriberClickActivity 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from SubscriberOpenActivity 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from SubscriberTopicActivity 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from PubSubscriptionDetail 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)

		delete 
		from PubSubscriptions 
		where PubSubscriptionID in (select t.PubSubscriptionID from #TempPubSubscriptionIDToRemove t)
				
		select ps.PubSubscriptionID into #tmp1 
		from #TempPubSubscriptionIDToKeep t 
			join PubSubscriptions ps on t.pubsubscriptionID = ps.pubsubscriptionID
		where ps.SubscriptionID = @SubscriptionIDToRemove

		update History  
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update HistoryResponseMap 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update HistorySubscription 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update IssueArchiveProductSubscription 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update IssueArchiveProductSubscriptionDetail 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update WaveMailingDetail 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID in (select t.PubSubscriptionID from #tmp1 t)

		update SubscriberClickActivity 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID  in (select t.PubSubscriptionID from #tmp1 t)

		update SubscriberOpenActivity 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID  in (select t.PubSubscriptionID from #tmp1 t)

		update SubscriberTopicActivity 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID  in (select t.PubSubscriptionID from #tmp1 t)

		update PubSubscriptionDetail 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID  in (select t.PubSubscriptionID from #tmp1 t)

		update PubSubscriptions 
			set SubscriptionID = @SubscriptionIDToKeep 
			Where PubSubscriptionID  in (select t.PubSubscriptionID from #tmp1 t)

		delete 
		from ShoppingCarts 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from OrderDetails 
		where SubscriptionID  = @SubscriptionIDToRemove
		
		delete 
		from WaveMailingDetail 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from IssueArchiveProductSubscriptionDetail 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from History 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from HistoryResponseMap 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from HistorySubscription 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from IssueArchiveProductSubscription 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from PaidOrder 
		where SubscriptionID  = @SubscriptionIDToRemove
			
		delete 
		from BrandScore  
		where SubscriptionID = @SubscriptionIDToRemove

		delete 
		from CampaignFilterDetails  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriberClickActivity  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriberOpenActivity  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriberTopicActivity  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriberVisitActivity  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriberMasterValues 
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriptionsExtension  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from PubSubscriptionDetail  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from PubSubscriptions  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from SubscriptionDetails  
		where SubscriptionID  = @SubscriptionIDToRemove

		delete 
		from Subscriptions  
		where SubscriptionID  = @SubscriptionIDToRemove
		
		DROP TABLE #tmp1;
		DROP TABLE #TempPubSubscriptionIDToKeep;
		DROP TABLE #TempPubSubscriptionIDToRemove;
		
		declare @pubsubscriptionID int,
				@UpdateAddressinSubscriptions bit = 0,
				@ResetGeoCodesinSubscriptions bit = 0
		
		select top 1 @pubsubscriptionID =  ps.pubsubscriptionID ,
				@UpdateAddressinSubscriptions = (CASE WHEN ISNULL(ps.Address1,'')!='' AND ISNULL(ps.City,'')!='' AND ISNULL(ps.RegionCode,'')!='' AND ISNULL(ps.ZipCode,'')!='' THEN 1 ELSE 0 END)
		from PubSubscriptions ps with (NOLOCK)
		where SubscriptionID = @SubscriptionIDToKeep
		order by Qualificationdate desc, ISNULL(DateUpdated, DateCreated) desc
		
			
		if @UpdateAddressinSubscriptions = 1
			Begin
				if exists(	select top 1 1
							from Subscriptions s WITH (nolock) 
								join PubSubscriptions ps with (NOLOCK) on s.SubscriptionID = ps.SubscriptionID 
							where ps.PubSubscriptionID = @pubsubscriptionID and
								(
								ISNULL(ps.address1,'') <> ISNULL(s.address,'') or 
								ISNULL(ps.City,'') <> ISNULL(s.CITY,'') or 
								ISNULL(ps.RegionCode,'') <> ISNULL(s.STATE,'') or 
								ISNULL(ps.ZipCode,'') <>  ISNULL(s.ZIP,'') or 
								ISNULL(ps.CountryID,0) <> ISNULL(s.CountryID,0)
								)
						)
					Begin
						set @ResetGeoCodesinSubscriptions = 1 
					End
			End
				
		Update S
		Set [SEQUENCE]	= convert(int,isnull(ps.[SequenceID], 0)),
			FNAME		= (CASE WHEN ISNULL(ps.FirstName,'')!='' AND ISNULL(ps.LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.FirstName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FNAME END),
			LNAME		= (CASE WHEN ISNULL(ps.FirstName,'')!='' AND ISNULL(ps.LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.LastName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.LName END),
			TITLE		= (CASE WHEN ISNULL(ps.TITLE,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.TITLE END),
			COMPANY		= (CASE WHEN ISNULL(ps.COMPANY,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COMPANY END),
			ADDRESS		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.ADDRESS1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS END),
			MAILSTOP		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.Address2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.MAILSTOP END),
			ADDRESS3		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS3 END),               
			CITY			= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.CITY END),
			STATE		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.RegionCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.STATE END),
			ZIP			= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.ZipCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ZIP END),
			PLUS4		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PLUS4 END),
			--FORZIP		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FORZIP END),
			COUNTY		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COUNTY END),
			COUNTRY		= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(ps.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COUNTRY END),
			CountryID	= (CASE WHEN @UpdateAddressinSubscriptions = 1 THEN ps.CountryID ELSE s.CountryID END),
			Latitude		= (CASE WHEN @ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Latitude END),
			Longitude	= (CASE WHEN @ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Longitude END),
			IsLatLonValid = (CASE WHEN @ResetGeoCodesinSubscriptions = 1 THEN 0		ELSE s.IsLatLonValid END),
			LatLonMsg	= (CASE WHEN @ResetGeoCodesinSubscriptions = 1 THEN ''	ELSE s.LatLonMsg END),
			PHONE		= (CASE WHEN ISNULL(ps.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PHONE END),
			FAX			= (CASE WHEN ISNULL(ps.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FAX END),
			MOBILE		= (CASE WHEN ISNULL(ps.MOBILE,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.mobile END),
			CategoryID	= ps.PubCategoryID,
			TransactionID = ps.PubTransactionID,
			--TransactionDate = ps.TransactionDate,
			QDate		=  ps.Qualificationdate,
			Email		= (CASE WHEN ISNULL(ps.EMail,'')!='' THEN ps.Email ELSE s.EMAIL END),
			MailPermission			= (CASE WHEN ps.MailPermission is null THEN S.MailPermission ELSE ps.MailPermission END),
			FaxPermission			= (CASE WHEN ps.FaxPermission is null THEN S.FaxPermission ELSE ps.FaxPermission END),
			PhonePermission			= (CASE WHEN ps.PhonePermission is null THEN S.PhonePermission ELSE ps.PhonePermission END),
			OtherProductsPermission  = (CASE WHEN ps.OtherProductsPermission is null THEN S.OtherProductsPermission ELSE ps.OtherProductsPermission END),
			ThirdPartyPermission     = (CASE WHEN ps.ThirdPartyPermission is null THEN S.ThirdPartyPermission ELSE ps.ThirdPartyPermission END),
			EmailRenewPermission     = (CASE WHEN ps.EmailRenewPermission is null THEN S.EmailRenewPermission ELSE ps.EmailRenewPermission END),
			TextPermission			= (CASE WHEN ps.TextPermission is null THEN S.TextPermission ELSE ps.TextPermission END),
			Demo7		= case when isnull(ps.demo7,'') = '' then 'A' else ps.demo7 end,
			QSourceID	= case when isnull(ps.PubQSourceID, -1) > 0 then ps.PubQSourceID else S.QSourceID end,
			PAR3C		= ps.Par3CID,
			--IGRP_CNT		= ps.IGRP_CNT,
			emailexists	= (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(ps.EMail,'')!='' THEN ps.Email ELSE s.EMAIL END),''))) <> '' then 1 else 0 end), 
			Faxexists	= (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(ps.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FAX END),''))) <> '' then 1 else 0 end), 
			PhoneExists	= (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(ps.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PHONE END),''))) <> '' then 1 else 0 end),
			Gender		= (CASE WHEN ISNULL(ps.Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(ps.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.Gender END),
			IsMailable	= 
			(CASE 
				WHEN (@UpdateAddressinSubscriptions = 0 or (@UpdateAddressinSubscriptions = 1 and @ResetGeoCodesinSubscriptions = 0)) then s.IsMailable
				WHEN @ResetGeoCodesinSubscriptions = 1 and ps.COUNTRYID in(1 ,2) THEN 0
				WHEN ps.COUNTRYID >=3 and (LEN(ps.Address1) = 0 or LEN(ps.RegionCode ) = 0 or LEN(ps.Country) = 0)  THEN 0 ELSE 1
			END	),
			   
			SubSrc		= ps.SubSrcID,
			OrigsSrc		= ps.OrigsSrc,
			--Verified		= (CASE WHEN ISNULL(ps.Verified,'')!=''THEN ps.Verified ELSE s.Verified END),
			ExternalKeyId = (CASE WHEN ISNULL(ps.ExternalKeyId,'')!=''THEN ps.ExternalKeyId ELSE s.ExternalKeyId END),			   
			EmailID		= (CASE WHEN ISNULL(ps.EmailID,'')!=''THEN ps.EmailID ELSE s.EmailID END),
			DateUpdated	= GETDATE()
		From Subscriptions s 
			join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID 
		where ps.PubSubscriptionID = @pubsubscriptionID
	          
		delete sd 
		from SubscriptionDetails sd 
			join CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID
		Where sd.SubscriptionID = @SubscriptionIDToKeep 
				
		delete from SubscriberMasterValues
		Where SubscriptionID = @SubscriptionIDToKeep 

		insert into subscriptiondetails
		select distinct subscriptionID, cb.masterID 
		from PubSubscriptionDetail psd with (NOLOCK) 
			join CodeSheet_Mastercodesheet_Bridge cb  with (NOLOCK)  on psd.CodeSheetID = cb.CodeSheetID
		where SubscriptionID = @SubscriptionIDToKeep 	
		
		insert into SubscriberMasterValues (MasterGroupID, SubscriptionID, MastercodesheetValues)
		SELECT MasterGroupID, [SubscriptionID] , 
			STUFF((
			SELECT ',' + CAST([MasterValue] AS VARCHAR(MAX)) 
			FROM [dbo].[SubscriptionDetails] sd1 with (NOLOCK) 
				join Mastercodesheet mc1 with (NOLOCK) on sd1.MasterID = mc1.MasterID  
			WHERE (sd1.SubscriptionID = Results.SubscriptionID and mc1.MasterGroupID = Results.MasterGroupID) 
			FOR XML PATH (''))
			,1,1,'') AS CombinedValues
		FROM 
			(
				SELECT distinct sd.SubscriptionID, mg.MasterGroupID
				FROM [dbo].[SubscriptionDetails] sd with (NOLOCK) 
					join Mastercodesheet mc with (NOLOCK)on sd.MasterID = mc.MasterID 
					join MasterGroups mg on mg.MasterGroupID = mc.MasterGroupID
				where SubscriptionID = @SubscriptionIDToKeep 
			)
		 Results
		GROUP BY [SubscriptionID] , MasterGroupID
	 
		set ANSI_WARNINGS  ON
	    
		COMMIT TRANSACTION;
	END TRY
		
	BEGIN CATCH
			
		ROLLBACK TRANSACTION;
    
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
		RAISERROR (@ErrorMessage, -- Message text.
				   16, 1);
    
	END CATCH;

End