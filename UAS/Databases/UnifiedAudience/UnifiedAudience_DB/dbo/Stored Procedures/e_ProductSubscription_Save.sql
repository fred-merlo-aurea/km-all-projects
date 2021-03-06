﻿CREATE procedure [dbo].[e_ProductSubscription_Save]
@PubSubscriptionID int,
@SubscriptionID	int,
@PubID int,
@Demo7 varchar(1),
@QualificationDate date,
@PubQSourceID int,
@PubCategoryID int,
@PubTransactionID int,
@EmailStatusID int,
@StatusUpdatedDate datetime,
@StatusUpdatedReason varchar(200),
@Email varchar(100),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
@IsComp bit = 'false',
@SubscriptionStatusID int = 1,
@AddRemoveID int = 0,
@Copies int = 1,
@GraceIssues int = 0,
@IMBSEQ varchar(256) = '',
@IsActive bit = 'true',
@IsPaid bit = 'false',
@IsSubscribed bit = 'false',
@MemberGroup varchar(256) = '',
@OnBehalfOf varchar(256) = '',
@OrigsSrc varchar(256) = '',
@Par3CID  int = 0,
@SequenceID int = 0, 
@Status  varchar(256) = '',
@SubscriberSourceCode  varchar(256) = '',
@SubSrcID int = 0,
@Verified varchar(100) = '',
@ExternalKeyID int = 0,
@FirstName varchar(50) = '',
@LastName varchar(50) = '',
@Company varchar(100) = '',
@Title varchar(50) = '',
@Occupation varchar(50) = '',
@AddressTypeID int = 0,
@Address1 varchar(100) = '',
@Address2 varchar(100) = '',
@Address3 varchar(100) = '',
@City varchar(50) = '',
@RegionCode varchar(50) = 0,
@RegionID int = 0,
@ZipCode varchar(10) = '',
@Plus4 varchar(10) = '',
@CarrierRoute varchar(10) = '',
@County varchar(50) = '',
@Country varchar(50) = '',
@CountryID int = 0,
@Latitude decimal(18,15) = 0,
@Longitude decimal(18,15) = 0,
@IsAddressValidated bit = 0,
@AddressValidationDate datetime = '',
@AddressValidationSource varchar(50) = '',
@AddressValidationMessage varchar(max) = '',
@Phone varchar(50) = '',
@Fax varchar(50) = '',
@Mobile varchar(50) = '',
@Website varchar(100) = '',
@Birthdate date = '',
@Age int = 0,
@Income varchar(50) = '',
@Gender varchar(50) = '',
@PhoneExt varchar(25) = '',
@IsInActiveWaveMailing bit = 0,
@AddressTypeCodeId int = 0,
@AddressLastUpdatedDate datetime = '',
@AddressUpdatedSourceTypeCodeId int = 0,
@WaveMailingID int = 0,
@IGrp_No uniqueidentifier = '',
@SFRecordIdentifier uniqueidentifier = '',
@ReqFlag int = 0,
@EmailID int = 0,
@MailPermission bit,
@FaxPermission bit,
@PhonePermission bit,
@OtherProductsPermission bit,
@EmailRenewPermission bit,
@ThirdPartyPermission bit,
@TextPermission bit,
@SubGenSubscriberID int = 0,
@SubGenSubscriptionID int = 0,
@SubGenPublicationID int = 0,
@SubGenMailingAddressId int = 0,
@SubGenBillingAddressId int = 0,
@IssuesLeft int = 0, 
@UnearnedReveue money = 0,
@SubGenIsLead bit = 0,
@SubGenRenewalCode varchar(50) = '',
@SubGenSubscriptionRenewDate date = null,
@SubGenSubscriptionExpireDate date = null,
@SubGenSubscriptionLastQualifiedDate date = null
as
BEGIN

	set nocount on

	IF @PubSubscriptionID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE PubSubscriptions
			SET 
				[PubID] = @PubID,
				[Demo7] = @Demo7,
				[QualificationDate] = isnull(@QualificationDate,[QualificationDate]),
				[PubQSourceID] = @PubQSourceID,
				[PubCategoryID] = @PubCategoryID,
				[PubTransactionID] = @PubTransactionID,
				[EmailStatusID] = @EmailStatusID,
				[StatusUpdatedDate] = @StatusUpdatedDate,
				[StatusUpdatedReason] = @StatusUpdatedReason,
				[Email] = @Email,
				[DateCreated] = @DateCreated,
				[DateUpdated] = @DateUpdated,
				[CreatedByUserID] = @CreatedByUserID,
				[UpdatedByUserID] = @UpdatedByUserID,
				[IsComp] = @IsComp,
				[SubscriptionStatusID] = @SubscriptionStatusID,
				[AddRemoveID] = @AddRemoveID,
				[Copies] = @Copies,
				[GraceIssues] = @GraceIssues,
				[IMBSEQ] = @IMBSEQ,
				[IsActive]	= @IsActive,
				[IsPaid] = @IsPaid,
				[IsSubscribed] = @IsSubscribed,
				[MemberGroup] = @MemberGroup,
				[OnBehalfOf] = @OnBehalfOf,
				[OrigsSrc] = @OrigsSrc,
				[Par3CID]	= @Par3CID,
				[SequenceID] = @SequenceID, 
				[Status] = @Status,
				[SubscriberSourceCode] = @SubscriberSourceCode,
				[SubSrcID] = @SubSrcID,
				[Verify] = @Verified,
				ExternalKeyID = @ExternalKeyID,
				FirstName = @FirstName,
				LastName = @LastName,
				Company = @Company,
				Title = @Title,
				Occupation = @Occupation,
				AddressTypeID = @AddressTypeID,
				Address1 = @Address1,
				Address2 = @Address2,
				Address3 = @Address3,
				City = @City,
				RegionCode = @RegionCode,
				RegionID = @RegionID,
				ZipCode = @ZipCode,
				Plus4 = @Plus4,
				CarrierRoute = @CarrierRoute,
				County = @County,
				Country = @Country,
				CountryID = @CountryID,
				Latitude = @Latitude,
				Longitude = @Longitude,
				IsAddressValidated = @IsAddressValidated,
				AddressValidationDate = @AddressValidationDate,
				AddressValidationSource = @AddressValidationSource,
				AddressValidationMessage = @AddressValidationMessage,
				Phone = @Phone,
				Fax = @Fax,
				Mobile = @Mobile,
				Website = @Website,
				Birthdate = @Birthdate,
				Age = @Age,
				Income = @Income,
				Gender = @Gender, 
				PhoneExt = @PhoneExt,
				IsInActiveWaveMailing = @IsInActiveWaveMailing,
				AddressTypeCodeId = @AddressTypeCodeId,
				AddressLastUpdatedDate = @AddressLastUpdatedDate,
				AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId,
				WaveMailingID = @WaveMailingID,
				IGrp_No = @IGrp_No,
				SFRecordIdentifier = @SFRecordIdentifier,
				ReqFlag = @ReqFlag,
				EmailID = @EmailID,
				MailPermission = @MailPermission,
				FaxPermission = @FaxPermission,
				PhonePermission = @PhonePermission,
				OtherProductsPermission = @OtherProductsPermission,
				EmailRenewPermission = @EmailRenewPermission,
				ThirdPartyPermission = @ThirdPartyPermission,
				TextPermission = @TextPermission,
				SubGenSubscriberID = @SubGenSubscriberID,
				SubGenSubscriptionID = @SubGenSubscriptionID,
				SubGenPublicationID = @SubGenPublicationID,
				SubGenMailingAddressId = @SubGenMailingAddressId,
				SubGenBillingAddressId = @SubGenBillingAddressId,
				IssuesLeft = @IssuesLeft,
				UnearnedReveue = @UnearnedReveue,
				SubGenIsLead = @SubGenIsLead,
				SubGenRenewalCode = @SubGenRenewalCode,
				SubGenSubscriptionRenewDate = @SubGenSubscriptionRenewDate,
				SubGenSubscriptionExpireDate = @SubGenSubscriptionExpireDate,
				SubGenSubscriptionLastQualifiedDate = @SubGenSubscriptionLastQualifiedDate
			WHERE PubSubscriptionID = @PubSubscriptionID;
			
			UPDATE Subscriptions
			SET Sequence = convert(int,isnull(@SequenceID, 0)),
				FName = (CASE WHEN ISNULL(@FirstName,'')!='' AND ISNULL(@LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(@FirstName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE FNAME END),
				LName = (CASE WHEN ISNULL(@FirstName,'')!='' AND ISNULL(@LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(@LastName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE LNAME END),
				Title = (CASE WHEN ISNULL(@Title,'')!='' THEN REPLACE(REPLACE(REPLACE(@Title, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE TITLE END),
				Company = (CASE WHEN ISNULL(@Company,'')!='' THEN REPLACE(REPLACE(REPLACE(@Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COMPANY END),
				Address = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@Address1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ADDRESS END),
				MailStop = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@Address2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE MAILSTOP END),
				City = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE CITY END),
				State = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@RegionCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE STATE END),
				Zip = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@ZipCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ZIP END),
				Plus4 = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
                                  THEN REPLACE(REPLACE(REPLACE(@Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE PLUS4 END),
				County = (CASE WHEN ISNULL(@County,'')!='' THEN REPLACE(REPLACE(REPLACE(@County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COUNTY END),
				Country = (CASE WHEN ISNULL(@Country,'')!='' THEN REPLACE(REPLACE(REPLACE(@Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COUNTRY END),
				CountryID = (CASE WHEN @CountryID IS NOT NULL THEN @CountryID ELSE CountryID END),
				Phone = (CASE WHEN ISNULL(@Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(@Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE PHONE END),
				Fax = (CASE WHEN ISNULL(@Fax,'')!='' THEN REPLACE(REPLACE(REPLACE(@Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE FAX END),
				mobile = (CASE WHEN ISNULL(@Mobile,'')!='' THEN REPLACE(REPLACE(REPLACE(@Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE mobile END),
				Email = (CASE WHEN ISNULL(@Email,'')!='' THEN @Email ELSE EMAIL END),
				emailexists = (case when ltrim(rtrim(isnull(@Email,''))) <> '' then 1 else 0 end), 
				Faxexists   = (case when ltrim(rtrim(isnull(@Fax,''))) <> '' then 1 else 0 end), 
				PhoneExists = (case when ltrim(rtrim(isnull(@Phone,''))) <> '' then 1 else 0 end),
				CategoryID  = @PubCategoryID,
				TransactionID = @PubTransactionID,
				QDate       =  @QualificationDate,
				SubSrc = @SubSrcID,
			    OrigsSrc = @OrigsSrc,
				Par3C = @Par3CID,
				Gender = (CASE WHEN ISNULL(@Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(@Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE Gender END),
				ADDRESS3 = (CASE WHEN ISNULL(@Address3,'')!='' THEN REPLACE(REPLACE(REPLACE(@Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ADDRESS3 END),
				Demo7 = @Demo7,
				AddressTypeCodeId = (CASE WHEN LEN(@AddressTypeCodeId) > 0 THEN @AddressTypeCodeId else AddressTypeCodeId end),
				AddressLastUpdatedDate = (CASE WHEN LEN(@AddressLastUpdatedDate) > 0 THEN @AddressLastUpdatedDate else AddressLastUpdatedDate end),
				AddressUpdatedSourceTypeCodeId = (CASE WHEN LEN(@AddressUpdatedSourceTypeCodeId) > 0 THEN @AddressUpdatedSourceTypeCodeId else AddressUpdatedSourceTypeCodeId end),
				IsActive = (CASE WHEN LEN(@IsActive) > 0 THEN @IsActive else IsActive end),
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID,
				MailPermission = (CASE WHEN ISNULL(MailPermission, '') = '' AND ISNULL(@MailPermission, '') != '' THEN @MailPermission ELSE MailPermission END),
				FaxPermission = (CASE WHEN ISNULL(FaxPermission, '') = '' AND ISNULL(@FaxPermission, '') != '' THEN @MailPermission ELSE FaxPermission END),
				PhonePermission = (CASE WHEN ISNULL(PhonePermission, '') = '' AND ISNULL(@PhonePermission, '') != '' THEN @PhonePermission ELSE PhonePermission END),
				OtherProductsPermission = (CASE WHEN ISNULL(OtherProductsPermission, '') = '' AND ISNULL(@OtherProductsPermission, '') != '' THEN @OtherProductsPermission ELSE OtherProductsPermission END),
				EmailRenewPermission = (CASE WHEN ISNULL(EmailRenewPermission, '') = '' AND ISNULL(@EmailRenewPermission, '') != '' THEN @EmailRenewPermission ELSE EmailRenewPermission END),
				ThirdPartyPermission = (CASE WHEN ISNULL(ThirdPartyPermission, '') = '' AND ISNULL(@ThirdPartyPermission, '') != '' THEN @ThirdPartyPermission ELSE ThirdPartyPermission END),
				TextPermission = (CASE WHEN ISNULL(TextPermission, '') = '' AND ISNULL(@TextPermission, '') != '' THEN @TextPermission ELSE TextPermission END)
			WHERE SubscriptionID = @SubscriptionID

			SELECT @PubSubscriptionID;
		END
	ELSE
		BEGIN
		SET @SequenceID = ISNULL((SELECT MAX(SequenceID) FROM PubSubscriptions with(nolock) WHERE PubID = @PubID), 0)
		SET @SequenceID = @SequenceID + 1
		SET @IGrp_No = NEWID()
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			if(@SubscriptionID = 0)
				begin
					INSERT INTO Subscriptions (Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,County,Country,CountryID,Phone,Fax,
									   Email,emailexists,Faxexists,PhoneExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,SubSrc,
									   Gender,Address3,
									   Demo7,Mobile,Latitude,Longitude,IsLatLonValid,LatLonMsg,IGrp_No,DateCreated,CreatedByUserID,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,IsActive,
									   MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission)
					VALUES(@SequenceID,@FirstName,@LastName,@Title,@Company,@Address1,@Address2,@City,@RegionCode,@ZipCode,@Plus4,@County,@Country,@CountryID,@Phone,@Fax,
						   @Email,(case when ltrim(rtrim(isnull(@Email,''))) <> '' then 1 else 0 end), (case when ltrim(rtrim(isnull(@Fax,''))) <> '' then 1 else 0 end),
						   (case when ltrim(rtrim(isnull(@Phone,''))) <> '' then 1 else 0 end),@PubCategoryID,@PubTransactionID,GETDATE(),
						   @QualificationDate,@PubQSourceID,@SubscriberSourceCode,@Gender,@Address3,@Demo7,@Mobile,@Latitude,@Longitude,
						   @IsAddressValidated,@AddressValidationMessage,@IGrp_No,@DateCreated,@CreatedByUserID,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId,@IsActive,
						   @MailPermission, @FaxPermission, @PhonePermission, @OtherProductsPermission, @EmailRenewPermission, @ThirdPartyPermission, @TextPermission);
						   set @SubscriptionID = (SELECT @@IDENTITY);
				end
			else
				BEGIN
				UPDATE Subscriptions
				SET Sequence = convert(int,isnull(@SequenceID, 0)),
					FName = (CASE WHEN ISNULL(@FirstName,'')!='' AND ISNULL(@LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(@FirstName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE FNAME END),
					LName = (CASE WHEN ISNULL(@FirstName,'')!='' AND ISNULL(@LastName,'')!='' THEN REPLACE(REPLACE(REPLACE(@LastName, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE LNAME END),
					Title = (CASE WHEN ISNULL(@Title,'')!='' THEN REPLACE(REPLACE(REPLACE(@Title, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE TITLE END),
					Company = (CASE WHEN ISNULL(@Company,'')!='' THEN REPLACE(REPLACE(REPLACE(@Company, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COMPANY END),
					Address = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@Address1, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ADDRESS END),
					MailStop = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@Address2, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE MAILSTOP END),
					City = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@City, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE CITY END),
					State = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@RegionCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE STATE END),
					Zip = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@ZipCode, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ZIP END),
					Plus4 = (CASE WHEN ISNULL(@Address1,'')!='' AND ISNULL(@City,'')!='' AND ISNULL(@RegionCode,'')!='' AND ISNULL(@ZipCode,'')!=''
									  THEN REPLACE(REPLACE(REPLACE(@Plus4, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE PLUS4 END),
					County = (CASE WHEN ISNULL(@County,'')!='' THEN REPLACE(REPLACE(REPLACE(@County, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COUNTY END),
					Country = (CASE WHEN ISNULL(@Country,'')!='' THEN REPLACE(REPLACE(REPLACE(@Country, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE COUNTRY END),
					CountryID = (CASE WHEN @CountryID IS NOT NULL THEN @CountryID ELSE CountryID END),
					Phone = (CASE WHEN ISNULL(@Phone,'')!='' THEN REPLACE(REPLACE(REPLACE(@Phone, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE PHONE END),
					Fax = (CASE WHEN ISNULL(@Fax,'')!='' THEN REPLACE(REPLACE(REPLACE(@Fax, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE FAX END),
					mobile = (CASE WHEN ISNULL(@Mobile,'')!='' THEN REPLACE(REPLACE(REPLACE(@Mobile, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE mobile END),
					Email = (CASE WHEN ISNULL(@Email,'')!='' THEN @Email ELSE EMAIL END),
					emailexists = (case when ltrim(rtrim(isnull(@Email,''))) <> '' then 1 else 0 end), 
				    Faxexists   = (case when ltrim(rtrim(isnull(@Fax,''))) <> '' then 1 else 0 end), 
				    PhoneExists = (case when ltrim(rtrim(isnull(@Phone,''))) <> '' then 1 else 0 end),
					CategoryID  = @PubCategoryID,
					TransactionID = @PubTransactionID,
					QDate       =  @QualificationDate,
					SubSrc = @SubSrcID,
					OrigsSrc = @OrigsSrc,
					Par3C = @Par3CID,
					Gender = (CASE WHEN ISNULL(@Gender,'')!='' THEN REPLACE(REPLACE(REPLACE(@Gender, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE Gender END),
					ADDRESS3 = (CASE WHEN ISNULL(@Address3,'')!='' THEN REPLACE(REPLACE(REPLACE(@Address3, CHAR(13), ''), CHAR(10), ''), CHAR(9), ' ') ELSE ADDRESS3 END),
					Demo7 = @Demo7,
					AddressTypeCodeId = (CASE WHEN LEN(@AddressTypeCodeId) > 0 THEN @AddressTypeCodeId else AddressTypeCodeId end),
					AddressLastUpdatedDate = (CASE WHEN LEN(@AddressLastUpdatedDate) > 0 THEN @AddressLastUpdatedDate else AddressLastUpdatedDate end),
					AddressUpdatedSourceTypeCodeId = (CASE WHEN LEN(@AddressUpdatedSourceTypeCodeId) > 0 THEN @AddressUpdatedSourceTypeCodeId else AddressUpdatedSourceTypeCodeId end),
					IsActive = (CASE WHEN LEN(@IsActive) > 0 THEN @IsActive else IsActive end),
					DateUpdated = @DateUpdated,
					UpdatedByUserID = @UpdatedByUserID,
					MailPermission = (CASE WHEN ISNULL(MailPermission, '') = '' AND ISNULL(@MailPermission, '') != '' THEN @MailPermission ELSE MailPermission END),
					FaxPermission = (CASE WHEN ISNULL(FaxPermission, '') = '' AND ISNULL(@FaxPermission, '') != '' THEN @MailPermission ELSE FaxPermission END),
					PhonePermission = (CASE WHEN ISNULL(PhonePermission, '') = '' AND ISNULL(@PhonePermission, '') != '' THEN @PhonePermission ELSE PhonePermission END),
					OtherProductsPermission = (CASE WHEN ISNULL(OtherProductsPermission, '') = '' AND ISNULL(@OtherProductsPermission, '') != '' THEN @OtherProductsPermission ELSE OtherProductsPermission END),
					EmailRenewPermission = (CASE WHEN ISNULL(EmailRenewPermission, '') = '' AND ISNULL(@EmailRenewPermission, '') != '' THEN @EmailRenewPermission ELSE EmailRenewPermission END),
					ThirdPartyPermission = (CASE WHEN ISNULL(ThirdPartyPermission, '') = '' AND ISNULL(@ThirdPartyPermission, '') != '' THEN @ThirdPartyPermission ELSE ThirdPartyPermission END),
					TextPermission = (CASE WHEN ISNULL(TextPermission, '') = '' AND ISNULL(@TextPermission, '') != '' THEN @TextPermission ELSE TextPermission END)
				WHERE SubscriptionID = @SubscriptionID
				
				END

			INSERT INTO PubSubscriptions ([SubscriptionID],[PubID],[Demo7],[QualificationDate],[PubQSourceID],[PubCategoryID],[PubTransactionID],[EmailStatusID],[StatusUpdatedDate],[StatusUpdatedReason],
				[Email],[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],[IsComp],[SubscriptionStatusID],[AddRemoveID],[Copies],	[GraceIssues],[IMBSEQ],[IsActive],[IsPaid],[IsSubscribed],
				[MemberGroup],[OnBehalfOf],[OrigsSrc],[Par3CID],[SequenceID],[Status],[SubscriberSourceCode],[SubSrcID],[Verify],ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,
				Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,
				AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,PhoneExt,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId, 
				IsInActiveWaveMailing, WaveMailingID, IGrp_No, SFRecordIdentifier, ReqFlag, EmailID, SubGenSubscriberID, MailPermission, FaxPermission, PhonePermission, OtherProductsPermission,
				EmailRenewPermission, ThirdPartyPermission, TextPermission,
				SubGenPublicationID,SubGenMailingAddressId,SubGenBillingAddressId,IssuesLeft,UnearnedReveue,
				SubGenIsLead,SubGenRenewalCode,SubGenSubscriptionRenewDate,SubGenSubscriptionExpireDate,SubGenSubscriptionLastQualifiedDate)
			VALUES(@SubscriptionID,@PubID,@Demo7,@QualificationDate,@PubQSourceID,@PubCategoryID,@PubTransactionID,@EmailStatusID,@StatusUpdatedDate,@StatusUpdatedReason,@Email,@DateCreated,@DateUpdated,
				@CreatedByUserID,@UpdatedByUserID,@IsComp,@SubscriptionStatusID,@AddRemoveID,@Copies,@GraceIssues,@IMBSEQ,@IsActive,@IsPaid,@IsSubscribed,@MemberGroup,@OnBehalfOf,@OrigsSrc,@Par3CID,
				@SequenceID,@Status,@SubscriberSourceCode,@SubSrcID,@Verified,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Phone,@Fax,@Mobile,
				@Website,@Birthdate,@Age,@Income,@Gender,@PhoneExt,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId, 
				@IsInActiveWaveMailing, @WaveMailingID, @IGrp_No, @SFRecordIdentifier, @ReqFlag, @EmailID, @SubGenSubscriberID, @MailPermission, @FaxPermission, @PhonePermission,
				@OtherProductsPermission, @EmailRenewPermission, @ThirdPartyPermission, @TextPermission,
				@SubGenPublicationID,@SubGenMailingAddressId,@SubGenBillingAddressId,@IssuesLeft,@UnearnedReveue,
				@SubGenIsLead,@SubGenRenewalCode,@SubGenSubscriptionRenewDate,@SubGenSubscriptionExpireDate,@SubGenSubscriptionLastQualifiedDate);SELECT @@IDENTITY;
		END

END
