﻿CREATE PROCEDURE e_IssueCompDetail_Save
@IssueCompDetailId int,
@IssueCompID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar (255),
@Occupation varchar (50),
@AddressTypeID  int,
@Address1  varchar (100),
@Address2  varchar (100),
@Address3  varchar (100),
@City varchar (50),
@RegionCode varchar (50),
@RegionID  int,
@ZipCode   varchar (50),
@Plus4 varchar (10),
@CarrierRoute varchar (10),
@County varchar (50),
@Country varchar (50),
@CountryID int,
@Latitude decimal(18, 15),
@Longitude decimal(18, 15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(255),
@Phone varchar(25),
@Fax varchar(25),
@Mobile varchar(25),
@Website varchar(255),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@IsLocked bit,
@PhoneExt varchar(25), 
@SequenceID int,
@PublisherID int,
@SubscriberID int,
@PublicationID int,
@ActionID_Current int,
@ActionID_Previous int,
@SubscriptionStatusID int,
@IsPaid bit,
@QSourceID int,
@QSourceDate date,
@DeliverabilityID int,
@IsSubscribed bit,
@SubscriberSourceCode varchar(256),
@Copies int,
@OriginalSubscriberSourceCode varchar(256),
@Par3cID int,
@SubsrcTypeID int,
@AccountNumber varchar(50),
@OnBehalfOf varchar(256), 
@MemberGroup varchar(256), 
@Verify varchar(256), 
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
IF @IssueCompDetailId > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
		UPDATE IssueCompDetail
		SET IssueCompID = @IssueCompID,
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
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			Birthdate = @Birthdate,
			Age = @Age,
			Income = @Income,
			Gender = @Gender,
			IsLocked = @IsLocked,
			PhoneExt = @PhoneExt, 
			SequenceID = @SequenceID,
			PublisherID = @PublisherID,
			SubscriberID = @SubscriberID,
			PublicationID = @PublicationID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			SubscriptionStatusID = @SubscriptionStatusID,
			IsPaid = @IsPaid,
			QSourceID = @QSourceID,
			QSourceDate = @QSourceDate,
			DeliverabilityID = @DeliverabilityID,
			IsSubscribed = @IsSubscribed,
			SubscriberSourceCode = @SubscriberSourceCode,
			Copies = @Copies,
			OriginalSubscriberSourceCode = @OriginalSubscriberSourceCode,
			Par3cID = @Par3cID,
			SubsrcTypeID = @SubsrcTypeID,
			AccountNumber = @AccountNumber,
			OnBehalfOf = @OnBehalfOf, 
			MemberGroup = @MemberGroup, 
			Verify = @Verify, 
			DateCreated = @DateCreated,
			DateUpdated = @DateUpdated,
			CreatedByUserID = @CreatedByUserID,
			UpdatedByUserID = @UpdatedByUserID
		WHERE IssueCompDetailId = @IssueCompDetailId;

		SELECT @IssueCompDetailId;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT into IssueCompDetail (IssueCompID,ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,
									 County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
									 Email,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,IsLocked,PhoneExt,SequenceID,PublisherID,SubscriberID,PublicationID,
									 ActionID_Current,ActionID_Previous,SubscriptionStatusID,IsPaid,QSourceID,QSourceDate,DeliverabilityID,IsSubscribed,SubscriberSourceCode,
									 Copies,OriginalSubscriberSourceCode,Par3cID,SubsrcTypeID,AccountNumber,OnBehalfOf,MemberGroup,Verify,DateCreated,CreatedByUserID)
		VALUES(@IssueCompID,@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,
			   @County,@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,
			   @Email,@Phone,@Fax,@Mobile,@Website,@Birthdate,@Age,@Income,@Gender,@IsLocked,@PhoneExt,@SequenceID,@PublisherID,@SubscriberID,@PublicationID,
			   @ActionID_Current,@ActionID_Previous,@SubscriptionStatusID,@IsPaid,@QSourceID,@QSourceDate,@DeliverabilityID,
			   @IsSubscribed,@SubscriberSourceCode,@Copies,@OriginalSubscriberSourceCode,@Par3cID,@SubsrcTypeID,@AccountNumber,
			   @OnBehalfOf,@MemberGroup,@Verify,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
GO