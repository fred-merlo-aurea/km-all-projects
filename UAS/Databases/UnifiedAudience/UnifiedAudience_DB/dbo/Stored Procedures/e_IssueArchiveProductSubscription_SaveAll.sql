CREATE PROCEDURE [dbo].[e_IssueArchiveProductSubscription_SaveAll]
	@IssueArchiveSubscriptionId int,
	@IsComp bit,
	@CompId int,
	@IssueID int,
	@SplitID int,
	@PubSubscriptionID int,
	@SubscriptionID int,
	@PubID int,
	@Demo7 varchar(1) = null,
	@QualificationDate date = null,
	@PubQSourceID int = null,
	@PubCategoryID int = null,
	@PubTransactionID int = null,
	@EmailStatusID int = null,
	@StatusUpdatedDate datetime = null,
	@StatusUpdatedReason varchar(200) = null,
	@Email varchar(100) = null,
	@DateCreated datetime = null,
	@DateUpdated datetime = null,
	@CreatedByUserID int = null,
	@UpdatedByUserID int = null,
	@SubscriptionStatusID int = null,
	@AccountNumber varchar(50) = null,
	@AddRemoveID int = null,
	@Copies int = null,
	@GraceIssues int = null,
	@IMBSEQ varchar(256) = null,
	@IsActive bit = null,
	@IsPaid bit = null,
	@IsSubscribed bit = null,
	@MemberGroup varchar(256) = null,
	@OnBehalfOf varchar(256) = null,
	@OrigsSrc varchar(256) = null,
	@Par3CID int = null,
	@SequenceID int = null,
	@Status varchar(50) = null,
	@SubscriberSourceCode varchar(100) = null,
	@SubSrcID int = null,
	@Verify varchar(100) = null,
	@ExternalKeyID int = null,
	@FirstName varchar(50) = null,
	@LastName varchar(50) = null,
	@Company varchar(100) = null,
	@Title varchar(255) = null,
	@Occupation varchar(50) = null,
	@AddressTypeID int = null,
	@Address1 varchar(256) = null,
	@Address2 varchar(256) = null,
	@Address3 varchar(256) = null,
	@City varchar(50) = null,
	@RegionCode varchar(50) = null,
	@RegionID int = null,
	@ZipCode varchar(50) = null,
	@Plus4 varchar(10) = null,
	@CarrierRoute varchar(10) = null,
	@County varchar(50) = null,
	@Country varchar(50) = null,
	@CountryID int = null,
	@Latitude decimal(18,15) = null,
	@Longitude decimal(18,15) = null,
	@IsAddressValidated bit = 'false',
	@AddressValidationDate datetime = null,
	@AddressValidationSource varchar(50) = null,
	@AddressValidationMessage varchar(max) = null,
	@Phone varchar(100) = null,
	@Fax varchar(100) = null,
	@Mobile varchar(100) = null,
	@Website varchar(255) = null,
	@Birthdate date = null,
	@Age int = null,
	@Income varchar(50) = null,
	@Gender varchar(50) = null,
	@IsLocked bit = 'false',
	@LockedByUserID int = null,
	@LockDate datetime = null,
	@LockDateRelease datetime = null,
	@PhoneExt varchar(25) = null,
	@IsInActiveWaveMailing bit = 'false',
	@AddressTypeCodeId int = null,
	@AddressLastUpdatedDate datetime = null,
	@AddressUpdatedSourceTypeCodeId int = null,
	@WaveMailingID int = null,
	@IGrp_No uniqueidentifier = null,
	@SFRecordIdentifier uniqueidentifier = null,
	@ReqFlag int = null,
	@SubGenSubscriberID int = null,
	@MailPermission bit = null,
	@FaxPermission bit = null,
	@PhonePermission bit = null,
	@OtherProductsPermission bit = null,
	@ThirdPartyPermission bit = null,
	@EmailRenewPermission bit = null,
	@TextPermission bit = null
