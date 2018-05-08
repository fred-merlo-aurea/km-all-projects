CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Save]
@WaveMailingDetailID int,
@WaveMailingID int,
@PubSubscriptionID int,
@SubscriptionID int,
@Demo7 varchar(2),
@PubCategoryID int,
@PubTransactionID int,
@IsSubscribed bit,
@IsPaid bit = NULL,
@SubscriptionStatusID int,
@Copies int,
@FirstName varchar(50),
@LastName varchar(50),
@Company varchar(100),
@Title varchar(50),
@AddressTypeID int,
@Address1 varchar(100),
@Address2 varchar(100),
@Address3 varchar(100),
@City varchar(50),
@RegionCode varchar(50),
@RegionID int,
@ZipCode varchar(10),
@Plus4 varchar(10),
@County varchar(50),
@Country varchar(50),
@CountryID int,
@Email varchar(100),
@Phone varchar(50),
@PhoneExt varchar(25) = '',
@Fax varchar(50),
@Mobile varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS
BEGIN
	
	SET NOCOUNT ON

	DECLARE @IAFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAFree')
	DECLARE @IAPAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAPaid')
	DECLARE @AFree int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AFree')
	DECLARE @APAid int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'APaid')
	DECLARE @APros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'AProsp')
	DECLARE @IAPros int = (SELECT SubscriptionStatusID FROM UAD_Lookup..SubscriptionStatus WHERE StatusCode = 'IAProsp')

	DECLARE @PubSub table (PubSubscriptionID int, PubCategoryID int, PubTransactionID int)

	Insert into @PubSub (PubSubscriptionID, PubCategoryID, PubTransactionID)
	Select PubSubscriptionID, PubCategoryID, PubTransactionID from PubSubscriptions where PubSubscriptionID = @PubSubscriptionID

	--Attempt to see if record exists in WaveMailingDetail
	IF @WaveMailingDetailID < 1
		BEGIN
			IF EXISTS (SELECT 1 FROM WaveMailingDetail WITH(NOLOCK) WHERE PubSubscriptionID = @PubSubscriptionID and WaveMailingID = @WaveMailingID)
			BEGIN
				SET @WaveMailingDetailID = (SELECT WaveMailingDetailID FROM WaveMailingDetail WITH(NOLOCK) WHERE PubSubscriptionID = @PubSubscriptionID and WaveMailingID = @WaveMailingID)
			END
		END
	
	IF @WaveMailingDetailID > 0
		BEGIN
			IF @DateUpdated IS NULL
				BEGIN
					SET @DateUpdated = GETDATE();
				END
			
			UPDATE wmd 
			SET WaveMailingID = @WaveMailingID,
				Demo7 = CASE WHEN LEN(@Demo7) > 0 THEN @Demo7 ELSE wmd.Demo7 END,
				PubCategoryID = CASE WHEN @PubCategoryID > 0 THEN @PubCategoryID ELSE ps.PubCategoryID END,
				PubTransactionID = CASE WHEN @PubTransactionID > 0 THEN @PubTransactionID ELSE ps.PubTransactionID END,
				IsSubscribed = CASE WHEN @IsSubscribed = 'true' Or @IsSubscribed = 'false' THEN @IsSubscribed ELSE wmd.IsSubscribed END,
				IsPaid = CASE WHEN @IsPaid = 'true' Or @IsPaid = 'false' THEN @IsPaid ELSE wmd.IsPaid END,
				SubscriptionStatusID = 	CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
											WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
											WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
											WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
											WHEN cc.CategoryCodeTypeID in (1,2,3,4) AND tc.TransactionCodeTypeID in (1,3) AND cc.CategoryCodeValue in (70,71) THEN @APros 
											WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros END,
				Copies = CASE WHEN @Copies > 0 THEN @Copies ELSE wmd.Copies END,
				FirstName = CASE WHEN LEN(@FirstName) > 0 THEN @FirstName ELSE wmd.FirstName END,
				LastName = CASE WHEN LEN(@LastName) > 0 THEN @LastName ELSE wmd.LastName END,
				Company = CASE WHEN LEN(@Company) > 0 THEN @Company ELSE wmd.Company END,
				Title = CASE WHEN LEN(@Title) > 0 THEN @Title ELSE wmd.Title END,
				AddressTypeID = CASE WHEN @AddressTypeID > 0 THEN @AddressTypeID ELSE wmd.AddressTypeID END,
				Address1 = CASE WHEN LEN(@Address1) > 0 THEN @Address1 ELSE wmd.Address1 END,
				Address2 = CASE WHEN LEN(@Address2) > 0 THEN @Address2 ELSE wmd.Address2 END,
				Address3 = CASE WHEN LEN(@Address3) > 0 THEN @Address3 ELSE wmd.Address3 END,
				City = CASE WHEN LEN(@City) > 0 THEN @City ELSE wmd.City END,
				RegionCode = CASE WHEN LEN(@RegionCode) > 0 THEN @RegionCode ELSE wmd.RegionCode END,
				RegionID = CASE WHEN @RegionID > 0 THEN @RegionID ELSE wmd.RegionID END,
				ZipCode = CASE WHEN LEN(@ZipCode) > 0 THEN @ZipCode ELSE wmd.ZipCode END,
				Plus4 = CASE WHEN LEN(@Plus4) > 0 THEN @Plus4 ELSE wmd.Plus4 END,
				County = CASE WHEN LEN(@County) > 0 THEN @County ELSE wmd.County END,
				Country = CASE WHEN LEN(@Country) > 0 THEN @Country ELSE wmd.Country END,
				CountryID = CASE WHEN @CountryID > 0 THEN @CountryID ELSE wmd.CountryID END,
				Email = CASE WHEN LEN(@Email) > 0 THEN @Email ELSE wmd.Email END,
				Phone = CASE WHEN LEN(@Phone) > 0 THEN @Phone ELSE wmd.Phone END,
				PhoneExt = CASE WHEN LEN(@PhoneExt) > 0 THEN @PhoneExt ELSE wmd.PhoneExt END,
				Fax = CASE WHEN LEN(@Fax) > 0 THEN @Fax ELSE wmd.Fax END,
				Mobile = CASE WHEN LEN(@Mobile) > 0 THEN @Mobile ELSE wmd.Mobile END,
				DateUpdated = @DateUpdated,
				UpdatedByUserID = @UpdatedByUserID
			FROM WaveMailingDetail wmd
				join PubSubscriptions ps on wmd.PubSubscriptionID = ps.PubSubscriptionID
				left join UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = (CASE WHEN @PubCategoryID <> ps.PubCategoryID THEN @PubCategoryID ELSE ps.PubCategoryID END)
				left join UAD_LookUp..TransactionCode tc ON tc.TransactionCodeID = (CASE WHEN @PubTransactionID <> ps.PubTransactionID THEN @PubTransactionID ELSE ps.PubTransactionID END)
			WHERE WaveMailingDetailID = @WaveMailingDetailID;
		
			SELECT @WaveMailingDetailID;
		END
	ELSE
		BEGIN
			IF @DateCreated IS NULL
				BEGIN
					SET @DateCreated = GETDATE();
				END
			IF LEN(@Demo7) < 1 
				BEGIN
					SET @Demo7 = NULL
				END
			IF @PubCategoryID < 1
				BEGIN
					SET @PubCategoryID = (Select PubCategoryID from @PubSub)
				END
			IF @PubTransactionID < 1
				BEGIN
					SET @PubTransactionID = (Select PubTransactionID from @PubSub)
				END
			IF @SubscriptionStatusID < 1
				BEGIN
					SET @SubscriptionStatusID = (Select CASE WHEN cc.CategoryCodeTypeID in (1,2) AND tc.TransactionCodeTypeID = 1 AND cc.CategoryCodeValue not in (70,71) THEN @AFree 
											WHEN cc.CategoryCodeTypeID in (3,4) AND tc.TransactionCodetypeID = 3 AND cc.CategoryCodeValue not in (70,71) THEN @APAid
											WHEN tc.TransactionCodetypeID = 2 AND cc.CategoryCodeValue not in (70,71) THEN @IAFree
											WHEN tc.TransactionCodeTypeID = 4 AND cc.CategoryCodeValue not in (70,71) THEN @IAPAid
											WHEN cc.CategoryCodeTypeID in (1,2,3,4) AND tc.TransactionCodeTypeID in (1,3) AND cc.CategoryCodeValue in (70,71) THEN @APros 
											WHEN tc.TransactionCodetypeID in (2,4) AND cc.CategoryCodeValue in (70,71) THEN @IAPros END
											from @PubSub 
											left join UAD_LookUp..CategoryCode cc ON cc.CategoryCodeID = (CASE WHEN @PubCategoryID <> PubCategoryID THEN @PubCategoryID ELSE PubCategoryID END)
											left join UAD_LookUp..TransactionCode tc ON tc.TransactionCodeID = (CASE WHEN @PubTransactionID <> PubTransactionID THEN @PubTransactionID ELSE PubTransactionID END))
				END

			INSERT INTO WaveMailingDetail(WaveMailingID, PubSubscriptionID, SubscriptionID,Demo7,PubCategoryID,PubTransactionID,IsSubscribed,SubscriptionStatusID,Copies,FirstName,LastName,Company,Title,
						AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,County,Country,CountryID,Email,Phone,Fax,Mobile,DateCreated,CreatedByUserID, PhoneExt, IsPaid)
			VALUES(@WaveMailingID,@PubSubscriptionID,@SubscriptionID,@Demo7,@PubCategoryID,@PubTransactionID,@IsSubscribed,@SubscriptionStatusID,@Copies,@FirstName,@LastName,@Company,@Title,@AddressTypeID,
					@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@County,@Country,@CountryID,@Email,@Phone,@Fax,@Mobile,@DateCreated,@CreatedByUserID, @PhoneExt, @IsPaid);SELECT @@IDENTITY;
		END

END