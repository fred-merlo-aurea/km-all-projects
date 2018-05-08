
CREATE PROCEDURE [dbo].[e_Subscriber_Save]
@SubscriberID int,
@ExternalKeyID int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar(50),
@Occupation varchar(50),
@AddressTypeID int,
@Address1 varchar(100),
@Address2 varchar(100),
@Address3 varchar(100),
@City varchar(50),
@RegionCode varchar(50),
@RegionID int,
@ZipCode varchar(10),
@Plus4 varchar(10),
@CarrierRoute varchar(10),
@County varchar(50),
@Country varchar(50),
@CountryID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource varchar(50),
@AddressValidationMessage varchar(max),
@Email varchar(100),
@Phone varchar(50),
@Fax varchar(50),
@Mobile varchar(50),
@Website varchar(100),
@Birthdate date,
@Age int,
@Income varchar(50),
@Gender varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int,
--@IsLocked bit,
--@LockedByUserID int,
--@LockDate int,
@PhoneExt varchar(25),
@IsInActiveWaveMailing bit,
@AddressTypeCodeId int,
@AddressLastUpdatedDate datetime,
@AddressUpdatedSourceTypeCodeId int,
@WaveMailingID int,
@IGrp_No uniqueidentifier,
@SFRecordIdentifier uniqueidentifier
AS

IF @SubscriberID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE Subscriber
		SET 
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
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID,
			--IsLocked = @IsLocked,
			--LockedByUserID = @LockedByUserID,
			--LockDate = @LockDate,
			PhoneExt = @PhoneExt,
			IsInActiveWaveMailing = @IsInActiveWaveMailing,
			AddressTypeCodeId = @AddressTypeCodeId,
			AddressLastUpdatedDate = @AddressLastUpdatedDate,
			AddressUpdatedSourceTypeCodeId = @AddressUpdatedSourceTypeCodeId,
			WaveMailingID = @WaveMailingID,
			IGrp_No = @IGrp_No,
			SFRecordIdentifier = @SFRecordIdentifier
 
		WHERE SubscriberID = @SubscriberID;
		
		SELECT @SubscriberID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO Subscriber (ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,
								Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,Fax,Mobile,
								Website,Birthdate,Age,Income,Gender,DateCreated,CreatedByUserID,PhoneExt,AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId, 
								IsInActiveWaveMailing, WaveMailingID, IGrp_No, SFRecordIdentifier)
		VALUES(@ExternalKeyID,@FirstName,@LastName,@Company,@Title,@Occupation,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@Fax,@Mobile,
				@Website,@Birthdate,@Age,@Income,@Gender,@DateCreated,@CreatedByUserID,@PhoneExt,@AddressTypeCodeId,@AddressLastUpdatedDate,@AddressUpdatedSourceTypeCodeId, 
				@IsInActiveWaveMailing, @WaveMailingID, @IGrp_No, @SFRecordIdentifier);SELECT @@IDENTITY;
	END
