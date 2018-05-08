CREATE PROCEDURE [dbo].[e_WaveMailingDetail_Save]
@WaveMailingDetailID int,
@WaveMailingID int,
@SubscriberID int,
@SubscriptionID int,
@DeliverabilityID int,
@ActionID_Current int,
@ActionID_Previous int,
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
@Fax varchar(50),
@Mobile varchar(50),
@DateCreated datetime,
@DateUpdated datetime,
@CreatedByUserID int,
@UpdatedByUserID int
AS

IF @WaveMailingDetailID > 0
	BEGIN
		IF @DateUpdated IS NULL
			BEGIN
				SET @DateUpdated = GETDATE();
			END
			
		UPDATE WaveMailingDetail
		SET 
			WaveMailingID = @WaveMailingID,
			DeliverabilityID = @DeliverabilityID,
			ActionID_Current = @ActionID_Current,
			ActionID_Previous = @ActionID_Previous,
			Copies = @Copies,
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
			County = @County,
			Country = @Country,
			CountryID = @CountryID,
			Email = @Email,
			Phone = @Phone,
			Fax = @Fax,
			Mobile = @Mobile,
			DateUpdated = @DateUpdated,
			UpdatedByUserID = @UpdatedByUserID
		WHERE WaveMailingDetailID = @WaveMailingDetailID;
		
		SELECT @WaveMailingDetailID;
	END
ELSE
	BEGIN
		IF @DateCreated IS NULL
			BEGIN
				SET @DateCreated = GETDATE();
			END
		INSERT INTO WaveMailingDetail(WaveMailingID, SubscriberID, SubscriptionID,DeliverabilityID,ActionID_Current,ActionID_Previous,Copies,FirstName,LastName,Company,Title,
					AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,County,Country,CountryID,Email,Phone,Fax,Mobile,DateCreated,CreatedByUserID)
		VALUES(@WaveMailingID,@SubscriberID,@SubscriptionID,@DeliverabilityID,@ActionID_Current,@ActionID_Previous,@Copies,@FirstName,@LastName,@Company,@Title,@AddressTypeID,
				@Address1,@Address2,@Address3,@City,@RegionCode,@RegionID,@ZipCode,@Plus4,@County,@Country,@CountryID,@Email,@Phone,@Fax,@Mobile,@DateCreated,@CreatedByUserID);SELECT @@IDENTITY;
	END
