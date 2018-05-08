CREATE PROCEDURE [dbo].[e_WaveMailingDetail_UpdateOriginal]
(
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
)
AS
BEGIN
	
	SET NOCOUNT ON
	
	DECLARE	@executeString varchar(8000)
	DECLARE @subscriberString varchar(8000)
	DECLARE @subscriptionString varchar(8000)
	DECLARE @subscriberFinalString varchar(8000)
	DECLARE @subscriptionFinalString varchar(8000)	
							
	SET @subscriptionFinalString = 'UPDATE PubSubscriptions SET'

	SET @subscriberString = ''

	if(LEN(@Demo7) > 0)
		set @subscriberString = @subscriberString + ' Demo7 = ' + ''''+ CONVERT(varchar(10),@Demo7) + '''' +  ','
		
	if(@PubCategoryID > 0)
		set @subscriberString = @subscriberString + ' PubCategoryID = ' + CONVERT(varchar(10),@PubCategoryID) + ','
		
	if(@PubTransactionID > 0)
		set @subscriberString = @subscriberString + ' PubTransactionID = ' + CONVERT(varchar(10),@PubTransactionID) + ','
		
	if(LEN(@Copies) > 0)
		set @subscriberString = @subscriberString + ' Copies = ' + ''''+ CONVERT(varchar(10),@Copies) + '''' + ','

	if(@SubscriptionStatusID > 0)
		set @subscriberString = @subscriberString + ' SubscriptionStatusID = ' + ''''+ CONVERT(varchar(10),@SubscriptionStatusID) + '''' + ','

	if(LEN(@IsSubscribed) > 0)
		set @subscriberString = @subscriberString + ' IsSubscribed = ' + ''''+ CONVERT(varchar(10),@IsSubscribed) + '''' + ','
		
	if(LEN(@FirstName) > 0)
		set @subscriberString = @subscriberString + ' FirstName = ' + '''' + @FirstName + '''' + ','
		
	if(LEN(@LastName) > 0)
		set @subscriberString = @subscriberString + ' LastName = ' + '''' + @LastName + '''' + ','
		
	if(LEN(@Company) > 0)
		set @subscriberString = @subscriberString + ' Company = ' + ''''+ @Company + '''' + ','
		
	if(LEN(@Title) > 0)
		set @subscriberString = @subscriberString + ' Title = ' + '''' + @Title + '''' + ','

	if(LEN(@AddressTypeID) > 0)
		set @subscriberString = @subscriberString + ' AddressTypeCodeId = ' + CONVERT(varchar(10),@AddressTypeID) + ','

	if(LEN(@Address1) > 0)
		set @subscriberString = @subscriberString + ' Address1 = ' + '''' + @Address1 + ''''+ ','

	if(LEN(@Address2) > 0)
		begin
			set @subscriberString = @subscriberString + ' Address2 = ' + '''' + @Address2 + '''' + ','
		end
		
	if(LEN(@Address3) > 0)
		set @subscriberString = @subscriberString + ' Address3 = ' + '''' + @Address3 + '''' + ','
		
	if(LEN(@City) > 0)
		set @subscriberString = @subscriberString + ' City = ' + '''' + @City + '''' + ','
		
	if(LEN(@RegionCode) > 0)
		set @subscriberString = @subscriberString + ' RegionCode = ' + '''' + @RegionCode + '''' + ','
		
	if(LEN(@RegionID) > 0)
		set @subscriberString = @subscriberString + ' RegionID = ' + CONVERT(varchar(10),@RegionID) + ','

	if(LEN(@ZipCode) > 0)
		set @subscriberString = @subscriberString + ' ZipCode = ' + '''' + @ZipCode + '''' + ','

	if(LEN(@Plus4) > 0)
		set @subscriberString = @subscriberString + ' Plus4 = ' + '''' + @Plus4 + '''' + ','
		
	if(LEN(@County) > 0)
		set @subscriberString = @subscriberString + ' County = ' + '''' + @County + '''' + ','
		
	if(LEN(@Country) > 0)
		set @subscriberString = @subscriberString + ' Country = ' + '''' + @Country + '''' + ','
		
	if(LEN(@CountryID) > 0)
		set @subscriberString = @subscriberString + ' CountryID = ' + CONVERT(varchar(10),@CountryID) + ','

	if(LEN(@Email) > 0)
		set @subscriberString = @subscriberString + ' Email = ' + '''' + @Email + '''' + ','
		
	if(LEN(@Phone) > 0)
		set @subscriberString = @subscriberString + ' Phone = ' + '''' + @Phone + '''' + ','
		
	if(LEN(@Mobile) > 0)
		set @subscriberString = @subscriberString + ' Mobile = ' + '''' + @Mobile + '''' + ','
		
	if(LEN(@Fax) > 0)
		set @subscriberString = @subscriberString + ' Fax = ' + '''' + @Fax + '''' + ','

	if(LEN(@IsPaid) > 0)
		set @subscriberString = @subscriberString + ' IsPaid = ' + '''' + CONVERT(varchar(10),@IsPaid)  + '''' + ','

	if(LEN(@PhoneExt) > 0)
		set @subscriberString = @subscriberString + ' PhoneExt = ' + '''' + @PhoneExt + '''' + ','
		
	if(LEN(@subscriberString) > 0)
		BEGIN			
			set @subscriberString = LEFT(@subscriberString, LEN(@subscriberString) - 1)
			set @subscriptionFinalString = @subscriptionFinalString + @subscriberString + ' WHERE SubscriptionID = ' + CONVERT(varchar(255),@SubscriptionID)	
			EXEC(@subscriptionFinalString)
			INSERT INTO HistorySubscription (PubSubscriptionID,	SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID,	PubCategoryID,	PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
				StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
				GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
				ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
				CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
				Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
				IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier)
			SELECT ps.PubSubscriptionID,ps.SubscriptionID,	PubID,	Demo7,	QualificationDate,	PubQSourceID, ps.PubCategoryID, ps.PubTransactionID,	EmailStatusID,	StatusUpdatedDate,
				StatusUpdatedReason, Email,	DateCreated, DateUpdated, CreatedByUserID, UpdatedByUserID,	IsComp,	SubscriptionStatusID,	AccountNumber,	AddRemoveID, Copies,	
				GraceIssues, IMBSEQ, IsActive, IsPaid,	IsSubscribed, MemberGroup, OnBehalfOf,	OrigsSrc, Par3CID, SequenceID, [Status], SubscriberSourceCode, SubSrcID, Verify,
				ExternalKeyID,	FirstName,	LastName, Company, Title, Occupation, AddressTypeID, Address1, Address2, Address3, City, RegionCode, RegionID, ZipCode,	Plus4,
				CarrierRoute, County, Country, CountryID, Latitude,	Longitude,	IsAddressValidated,	AddressValidationDate,	AddressValidationSource, AddressValidationMessage, Phone,
				Fax, Mobile, Website, Birthdate, Age, Income, Gender, tmpSubscriptionID, IsLocked,	LockedByUserID,	LockDate,	LockDateRelease, PhoneExt,
				IsInActiveWaveMailing,	AddressTypeCodeId,	AddressLastUpdatedDate,	AddressUpdatedSourceTypeCodeId,	WaveMailingID,	IGrp_No, SFRecordIdentifier 
			FROM PubSubscriptions ps
			WHERE PubSubscriptionID = @PubSubscriptionID
		END					
END