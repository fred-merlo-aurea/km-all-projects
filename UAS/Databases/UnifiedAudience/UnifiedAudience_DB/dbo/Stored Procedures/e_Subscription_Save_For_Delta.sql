create procedure dbo.e_Subscription_Save_For_Delta
(
	@IncomingReferenceID varchar(100),
	@FullFile bit = 0,
	@DeleteUpdateMissingSubscribers varchar(10) = '' --delete or update
)
as 
BEGIN

	SET NOCOUNT ON

	Create table #tmpIncomingDataDetails (cdid int IDENTITY(1,1), PubSubscriptionID int not null,  pubid int  not null, responsegroup varchar(200), responsevalue varchar(200), NotExists bit)

	Create table #tmpProductDemographics (PubSubscriptionID int not null, pubID int  not null, responsevalue varchar(max))

	declare 
		    @s varchar(100),
			@b varchar(max)
				

	update API_IncomingData set ctry='2' where IncomingReferenceID = @IncomingReferenceID and country= 'canada'
	update API_IncomingData set CTRY = null where IncomingReferenceID = @IncomingReferenceID and  CTRY = ','
	update API_IncomingData set XACT = 10 where IncomingReferenceID = @IncomingReferenceID and  XACT = 0
	update API_IncomingData set cat = 10 where IncomingReferenceID = @IncomingReferenceID and  cat = 0

	/* Delete or Update Missing Igrpno in LIVE */
	
	if (@FullFile = 1 and len(@DeleteUpdateMissingSubscribers) > 0)
	Begin
		create table #tblMissingIgrpno (SubscriptionID int, pubsubscriptionID int, Igrp_no uniqueidentifier)
		
		Insert into #tblMissingIgrpno		
		SELECT		s.SubscriptionID, ps.PubSubscriptionID, s.IGRP_NO   
		FROM  
					PubSubscriptions ps  WITH (nolock) join 
					Subscriptions s  WITH (nolock) on ps.subscriptionID = s.SubscriptionID join 
					Pubs p  WITH (nolock) on ps.PubID = p.pubID left outer join
					[API_IncomingData] a  WITH (nolock) on IncomingReferenceID = @IncomingReferenceID and a.PubCode = p.PUBCODE and a.IGRP_NO = s.IGRP_NO
		where 
					p.PubCode in 
					(
						select distinct PUBCODE from API_IncomingData a WITH (nolock) WHERE IncomingReferenceID = @IncomingReferenceID
					) and 
					a.IGRP_NO is null 


		select COUNT(*) from #tblMissingIgrpno
		
		if (@DeleteUpdateMissingSubscribers = 'delete')
		Begin
			
			delete from SubscriberClickActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from SubscriberOpenActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from SubscriberTopicActivity where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from PubSubscriptionDetail where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			delete from PubSubscriptions where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
			
			declare @tblDeleteSubscriptionIDs table (SubscriptionID int PRIMARY KEY)
			
			insert into @tblDeleteSubscriptionIDs
			select distinct s.subscriptionID 
			from
					Subscriptions s  WITH (nolock) left outer join
					PubSubscriptions ps  WITH (nolock) on s.SubscriptionID = ps.SubscriptionID
			where
					ps.SubscriptionID is null

			if exists (select top 1 SubscriptionID from @tblDeleteSubscriptionIDs)	
			Begin					
				delete from BrandScore  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from CampaignFilterDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriberClickActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberOpenActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberTopicActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriberVisitActivity  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriberMasterValues  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from SubscriptionsExtension  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from PubSubscriptionDetail  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from PubSubscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )

				delete from SubscriptionDetails  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
				delete from Subscriptions  where SubscriptionID in (select T.SubscriptionID from @tblDeleteSubscriptionIDs t )
			End
			
			-- repopulate concensus
			Delete from SubscriptionDetails
			Where 
					SubscriptionID in (select distinct SubscriptionID from #tblMissingIgrpno) and
					MasterID in (select MasterID from CodeSheet_Mastercodesheet_Bridge)

			insert into SubscriptionDetails (SubscriptionID, MasterID)
			select distinct psd.SubscriptionID, cmb.masterID 
			from 
					#tblMissingIgrpno t join
					PubSubscriptionDetail psd with(nolock) on t.subscriptionID  = psd.SubscriptionID
					join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
					left outer join SubscriptionDetails sd with(nolock)  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
			where  
					sd.sdID is null	    

			Delete from SubscriberMasterValues
			Where 
					SubscriptionID in (select SubscriptionID from #tblMissingIgrpno) 

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
					FROM	
							#tblMissingIgrpno t 
							join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.subscriptionID
							join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID 			 
				)
			 Results
			GROUP BY [SubscriptionID] , MasterGroupID
			order by SubscriptionID    

		End
		else if (@DeleteUpdateMissingSubscribers = 'update')
		Begin
			update PubSubscriptions
			set PubTransactionID = 38
			where PubSubscriptionID in (select PubSubscriptionID from #tblMissingIgrpno)
		End
		
		drop table #tblMissingIgrpno
	End
	


	Create table #tbl1 (IncomingDataID int, Igrp_No uniqueidentifier, Igrp_Rank varchar(10), PubID int, SubscriptionID int, PubSubscriptionID int)

	CREATE CLUSTERED INDEX IDX_C_tbl1_IncomingDataID ON #tbl1(IncomingDataID)
    
    CREATE INDEX IDX_tbl1_Igrp_No ON #tbl1(Igrp_No)
    CREATE INDEX IDX_tbl1_SubscriptionID ON #tbl1(SubscriptionID)
    CREATE INDEX IDX_tbl1_PubSubscriptionID ON #tbl1(PubSubscriptionID)
    CREATE INDEX IDX_tbl1_Igrp_Rank ON #tbl1(Igrp_Rank)

	insert into #tbl1 (IncomingDataID, IGRP_NO, Igrp_Rank, PubID, SubscriptionID)
	SELECT IncomingDataID, a.IGRP_NO, a.IGRP_RANK, p.PubID, s.SubscriptionID
	FROM 
		API_IncomingData a WITH (nolock)
		left outer join Subscriptions s WITH (nolock) on a.IGRP_NO = s.IGRP_NO
		JOIN Pubs p WITH (nolock) ON p.PubCode = a.PubCode
	WHERE 
		IncomingReferenceID = @IncomingReferenceID  --and a.email = 'ifynzeka@yahoo.com'

	Print (  'Insert into #tbl1 COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

	update t
	set t.PubSubscriptionID = ps.PubSubscriptionID
	from #tbl1 t join PubSubscriptions ps on t.PubID = ps.PubID and t.SubscriptionID = ps.SubscriptionID

	Print ('Update @tbl PubSubscriptionID COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

	BEGIN TRY
		BEGIN TRANSACTION;

		DECLARE @TranIDFromValue int = (Select TransactionCodeValue From UAD_Lookup..TransactionCode where TransactionCodeValue = 10)
		DECLARE @CatIDFromValue int = (Select CategoryCodeValue From UAD_Lookup..CategoryCode where CategoryCodeValue = 10)
		
		UPDATE API_IncomingData
		SET CAT = @CatIDFromValue
		WHERE IncomingReferenceID = @IncomingReferenceID 
		AND (isnull(CAT,0) = 0 OR CAT NOT IN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock)))

		UPDATE API_IncomingData
		SET XACT = @TranIDFromValue
		WHERE IncomingReferenceID = @IncomingReferenceID 
		AND (isnull(XACT,0) = 0 OR XACT NOT IN (SELECT TransactionCodeValue FROM UAD_Lookup..[TransactionCode] with(nolock)))

		UPDATE API_IncomingData
		set CAT = a.CategoryCodeID,
			XACT = a.TransactionCodeID
		FROM API_IncomingData ai 
		join uad_lookup..vw_Action a on a.CategoryCodeValue = ai.CAT and a.TransactionCodeValue = ai.XACT and actiontypeid = 2 and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
		WHERE IncomingReferenceID = @IncomingReferenceID 
    
	    delete	ps
	    from	PubSubscriptionDetail ps 
				join #tbl1 t on t.PubSubscriptionID = ps.PubSubscriptionID 
				join CodeSheet c on c.CodeSheetID = ps.CodesheetID 
				join ResponseGroups rg on rg.ResponseGroupID = c.ResponseGroupID 
	    Where 
				rg.ResponseGroupName <> 'TOPICS'
	    
	    Print ('DELETE PubSubscriptionDetail COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		delete	sd
		from	SubscriptionDetails sd
				join (select distinct subscriptionID from #tbl1) t  on sd.SubscriptionID = t.SubscriptionID 
				join CodeSheet_Mastercodesheet_Bridge cmb on sd.MasterID = cmb.MasterID 
				 
		Print ('Delete SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		delete	smv
		from	SubscriberMasterValues smv
				join (select distinct subscriptionID from #tbl1) t  on smv.SubscriptionID = t.SubscriptionID 
				 
		Print ('Delete SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    	    
		Update S1
		Set
			[SEQUENCE] = convert(int,isnull(a.[SEQUENCE], 0)),
			FNAME		= CASE WHEN ISNULL(a.FNAME, '') = '' and ISNULL(a.LNAME, '') = '' THEN s1.FNAME ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			LNAME		= CASE WHEN ISNULL(a.LNAME, '') = '' and ISNULL(a.LNAME, '') = '' THEN s1.LNAME ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100)  END,
			TITLE		= CASE WHEN ISNULL(a.TITLE, '') = '' THEN s1.TITLE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100)  END,
			COMPANY		= CASE WHEN ISNULL(a.COMPANY, '') = '' THEN s1.COMPANY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			ADDRESS     = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.ADDRESS END),
			MAILSTOP    = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.MAILSTOP END),
			CITY        = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.CITY END),
			STATE       = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.STATE END),
			ZIP         = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.ZIP END),
			PLUS4       = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!=''
							  THEN REPLACE(REPLACE(REPLACE(a.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.PLUS4 END),
			FORZIP      = (CASE WHEN ISNULL(a.Address,'')!='' AND ISNULL(a.City,'')!='' AND ISNULL(a.State,'')!='' AND ISNULL(a.ZIP,'')!='' AND ISNULL(a.FORZIP,'') !='' 
							  THEN REPLACE(REPLACE(REPLACE(a.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s1.FORZIP END),
			COUNTY		= CASE WHEN ISNULL(a.COUNTY, '') = '' THEN s1.COUNTY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 20) END,
			COUNTRY		= CASE WHEN ISNULL(a.COUNTRY, '') = '' THEN s1.COUNTRY ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			CountryID   = CASE WHEN ISNULL(a.CTRY,'') = '' THEN s1.CountryID ELSE CONVERT(int,a.CTRY) END,
			PHONE		= CASE WHEN ISNULL(a.PHONE, '') = '' THEN s1.PHONE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			FAX			= CASE WHEN ISNULL(a.FAX, '') = '' THEN s1.FAX ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 30) END,
			MOBILE		= CASE WHEN ISNULL(a.MOBILE, '') = '' THEN s1.MOBILE ELSE SUBSTRING(REPLACE(REPLACE(REPLACE(a.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100) END,
			CategoryID	= case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end,
			TransactionID = CONVERT(INT,a.XACT),
			TransactionDate = CONVERT(VARCHAR(10),a.XACTDATE,101),
			QDate		=  CONVERT(VARCHAR(10),a.QDATE,101),
			Email		= SUBSTRING(a.Email, 1, 100),
			MailPermission	= case when a.Demo31 is null then 1 when a.Demo31 ='Y' then 1 else 0 end,
			FaxPermission	= case when a.Demo32 is null then 1 when a.Demo32 ='Y' then 1 else 0 end,
			PhonePermission	= case when a.Demo33 is null then 1 when a.Demo33 ='Y' then 1 else 0 end,
			OtherProductsPermission	= case when a.Demo34 is null then 1 when a.Demo34 ='Y' then 1 else 0 end,
			ThirdPartyPermission	= case when a.Demo35 is null then 1 when a.Demo35 ='Y' then 1 else 0 end,
			EmailRenewPermission	= case when a.Demo36 is null then 1 when a.Demo36 ='Y' then 1 else 0 end,
			Demo7		=  a.Demo7,
			QSourceID	= DBO.FN_GetQSourceID(a.QSOURCE),
			PAR3C		= SUBSTRING(a.PAR3C, 1, 1),
			IGRP_NO		= a.IGRP_NO, 
			IGRP_CNT	= CONVERT(INT,a.IGRP_CNT),
			emailexists = (case when ltrim(rtrim(isnull(a.email,''))) <> '' then 1 else 0 end), 
			Faxexists	= (case when ltrim(rtrim(isnull(a.Fax,''))) <> '' then 1 else 0 end), 
			PhoneExists = (case when ltrim(rtrim(isnull(a.PHONE,''))) <> '' then 1 else 0 end)
		From 
				Subscriptions s1
				join #tbl1 t on s1.SubscriptionID = t.SubscriptionID 
				join api_incomingdata a on a.IncomingDataID  = t.IncomingDataID
		where 
				t.Igrp_Rank = 'M'

		Print ('Update Subscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		Update [Subscriptions_DQM] 
		Set ZZ_PAR_ADDRESS_STD =  a.ZZ_PAR_ADDRESS_STD,
			ZZ_PAR_CITY_STD =  a.ZZ_PAR_CITY_STD,
			ZZ_PAR_COMPANY_MATCH1 =  a.ZZ_PAR_COMPANY_MATCH1,
			ZZ_PAR_COMPANY_MATCH2 =  a.ZZ_PAR_COMPANY_MATCH2,
			ZZ_PAR_COMPANY_STD =  a.ZZ_PAR_COMPANY_STD,
			ZZ_PAR_COMPANY2 =  a.ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD =  a.ZZ_PAR_EMAIL_STD,
			ZZ_PAR_FNAME_MATCH1 =  a.ZZ_PAR_FNAME_MATCH1,
			ZZ_PAR_FNAME_MATCH2 =  a.ZZ_PAR_FNAME_MATCH2,
			ZZ_PAR_FNAME_MATCH3 =  a.ZZ_PAR_FNAME_MATCH3,
			ZZ_PAR_FNAME_MATCH4 =  a.ZZ_PAR_FNAME_MATCH4,
			ZZ_PAR_FNAME_MATCH5 =  a.ZZ_PAR_FNAME_MATCH5,
			ZZ_PAR_FNAME_MATCH6 =  a.ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD =  a.ZZ_PAR_FNAME_STD,
			ZZ_PAR_FORZIP_STD =  a.ZZ_PAR_FORZIP_STD,
			ZZ_PAR_INTL_PHONE =  a.ZZ_PAR_INTL_PHONE,
			ZZ_PAR_LNAME_STD =  a.ZZ_PAR_LNAME_STD,
			ZZ_PAR_MAILSTOP_STD = a.ZZ_PAR_MAILSTOP_STD,
			ZZ_PAR_PLUS4_STD =  a.ZZ_PAR_PLUS4_STD,
			ZZ_PAR_POBOX =  a.ZZ_PAR_POBOX,
			ZZ_PAR_POSTCODE =  a.ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER =  a.ZZ_PAR_PRIMARY_NUMBER,
			ZZ_PAR_PRIMARY_POSTFIX =  a.ZZ_PAR_PRIMARY_POSTFIX,
			ZZ_PAR_PRIMARY_PREFIX =  a.ZZ_PAR_PRIMARY_PREFIX,
			ZZ_PAR_PRIMARY_STREET =  a.ZZ_PAR_PRIMARY_STREET,
			ZZ_PAR_PRIMARY_TYPE =  a.ZZ_PAR_PRIMARY_TYPE,
			ZZ_PAR_RR_BOX =  a.ZZ_PAR_RR_BOX,
			ZZ_PAR_RR_NUMBER =  a.ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD =  a.ZZ_PAR_STATE_STD,
			ZZ_PAR_TITLE_STD =  a.ZZ_PAR_TITLE_STD,
			ZZ_PAR_UNIT_DESCRIPTION =  a.ZZ_PAR_UNIT_DESCRIPTION,
			ZZ_PAR_UNIT_NUMBER =  a.ZZ_PAR_UNIT_NUMBER,
			ZZ_PAR_USCAN_PHONE =  a.ZZ_PAR_USCAN_PHONE,
			ZZ_PAR_ZIP_STD = a.ZZ_PAR_ZIP_STD
		from 
				[Subscriptions_DQM]  s 
				join  #tbl1 t on s.SubscriptionID = t.SubscriptionID 
				join api_incomingdata a on a.IncomingDataID  = t.IncomingDataID 
		where 
				t.Igrp_Rank = 'M'
	    
	    Print ('Update [Subscriptions_DQM] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    Insert into [Subscriptions] 
			(	
				[SEQUENCE], FNAME, LNAME, TITLE, COMPANY, ADDRESS, MAILSTOP, CITY, STATE, ZIP, PLUS4,FORZIP,COUNTY,COUNTRY,CountryID,PHONE,MOBILE,FAX,EMAIL,
				CategoryID, TransactionID, TransactionDate,QDate, 
				QSourceID,PAR3C,
				MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,EmailRenewPermission, 
				IGRP_NO, IGRP_CNT,
				emailexists, Faxexists, PhoneExists 
			)
		select
				convert(int,[SEQUENCE]) as sequence, 
				SUBSTRING(REPLACE(REPLACE(REPLACE(FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 255),
				SUBSTRING(REPLACE(REPLACE(REPLACE(MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 255),
				SUBSTRING(REPLACE(REPLACE(REPLACE(CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1,50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1,10),
				SUBSTRING(REPLACE(REPLACE(REPLACE(PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 4),
				SUBSTRING(REPLACE(REPLACE(REPLACE(FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 50),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 20),
				SUBSTRING(REPLACE(REPLACE(REPLACE(COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				CONVERT(int,CTRY),
				SUBSTRING(REPLACE(REPLACE(REPLACE(PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 30),
				SUBSTRING(REPLACE(REPLACE(REPLACE(FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				SUBSTRING(REPLACE(REPLACE(REPLACE(EMAIL, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 1, 100),
				case when CONVERT(INT,CAT) =0  then NULL else CONVERT(INT,CAT) end as CategoryID, 
				CONVERT(INT,XACT) as TransactionID, 
				CONVERT(VARCHAR(10),XACTDATE,101) as TransactionDate, 
				CONVERT(VARCHAR(10),QDATE,101) as QDate, 
				DBO.FN_GetQSourceID(QSOURCE) as QSourceID, 
				SUBSTRING(PAR3C, 1, 1),
				case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end as Demo31, 
				case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end as Demo32, 
				case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end as Demo33, 
				case when DEMO34 is null then 1 when DEMO34 ='Y' then 1 else 0 end as Demo34, 
				case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end as Demo35, 
				case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end as Demo36, 
				a.Igrp_no, CONVERT(INT,IGRP_CNT) as IGRP_CNT,
				(case when ltrim(rtrim(isnull(email,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(Fax,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(PHONE,''))) <> '' then 1 else 0 end)
		from 
				#tbl1 t 
				join  api_incomingdata a with (NOLOCK) on t.IncomingDataID = a.IncomingDataID 
		where 
				t.SubscriptionID is null  and 
				t.Igrp_Rank = 'M' 

	    Print ('Insert [Subscriptions] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
				
		Insert into [Subscriptions_DQM] 
		(	
			SubscriptionID, IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
		)
		select
			s.SubscriptionID, s.IGRP_NO, ZZ_PAR_ADDRESS_STD,ZZ_PAR_CITY_STD,ZZ_PAR_COMPANY_MATCH1,ZZ_PAR_COMPANY_MATCH2,ZZ_PAR_COMPANY_STD,ZZ_PAR_COMPANY2,
			ZZ_PAR_EMAIL_STD,ZZ_PAR_FNAME_MATCH1,ZZ_PAR_FNAME_MATCH2,ZZ_PAR_FNAME_MATCH3,ZZ_PAR_FNAME_MATCH4,ZZ_PAR_FNAME_MATCH5,ZZ_PAR_FNAME_MATCH6,
			ZZ_PAR_FNAME_STD,ZZ_PAR_FORZIP_STD,ZZ_PAR_INTL_PHONE,ZZ_PAR_LNAME_STD,ZZ_PAR_MAILSTOP_STD,ZZ_PAR_PLUS4_STD,ZZ_PAR_POBOX,ZZ_PAR_POSTCODE,
			ZZ_PAR_PRIMARY_NUMBER,ZZ_PAR_PRIMARY_POSTFIX,ZZ_PAR_PRIMARY_PREFIX,ZZ_PAR_PRIMARY_STREET,ZZ_PAR_PRIMARY_TYPE,ZZ_PAR_RR_BOX,ZZ_PAR_RR_NUMBER,
			ZZ_PAR_STATE_STD,ZZ_PAR_TITLE_STD,ZZ_PAR_UNIT_DESCRIPTION,ZZ_PAR_UNIT_NUMBER,ZZ_PAR_USCAN_PHONE,ZZ_PAR_ZIP_STD
		from
			#tbl1 t 
			join Subscriptions s  with (NOLOCK) on t.IGRP_NO = s.IGRP_NO 
			join api_incomingdata a  with (NOLOCK) on t.IncomingDataID = a.IncomingDataID 
		where 
			t.SubscriptionID is null and  
			t.Igrp_Rank = 'M'   
	    
	    Print ('Insert [Subscriptions_DQM] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
	    Update t
	    set t.SubscriptionID = s.subscriptionID
		FROM #tbl1 t join Subscriptions s WITH (nolock) on t.IGRP_NO = s.IGRP_NO
		WHERE t.SubscriptionID is null

		Print ('Update #tbl1 SubscriptioniID COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
		UPDATE ps
		SET [demo7] = case when isnull(a.demo7,'') = '' then 'A' else a.demo7 end
			,[Qualificationdate] = a.QDate
			,[PubCategoryID] = case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end
			,[PubTransactionID] = CONVERT(INT,a.XACT)
			,[Email] = a.EMAIL
			,[EmailStatusID] =  case when es.EmailStatusID is null then 1 else  es.EmailStatusID end
			,[StatusUpdatedDate] = case when ps.emailstatusID = (case when es.EmailStatusID is null then 1 else  es.EmailStatusID end) then ps.StatusUpdatedDate else GETDATE() end
			,PubQSourceID = DBO.FN_GetQSourceID(QSOURCE)
			,SequenceID = SEQUENCE
			,Verify = VERIFIED
			,FirstName = FNAME
			,LastName = LNAME
			,Company = a.COMPANY
			,Title = a.TITLE
			,Address1 = ADDRESS
			,Address2 = MAILSTOP			
			,City = a.CITY
			,RegionCode = STATE
			,ZipCode = ZIP
			,Plus4 = a.PLUS4
			,County = a.COUNTY
			,Country = a.COUNTRY
			,CountryID = CTRY
			,Phone = a.PHONE
			,Fax = a.FAX
			,PubTransactionDate = XACTDATE
			,MailPermission = case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end
			,FaxPermission = case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end
			,PhonePermission = case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end
			,OtherProductsPermission = case when DEMO34 is null then 1 when DEMO34 ='Y' then 1 else 0 end
			,ThirdPartyPermission = case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end
			,EmailRenewPermission = case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end
		from 
				PubSubscriptions ps 
				join #tbl1 t on ps.PubSubscriptionID = t.PubSubscriptionID 
				join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
				left outer join EmailStatus es on es.Status = a.EMAILSTATUS
				
	    Print ('Update pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    select *
		from
				#tbl1 t
		Where 
				t.SubscriptionID is null
	    
	    INSERT INTO pubsubscriptions 
	    (
			SubscriptionID,PubID,demo7,Qualificationdate,PubQSourceID, PubCategoryID,PubTransactionID,Email,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,
			FirstName,LastName,Company,Title,Address1,Address2,City,RegionCode,ZipCode,Plus4,County,Country,CountryID,Phone,
			Fax,PubTransactionDate,MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,EmailRenewPermission
		)
		select t.SubscriptionID , t.PubID, case when isnull(a.demo7,'') = '' then 'A' else a.demo7 end, a.QDATE, 
		DBO.FN_GetQSourceID(a.QSOURCE), 
		case when CONVERT(INT,a.CAT) =0  then NULL else CONVERT(INT,a.CAT) end, CONVERT(INT,a.XACT) as TransactionID,
		a.Email,
		case when es.EmailStatusID is null then 1 else es.EmailStatusID end, a.QDATE, '',
		FNAME,LNAME,COMPANY,TITLE,ADDRESS,MAILSTOP,CITY,STATE,ZIP,PLUS4,COUNTY,COUNTRY,CTRY,PHONE,
		FAX,XACTDATE,
		case when Demo31 is null then 1 when Demo31 ='Y' then 1 else 0 end as Demo31, 
		case when DEMO32 is null then 1 when DEMO32 ='Y' then 1 else 0 end as Demo32, 
		case when DEMO33 is null then 1 when DEMO33 ='Y' then 1 else 0 end as Demo33, 
		case when DEMO34 is null then 1 when DEMO34 ='Y' then 1 else 0 end as Demo34, 
		case when DEMO35 is null then 1 when DEMO35 ='Y' then 1 else 0 end as Demo35, 
		case when DEMO36 is null then 1 when DEMO36 ='Y' then 1 else 0 end as Demo36
		from
				#tbl1 t
				join API_IncomingData a on t.IncomingDataID = a.IncomingDataID
				left outer join EmailStatus es on es.Status = a.EMAILSTATUS
		Where 
				t.PubSubscriptionID is null
	    
	    Print ('Insert pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
		    
		update t
		set t.PubSubscriptionID = ps.PubSubscriptionID
		from #tbl1 t join PubSubscriptions ps on t.PubID = ps.PubID and t.SubscriptionID = ps.SubscriptionID
		where t.PubSubscriptionID is null

		Print ('Update @tbl PubSubscriptionID COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
	    declare @rgname varchar(50),
				@PubSubscriptionID int, 
				@pubID int, 
				@responsevalue varchar(max)
		
		DECLARE c_ProductDemographics CURSOR FOR 
		select distinct ResponseGroupName from responsegroups 

		OPEN c_ProductDemographics  
		FETCH NEXT FROM c_ProductDemographics INTO @rgname

		WHILE @@FETCH_STATUS = 0  
		BEGIN  	
		
			truncate table #tmpProductDemographics
			
			exec ('
			insert into #tmpIncomingDataDetails	 (PubSubscriptionID, pubid, responsegroup, responsevalue, notExists)
			select distinct t.PubSubscriptionID, p.pubID, ''' + @rgname + ''', a.[' + @rgname + '], 1 from #tbl1 t join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
			join Pubs p on p.pubID = t.pubID join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(a.[' + @rgname + '])) <> '''' and CHARINDEX('','', a.[' + @rgname + ']) = 0')
			 
			exec ('
			insert into #tmpProductDemographics 
			select t.PubSubscriptionID, p.pubID, a.[' + @rgname + '] from #tbl1 t join API_IncomingData a on a.IncomingDataID = t.IncomingDataID
			join Pubs p on p.pubID = t.pubID join ResponseGroups rg on p.PubID = rg.PubID
			where rg.ResponseGroupName = ''' + @rgname + ''' and LTRIM(rtrim(a.[' + @rgname + '])) <> '''' and CHARINDEX('','', a.[' + @rgname + ']) > 0')
			
			DECLARE c_ProductDemographicsData CURSOR FOR 
			select PubSubscriptionID , pubID , responsevalue  from  #tmpProductDemographics  
			
			OPEN c_ProductDemographicsData  
			FETCH NEXT FROM c_ProductDemographicsData INTO @PubSubscriptionID, @pubID, @responsevalue

			WHILE @@FETCH_STATUS = 0  
			BEGIN  					
				insert into #tmpIncomingDataDetails (PubSubscriptionID, pubid, responsegroup, responsevalue, NotExists)
				select @PubSubscriptionID, @pubId, @rgname, (case when PATINDEX('%[^0-9]%', items) = 0 and (Items not like '%$%' and Items not like '%.%') then CONVERT(varchar(100),CONVERT(int,items)) else items end), 1  
				from dbo.fn_split(@RESPONSEVALUE, ',')
			
				FETCH NEXT FROM c_ProductDemographicsData INTO  @PubSubscriptionID, @pubID, @responsevalue
			END

			CLOSE c_ProductDemographicsData  
			DEALLOCATE c_ProductDemographicsData  
			
			FETCH NEXT FROM c_ProductDemographics INTO  @rgname
		END

		CLOSE c_ProductDemographics  
		DEALLOCATE c_ProductDemographics  
		
		Print ('c_ProductDemographics complete COUNT : ' + ' / ' + convert(varchar(20), getdate(), 114))
		
		update #tmpIncomingDataDetails 
		set responsevalue = REPLACE(responsevalue, ' ' ,'')
		
		insert into PubSubscriptionDetail (PubSubscriptionID, SubscriptionID, CodeSheetID)
		select   ps.PubSubscriptionID, ps.SubscriptionID, cs.CodeSheetID--, idetail.responsegroup, idetail.responsevalue
		from 
			PubSubscriptions ps 
			join #tmpIncomingDataDetails idetail on ps.PubSubscriptionID = idetail.PubSubscriptionID 
			join ResponseGroups rg on idetail.responsegroup = rg.ResponseGroupName and idetail.pubid = rg.PubID
			join CodeSheet cs on rg.ResponseGroupID = cs.ResponseGroupID and rg.PubID = cs.PubID and 
				(case when LEN(idetail.responsevalue)= 1 and ISNUMERIC(idetail.responsevalue) = 1 then '0' + idetail.Responsevalue else idetail.responsevalue end) 
				= 
				(case when LEN(cs.responsevalue)= 1 and ISNUMERIC(cs.responsevalue) = 1 then '0' + cs.Responsevalue else cs.responsevalue end)
		order by ps.subscriptionID, cs.codesheetID	    
	    
	    Print ('Insert PubSubscriptionDetail COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		insert into SubscriptionDetails (SubscriptionID, MasterID)
		select distinct psd.SubscriptionID, cmb.masterID 
		from 
				(select distinct subscriptionID from #tbl1) t  
				join PubSubscriptionDetail psd on t.SubscriptionID = psd.SubscriptionID 
				join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
				left outer join SubscriptionDetails sd  on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
		where  
				sd.sdID is null	    
		
		Print ('Insert SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		/***** Final Step *****/
	    
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
				FROM	
						(select distinct subscriptionID from #tbl1) t 
						join [dbo].[SubscriptionDetails] sd  with (NOLOCK) on t.SubscriptionID = sd.SubscriptionID
						join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID 			 
			)
		 Results
		GROUP BY [SubscriptionID] , MasterGroupID
		order by SubscriptionID    
	    
		Print ('Insert into SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
	    
	    /*
		********** Insert/Update SubscriptionsExtension
		*/
		
		if exists (select top 1 * from SubscriptionsExtensionMapper where Active = 1)
		Begin
		
			DECLARE @ColumnName VARCHAR(255)
			DECLARE @FieldName VARCHAR(10)
			DECLARE @ColumnNamesCsv AS VARCHAR(MAX)
			DECLARE @FieldNamesCsv AS VARCHAR(MAX)
			DECLARE @WhereOneIsNotNull AS VARCHAR(MAX),
					@Updatestring AS VARCHAR(MAX)
						
			
			--copy data out of the old table into the new extenstion table
			DECLARE c CURSOR LOCAL FAST_FORWARD FOR SELECT CustomField, StandardField FROM SubscriptionsExtensionMapper  where Active = 1
			OPEN c
			FETCH NEXT FROM c INTO @ColumnName, @FieldName
			WHILE @@FETCH_STATUS = 0  
			BEGIN 

--				set @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = [' + @ColumnName + ']', @FieldName + ' = [' + @ColumnName + ']') 		
--			    SET @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' THEN [' + @fieldName + '] ELSE [' + @ColumnName + '] END ', @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') = '''' THEN [' + @fieldName + '] ELSE [' + @ColumnName + '] END ')
-- CHANGED 5/28/2015 per Task 22236 request
				SET @Updatestring =  ISNULL(@Updatestring + ', ' + @FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' or ISNULL([' + @ColumnName + '],'''' ) = ISNULL([' + @fieldName + '], '''') THEN [' + @fieldName + '] WHEN ISNULL([' + @FieldName + '], '''') = '''' THEN CAST([' + @ColumnName + '] AS VARCHAR(2048)) ELSE CAST([' + @FieldName + '] + '','' + [' + @ColumnName + '] AS VARCHAR(2048)) END',@FieldName + ' = CASE WHEN ISNULL([' + @ColumnName + '], '''') ='''' or ISNULL([' + @ColumnName + '], '''') = ISNULL([' + @fieldName + '],'''') THEN [' + @fieldName + '] WHEN ISNULL([' + @FieldName + '], '''') = '''' THEN CAST([' + @ColumnName + '] AS VARCHAR(2048)) ELSE CAST([' + @FieldName + '] + '','' + [' + @ColumnName + '] AS VARCHAR(2048)) END')
				SET @ColumnNamesCsv = ISNULL(@ColumnNamesCsv + ', [' + @ColumnName + ']', '[' + @ColumnName + ']')
				SET @FieldNamesCsv = ISNULL(@FieldNamesCsv + ', ' + @FieldName, @FieldName)
				
				IF(@WhereOneIsNotNull IS NULL)
				BEGIN
					SET @WhereOneIsNotNull = '[' + @ColumnName + ']' + ' IS NOT NULL'
				END
				ELSE
				BEGIN
					SET @WhereOneIsNotNull = @WhereOneIsNotNull + ' OR ' + '[' + @ColumnName + ']' + ' IS NOT NULL'
				END
			
				FETCH NEXT FROM c INTO @ColumnName, @FieldName 
			END 
			CLOSE c
			DEALLOCATE c
			
			EXEC ('Update s1 Set ' + @Updatestring + '
			FROM SubscriptionsExtension s1 join #tbl1 t on s1.subscriptionID = t.subscriptionID join api_incomingdata a on t.incomingdataID = a.incomingdataID
			WHERE t.igrp_rank=''M''')
		
			Print ('Update SubscriptionsExtension COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
		
			EXEC ('INSERT INTO SubscriptionsExtension (SubscriptionID, ' + @FieldNamesCsv + ') 
			SELECT t.subscriptionID, ' + @ColumnNamesCsv + '
			FROM #tbl1 t join api_incomingdata a on t.incomingdataID = a.incomingdataID left outer join SubscriptionsExtension se on se.subscriptionID = t.subscriptionID
			WHERE t.igrp_rank=''M'' AND se.subscriptionID is null AND (' + @WhereOneIsNotNull + ')')

     		Print ('Insert SubscriptionsExtension COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
			
			if DB_NAME() = 'NorthstarmasterDB' 
			Begin
				update ps
				set pubcategoryID = 115, pubtransactionID = 111
				from pubsubscriptions ps join pubs p on p.pubID = ps.pubID
				where 
					p.pubcode in ('WEB_BEAT','WEB_BTP','WEB_INTELLIGUIDE','WEB_PCWI','WEB_STAR','WEB_T42','WEB_TWPLUS') 
					and PubTransactionID = 101
					and ( PubCategoryID = 101 or PubCategoryID = 114)

				update ps
				set pubcategoryID = 115, pubtransactionID = 130
				from pubsubscriptions ps join pubs p on p.pubID = ps.pubID
				where 
					p.pubcode in ('WEB_BEAT','WEB_BTP','WEB_INTELLIGUIDE','WEB_PCWI','WEB_STAR','WEB_T42','WEB_TWPLUS') 
					and PubTransactionID = 109
					and
					( PubCategoryID = 101 or PubCategoryID = 104 or PubCategoryID = 114 )

				update ps
				set pubcategoryID = 115, pubtransactionID = 111
				from pubsubscriptions ps join pubs p on p.pubID = ps.pubID
				where 
					p.pubcode in ('WEB_BEAT','WEB_BTP','WEB_INTELLIGUIDE','WEB_PCWI','WEB_STAR','WEB_T42','WEB_TWPLUS') 
					and 
					NOT  (( PubCategoryID = 115 and PubTransactionID = 111) or (PubCategoryID = 115 and PubTransactionID = 130 ))
			End
			
			
			update a
			set a.processed = 1
			from API_IncomingData a join #tbl1 t on a.IncomingDataID = t.IncomingDataID
			where t.PubSubscriptionID is not null
			
			Print ('Update API_IncomingData COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
			
		End
	    
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		SELECT
		ERROR_NUMBER() AS ErrorNumber
		,ERROR_SEVERITY() AS ErrorSeverity
		,ERROR_STATE() AS ErrorState
		,ERROR_PROCEDURE() AS ErrorProcedure
		,ERROR_LINE() AS ErrorLine
		,ERROR_MESSAGE() AS ErrorMessage;	
		
		Print 'ERROR'
			
		ROLLBACK TRANSACTION;
		
		SET @s = 'API Import Notification Failed ' + convert(varchar(100),DB_NAME());
		SELECT @b = ERROR_MESSAGE() 

  --  EXEC msdb..sp_send_dbmail 
		--@profile_name='SQLAdmin', 
		--@recipients='sunil@knowledgemarketing.com', 
		--@importance='High',
		--@body_format = 'HTML',
		--@subject= @s, 
		--@body=@b
    
		
	END CATCH;


	drop table #tmpIncomingDataDetails 

	drop table #tmpProductDemographics 
	
	drop table #tbl1

End