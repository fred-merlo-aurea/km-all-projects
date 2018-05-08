CREATE PROCEDURE [dbo].[e_PaidBillTo_Save]
@PaidBillToID int,
@SubscriptionPaidID int,
@SubscriptionID int,
@FirstName varchar(50)='',
@LastName varchar(50)='',
@Company varchar(100)='',
@Title varchar(255)='',
@AddressTypeID int='',
@Address1 varchar(100)='',
@Address2 varchar(100)='',
@Address3 varchar(100)='',
@City varchar(50)='',
@RegionCode varchar(50)='',
@RegionID int='',
@ZipCode varchar(50)='',
@Plus4 varchar(10)='',
@CarrierRoute varchar(10)='',
@County varchar(50)='',
@Country varchar(50)='',
@CountryID int='',
@Latitude decimal(18,15)='',
@Longitude decimal(18,15)='',
@IsAddressValidated bit='',
@AddressValidationDate datetime='',
@AddressValidationSource varchar(50)='',
@AddressValidationMessage varchar(max)='',
@Email varchar(255)='',
@Phone varchar(50)='',
@PhoneExt varchar(25)='',
@Fax varchar(50)='',
@Mobile varchar(50)='',
@Website varchar(255)='',
@DateCreated datetime='',
@DateUpdated datetime='',
@CreatedByUserID int='',
@UpdatedByUserID int=''
AS

IF @PaidBillToID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE PaidBillTo
		SET 
			FirstName = @FirstName,
			LastName = @LastName,
			Company = @Company,
			Title = @Title,
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
			PhoneExt = @PhoneExt,
			Fax = @Fax,
			Mobile = @Mobile,
			Website = @Website,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE PaidBillToID = @PaidBillToID;
		
		SELECT @PaidBillToID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO PaidBillTo (SubscriptionPaidID,SubscriptionID,FirstName,LastName,Company,Title,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,
								Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,Email,Phone,PhoneExt,Fax,Mobile,
								Website,DateCreated,CreatedByUserID)
		VALUES(@SubscriptionPaidID,@SubscriptionID,@FirstName,@LastName,@Company,@Title,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@PhoneExt,@Fax,@Mobile,
				@Website,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END