AS
	BEGIN

	SET NOCOUNT ON

	IF @IssueArchiveSubscriptionId > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			UPDATE IssueArchiveProductSubscription
			SET IsComp = @IsComp,
				CompId = @CompId,
				IssueID = @IssueID,
				SplitID = @SplitID,
				PubSubscriptionID = @PubSubscriptionID,
				SubscriptionID = @SubscriptionID,
				PubID = @PubID,
				Demo7 = @Demo7,
				QualificationDate = @QualificationDate,
				PubQSourceID = @PubQSourceID,
				PubCategoryID = @PubCategoryID,
				PubTransactionID = @PubTransactionID,
				EmailStatusID = @EmailStatusID,
				StatusUpdatedDate = @StatusUpdatedDate,
				StatusUpdatedReason = @StatusUpdatedReason,
				Email = @Email,
				DateCreated = @DateCreated,
				DateUpdated = @DateUpdated,
				CreatedByUserID = @CreatedByUserID,
				UpdatedByUserID = @UpdatedByUserID,
				SubscriptionStatusID = @SubscriptionStatusID,
				AccountNumber = @AccountNumber,
				AddRemoveID = @AddRemoveID,
				Copies = @Copies,
				GraceIssues = @GraceIssues,
				IMBSEQ = @IMBSEQ,
				IsActive = @IsActive,
				IsPaid = @IsPaid,
				IsSubscribed = @IsSubscribed,
				MemberGroup = @MemberGroup,
				OnBehalfOf = @OnBehalfOf,
				OrigsSrc = @OrigsSrc,
				Par3CID = @Par3CID,
				SequenceID = @SequenceID,
				Status = @Status,
				SubscriberSourceCode = @SubscriberSourceCode,
				SubSrcID = @SubSrcID,
				Verify = @Verify,
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
				IsLocked = @IsLocked,
				LockedByUserID = @LockedByUserID,
				LockDate = @LockDate,
				LockDateRelease = @LockDateRelease,
				PhoneExt = @PhoneExt,
				IsInActiveWaveMailing = @IsInActiveWaveMailing,
				AddressTypeCodeId = @AddressTypeCodeId,
				AddressLastUpdatedDate = @AddressLastUpdatedDate,
				AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId,
				WaveMailingID = @WaveMailingID,
				IGrp_No = @IGrp_No,
				SFRecordIdentifier = @SFRecordIdentifier,
				ReqFlag = @ReqFlag,
				SubGenSubscriberID = @SubGenSubscriberID,
				MailPermission = @MailPermission,
				FaxPermission = @FaxPermission,
				PhonePermission = @PhonePermission,
				OtherProductsPermission = @OtherProductsPermission,
				ThirdPartyPermission = @ThirdPartyPermission,
				EmailRenewPermission = @EmailRenewPermission,
				TextPermission = @TextPermission
			WHERE IssueArchiveSubscriptionId = @IssueArchiveSubscriptionId;

			SELECT @IssueArchiveSubscriptionId;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			INSERT INTO IssueArchiveProductSubscription 
			(IsComp,CompId,IssueID,SplitID,PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID
			,StatusUpdatedDate,StatusUpdatedReason,Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies
			,GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,ExternalKeyID
			,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country
			,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate
			,Age,Income,Gender,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing,AddressTypeCodeId,AddressLastUpdatedDate
			,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier,ReqFlag,SubGenSubscriberID,MailPermission,FaxPermission,PhonePermission
			,OtherProductsPermission,ThirdPartyPermission,EmailRenewPermission,TextPermission)
			VALUES
			(@IsComp,@CompId,@IssueID,@SplitID,@PubSubscriptionID,@SubscriptionID,@PubID,@Demo7,@QualificationDate,@PubQSourceID,@PubCategoryID
			,@PubTransactionID,@EmailStatusID,@StatusUpdatedDate,@StatusUpdatedReason,@Email,@DateCreated,@DateUpdated,@CreatedByUserID,@UpdatedByUserID,@SubscriptionStatusID
			,@AccountNumber,@AddRemoveID,@Copies,@GraceIssues,@IMBSEQ,@IsActive,@IsPaid,@IsSubscribed,@MemberGroup,@OnBehalfOf,@OrigsSrc,@Par3CID,@SequenceID,@Status
			,@SubscriberSourceCode,@SubSrcID,@Verify,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City
			,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate
			,@AddressValidationSource,@AddressValidationMessage,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@IsLocked,@LockedByUserID,@LockDate,@LockDateRelease
			,@PhoneExt,@IsInActiveWaveMailing,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId,@WaveMailingID,@IGrp_No,@SFRecordIdentifier,@ReqFlag
			,@SubGenSubscriberID,@MailPermission,@FaxPermission,@PhonePermission,@OtherProductsPermission,@ThirdPartyPermission,@EmailRenewPermission,@TextPermission);
				SELECT @@IDENTITY;
		END

END
GO
