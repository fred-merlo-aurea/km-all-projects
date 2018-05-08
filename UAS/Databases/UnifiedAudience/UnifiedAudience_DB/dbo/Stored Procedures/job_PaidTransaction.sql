CREATE PROCEDURE job_PaidTransaction
	@ProcessCode varchar(50),
	@OverrideQDate bit = 'false',
	@MailPermissionOverRide bit = 'false',
	@FaxPermissionOverRide bit = 'false',
	@PhonePermissionOverRide bit = 'false',
	@OtherProductsPermissionOverRide bit = 'false',
	@ThirdPartyPermissionOverRide bit = 'false',
	@EmailRenewPermissionOverRide	 bit = 'false',
	@TextPermissionOverRide	 bit = 'false'
AS
BEGIN

	SET NOCOUNT ON
	
	declare @ActionTypeID int = (select CodeId from UAD_Lookup..Code where CodeName = 'System Generated')

	declare @Par3cOne int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'INTF')
	declare @Par3cTwo int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'INO')
	declare @Par3cThree int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'TFO')
	declare @Par3cFour int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'CNO')
	declare @Par3cFive int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'BC')


	/* Get SubscriberFinal search by IGrp_No. (List should include Sequence Matches on Name where IGrp was updated and other existing records) */
	Select SF.*, PS.PubSubscriptionID, S.SubscriptionID
	into #tmpPubSubMatches
	from SubscriberFinal SF
		left join Subscriptions S on S.IGrp_No = SF.IGrp_No
		left join Pubs P on P.PubCode = SF.PubCode
		left join PubSubscriptions PS on PS.SubscriptionID = S.SubscriptionID and PS.PubID = P.PubID	
	where SF.ProcessCode = @ProcessCode and PS.PubSubscriptionID is not null

	/* Rest of SubscriberFinal these will be the records without a Sub or PubSub entry from above */
	Insert into #tmpPubSubMatches
	Select SF.*, isnull(PS.PubSubscriptionID, 0), isnull(S.SubscriptionID, 0)
	from SubscriberFinal SF
		left join Subscriptions S on S.IGrp_No = SF.IGrp_No
		left join Pubs P on P.PubCode = SF.PubCode
		left join PubSubscriptions PS on PS.SubscriptionID = S.SubscriptionID and PS.PubID = P.PubID	
	where SF.ProcessCode = @ProcessCode and SF.SubscriberFinalID not in (Select SubscriberFinalID from #tmpPubSubMatches)	

	/* Set Copies to database record */
	Update sf
	set sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN PS.Copies ELSE sf.Copies END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
		left join PubSubscriptions PS on t.PubSubscriptionID = PS.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode

	/* CategoryID and TransactionID Maintain old if invalid code */
	/* Default invalid Cat to -1 if invalid value */
	UPDATE SubscriberFinal
			SET CategoryID = NULL
	WHERE ProcessCode = @ProcessCode 
		AND (CategoryID NOT IN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock)))

	/* Convert -1 to the current records converted value by id */
	--Update sf
	--	set sf.CategoryID = CASE WHEN sf.CategoryID = -1 or sf.CategoryID = 0 THEN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock) where CategoryCodeID = cs.PubCategoryID) ELSE sf.CategoryID END
	--from SubscriberFinal sf with(nolock)
	--left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	--left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	--where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'

	/* Default invalid Tran to -1 if invalid value */
	UPDATE SubscriberFinal
		SET TransactionID = NULL
	WHERE ProcessCode = @ProcessCode 
		AND (TransactionID NOT IN (SELECT TransactionCodeValue FROM UAD_Lookup..[TransactionCode] with(nolock)))

	/* Convert -1 to the current records converted value by id */
	--Update sf
	--	set sf.TransactionID = CASE WHEN sf.TransactionID = -1 or sf.TransactionID = 0 THEN (SELECT TransactionCodeValue FROM UAD_Lookup..[TransactionCode] with(nolock) where TransactionCodeID = cs.PubTransactionID) ELSE sf.TransactionID END
	--from SubscriberFinal sf with(nolock)
	--left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	--left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	--where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'

	/* Change from Value to ID */
	UPDATE SubscriberFinal
		set CategoryID = a.CategoryCodeID
	FROM SubscriberFinal sf 
		join uad_lookup..vw_Action a on a.CategoryCodeValue = sf.CategoryID and actiontypeid = 2 and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
	WHERE ProcessCode = @ProcessCode and a.isFreeTransactionCodeType = 'false'

	UPDATE SubscriberFinal
		set TransactionID = a.TransactionCodeID
	FROM SubscriberFinal sf 
		join uad_lookup..vw_Action a on a.TransactionCodeValue = sf.TransactionID and actiontypeid = 2 and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
	WHERE ProcessCode = @ProcessCode and a.isFreeTransactionCodeType = 'false'

	
	/* Ignore Records where Cat/Tran not updated to ID. Most likely a bad Cat/Tran combo */
	--UPDATE SubscriberFinal
	--	set Ignore = 'true'
	--WHERE ProcessCode = @ProcessCode and
	--(CategoryID not in (SELECT CategoryCodeID FROM UAD_Lookup..CategoryCode with(nolock)) or TransactionID not in (SELECT TransactionCodeID FROM UAD_Lookup..[TransactionCode] with(nolock)))


	/* Start AccountNumber/Transaction Duplication Ignore */
	DECLARE @SaveSubFinalRecords Table (SubscriberFinalID int, PubCode varchar(100), SubGenSubscriberID int)

	Select Count(*) as Dupes, PubCode, SubGenSubscriberID, MAX(QDate) as MaxQDate
	into #tmpDupeAccts 
	from SubscriberFinal
	where ProcessCode = @ProcessCode and Ignore = 'false' and ISNULL(SubGenSubscriberID,0) != 0
	group by PubCode, SubGenSubscriberID
	having Count(*) > 1

	/* Add record with one newest qdate to temp table */
	Insert into @SaveSubFinalRecords
	Select sf1.SubscriberFinalID, sf1.PubCode, sf1.SubGenSubscriberID
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.SubGenSubscriberID, sf.QDate from SubscriberFinal sf
				join #tmpDupeAccts tmp on sf.SubGenSubscriberID = tmp.SubGenSubscriberID and sf.PubCode = tmp.PubCode
			where sf.QDate = tmp.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.SubGenSubscriberID, sf.QDate
			having Count(*) < 2
		) as a1 on sf1.SubGenSubscriberID = a1.SubGenSubscriberID and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode

	/* Add record with newest qdate and active paid record to temp table */
	Insert into @SaveSubFinalRecords
	Select MIN(sf1.SubscriberFinalID) as SubscriberFinalID, sf1.PubCode, sf1.SubGenSubscriberID
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.SubGenSubscriberID, sf.QDate from SubscriberFinal sf
				join #tmpDupeAccts da on sf.SubGenSubscriberID = da.SubGenSubscriberID and sf.PubCode = da.PubCode
			where sf.QDate = da.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.SubGenSubscriberID, sf.QDate
			having Count(*) > 1
		) as a1 on sf1.SubGenSubscriberID = a1.SubGenSubscriberID and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode and sf1.Ignore = 'false'
		and sf1.TransactionID in (Select TransactionCodeID from UAD_Lookup..TransactionCode where TransactionCodeTypeId = 3)
	group by sf1.PubCode, sf1.SubGenSubscriberID
	
	/* Add record with newest qdate and inactive paid record to temp table because all dupe records aren't active paid */
	Insert into @SaveSubFinalRecords
	Select MIN(sf1.SubscriberFinalID) as SubscriberFinalID, sf1.PubCode, sf1.SubGenSubscriberID
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.SubGenSubscriberID, sf.QDate from SubscriberFinal sf
				join #tmpDupeAccts da on sf.SubGenSubscriberID = da.SubGenSubscriberID and sf.PubCode = da.PubCode
			where sf.QDate = da.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.SubGenSubscriberID, sf.QDate
			having Count(*) > 1
		) as a1 on sf1.SubGenSubscriberID = a1.SubGenSubscriberID and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode and sf1.Ignore = 'false'
		and sf1.TransactionID in (Select TransactionCodeID from UAD_Lookup..TransactionCode where TransactionCodeTypeId = 4)
		and sf1.SubGenSubscriberID not in (Select SubGenSubscriberID from @SaveSubFinalRecords)
	group by sf1.PubCode, sf1.SubGenSubscriberID

	/* Ignore Records that have dupe account number */
	Update sf
		set Ignore = 'true'
	from SubscriberFinal sf
		join @SaveSubFinalRecords ss on sf.SubGenSubscriberID = ss.SubGenSubscriberID and sf.PubCode = ss.PubCode
	where sf.ProcessCode = @ProcessCode and sf.SubscriberFinalID <> ss.SubscriberFinalID

	/* END ACCOUNT NUMBER DE-DUPE */


	/* Maintain current Par3c */
	Update sf
		set sf.Par3C = cs.Par3CID
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'


	/* Maintain current QSource if incoming blank/null */
	--Update sf
	--	set sf.QSourceID = CASE WHEN sf.QSourceID = -1 OR sf.QSourceID = 0 THEN cs.PubQSourceID ELSE sf.QSourceID END
	--from SubscriberFinal sf with(nolock)
	--left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	--left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	--where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'


	/* Maintain current QDate if incoming blank/null/or not originally mapped */
	--if (@OverrideQDate = 'false')
	--	BEGIN
	--		Update sf
	--			set sf.QDate = cs.QualificationDate
	--		from SubscriberFinal sf with(nolock)
	--		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	--		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	--		where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'
	--	END
	--else
	--	BEGIN
	--		Update sf
	--			set sf.QDate = CASE WHEN sf.QDate is null THEN cs.QualificationDate ELSE sf.QDate END
	--		from SubscriberFinal sf with(nolock)
	--		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	--		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	--		where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false' and sf.QDate is null
	--	END


	/* Update Par3C */
	update sf
    set sf.Par3C = @Par3cOne,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value is not null and sdf.Value != '' and sdf.Value != 'zz' and sdf.Value != '99') or sf.Title != '') and (sf.FName != '' or sf.LName != '')	
		and sf.ProcessCode = @ProcessCode	

	update sf
    set sf.Par3C = @Par3cOne,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)	
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') AND (sf.FName != '' OR sf.LName != '')	
		and sf.ProcessCode = @ProcessCode	
	
	--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' or function = 'ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
	--new 7/6/15
	--REPLACE ALL PAR3C WITH '2' FOR ((function ='  ' OR function ='ZZ') AND TITLE ='  ') AND (FNAME#'  ' OR LNAME #'  ')
	--REPLACE ALL PAR3C WITH '2' FOR (function ='99' AND TITLE ='  ' AND FUNCTEXT ='  ') AND (FNAME#'  ' OR LNAME #'  ')	

	update sf
    set sf.Par3C = @Par3cTwo,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value is null or sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
		and sf.ProcessCode = @ProcessCode	
	
	update sf
    set sf.Par3C = @Par3cTwo,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
		and sf.ProcessCode = @ProcessCode	
	
	--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function # 'ZZ') OR TITLE #'  ') AND (FNAME ='  ' AND LNAME ='  ')
	--new 7/6/2015
	--REPLACE ALL PAR3C WITH '3' FOR ((function# '  ' AND function# '99' AND function #'ZZ') OR TITLE #' ') AND (FNAME ='  ' AND LNAME ='  ')
	--REPLACE ALL PAR3C WITH '3' FOR (function = '99'  and functext #'  ') AND (FNAME ='  ' AND LNAME ='  ')	
	
	update sf
    set sf.Par3C = @Par3cThree,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value is not null and sdf.Value != '' and sdf.Value != '99' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName = '' or sf.LName = '') 
		and sf.ProcessCode = @ProcessCode	

	update sf
    set sf.Par3C = @Par3cThree,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') and (sf.FName = '' or sf.LName = '') 
		and sf.ProcessCode = @ProcessCode	

	--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' or function = 'ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
	--new 7/6/2015
	--REPLACE ALL PAR3C WITH '4' FOR ((function =' ' OR FUNCTION ='ZZ') AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')
	--REPLACE ALL PAR3C WITH '4' FOR (function ='99' AND FUNCTEXT ='  ' AND TITLE ='  ' AND FNAME ='  ' AND LNAME ='  ' AND COMPANY #'  ')	
	
	update sf
    set sf.Par3C = @Par3cFour,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value is null or sdf.Value = '' or sdf.Value = 'zz') and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
		and sf.ProcessCode = @ProcessCode	

	update sf
    set sf.Par3C = @Par3cFour,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
		and sf.ProcessCode = @ProcessCode	

	--REPLACE ALL PAR3C WITH '5' FOR COPIES > 1
	
	update sf
    set sf.Par3C = @Par3cFive,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where sf.Copies > 1
		and sf.ProcessCode = @ProcessCode	

END
go