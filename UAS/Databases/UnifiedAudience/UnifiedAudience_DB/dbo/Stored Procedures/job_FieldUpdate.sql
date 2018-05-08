--IF EXISTS (SELECT 1 FROM Sysobjects where name = 'job_FieldUpdate')
--DROP Proc job_FieldUpdate
--GO


--SET ANSI_NULLS ON
--GO

--SET QUOTED_IDENTIFIER ON
--GO

--execute [e_ImportFromUAS] @processcode = '55Lg638wGFpo_12242016_02:11:02'
CREATE PROCEDURE [dbo].[job_FieldUpdate]
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
	declare @FreeActive int = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Active')
	declare @FreeInactive int = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Inactive')

    /* Get SubscriberFinal search by IGrp_No. (List should include Sequence Matches on Name where IGrp was updated and other existing records) */
    Select SF.SFRecordIdentifier, PS.PubSubscriptionID, S.SubscriptionID
    into #tmpPubSubMatches
    from SubscriberFinal SF with(nolock)
		join Subscriptions S with(nolock) on SF.IGrp_No = S.IGrp_No
		join Pubs P with(nolock) on SF.PubCode = P.PubCode
		join PubSubscriptions PS with(nolock) on S.SubscriptionID = PS.SubscriptionID and P.PubID = PS.PubID    
    where SF.ProcessCode = @ProcessCode


	CREATE CLUSTERED INDEX IDX_tmpPubSubMatches_PubSubscriptionID ON #tmpPubSubMatches(PubSubscriptionID)   
    CREATE INDEX IDX_tmpPubSubMatches_SFRecordIdentifier ON #tmpPubSubMatches(SFRecordIdentifier)


  --  /* Rest of SubscriberFinal these will be the records without a Sub or PubSub entry from above */
  --  Insert into #tmpPubSubMatches
  --  Select SF.SFRecordIdentifier, 0, isnull(S.SubscriptionID, 0)
  --  from SubscriberFinal SF with(nolock)
		--left join Subscriptions S with(nolock) on SF.IGrp_No = S.IGrp_No 
  --  where SF.ProcessCode = @ProcessCode and SF.SFRecordIdentifier not in (Select SFRecordIdentifier from #tmpPubSubMatches)  	

 
	/* REVISED - Next ignore all records that are new... Paid are ok as of 03-02-2016. */
	Update sf
		set sf.Ignore = 'true'
	from SubscriberFinal sf with(nolock)
		left join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		--left join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
    where sf.ProcessCode = @ProcessCode and  t.PubSubscriptionID is null
    --(ISNULL(t.PubSubscriptionID,0) = 0)
    
			--(t.PubSubscriptionID <> 0 and (cs.IsPaid = 'true' or cs.PubTransactionID in 
			--(Select tc.TransactionCodeID
			--From UAD_Lookup..Action a with(nolock) 
			--join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
			--join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
			--where cc.CategoryCodeID = cs.PubCategoryID and tc.TransactionCodeID = cs.PubTransactionID
			--and tc.TransactionCodeTypeID in (3,4) and a.IsActive = 'true' and a.ActionTypeID = 2)))
			--OR 
			--ISNULL(t.PubSubscriptionID,0) = 0)


    /* REVISED Address Changes, ZipCode changes, Maintain Par3c, Maintain QSource, Maintain QDate */
    Update sf						
		set sf.MailStop = (CASE WHEN ISNULL(sf.Address,'') = '' THEN (CASE WHEN ISNULL(sf.MailStop,'') <> '' THEN sf.MailStop ELSE cs.Address2 END)
								WHEN (ISNULL(sf.Address,'') != '' and sf.Address != cs.Address1) THEN (CASE WHEN ISNULL(sf.MailStop,'') <> '' THEN sf.MailStop ELSE '' END)
								WHEN (ISNULL(sf.Address,'') != '' and sf.Address = cs.Address1) THEN (CASE WHEN ISNULL(sf.MailStop,'') <> '' THEN sf.MailStop ELSE cs.Address2 END)
								ELSE (CASE WHEN ISNULL(sf.MailStop,'') <> '' THEN sf.MailStop ELSE cs.Address2 END) END),
		    sf.Address3 = (CASE WHEN ISNULL(sf.Address,'') = '' THEN (CASE WHEN ISNULL(sf.Address3,'') <> '' THEN sf.Address3 ELSE cs.Address3 END)
								WHEN (ISNULL(sf.Address,'') != '' and sf.Address != cs.Address1) THEN (CASE WHEN ISNULL(sf.Address3,'') <> '' THEN sf.Address3 ELSE '' END)
								WHEN (ISNULL(sf.Address,'') != '' and sf.Address = cs.Address1) THEN (CASE WHEN ISNULL(sf.Address3,'') <> '' THEN sf.Address3 ELSE cs.Address3 END)
								ELSE (CASE WHEN ISNULL(sf.Address3,'') <> '' THEN sf.Address3 ELSE cs.Address3 END) END),
		   	sf.Plus4 = (CASE WHEN ISNULL(sf.Zip,'') = '' THEN (CASE WHEN ISNULL(sf.Plus4,'') <> '' THEN sf.Plus4 ELSE cs.Plus4 END) 
		   					 WHEN (ISNULL(sf.Zip,'') != '' and sf.Zip != cs.ZipCode) THEN (CASE WHEN ISNULL(sf.Plus4,'') <> '' THEN sf.Plus4 ELSE '' END)
		   					 WHEN (ISNULL(sf.Zip,'') != '' and sf.Zip = cs.ZipCode) THEN (CASE WHEN ISNULL(sf.Plus4,'') <> '' THEN sf.Plus4 ELSE cs.Plus4 END)
		   					 ELSE (CASE WHEN ISNULL(sf.Plus4,'') <> '' THEN sf.Plus4 ELSE cs.Plus4 END) END),
		   	sf.QSourceID = (CASE WHEN sf.QSourceID = -1 OR sf.QSourceID = 0 THEN cs.PubQSourceID ELSE sf.QSourceID END),
		   	sf.Par3C = (CASE WHEN ISNULL(sf.Par3C,'') = '' OR sf.Par3C = '-1' OR sf.Par3C = '0' THEN cs.Par3CID ELSE sf.Par3C END),
		   	sf.QDate = (CASE WHEN @OverrideQDate = 'false' THEN cs.QualificationDate 
		   					 ELSE (CASE WHEN sf.QDate is null THEN cs.QualificationDate ELSE sf.QDate END) END)
    from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
    where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'        


	/* Update Null IsPaid before setting CAT/TRAN - No default the next step will do this */
	Update ps
		set IsPaid = (CASE WHEN a.TransactionCodeTypeName = 'Free Active' THEN 'false'
                WHEN a.TransactionCodeTypeName = 'Free Inactive' THEN 'false'
                WHEN a.TransactionCodeTypeName = 'Paid Active' THEN 'true'
                WHEN a.TransactionCodeTypeName = 'Paid Inactive' THEN 'true' END)
	from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		join PubSubscriptions ps with(nolock) on t.PubSubscriptionID = ps.PubSubscriptionID
		join Pubs p on ps.PubID = p.PubID
		join UAD_Lookup..vw_Action a on ps.PubCategoryID = a.CategoryCodeID and ps.PubTransactionID = a.TransactionCodeID 
			and a.isFreeCategoryCodeType = isFreeTransactionCodeType and a.ActionTypeID = 2
	where p.IsCirc = 'true' and ISNull(ps.IsPaid,0) = 0 and sf.Ignore = 'false'


	/* Update Null IsPaid before setting CAT/TRAN - Provides default of false if can't determine by tran then cat */
	Update ps
		set IsPaid = (CASE WHEN tc.TransactionCodeTypeID in (3,4) THEN 'true'
			WHEN tc.TransactionCodeTypeID in (1,2) THEN 'false'
			WHEN cc.CategoryCodeTypeID in (3,4) THEN 'true'
			WHEN cc.CategoryCodeTypeID in (1,2) THEN 'false' ELSE 'false' END)
	from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		join PubSubscriptions ps with(nolock) on t.PubSubscriptionID = ps.PubSubscriptionID
		join Pubs p on ps.PubID = p.PubID
		left join UAD_Lookup..CategoryCode cc with(nolock) on ps.PubCategoryID = cc.CategoryCodeID
		left join UAD_Lookup..TransactionCode tc with(nolock) on ps.PubTransactionID = tc.TransactionCodeID		
	where p.IsCirc = 'true' and ISNull(ps.IsPaid,0) = 0 and sf.Ignore = 'false'


	/* REVISE CAT/TRAN Convert -1 to the current records convert value to id for valid (PubSub isPaid and transaction matches Paid = Paid/Free = Free) */
    Update sf
        set sf.CategoryID = ISNULL(cc.CategoryCodeID,-1),
			sf.TransactionID = ISNULL(tc.TransactionCodeID,-1),
			sf.Ignore = (CASE WHEN (ISNULL(a.CategoryCodeID,0) = 0 OR ISNULL(a.TransactionCodeID,0) = 0) THEN 'true' ELSE 'false' END)
    from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID
		left join UAD_Lookup..CategoryCode cc with(nolock) on (CASE WHEN sf.CategoryID = -1 or sf.CategoryID = 0 THEN (SELECT CategoryCodeValue FROM UAD_Lookup..CategoryCode with(nolock) where CategoryCodeID = cs.PubCategoryID) ELSE sf.CategoryID END) = cc.CategoryCodeValue
		left join UAD_Lookup..TransactionCode tc with(nolock) on (CASE WHEN sf.TransactionID = -1 or sf.TransactionID = 0 THEN (SELECT TransactionCodeValue FROM UAD_Lookup..TransactionCode with(nolock) where TransactionCodeID = cs.PubTransactionID) ELSE sf.TransactionID END) = tc.TransactionCodeValue
		left join UAD_Lookup..vw_Action a with(nolock) on a.CategoryCodeID = ISNULL(cc.CategoryCodeID,-1) and a.TransactionCodeID = ISNULL(tc.TransactionCodeID,-1) and actiontypeid = @ActionTypeID and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
    where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'
		and (CASE WHEN cs.IsPaid = 'false' and tc.TransactionCodeTypeID = 1 THEN 'true'
				   WHEN cs.IsPaid = 'false' and tc.TransactionCodeTypeID = 2 THEN 'true'
				   WHEN cs.IsPaid = 'true' and tc.TransactionCodeTypeID = 3 THEN 'true'
				   WHEN cs.IsPaid = 'true' and tc.TransactionCodeTypeID = 4 THEN 'true' else 'false' end) = 'true'


	/* CAT/TRAN Ignore invalid */
    Update sf
		set sf.Ignore = (CASE WHEN (ISNULL(a.CategoryCodeID,0) = 0 OR ISNULL(a.TransactionCodeID,0) = 0) THEN 'true' ELSE 'false' END)
    from SubscriberFinal sf with(nolock)
		join #tmpPubSubMatches t with(nolock) on sf.SFRecordIdentifier = t.SFRecordIdentifier
		join PubSubscriptions cs with(nolock) on t.PubSubscriptionID = cs.PubSubscriptionID    
		left join UAD_Lookup..vw_Action a with(nolock) on a.CategoryCodeID = ISNULL(sf.CategoryID,-1) and a.TransactionCodeID = ISNULL(sf.TransactionID,-1) and actiontypeid = @ActionTypeID and a.isFreeCategoryCodeType = a.isFreeTransactionCodeType
    where sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'


	/* Maintain Values where Blank */
	Update sf
		Set sf.[SEQUENCE]  = convert(int,isnull(sf.[SEQUENCE], 0)),
			sf.FNAME       = (CASE WHEN ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FirstName END),
			sf.LNAME       = (CASE WHEN ISNULL(sf.Fname,'')!='' AND ISNULL(sf.LName,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.LNAME, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.LastName END),
			sf.TITLE       = (CASE WHEN ISNULL(sf.TITLE,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.TITLE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.TITLE END),
			sf.COMPANY     = (CASE WHEN ISNULL(sf.COMPANY,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COMPANY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COMPANY END),
			sf.ADDRESS     = (CASE WHEN	ISNULL(sf.Address,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS1 END),
			--sf.MAILSTOP    = (CASE WHEN ISNULL(sf.MailStop,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS2 END),
			--sf.ADDRESS3    = (CASE WHEN ISNULL(sf.Address3,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ADDRESS3 END),               
			sf.CITY        = (CASE WHEN ISNULL(sf.City,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.CITY END),
			sf.STATE       = (CASE WHEN ISNULL(sf.State,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.RegionCode END),
			sf.ZIP         = (CASE WHEN ISNULL(sf.Zip,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.ZIPCODE END),
			--sf.PLUS4       = (CASE WHEN ISNULL(sf.Plus4,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PLUS4 END),
			sf.FORZIP      = (CASE WHEN ISNULL(sf.ForZip,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FORZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.FORZIP END),
			sf.COUNTY      = (CASE WHEN ISNULL(sf.County,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COUNTY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COUNTY END),
			sf.COUNTRY     = (CASE WHEN ISNULL(sf.Country,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.COUNTRY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.COUNTRY END),
			sf.CountryID   = (CASE WHEN ISNULL(sf.CountryID,0) > 0 THEN sf.CountryID ELSE ps.CountryID END),
			sf.PHONE       = (CASE WHEN ISNULL(sf.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PHONE END),
			--sf.PhoneExists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PHONE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.PHONE END),''))) <> '' then 1 else 0 end),
			sf.FAX         = (CASE WHEN ISNULL(sf.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FAX END),
			--sf.Faxexists   = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.FAX,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.FAX, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.FAX END),''))) <> '' then 1 else 0 end), 
			sf.Email       = (CASE WHEN ISNULL(sf.EMail,'')!='' THEN sf.Email ELSE ps.EMAIL END),
			--sf.emailexists = (case when ltrim(rtrim(isnull((CASE WHEN ISNULL(sf.EMail,'')!='' THEN sf.Email ELSE ps.EMAIL END),''))) <> '' then 1 else 0 end), 
			sf.CategoryID  = sf.CategoryID,
			sf.TransactionID = sf.TransactionID,
			sf.TransactionDate = (CASE WHEN ISNULL(sf.TransactionDate,GETDATE())!=GETDATE() THEN sf.TransactionDate ELSE ps.PubTransactionDate END),
			sf.QDate       =  sf.QDate,
			sf.QSourceID   = sf.QSourceID,
			sf.RegCode	   = case when isnull(sf.RegCode,'')!='' then sf.RegCode else s.REGCODE end,
			sf.Verified	   = case when isnull(sf.Verified,'')!='' then sf.Verified else ps.Verify end,
			sf.SubSrc	   = case when isnull(sf.SubSrc,'')!='' then sf.SubSrc else ps.SubscriberSourceCode end,
			sf.OrigsSrc    = ps.OrigsSrc,
			sf.PAR3C       = sf.PAR3C,
			sf.MailPermission      = (CASE WHEN @MailPermissionOverRide = 'false' OR sf.MailPermission is null THEN ps.MailPermission ELSE sf.MailPermission END),
			sf.FaxPermission      = (CASE WHEN @FaxPermissionOverRide = 'false' OR sf.FaxPermission is null THEN ps.FaxPermission ELSE sf.FaxPermission END),
			sf.PhonePermission      = (CASE WHEN @PhonePermissionOverRide = 'false' OR sf.PhonePermission is null THEN ps.PhonePermission ELSE sf.PhonePermission END),
			sf.OtherProductsPermission      = (CASE WHEN @OtherProductsPermissionOverRide = 'false' OR sf.OtherProductsPermission is null THEN ps.OtherProductsPermission ELSE sf.OtherProductsPermission END),
			sf.ThirdPartyPermission      = (CASE WHEN @ThirdPartyPermissionOverRide = 'false' OR sf.ThirdPartyPermission is null THEN ps.ThirdPartyPermission ELSE sf.ThirdPartyPermission END),
			sf.EmailRenewPermission      = (CASE WHEN @EmailRenewPermissionOverRide = 'false' OR sf.EmailRenewPermission is null THEN ps.EmailRenewPermission ELSE sf.EmailRenewPermission END),
			sf.TextPermission      = (CASE WHEN @TextPermissionOverRide = 'false' OR sf.TextPermission is null THEN ps.TextPermission ELSE sf.TextPermission END),
			sf.[Source]	   = case when isnull(sf.[Source],'')!='' then sf.[Source] else s.[Source] end,
			sf.[Priority]  = case when isnull(sf.[Priority],'')!='' then sf.[Priority] else s.[Priority] end,
			sf.IGRP_CNT    = case when isnull(sf.IGRP_CNT, -1) > 0 then sf.IGRP_CNT else s.IGRP_CNT end,
			sf.CGrp_No	   = case when sf.CGrp_No is not null then sf.CGrp_No else s.CGrp_No end,
			sf.CGrp_Cnt	   = case when isnull(sf.CGrp_Cnt, -1) > 0 then sf.CGrp_Cnt else s.CGrp_Cnt end,
			sf.StatList	   = sf.StatList,
			sf.Sic		   = case when isnull(sf.Sic,'')!='' then sf.Sic else s.Sic end,
			sf.SicCode	   = case when isnull(sf.SicCode,'')!='' then sf.SicCode else s.SicCode end,
			sf.Gender      = (CASE WHEN ISNULL(sf.Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.Gender END),
			sf.IGrp_Rank   = case when isnull(sf.IGrp_Rank,'')!='' then sf.IGrp_Rank else s.IGrp_Rank end,
			sf.CGrp_Rank   = case when isnull(sf.CGrp_Rank,'')!='' then sf.CGrp_Rank else s.CGrp_Rank end,
			sf.Home_Work_Address = case when isnull(sf.Home_Work_Address,'')!='' then sf.Home_Work_Address else s.Home_Work_Address end,
			--sf.PubIDs	   = case when isnull(sf.PubIDs,'')!='' then sf.PubIDs else s.PubIDs end,
			sf.Demo7       = case when isnull(sf.demo7,'') != '' then sf.demo7 else ps.Demo7 end,
			--sf.IsExcluded  = case when sf.IsExcluded is not null then sf.IsExcluded else s.IsExcluded end,
			sf.MOBILE      = (CASE WHEN ISNULL(sf.MOBILE,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.MOBILE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ps.mobile END),		
			--sf.Score	   = case when isnull(sf.Score, -1) > 0 then sf.Score else s.Score end,																																							
			sf.DateUpdated = GETDATE(),
			sf.IsMailable  = sf.IsMailable,
			sf.ExternalKeyId = case when sf.ExternalKeyId = -1 OR sf.ExternalKeyId = 0 then ps.ExternalKeyID else sf.ExternalKeyId end,
			sf.AccountNumber = case when isnull(sf.AccountNumber,'') != '' then sf.AccountNumber else ps.AccountNumber end,
			sf.EmailID = case when sf.EmailID = -1 OR sf.EmailID = 0 then ps.EmailID else sf.EmailID end,
			sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN ps.Copies ELSE sf.Copies END)
		From SubscriberFinal sf With(NoLock) 
			join Subscriptions s With(NoLock) on sf.IGrp_No = s.IGrp_No
			join PubSubscriptions ps With (NoLock) on s.SubscriptionID = ps.SubscriptionID
			join Pubs p With(NoLock) on ps.PubID = p.PubID and sf.PubCode = p.PubCode
		WHERE sf.ProcessCode = @ProcessCode and sf.Ignore = 'false'


END
GO
