create procedure job_ACS_UpdateSubscriberAddress
@xml xml,
@publicationID int,
@publisherID int
as
	SET NOCOUNT ON           
	DECLARE @docHandle int
    declare @insertcount int

	DECLARE @import TABLE
	(
					AcsFileDetailId int NOT NULL,
					ClientId int NULL,
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
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
					Prefix,Suffix,OldAddressType,OldUrb,OldPrimaryNumber,OldPreDirectional,OldStreetName,OldSuffix,OldPostDirectional,OldUnitDesignator,OldSecondaryNumber,OldCity,OldStateAbbreviation,
					OldZipCode,NewAddressType,NewPmb,NewUrb,NewPrimaryNumber,NewPreDirectional,NewStreetName,NewSuffix,NewPostDirectional,NewUnitDesignator,NewSecondaryNumber,NewCity,NewStateAbbreviation,
					NewZipCode,Hyphen,NewPlus4Code,NewDeliveryPoint,NewAbbreviatedCityName,NewAddressLabel,FeeNotification,NotificationType,IntelligentMailBarcode,IntelligentMailPackageBarcode,IdTag,
					HardcopyToElectronicFlag,TypeOfAcs,FulfillmentDate,ProcessingType,CaptureType,MadeAvailableDate,ShapeOfMail,MailActionCode,NixieFlag,ProductCode1,ProductCodeFee1,ProductCode2,ProductCodeFee2,
					ProductCode3,ProductCodeFee3,ProductCode4,ProductCodeFee4,ProductCode5,ProductCodeFee5,ProductCode6,ProductCodeFee6,Filler,EndMarker,ProductCode,OldAddress1,OldAddress2,OldAddress3,
					NewAddress1,NewAddress2,NewAddress3,SequenceID,TransactionCodeValue,CategoryCodeValue,IsIgnored,ProcessCode
	)  

	SELECT 
					AcsFileDetailId,ClientId,RecordType,FileVersion,SequenceNumber,AcsMailerId,KeylineSequenceSerialNumber,MoveEffectiveDate,MoveType,DeliverabilityCode,UspsSiteID,LastName,FirstName,
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
					ClientId int 'ClientId',
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

    --Get a BatchID
	declare @batchCount int =	(
								select count(i.AcsFileDetailId)
								from @import i
								join Subscription s with(nolock) on i.SequenceID = s.SequenceID
								join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
								where sub.Address1 = i.OldAddress1
								and sub.City = i.OldCity
								and sub.RegionCode = i.OldStateAbbreviation 
								and sub.ZipCode = i.OldZipCode
							)
	declare @userID int = (select UserID from UAS..[User] where EmailAddress = 'AcsImport@TeamKM.com')
	declare @batchID int
	if(@batchCount > 0)
		begin
			insert into Batch (PublicationID,UserID,BatchCount,IsActive,DateCreated,DateFinalized)
			values(@publicationID,@userID,@batchCount,'false',getdate(),getdate());
			set @batchID = (select @@IDENTITY);
		end

	--foreach address in import where oldAddress = KmAddress update - log before/after in userlog table
	declare @sequenceID int
	declare @subscriptionID int
	declare @subscriberID int
	declare @acsFileDetailId int

	DECLARE c CURSOR
	FOR 
					select s.SequenceID,s.SubscriptionID,s.SubscriberID,i.AcsFileDetailId
					from @import i
					join Subscription s with(nolock) on i.SequenceID = s.SequenceID
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where sub.Address1 = i.OldAddress1
					and sub.City = i.OldCity
					and sub.RegionCode = i.OldStateAbbreviation 
					and sub.ZipCode = i.OldZipCode
	OPEN c
	FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId
	WHILE @@FETCH_STATUS = 0
	BEGIN
	                
					declare @historySubscriptionID int
					--create HistorySubscription record
					insert into HistorySubscription (SubscriptionID,PublisherID,SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
																									IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,SubscriptionDateCreated,
																									SubscriptionDateUpdated,SubscriptionCreatedByUserID,SubscriptionUpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
																									OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
																									Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
																									AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
																									SubscriberDateCreated,SubscriberDateUpdated,SubscriberCreatedByUserID,SubscriberUpdatedByUserID,DateCreated,CreatedByUserID,IsLocked,
																									LockDate,LockDateRelease,LockedByUserID,PhoneExt)
	                
					 select 
									SubscriptionID,PublisherID,s.SubscriberID,PublicationID,ActionID_Current,ActionID_Previous,SubscriptionStatusID,
									IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,Copies,OriginalSubscriberSourceCode,s.DateCreated,
									s.DateUpdated,s.CreatedByUserID,s.UpdatedByUserID,AccountNumber,GraceIssues,IsNewSubscription,MemberGroup,
									OnBehalfOf,Par3cID,SequenceID,SubsrcTypeID,Verify,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
									Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,
									AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,
									sub.DateCreated,sub.DateUpdated,sub.CreatedByUserID,sub.UpdatedByUserID,sub.DateCreated,sub.CreatedByUserID,IsLocked,
									LockDate,LockDateRelease,LockedByUserID,PhoneExt
					from Subscription s with(nolock)
					join Subscriber sub with(nolock) on s.SubscriberID = sub.SubscriberID
					where s.SubscriptionID = @subscriptionID;
					set @historySubscriptionID = (select @@IDENTITY);

					declare @historyId int
					Insert Into History (BatchID,BatchCountItem,PublisherID,PublicationID,SubscriberID,SubscriptionID,HistorySubscriptionID,HistoryPaidID,
																													HistoryPaidBillToID,DateCreated,CreatedByUserID)
					Values(@batchID,@batchCount,@publisherID,@publicationID,@subscriberID,@subscriptionID,@historySubscriptionID,0,0,GETDATE(),@userID);
					set @historyId = (select @@IDENTITY);
					
					declare @userLogTypeId int = (select CodeId from uas..Code with(nolock) where CodeTypeId = (select CodeTypeId from uas..CodeType with(nolock) where CodeTypeName='User Log') and CodeName='Edit')
					declare @appId int = (select ApplicationID from uas..Application with(nolock) where ApplicationName='Circulation')
					declare @userLogId int
					
					Insert Into UserLog (ApplicationID,UserLogTypeID,UserID,Object,FromObjectValues,ToObjectValues,DateCreated)
					Values(@appId,@userLogTypeId,@userID,'Subscriber','job_ACS_UpdateSubscriberAddress','',GETDATE());
					set @userLogId = (select @@IDENTITY);
					
					Insert Into HistoryToUserLog (HistoryID,UserLogID)
					Values(@historyId,@userLogId);
					
					
					--Now actually update the address
					Update Subscriber
					set Address1 = i.NewAddress1,
						Address2 = i.NewAddress2,
						Address3 = i.NewAddress3,
						City = i.NewCity,
						RegionCode = i.NewStateAbbreviation,
						RegionID = (select RegionID from UAS..Region with(nolock) where RegionCode = i.NewStateAbbreviation),
						ZipCode = i.NewZipCode,
						Plus4 = i.NewPlus4Code,
						IsAddressValidated = 'false',
						Latitude = 0,
						Longitude = 0,
						AddressValidationDate = null,
						AddressValidationMessage = '',
						DateUpdated = GETDATE(),
						UpdatedByUserID = @userID
					from @import i
					where i.AcsFileDetailId = @acsFileDetailId
					and Subscriber.SubscriberID = @subscriberID
					
					--Now update the ActionID_Current and Previous
					declare @isPaid bit = (select IsPaid from Subscription with(nolock) where SubscriptionID = @subscriptionID)
					declare @currentActionID int = (select ActionID_Current from Subscription with(nolock) where SubscriptionID = @subscriptionID)
					declare @catCodeID int = (select CategoryCodeID from Action where ActionID = @currentActionID)
					declare @newActionID int = 0
					IF(@isPaid = 'true')
						begin
							set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 2 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

							Update Subscription
							set ActionID_Previous = @currentActionID,
								ActionID_Current = @newActionID --TransactionCodeID=2 TransactionCodeValue = 21
							where SubscriptionID = @subscriptionID 
						end
					else
						begin
							set @newActionID = (select IsNull(ActionID,0) from Action with(nolock) where TransactionCodeID = 17 and CategoryCodeID = @catCodeID and ActionTypeID = 2)

							Update Subscription
							set ActionID_Previous = @currentActionID,
								ActionID_Current = @newActionID --TransactionCodeID=17 TransactionCodeValue = 21
							where SubscriptionID = @subscriptionID 
						end
						
			FETCH NEXT FROM c INTO @sequenceID,@subscriptionID,@subscriberID,@acsFileDetailId
	END
	CLOSE c
	DEALLOCATE c

go
