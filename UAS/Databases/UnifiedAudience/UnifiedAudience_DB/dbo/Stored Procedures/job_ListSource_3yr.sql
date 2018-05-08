CREATE PROCEDURE [dbo].[job_ListSource_3yr]
	@ProcessCode varchar(50)
AS
BEGIN

	SET NOCOUNT ON

	declare @ListSourceYear int = 3 - 1 --Subtract one because the code to get start and end date is already considered 1yr
	declare @ActionTypeID int = (select CodeId from UAD_Lookup..Code with(nolock) where CodeName = 'System Generated')
	declare @DefaultSubSrc varchar(10) = 'LIST' + CONVERT(varchar(2), GETDATE(), 101) + Right(Year(getDate()),2)
	declare @yearTemp int,
			@i int = 0,
			@sqlstring varchar(4000) = '',
			@startperiod varchar(10),
			@endperiod varchar(10),
			@startdateTemp datetime,
			@enddateTemp datetime
	declare @PublicationID int = (Select distinct p.PubID from Pubs p with(nolock) 
								join SubscriberFinal sf with(nolock) on sf.PubCode = p.PubCode 
								where sf.ProcessCode = @ProcessCode)

	select @startperiod = p.YearStartDate , @endperiod = p.YearEndDate from Pubs p with(nolock) where PubID = @PublicationID

	if getdate() > convert(datetime,@startperiod + '/' + convert(varchar,year(getdate())))
		set @yearTemp = year(getdate()) 
	else
		set @yearTemp = year(getdate()) - 1

	select @startdateTemp = @startperiod + '/' + convert(varchar,@yearTemp)
	select @endDateTemp =  dateadd(ss, -1, dateadd(yy, 1, @startdateTemp) )
	

	declare @Par3cOne int = (Select CodeId from UAD_Lookup..Code with(nolock) where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Par3c') and CodeValue = 'INTF')
	declare @Par3cTwo int = (Select CodeId from UAD_Lookup..Code with(nolock) where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Par3c') and CodeValue = 'INO')
	declare @Par3cThree int = (Select CodeId from UAD_Lookup..Code with(nolock) where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Par3c') and CodeValue = 'TFO')
	declare @Par3cFour int = (Select CodeId from UAD_Lookup..Code with(nolock) where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Par3c') and CodeValue = 'CNO')
	declare @Par3cFive int = (Select CodeId from UAD_Lookup..Code with(nolock) where CodeTypeID = (Select CodeTypeID from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Par3c') and CodeValue = 'BC')

	
	/* Get SubscriberFinal search by IGrp_No	(List should include Sequence Matches on Name where IGrp was updated and other existing records) */
	Select SF.*, PS.PubSubscriptionID, S.SubscriptionID
	into #tmpPubSubMatches
	from SubscriberFinal SF with(nolock)
	left join Subscriptions S with(nolock) on S.IGrp_No = SF.IGrp_No
	left join Pubs P with(nolock) on P.PubCode = SF.PubCode
	left join PubSubscriptions PS with(nolock) on PS.SubscriptionID = S.SubscriptionID and PS.PubID = P.PubID	
	where SF.ProcessCode = @ProcessCode and PS.PubSubscriptionID is not null


	/* Rest of SubscriberFinal these will be the records without a Sub or PubSub entry from above */
	Insert into #tmpPubSubMatches
	Select SF.*, isnull(PS.PubSubscriptionID, 0), isnull(S.SubscriptionID, 0)
	from SubscriberFinal SF with(nolock)
	left join Subscriptions S with(nolock) on S.IGrp_No = SF.IGrp_No
	left join Pubs P with(nolock) on P.PubCode = SF.PubCode
	left join PubSubscriptions PS with(nolock) on PS.SubscriptionID = S.SubscriptionID and PS.PubID = P.PubID	
	where SF.ProcessCode = @ProcessCode and SF.SubscriberFinalID not in (Select SubscriberFinalID from #tmpPubSubMatches)	


	/* Set Copies to database record */
	Update sf
	set sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN PS.Copies ELSE sf.Copies END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
	left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	left join PubSubscriptions PS with(nolock) on t.PubSubscriptionID = PS.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode


	/* Set every record in Subscriber to Ignore before determining which record should be updated */
	Update sf
	set sf.Ignore = 'true'
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode and t.PubSubscriptionID <> 0

	/* Ignore to false where records are inactive */
	Update sf
	set sf.Ignore = 'false'
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode and (sf.QDate >= cs.Qualificationdate)
	and cs.PubTransactionID in (Select tc.TransactionCodeID
							   From UAD_Lookup..TransactionCode tc with(nolock)
							   where tc.TransactionCodeID = cs.PubTransactionID and tc.TransactionCodeValue = 38)
	and cs.pubCategoryID in (Select cc.CategoryCodeID
							   From UAD_Lookup..CategoryCode cc with(nolock) 
							   where cc.CategoryCodeID = cs.PubCategoryID and cc.CategoryCodeValue in (10,17,70,71))	

	/* Ignore to false where file QDate > record QDate AND record QSource in (I,J,K,L,M,N,E,F,G,H,S) Skip Paid Active/Inactives */
	Update sf
	set sf.Ignore = 'false'
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode and (sf.QDate >= cs.Qualificationdate and cs.PubQSourceID in 
		(Select C.CodeId from UAD_Lookup..Code C with(nolock) join UAD_Lookup..CodeType CT with(nolock) on C.CodeTypeId = CT.CodeTypeId 
			where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('I','J','K','L','M','N', 'E','F','G','H','S')))
	and cs.PubTransactionID not in (Select tc.TransactionCodeID
							   From UAD_Lookup..Action a with(nolock) join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
							   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
							   and tc.TransactionCodeTypeID in (3,4) and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID)


	/* Ignore to false where file QDate > record QDate AND record QDate is a 3+ yr AND record QSource in (A,B,C,D,Q,R) Skip Paid Active/Inactives */
	Update sf
	set sf.Ignore = 'false'
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode and (sf.QDate >= cs.Qualificationdate 
		and (cs.Qualificationdate between dateadd(yy, -@ListSourceYear, @startdateTemp) and dateadd(yy, -@ListSourceYear,  @endDateTemp) OR cs.Qualificationdate <= dateadd(yy, -@ListSourceYear, @endDateTemp))
		and cs.PubQSourceID in (Select C.CodeId from UAD_Lookup..Code C with(nolock) join UAD_Lookup..CodeType CT with(nolock) on C.CodeTypeId = CT.CodeTypeId 
			where CT.CodeTypeName = 'Qualification Source' and C.CodeValue in ('A','B','C','D','Q','R')))
	and cs.PubTransactionID not in (Select tc.TransactionCodeID
							   From UAD_Lookup..Action a with(nolock) join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
							   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
							   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
							   and tc.TransactionCodeTypeID in (3,4) and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID)

	
	/* Update Cat/Tran for new records */
	update sf
    set sf.CategoryID = (Select cc.CategoryCodeID		
						   From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
						   and a.IsActive = 'true'
						   and a.ActionTypeID = @ActionTypeID),
		sf.TransactionID = (Select tc.TransactionCodeID		
						   From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10
						   and a.IsActive = 'true'
						   and a.ActionTypeID = @ActionTypeID),
		sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where (cs.PubSubscriptionID = 0 or cs.PubSubscriptionID is null) and sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'


	/* Update Cat/Tran for Match Active */
	update sf
	set sf.CategoryID = (Select cc.CategoryCodeID From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeValue = 27
						   and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID),
		sf.TransactionID = (Select tc.TransactionCodeID From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeValue = 27
						   and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID)	
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where
	sf.ProcessCode = @ProcessCode and sf.Ignore = 'false' and cs.PubSubscriptionID > 0
	and cs.PubCategoryID in (Select cc.CategoryCodeID From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where (cc.CategoryCodeTypeID = 1 or cc.CategoryCodeTypeID = 2)
						   and tc.TransactionCodeTypeID = 1 and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID)	


	/* Inactive (Free or Paid) */
	Update sf
	set sf.CategoryID = (Select cc.CategoryCodeID From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10 and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID), 						   
	   sf.TransactionID = (Select tc.TransactionCodeID From UAD_Lookup..Action a with(nolock)
						   join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
						   join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
						   where cc.CategoryCodeValue = 70 and tc.TransactionCodeValue = 10 and a.IsActive = 'true' and a.ActionTypeID = @ActionTypeID)
	from SubscriberFinal sf with(nolock)
	join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	where
	sf.ProcessCode = @ProcessCode and cs.PubSubscriptionID > 0
        and cs.PubTransactionID in (Select distinct tc.TransactionCodeID From UAD_Lookup..Action a with(nolock)
                            join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
                            join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID                           
                            where (cc.CategoryCodeTypeID in (1,2,3,4)) and tc.TransactionCodeTypeID in (2,4) and a.ActionTypeID = @ActionTypeID)           


	/* Set Subsrc and OrigsSrc if blank and for new records */
	Update sf
	set sf.OrigsSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),
		sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
	left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	where sf.ProcessCode = @ProcessCode	and (t.PubSubscriptionID is null or t.PubSubscriptionID = 0) and sf.Ignore = 'false'


	/* Set Subsrc if blank existing records */
	Update sf
	set sf.SubSrc = (CASE WHEN LEN(sf.SubSrc) > 0 THEN sf.SubSrc ELSE @DefaultSubSrc END),		
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	from SubscriberFinal sf With(NoLock)
	left join #tmpPubSubMatches t on sf.SFRecordIdentifier = t.SFRecordIdentifier
	left join PubSubscriptions PS with(nolock) on t.PubSubscriptionID = PS.PubSubscriptionID
	where sf.ProcessCode = @ProcessCode	and t.PubSubscriptionID > 0 and sf.Ignore = 'false'


	/* Update Par3C */
	update sf
    set sf.Par3C = @Par3cOne,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where ((sdf.Value is not null and sdf.Value != '' and sdf.Value != 'zz' and sdf.Value != '99') or sf.Title != '') and (sf.FName != '' or sf.LName != '')	
	and sf.ProcessCode = @ProcessCode	
	and sf.Ignore = 'false'

	update sf
    set sf.Par3C = @Par3cOne,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)	
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') AND (sf.FName != '' OR sf.LName != '')	
	and sf.ProcessCode = @ProcessCode	
	and sf.Ignore = 'false'
	
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
	and sf.Ignore = 'false'
	
	update sf
    set sf.Par3C = @Par3cTwo,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '') and (sf.FName != '' or sf.LName != '') 
	and sf.ProcessCode = @ProcessCode	
	and sf.Ignore = 'false'
	
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
	and sf.Ignore = 'false'

	update sf
    set sf.Par3C = @Par3cThree,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99') and (sf.FName = '' and sf.LName = '') 
	and sf.ProcessCode = @ProcessCode	
	and sf.Ignore = 'false'

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
	and sf.Ignore = 'false'

	update sf
    set sf.Par3C = @Par3cFour,
	sf.DateUpdated = GETDATE(), sf.UpdatedByUserID = 1
	from SubscriberFinal sf with(nolock)
	left join #tmpPubSubMatches t on t.SFRecordIdentifier = sf.SFRecordIdentifier 
	left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
	left join SubscriberDemographicFinal sdf with(nolock) on sdf.SFRecordIdentifier = sf.SFRecordIdentifier and sdf.MAFField = 'Function'
	where (sdf.Value = '99' and sf.Title = '' and sf.FName = '' and sf.LName = '' and sf.Company != '')
	and sf.ProcessCode = @ProcessCode	
	and sf.Ignore = 'false'

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
	and sf.Ignore = 'false'

	----Commented out 11-3-2016, Engine now handles this correctly.
	/* Update EmailStatusID */
	--Update sf
	--set sf.EmailStatusID = case when LEN(sf.Email) > 0 then 1 else 0 end,
	--	DateUpdated = GETDATE(),
	--	UpdatedByUserID = 1
	--from SubscriberFinal sf With(NoLock)
	--where sf.ProcessCode = @ProcessCode
	--and sf.Ignore = 'false' 


	--/* Update QSource to PubSubscriptions where file QSource doesn't exist or zero */
	--Update sf
	--set sf.QSourceID = ps.PubQSourceID,				
	--	DateUpdated = GETDATE(),
	--	UpdatedByUserID = 1
	--from SubscriberFinal sf With(NoLock)
	--join Subscriptions s With(NoLock) on sf.IGrp_No = s.IGrp_No
	--join PubSubscriptions ps With(NoLock) on s.SubscriptionID = ps.SubscriptionID
	--join Pubs p With(NoLock) on ps.PubID = p.PubID and sf.PubCode = p.PubCode
	--where sf.ProcessCode = @ProcessCode
	--and sf.Ignore = 'false' and sf.QSourceID < 1


	/* UPDATE DEMO7 Blank/Null to A */
	update sf
	set sf.DateUpdated = GETDATE(),
	sf.UpdatedByUserID = 1,
	sf.Demo7 = isnull(nullif(sf.Demo7,''),'A')
	from SubscriberFinal sf With(NoLock)
	where sf.ProcessCode = @ProcessCode AND sf.Ignore = 'false' and
		isnull(nullif(sf.Demo7,''),'A') in 
		(Select CodeValue from UAD_Lookup..Code with(nolock) where CodeTypeId in (Select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Deliver')) 		


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