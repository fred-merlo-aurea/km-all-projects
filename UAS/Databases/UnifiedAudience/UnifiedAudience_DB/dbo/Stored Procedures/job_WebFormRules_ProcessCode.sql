CREATE PROCEDURE [dbo].[job_WebFormRules_ProcessCode]
@ProcessCode varchar(50)
as
BEGIN

	SET NOCOUNT ON

	declare @ActionTypeID int = (select CodeId from UAD_Lookup..Code where CodeName = 'System Generated')
	declare @PubSourceQ int = (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue = 'Q')
	declare @DefaultSubSrc varchar(10) = 'WEB' + CONVERT(varchar(2), GETDATE(), 101) + Right(Year(getDate()),2)

	declare @Par3cOne int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'INTF')
	declare @Par3cTwo int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'INO')
	declare @Par3cThree int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'TFO')
	declare @Par3cFour int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'CNO')
	declare @Par3cFive int = (Select CodeId from UAD_Lookup..Code where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType where CodeTypeName = 'Par3c') and CodeValue = 'BC')

	/* Get SubscriberFinal search by IGrp_No	(List should include Sequence Matches on Name where IGrp was updated and other existing records) */
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


	/* Set Subsrc and OrigsSrc if blank and for new records */
	Update sf
	set sf.OrigsSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),
		sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode	and (t.PubSubscriptionID is null or t.PubSubscriptionID = 0)


	/* Set Subsrc if blank existing records */
	Update sf
	set sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
		left join PubSubscriptions PS on t.PubSubscriptionID = PS.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode	and t.PubSubscriptionID > 0


	/* Set Copies to database record */
	Update sf
	set sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN PS.Copies ELSE sf.Copies END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
		left join PubSubscriptions PS on t.PubSubscriptionID = PS.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode
	 

	/* Ignore records where Subscription Field Value = 'N' will come in as a demographic field */
	--Update sf
	--set sf.Ignore = 'true', sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	--from SubscriberFinal sf With(NoLock)
	--left join SubscriberDemographicFinal sdf With(NoLock) on sf.SFRecordIdentifier = sdf.SFRecordIdentifier
	--where sf.ProcessCode = @ProcessCode and sdf.MAFField = 'Subscription' and sdf.Value = 'N'
	--and (sf.QDate < cs.Qualificationdate
	--and cs.PubQSourceID not in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId 
	--	where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))


	/* Record incoming qdate older than db set to ignore */
	update sf
    set sf.Ignore = 'true', sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1	
	from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode and (sf.QDate < cs.Qualificationdate
		and cs.PubQSourceID not in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId 
		where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
	

	/* Clear @pubcode.kmpsgroup.com Email */
	update sf
	set sf.Email = '', EmailStatusID = 0
	from SubscriberFinal sf with(nolock)
	where sf.Email like '%@pubcode.kmpsgroup.com%'


	/* Paid Active With Different Name Set Ignore */
	update sf
		set sf.Ignore = 'true', sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs With(NoLock) on t.PubSubscriptionID = cs.PubSubscriptionID
		--left join PubSubscriptions cs1 With(NoLock) on sf.Sequence = cs1.SequenceID
	where sf.ProcessCode = @ProcessCode
		and ((sf.FName != cs.FirstName or sf.LName != cs.LastName) 
		--and sf.Sequence > 0
		and cs.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
									   and tc.TransactionCodeTypeID = 3
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID))


	/* Paid Active Sequence With Same Name Set Ignore */
	update sf
	set sf.Ignore = 'true', sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs With(NoLock) on t.PubSubscriptionID = cs.PubSubscriptionID
		--left join PubSubscriptions cs1 With(NoLock) on sf.Sequence = cs1.SequenceID
	where sf.ProcessCode = @ProcessCode
		and ((sf.FName = cs.FirstName and sf.LName = cs.LastName)
		--and sf.Sequence > 0
		and cs.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
									   and tc.TransactionCodeTypeID = 3
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID))


	/* Paid InActive With Different Name Set New Record */
	update sf
		set sf.CategoryID = (Select cc.CategoryCodeID		
							From UAD_Lookup..Action a
								join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
								and a.IsActive = 'true'
								and a.ActionTypeID = @ActionTypeID),
			sf.TransactionID = (Select tc.TransactionCodeID		
							   From UAD_Lookup..Action a
								   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
								   and a.IsActive = 'true'
								   and a.ActionTypeID = @ActionTypeID),
			sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1,
			sf.OrigsSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),
			sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END)
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs With(NoLock) on t.PubSubscriptionID = cs.PubSubscriptionID
		--left join PubSubscriptions cs1 With(NoLock) on sf.Sequence = cs1.SequenceID
	where sf.ProcessCode = @ProcessCode
		and ((sf.FNAME != cs.FirstName or sf.LNAME != cs.LastName) 
		--and sf.Sequence > 0
		and cs.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
									   and tc.TransactionCodeTypeID = 4
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID))


	/* Paid InActive Sequence With Same Name Set New Record */
	update sf
		set sf.CategoryID = (Select cc.CategoryCodeID		
							From UAD_Lookup..Action a
								join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
								and a.IsActive = 'true'
								and a.ActionTypeID = @ActionTypeID),
			sf.TransactionID = (Select tc.TransactionCodeID		
							   From UAD_Lookup..Action a
								   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
								   and a.IsActive = 'true'
								   and a.ActionTypeID = @ActionTypeID),
			sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1,
			sf.OrigsSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),
			sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END)
	from SubscriberFinal sf With(NoLock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs With(NoLock) on t.PubSubscriptionID = cs.PubSubscriptionID
		--left join PubSubscriptions cs1 With(NoLock) on sf.Sequence = cs1.SequenceID
	where sf.ProcessCode = @ProcessCode
		and ((sf.FName = cs.FirstName and sf.LName = cs.LastName)
		--and sf.Sequence > 0
		and cs.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
									   and tc.TransactionCodeTypeID = 4
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID))


	/* Update Cat/Tran for new records */
	update sf
    set sf.CategoryID = (Select cc.CategoryCodeID		
						From UAD_Lookup..Action a
							join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							and a.IsActive = 'true'
							and a.ActionTypeID = @ActionTypeID),
		sf.TransactionID = (Select tc.TransactionCodeID		
							From UAD_Lookup..Action a
								join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
								and a.IsActive = 'true'
								and a.ActionTypeID = @ActionTypeID),
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	where (cs.PubSubscriptionID = 0 or cs.PubSubscriptionID is null) and sf.ProcessCode = @ProcessCode


	/* Update Cat/Tran for Match Active */
	update sf
	set sf.CategoryID = (Select cc.CategoryCodeID
						From UAD_Lookup..Action a
							join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeValue = 27
							and a.IsActive = 'true'
							and a.ActionTypeID = @ActionTypeID),
		sf.TransactionID = (Select tc.TransactionCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeValue = 27
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)	
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode		
	and (sf.QDate >= cs.Qualificationdate 
	or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
	and cs.PubCategoryID in (Select cc.CategoryCodeID
							From UAD_Lookup..Action a
								join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							where (cc.CategoryCodeTypeID = 1 or cc.CategoryCodeTypeID = 2)
								and tc.TransactionCodeTypeID = 1
								--and a.IsActive = 'true'
								and a.ActionTypeID = @ActionTypeID)
	and cs.PubSubscriptionID > 0

	/* Inactive (Free or Paid) and Cat 71 */
	Update sf
	set sf.CategoryID = (Select cc.CategoryCodeID
						From UAD_Lookup..Action a
							join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							and a.IsActive = 'true'
							and a.ActionTypeID = @ActionTypeID), 						   
	   sf.TransactionID = (Select tc.TransactionCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)
	from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		join PubSubscriptions cs on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode
		and (cs.PubCategoryID = (Select cc.CategoryCodeID
							   From UAD_Lookup..Action a
								   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
								   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
							   where cc.CategoryCodeValue = 71 and tc.TransactionCodeValue = 10
								   and a.IsActive = 'true'
								   and a.ActionTypeID = @ActionTypeID)
        or cs.PubTransactionID in (Select distinct tc.TransactionCodeID
									From UAD_Lookup..Action a
										join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
										join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID                           
									where (cc.CategoryCodeTypeID in (1,2,3,4))
										and tc.TransactionCodeTypeID in (2,4)
										--and a.IsActive = 'true'
										and a.ActionTypeID = @ActionTypeID))        
		and cs.PubSubscriptionID > 0	


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
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID		
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

	update sf
    set sf.Par3C = @Par3cOne,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)	
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') AND (sf.FName != '' OR sf.LName != '')	
		and sf.ProcessCode = @ProcessCode	
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))
	
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
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))
	
	update sf
    set sf.Par3C = @Par3cTwo,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
		and sf.ProcessCode = @ProcessCode	
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))
	
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
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

	update sf
    set sf.Par3C = @Par3cThree,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') and (sf.FName = '' and sf.LName = '') 
		and sf.ProcessCode = @ProcessCode	
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

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
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

	update sf
    set sf.Par3C = @Par3cFour,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
		and sf.ProcessCode = @ProcessCode	
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

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
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeID
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))

	----Commented out EmailStatusID 11-3-2016, Engine now handles this correctly.
	/* Update QSource */
	Update sf
	set sf.QSourceID = @PubSourceQ,		
		--sf.EmailStatusID = case when LEN(sf.Email) > 0 then 1 else 0 end,	
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
	where sf.ProcessCode = @ProcessCode


	/* UPDATE DEMO7 */
	update sf
	set sf.DateUpdated = GETDATE(),
	sf.UpdatedByUserID = 1,
	sf.Demo7 = isnull(nullif(sf.Demo7,''),'A')
	from SubscriberFinal sf With(NoLock)
		join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
		join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where isnull(nullif(sf.Demo7,''),'A') in 
		(Select CodeValue from UAD_Lookup..Code where CodeTypeId in (Select CodeTypeId from UAD_Lookup..CodeType where CodeTypeName = 'Deliver')) 	
		and ((sf.QDate >= cs.Qualificationdate or cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C join UAD_Lookup..CodeType CT on C.CodeTypeId = CT.CodeTypeId where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('G','H','I','J','K','L','M','N','S')))
		or (sf.CategoryID = (Select cc.CategoryCodeValue
						   From UAD_Lookup..Action a
							   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
							   and a.IsActive = 'true'
							   and a.ActionTypeID = @ActionTypeID)))


	/* UPDATE DEMO7 B,C with Blank Email to A */
	update sf
	set sf.DateUpdated = GETDATE(),
	sf.UpdatedByUserID = 1,
	sf.Demo7 = 'A'
	from SubscriberFinal sf With(NoLock)
	where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false' and
		sf.Demo7 in ('B','C') and sf.Email = '' 

	drop table #tmpPubSubMatches

END