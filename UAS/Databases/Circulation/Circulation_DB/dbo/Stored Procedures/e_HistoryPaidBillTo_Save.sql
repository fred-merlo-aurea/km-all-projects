CREATE PROCEDURE [dbo].[e_HistoryPaidBillTo_Save]
@PaidBillToID int,
@SubscriptionPaidID int,
@SubscriptionID int,
@FirstName nvarchar(50),
@LastName nvarchar(50),
@Company nvarchar(100),
@Title nvarchar(50),
@Occupation nvarchar(50)='',
@AddressTypeID int,
@Address1 nvarchar(100),
@Address2 nvarchar(100),
@Address3 nvarchar(100),
@City nvarchar(50),
@RegionCode nvarchar(50),
@RegionID int,
@ZipCode nchar(10),
@Plus4 nchar(10),
@CarrierRoute varchar(10),
@County nvarchar(50),
@Country nvarchar(50),
@CountryID int,
@Latitude decimal(18,15),
@Longitude decimal(18,15),
@IsAddressValidated bit,
@AddressValidationDate datetime,
@AddressValidationSource nvarchar(50),
@AddressValidationMessage nvarchar(max),
@Email nvarchar(100),
@Phone nchar(10),
@Fax nchar(10),
@Mobile nchar(10),
@Website nvarchar(100),
@Birthdate date='',
@Age int='',
@Income nvarchar(50)='',
@Gender nvarchar(50)='',
@DateCreated datetime,
@CreatedByUserID int
AS
IF @DateCreated IS NULL
	BEGIN
		SET @DateCreated = GETDATE();
	END
	
INSERT INTO HistoryPaidBillTo (PaidBillToID,SubscriptionPaidID,SubscriptionID,FirstName,LastName,Company,Title,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,
							   CarrierRoute,County,Country,CountryID,Latitude,Longitude,IsAddressValidated,AddressValidationDate,AddressValidationSource,AddressValidationMessage,
							   Email,Phone,Fax,Mobile,Website,DateCreated,CreatedByUserID)
		VALUES(@PaidBillToID,@SubscriptionPaidID,@SubscriptionID,@FirstName,@LastName,@Company,@Title,@AddressTypeID,@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@CarrierRoute,@County,
				@Country,@CountryID,@Latitude,@Longitude,@IsAddressValidated,@AddressValidationDate,@AddressValidationSource,@AddressValidationMessage,@Email,@Phone,@Fax,@Mobile,
				@Website,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
