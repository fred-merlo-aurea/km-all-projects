create procedure o_Get_FileMappingColumns_ClientId_PubId
@clientId int = 0, 
@pubId int
as
	begin
		set nocount on
		declare @ruleField table
		(
			Id int identity(1,1),
			DataTable varchar(75),
			TablePrefix varchar(5),
			ColumnName varchar(50),
			DataType varchar(50),
			IsDemographic bit,
			IsDemographicOther bit,
			IsMultiSelect bit,
			ClientId int,
			UxControl varchar(50)
		)

		insert into @ruleField (DataTable,TablePrefix,ColumnName,DataType,IsDemographic,IsDemographicOther,IsMultiSelect,ClientId,UxControl)
		SELECT 'PubSubscriptions' as DataTable, 'ps' as TablePrefix, 'PUBCODE' as ColumnName, 'VARCHAR' as DataType,CAST('false' AS BIT) as IsDemographic, CAST('false' AS BIT) as IsDemographicOther,'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			UNION
			SELECT 'Subscriptions' as DataTable, 's' as TablePrefix, 'FORZIP' as ColumnName, 'VARCHAR' as DataType,CAST('false' AS BIT) as IsDemographic, CAST('false' AS BIT) as IsDemographicOther,'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			UNION        
			SELECT 'PubSubscriptions' as DataTable, 'ps' as TablePrefix,UPPER(sc.name), UPPER(t.name),CAST('false' AS BIT), CAST('false' AS BIT),'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			FROM SysObjects so WITH(NOLOCK)
				INNER JOIN SysColumns sc WITH(NOLOCK) ON so.id = sc.id
				INNER JOIN Sys.Types t WITH(NOLOCK) ON t.system_type_id = sc.xtype
			WHERE so.name = 'PubSubscriptions'
				AND LOWER(sc.name) NOT IN (
					'AddRemoveID',
					'AddressLastUpdatedDate',
					'AddressTypeCodeId',
					'AddressTypeID',
					'AddressUpdatedSourceTypeCodeId',
					'AddressValidationDate',
					'AddressValidationMessage',
					'AddressValidationSource',
					'Age',
					'Birthdate',
					'CarrierRoute',
					'CreatedByUserID',
					'DateCreated',
					'DateUpdated',
					'IMBSEQ',
					'Income',
					'IsActive',
					'IsAddressValidated',
					'IsInActiveWaveMailing',
					'IsLocked',
					'LockDate',
					'LockDateRelease',
					'LockedByUserID',
					'MemberGroup',
					'OnBehalfOf',
					'PhoneExt',
					'PubID',
					'PubSubscriptionID',
					'RegionID',
					'ReqFlag',
					'SFRecordIdentifier',
					'Status',
					'StatusUpdatedDate',
					'StatusUpdatedReason',
					'SubscriptionID',
					'tmpSubscriptionID',
					'UpdatedByUserID',
					'WaveMailingID',
					'IGRP_NO',
					'SubGenSubscriberID',
					'SubGenSubscriptionID',
					'SubGenPublicationID',
					'SubGenMailingAddressId',
					'SubGenBillingAddressId',
					'IssuesLeft',
					'UnearnedReveue' 
					)
			UNION     
			SELECT DISTINCT  
				'SubscriptionsExtensionMapper' as DataTable, 'sem' as TablePrefix,
				UPPER(CustomField), 
				UPPER(CustomFieldDataType),
				CAST('true' AS BIT), CAST('false' AS BIT),'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			FROM SubscriptionsExtensionMapper WITH(NOLOCK)
			UNION         
			SELECT DISTINCT 
				'PubSubscriptionsExtensionMapper' as DataTable, 'psem' as TablePrefix,
				UPPER(CustomField), 
				UPPER(CustomFieldDataType),
				CAST('true' AS BIT), CAST('false' AS BIT),'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			FROM  PubSubscriptionsExtensionMapper WITH(NOLOCK) 
			where PubID = @pubId

			UNION             
			SELECT DISTINCT 
				'ResponseGroups' as DataTable, 'rg' as TablePrefix,
				UPPER(ResponseGroupName), 
				UPPER(isc.DATA_TYPE),
				CAST('true' AS BIT), 
				CAST('false' AS BIT),'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			FROM ResponseGroups WITH(NOLOCK)
				JOIN INFORMATION_SCHEMA.COLUMNS isc WITH(NOLOCK) ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
				and PubID = @pubId
			UNION         
			SELECT DISTINCT 
				'ResponseGroups' as DataTable, 'rg' as TablePrefix,
				UPPER(ResponseGroupName) + '_RESPONSEOTHER' , 
				UPPER(isc.DATA_TYPE),
				CAST('true' AS BIT),
				CAST('true' AS BIT),'false' as 'IsMultiSelect',@clientId as 'ClientId','textbox' as 'UxControl'
			FROM ResponseGroups WITH(NOLOCK)
				JOIN INFORMATION_SCHEMA.COLUMNS isc WITH(NOLOCK) ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
			where ResponseGroupID IN (
										  SELECT DISTINCT rg.ResponseGroupID 
										  FROM Pubs p 
										  join ResponseGroups rg on p.PubID = rg.PubID 
										  join CodeSheet c on c.ResponseGroupID = rg.ResponseGroupID 
										  WHERE isnull(IsCirc,0) = 1 
										  and ISNULL(IsOther,0) = 1
										  and p.PubID = @pubId
									  )
			and PubID = @pubId
			ORDER BY 1

		update @ruleField
		set IsMultiSelect = 'true'
		where DataType = 'bit'

		Update @ruleField
		set ClientId = 0
		where DataTable in ('PubSubscriptions','Subscriptions','SubscriberFinal')

		Update @ruleField
		set ClientId = @clientId
		where DataTable = 'PubSubscriptions' and ColumnName = 'PUBCODE'

		--field specific ui control
		Update @ruleField
		set UxControl = 'DropDownList'
		where ColumnName in ('COUNTRY','COUNTRYID','GENDER','PUBCODE','REGIONCODE','ZIPCODE','EMAILSTATUSID','PAR3CID','PUBCATEGORYID','PUBQSOURCEID','PUBTRANSACTIONID','SUBSCRIPTIONSTATUSID') 
		and DataTable = 'PubSubscriptions'

		Update @ruleField
		set UxControl = 'DropDownList'
		where DataTable = 'ResponseGroups'

		update @ruleField
		set IsMultiSelect = 'true'
		where ColumnName in ('EMAILSTATUSID','PAR3CID','PUBCATEGORYID','PUBQSOURCEID','PUBTRANSACTIONID','SUBSCRIPTIONSTATUSID') 
		and DataTable = 'PubSubscriptions'

		Update @ruleField
		set UxControl = 'TextBox'
		where DataTable in ('SubscriptionsExtensionMapper','PubSubscriptionsExtensionMapper')

		select * from @ruleField order by UxControl
	end
go
