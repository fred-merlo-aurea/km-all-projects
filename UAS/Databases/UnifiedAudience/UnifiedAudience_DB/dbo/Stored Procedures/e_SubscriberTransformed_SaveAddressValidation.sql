CREATE PROCEDURE [dbo].[e_SubscriberTransformed_SaveAddressValidation]
@xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SORecordIdentifier uniqueidentifier, [Address] varchar(255) NULL, MailStop varchar(255) NULL, City varchar(50) NULL, [State] varchar(50) NULL,
		Zip varchar(10) NULL, Plus4 varchar(50) NULL, Latitude decimal(18, 15) NULL, Longitude decimal(18, 15) NULL, IsLatLonValid bit NULL, IsMailable bit NULL, 
		LatLongMsg nvarchar(500) NULL, Country varchar(100) NULL, County varchar(100) NULL
	)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	insert into @import 
	(
		 SORecordIdentifier, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, IsMailable, LatLongMsg, Country, County
	)  
	
	SELECT 
		SORecordIdentifier, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, IsMailable, LatLongMsg, Country, County
	FROM OPENXML(@docHandle, N'/XML/Subscriber') --SubscriberOriginal  
	WITH   
	(  
		SORecordIdentifier uniqueidentifier 'SORecordIdentifier',
		[Address] varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City', 
		[State] varchar(50) 'State',
		Zip varchar(10) 'Zip', 
		Plus4 varchar(10) 'Plus4', 
		Latitude decimal(18, 15) 'Latitude', 
		Longitude decimal(18, 15) 'Longitude', 
		IsLatLonValid bit 'IsLatLonValid', 
		IsMailable bit 'IsMailable', 
		LatLongMsg nvarchar(500) 'LatLongMsg',
		Country varchar(100) 'Country',
		County varchar(100) 'County'
	)

	EXEC sp_xml_removedocument @docHandle  

	UPDATE SubscriberTransformed
	SET	Address = i.Address,
		MailStop = i.Mailstop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = case when len(i.Plus4) > 0 and SubscriberTransformed.Plus4 != i.Plus4 then i.Plus4 else SubscriberTransformed.Plus4 end,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		--IsMailable = i.IsMailable,
		LatLonMsg = i.LatLongMsg,
		Country = case when len(i.Country) > 0 and SubscriberTransformed.Country != i.Country then i.Country else SubscriberTransformed.Country end,
		County = case when len(i.County) > 0 and SubscriberTransformed.County != i.County then i.County else SubscriberTransformed.County end,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1
	FROM @import i
	WHERE SubscriberTransformed.SORecordIdentifier = i.SORecordIdentifier
	
END	