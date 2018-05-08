CREATE PROCEDURE e_Subscription_AddressUpdate
@xml xml

AS
BEGIN

	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SubscriptionID int, [Address] varchar(255) NULL, MailStop varchar(255) NULL, City varchar(50) NULL, [State] varchar(50) NULL,
		Zip varchar(10) NULL, Plus4 varchar(50) NULL, Latitude decimal(18, 15) NULL, Longitude decimal(18, 15) NULL, IsLatLonValid bit NULL,  
		LatLongMsg nvarchar(500) NULL, Country varchar(100) NULL, County varchar(100) NULL
	)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	insert into @import 
	(
		 SubscriptionID, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, LatLongMsg, Country, County
	)  
	
	SELECT 
		SubscriptionID, [Address], MailStop, City, [State],Zip, Plus4, Latitude, Longitude, IsLatLonValid, LatLongMsg, Country, County
	FROM OPENXML(@docHandle, N'/XML/Subscriber') 
	WITH   
	(  
		SubscriptionID int 'SubscriptionID',
		[Address] varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City', 
		[State] varchar(50) 'State',
		Zip varchar(10) 'Zip', 
		Plus4 varchar(10) 'Plus4', 
		Latitude decimal(18, 15) 'Latitude', 
		Longitude decimal(18, 15) 'Longitude', 
		IsLatLonValid bit 'IsLatLonValid', 
		LatLongMsg nvarchar(500) 'LatLongMsg',
		Country varchar(100) 'Country',
		County varchar(100) 'County'
	)

	EXEC sp_xml_removedocument @docHandle  

	DECLARE @addressUpdate int = (select CodeId from UAD_Lookup..Code with(nolock) where CodeName = 'Geocoding' and CodeTypeId = (select CodeTypeId from UAD_Lookup..CodeType with(nolock) where CodeTypeName = 'Address Update Source'))
	UPDATE Subscriptions
	SET	Address = i.Address,
		MailStop = i.Mailstop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = case when len(i.Plus4) > 0 and Subscriptions.Plus4 != i.Plus4 then i.Plus4 else Subscriptions.Plus4 end,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		LatLonMsg = i.LatLongMsg,
		Country = case when len(i.Country) > 0 and Subscriptions.Country != i.Country then i.Country else Subscriptions.Country end,
		County = case when len(i.County) > 0 and Subscriptions.County != i.County then Substring(i.County,0,20) else Subscriptions.County end,
		IsMailable= case 
						when 
							i.IsLatLonValid = 1 and 
							Len(Subscriptions.address)>0 and 
							len(i.City)>0 and 
							len(i.State)>0 and 
							len(i.zip)>= 5 and 
							(Subscriptions.CountryID = 1 or Subscriptions.CountryID = 2)  
							and i.LatLongMsg like 'Rooftop%'
						then 1 else 0 
					end,
		AddressLastUpdatedDate = GETDATE(),
		AddressUpdatedSourceTypeCodeId = @addressUpdate
	FROM @import i
	WHERE Subscriptions.SubscriptionID = i.SubscriptionID
	
END	
GO