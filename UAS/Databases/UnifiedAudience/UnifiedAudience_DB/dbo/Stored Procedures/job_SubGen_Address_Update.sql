create procedure job_SubGen_Address_Update
@xml xml
as
BEGIN

	SET NOCOUNT ON 	

	declare @docHandle int
    declare @insertcount int
    
	declare @import table    
	(  
		[address_id] [int] NOT NULL,
		[account_id] [int] NOT NULL,
		[first_name] [varchar](50) NULL,
		[last_name] [varchar](50) NULL,
		[address] [varchar](50) NULL,
		[address_line_2] [varchar](50) NULL,
		[company] [varchar](50) NULL,
		[city] [varchar](50) NULL,
		[state] [varchar](60) NULL,
		[subscriber_id] [int] NULL,
		[country] [varchar](50) NULL,
		[country_name] [varchar](50) NULL,
		[country_abbreviation] [varchar](50) NULL,
		[latitude] [decimal](18, 10) NULL,
		[longitude] [decimal](18, 10) NULL,
		[verified] [bit] NULL,
		[zip_code] [varchar](12) NULL
	)
	exec sp_xml_preparedocument @docHandle output, @xml  
	insert into @import 
	(
		 address_id,account_id,first_name,last_name,address,address_line_2,company,city,state,subscriber_id,country,country_name,
		 country_abbreviation,latitude,longitude,verified,zip_code
	)  
	
	select address_id,account_id,first_name,last_name,address,address_line_2,company,city,state,subscriber_id,country,country_name,
		 country_abbreviation,latitude,longitude,verified,zip_code
	from openxml(@docHandle,N'/ArrayOfAddress/Address')
	with
	(
		address_id int 'address_id',
		account_id int 'account_id',
		first_name varchar(50) 'first_name',
		last_name varchar(50) 'last_name',
		address varchar(50) 'address',
		address_line_2 varchar(50) 'address_line_2',
		company varchar(50) 'company',
		city varchar(50) 'city',
		state varchar(60) 'state',
		subscriber_id int 'subscriber_id',
		country varchar(50) 'country',
		country_name varchar(50) 'country_name',
		country_abbreviation varchar(50) 'country_abbreviation',
		latitude decimal(18, 10) 'latitude',
		longitude decimal(18, 10) 'longitude',
		verified bit 'verified',
		zip_code varchar(12) 'zip_code'
	)
	
	exec sp_xml_removedocument @docHandle

	update ps
		set ps.FirstName = i.first_name,
			ps.LastName = i.last_name,
			ps.Address1 = i.address,
			ps.Address2 = i.address_line_2,
			ps.Company = i.company,
			ps.City = i.city,
			ps.RegionCode = i.state,
			ps.RegionID = (Select RegionID from UAD_Lookup..Region where RegionCode = i.state),
			ps.Country = i.country,
			ps.Latitude = i.latitude,
			ps.Longitude = i.longitude,
			ps.AddressValidationDate = null,
			ps.AddressValidationMessage = '',
			ps.AddressValidationSource = 'SubGen',
			ps.ZipCode = i.zip_code,
			ps.PubTransactionDate = GetDate(),
			ps.PubTransactionID = 21,
			ps.DateUpdated = GetDate(),
			ps.UpdatedByUserID = 1
		from PubSubscriptions ps
			join @import i on ps.SubGenSubscriberID = i.subscriber_id

	update s
		set s.FName = i.first_name,
			s.LName = i.last_name,
			s.Address = i.address,
			s.MAILSTOP = i.address_line_2,
			s.Company = i.company,
			s.City = i.city,
			s.State = i.state,
			s.Country = i.country,
			s.Latitude = i.latitude,
			s.Longitude = i.longitude,
			s.AddressLastUpdatedDate = null,
			s.AddressUpdatedSourceTypeCodeId = null,
			s.IsLatLonValid = null,
			s.LatLonMsg = '',
			s.Zip = i.zip_code,
			s.TransactionDate = GetDate(),
			s.TransactionID = 21,
			s.DateUpdated = GetDate(),
			s.UpdatedByUserID = 1
		from Subscriptions s
			join PubSubscriptions ps on s.SubscriptionID = ps.SubscriptionID
			join @import i on ps.SubGenSubscriberID = i.subscriber_id

END
go