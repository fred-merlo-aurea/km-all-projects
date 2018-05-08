CREATE PROCEDURE [dbo].[e_WaveMailingDetail_UpdateOriginal]
(
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
)
AS
BEGIN

	DECLARE	@executeString varchar(8000)
	DECLARE @subscriberString varchar(8000)
	DECLARE @subscriptionString varchar(8000)
	DECLARE @subscriberFinalString varchar(8000)
	DECLARE @subscriptionFinalString varchar(8000)	

	SET @subscriberFinalString = 'UPDATE Subscriber SET'
							
	SET @subscriptionFinalString = 'UPDATE Subscription SET'

	SET @subscriberString = ''
	SET @subscriptionString = ''

	if(@DeliverabilityID > 0)
		set @subscriptionString = @subscriptionString + ' DeliverabilityID = ' + ''''+ CONVERT(varchar(10),@DeliverabilityID) + '''' +  ','
		
	if(@ActionID_Current > 0)
		set @subscriptionString = @subscriptionString + ' ActionID_Current = ' + CONVERT(varchar(10),@ActionID_Current) + ','
		
	if(@ActionID_Previous > 0)
		set @subscriptionString = @subscriptionString + ' ActionID_Previous = ' + CONVERT(varchar(10),@ActionID_Previous) + ','
		
	if(@Copies > 0)
		set @subscriptionString = @subscriptionString + ' Copies = ' + ''''+ CONVERT(varchar(10),@Copies) + '''' + ','
		
	if(LEN(@FirstName) > 0)
		set @subscriberString = @subscriberString + ' FirstName = ' + '''' + @FirstName + '''' + ','
		
	if(LEN(@LastName) > 0)
		set @subscriberString = @subscriberString + ' LastName = ' + '''' + @LastName + '''' + ','
		
	if(LEN(@Company) > 0)
		set @subscriberString = @subscriberString + ' Company = ' + ''''+ @Company + '''' + ','
		
	if(LEN(@Title) > 0)
		set @subscriberString = @subscriberString + ' Title = ' + '''' + @Title + '''' + ','

	if(@AddressTypeID > 0)
		set @subscriberString = @subscriberString + ' AddressTypeID = ' + CONVERT(varchar(10),@AddressTypeID) + ','

	if(LEN(@Address1) > 0)
		set @subscriberString = @subscriberString + ' Address1 = ' + '''' + @Address1 + ''''+ ','

	if(LEN(@Address2) > 0)
		set @subscriberString = @subscriberString + ' Address2 = ' + '''' + @Address2 + '''' + ','
		
	if(LEN(@Address3) > 0)
		set @subscriberString = @subscriberString + ' Address3 = ' + '''' + @Address3 + '''' + ','
		
	if(LEN(@City) > 0)
		set @subscriberString = @subscriberString + ' City = ' + '''' + @City + '''' + ','
		
	if(LEN(@RegionCode) > 0)
		set @subscriberString = @subscriberString + ' RegionCode = ' + '''' + @RegionCode + '''' + ','
		
	if(@RegionID > 0)
		set @subscriberString = @subscriberString + ' RegionID = ' + CONVERT(varchar(10),@RegionID) + ','

	if(LEN(@ZipCode) > 0)
		set @subscriberString = @subscriberString + ' ZipCode = ' + '''' + @ZipCode + '''' + ','

	if(LEN(@Plus4) > 0)
		set @subscriberString = @subscriberString + ' Plus4 = ' + '''' + @Plus4 + '''' + ','
		
	if(LEN(@County) > 0)
		set @subscriberString = @subscriberString + ' County = ' + '''' + @County + '''' + ','
		
	if(LEN(@Country) > 0)
		set @subscriberString = @subscriberString + ' Country = ' + '''' + @Country + '''' + ','
		
	if(@CountryID > 0)
		set @subscriberString = @subscriberString + ' CountryID = ' + CONVERT(varchar(10),@CountryID) + ','

	if(LEN(@Email) > 0)
		set @subscriberString = @subscriberString + ' Email = ' + '''' + @Email + '''' + ','
		
	if(LEN(@Phone) > 0)
		set @subscriberString = @subscriberString + ' Phone = ' + '''' + @Phone + '''' + ','
		
	if(LEN(@Mobile) > 0)
		set @subscriberString = @subscriberString + ' Mobile = ' + '''' + @Mobile + '''' + ','
		
	if(LEN(@Fax) > 0)
		set @subscriberString = @subscriberString + ' Fax = ' + '''' + @Fax + '''' + ','
	
	if(LEN(@subscriberString) > 0)
		BEGIN
			set @subscriberString =	LEFT(@subscriberString, LEN(@subscriberString) - 1)
			set @subscriberFinalString = @subscriberFinalString + @subscriberString + ' WHERE SubscriberID = ' + CONVERT(varchar(255),@SubscriberID)
			EXEC(@subscriberFinalString)
		END
	
	if(LEN(@subscriptionString) > 0)
		BEGIN
			set @subscriptionString = LEFT(@subscriptionString, LEN(@subscriptionString) - 1)
			set @subscriptionFinalString = @subscriptionFinalString + @subscriptionString + ' WHERE SubscriptionID = ' + CONVERT(varchar(255),@SubscriptionID)	
			EXEC(@subscriptionFinalString)
		END
						
END