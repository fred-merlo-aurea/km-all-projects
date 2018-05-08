CREATE PROCEDURE [o_Get_FileMappingColumns]
WITH EXECUTE AS OWNER
AS
BEGIN

	SET NOCOUNT ON

	SELECT 'PubSubscriptions' as DataTable, 'ps' as TablePrefix, 'PUBCODE' as ColumnName, 'VARCHAR' as DataType,CAST('false' AS BIT) as IsDemographic, CAST('false' AS BIT) as IsDemographicOther, CAST('false' AS BIT) as IsDemographicDate
	UNION
	SELECT 'Subscriptions' as DataTable, 's' as TablePrefix, 'FORZIP' as ColumnName, 'VARCHAR' as DataType,CAST('false' AS BIT) as IsDemographic, CAST('false' AS BIT) as IsDemographicOther, CAST('false' AS BIT) as IsDemographicDate
	UNION        
	SELECT 'PubSubscriptions' as DataTable, 'ps' as TablePrefix,UPPER(sc.name), UPPER(t.name),CAST('false' AS BIT), CAST('false' AS BIT), CAST('false' AS BIT)
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
		CAST('true' AS BIT), CAST('false' AS BIT), CAST('false' AS BIT)
	FROM SubscriptionsExtensionMapper WITH(NOLOCK)
	UNION         
	SELECT DISTINCT 
		'PubSubscriptionsExtensionMapper' as DataTable, 'psem' as TablePrefix,
		UPPER(CustomField), 
		UPPER(CustomFieldDataType),
		CAST('true' AS BIT), CAST('false' AS BIT), CAST('false' AS BIT) 
	FROM  PubSubscriptionsExtensionMapper WITH(NOLOCK)
	WHERE UPPER(CustomField) not like '%_DEMODATE'
	UNION             
	SELECT DISTINCT 
		'ResponseGroups' as DataTable, 'rg' as TablePrefix,
		UPPER(ResponseGroupName), 
		UPPER(isc.DATA_TYPE),
		CAST('true' AS BIT), 
		CAST('false' AS BIT),
		CAST('false' AS BIT)
	FROM ResponseGroups WITH(NOLOCK)
		JOIN INFORMATION_SCHEMA.COLUMNS isc WITH(NOLOCK) ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
	UNION         
	SELECT DISTINCT 
		'ResponseGroups' as DataTable, 'rg' as TablePrefix,
		UPPER(ResponseGroupName) + '_RESPONSEOTHER' , 
		UPPER(isc.DATA_TYPE),
		CAST('true' AS BIT),
		CAST('true' AS BIT),
		CAST('false' AS BIT)
	FROM ResponseGroups WITH(NOLOCK)
		JOIN INFORMATION_SCHEMA.COLUMNS isc WITH(NOLOCK) ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
	where ResponseGroupID IN (SELECT DISTINCT rg.ResponseGroupID FROM Pubs p join ResponseGroups rg on p.PubID = rg.PubID join CodeSheet c on c.ResponseGroupID = rg.ResponseGroupID 
	WHERE isnull(IsCirc,0) = 1 and ISNULL(IsOther,0) = 1)
	UNION
	SELECT DISTINCT 
		'ResponseGroups' as DataTable, 'rg' as TablePrefix,
		UPPER(ResponseGroupName) + '_DEMODATE' , 
		UPPER(isc.DATA_TYPE),
		CAST('true' AS BIT),
		CAST('false' AS BIT),
		CAST('true' AS BIT)
	FROM ResponseGroups WITH(NOLOCK)
		JOIN INFORMATION_SCHEMA.COLUMNS isc WITH(NOLOCK) ON UPPER(isc.COLUMN_NAME) = 'RESPONSEGROUPNAME' AND UPPER(isc.TABLE_NAME) = 'RESPONSEGROUPS' AND UPPER(ResponseGroupName) != 'PUBCODE'
	where ResponseGroupID IN (SELECT DISTINCT rg.ResponseGroupID FROM Pubs p join ResponseGroups rg on p.PubID = rg.PubID join CodeSheet c on c.ResponseGroupID = rg.ResponseGroupID 
	WHERE isnull(IsCirc,0) = 1 and ISNULL(IsOther,0) = 1)
	ORDER BY 1

END