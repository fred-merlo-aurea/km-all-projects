create procedure job_NCOA_AddressUpdate
@xml xml,
@userId int,
@SourceFileID int
as
BEGIN

	SET NOCOUNT ON
          
	DECLARE @docHandle int
    declare @insertcount int

	CREATE TABLE #NCOAimport 
	(
		SequenceID int NOT NULL,
		Address1 varchar(100) null,
		Address2 varchar(100) null,
		City varchar(50) null,
		RegionCode varchar(50) null,
		ZipCode varchar(50) null,
		Plus4 varchar(10) null,
		ProductCode varchar(50),
		ProcessCode varchar(50) not null
	)
	CREATE NONCLUSTERED INDEX [IDX_SequenceID] ON #NCOAimport (SequenceID ASC)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into #NCOAimport 
	(
		SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,ProductCode,ProcessCode 
	)  

	SELECT SequenceID,Address1,Address2,City,RegionCode,ZipCode,Plus4,ProductCode,ProcessCode 
	FROM OPENXML(@docHandle, N'/XML/NCOA') 
	WITH   
	(  
		SequenceID int 'SequenceID',
		Address1 varchar(100) 'Address1',
		Address2 varchar(100) 'Address2',
		City varchar(50) 'City',
		RegionCode varchar(50) 'RegionCode',
		ZipCode varchar(50) 'ZipCode',
		Plus4 varchar(10) 'Plus4',
		ProductCode varchar(50) 'ProductCode',
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle

	update #NCOAimport set Address1 = UAD_Lookup.dbo.RevertXmlFormatting(Address1);
	update #NCOAimport set Address2 = UAD_Lookup.dbo.RevertXmlFormatting(Address2);
	update #NCOAimport set City = UAD_Lookup.dbo.RevertXmlFormatting(City);
	update #NCOAimport set RegionCode = UAD_Lookup.dbo.RevertXmlFormatting(RegionCode);
	update #NCOAimport set ZipCode = UAD_Lookup.dbo.RevertXmlFormatting(ZipCode);
	update #NCOAimport set Plus4 = UAD_Lookup.dbo.RevertXmlFormatting(Plus4);
	update #NCOAimport set ProductCode = UAD_Lookup.dbo.RevertXmlFormatting(ProductCode);
	update #NCOAimport set ProcessCode = UAD_Lookup.dbo.RevertXmlFormatting(ProcessCode);

    delete #NCOAimport where SequenceID not in (Select SequenceID from PubSubscriptions with(nolock))	


	DECLARE @FreeActionTranCodeTypeID int = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Active')
	DECLARE @ActionTypeID int = (select CodeId from UAD_Lookup..Code with(nolock) where CodeName='System Generated' and CodeTypeId = 
									(select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName='Action'))
	DECLARE @NewFreeTransactionCodeID int = (Select TransactionCodeID from UAD_Lookup..TransactionCode where TransactionCodeValue = 21 and TransactionCodeTypeID = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Active'))
	

	/* Insert ACS Record into Subscriber Final for next process to finish */
	Insert into SubscriberFinal (STRecordIdentifier,SourceFileID,PubCode,Sequence,Address,MailStop,Address3,City,State,Zip,Plus4,CategoryID,TransactionID,
								Ignore,IsDQMProcessFinished,IsUpdatedInLive,Latitude,Longitude,IsLatLonValid,DateCreated,SFRecordIdentifier,ProcessCode)
	Select NEWID(),@SourceFileID,p.PubCode,i.SequenceID,i.Address1,i.Address2,'',i.City,i.RegionCode,i.ZipCode,i.Plus4,ps.PubCategoryID,
			@NewFreeTransactionCodeID,0,0,0,0,0,0,GETDATE(),NEWID(),i.ProcessCode
	from #NCOAimport i
		join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
		join Pubs p With(NoLock) on i.ProductCode = p.PubCode and ps.PubID = p.PubID		
	where ps.IsPaid = 'false'
		and ps.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = ps.PubCategoryID and tc.TransactionCodeID = ps.PubTransactionID
									   and tc.TransactionCodeTypeID = @FreeActionTranCodeTypeID
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID)		
			
	DECLARE @ProcessCode varchar(50) = (Select distinct ProcessCode from #NCOAimport)
	--DECLARE @MM31 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO31')
	--DECLARE @MM32 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO32')
	--DECLARE @MM33 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO33')
	--DECLARE @MM34 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO34')
	--DECLARE @MM35 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO35')
	--DECLARE @MM36 int = (Select codeID from UAD_Lookup..Code where CodeTypeId= 48 and CodeDescription = 'DEMO36')

	/* Maintain Values where Blank */
	Update sf
		Set
			--sf.[SEQUENCE]  = convert(int,isnull(sf.[SEQUENCE], 0)),
			sf.FNAME       = ps.FIRSTNAME,
			sf.LNAME       = ps.LastName,
			sf.TITLE       = ps.TITLE,
			sf.COMPANY     = ps.COMPANY,
			--sf.ADDRESS     = (CASE WHEN	ISNULL(sf.Address,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ADDRESS, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS END),
			--sf.MAILSTOP    = (CASE WHEN ISNULL(sf.MailStop,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.MAILSTOP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.MAILSTOP END),
			--sf.ADDRESS3    = (CASE WHEN ISNULL(sf.Address3,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ADDRESS3 END),               
			--sf.CITY        = (CASE WHEN ISNULL(sf.City,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.CITY, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.CITY END),
			--sf.STATE       = (CASE WHEN ISNULL(sf.State,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.STATE, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.STATE END),
			--sf.ZIP         = (CASE WHEN ISNULL(sf.Zip,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.ZIP, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.ZIP END),
			--sf.PLUS4       = (CASE WHEN ISNULL(sf.Plus4,'')!='' THEN REPLACE(REPLACE(REPLACE(sf.PLUS4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE s.PLUS4 END),
			sf.FORZIP      = s.FORZIP,
			sf.COUNTY      = ps.COUNTY,
			sf.COUNTRY     = ps.COUNTRY,
			sf.CountryID   = ps.CountryID,
			sf.PHONE       = ps.PHONE,
			--sf.PhoneExists = s.PHONEEXISTS,
			sf.FAX         = ps.FAX,
			--sf.Faxexists   = s.FAXEXISTS, 
			sf.Email       = ps.EMAIL,
			--sf.emailexists = s.EMAILEXISTS, 
			--sf.CategoryID  = sf.CategoryID,
			--sf.TransactionID = sf.TransactionID,
			sf.TransactionDate = ps.PubTransactionDate,
			sf.QDate       = ps.QualificationDate,
			sf.QSourceID   = ps.PubQSourceID,
			sf.RegCode	   = s.REGCODE,
			sf.Verified	   = ps.Verify,
			sf.SubSrc	   = ps.SubscriberSourceCode,
			sf.OrigsSrc    = ps.OrigsSrc,
			sf.PAR3C       = ps.PAR3CID,
			sf.MailPermission			= ps.MailPermission,
			sf.FaxPermission			= ps.FaxPermission,
			sf.PhonePermission			= ps.PhonePermission,
			sf.OtherProductsPermission  = ps.OtherProductsPermission,
			sf.ThirdPartyPermission     = ps.ThirdPartyPermission,
			sf.EmailRenewPermission     = ps.EmailRenewPermission,
			sf.TextPermission			= ps.TextPermission,
			sf.[Source]	   = s.[Source],
			sf.[Priority]  = s.[Priority],
			sf.IGRP_CNT    = s.IGRP_CNT,
			sf.CGrp_No	   = s.CGrp_No,
			sf.CGrp_Cnt	   = s.CGrp_Cnt,
			sf.StatList	   = s.StatList,
			sf.Sic		   = s.Sic,
			sf.SicCode	   = s.SicCode,
			sf.Gender      = s.Gender,
			sf.IGrp_Rank   = s.IGrp_Rank,
			sf.CGrp_Rank   = s.CGrp_Rank,
			sf.Home_Work_Address = s.Home_Work_Address,
			--sf.PubIDs	   = s.PubIDs,
			sf.Demo7       = ps.Demo7,
			--sf.IsExcluded  = s.IsExcluded,
			sf.MOBILE      = ps.mobile,		
			--sf.Score	   = s.Score,																																							
			sf.DateUpdated = GETDATE(),
			sf.IsMailable  = s.IsMailable,
			sf.Copies = (CASE WHEN ISNULL(sf.Copies,0) < 1 THEN ps.Copies ELSE sf.Copies END)
		From SubscriberFinal sf With(NoLock)                                        
				join PubSubscriptions ps With(NoLock) on sf.Sequence = ps.SequenceID
				join Subscriptions s With(NoLock) on ps.SubscriptionID = s.SubscriptionID
				join Pubs p With(NoLock) on ps.PubID = p.PubID and sf.PubCode = p.PubCode
		WHERE sf.Ignore = 'false' and sf.ProcessCode = @ProcessCode

	DROP TABLE #NCOAimport

END
GO