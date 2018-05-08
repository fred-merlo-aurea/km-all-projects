create procedure job_RuleField_Setup
@xml xml,
@clientId int
as
	begin
		set nocount on

		--have an engine that runs and gets list of active clients
		--foreach client call exec o_Get_FileMappingColumns
		--pass results as xml
		--parse xml into a table - add ClientId / IsClientField
		declare @docHandle int
		create table #rfXML
		(
			DataTable varchar(75),
			TablePrefix varchar(5),
			ColumnName varchar(50),
			DataType varchar(50),
			IsMultiSelect bit,
			IsDemographic bit,
			IsDemographicOther bit,
			ClientId int,
			UIControl varchar(50)
		)
		create index EA_1 ON #rfXML (ClientId,DataTable,ColumnName)

		exec sp_xml_preparedocument @docHandle OUTPUT, @xml  

		insert into #rfXML (DataTable,TablePrefix,ColumnName,DataType,IsMultiSelect,IsDemographic,IsDemographicOther,ClientId,UIControl)  
		select  DataTable,TablePrefix,ColumnName,DataType,'false',IsDemographic,IsDemographicOther,@clientId,
		case when DataType = 'bit' then 'DropDownList'
			 when DataType = 'Date' then 'DatePicker'
			 when DataType = 'int' then 'Numeric'
			 else 'TextBox' end
		from openxml(@docHandle, N'/XML/FileMappingColumn')   
		with   
		(  
			DataTable varchar(75) 'DataTable',
			TablePrefix varchar(5) 'TablePrefix',
			ColumnName varchar(50) 'ColumnName',
			DataType varchar(50) 'DataType',
			IsDemographic bit 'IsDemographic',
			IsDemographicOther bit 'IsDemographicOther'
		)
	 
		exec sp_xml_removedocument @docHandle  
		  
		update #rfXML
		set IsMultiSelect = 'true'
		where DataType = 'bit'

		Update #rfXML
		set ClientId = 0
		where DataTable in ('PubSubscriptions','Subscriptions','SubscriberFinal')

		Update #rfXML
		set ClientId = @clientId
		where DataTable = 'PubSubscriptions' and ColumnName = 'PUBCODE'

		--field specific ui control
		Update #rfXML
		set UIControl = 'DropDownList'
		where ColumnName in ('COUNTRY','COUNTRYID','GENDER','PUBCODE','REGIONCODE','ZIPCODE','EMAILSTATUSID','PAR3CID','PUBCATEGORYID','PUBQSOURCEID','PUBTRANSACTIONID','SUBSCRIPTIONSTATUSID') 
		and DataTable = 'PubSubscriptions'

		Update #rfXML
		set UIControl = 'DropDownList'
		where DataTable = 'ResponseGroups'

		update #rfXML
		set IsMultiSelect = 'true'
		where ColumnName in ('EMAILSTATUSID','PAR3CID','PUBCATEGORYID','PUBQSOURCEID','PUBTRANSACTIONID','SUBSCRIPTIONSTATUSID') 
		and DataTable = 'PubSubscriptions'

		Update #rfXML
		set UIControl = 'TextBox'
		where DataTable in ('SubscriptionsExtensionMapper','PubSubscriptionsExtensionMapper')
		
		--RuleField - primaryKey ClientId+DataTable+Field
		Insert Into RuleField (ClientId,IsClientField,DataTable,TablePrefix,Field,DataType,UIControl,IsMultiSelect,IsActive,CreatedDate,CreatedByUserId,UpdatedByUserId)
		select r.ClientId,case when r.ClientId > 0 then 'true' else 'false' end,
			   r.DataTable,r.TablePrefix, r.ColumnName, r.DataType, r.UIControl, r.IsMultiSelect, 'true',getdate(),1,1
		from #rfXML r
		left join RuleField f on r.ClientId = f.ClientId and r.DataTable = f.DataTable and r.ColumnName = f.Field
		where f.ruleFieldId is null
		
		drop table #rfXML
	end
go