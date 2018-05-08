--IF EXISTS (SELECT 1 FROM Sysobjects where name = 'e_ImportFromUAS')
--DROP Proc e_ImportFromUAS
--GO


--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--execute [e_ImportFromUAS] @processcode = '0UT7WL0Rs7AA_03242017_13:45:34'


CREATE PROCEDURE [dbo].[e_ImportFromUAS] 
@ProcessCode varchar(50),
@MailPermissionOverRide bit = 'false',
@FaxPermissionOverRide bit = 'false',
@PhonePermissionOverRide bit = 'false',
@OtherProductsPermissionOverRide bit = 'false',
@ThirdPartyPermissionOverRide bit = 'false',
@EmailRenewPermissionOverRide bit = 'false',
@TextPermissionOverRide bit = 'false',
@FileType varchar(50) = '',
@OverwriteValues varchar(max) = '',
@ReplaceValues varchar(max) = ''

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET XACT_ABORT ON;
	SET NOCOUNT ON

--declare @ProcessCode varchar(50) = '5o05Bs7o737w_01162017_02:16:02',
--@MailPermissionOverRide bit = 'false',
--@FaxPermissionOverRide bit = 'false',
--@PhonePermissionOverRide bit = 'false',
--@OtherProductsPermissionOverRide bit = 'false',
--@ThirdPartyPermissionOverRide bit = 'false',
--@EmailRenewPermissionOverRide bit = 'false',
--@TextPermissionOverRide bit = 'false',
--@FileType varchar(50) = '',
--@OverwriteValues varchar(max) = '',
--@ReplaceValues varchar(max) = ''	


	--Drop table #MatchGroups
	CREATE TABLE #MatchGroups(
		PubMatchFound Int default 0
		,SubscriberMatchFound int default 0
		,SubscriberFinalID int
		,SubscriptionID int
		,PubSubscriptionID int
		,SFRecordIdentifier UNIQUEIDENTIFIER
		,IsOriginal bit
		,PubID int
		,Igrp_no UNIQUEIDENTIFIER
		,FName varchar(100)
		,LName varchar(100)
		,Company varchar(100)
		,Title varchar(100)
		,Address varchar(255)
		,city varchar(50)
		,State varchar(50)
		,Phone varchar(100)
		,Email varchar(100)
		,FName3 varchar(3)
		,LName6 varchar(6)
		,Address15 varchar(15)
		,Company8 varchar(8)
		,RecordType varchar(20)
		,MatchID UNIQUEIDENTIFIER
		,SubscriptionMasterRecord bit default 0
		,Qdate DateTime
		,UpdateAddressinSubscriptions bit default 0
		,ResetGeoCodesinSubscriptions bit default 0
		 )

	CREATE INDEX idx_MatchGroups_SFRecordIdentifier ON #MatchGroups(SFRecordIdentifier)
	CREATE INDEX idx_MatchGroups_MatchID_RecordType ON #MatchGroups(MatchID, RecordType)
	CREATE INDEX idx_MatchGroups_SubscriptionID ON #MatchGroups(subscriptionID)
	CREATE INDEX idx_MatchGroups_IgrpNo_RecordType ON #MatchGroups(Igrp_no, RecordType)
	CREATE INDEX idx_MatchGroups_PubsubscriptionID ON #MatchGroups(PubSubscriptionID)
	CREATE INDEX idx_MatchGroups_SubscriberFinalID ON #MatchGroups(SubscriberFinalID)

	CREATE INDEX idx_MatchGroups_Fname_Lname ON #MatchGroups(Fname, Lname)
	CREATE INDEX idx_MatchGroups_Fname3_Lname6 ON #MatchGroups(Fname3, Lname6)
	CREATE INDEX idx_MatchGroups_Phone ON #MatchGroups(Phone)
	CREATE INDEX idx_MatchGroups_Address ON #MatchGroups(Address)
	CREATE INDEX idx_MatchGroups_Company ON #MatchGroups(Company)
	CREATE INDEX idx_MatchGroups_Address15 ON #MatchGroups(Address15)
	CREATE INDEX idx_MatchGroups_Company8 ON #MatchGroups(Company8)
	CREATE INDEX idx_MatchGroups_Email ON #MatchGroups(Email)

	Create table #psdNewDimensions (PubSubscriptionID int, ResponseGroupName varchar(100))
	Create table #PubBatch (PubID int, BatchID int)
	Create table #HistoryResponseMap (HistoryResponseMapID int, PubSubscriptionID int)
	Create table #HistorySubscription (HistorySubscriptionID int, SubscriptionID int, PubSubscriptionID int)
	Create table #History  (HistoryID int, PubSubscriptionID int)

	CREATE INDEX idx_#psdNewDimensions_PubSubscriptionID ON #History(PubSubscriptionID)

	CREATE INDEX idx_PubBatch_BatchID ON #PubBatch(BatchID)

	CREATE INDEX idx_History_HistoryID ON #History(HistoryID)
	CREATE INDEX idx_History_PubSubscriptionID ON #History(PubSubscriptionID)
	
	CREATE INDEX idx_HistoryResponseMap_HistoryResponseMapID ON #HistoryResponseMap(HistoryResponseMapID)
	CREATE INDEX idx_HistoryRespinseMap_PubSubscriptionID ON #HistoryResponseMap(PubSubscriptionID)
	
	CREATE INDEX idx_HistorySubscription_HistorySubscriptionID ON #HistorySubscription(HistorySubscriptionID)
	CREATE INDEX idx_HistorySubscription_SubscriptionID ON #HistorySubscription(SubscriptionID)
	CREATE INDEX idx_HistorySubscription_PubSubscriptionID ON #HistorySubscription(PubSubscriptionID)

	declare @s varchar(100),
			@b varchar(max)
			
	if (@FileType <> 'QuickFill')
	BEGIN
		update SubscriberFinal set CategoryID = 10 where ProcessCode = @ProcessCode and Ignore = 0 and isUpdatedinLIVE = 0 and  ISNULL(CategoryID,0) = 0
		update SubscriberFinal set TransactionID = 10 where ProcessCode = @ProcessCode and  Ignore = 0 and isUpdatedinLIVE = 0 and  ISNULL(TransactionID,0) = 0
	END

	/******************** CountryID update to Null for -1 ******************************************/
	update SubscriberFinal set CountryID = null where CountryID = -1 and ProcessCode = @ProcessCode

	----/*********************** Temporary Fix for merge Master & Subordinate Demographics ***************************/
	----create table #tmpDupes (ProcessCode varchar(100), Igrp_no uniqueidentifier, pubcode varchar(100),  subscriberFinalID int) --SFRecordIdentifier uniqueidentifier,

	----CREATE INDEX IDX_tmpDupes_1 ON #tmpDupes(ProcessCode, Igrp_no, pubcode)

	----insert into #tmpDupes
	----select sf.ProcessCode, Igrp_no, PubCode, MAX(case when igrp_rank = 'M' then SubscriberFinalID end)  --, MAX(case when igrp_rank = 'M' then SFRecordIdentifier end)
	----from SubscriberFinal sf With(NoLock) 
	----	join (	select distinct ProcessCode 
	----			from SubscriberFinal sf With(NoLock) 
	----			where Ignore = 0 and IsUpdatedInLive = 0) t on sf.ProcessCode = t.ProcessCode
	----where sf.ProcessCode = @ProcessCode
	----group by sf.ProcessCode, IGrp_No, PubCode
	----having COUNT(SubscriberFinalID) > 1

	----insert into SubscriberDemographicFinal (PubID,SFRecordIdentifier,MAFField,Value,NotExists,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ResponseOther)
	----select distinct sdf.PubID, sf1.SFRecordIdentifier, sdf.MAFField, sdf.Value, sdf.NotExists, GETDATE(), GETDATE(), sdf.CreatedByUserID, sdf.UpdatedByUserID, sdf.ResponseOther
	----from #tmpDupes t
	----	join SubscriberFinal sf  WITH (nolock) on t.ProcessCode = sf.ProcessCode and t.Igrp_no = sf.IGrp_No and t.pubcode = sf.PubCode and t.subscriberFinalID <> sf.SubscriberFinalID
	----	join SubscriberDemographicFinal sdf  WITH (nolock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier 
	----	join SubscriberFinal sf1  WITH (nolock) on sf1.SubscriberFinalID = t.SubscriberFinalID
	----	left outer join SubscriberDemographicFinal sdf1  WITH (nolock) on sf1.SFRecordIdentifier = sdf1.SFRecordIdentifier and sdf1.MAFField = sdf.MAFField and sdf1.Value = sdf.Value
	----where sdf1.SDFinalID is null		
			
	----drop table #tmpDupes

	--
	-- GET Temp SubscriberFile table loaded
	--
	Select * 
	into #SubscriberFinalRollup
	from Subscriberfinal 
	WHERE ProcessCode = @ProcessCode
		and Ignore = 0 
		and isUpdatedinLIVE = 0
	
	CREATE CLUSTERED INDEX idx_SubscriberFinalRollup_SFRecordIdentifier ON #SubscriberFinalRollup(SFRecordIdentifier)
	CREATE INDEX idx_SubscriberFinalRollup_Igrp_No ON #SubscriberFinalRollup(Igrp_No)
	CREATE INDEX idx_SubscriberFinalRollup_subscriberfinalID ON #SubscriberFinalRollup(subscriberfinalID)
	
	Print ('Get Subscriber Final COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

	--
	--  LOAD #Matchgroups temp table 
	--		this table will hold/mark the master record, if there are duplicate records across pubs
	--

	INSERT INTO #MatchGroups(SubscriberFinalID, IGrp_no, SFRecordIdentifier,FName,LName,Company,Title,Address,city,State ,Phone ,Email ,FName3 ,LName6 ,Address15,Company8,IsOriginal, PubID, RecordType, MatchID, Qdate)
		SELECT distinct SubscriberFinalID, IGrp_no, SFRecordIdentifier, isnull(FName,''), isnull(LName,''), isnull(Company,''), isnull(Title, ''), isnull(Address,''),ISNULL(city,''), isnull(State,'') , isnull(Phone,'') , isnull(Email,'') , 
						left(isnull(FName,''),3) , left(isnull(LName,''),6) , left(isnull(Address,''),15), left(isnull(Company,''),8 ), 1, p.PubID, NULL, NULL, IsNull(QDate, GetDate())
			FROM #SubscriberFinalRollup sf
			JOIN pubs p  WITH (nolock) on p.PubCode = sf.PubCode

		Print ('Build MatchGroup COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))


	-------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Look for matching records within ProcessCode for all PubCodes(PubID)   IGRP_no links the matching records
	-------------------------------------------------------------------------------------------------------------------------------------------------------

	--
	-- Mark Subscription Master record (row = 1) for 1 distinct IGrp_No
	--
	;WITH cte AS
	(
		select ROW_NUMBER() over (partition by Igrp_no order by Igrp_no, QDate DESC) as PartitionRow,* from #MatchGroups
	)
	UPDATE cte SET SubscriptionMasterRecord = 1
	WHERE PartitionRow = 1


	--
	-- Mark Pub Master record (row = 1) for 1 distinct PubID, IGrp_No
	--
	;WITH cte AS
	(
		select ROW_NUMBER() over (partition by PubID, Igrp_no order by PubID, Igrp_no, QDate DESC) as PartitionRow,* /* MatchID, RecordType */ from #MatchGroups
	)
	UPDATE cte SET MatchID = NEWID(), RecordType = 'Master'
	WHERE PartitionRow = 1

	-- mark remaining records as duplicate and set MatchID to master
	update x set x.MatchID = mg.MatchID, RecordType = 'Duplicate'
	from #MatchGroups x
	join #MatchGroups mg on mg.RecordType = 'Master' AND mg.Igrp_no = x.Igrp_no
	where x.MatchID is null

--select recordtype as rt, * from #matchgroups order by matchid, recordtype desc

	update mg
	set mg.UpdateAddressinSubscriptions = 
			CASE WHEN ISNULL(mg.Address,'')<>'' AND ISNULL(mg.City,'')<>'' AND ISNULL(mg.State,'')<>'' THEN 1 ELSE 0 END
	from #MatchGroups mg 

	update mg
	set mg.ResetGeoCodesinSubscriptions = 1
	from	#MatchGroups mg 
		join #SubscriberFinalRollup sf WITH (nolock)  on mg.SFRecordIdentifier = sf.SFRecordIdentifier 
		join Subscriptions s WITH (nolock) on sf.IGRP_NO = s.IGRP_NO 
		join PubSubscriptions ps WITH (nolock) on ps.SubscriptionID = s.SubscriptionID 
	Where  
		UpdateAddressinSubscriptions = 1 and
		(
			ISNULL(sf.address,'') <> ISNULL(ps.address1,'') or 
			ISNULL(sf.City,'') <> ISNULL(ps.CITY,'') or 
			ISNULL(sf.State,'') <> ISNULL(ps.RegionCode,'') or 
			ISNULL(sf.CountryID,0) <> ISNULL(ps.CountryID,0)
		)

	--
	--  Rollup duplicate SubscriberFinal records into Master
	--
	
	DECLARE @MatchID			UniqueIdentifier		
	DECLARE @SFRecordIdentifier	UniqueIdentifier		
	DECLARE @MAFField varchar(100)
	
	DECLARE c_MatchIDs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR select Distinct MatchID from #MatchGroups where RecordType = 'Duplicate' and IsOriginal = 1

	OPEN c_MatchIDs  
	FETCH NEXT FROM c_MatchIDs INTO @MatchID

	-- Loop through all MatchIDs
	WHILE @@FETCH_STATUS = 0  
	BEGIN  
	--SELECT 'MatchID = ' + Convert(varchar(100), @MatchID)

		DECLARE c_DupRecs CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR 
		Select mg.SfRecordIdentifier from #MatchGroups mg where MatchID = @MatchID order by QDate ASC
		OPEN c_DupRecs  

		FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
		WHILE @@FETCH_STATUS = 0  
		BEGIN  
	--SELECT 'Duplicate SFRecordIdentifier = ' + IsNull(Convert(varchar(100), @SFRecordIdentifier), '')
			UPDATE mstr set 
				mstr.QDate = CASE when dups.Qdate > mstr.QDate THEN dups.Qdate else mstr.qdate end,
				mstr.QSourceID=CASE when ISNULL(dups.QSourceID,'') <> '' AND (ISNULL(mstr.QSourceID,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.QSourceID ELSE mstr.QSourceID END ,
				-- move first and last name from the same record
				mstr.FName = case when (ISNULL(dups.FName, '') <> '' AND ISNULL(dups.LName, '') <> '') AND (ISNULL(mstr.FName, '') = '' OR ISNULL(mstr.LName, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.FName else mstr.FName end,
				mstr.LName = case when (ISNULL(dups.FName, '') <> '' AND ISNULL(dups.LName, '') <> '') AND (ISNULL(mstr.FName, '') = '' OR ISNULL(mstr.LName, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.LName else mstr.LName end,
				mstr.Email = CASE when ISNULL(dups.Email, '') <> '' AND (IsNull(mstr.Email, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Email else mstr.Email end,
				-- move all adress info from the same record, do not split the fields up
				mstr.address = case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Address else mstr.address end, 
				mstr.MailStop=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.MailStop ELSE mstr.MailStop END ,
				mstr.Address3=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Address3 ELSE mstr.Address3 END ,
				mstr.city = case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) then dups.City else mstr.city end, 
				mstr.state = case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) then dups.state else mstr.state end,
				mstr.Zip=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Zip ELSE mstr.Zip END ,
				mstr.Plus4=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Plus4 ELSE mstr.Plus4 END ,
				mstr.ForZip=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.ForZip ELSE mstr.ForZip END ,
				mstr.County=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.County ELSE mstr.County END ,
				mstr.Country=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Country ELSE mstr.Country END ,
				mstr.CountryID=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.CountryID ELSE mstr.CountryID END ,
				mstr.Home_Work_Address=case when (ISNULL(dups.Address, '') <> '' OR (ISNULL(dups.City, '') <> '' AND ISNULL(dups.State, '') <> '')) AND (ISNULL(mstr.Address, '') = '' OR ISNULL(mstr.city, '') = '' OR ISNULL(mstr.state, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Home_Work_Address ELSE mstr.Home_Work_Address END ,
				mstr.Company = CASE when ISNULL(dups.Company, '') <> '' AND (IsNull(mstr.Company, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Company else mstr.Company end,
				mstr.Title=CASE when ISNULL(dups.Title,'') <> '' AND (ISNULL(mstr.Title,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Title ELSE mstr.Title END ,
				mstr.Phone = CASE when ISNULL(dups.Phone, '') <> '' AND (IsNull(mstr.Phone, '') = '' OR dups.Qdate > mstr.QDate) THEN dups.Phone else mstr.Phone end,
				mstr.Fax=CASE when ISNULL(dups.Fax,'') <> '' AND (ISNULL(mstr.Fax,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Fax ELSE mstr.Fax END ,
				mstr.Mobile=CASE when ISNULL(dups.Mobile,'') <> '' AND (ISNULL(mstr.Mobile,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Mobile ELSE mstr.Mobile END ,
				mstr.Gender=CASE when ISNULL(dups.Gender,'') <> '' AND (ISNULL(mstr.Gender,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Gender ELSE mstr.Gender END ,
				mstr.Sequence=CASE when ISNULL(dups.Sequence,0) <> 0 AND (ISNULL(mstr.Sequence,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Sequence ELSE mstr.Sequence END ,
				mstr.CategoryID=CASE when ISNULL(dups.CategoryID,0) <> 0 AND (ISNULL(mstr.CategoryID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.CategoryID ELSE mstr.CategoryID END ,
				mstr.TransactionID=CASE when ISNULL(dups.TransactionID,0) <> 0 AND (ISNULL(mstr.TransactionID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.TransactionID ELSE mstr.TransactionID END ,
				mstr.TransactionDate=CASE when ISNULL(dups.TransactionDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.TransactionDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.TransactionDate ELSE mstr.TransactionDate END ,
				mstr.RegCode=CASE when ISNULL(dups.RegCode,'') <> '' AND (ISNULL(mstr.RegCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.RegCode ELSE mstr.RegCode END ,
				mstr.Verified=CASE when ISNULL(dups.Verified,'') <> '' AND (ISNULL(mstr.Verified,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Verified ELSE mstr.Verified END ,
				mstr.SubSrc=CASE when ISNULL(dups.SubSrc,'') <> '' AND (ISNULL(mstr.SubSrc,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SubSrc ELSE mstr.SubSrc END ,
				mstr.OrigsSrc=CASE when ISNULL(dups.OrigsSrc,'') <> '' AND (ISNULL(mstr.OrigsSrc,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.OrigsSrc ELSE mstr.OrigsSrc END ,
				mstr.Par3C=CASE when ISNULL(dups.Par3C,'') <> '' AND (ISNULL(mstr.Par3C,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Par3C ELSE mstr.Par3C END ,
				mstr.Source=CASE when ISNULL(dups.Source,'') <> '' AND (ISNULL(mstr.Source,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Source ELSE mstr.Source END ,
				mstr.Priority=CASE when ISNULL(dups.Priority,'') <> '' AND (ISNULL(mstr.Priority,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Priority ELSE mstr.Priority END ,
				mstr.StatList=CASE when ISNULL(dups.StatList,0) <> 0 AND (ISNULL(mstr.StatList,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.StatList ELSE mstr.StatList END ,
				mstr.Sic=CASE when ISNULL(dups.Sic,'') <> '' AND (ISNULL(mstr.Sic,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Sic ELSE mstr.Sic END ,
				mstr.SicCode=CASE when ISNULL(dups.SicCode,'') <> '' AND (ISNULL(mstr.SicCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SicCode ELSE mstr.SicCode END ,
				mstr.Demo7=CASE when ISNULL(dups.Demo7,'') <> '' AND (ISNULL(mstr.Demo7,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Demo7 ELSE mstr.Demo7 END ,
				mstr.Latitude=CASE when ISNULL(dups.Latitude,0) <> 0 AND (ISNULL(mstr.Latitude,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Latitude ELSE mstr.Latitude END ,
				mstr.Longitude=CASE when ISNULL(dups.Longitude,0) <> 0 AND (ISNULL(mstr.Longitude,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Longitude ELSE mstr.Longitude END ,
				mstr.IsLatLonValid=CASE when ISNULL(dups.IsLatLonValid,0) <> 0 AND (ISNULL(mstr.IsLatLonValid,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsLatLonValid ELSE mstr.IsLatLonValid END ,
				mstr.LatLonMsg=CASE when ISNULL(dups.LatLonMsg,'') <> '' AND (ISNULL(mstr.LatLonMsg,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.LatLonMsg ELSE mstr.LatLonMsg END ,
				mstr.EmailStatusID=CASE when ISNULL(dups.EmailStatusID,0) <> 0 AND (ISNULL(mstr.EmailStatusID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.EmailStatusID ELSE mstr.EmailStatusID END ,
				mstr.Ignore=CASE when ISNULL(dups.Ignore,0) <> 0 AND (ISNULL(mstr.Ignore,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Ignore ELSE mstr.Ignore END ,
				mstr.IsMailable=CASE when ISNULL(dups.IsMailable,0) <> 0 AND (ISNULL(mstr.IsMailable,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsMailable ELSE mstr.IsMailable END ,
				mstr.IsActive=CASE when ISNULL(dups.IsActive,0) <> 0 AND (ISNULL(mstr.IsActive,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsActive ELSE mstr.IsActive END ,
				mstr.ExternalKeyId=CASE when ISNULL(dups.ExternalKeyId,0) <> 0 AND (ISNULL(mstr.ExternalKeyId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.ExternalKeyId ELSE mstr.ExternalKeyId END ,
				mstr.AccountNumber=CASE when ISNULL(dups.AccountNumber,'') <> '' AND (ISNULL(mstr.AccountNumber,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.AccountNumber ELSE mstr.AccountNumber END ,
				mstr.EmailID=CASE when ISNULL(dups.EmailID,0) <> 0 AND (ISNULL(mstr.EmailID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.EmailID ELSE mstr.EmailID END ,
				mstr.Copies=CASE when ISNULL(dups.Copies,0) <> 0 AND (ISNULL(mstr.Copies,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.Copies ELSE mstr.Copies END ,
				mstr.GraceIssues=CASE when ISNULL(dups.GraceIssues,0) <> 0 AND (ISNULL(mstr.GraceIssues,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.GraceIssues ELSE mstr.GraceIssues END ,
				mstr.IsComp=CASE when ISNULL(dups.IsComp,0) <> 0 AND (ISNULL(mstr.IsComp,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsComp ELSE mstr.IsComp END ,
				mstr.IsPaid=CASE when ISNULL(dups.IsPaid,0) <> 0 AND (ISNULL(mstr.IsPaid,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsPaid ELSE mstr.IsPaid END ,
				mstr.IsSubscribed=CASE when ISNULL(dups.IsSubscribed,0) <> 0 AND (ISNULL(mstr.IsSubscribed,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IsSubscribed ELSE mstr.IsSubscribed END ,
				mstr.Occupation=CASE when ISNULL(dups.Occupation,'') <> '' AND (ISNULL(mstr.Occupation,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Occupation ELSE mstr.Occupation END ,
				mstr.SubscriptionStatusID=CASE when ISNULL(dups.SubscriptionStatusID,0) <> 0 AND (ISNULL(mstr.SubscriptionStatusID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubscriptionStatusID ELSE mstr.SubscriptionStatusID END ,
				mstr.SubsrcID=CASE when ISNULL(dups.SubsrcID,0) <> 0 AND (ISNULL(mstr.SubsrcID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubsrcID ELSE mstr.SubsrcID END ,
				mstr.Website=CASE when ISNULL(dups.Website,'') <> '' AND (ISNULL(mstr.Website,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.Website ELSE mstr.Website END ,

				-- Permission fields, do not base on Qdate,  IF permission for any is 0, use 0					
				mstr.MailPermission=CASE when mstr.MailPermission IS NULL OR (dups.MailPermission IS NOT NULL AND ISNULL(dups.MailPermission, 0) = 0) THEN dups.MailPermission ELSE mstr.MailPermission END ,
				mstr.FaxPermission=CASE when mstr.FaxPermission IS NULL OR (dups.FaxPermission IS NOT NULL AND ISNULL(dups.FaxPermission,0) = 0) THEN dups.FaxPermission ELSE mstr.FaxPermission END ,
				mstr.PhonePermission=CASE when mstr.PhonePermission IS NULL OR (dups.PhonePermission IS NOT NULL AND ISNULL(dups.PhonePermission,0) = 0) THEN dups.PhonePermission ELSE mstr.PhonePermission END ,
				mstr.OtherProductsPermission=CASE when mstr.OtherProductsPermission IS NULL OR (dups.OtherProductsPermission IS NOT NULL AND ISNULL(dups.OtherProductsPermission,0) = 0) THEN dups.OtherProductsPermission ELSE mstr.OtherProductsPermission END ,
				mstr.ThirdPartyPermission=CASE when mstr.ThirdPartyPermission IS NULL OR (dups.ThirdPartyPermission IS NOT NULL AND ISNULL(dups.ThirdPartyPermission,0) = 0) THEN dups.ThirdPartyPermission ELSE mstr.ThirdPartyPermission END ,
				mstr.EmailRenewPermission=CASE when mstr.EmailRenewPermission IS NULL OR (dups.EmailRenewPermission IS NOT NULL AND ISNULL(dups.EmailRenewPermission,0) = 0) THEN dups.EmailRenewPermission ELSE mstr.EmailRenewPermission END ,
				mstr.TextPermission=CASE when mstr.TextPermission IS NULL OR (dups.TextPermission IS NOT NULL AND ISNULL(dups.TextPermission,0) = 0) THEN dups.TextPermission ELSE mstr.TextPermission END ,

				mstr.SubGenSubscriberID=CASE when ISNULL(dups.SubGenSubscriberID,0) <> 0 AND (ISNULL(mstr.SubGenSubscriberID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriberID ELSE mstr.SubGenSubscriberID END ,
				mstr.SubGenSubscriptionID=CASE when ISNULL(dups.SubGenSubscriptionID,0) <> 0 AND (ISNULL(mstr.SubGenSubscriptionID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionID ELSE mstr.SubGenSubscriptionID END ,
				mstr.SubGenPublicationID=CASE when ISNULL(dups.SubGenPublicationID,0) <> 0 AND (ISNULL(mstr.SubGenPublicationID,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenPublicationID ELSE mstr.SubGenPublicationID END ,
				mstr.SubGenMailingAddressId=CASE when ISNULL(dups.SubGenMailingAddressId,0) <> 0 AND (ISNULL(mstr.SubGenMailingAddressId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenMailingAddressId ELSE mstr.SubGenMailingAddressId END ,
				mstr.SubGenBillingAddressId=CASE when ISNULL(dups.SubGenBillingAddressId,0) <> 0 AND (ISNULL(mstr.SubGenBillingAddressId,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenBillingAddressId ELSE mstr.SubGenBillingAddressId END ,
				mstr.IssuesLeft=CASE when ISNULL(dups.IssuesLeft,0) <> 0 AND (ISNULL(mstr.IssuesLeft,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.IssuesLeft ELSE mstr.IssuesLeft END ,
				mstr.UnearnedReveue=CASE when ISNULL(dups.UnearnedReveue,0) <> 0 AND (ISNULL(mstr.UnearnedReveue,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.UnearnedReveue ELSE mstr.UnearnedReveue END ,
				mstr.SubGenIsLead=CASE when ISNULL(dups.SubGenIsLead,0) <> 0 AND (ISNULL(mstr.SubGenIsLead,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.SubGenIsLead ELSE mstr.SubGenIsLead END ,
				mstr.SubGenRenewalCode=CASE when ISNULL(dups.SubGenRenewalCode,'') <> '' AND (ISNULL(mstr.SubGenRenewalCode,'') = '' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenRenewalCode ELSE mstr.SubGenRenewalCode END ,
				mstr.SubGenSubscriptionRenewDate=CASE when ISNULL(dups.SubGenSubscriptionRenewDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionRenewDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionRenewDate ELSE mstr.SubGenSubscriptionRenewDate END ,
				mstr.SubGenSubscriptionExpireDate=CASE when ISNULL(dups.SubGenSubscriptionExpireDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionExpireDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionExpireDate ELSE mstr.SubGenSubscriptionExpireDate END ,
				mstr.SubGenSubscriptionLastQualifiedDate=CASE when ISNULL(dups.SubGenSubscriptionLastQualifiedDate,'1/1/1900') <> '1/1/1900' AND (ISNULL(mstr.SubGenSubscriptionLastQualifiedDate,'1/1/1900') = '1/1/1900' OR dups.Qdate > mstr.Qdate) THEN dups.SubGenSubscriptionLastQualifiedDate ELSE mstr.SubGenSubscriptionLastQualifiedDate END ,
				mstr.ConditionApplied=CASE when ISNULL(dups.ConditionApplied,0) <> 0 AND (ISNULL(mstr.ConditionApplied,0) = 0 OR dups.Qdate > mstr.Qdate) THEN dups.ConditionApplied ELSE mstr.ConditionApplied END
			from #MatchGroups mg
			join #SubscriberFinalRollup mstr on mstr.SFRecordIdentifier = mg.SFRecordIdentifier
			join #MatchGroups mg2 ON mg2.MatchID = mg.MatchID AND mg2.RecordType = 'Duplicate' and mg2.IsOriginal = 1
			join #SubscriberFinalRollup dups on dups.SFRecordIdentifier = mg2.SFRecordIdentifier
			where mg.MatchID = @MatchID and mg.RecordType = 'Master' AND
				  mg2.SFRecordIdentifier = @SFRecordIdentifier

			DECLARE c_DupDemos CURSOR LOCAL FORWARD_ONLY STATIC READ_ONLY FOR select MAFField from SubscriberDemographicFinal where SFRecordIdentifier = @SFRecordIdentifier
			OPEN c_DupDemos  

			FETCH NEXT from c_DupDemos INTO @MAFField
			WHILE @@FETCH_STATUS = 0  
			BEGIN  
				-- BEGIN ROLLUP SUBSCRIBER DEMOGRAPHICS
				-- Rollup duplicate demographics if they do not exist at master level.
				IF NOT EXISTS (select top 1 1 
									FROM #MatchGroups mg 
									join #SubscriberFinalRollup sfmstr on sfmstr.SFRecordIdentifier = mg.SFRecordIdentifier 
									join SubscriberDemographicFinal sdfmstr on sdfmstr.SFRecordIdentifier = sfmstr.SFRecordIdentifier 
									where mg.MatchID = @MatchID and mg.RecordType = 'Master' and sdfmstr.MAFField = @MAFField)
				BEGIN
					-- SET all duplicate SFRecordIdentifier to that of the Master record
	--Select 'Demographic Update', * 
					UPDATE sdfdup set sdfdup.SFRecordIdentifier = mg.SFRecordIdentifier
						FROM #MatchGroups mg2 
						join #SubscriberFinalRollup sfdup on sfdup.SFRecordIdentifier = mg2.SFRecordIdentifier 
						join SubscriberDemographicFinal sdfdup on sdfdup.SFRecordIdentifier = sfdup.SFRecordIdentifier 
						join #MatchGroups mg on mg.MatchID = mg2.MatchID and mg.RecordType = 'Master'
					where mg2.MatchID = @MatchID and mg2.RecordType = 'Duplicate' and sdfdup.MAFField = @MAFField
				END				

						-- END ROLLUP SUBSCRIBER DEMOGRAPHICS

				FETCH NEXT from c_DupDemos INTO @MAFField
			END
			CLOSE c_DupDemos  
			DEALLOCATE c_DupDemos

			FETCH NEXT from c_DupRecs INTO @SFRecordIdentifier
		END
		CLOSE c_DupRecs  
		DEALLOCATE c_DupRecs
	 
	FETCH NEXT FROM c_MatchIDs INTO @MatchID
	END

	CLOSE c_MatchIDs  
	DEALLOCATE c_MatchIDs

	PRINT ('END FULL ROLLUP / '  + CONVERT(VARCHAR(20), GETDATE(), 114))


	--
	--	Try to match against Subscriptions and PubSubscriptions tables
	--
	Update mg set mg.PubSubscriptionID = ps.PubSubscriptionID, mg.PubMatchFound = 1
	From #MatchGroups mg  
	join Subscriptions s  WITH (nolock) on s.IGRP_NO = mg.Igrp_no
	join PubSubscriptions ps WITH (nolock) on ps.SubscriptionID = s.SubscriptionID and ps.PubID = mg.PubID

	PRINT ('END MATCHING AGAINST PUBSUBSCRIPTIONS COUNT: ' + CONVERT(varchar,@@RowCount) + ' / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

	Update mg set mg.SubscriptionID = s.SubscriptionID, mg.SubscriberMatchFound = 1
	From #MatchGroups mg  
	join Subscriptions s  WITH (nolock) on s.Igrp_no = mg.Igrp_no
	where mg.RecordType = 'Master'

	PRINT ('END MATCHING AGAINST SUBSCRIPTIONS COUNT: ' + CONVERT(varchar,@@RowCount) + ' / '  + CONVERT(VARCHAR(20), GETDATE(), 114))

	-- Build Temp table for SubscriberDemographicFinal,  this will be used when creating records for SDF when IsDateDate is false
	create table #tmpSDF (SFRecordIdentifier uniqueIdentifier, PubSubscriptionID int, SubscriptionID int, Qdate datetime, pubID int, MAFField varchar(255), Value  varchar(max), responseother varchar(256), CodesheetID int, pubsubscriptiondetailID int, NotExists bit, DateCreated datetime)

	-- Build Temp table for SubscriberDemographicFinal,  this will be used when creating records for SDF when IsDateDate is true
	create table #tmpSDFdemodate (SFRecordIdentifier uniqueIdentifier, PubSubscriptionID int, SubscriptionID int, Qdate datetime, pubID int, MAFField varchar(255), Value  varchar(max), responseother varchar(256), CodesheetID int, pubsubscriptiondetailID int, NotExists bit, DateCreated datetime)

	CREATE INDEX idx_tmpPSD_pubID_MAFField ON #tmpSDF(pubID, MAFField)
	CREATE INDEX idx_tmpPSD_pubsubscriptiondetailID ON #tmpSDF(pubsubscriptiondetailID)
	CREATE INDEX idx_tmpPSD_MAFField ON #tmpSDF(MAFField)

	insert into #tmpSDF (SFRecordIdentifier, PubSubscriptionID , SubscriptionID , Qdate , pubID , MAFField , Value, responseother, NotExists, DateCreated)
		select  sdf.SFRecordIdentifier, mg.PubSubscriptionID, mg.SubscriptionID, mg.QDate, mg.pubID, sdf.MAFField, (case when LEN(sdf.Value)= 1 and ISNUMERIC(sdf.Value) = 1 then '0' + sdf.Value else sdf.Value end) , sdf.ResponseOther, sdf.NotExists, sdf.DateCreated
			from #MatchGroups mg with(NOLOCK) join 
				 SubscriberDemographicFinal sdf  with(NOLOCK) on mg.SFRecordIdentifier = sdf.SFRecordIdentifier		
		   where sdf.IsDemoDate = 'false'
	
	insert into #tmpSDFdemodate (SFRecordIdentifier, PubSubscriptionID , SubscriptionID , Qdate , pubID , MAFField , Value, responseother, NotExists, DateCreated)
		select  sdf.SFRecordIdentifier, mg.PubSubscriptionID, mg.SubscriptionID, mg.QDate, mg.pubID, sdf.MAFField, (case when LEN(sdf.Value)= 1 and ISNUMERIC(sdf.Value) = 1 then '0' + sdf.Value else sdf.Value end) , sdf.ResponseOther, sdf.NotExists, sdf.DateCreated
			from #MatchGroups mg with(NOLOCK) join 
				 SubscriberDemographicFinal sdf  with(NOLOCK) on mg.SFRecordIdentifier = sdf.SFRecordIdentifier		
		   where sdf.IsDemoDate = 'true'

	Insert Into #tmpSDF (SFRecordIdentifier, PubSubscriptionID, SubscriptionID, QDate, pubID, MAFField, Value, NotExists, DateCreated)
		select mg.SFRecordIdentifier, mg.PubSubscriptionID, mg.SubscriptionID, mg.Qdate, mg.PubID, 'PUBCODE', p.PubCode, 0, QDate
		from #MatchGroups mg join 
			 Pubs p  WITH (nolock) on mg.PubID = p.PubID 
		where not exists (select top 1 1 from #tmpSDF where MAFField = 'PUBCODE')


	declare @updatedemorules table (MafField varchar(100), ruletype varchar(100))
	insert into @updatedemorules 
    select distinct items, 'overwrite' from dbo.fn_Split(@OverwriteValues, ',')
        
    insert into @updatedemorules 
    select distinct items, 'replace' from dbo.fn_Split(@ReplaceValues, ',')
        
	BEGIN TRY
	print 'begin try'
		BEGIN TRANSACTION;
  
		set ANSI_WARNINGS  OFF
			
		--delete existing dimension only if the new record has the dimension.  (for example, if a record in UAD has business, function and demo10, and incoming record has Business & function
		-- do not delete demo10 and bring in the new values for Business & function
			
		if exists( select top 1 1 from @updatedemorules where ruletype='overwrite')
		Begin
			delete ps
--select *
				from  PubSubscriptionDetail ps 
				join #MatchGroups mg on mg.PubSubscriptionID = ps.PubSubscriptionID 
				join CodeSheet c  WITH (nolock) on c.CodeSheetID = ps.CodesheetID 
				join ResponseGroups rg  WITH (nolock) on rg.ResponseGroupID = c.ResponseGroupID 
				join @updatedemorules u on u.maffield = rg.ResponseGroupName and ruletype='overwrite'
			Where 
				mg.PubMatchFound = 1 
				AND rg.ResponseGroupName <> 'TOPICS'

			Print ('PubSubscriptionDetail Overwrite COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
		End
            
        if exists( select top 1 1 from @updatedemorules where ruletype='replace')
		Begin
			insert into #psdNewDimensions
				select distinct mg.PubSubscriptionID, maffield
					from #MatchGroups mg 
					join #tmpSDF sdf with (NOLOCK) on sdf.SFRecordIdentifier = mg.SFRecordIdentifier 
			where mg.PubMatchFound = 1 and mg.PubSubscriptionID > 0 and isnull(sdf.Value,'') <> '' and sdf.NotExists = 0

			delete ps
				from  PubSubscriptionDetail ps 
					join #MatchGroups mg on mg.PubSubscriptionID = ps.PubSubscriptionID 
					join CodeSheet c  WITH (nolock) on c.CodeSheetID = ps.CodesheetID 
					join ResponseGroups rg  WITH (nolock) on rg.ResponseGroupID = c.ResponseGroupID 
					join #psdNewDimensions pd  WITH (nolock) on pd.PubSubscriptionID = ps.PubSubscriptionID and pd.ResponseGroupName = rg.ResponseGroupName 
				join @updatedemorules u on u.maffield = rg.ResponseGroupName and ruletype='replace'
			Where 
				rg.ResponseGroupName <> 'TOPICS'
			Print ('PubSubscriptionDetail Replace COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
		End		
				    
		delete	sd
		from	SubscriptionDetails sd
				join CodeSheet_Mastercodesheet_Bridge cmb  WITH (nolock) on sd.MasterID = cmb.MasterID 
		where sd.SubscriptionID in (select distinct subscriptionID from #MatchGroups mg where mg.RecordType = 'Master' and mg.SubscriberMatchFound = 1)

		Print ('Delete SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		delete	smv
		from	SubscriberMasterValues smv
		where smv.SubscriptionID in (select distinct subscriptionID from #MatchGroups mg where mg.RecordType = 'Master' and mg.SubscriberMatchFound = 1)
				 
		Print ('Delete SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	

	
		--
		--  For Subscription table, use the data coming from the Rollup of the SubscriberFinal table (#SubscriberFinalRollup)
		Update S
		Set
			-- Sequenceid is not update in the Subscription record, only Pubsubscriptions
			--[SEQUENCE]  = convert(int,isnull(sf.[SEQUENCE], 0)),
			FNAME       = (CASE WHEN ISNULL(sf.Fname,'')<>'' AND ISNULL(sf.LName,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FNAME END),
			LNAME       = (CASE WHEN ISNULL(sf.Fname,'')<>'' AND ISNULL(sf.LName,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.LName END),
			TITLE       = (CASE WHEN ISNULL(sf.TITLE,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.TITLE END),
			COMPANY     = (CASE WHEN ISNULL(sf.COMPANY,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COMPANY END),
			ADDRESS     = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS END),
			MAILSTOP    = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.MAILSTOP END),
			ADDRESS3    = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS3 END),               
			CITY        = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.CITY END),
			STATE       = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.STATE END),
			ZIP         = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ZIP END),
			PLUS4       = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PLUS4 END),
			FORZIP      = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FORZIP END),
			COUNTY      = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COUNTY END),
			COUNTRY     = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN REPLACE(REPLACE(REPLACE(sf.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.COUNTRY END),
			CountryID   = (CASE WHEN mg.UpdateAddressinSubscriptions = 1 THEN sf.CountryID ELSE s.CountryID END),
			Latitude    = (CASE WHEN mg.ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Latitude END),
			Longitude   = (CASE WHEN mg.ResetGeoCodesinSubscriptions = 1 THEN NULL	ELSE s.Longitude END),
			IsLatLonValid = (CASE WHEN mg.ResetGeoCodesinSubscriptions = 1 THEN 0	ELSE s.IsLatLonValid END),
			LatLonMsg   = (CASE WHEN mg.ResetGeoCodesinSubscriptions = 1 THEN ''	ELSE s.LatLonMsg END),
			PHONE       = (CASE WHEN ISNULL(sf.Phone,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PHONE END),
			FAX         = (CASE WHEN ISNULL(sf.FAX,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FAX END),
			MOBILE      = (CASE WHEN ISNULL(sf.MOBILE,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.mobile END),
			Email       = (CASE WHEN ISNULL(sf.EMail,'')<>'' THEN sf.Email ELSE s.EMAIL END),
			CategoryID  = sf.CategoryID,
			TransactionID = sf.TransactionID,
			TransactionDate = sf.TransactionDate,
			QDate       =  sf.QDate,
			MailPermission          = (CASE WHEN @MailPermissionOverRide = 'false' OR SF.MailPermission is null THEN S.MailPermission ELSE SF.MailPermission END),
			FaxPermission           = (CASE WHEN @FaxPermissionOverRide = 'false' OR SF.FaxPermission is null THEN S.FaxPermission ELSE SF.FaxPermission END),
			PhonePermission         = (CASE WHEN @PhonePermissionOverRide = 'false' OR SF.PhonePermission is null THEN S.PhonePermission ELSE SF.PhonePermission END),
			OtherProductsPermission = (CASE WHEN @OtherProductsPermissionOverRide = 'false' OR SF.OtherProductsPermission is null THEN S.OtherProductsPermission ELSE SF.OtherProductsPermission END),
			ThirdPartyPermission    = (CASE WHEN @ThirdPartyPermissionOverRide = 'false' OR SF.ThirdPartyPermission is null THEN S.ThirdPartyPermission ELSE SF.ThirdPartyPermission END),
			EmailRenewPermission    = (CASE WHEN @EmailRenewPermissionOverRide = 'false' OR SF.EmailRenewPermission is null THEN S.EmailRenewPermission ELSE SF.EmailRenewPermission END),
			TextPermission          = (CASE WHEN @TextPermissionOverRide = 'false' OR SF.TextPermission is null THEN S.TextPermission ELSE SF.TextPermission END),
			Demo7       =  case when isnull(sf.demo7,'') = '' then 'A' else sf.demo7 end,
			QSourceID   = case when isnull(sf.QSourceID, -1) > 0 then sf.QSourceID else S.QSourceID end,
			PAR3C       =  sf.PAR3C,
			IGRP_CNT    = sf.IGRP_CNT,
			emailexists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.EMail,'')<>'' THEN sf.Email ELSE s.EMAIL END),''))) <> '' then 1 else 0 end), 
			Faxexists   = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.FAX,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FAX END),''))) <> '' then 1 else 0 end), 
			PhoneExists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.Phone,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PHONE END),''))) <> '' then 1 else 0 end),
			Gender = (CASE WHEN ISNULL(sf.Gender,'')<>'' THEN REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.Gender END),
			IsMailable = 
			(CASE 
				WHEN (mg.UpdateAddressinSubscriptions = 0 or (mg.UpdateAddressinSubscriptions = 1 and mg.ResetGeoCodesinSubscriptions = 0)) then s.IsMailable
				WHEN mg.ResetGeoCodesinSubscriptions = 1 and sf.COUNTRYID in(1 ,2) THEN 0
				WHEN sf.COUNTRYID >=3 and (LEN(sf.Address) = 0 or LEN(sf.state) = 0 or LEN(sf.Country) = 0)  THEN 0 ELSE 1
			END	),
			   
			SubSrc = sf.SubSrc,
			OrigsSrc = sf.OrigsSrc,
			Verified = (CASE WHEN ISNULL(sf.Verified,'')<>''THEN sf.Verified ELSE s.Verified END),
			ExternalKeyId = (CASE WHEN ISNULL(sf.ExternalKeyId,'')<>''THEN sf.ExternalKeyId ELSE s.ExternalKeyId END),			   
			-- emailid is not update in the Subscription record, only Pubsubscriptions
--			EmailID = (CASE WHEN ISNULL(sf.EmailID,'')<>''THEN sf.EmailID ELSE s.EmailID END),
			DateUpdated = GETDATE()
--select *
		From Subscriptions s 
			join #MatchGroups mg on mg.SubscriptionID = s.SubscriptionID 
			join #SubscriberFinalRollup sf With(NoLock) on sf.SubscriberFinalID = mg.SubscriberFinalID
		where mg.SubscriptionMasterRecord = 1 and mg.SubscriberMatchFound = 1

		Print ('Subscription Update COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		--
		--  For Subscription table, use the data coming from the Rollup of the SubscriberFinal table (#SubscriberFinalRollup)
		--
		Insert into [Subscriptions] (
			[SEQUENCE], 
			FNAME, 
			LNAME, 
			TITLE, 
			COMPANY, 
			ADDRESS, 
			MAILSTOP, 
			CITY, 
			STATE, 
			ZIP, 
			PLUS4,
			FORZIP,
			COUNTY,
			COUNTRY,
			CountryID,
			PHONE,
			MOBILE,
			FAX,
			EMAIL,
			CategoryID, 
			TransactionID, 
			TransactionDate,
			QDate, 
			QSourceID,
			PAR3C,
			MailPermission,
			FaxPermission,
			PhonePermission,
			OtherProductsPermission,
			ThirdPartyPermission,
			EmailRenewPermission,
			TextPermission,
			IGRP_NO, 
			IGRP_CNT,
			emailexists, 
			Faxexists, 
			PhoneExists,
			Latitude, 
			Longitude, 
			IsLatLonValid, 
			LatLonMsg,
			ADDRESS3,
			Gender,
			IsMailable,
			SubSrc,
			OrigsSrc,
			[demo7],
			Verified,
			ExternalKeyId,
			EmailID
		)
		select  sf.[SEQUENCE],  
				REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				sf.CountryID,
				REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '), 
				sf.CategoryID, 
				sf.TransactionID, 
				sf.TransactionDate, 
				sf.QDate, 
				case when isnull(sf.QSourceID, -1) > 0 then sf.QSourceID else null end as QSourceID, 
				sf.PAR3C,
				sf.MailPermission, 
				sf.FaxPermission, 
				sf.PhonePermission, 
				sf.OtherProductsPermission, 
				sf.ThirdPartyPermission, 
				sf.EmailRenewPermission, 
				sf.TextPermission,
				sf.Igrp_no, 
				sf.IGRP_CNT,
				(case when ltrim(rtrim(isnull(sf.email,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(sf.Fax,''))) <> '' then 1 else 0 end),
				(case when ltrim(rtrim(isnull(sf.PHONE,''))) <> '' then 1 else 0 end),
				sf.Latitude,
				sf.Longitude,
				sf.IsLatLonValid,
				sf.LatLonMsg,
				REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
				(CASE 
					WHEN sf.COUNTRYID in (1,2)  THEN 0
					WHEN sf.COUNTRYID >=3 and (ISNULL(sf.Address,'')<>'' AND ISNULL(sf.City,'')<>'' AND ISNULL(sf.State,'')<>'' AND (ISNULL(sf.ZIP,'')<>'' or ISNULL(sf.ForZip,'')<>'')) THEN 1 ELSE 0 
				END),
				sf.SubSrc,
				sf.OrigsSrc,
				case when isnull(sf.demo7,'') = '' then 'A' else sf.demo7 end,
				sf.Verified,
				sf.ExternalKeyId,
				EmailID 
--select *
		from #MatchGroups mg 
		join  #SubscriberFinalRollup sf with (NOLOCK) on sf.SubscriberFinalID = mg.SubscriberFinalID
		where mg.SubscriptionID is null and mg.SubscriptionMasterRecord = 1 and mg.SubscriberMatchFound = 0
				
		Print ('Insert [Subscriptions] COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
		Update mg 
			set mg.SubscriptionID = s.subscriptionID, mg.SubscriberMatchFound = 1
		FROM #MatchGroups mg 
		join Subscriptions s WITH (nolock) on mg.IGRP_NO = s.IGRP_NO
		WHERE mg.SubscriptionID is null and mg.SubscriberMatchFound = 0

		Print ('Update #MatchGroups SubscriptioniID COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))		
	    
		DECLARE @IAFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAFree')
		DECLARE @IAPAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAPaid')
		DECLARE @AFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AFree')
		DECLARE @APAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'APaid')
		DECLARE @APros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AProsp')
		DECLARE @IAPros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAProsp')


		--
		--  For Pubsubscription table, use the data coming from the real SubscriberFinal table and not rollup data
		UPDATE ps
		SET ps.SubscriptionID = mstr.SubscriptionID
			,[demo7] = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN ( case when isnull(sf.demo7,'') = '' then 'A' else sf.demo7 end ) ELSE ps.Demo7 END
			,[Qualificationdate] = sf.QDate
			,[PubCategoryID] = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.CategoryID ELSE ps.PubCategoryID END  
			,[PubTransactionID] = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.TransactionID ELSE ps.PubTransactionID END  
			,[EmailStatusID] = 
					case	when isnull(sf.Email, '') = '' then null 
							when isnull(sf.Email, '') = ps.Email then (Case when isnull(ps.EmailStatusID,0) in (2,4,5,6) then ps.EmailStatusID else isnull(sf.EmailStatusID,ps.EmailStatusID) end )
							else (
								case when ISNULL(sf.EmailStatusID,-1) <= 0 then 1 else sf.EmailStatusID end 
								 )
					end
			,Email = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN (CASE WHEN ISNULL(sf.EMail,'') <> '' THEN sf.Email ELSE ps.EMAIL END) ELSE ps.Email END,
			--,StatusUpdatedDate = sf.StatusUpdatedDate
			--,StatusUpdatedReason = sf.StatusUpdatedReason,
			--SequenceID  = convert(int,isnull(sf.[SEQUENCE], 0)), --commented on 9/24/2015 - Per Meghan - sequenceID should be updated for existing records.
			FirstName   = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.FNAME ELSE ps.FirstName END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			LastName    = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.LNAME ELSE ps.LastName END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Title       = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.TITLE ELSE ps.Title END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Company     = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.COMPANY ELSE ps.Company END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Address1    = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.ADDRESS ELSE ps.Address1 END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Address2    = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.MAILSTOP ELSE ps.Address2 END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			City        = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.CITY ELSE ps.City END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			RegionCode  = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.STATE ELSE ps.RegionCode END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			RegionID    =  (CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN 
							(select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) ELSE
							ps.RegionID END), --added 9/27/2015
			ZipCode     = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.ZIP ELSE ps.ZipCode END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Plus4       = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.PLUS4 ELSE ps.Plus4 END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			County      = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.COUNTY ELSE ps.County END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Country     = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.COUNTRY ELSE ps.Country END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			CountryID   = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.CountryID ELSE ps.CountryID END,
			Phone       = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.PHONE ELSE ps.Phone END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Fax         = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.FAX ELSE ps.Fax END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Mobile      = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.MOBILE ELSE ps.Mobile END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			PubQSourceID   = case when isnull(sf.QSourceID, -1) > 0 then sf.QSourceID else ps.PubQSourceID end,
			Latitude    = sf.Latitude,
			Longitude   = sf.Longitude,
			IsAddressValidated = sf.IsLatLonValid,
			AddressValidationMessage = sf.LatLonMsg,
			Address3 = REPLACE(REPLACE(REPLACE(CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN sf.ADDRESS3 ELSE ps.Address3 END, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Gender = REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			IGrp_No = sf.IGrp_No,
			SFRecordIdentifier = sf.SFRecordIdentifier,
			--OrigsSrc = sf.OrigsSrc,
			--SubscriptionStatusID = sf.SubscriptionstatusID,
			--IsSubscribed = sf.isSubscribed,
			--Copies = sf.Copies,
			Verify = case when isnull(sf.Verified, '') = '' then Verify else sf.Verified end,
			Par3cID = sf.Par3c,
			SubscriberSourceCode = sf.subsrc,
			isActive = sf.IsActive,
			SubscriptionStatusID = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN 
			(CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
			WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
			WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
			WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
			WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID in (1) AND cc.CategoryCodeValue in (70,71) THEN @APros  
			WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros ELSE NULL END) ELSE ps.SubscriptionStatusID END,
			IsSubscribed = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN (CASE WHEN tc.TransactionCodeTypeID in (1,3) THEN 1 ELSE 0 END) ELSE ps.IsSubscribed END,
			IsPaid = CASE WHEN IsNull(ps.IsInActiveWaveMailing,0) = 0 THEN (CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 1 ELSE 0 END) ELSE ps.IsPaid END,
			DateUpdated = GETDATE(),
			PubTransactionDate = sf.TransactionDate,
			ExternalKeyId = case when isnull(sf.ExternalKeyId, 0) = 0 then ps.ExternalKeyId else sf.ExternalKeyId end,
			AccountNumber = case when isnull(sf.AccountNumber, '') = '' then ps.AccountNumber else sf.AccountNumber end,
			EmailID = case when isnull(sf.EmailID, 0) = 0 then ps.EmailID else sf.EmailID end,
			Copies = CASE WHEN ISNULL(sf.Copies,0) < 1 THEN 1 ELSE sf.Copies END,
			GraceIssues = sf.GraceIssues,
			IsComp =  ISNULL(sf.IsComp,0),			   
			Occupation = sf.Occupation,
			SubSrcID = sf.SubsrcID,
			Website = sf.Website,
			MailPermission      = (CASE WHEN @MailPermissionOverRide = 'false' OR SF.MailPermission is null THEN ps.MailPermission ELSE SF.MailPermission END),
			FaxPermission      = (CASE WHEN @FaxPermissionOverRide = 'false' OR SF.FaxPermission is null THEN ps.FaxPermission ELSE SF.FaxPermission END),
			PhonePermission      = (CASE WHEN @PhonePermissionOverRide = 'false' OR SF.PhonePermission is null THEN ps.PhonePermission ELSE SF.PhonePermission END),
			OtherProductsPermission      = (CASE WHEN @OtherProductsPermissionOverRide = 'false' OR SF.OtherProductsPermission is null THEN ps.OtherProductsPermission ELSE SF.OtherProductsPermission END),
			ThirdPartyPermission      = (CASE WHEN @ThirdPartyPermissionOverRide = 'false' OR SF.ThirdPartyPermission is null THEN ps.ThirdPartyPermission ELSE SF.ThirdPartyPermission END),
			EmailRenewPermission      = (CASE WHEN @EmailRenewPermissionOverRide = 'false' OR SF.EmailRenewPermission is null THEN ps.EmailRenewPermission ELSE SF.EmailRenewPermission END),
			TextPermission      = (CASE WHEN @TextPermissionOverRide = 'false' OR SF.TextPermission is null THEN ps.TextPermission ELSE SF.TextPermission END),
			SubGenSubscriberID = case when isnull(sf.SubGenSubscriberID, 0) = 0 then ps.SubGenSubscriberID else sf.SubGenSubscriberID end,
			SubGenSubscriptionID = case when isnull(sf.SubGenSubscriptionID, 0) = 0 then ps.SubGenSubscriptionID else sf.SubGenSubscriptionID end,
			SubGenPublicationID = case when isnull(sf.SubGenPublicationID, 0) = 0 then ps.SubGenPublicationID else sf.SubGenPublicationID end,
			SubGenMailingAddressId = case when isnull(sf.SubGenMailingAddressId, 0) = 0 then ps.SubGenMailingAddressId else sf.SubGenMailingAddressId end,
			SubGenBillingAddressId = case when isnull(sf.SubGenBillingAddressId, 0) = 0 then ps.SubGenBillingAddressId else sf.SubGenBillingAddressId end,			   
			IssuesLeft = case when isnull(sf.IssuesLeft, 0) = 0 then ps.IssuesLeft else sf.IssuesLeft end,
			UnearnedReveue = case when isnull(sf.UnearnedReveue, 0.00) = 0.00 then ps.UnearnedReveue else sf.UnearnedReveue end,
			SubGenIsLead = case when sf.SubGenIsLead is null then ps.SubGenIsLead else sf.SubGenIsLead end,
			SubGenRenewalCode = case when isnull(sf.SubGenRenewalCode, '') = '' then ps.SubGenRenewalCode else sf.SubGenRenewalCode end,
			SubGenSubscriptionRenewDate = case when sf.SubGenSubscriptionRenewDate is null then ps.SubGenSubscriptionRenewDate else sf.SubGenSubscriptionRenewDate end,
			SubGenSubscriptionExpireDate = case when sf.SubGenSubscriptionExpireDate is null then ps.SubGenSubscriptionExpireDate else sf.SubGenSubscriptionExpireDate end,
			SubGenSubscriptionLastQualifiedDate = case when sf.SubGenSubscriptionLastQualifiedDate is null then ps.SubGenSubscriptionLastQualifiedDate else sf.SubGenSubscriptionLastQualifiedDate end
		from #MatchGroups mg
			join #MatchGroups mstr on mstr.MatchID = mg.MatchID and mstr.RecordType = 'Master'
			join Subscriberfinal sf With(NoLock) on sf.SubscriberFinalID = mg.SubscriberFinalID
			join  PubSubscriptions ps  WITH (nolock) on ps.PubSubscriptionID = mg.PubSubscriptionID 
			left outer join UAD_LookUp..CategoryCode cc WITH (nolock) ON cc.CategoryCodeID = (CASE WHEN ISNULL(sf.CategoryID,0) <> ISNULL(ps.PubCategoryID,0) THEN sf.CategoryID ELSE ps.PubCategoryID END)
			left outer join UAD_LookUp..TransactionCode tc WITH (nolock) ON tc.TransactionCodeID = (CASE WHEN ISNULL(sf.TransactionID,0) <> ISNULL(ps.PubTransactionID,0) THEN sf.TransactionID ELSE ps.PubTransactionID END)
		where mg.PubMatchFound = 1
		Print ('Update pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

    
		declare @pubSequenceID table (PubCode varchar(50), CurrentMAXsequenceID int)
	    
		insert into @pubSequenceID
			select p.PubCode, ISNULL(max(sequenceID),0) 
				from Pubs p with (NOLOCK) 
				left outer join PubSubscriptions ps with (NOLOCK) on ps.PubID = p.PubID
			where p.Pubcode in (select distinct sf.pubcode from #SubscriberFinalRollup sf)
			group by p.pubcode
	    
		Print ('insert into @pubSequenceID : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

    
		--
		--  For Pubsubscription table, use the data coming from the real SubscriberFinal table and not rollup data
		INSERT INTO pubsubscriptions 
		(
			SubscriptionID,
			PubID,
			demo7,
			Qualificationdate,
			PubQSourceID, 
			PubCategoryID,
			PubTransactionID,
			Email,
			EmailStatusID,
			FirstName,
			LastName,
			Title,
			Company,
			Address1,
			Address2,
			City,
			RegionCode,
			RegionID, 
			ZipCode,
			Plus4,
			County,
			Country,
			CountryID,
			Phone,
			Fax,
			Mobile,
			Par3CID, 
			Latitude,
			Longitude,
			IsAddressValidated,
			AddressValidationMessage,
			Address3,
			Gender,
			IGrp_No,
			SFRecordIdentifier, 
			SubscriberSourceCode, 
			OrigsSrc, 
			SequenceID, 
			isActive, 
			PubTransactionDate,
			ExternalKeyId,
			AccountNumber,
			EmailID,
			SubscriptionStatusID,
			IsSubscribed,
			IsPaid,
			Copies,
			GraceIssues,
			IsComp,
			Occupation,
			SubSrcID,
			Website,
			MailPermission,
			FaxPermission,
			PhonePermission,
			OtherProductsPermission,
			ThirdPartyPermission,
			EmailRenewPermission,
			TextPermission,
			Verify,
			SubGenSubscriberID,
			SubGenSubscriptionID,
			SubGenPublicationID,
			SubGenMailingAddressId,
			SubGenBillingAddressId,
			IssuesLeft,
			UnearnedReveue,
			SubGenIsLead,
			SubGenRenewalCode,
			SubGenSubscriptionRenewDate,
			SubGenSubscriptionExpireDate,
			SubGenSubscriptionLastQualifiedDate
		)
		select mstr.SubscriptionID , 
			mg.PubID, 
			case when isnull(sf.demo7,'') = '' then 'A' else sf.demo7 end, 
			sf.QDate, 
			sf.QSourceID, 
			sf.CategoryID, 
			sf.TransactionID, 
			sf.Email,
			case when isnull(sf.Email, '') = '' then null else (case when ISNULL(sf.EmailStatusID,-1) <= 0 then 1 else sf.EmailStatusID end ) end,
			FirstName   =  REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			LastName    =  REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Title       =  REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Company     =  REPLACE(REPLACE(REPLACE(sf.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Address1    =  REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Address2    =  REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			City        =  REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			RegionCode  =  REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			RegionID    =  (select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')), --added 9/27/2015
			ZipCode     =  REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Plus4       =  REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			County      =  REPLACE(REPLACE(REPLACE(sf.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Country     =  REPLACE(REPLACE(REPLACE(sf.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			CountryID   =  sf.CountryID,
			Phone       =  REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Fax         =  REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Mobile      =  REPLACE(REPLACE(REPLACE(sf.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Par3CID       =  sf.PAR3C,  --added 9/27/2015
			Latitude    = sf.Latitude,
			Longitude   = sf.Longitude,
			IsAddressValidated = sf.IsLatLonValid,
			AddressValidationMessage = sf.LatLonMsg,
			Address3 = REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			Gender   = REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' '),
			sf.IGrp_No,
			sf.SFRecordIdentifier,
			sf.SubSrc,
			sf.SubSrc,
			(CASE WHEN @FileType = 'DBF' THEN sf.Sequence ELSE psID.CurrentMAXsequenceID + (row_number() over (partition by sf.pubcode order by sf.pubcode)) END),
			sf.IsActive,
			sf.TransactionDate,
			sf.ExternalKeyId,sf.AccountNumber,sf.EmailID,
			CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
				WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
				WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
				WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
				WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID in (1) AND cc.CategoryCodeValue in (70,71) THEN @APros  
				WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros 
				ELSE null
			END,
			CASE WHEN tc.TransactionCodeTypeID in (1,3) THEN 1 ELSE 0 END,
				CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 1 ELSE 0 END,
				CASE WHEN ISNULL(sf.Copies,0) < 1 THEN 1 ELSE sf.Copies 
			END,
			sf.GraceIssues,
			sf.IsComp,
			sf.Occupation,			    
			sf.SubsrcID,
			sf.Website,
			sf.MailPermission,
			sf.FaxPermission,
			sf.PhonePermission,
			sf.OtherProductsPermission,
			sf.ThirdPartyPermission,
			sf.EmailRenewPermission,
			sf.TextPermission,
			sf.Verified,
			sf.SubGenSubscriberID,
			sf.SubGenSubscriptionID,
			sf.SubGenPublicationID,
			sf.SubGenMailingAddressId,
			sf.SubGenBillingAddressId,
			sf.IssuesLeft,
			sf.UnearnedReveue,
			sf.SubGenIsLead,
			sf.SubGenRenewalCode,
			sf.SubGenSubscriptionRenewDate,
			sf.SubGenSubscriptionExpireDate,
			sf.SubGenSubscriptionLastQualifiedDate
--select *
		from #MatchGroups mg
			join #MatchGroups mstr on mstr.MatchID = mg.MatchID and mstr.RecordType = 'Master'
			join Subscriberfinal sf With(NoLock) on sf.SubscriberFinalID = mg.SubscriberFinalID
			join @pubSequenceID psID on sf.pubcode = psID.PubCode
			left outer join UAD_LookUp..CategoryCode cc  WITH (nolock) ON cc.CategoryCodeID = sf.CategoryID
			left outer join UAD_LookUp..TransactionCode tc  WITH (nolock) ON tc.TransactionCodeID = sf.TransactionID
		Where mg.PubMatchFound = 0
		
		Print ('Insert pubsubscriptions COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
		    
		update mg
			set mg.PubSubscriptionID = ps.PubSubscriptionID
		from #MatchGroups mg
			join PubSubscriptions ps  WITH (nolock) on mg.PubID = ps.PubID and mg.SubscriptionID = ps.SubscriptionID
		where mg.PubSubscriptionID is null

		Print ('Update MatchGroup PubSubscriptionID COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
		
		Print ('Merge Start in PubsubscriptionDetail : ' + convert(varchar(20), getdate(), 114))

--SUNIL code below to replace this which was to replace the merge

		create table #tmpcodesheet (pubID int, MAFField varchar(255), codesheetID int, Value varchar(max), IsOther bit)
		create table #tmpcodesheetdemodate (pubID int, MAFField varchar(255), codesheetID int, Value varchar(max), IsOther bit)

		print ' insert into #tmpSMV1' + convert(varchar(20), getdate(), 109)		

		insert into #tmpcodesheet
			select t.pubID, t.maffield, codesheetID, (case when LEN(responsevalue )= 1 and ISNUMERIC(responsevalue ) = 1 then '0' + responsevalue  else responsevalue  end), IsOther
				from 
					(select distinct pubID, maffield from #tmpSDF with (NOLOCK)) t join
					ResponseGroups rg with (NOLOCK) on t.pubID =  rg.PubID and t.MAFField = rg.ResponseGroupName join
					CodeSheet c with (NOLOCK) on c.ResponseGroupID = rg.ResponseGroupID 

		insert into #tmpcodesheetdemodate
			select t.pubID, t.maffield, codesheetID, (case when LEN(responsevalue )= 1 and ISNUMERIC(responsevalue ) = 1 then '0' + responsevalue  else responsevalue  end), IsOther
				from 
					(select distinct pubID, maffield from #tmpSDFdemodate with (NOLOCK)) t join
					ResponseGroups rg with (NOLOCK) on t.pubID =  rg.PubID and t.MAFField = rg.ResponseGroupName join
					CodeSheet c with (NOLOCK) on c.ResponseGroupID = rg.ResponseGroupID 

		Update sdf set sdf.CodesheetID = t.codesheetID, sdf.pubsubscriptiondetailID = psd.pubsubscriptiondetailID
			from #tmpSDF sdf  with(NOLOCK) join 
				 #tmpcodesheet t with(NOLOCK)  on t.pubID = sdf.pubID and t.MAFField = sdf.MAFField and t.Value = sdf.value left outer join 
				 PubSubscriptionDetail psd on psd.PubSubscriptionID = sdf.PubSubscriptionID and psd.CodesheetID = t.codesheetID

		-- Update PubSubscriptionDetail table
		Update psd Set psd.DateCreated = t.DateCreated
			from PubSubscriptionDetail psd
			join #tmpSDF t on t.pubsubscriptiondetailID = psd.PubSubscriptionDetailID
		Print ('Update PubSubScriptionDetail : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		Update psd Set psd.DateCreated = sdf.DateCreated
			from #tmpSDFdemodate sdf  with(NOLOCK) join 
				 #tmpcodesheetdemodate t with(NOLOCK)  on t.pubID = sdf.pubID and t.MAFField = sdf.MAFField left outer join 
				 PubSubscriptionDetail psd on psd.PubSubscriptionID = sdf.PubSubscriptionID and psd.CodesheetID = t.codesheetID
		Print ('Update PubSubScriptionDetail : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

		insert into PubSubscriptionDetail  (PubSubscriptionID, SubscriptionID, CodeSheetID, DateCreated, ResponseOther)
			Select ps.PubSubscriptionID, ps.SubscriptionID, t.CodeSheetID, t.DateCreated, t.ResponseOther
				from #tmpSDF t
				join PubSubscriptions ps on ps.SFRecordIdentifier = t.SFRecordIdentifier
				where t.PubSubscriptionDetailID is NULL and CodesheetID is not null
		Print ('Insert PubSubScriptionDetail : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))


		drop table #tmpSDF
		drop table #tmpSDFdemodate
		drop table #tmpcodesheet
		drop table #tmpcodesheetdemodate


		-- Replaced by above UPDATE and INSERT  
		--MERGE PubSubscriptionDetail AS target  
		--USING (
		--	select   mg.PubSubscriptionID, mg.SubscriptionID, cs.CodeSheetID, s.QDate, CASE WHEN cs.IsOther = 1 THEN sdf.ResponseOther ELSE '' END as ResponseOther
		--	from #MatchGroups mg
		--		join SubscriberFinal s with(NOLOCK) on mg.SubscriberFinalID = s.SubscriberFinalID
		--		join SubscriberDemographicFinal sdf  with(NOLOCK) on s.SFRecordIdentifier = sdf.SFRecordIdentifier
		--		join ResponseGroups rg with(NOLOCK)  on sdf.MAFField = rg.ResponseGroupName and rg.pubid = mg.PubID
		--		join CodeSheet cs with(NOLOCK)  on rg.ResponseGroupID = cs.ResponseGroupID and rg.PubID = cs.PubID and 
		--			(case when LEN(sdf.Value)= 1 and ISNUMERIC(sdf.Value) = 1 then '0' + sdf.Value else sdf.Value end) 
		--			= 
		--			(case when LEN(cs.responsevalue)= 1 and ISNUMERIC(cs.responsevalue) = 1 then '0' + cs.Responsevalue else cs.responsevalue end)
		--	union
		--	select   mg.PubSubscriptionID, mg.SubscriptionID, c.CodeSheetID, getdate(), null
		--	from #MatchGroups mg 
		--		join Pubs p  WITH (nolock) on mg.PubID = p.PubID 
		--		join ResponseGroups rg  WITH (nolock) on rg.PubID = p.PubID and ResponseGroupName = 'PUBCODE'
		--		join CodeSheet c  WITH (nolock) on c.ResponseGroupID = rg.ResponseGroupID and c.Responsevalue = p.PubCode
		--) AS source (PubSubscriptionID, SubscriptionID,CodeSheetID,QDate,ResponseOther )  
		--ON (target.PubSubscriptionID = source.PubSubscriptionID and target.CodesheetID = source.CodesheetID)  
		--	WHEN MATCHED THEN   
		--	UPDATE Set DateCreated = source.QDate
		--	WHEN NOT MATCHED THEN			
		--	insert  (PubSubscriptionID, SubscriptionID, CodeSheetID, DateCreated, ResponseOther)
		--	VALUES (source.PubSubscriptionID, source.SubscriptionID, source.CodeSheetID, source.QDate, source.ResponseOther) ;		
			
	    --Print ('Merge End in PubSubScriptionDetail : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
	    
		--Print ('Insert PubSubscriptionDetail COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		insert into SubscriptionDetails (SubscriptionID, MasterID)
		select distinct psd.SubscriptionID, cmb.masterID 
		from PubSubscriptionDetail psd 
			join CodeSheet_Mastercodesheet_Bridge cmb with (NOLOCK) on psd.CodesheetID = cmb.CodeSheetID 
			left outer join SubscriptionDetails sd   WITH (nolock) on sd.SubscriptionID = psd.SubscriptionID and sd.MasterID = cmb.MasterID
		where psd.SubscriptionID in (select distinct subscriptionID from #MatchGroups mg where mg.RecordType = 'Master' and mg.SubscriberMatchFound = 1) 
			and sd.sdID is null	
		
		Print ('Insert SubscriptionDetails COUNT : ' + convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))	
	    
		/***** Final Step *****/
	    
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
				FROM SubscriptionDetails sd  with (NOLOCK)
				join Mastercodesheet mc  with (NOLOCK)  on sd.MasterID = mc.MasterID
				where sd.SubscriptionID in (select distinct subscriptionID from #MatchGroups mg where mg.RecordType = 'Master' and mg.SubscriberMatchFound = 1) 
			)
			Results
		GROUP BY [SubscriptionID] , MasterGroupID
		order by SubscriptionID    
	    
		Print ('Insert into SubscriberMasterValues COUNT : ' + convert(varchar(100),@@ROWCOUNT)+ ' / ' + convert(varchar(20), getdate(), 114))
	    

		/*
		********** Insert/Update PubSubscriptionsExtension
		*/
		declare @PubCodeID int,
			@PubStandardField varchar(255),
			@PubCustomField varchar(255),
			@PubRuleType varchar(100),
			@PubAdhocisAlsoDimension bit = 0

		Create table #tblPubAdhoc (PubSubscriptionID int, AdhocValue varchar(max))

		CREATE INDEX IDX_tblPubAdhoc_SubscriptionID ON #tblPubAdhoc(PubSubscriptionID)

		CREATE TABLE #tempPubAdhocFields (PubID int, CustomField varchar(255), StandardField varchar(255))

		Insert into #tempPubAdhocFields (PubID,CustomField,StandardField)
		Select distinct sdt.PubID, pem.CustomField, pem.StandardField		
		from PubSubscriptionsExtensionMapper pem WITH(NOLOCK)
		join SubscriberDemographicTransformed sdt WITH(NOLOCK) on pem.CustomField = sdt.MAFField and pem.PubID = sdt.PubID 
		join SubscriberTransformed st WITH(NOLOCK) on sdt.STRecordIdentifier = st.STRecordIdentifier
		where st.ProcessCode = @ProcessCode and pem.Active = 1
		
		DECLARE curPubAdhoc CURSOR LOCAL FAST_FORWARD FOR SELECT PubID, CustomField, StandardField, ISNULL(u.ruletype,'') as RuleType from #tempPubAdhocFields t left join @updatedemorules u on t.CustomField = u.MafField
		OPEN curPubAdhoc
		FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField, @PubRuleType
		WHILE @@FETCH_STATUS = 0  
		BEGIN 
		
			if exists (select 1 from ResponseGroups WITH(NOLOCK) where ResponseGroupName = @PubCustomField and PubID = @PubCodeID)
				set @PubAdhocisAlsoDimension = 1
		
			;WITH PubAdhoc_CTE (PubSubscriptionID,  Value)
			AS
			(
				Select distinct mg.PubSubscriptionID,  Value
				from 
						#MatchGroups mg 
						join #SubscriberFinalRollup sf with(NOLOCK) on mg.SubscriberFinalID = sf.SubscriberFinalID
						join SubscriberDemographicFinal sdf  with(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
				where mg.PubSubscriptionID is Not null and mg.PubID = @PubCodeID and MAFField = @PubCustomField and mg.RecordType = 'Master' and mg.SubscriberMatchFound = 1
				union
				Select distinct mg.PubSubscriptionID,  Value
				from 
						#MatchGroups mg 
						join #SubscriberFinalRollup sf with(NOLOCK) on mg.SubscriberFinalID = sf.SubscriberFinalID
						join SubscriberDemographicTransformed sdt  with(NOLOCK) on sf.STRecordIdentifier = sdt.STRecordIdentifier
				where @PubAdhocisAlsoDimension = 1 and mg.PubSubscriptionID is Not null and mg.PubID = @PubCodeID and MAFField = @PubCustomField and sdt.NotExists = 1
			)
			insert into #tblPubAdhoc (PubSubscriptionID, AdhocValue)		
			
			SELECT   DISTINCT PubSubscriptionID, STUFF((SELECT ',' + inn.Value FROM PubAdhoc_CTE AS inn WHERE inn.PubSubscriptionID = PubAdhoc_CTE.PubSubscriptionID FOR XML PATH('')), 1, 1, '') AS AdhocValue
			FROM      PubAdhoc_CTE
			ORDER BY   PubSubscriptionID		
		
			print ' PubSubscriptionsExtensionMapper : ' + @PubStandardField + ' / ' + @PubCustomField + ' / counts : ' + convert(varchar(100), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114)
			
			if exists (select top 1 1 from #tblPubAdhoc)
			Begin
	
					if (@PubRuleType = 'overwrite')
					BEGIN
						EXEC ('Update s1 Set ' + @PubStandardField + ' = left(AdhocValue,2048) 
						FROM PubSubscriptionsExtension s1 
						join #tblPubAdhoc t on s1.PubSubscriptionID = t.PubSubscriptionID
						')
					END

					if (@PubRuleType = 'replace')
					BEGIN
						EXEC ('Update s1 Set ' + @PubStandardField + ' = left(AdhocValue,2048) 
						FROM PubSubscriptionsExtension s1 
						join #tblPubAdhoc t on s1.PubSubscriptionID = t.PubSubscriptionID
						where len(t.AdhocValue) > 0
						')						
					END

					if (@PubRuleType = '')
					BEGIN
						EXEC ('Update s1 Set ' + @PubStandardField + ' = CASE 
								WHEN t.AdhocValue = ' + @PubStandardField + ' THEN ' + @PubStandardField + '
								WHEN ' + @PubStandardField + ' is null THEN t.AdhocValue
								ELSE CAST(LTRIM(RTRIM(ISNULL(' + @PubStandardField + ',''''))) + '','' + LTRIM(RTRIM(t.AdhocValue)) AS VARCHAR(2048)) END
						FROM PubSubscriptionsExtension s1 
						join #tblPubAdhoc t on s1.PubSubscriptionID = t.PubSubscriptionID
						where len(t.AdhocValue) > 0')
					END

					DECLARE @InsertPubAdhocStatement varchar(1000) = 'INSERT INTO PubSubscriptionsExtension (PubSubscriptionID, ' + @PubStandardField + ') 
						SELECT t.PubSubscriptionID, left(AdhocValue,2048)
						FROM #tblPubAdhoc t  left outer join PubSubscriptionsExtension se on se.PubSubscriptionID = t.PubSubscriptionID
						WHERE se.PubSubscriptionID is null '
					if (@PubRuleType = 'replace' OR @PubRuleType = '')
					BEGIN
						set @InsertPubAdhocStatement = @InsertPubAdhocStatement + ' and len(t.AdhocValue) > 0'
					END

					Exec (@InsertPubAdhocStatement)					
		  
					Truncate table #tblPubAdhoc
			End
			
			FETCH NEXT FROM curPubAdhoc INTO @PubCodeID, @PubCustomField, @PubStandardField, @PubRuleType
		END 
		CLOSE curPubAdhoc
		DEALLOCATE curPubAdhoc		
		
		drop table #tblPubAdhoc
		drop table #tempPubAdhocFields


		/*
		********** Insert/Update SubscriptionsExtension
		*/
		
		declare @StandardField varchar(255),
			@CustomField varchar(255),
			@RuleType varchar(100), 
			@AdhocisAlsoDimension bit = 0

		Create table #tblAdhoc (SubscriptionID int, AdhocValue varchar(max) )

		CREATE INDEX IDX_tblAdhoc_SubscriptionID ON #tblAdhoc(SubscriptionID)
		
		DECLARE curAdhoc CURSOR LOCAL FAST_FORWARD FOR SELECT CustomField, StandardField, ISNULL(u.ruletype,'') FROM SubscriptionsExtensionMapper s left join @updatedemorules u on s.CustomField = u.MafField where s.Active = 1
		OPEN curAdhoc
		FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField, @RuleType
		WHILE @@FETCH_STATUS = 0  
		BEGIN 
		
			if exists (select 1 from ResponseGroups where ResponseGroupName = @CustomField)
				set @AdhocisAlsoDimension = 1
		
			;WITH Adhoc_CTE (SubscriptionID,  Value)
			AS
			(
				Select distinct mg.SubscriptionID,  Value
				from 
						#MatchGroups mg 
						join #SubscriberFinalRollup sf with(NOLOCK) on mg.SubscriberFinalID = sf.SubscriberFinalID
						join SubscriberDemographicFinal sdf  with(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
				where mg.SubscriptionID is Not null and MAFField = @CustomField
				union
				Select distinct mg.SubscriptionID,  Value
				from 
						#MatchGroups mg	
						join #SubscriberFinalRollup sf with(NOLOCK) on mg.SubscriberFinalID = sf.SubscriberFinalID
						join SubscriberDemographicTransformed sdt  with(NOLOCK) on sf.STRecordIdentifier = sdt.STRecordIdentifier
				where @AdhocisAlsoDimension = 1 and mg.SubscriptionID is Not null and MAFField = @CustomField and sdt.NotExists = 1
			)
			insert into #tblAdhoc (SubscriptionID, AdhocValue)		
			
			SELECT   DISTINCT SubscriptionID, STUFF((SELECT ',' + inn.Value FROM Adhoc_CTE AS inn WHERE inn.SubscriptionID = Adhoc_CTE.SubscriptionID FOR XML PATH('')), 1, 1, '') AS AdhocValue
			FROM      Adhoc_CTE
			ORDER BY   SubscriptionID		
		
			print ' SubscriptionsExtensionMapper : ' + @standardField + ' / ' + @CustomField + ' / counts : ' + convert(varchar(100), @@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114)


-- Overwrite/Replace logic is been removed per Sunil on Consensus record			
			--if (@RuleType = 'overwrite')
			--BEGIN
			--	EXEC ('Update s1 Set ' + @StandardField + ' = left(AdhocValue,2048)
			--	FROM SubscriptionsExtension s1 
			--	join #tblAdhoc t on s1.subscriptionID = t.subscriptionID')
			--END

			--if (@RuleType = 'replace')
			--BEGIN
			--	EXEC ('Update s1 Set ' + @StandardField + ' = left(AdhocValue,2048)
			--	FROM SubscriptionsExtension s1 
			--	join #tblAdhoc t on s1.subscriptionID = t.subscriptionID
			--	where len(t.AdhocValue) > 0')
			--END

			--if (@RuleType = '')
			--BEGIN
				EXEC ('Update s1 Set ' + @StandardField + ' = CASE 
						WHEN t.AdhocValue = ' + @StandardField + ' THEN ' + @StandardField + '
						WHEN ' + @StandardField + ' is null THEN t.AdhocValue
						ELSE CAST(LTRIM(RTRIM(ISNULL(' + @StandardField + ',''''))) + '','' + LTRIM(RTRIM(t.AdhocValue)) AS VARCHAR(2048)) END
				FROM SubscriptionsExtension s1 
				join #tblAdhoc t on s1.subscriptionID = t.subscriptionID
				where len(t.AdhocValue) > 0')
			--END

			Exec ('INSERT INTO SubscriptionsExtension (SubscriptionID, ' + @StandardField + ') 
			SELECT t.subscriptionID, left(AdhocValue,2048)
			FROM #tblAdhoc t  left outer join SubscriptionsExtension se on se.subscriptionID = t.subscriptionID
			WHERE se.subscriptionID is null')
				
			Truncate table #tblAdhoc
			
			FETCH NEXT FROM curAdhoc INTO @CustomField, @StandardField, @RuleType
		END 
		CLOSE curAdhoc
		DEALLOCATE curAdhoc		
		
		drop table #tblAdhoc
	
	
		--FOR CIRC PRODUCTS Create BATCH, UPDTE HISTORY, HISTORYSUBSCRIPTION, HISTORYRESPONSE
		--This is only for CIRC PRODUCTS.
		
		if exists (select top 1 1 from Pubs p with (NOLOCK) join #MatchGroups mg on p.PubID = mg.PubID where isnull(p.IsCirc,0) = 1)
		Begin
		

			Update wmd
			SET
				Demo7 = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.demo7 THEN REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), 
					CHAR(9), ' ') ELSE wmd.Demo7 END,
				PubCategoryID = CASE WHEN sf.CategoryID <> ps.PubCategoryID THEN sf.CategoryID ELSE ps.PubCategoryID END,
				PubTransactionID = CASE WHEN sf.TransactionID <> ps.PubTransactionID THEN sf.TransactionID ELSE ps.PubTransactionID END,
				Copies = CASE WHEN ISNULL(sf.Copies,0) < 1 THEN 1 ELSE sf.Copies END,
				FirstName = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.FirstName 
					THEN REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.FirstName END,
				LastName = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.LastName 
					THEN REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.LastName END,
				Title = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.TITLE THEN 
					REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Title END,
				Company = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Company THEN 
					REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Company END,
				AddressTypeID = NULL,
				Address1 = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address1 THEN 
					REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Address1 END,
				Address2 = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address2 THEN 
					REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Address2 END,
				Address3 = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 
					AND REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address3 
					THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Address3 END,
				City = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 
					AND REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.City 
					THEN REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.City END,
				RegionCode = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.RegionCode THEN
					REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.RegionCode END,
				RegionID = CASE WHEN (select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
							sf.STATE <> ps.RegionCode THEN (select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) 
							ELSE wmd.RegionID END,
				ZipCode = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.ZipCode THEN 
					REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.ZipCode END,
				Plus4 = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Plus4 THEN 
					REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Plus4 END,
				County = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.County THEN 
					REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.County END,
				Country = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Country THEN 
					REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Country END,
				CountryID = CASE WHEN sf.CountryID > 0 AND sf.CountryID <> ps.CountryID THEN sf.CountryID ELSE wmd.CountryID END,
				Email = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Email THEN 
					REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Email END,
				Phone = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Phone THEN 
					REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Phone END,
				Fax = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Fax THEN 
					REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Fax END,
				Mobile = CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
					REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Mobile THEN 
					REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE wmd.Mobile END,					
				DateUpdated = GETDATE(),					
				UpdatedByUserID = 1,
				SubscriptionStatusID = CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
				WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
				WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
				WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
				WHEN cc.CategoryCodeTypeID in (1,2,3,4) AND tc.TransactionCodeTypeID in (1,3) AND cc.CategoryCodeValue in (70,71) THEN @APros 
				WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros END,
				IsSubscribed = CASE WHEN tc.TransactionCodeTypeID in (1,3) THEN 1 ELSE 0 END,
				IsPaid = CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 1 ELSE 0 END
			FROM PubSubscriptions ps
			join #MatchGroups mg on ps.PubSubscriptionID = mg.PubSubscriptionID 
			join Subscriberfinal sf With(NoLock) on sf.SubscriberFinalID = mg.SubscriberFinalID
			left join UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = (CASE WHEN sf.CategoryID <> ps.PubCategoryID THEN sf.CategoryID ELSE ps.PubCategoryID END)
			left join UAD_LookUp..TransactionCode tc ON tc.TransactionCodeID = (CASE WHEN sf.TransactionID <> ps.PubTransactionID THEN sf.TransactionID ELSE ps.PubTransactionID END)
			left join WaveMailing wm ON ps.WaveMailingID = wm.WaveMailingID
			left join Issue i ON i.PublicationId = ps.PubID and i.IssueId = wm.IssueID
			left join WaveMailingDetail wmd on wm.WaveMailingID = wmd.WaveMailingID and mg.PubSubscriptionID = wmd.PubSubscriptionID
			where IsNull(ps.IsInActiveWaveMailing,0) = 1 and i.IsComplete = 0 and mg.PubMatchFound = 1 and wmd.PubSubscriptionID is not null


			INSERT INTO WaveMailingDetail (WaveMailingID,PubSubscriptionID,SubscriptionID,Demo7,PubCategoryID,PubTransactionID,Copies,FirstName,LastName,Title,Company,AddressTypeID,Address1,
			Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,County,Country,CountryID,Email,Phone,Fax,Mobile,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriptionStatusID,
			IsSubscribed, IsPaid)
			SELECT
					wm.WaveMailingID,
					ps.PubSubscriptionID,
					ps.SubscriptionID,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.demo7 THEN REPLACE(REPLACE(REPLACE(sf.demo7, CHAR(13), ''), CHAR(10), ''), 
						CHAR(9), ' ') ELSE NULL END,
					CASE WHEN sf.CategoryID <> ps.PubCategoryID THEN sf.CategoryID ELSE NULL END,
					CASE WHEN sf.TransactionID <> ps.PubTransactionID THEN sf.TransactionID ELSE NULL END,
					CASE WHEN ISNULL(sf.Copies,0) < 1 THEN 1 ELSE sf.Copies END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.FirstName 
						THEN REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.LastName 
						THEN REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.TITLE THEN 
						REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Company THEN 
						REPLACE(REPLACE(REPLACE(sf.Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					NULL,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address1 THEN 
						REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address2 THEN 
						REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 
						AND REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Address3 
						THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 
						AND REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.City 
						THEN REPLACE(REPLACE(REPLACE(sf.City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.RegionCode THEN
						REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN (select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
								sf.STATE <> ps.RegionCode THEN (select RegionID from UAD_Lookup..Region where RegionCode = REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) 
								ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.ZipCode THEN 
						REPLACE(REPLACE(REPLACE(sf.Zip, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Plus4 THEN 
						REPLACE(REPLACE(REPLACE(sf.Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.County THEN 
						REPLACE(REPLACE(REPLACE(sf.County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Country THEN 
						REPLACE(REPLACE(REPLACE(sf.Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN sf.CountryID > 0 AND sf.CountryID <> ps.CountryID THEN sf.CountryID ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Email THEN 
						REPLACE(REPLACE(REPLACE(sf.Email, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Phone THEN 
						REPLACE(REPLACE(REPLACE(sf.Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Fax THEN 
						REPLACE(REPLACE(REPLACE(sf.Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					CASE WHEN LEN(REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ')) > 0 AND 
						REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') <> ps.Mobile THEN 
						REPLACE(REPLACE(REPLACE(sf.Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE NULL END,
					GETDATE(),
					NULL,
					1,
					1,
					CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
					WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
					WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
					WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
					WHEN cc.CategoryCodeTypeID in (1,2,3,4) AND tc.TransactionCodeTypeID in (1,3) AND cc.CategoryCodeValue in (70,71) THEN @APros 
					WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros END,
					CASE WHEN tc.TransactionCodeTypeID in (1,3) THEN 1 ELSE 0 END,
					CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 1 ELSE 0 END
			FROM PubSubscriptions ps
			join #MatchGroups mg on ps.PubSubscriptionID = mg.PubSubscriptionID 
			join Subscriberfinal sf With(NoLock) on sf.SubscriberFinalID = mg.SubscriberFinalID
			left join UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = (CASE WHEN sf.CategoryID <> ps.PubCategoryID THEN sf.CategoryID ELSE ps.PubCategoryID END)
			left join UAD_LookUp..TransactionCode tc ON tc.TransactionCodeID = (CASE WHEN sf.TransactionID <> ps.PubTransactionID THEN sf.TransactionID ELSE ps.PubTransactionID END)
			left join WaveMailing wm ON ps.WaveMailingID = wm.WaveMailingID
			left join Issue i ON i.PublicationId = ps.PubID and i.IssueId = wm.IssueID
			left join WaveMailingDetail wmd on wm.WaveMailingID = wmd.WaveMailingID and mg.PubSubscriptionID = wmd.PubSubscriptionID
			where IsNull(ps.IsInActiveWaveMailing,0) = 1 and i.IsComplete = 0 and mg.PubMatchFound = 1 and wmd.PubSubscriptionID is null
	    

			insert into Batch (PublicationID, UserID, BatchCount,IsActive, DateCreated, DateFinalized, BatchNumber)
			OUTPUT Inserted.PublicationID, Inserted.BatchID
			INTO #PubBatch
			select p.PubID, 1, COUNT(mg.SubscriberFinalID), 1, GETDATE() , GETDATE() , (select isnull(MAX(batchnumber),0) + 1 from Batch b with (NOLOCK) where b.PublicationID = p.PubID)
			from 
					Pubs p with (NOLOCK) 
					join #MatchGroups mg on p.PubID = mg.PubID 
			where isnull(p.IsCirc,0) = 1
			group by p.pubID
			
			Print ('insert into Batch COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
			
			--PubSubscriptionDetail History
			Insert into HistoryResponseMap (PubSubscriptionDetailID,PubSubscriptionID,SubscriptionID,CodeSheetID,IsActive,DateCreated,CreatedByUserID,ResponseOther, HistorySubscriptionID)
			OUTPUT Inserted.HistoryResponseMapID, Inserted.PubSubscriptionID
			INTO #HistoryResponseMap
			Select PubSubscriptionDetailID, mg.PubSubscriptionID, mg.SubscriptionID, CodeSheetID, 'true', GETDATE(), 1, ResponseOther, -1 
			from 
					#MatchGroups mg
					join Pubs p with (NOLOCK) on mg.pubID = p.PubID join 
					PubSubscriptionDetail psd with (NOLOCK) on mg.PubSubscriptionID = psd.PubSubscriptionID 
			where isnull(p.IsCirc,0) = 1 and psd.CodesheetID is not NULL

			Print ('Insert into HistoryResponseMap COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

			--PubSubscription History
			insert into HistorySubscription (PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,--StatusUpdatedDate, StatusUpdatedReason,
					Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,
					GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,
					ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
					CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
					Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,
					IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier)                       
			OUTPUT Inserted.HistorySubscriptionID, Inserted.SubscriptionID, Inserted.PubSubscriptionID
			INTO #HistorySubscription           
					select ps.PubSubscriptionID,ps.SubscriptionID,ps.PubID,ps.Demo7,ps.QualificationDate,ps.PubQSourceID,ps.PubCategoryID,ps.PubTransactionID,ps.EmailStatusID,--StatusUpdatedDate,ps.StatusUpdatedReason,
					ps.Email,GETDATE(),ps.DateUpdated,1,ps.UpdatedByUserID,ps.IsComp,ps.SubscriptionStatusID,ps.AccountNumber,ps.AddRemoveID,ps.Copies,ps.GraceIssues,ps.IMBSEQ,ps.IsActive,ps.IsPaid,
					ps.IsSubscribed,ps.MemberGroup,ps.OnBehalfOf,ps.OrigsSrc,ps.Par3CID,ps.SequenceID,ps.Status,ps.SubscriberSourceCode,ps.SubSrcID,ps.Verify,ps.ExternalKeyID,ps.FirstName,ps.LastName,ps.Company,ps.Title,ps.Occupation,
					ps.AddressTypeID,ps.Address1,ps.Address2,ps.Address3,ps.City,ps.RegionCode,ps.RegionID,ps.ZipCode,ps.Plus4,ps.CarrierRoute,ps.County,ps.Country,ps.CountryID,ps.Latitude,ps.Longitude,ps.IsAddressValidated,
					ps.AddressValidationDate,ps.AddressValidationSource,ps.AddressValidationMessage,ps.Phone,ps.Fax,ps.Mobile,ps.Website,ps.Birthdate,ps.Age,ps.Income,ps.Gender,ps.tmpSubscriptionID,ps.IsLocked,ps.LockedByUserID,
					ps.LockDate,ps.LockDateRelease,ps.PhoneExt,ps.IsInActiveWaveMailing,ps.AddressTypeCodeId,ps.AddressLastUpdatedDate,ps.AddressUpdatedSourceTypeCodeId,ps.WaveMailingID,ps.IGrp_No,ps.SFRecordIdentifier       
				from 
					#MatchGroups mg
					join Pubs p with (NOLOCK) on mg.pubID = p.PubID join 
					PubSubscriptions ps with (NOLOCK) on ps.PubSubscriptionID = mg.PubSubscriptionID 
			where isnull(p.IsCirc,0) = 1
			
			Print ('Insert into HistorySubscription COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

			Insert Into History (BatchID,BatchCountItem, PublicationID, PubSubscriptionID, SubscriptionID,HistorySubscriptionID,DateCreated,CreatedByUserID)
					OUTPUT Inserted.HistoryID, Inserted.PubSubscriptionID
					INTO #History
				select pb.BatchID, (row_number() over (partition by mg.PubSubscriptionID order by mg.PubSubscriptionID)), mg.PubID, mg.PubSubscriptionID, mg.SubscriptionID, hs.HistorySubscriptionID,GETDATE(),1
				from 
					#MatchGroups mg
					join #PubBatch pb on mg.PubID = pb.PubID
					join HistorySubscription hs with(nolock) on mg.SubscriptionID = hs.SubscriptionID 
				where 
					hs.HistorySubscriptionID in (Select HistorySubscriptionID from #HistorySubscription)

			Print ('Insert into History COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))

			Update hrm
			set HistorySubscriptionID = hs.HistorySubscriptionID
			from HistoryResponseMap hrm
			join #HistorySubscription hs on hrm.PubSubscriptionID = hs.PubSubscriptionID
			where 
					isnull(hrm.HistorySubscriptionID,0) = 0 or hrm.HistorySubscriptionID = -1 
					
			--Insert into HistoryToHistoryResponse (HistoryID,HistoryResponseID)
			--	   Select c.HistoryID, h.HistoryResponseMapID
			--	   from #History c join #HistoryResponseMap h on h.PubSubscriptionID = c.PubSubscriptionID
				   
			--Print ('Insert into HistoryToHistoryResponse COUNT : ' +  convert(varchar(100),@@ROWCOUNT) + ' / ' + convert(varchar(20), getdate(), 114))
			
		End
		-- END - Batch update *******************************************

		
	/* -- check for filetype (telemarketing, ECN_files, ncoa, acs) */

		--code type - database filetypeID.

		/* -- check for filetype (telemarketing, ECN_files, ncoa, acs) */
				--code type - database filetypeID.       
				--FileType comes from an enum that contains underscores
		if (@FileType = 'Telemarketing_Short_Form' or @FileType = 'Telemarketing_Long_Form' or @FileType = 'Web_Forms'  or @FileType = 'List_Source_2YR'  or @FileType = 'List_Source_3YR'  or @FileType = 'List_Source_Other')
			Begin
					Print (' exec job_TelemarketingRules_CustomCode_ProcessCode ' + ' / ' + convert(varchar(20), getdate(), 114))
					exec job_TelemarketingRules_CustomCode_ProcessCode @ProcessCode, @FileType
			End


		--
		--	UPDATE Subscriber final table		
		update sf 
		set		isUpdatedinLIVE = 1, 
				UpdateinLIVEDate = GETDATE() 
		from SubscriberFinal sf 
			join #MatchGroups mg on sf.SubscriberFinalID = mg.SubscriberFinalID
	    
		-- Add custom logic for Meister to update latest CIRC data on Master Record (Subscriptions) - sunil - 10/05/2015
		
		if (DB_NAME() like 'MEISTER%')
			exec job_SYNC_recent_CIRC_data_to_Subscriptions @ProcessCode

		set ANSI_WARNINGS  ON
	    
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		print 'begin catch'
		--SELECT
		--ERROR_NUMBER() AS ErrorNumber
		--,ERROR_SEVERITY() AS ErrorSeverity
		--,ERROR_STATE() AS ErrorState
		--,ERROR_PROCEDURE() AS ErrorProcedure
		--,ERROR_LINE() AS ErrorLine
		--,ERROR_MESSAGE() AS ErrorMessage;	
		
		--Print 'ERROR'
			
		ROLLBACK TRANSACTION;

		DECLARE @ErrorMessage NVARCHAR(4000);  
		DECLARE @ErrorSeverity INT;  
		DECLARE @ErrorState INT;  

		SET @ErrorMessage = ERROR_MESSAGE();  
		SET @ErrorSeverity = ERROR_SEVERITY();  
		SET @ErrorState = ERROR_STATE();  

		RAISERROR (@ErrorMessage, -- Message text.  
					  @ErrorSeverity, -- Severity.  
					  @ErrorState -- State.  
					  );  

		
	SET @s = 'API Import Notification Failed ' + convert(varchar(100),DB_NAME());
	SET @b = 'Processcode = ' + @Processcode + '; Error = ' + @ErrorMessage
	  EXEC msdb..sp_send_dbmail 
		@profile_name='SQLAdmin', 
		@recipients='charles.vashaw@teamkm.com;sunil.theenathayalu@TeamKM.com;Tod.Murray@TeamKM.com', 
		@importance='High',
		@body_format = 'HTML',
		@subject= @s, 
		@body=@b
    
		
	END CATCH;
	
	drop table #MatchGroups
	drop table #SubscriberFinalRollup
	drop table #psdNewDimensions 
	drop table #PubBatch 
	drop table #HistoryResponseMap 
	drop table #HistorySubscription 
	drop table #History
End


GO


