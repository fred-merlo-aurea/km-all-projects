CREATE PROCEDURE [dbo].[job_QuickFill]
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
	WHERE ProcessCode = @ProcessCode

	UPDATE SubscriberFinal
		set TransactionID = a.TransactionCodeID
	FROM SubscriberFinal sf 
		join uad_lookup..vw_Action a on a.TransactionCodeValue = sf.TransactionID and actiontypeid = 2 and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
	WHERE ProcessCode = @ProcessCode

	
	/* Ignore Records where Cat/Tran not updated to ID. Most likely a bad Cat/Tran combo */
	--UPDATE SubscriberFinal
	--	set Ignore = 'true'
	--WHERE ProcessCode = @ProcessCode and
	--(CategoryID not in (SELECT CategoryCodeID FROM UAD_Lookup..CategoryCode with(nolock)) or TransactionID not in (SELECT TransactionCodeID FROM UAD_Lookup..[TransactionCode] with(nolock)))


	/* Start AccountNumber/Transaction Duplication Ignore */
	DECLARE @SaveSubFinalRecords Table (SubscriberFinalID int, PubCode varchar(100), AccountNumber varchar(50))

	Select Count(*) as Dupes, PubCode, AccountNumber, MAX(QDate) as MaxQDate
	into #tmpDupeAccts 
	from SubscriberFinal
	where ProcessCode = @ProcessCode and Ignore = 'false' and ISNULL(AccountNumber,'') != ''
	group by PubCode, AccountNumber
	having Count(*) > 1

	/* Add record with one newest qdate to temp table */
	Insert into @SaveSubFinalRecords
	Select sf1.SubscriberFinalID, sf1.PubCode, sf1.AccountNumber
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.AccountNumber, sf.QDate 
			from SubscriberFinal sf
				join #tmpDupeAccts tmp on sf.AccountNumber = tmp.AccountNumber and sf.PubCode = tmp.PubCode
			where sf.QDate = tmp.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.AccountNumber, sf.QDate
			having Count(*) < 2
		) as a1 on sf1.AccountNumber = a1.AccountNumber and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode

	/* Add record with newest qdate and active paid record to temp table */
	Insert into @SaveSubFinalRecords
	Select MIN(sf1.SubscriberFinalID) as SubscriberFinalID, sf1.PubCode, sf1.AccountNumber
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.AccountNumber, sf.QDate 
			from SubscriberFinal sf
				join #tmpDupeAccts da on sf.AccountNumber = da.AccountNumber and sf.PubCode = da.PubCode
			where sf.QDate = da.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.AccountNumber, sf.QDate
			having Count(*) > 1
		) as a1 on sf1.AccountNumber = a1.AccountNumber and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode and sf1.Ignore = 'false'
		and sf1.TransactionID in (Select TransactionCodeID from UAD_Lookup..TransactionCode where TransactionCodeTypeId = 3)
	group by sf1.PubCode, sf1.AccountNumber
	
	/* Add record with newest qdate and inactive paid record to temp table because all dupe records aren't active paid */
	Insert into @SaveSubFinalRecords
	Select MIN(sf1.SubscriberFinalID) as SubscriberFinalID, sf1.PubCode, sf1.AccountNumber
	from SubscriberFinal sf1
		join
		(
			Select sf.PubCode, sf.AccountNumber, sf.QDate 
			from SubscriberFinal sf
				join #tmpDupeAccts da on sf.AccountNumber = da.AccountNumber and sf.PubCode = da.PubCode
			where sf.QDate = da.MaxQDate and sf.ProcessCode = @ProcessCode
			group by sf.PubCode, sf.AccountNumber, sf.QDate
			having Count(*) > 1
		) as a1 on sf1.AccountNumber = a1.AccountNumber and sf1.PubCode = a1.PubCode and sf1.QDate = a1.QDate
	where sf1.ProcessCode = @ProcessCode and sf1.Ignore = 'false'
		and sf1.TransactionID in (Select TransactionCodeID from UAD_Lookup..TransactionCode where TransactionCodeTypeId = 4)
		and sf1.AccountNumber not in (Select AccountNumber from @SaveSubFinalRecords)
	group by sf1.PubCode, sf1.AccountNumber

	/* Ignore Records that have dupe account number */
	Update sf
		set Ignore = 'true'
	from SubscriberFinal sf
		join @SaveSubFinalRecords ss on sf.AccountNumber = ss.AccountNumber and sf.PubCode = ss.PubCode
	where sf.ProcessCode = @ProcessCode and sf.SubscriberFinalID <> ss.SubscriberFinalID

	/* END ACCOUNT NUMBER DE-DUPE */

	/* ROLL UP Multiple Value Responses to the main, first grabs distinct from secondary and change the SFRecordIdentifier to main */
	DECLARE @RollUpMultiValueResponses Table (AccountNumber int, MAFField varchar(255), Value varchar(max), SFRecordIdentifier uniqueidentifier null)

	--Want records ignored (these are the secondary) and multiple value except don't take main to ensure no duplicates
	insert into @RollUpMultiValueResponses 
	(AccountNumber, MAFField, Value)
	Select * from
	(
		Select sf.AccountNumber, sdf.MAFField, sdf.Value 
			from SubscriberFinal sf WITH(NOLOCK) 
			join SubscriberDemographicFinal sdf WITH(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
			join CodeSheet cs WITH(NOLOCK) on sdf.MAFField = cs.ResponseGroup and sdf.PubID = cs.PubID and sdf.Value = cs.Responsevalue
			join ResponseGroups rg WITH(NOLOCK) on cs.ResponseGroupID = rg.ResponseGroupID and rg.IsMultipleValue = 1
			where sf.Ignore = 1 and sf.ProcessCode = @ProcessCode
		EXCEPT
		Select sf.AccountNumber, sdf.MAFField, sdf.Value 
			from SubscriberFinal sf WITH(NOLOCK) 
			join SubscriberDemographicFinal sdf WITH(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
			where sf.Ignore = 0 and sf.ProcessCode = @ProcessCode
	) as A

	--Set the SFRecordIdentifier (this will be the main record SFRecordIdentifier) used for next update
	Update r
	set SFRecordIdentifier = sf.SFRecordIdentifier
	from @RollUpMultiValueResponses r
		join SubscriberFinal sf WITH(NOLOCK) on r.AccountNumber = sf.AccountNumber and sf.Ignore = 0 and sf.ProcessCode = @ProcessCode	

	--Set SFRecordIdentifier, grabs records where only one instance then first record where multiple instances
	Update sdf
	set SFRecordIdentifier = a.SFRecordIdentifier
	from SubscriberDemographicFinal sdf
		join (
			Select MIN(sdf.SDFinalID) as SDFinalID, sdf.PubID, sdf.MAFField, sdf.Value, r.SFRecordIdentifier
			from @RollUpMultiValueResponses r
				join SubscriberFinal sf WITH(NOLOCK) on r.AccountNumber = sf.AccountNumber and sf.ProcessCode = @ProcessCode	
				join SubscriberDemographicFinal sdf WITH(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier and r.MAFField = sdf.MAFField and r.Value = sdf.Value
				group by sdf.PubID, sdf.MAFField, sdf.Value, r.SFRecordIdentifier
				having count(*) = 1
			union
			Select MIN(sdf.SDFinalID) as SDFinalID, sdf.PubID, sdf.MAFField, sdf.Value, r.SFRecordIdentifier
			from @RollUpMultiValueResponses r
				join SubscriberFinal sf WITH(NOLOCK) on r.AccountNumber = sf.AccountNumber and sf.ProcessCode = @ProcessCode	
				join SubscriberDemographicFinal sdf WITH(NOLOCK) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier and r.MAFField = sdf.MAFField and r.Value = sdf.Value
				group by sdf.PubID, sdf.MAFField, sdf.Value, r.SFRecordIdentifier
				having count(*) > 1
		) a on sdf.SDFinalID = a.SDFinalID
	/*END ROLLUP TO MAIN*/

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
	where ((sdf.Value is not null and sdf.Value != '' and sdf.Value != '99' and sdf.Value != 'zz') or sf.Title != '') and (sf.FName = '' and sf.LName = '') 
		and sf.ProcessCode = @ProcessCode	

	update sf
    set sf.Par3C = @Par3cThree,
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') and (sf.FName = '' and sf.LName = '') 
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


	/* Maintain Values where Blank */
	--Update sf
	--	Set
	--		sf.[SEQUENCE]  = convert(int,isnull(sf.[SEQUENCE], 0)),
	--		sf.FNAME       = (CASE WHEN ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FirstName END),
	--		sf.LNAME       = (CASE WHEN ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.LastName END),
	--		sf.TITLE       = (CASE WHEN ISNULL(sf.TITLE,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.TITLE END),
	--		sf.COMPANY     = (CASE WHEN ISNULL(sf.COMPANY,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COMPANY END),
	--		sf.ADDRESS     = (CASE WHEN	ISNULL(sf.Address,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS1 END),
	--		sf.MAILSTOP    = (CASE WHEN ISNULL(sf.MailStop,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS2 END),
	--		sf.ADDRESS3    = (CASE WHEN ISNULL(sf.Address3,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS3 END),               
	--		sf.CITY        = (CASE WHEN ISNULL(sf.City,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.CITY END),
	--		sf.STATE       = (CASE WHEN ISNULL(sf.State,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.RegionCode END),
	--		sf.ZIP         = (CASE WHEN ISNULL(sf.Zip,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ZIPCODE END),
	--		sf.PLUS4       = (CASE WHEN ISNULL(sf.Plus4,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PLUS4 END),
	--		sf.FORZIP      = (CASE WHEN ISNULL(sf.ForZip,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FORZIP END),
	--		sf.COUNTY      = (CASE WHEN ISNULL(sf.County,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COUNTY END),
	--		sf.COUNTRY     = (CASE WHEN ISNULL(sf.Country,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COUNTRY END),
	--		sf.CountryID   = (CASE WHEN ISNULL(sf.CountryID,'')!='' THEN sf.CountryID ELSE ps.CountryID END),
	--		sf.PHONE       = (CASE WHEN ISNULL(sf.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PHONE END),
	--		sf.PhoneExists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PHONE END),''))) <> '' then 1 else 0 end),
	--		sf.FAX         = (CASE WHEN ISNULL(sf.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FAX END),
	--		sf.Faxexists   = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FAX END),''))) <> '' then 1 else 0 end), 
	--		sf.Email       = (CASE WHEN ISNULL(sf.EMail,'')!='' THEN sf.Email ELSE ps.EMAIL END),
	--		sf.emailexists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.EMail,'')!='' THEN sf.Email ELSE ps.EMAIL END),''))) <> '' then 1 else 0 end), 
	--		sf.CategoryID  = sf.CategoryID,
	--		sf.TransactionID = sf.TransactionID,
	--		sf.TransactionDate = (CASE WHEN ISNULL(sf.TransactionDate,GETDATE())!=GETDATE() THEN sf.TransactionDate ELSE ps.PubTransactionDate END),
	--		sf.QDate       =  sf.QDate,
	--		sf.QSourceID   = sf.QSourceID,
	--		sf.RegCode	   = case when isnull(sf.RegCode,'')!='' then sf.RegCode else s.REGCODE end,
	--		sf.Verified	   = case when isnull(sf.Verified,'')!='' then sf.Verified else ps.Verify end,
	--		sf.SubSrc	   = case when isnull(sf.SubSrc,'')!='' then sf.SubSrc else ps.SubscriberSourceCode end,
	--		sf.OrigsSrc    = ps.OrigsSrc,
	--		sf.PAR3C       = sf.PAR3C,
	--		sf.Demo31      = (CASE WHEN @Demo31OverRide = 'false' THEN s.Demo31 ELSE sf.DEMO31 END),
	--		sf.Demo32      = (CASE WHEN @Demo32OverRide = 'false' THEN s.Demo32 ELSE sf.Demo32 END),
	--		sf.Demo33      = (CASE WHEN @Demo33OverRide = 'false' THEN s.Demo33 ELSE sf.DEMO33 END),
	--		sf.Demo34      = (CASE WHEN @Demo34OverRide = 'false' THEN s.Demo34 ELSE sf.DEMO34 END),
	--		sf.Demo35      = (CASE WHEN @Demo35OverRide = 'false' THEN s.Demo35 ELSE sf.DEMO35 END),
	--		sf.Demo36      = (CASE WHEN @Demo36OverRide = 'false' THEN s.Demo36 ELSE sf.DEMO36 END),
	--		sf.[Source]	   = case when isnull(sf.[Source],'')!='' then sf.[Source] else s.[Source] end,
	--		sf.[Priority]  = case when isnull(sf.[Priority],'')!='' then sf.[Priority] else s.[Priority] end,
	--		sf.IGRP_CNT    = case when isnull(sf.IGRP_CNT, -1) > 0 then sf.IGRP_CNT else s.IGRP_CNT end,
	--		sf.CGrp_No	   = case when sf.CGrp_No is not null then sf.CGrp_No else s.CGrp_No end,
	--		sf.CGrp_Cnt	   = case when isnull(sf.CGrp_Cnt, -1) > 0 then sf.CGrp_Cnt else s.CGrp_Cnt end,
	--		sf.StatList	   = sf.StatList,
	--		sf.Sic		   = case when isnull(sf.Sic,'')!='' then sf.Sic else s.Sic end,
	--		sf.SicCode	   = case when isnull(sf.SicCode,'')!='' then sf.SicCode else s.SicCode end,
	--		sf.Gender      = (CASE WHEN ISNULL(sf.Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.Gender END),
	--		sf.IGrp_Rank   = case when isnull(sf.IGrp_Rank,'')!='' then sf.IGrp_Rank else s.IGrp_Rank end,
	--		sf.CGrp_Rank   = case when isnull(sf.CGrp_Rank,'')!='' then sf.CGrp_Rank else s.CGrp_Rank end,
	--		sf.Home_Work_Address = case when isnull(sf.Home_Work_Address,'')!='' then sf.Home_Work_Address else s.Home_Work_Address end,
	--		sf.PubIDs	   = case when isnull(sf.PubIDs,'')!='' then sf.PubIDs else s.PubIDs end,
	--		sf.Demo7       = case when isnull(sf.demo7,'') != '' then sf.demo7 else ps.Demo7 end,
	--		sf.IsExcluded  = case when sf.IsExcluded is not null then sf.IsExcluded else s.IsExcluded end,
	--		sf.MOBILE      = (CASE WHEN ISNULL(sf.MOBILE,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.mobile END),		
	--		sf.Score	   = case when isnull(sf.Score, -1) > 0 then sf.Score else s.Score end,																																							
	--		sf.DateUpdated = GETDATE(),
	--		sf.IsMailable  = sf.IsMailable
	--	From 
 --           Subscriptions s With(NoLock)
	--		join PubSubscriptions ps With(NoLock) on s.SubscriptionID = ps.SubscriptionID
 --           join SubscriberFinal sf With(NoLock) on sf.IGrp_No = s.IGrp_No
	--		join Pubs p With(NoLock) on ps.PubID = p.PubID and sf.PubCode = p.PubCode
	--	WHERE 
	--		sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'

END