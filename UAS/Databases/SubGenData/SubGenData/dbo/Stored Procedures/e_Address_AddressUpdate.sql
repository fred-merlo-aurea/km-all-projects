create procedure e_Address_AddressUpdate
@xml xml
as
BEGIN

	set nocount on  	

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
	from openxml(@docHandle,N'/XML/Address')
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
	
	update @import
	set address = replace(address, '&amp;', '&')

	update @import
	set address = replace(address, '&quot;', '"' )

	update @import
	set address = replace(address, '&lt;', '<')

	update @import
	set address = replace(address, '&gt;', '>')

	update @import
	set address = replace(address, '&apos;', '''')

	update @import
	set address_line_2 = replace(address_line_2, '&amp;', '&')

	update @import
	set address_line_2 = replace(address_line_2, '&quot;', '"' )

	update @import
	set address_line_2 = replace(address_line_2, '&lt;', '<')

	update @import
	set address_line_2 = replace(address_line_2, '&gt;', '>')

	update @import
	set address_line_2 = replace(address_line_2, '&apos;', '''')

	update @import
	set city = replace(city, '&amp;', '&')

	update @import
	set city = replace(city, '&quot;', '"' )

	update @import
	set city = replace(city, '&lt;', '<')

	update @import
	set city = replace(city, '&gt;', '>')

	update @import
	set city = replace(city, '&apos;', '''')

	update @import
	set state = replace(state, '&amp;', '&')

	update @import
	set state = replace(state, '&quot;', '"' )

	update @import
	set state = replace(state, '&lt;', '<')

	update @import
	set state = replace(state, '&gt;', '>')

	update @import
	set state = replace(state, '&apos;', '''')

	update @import
	set country = replace(country, '&amp;', '&')

	update @import
	set country = replace(country, '&quot;', '"' )

	update @import
	set country = replace(country, '&lt;', '<')

	update @import
	set country = replace(country, '&gt;', '>')

	update @import
	set country = replace(country, '&apos;', '''')

	update @import
	set zip_code = replace(zip_code, '&amp;', '&')

	update @import
	set zip_code = replace(zip_code, '&quot;', '"' )

	update @import
	set zip_code = replace(zip_code, '&lt;', '<')

	update @import
	set zip_code = replace(zip_code, '&gt;', '>')

	update @import
	set zip_code = replace(zip_code, '&apos;', '''')

	exec sp_xml_removedocument @docHandle

	
	update a
	set a.address = i.address,
		a.address_line_2 = i.address_line_2,
		a.city = i.city,
		a.state = i.state,
		a.country = i.country,
		a.zip_code = i.zip_code
	from Address a
	join @import i on i.address_id = a.address_id

END
go