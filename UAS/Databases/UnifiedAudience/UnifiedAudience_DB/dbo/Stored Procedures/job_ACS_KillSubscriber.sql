create procedure [dbo].[job_ACS_KillSubscriber]
@xml xml,
@publicationID int,
@appId int, 
@userId int, 
@userLogId int,
@SourceFileID int				
as
BEGIN
	
	SET NOCOUNT ON
		           
	DECLARE @docHandle int
    declare @insertcount int

	DECLARE @import TABLE
	(
		AcsFileDetailId int NOT NULL,
		RecordType varchar(1) NULL,
		FileVersion varchar(2) NULL,
		SequenceNumber int NULL,
		AcsMailerId varchar(9) NULL,
		KeylineSequenceSerialNumber varchar(16) NULL,
		MoveEffectiveDate date NULL,
		MoveType varchar(1) NULL,
		DeliverabilityCode varchar(1) NULL,
		UspsSiteID int NULL,
		LastName varchar(20) NULL,
		FirstName varchar(15) NULL,
		Prefix varchar(6) NULL,
		Suffix varchar(6) NULL,
		OldAddressType varchar(1) NULL,
		OldUrb varchar(28) NULL,
		OldPrimaryNumber varchar(10) NULL,
		OldPreDirectional varchar(2) NULL,
		OldStreetName varchar(28) NULL,
		OldSuffix varchar(4) NULL,
		OldPostDirectional varchar(2) NULL,
		OldUnitDesignator varchar(4) NULL,
		OldSecondaryNumber varchar(10) NULL,
		OldCity varchar(28) NULL,
		OldStateAbbreviation varchar(2) NULL,
		OldZipCode varchar(5) NULL,
		NewAddressType varchar(1) NULL,
		NewPmb varchar(8) NULL,
		NewUrb varchar(28) NULL,
		NewPrimaryNumber varchar(10) NULL,
		NewPreDirectional varchar(2) NULL,
		NewStreetName varchar(28) NULL,
		NewSuffix varchar(4) NULL,
		NewPostDirectional varchar(2) NULL,
		NewUnitDesignator varchar(4) NULL,
		NewSecondaryNumber varchar(10) NULL,
		NewCity varchar(28) NULL,
		NewStateAbbreviation varchar(2) NULL,
		NewZipCode varchar(5) NULL,
		Hyphen varchar(1) NULL,
		NewPlus4Code varchar(4) NULL,
		NewDeliveryPoint varchar(2) NULL,
		NewAbbreviatedCityName varchar(13) NULL,
		NewAddressLabel varchar(66) NULL,
		FeeNotification varchar(1) NULL,
		NotificationType varchar(1) NULL,
		IntelligentMailBarcode varchar(31) NULL,
		IntelligentMailPackageBarcode varchar(35) NULL,
		IdTag varchar(16) NULL,
		HardcopyToElectronicFlag varchar(1) NULL,
		TypeOfAcs varchar(1) NULL,
		FulfillmentDate date NULL,
		ProcessingType varchar(1) NULL,
		CaptureType varchar(1) NULL,
		MadeAvailableDate date NULL,
		ShapeOfMail varchar(1) NULL,
		MailActionCode varchar(1) NULL,
		NixieFlag varchar(1) NULL,
		ProductCode1 int NULL,
		ProductCodeFee1 decimal(4, 2) NULL,
		ProductCode2 int NULL,
		ProductCodeFee2 decimal(4, 2) NULL,
		ProductCode3 int NULL,
		ProductCodeFee3 decimal(4, 2) NULL,
		ProductCode4 int NULL,
		ProductCodeFee4 decimal(4, 2) NULL,
		ProductCode5 int NULL,
		ProductCodeFee5 decimal(4, 2) NULL,
		ProductCode6 int NULL,
		ProductCodeFee6 decimal(4, 2) NULL,
		Filler varchar(405) NULL,
		EndMarker varchar(1) NULL,
		ProductCode varchar(50) NULL,
		OldAddress1 varchar(100) NULL,
		OldAddress2 varchar(100) NULL,
		OldAddress3 varchar(100) NULL,
		NewAddress1 varchar(100) NULL,
		NewAddress2 varchar(100) NULL,
		NewAddress3 varchar(100) NULL,
		SequenceID int NULL,
		TransactionCodeValue int NULL,
		CategoryCodeValue int NULL,
		IsIgnored bit NULL,
		ProcessCode varchar(50) NULL
	)

	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml

	insert into @import 
	(
		AcsFileDetailId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
		Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
		OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
		NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
		HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
		ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
		NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	)  

	SELECT 
		AcsFileDetailId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
		Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
		OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
		NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
		HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
		ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
		NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	FROM OPENXML(@docHandle, N'/XML/AcsFileDetail') 
	WITH   
	(  
		AcsFileDetailId int 'AcsFileDetailId',
		RecordType varchar(1) 'RecordType',
		FileVersion varchar(2) 'FileVersion',
		SequenceNumber int 'SequenceNumber',
		AcsMailerId varchar(9) 'AcsMailerId',
		KeylineSequenceSerialNumber varchar(16) 'KeylineSequenceSerialNumber',
		MoveEffectiveDate date 'MoveEffectiveDate',
		MoveType varchar(1) 'MoveType',
		DeliverabilityCode varchar(1) 'DeliverabilityCode',
		UspsSiteID int 'UspsSiteID',
		LastName varchar(20) 'LastName',
		FirstName varchar(15) 'FirstName',
		Prefix varchar(6) 'Prefix',
		Suffix varchar(6) 'Suffix',
		OldAddressType varchar(1) 'OldAddressType',
		OldUrb varchar(28) 'OldUrb',
		OldPrimaryNumber varchar(10) 'OldPrimaryNumber',
		OldPreDirectional varchar(2) 'OldPreDirectional',
		OldStreetName varchar(28) 'OldStreetName',
		OldSuffix varchar(4) 'OldSuffix',
		OldPostDirectional varchar(2) 'OldPostDirectional',
		OldUnitDesignator varchar(4) 'OldUnitDesignator',
		OldSecondaryNumber varchar(10) 'OldSecondaryNumber',
		OldCity varchar(28) 'OldCity',
		OldStateAbbreviation varchar(2) 'OldStateAbbreviation',
		OldZipCode varchar(5) 'OldZipCode',
		NewAddressType varchar(1) 'NewAddressType',
		NewPmb varchar(8) 'NewPmb',
		NewUrb varchar(28) 'NewUrb',
		NewPrimaryNumber varchar(10) 'NewPrimaryNumber',
		NewPreDirectional varchar(2) 'NewPreDirectional',
		NewStreetName varchar(28) 'NewStreetName',
		NewSuffix varchar(4) 'NewSuffix',
		NewPostDirectional varchar(2) 'NewPostDirectional',
		NewUnitDesignator varchar(4) 'NewUnitDesignator',
		NewSecondaryNumber varchar(10) 'NewSecondaryNumber',
		NewCity varchar(28) 'NewCity',
		NewStateAbbreviation varchar(2) 'NewStateAbbreviation',
		NewZipCode varchar(5) 'NewZipCode',
		Hyphen varchar(1) 'Hyphen',
		NewPlus4Code varchar(4) 'NewPlus4Code',
		NewDeliveryPoint varchar(2) 'NewDeliveryPoint',
		NewAbbreviatedCityName varchar(13) 'NewAbbreviatedCityName',
		NewAddressLabel varchar(66) 'NewAddressLabel',
		FeeNotification varchar(1) 'FeeNotification',
		NotificationType varchar(1) 'NotificationType',
		IntelligentMailBarcode varchar(31) 'IntelligentMailBarcode',
		IntelligentMailPackageBarcode varchar(35) 'IntelligentMailPackageBarcode',
		IdTag varchar(16) 'IdTag',
		HardcopyToElectronicFlag varchar(1) 'HardcopyToElectronicFlag',
		TypeOfAcs varchar(1) 'TypeOfAcs',
		FulfillmentDate date 'FulfillmentDate',
		ProcessingType varchar(1) 'ProcessingType',
		CaptureType varchar(1) 'CaptureType',
		MadeAvailableDate date 'MadeAvailableDate',
		ShapeOfMail varchar(1) 'ShapeOfMail',
		MailActionCode varchar(1) 'MailActionCode',
		NixieFlag varchar(1) 'NixieFlag',
		ProductCode1 int 'ProductCode1',
		ProductCodeFee1 decimal(4, 2) 'ProductCodeFee1',
		ProductCode2 int 'ProductCode2',
		ProductCodeFee2 decimal(4, 2) 'ProductCodeFee2',
		ProductCode3 int 'ProductCode3',
		ProductCodeFee3 decimal(4, 2) 'ProductCodeFee3',
		ProductCode4 int 'ProductCode4',
		ProductCodeFee4 decimal(4, 2) 'ProductCodeFee4',
		ProductCode5 int 'ProductCode5',
		ProductCodeFee5 decimal(4, 2) 'ProductCodeFee5',
		ProductCode6 int 'ProductCode6',
		ProductCodeFee6 decimal(4, 2) 'ProductCodeFee6',
		Filler varchar(405) 'Filler',
		EndMarker varchar(1) 'EndMarker',
		ProductCode varchar(50) 'ProductCode',
		OldAddress1 varchar(100) 'OldAddress1',
		OldAddress2 varchar(100) 'OldAddress2',
		OldAddress3 varchar(100) 'OldAddress3',
		NewAddress1 varchar(100) 'NewAddress1',
		NewAddress2 varchar(100) 'NewAddress2',
		NewAddress3 varchar(100) 'NewAddress3',
		SequenceID int 'SequenceID',
		TransactionCodeValue int 'TransactionCodeValue',
		CategoryCodeValue int 'CategoryCodeValue',
		IsIgnored bit 'IsIgnored',
		ProcessCode varchar(50) 'ProcessCode'
	)  

	EXEC sp_xml_removedocument @docHandle


	DECLARE @FreeActionTranCodeTypeID int = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Active')
	DECLARE @FreeInactiveActionTranCodeTypeID int = (Select TransactionCodeTypeID from UAD_Lookup..TransactionCodeType where TransactionCodeTypeName = 'Free Inactive')
	DECLARE @ActionTypeID int = (select CodeId from UAD_Lookup..Code with(nolock) where CodeName='System Generated' and CodeTypeId = 
									(select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName='Action'))
	DECLARE @TransID34 int = (Select TransactionCodeID from UAD_Lookup..TransactionCode with(Nolock) where TransactionCodeValue = 34)										
	--DECLARE @PaidActiveTransID int = (select TransactionCodeID from UAD_Lookup..TransactionCode with(nolock) where TransactionCodeValue = '61')
	DECLARE @FreeActiveTransID int = (select TransactionCodeID from UAD_Lookup..TransactionCode with(nolock) where TransactionCodeValue = '31')


	/* Paid Active update Cat/Tran value in import to be correct IDs */
	--Update i
	--	set i.TransactionCodeValue = @PaidActiveTransID,
	--		i.CategoryCodeValue = (select a.CategoryCodeID from UAD_Lookup..Action a with(nolock) 
	--							join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
	--							join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
	--							where a.IsActive = 'true' and tc.TransactionCodeID = @PaidActiveTransID
	--							and cc.CategoryCodeID = ps.PubCategoryID and ActionTypeID = @ActionTypeID)	
	--from @import i
	--join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
	--join Pubs p With(NoLock) on i.ProductCode = p.PubCode and ps.PubID = p.PubID	
	--where ps.IsPaid = 'true' and ps.PubTransactionID != @TransID34
	
			
	/* Free Active update Cat/Tran value in import to be correct IDs */
	Update i
		set i.TransactionCodeValue = (select TransactionCodeID from UAD_Lookup..TransactionCode with(nolock) where TransactionCodeValue = i.TransactionCodeValue),
			i.CategoryCodeValue = (select a.CategoryCodeID from UAD_Lookup..Action a with(nolock) 
								join UAD_Lookup..CategoryCode cc with(nolock) on a.CategoryCodeID = cc.CategoryCodeID
								join UAD_Lookup..TransactionCode tc with(nolock) on a.TransactionCodeID = tc.TransactionCodeID
								where a.IsActive = 'true' and tc.TransactionCodeID = (select TransactionCodeID from UAD_Lookup..TransactionCode with(nolock) where TransactionCodeValue = i.TransactionCodeValue)
								and cc.CategoryCodeID = ps.PubCategoryID and ActionTypeID = @ActionTypeID)	
	from @import i
		join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
		join Pubs p With(NoLock) on i.ProductCode = p.PubCode and ps.PubID = p.PubID	
	where 
		--ps.IsPaid = 'false' and 
		ps.PubTransactionID != @TransID34
		and ps.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = ps.PubCategoryID and tc.TransactionCodeID = ps.PubTransactionID
									   and tc.TransactionCodeTypeID in (@FreeActionTranCodeTypeID, @FreeInactiveActionTranCodeTypeID)
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID)
	
	
	/* Insert ACS Record into Subscriber Final for next process to finish */
	Insert into SubscriberFinal (STRecordIdentifier,SourceFileID,PubCode,Sequence,
				AccountNumber,ADDRESS,MailStop,Address3,CITY,COMPANY,Copies,COUNTRY,CountryID,COUNTY,DateUpdated,Demo7,Email,EmailID,EmailRenewPermission,EmailStatusID,
				ExternalKeyId,FAX,FaxPermission,FNAME,FORZIP,Gender,GraceIssues,Igrp_No,IGrp_Rank,isActive,IsComp,IsPaid,IsSubscribed,LNAME,Latitude,Longitude,MailPermission,MOBILE,
				Occupation,OtherProductsPermission,PAR3C,PHONE,PhonePermission,Plus4,CategoryID,QSourceID,TransactionDate,TransactionID,QDate,STATE,SubSrc,SubscriptionStatusID,
				SubSrcID,TextPermission,ThirdPartyPermission,TITLE,Verified,Website,ZIP,
				Ignore,IsDQMProcessFinished,IsUpdatedInLive,IsLatLonValid,DateCreated,SFRecordIdentifier,ProcessCode)
	Select NEWID(),@SourceFileID,p.PubCode,i.SequenceID,
				ps.AccountNumber,ps.Address1,ps.Address2,ps.Address3,ps.City,ps.Company,ps.Copies,ps.Country,ps.CountryID,ps.County,ps.DateUpdated,ps.demo7,ps.Email,ps.EmailID,ps.EmailRenewPermission,ps.EmailStatusID,
				ps.ExternalKeyId,ps.Fax,ps.FaxPermission,ps.FirstName,s.FORZIP,ps.Gender,ps.GraceIssues,s.IGrp_No,'M',ps.isActive,ps.IsComp,ps.IsPaid,ps.IsSubscribed,ps.LastName,ps.Latitude,ps.Longitude,ps.MailPermission,ps.Mobile,
				ps.Occupation,ps.OtherProductsPermission,ps.Par3cID,ps.Phone,ps.PhonePermission,ps.Plus4,i.CategoryCodeValue,ps.PubQSourceID,ps.PubTransactionDate,i.TransactionCodeValue,ps.Qualificationdate,ps.RegionCode,ps.SubscriberSourceCode,ps.SubscriptionStatusID,
				ps.SubSrcID,ps.TextPermission,ps.ThirdPartyPermission,ps.Title,ps.Verify,ps.Website,ps.ZipCode,			 
				0,0,0,0,GETDATE(),NEWID(),i.ProcessCode
	from @import i
		join PubSubscriptions ps with(nolock) on i.SequenceID = ps.SequenceID
		join Subscriptions s with(nolock) on ps.SubscriptionID = s.SubscriptionID
		join Pubs p With(NoLock) on i.ProductCode = p.PubCode and ps.PubID = p.PubID	
	where 
		--ps.IsPaid = 'false' and 
		ps.PubTransactionID != @TransID34
		and ps.PubTransactionID in (Select tc.TransactionCodeID
								   From UAD_Lookup..Action a
									   join UAD_Lookup..CategoryCode cc on a.CategoryCodeID = cc.CategoryCodeID
									   join UAD_Lookup..TransactionCode tc on a.TransactionCodeID = tc.TransactionCodeID
								   where cc.CategoryCodeID = ps.PubCategoryID and tc.TransactionCodeID = ps.PubTransactionID
									   and tc.TransactionCodeTypeID in (@FreeActionTranCodeTypeID, @FreeInactiveActionTranCodeTypeID)
									   and a.IsActive = 'true'
									   and a.ActionTypeID = @ActionTypeID)

END
GO