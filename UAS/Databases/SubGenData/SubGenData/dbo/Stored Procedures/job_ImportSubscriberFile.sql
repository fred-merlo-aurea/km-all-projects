CREATE procedure [dbo].[job_ImportSubscriberFile]
@xml xml
as
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		SystemSubscriberID int NULL,
		IsLead bit NULL,
		RenewalCode_CustomerID varchar(500) NULL,
		SubscriberAccountFirstName varchar(500) NULL,
		SubscriberAccountLastName varchar(500) NULL,
		SubscriberEmail varchar(500) NULL,
		SubscriberPhone varchar(500) NULL,
		SubscriberSource varchar(500) NULL,
		SubscriptionGeniusMailingAddressID int NULL,
		MailingAddressFirstName varchar(500) NULL,
		MailingAddressLastName varchar(500) NULL,
		MailingAddressTitle varchar(500) NULL,
		MailingAddressLine1 varchar(500) NULL,
		MailingAddressLine2 varchar(500) NULL,
		MailingAddressCity varchar(500) NULL,
		MailingAddressState varchar(500) NULL,
		MailingAddressZip varchar(500) NULL,
		MailingAddressCountry varchar(500) NULL,
		MailingAddressCompany varchar(500) NULL,
		SystemBillingAddressID int NULL,
		BillingAddressFirstName varchar(500) NULL,
		BillingAddressLastName varchar(500) NULL,
		BillingAddressLine1 varchar(500) NULL,
		BillingAddressLine2 varchar(500) NULL,
		BillingAddressCity varchar(500) NULL,
		BillingAddressState varchar(500) NULL,
		BillingAddressZip varchar(500) NULL,
		BillingAddressCountry varchar(500) NULL,
		BillingAddressCompany varchar(500) NULL,
		PublicationName varchar(500) NULL,
		IssuesLeft int NULL,
		UnearnedRevenue money NULL,
		Copies int NULL,
		SubscriptionID int NULL,
		SubscriptionCreatedDate date NULL,
		SubscriptionRenewDate date NULL,
		SubscriptionExpireDate date NULL,
		SubscriptionLastQualifiedDate date NULL,
		SubscriptionType varchar(500) NULL,
		AuditCategoryName varchar(500) NULL,
		AuditCategoryCode varchar(500) NULL,
		AuditRequestTypeName varchar(500) NULL,
		AuditRequestTypeCode varchar(500) NULL,
		TransactionID int NULL,
		PublicationID int NULL,
		DateCreated datetime default(getdate()) NOT NULL,
		DateUpdated datetime NULL,
		IsMergedToUAD bit NOT NULL,
		DateMergedToUAD datetime NULL,
		account_id int null,
		ParentSubscriptionID int null,
		IsSiteLicenseMaster	bit null,
		IsSiteLicenseSeat bit null
	)  
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT Subscriber
	insert into @import 
	(
		SystemSubscriberID,IsLead,RenewalCode_CustomerID,SubscriberAccountFirstName,SubscriberAccountLastName,SubscriberEmail,SubscriberPhone,
		SubscriberSource,SubscriptionGeniusMailingAddressID,MailingAddressFirstName,MailingAddressLastName,MailingAddressTitle,MailingAddressLine1,MailingAddressLine2,
		MailingAddressCity,MailingAddressState,MailingAddressZip,MailingAddressCountry,MailingAddressCompany,SystemBillingAddressID,BillingAddressFirstName,
		BillingAddressLastName,BillingAddressLine1,BillingAddressLine2,BillingAddressCity,BillingAddressState,BillingAddressZip,BillingAddressCountry,BillingAddressCompany,PublicationName,
		IssuesLeft,UnearnedRevenue,Copies,SubscriptionID,SubscriptionCreatedDate,SubscriptionRenewDate,SubscriptionExpireDate,SubscriptionLastQualifiedDate,
		SubscriptionType,AuditCategoryName,AuditCategoryCode,AuditRequestTypeName,AuditRequestTypeCode,TransactionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,account_id,ParentSubscriptionID,IsSiteLicenseMaster,IsSiteLicenseSeat
	)  
	
	SELECT 
			SystemSubscriberID,IsLead,RenewalCode_CustomerID,SubscriberAccountFirstName,SubscriberAccountLastName,SubscriberEmail,SubscriberPhone,
		SubscriberSource,SubscriptionGeniusMailingAddressID,MailingAddressFirstName,MailingAddressLastName,MailingAddressTitle,MailingAddressLine1,MailingAddressLine2,
		MailingAddressCity,MailingAddressState,MailingAddressZip,MailingAddressCountry,MailingAddressCompany,SystemBillingAddressID,BillingAddressFirstName,
		BillingAddressLastName,BillingAddressLine1,BillingAddressLine2,BillingAddressCity,BillingAddressState,BillingAddressZip,BillingAddressCountry,BillingAddressCompany,PublicationName,
		IssuesLeft,UnearnedRevenue,Copies,SubscriptionID,SubscriptionCreatedDate,SubscriptionRenewDate,SubscriptionExpireDate,SubscriptionLastQualifiedDate,
		SubscriptionType,AuditCategoryName,AuditCategoryCode,AuditRequestTypeName,AuditRequestTypeCode,TransactionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,account_id,ParentSubscriptionID,IsSiteLicenseMaster,IsSiteLicenseSeat
	FROM OPENXML(@docHandle, N'/XML/ImportSubscriber')   
	WITH   
	(  
		SystemSubscriberID int 'SystemSubscriberID',
		IsLead bit 'IsLead',
		RenewalCode_CustomerID varchar(500) 'RenewalCode_CustomerID',
		SubscriberAccountFirstName varchar(500) 'SubscriberAccountFirstName',
		SubscriberAccountLastName varchar(500) 'SubscriberAccountLastName',
		SubscriberEmail varchar(500) 'SubscriberEmail',
		SubscriberPhone varchar(500) 'SubscriberPhone',
		SubscriberSource varchar(500) 'SubscriberSource',
		SubscriptionGeniusMailingAddressID int 'SubscriptionGeniusMailingAddressID',
		MailingAddressFirstName varchar(500) 'MailingAddressFirstName',
		MailingAddressLastName varchar(500) 'MailingAddressLastName',
		MailingAddressTitle varchar(500) 'MailingAddressTitle',
		MailingAddressLine1 varchar(500) 'MailingAddressLine1',
		MailingAddressLine2 varchar(500) 'MailingAddressLine2',
		MailingAddressCity varchar(500) 'MailingAddressCity',
		MailingAddressState varchar(500) 'MailingAddressState',
		MailingAddressZip varchar(500) 'MailingAddressZip',
		MailingAddressCountry varchar(500) 'MailingAddressCountry',
		MailingAddressCompany varchar(500) 'MailingAddressCompany',
		SystemBillingAddressID int 'SystemBillingAddressID',
		BillingAddressFirstName varchar(500) 'BillingAddressFirstName',
		BillingAddressLastName varchar(500) 'BillingAddressLastName',
		BillingAddressLine1 varchar(500) 'BillingAddressLine1',
		BillingAddressLine2 varchar(500) 'BillingAddressLine2',
		BillingAddressCity varchar(500) 'BillingAddressCity',
		BillingAddressState varchar(500) 'BillingAddressState',
		BillingAddressZip varchar(500) 'BillingAddressZip',
		BillingAddressCountry varchar(500) 'BillingAddressCountry',
		BillingAddressCompany varchar(500) 'BillingAddressCompany',
		PublicationName varchar(500) 'PublicationName',
		IssuesLeft int 'IssuesLeft',
		UnearnedRevenue money 'UnearnedRevenue',
		Copies int 'Copies',
		SubscriptionID int 'SubscriptionID',
		SubscriptionCreatedDate date 'SubscriptionCreatedDate',
		SubscriptionRenewDate date 'SubscriptionRenewDate',
		SubscriptionExpireDate date 'SubscriptionExpireDate',
		SubscriptionLastQualifiedDate date 'SubscriptionLastQualifiedDate',
		SubscriptionType varchar(500) 'SubscriptionType',
		AuditCategoryName varchar(500) 'AuditCategoryName',
		AuditCategoryCode varchar(500) 'AuditCategoryCode',
		AuditRequestTypeName varchar(500) 'AuditRequestTypeName',
		AuditRequestTypeCode varchar(500) 'AuditRequestTypeCode',
		TransactionID int 'TransactionID',
		PublicationID int 'PublicationID',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		IsMergedToUAD bit 'IsMergedToUAD',
		DateMergedToUAD datetime 'DateMergedToUAD',
		account_id int 'account_id',
		ParentSubscriptionID int 'ParentSubscriptionID',
		IsSiteLicenseMaster bit 'IsSiteLicenseMaster',
		IsSiteLicenseSeat bit 'IsSiteLicenseSeat'
	)  

	--update for xml special chars
	update @import
	set RenewalCode_CustomerID = replace(RenewalCode_CustomerID, '&amp;', '&')

	update @import
	set RenewalCode_CustomerID = replace(RenewalCode_CustomerID, '&quot;', '"' )

	update @import
	set RenewalCode_CustomerID = replace(RenewalCode_CustomerID, '&lt;', '<')

	update @import
	set RenewalCode_CustomerID = replace(RenewalCode_CustomerID, '&gt;', '>')

	update @import
	set RenewalCode_CustomerID = replace(RenewalCode_CustomerID, '&apos;', '''')

	update @import
	set SubscriberAccountFirstName = replace(SubscriberAccountFirstName, '&amp;', '&')

	update @import
	set SubscriberAccountFirstName = replace(SubscriberAccountFirstName, '&quot;', '"' )

	update @import
	set SubscriberAccountFirstName = replace(SubscriberAccountFirstName, '&lt;', '<')

	update @import
	set SubscriberAccountFirstName = replace(SubscriberAccountFirstName, '&gt;', '>')

	update @import
	set SubscriberAccountFirstName = replace(SubscriberAccountFirstName, '&apos;', '''')

	update @import
	set SubscriberAccountLastName = replace(SubscriberAccountLastName, '&amp;', '&')

	update @import
	set SubscriberAccountLastName = replace(SubscriberAccountLastName, '&quot;', '"' )

	update @import
	set SubscriberAccountLastName = replace(SubscriberAccountLastName, '&lt;', '<')

	update @import
	set SubscriberAccountLastName = replace(SubscriberAccountLastName, '&gt;', '>')

	update @import
	set SubscriberAccountLastName = replace(SubscriberAccountLastName, '&apos;', '''')

	update @import
	set SubscriberEmail = replace(SubscriberEmail, '&amp;', '&')

	update @import
	set SubscriberEmail = replace(SubscriberEmail, '&quot;', '"' )

	update @import
	set SubscriberEmail = replace(SubscriberEmail, '&lt;', '<')

	update @import
	set SubscriberEmail = replace(SubscriberEmail, '&gt;', '>')

	update @import
	set SubscriberEmail = replace(SubscriberEmail, '&apos;', '''')

	update @import
	set SubscriberPhone = replace(SubscriberPhone, '&amp;', '&')

	update @import
	set SubscriberPhone = replace(SubscriberPhone, '&quot;', '"' )

	update @import
	set SubscriberPhone = replace(SubscriberPhone, '&lt;', '<')

	update @import
	set SubscriberPhone = replace(SubscriberPhone, '&gt;', '>')

	update @import
	set SubscriberPhone = replace(SubscriberPhone, '&apos;', '''')

	update @import
	set SubscriberSource = replace(SubscriberSource, '&amp;', '&')

	update @import
	set SubscriberSource = replace(SubscriberSource, '&quot;', '"' )

	update @import
	set SubscriberSource = replace(SubscriberSource, '&lt;', '<')

	update @import
	set SubscriberSource = replace(SubscriberSource, '&gt;', '>')

	update @import
	set SubscriberSource = replace(SubscriberSource, '&apos;', '''')

	update @import
	set MailingAddressFirstName = replace(MailingAddressFirstName, '&amp;', '&')

	update @import
	set MailingAddressFirstName = replace(MailingAddressFirstName, '&quot;', '"' )

	update @import
	set MailingAddressFirstName = replace(MailingAddressFirstName, '&lt;', '<')

	update @import
	set MailingAddressFirstName = replace(MailingAddressFirstName, '&gt;', '>')

	update @import
	set MailingAddressFirstName = replace(MailingAddressFirstName, '&apos;', '''')

	update @import
	set MailingAddressLastName = replace(MailingAddressLastName, '&amp;', '&')

	update @import
	set MailingAddressLastName = replace(MailingAddressLastName, '&quot;', '"' )

	update @import
	set MailingAddressLastName = replace(MailingAddressLastName, '&lt;', '<')

	update @import
	set MailingAddressLastName = replace(MailingAddressLastName, '&gt;', '>')

	update @import
	set MailingAddressLastName = replace(MailingAddressLastName, '&apos;', '''')

	update @import
	set MailingAddressTitle = replace(MailingAddressTitle, '&amp;', '&')

	update @import
	set MailingAddressTitle = replace(MailingAddressTitle, '&quot;', '"' )

	update @import
	set MailingAddressTitle = replace(MailingAddressTitle, '&lt;', '<')

	update @import
	set MailingAddressTitle = replace(MailingAddressTitle, '&gt;', '>')

	update @import
	set MailingAddressTitle = replace(MailingAddressTitle, '&apos;', '''')

	update @import
	set MailingAddressLine1 = replace(MailingAddressLine1, '&amp;', '&')

	update @import
	set MailingAddressLine1 = replace(MailingAddressLine1, '&quot;', '"' )

	update @import
	set MailingAddressLine1 = replace(MailingAddressLine1, '&lt;', '<')

	update @import
	set MailingAddressLine1 = replace(MailingAddressLine1, '&gt;', '>')

	update @import
	set MailingAddressLine1 = replace(MailingAddressLine1, '&apos;', '''')

	update @import
	set MailingAddressLine2 = replace(MailingAddressLine2, '&amp;', '&')

	update @import
	set MailingAddressLine2 = replace(MailingAddressLine2, '&quot;', '"' )

	update @import
	set MailingAddressLine2 = replace(MailingAddressLine2, '&lt;', '<')

	update @import
	set MailingAddressLine2 = replace(MailingAddressLine2, '&gt;', '>')

	update @import
	set MailingAddressLine2 = replace(MailingAddressLine2, '&apos;', '''')

	update @import
	set MailingAddressCity = replace(MailingAddressCity, '&amp;', '&')

	update @import
	set MailingAddressCity = replace(MailingAddressCity, '&quot;', '"' )

	update @import
	set MailingAddressCity = replace(MailingAddressCity, '&lt;', '<')

	update @import
	set MailingAddressCity = replace(MailingAddressCity, '&gt;', '>')

	update @import
	set MailingAddressCity = replace(MailingAddressCity, '&apos;', '''')

	update @import
	set MailingAddressState = replace(MailingAddressState, '&amp;', '&')

	update @import
	set MailingAddressState = replace(MailingAddressState, '&quot;', '"' )

	update @import
	set MailingAddressState = replace(MailingAddressState, '&lt;', '<')

	update @import
	set MailingAddressState = replace(MailingAddressState, '&gt;', '>')

	update @import
	set MailingAddressState = replace(MailingAddressState, '&apos;', '''')

	update @import
	set MailingAddressZip = replace(MailingAddressZip, '&amp;', '&')

	update @import
	set MailingAddressZip = replace(MailingAddressZip, '&quot;', '"' )

	update @import
	set MailingAddressZip = replace(MailingAddressZip, '&lt;', '<')

	update @import
	set MailingAddressZip = replace(MailingAddressZip, '&gt;', '>')

	update @import
	set MailingAddressZip = replace(MailingAddressZip, '&apos;', '''')

	update @import
	set MailingAddressCompany = replace(MailingAddressCompany, '&amp;', '&')

	update @import
	set MailingAddressCompany = replace(MailingAddressCompany, '&quot;', '"' )

	update @import
	set MailingAddressCompany = replace(MailingAddressCompany, '&lt;', '<')

	update @import
	set MailingAddressCompany = replace(MailingAddressCompany, '&gt;', '>')

	update @import
	set MailingAddressCompany = replace(MailingAddressCompany, '&apos;', '''')

	update @import
	set BillingAddressFirstName = replace(BillingAddressFirstName, '&amp;', '&')

	update @import
	set BillingAddressFirstName = replace(BillingAddressFirstName, '&quot;', '"' )

	update @import
	set BillingAddressFirstName = replace(BillingAddressFirstName, '&lt;', '<')

	update @import
	set BillingAddressFirstName = replace(BillingAddressFirstName, '&gt;', '>')

	update @import
	set BillingAddressFirstName = replace(BillingAddressFirstName, '&apos;', '''')

	update @import
	set BillingAddressLastName = replace(BillingAddressLastName, '&amp;', '&')

	update @import
	set BillingAddressLastName = replace(BillingAddressLastName, '&quot;', '"' )

	update @import
	set BillingAddressLastName = replace(BillingAddressLastName, '&lt;', '<')

	update @import
	set BillingAddressLastName = replace(BillingAddressLastName, '&gt;', '>')

	update @import
	set BillingAddressLastName = replace(BillingAddressLastName, '&apos;', '''')

	update @import
	set BillingAddressLine1 = replace(BillingAddressLine1, '&amp;', '&')

	update @import
	set BillingAddressLine1 = replace(BillingAddressLine1, '&quot;', '"' )

	update @import
	set BillingAddressLine1 = replace(BillingAddressLine1, '&lt;', '<')

	update @import
	set BillingAddressLine1 = replace(BillingAddressLine1, '&gt;', '>')

	update @import
	set BillingAddressLine1 = replace(BillingAddressLine1, '&apos;', '''')

	update @import
	set BillingAddressLine2 = replace(BillingAddressLine2, '&amp;', '&')

	update @import
	set BillingAddressLine2 = replace(BillingAddressLine2, '&quot;', '"' )

	update @import
	set BillingAddressLine2 = replace(BillingAddressLine2, '&lt;', '<')

	update @import
	set BillingAddressLine2 = replace(BillingAddressLine2, '&gt;', '>')

	update @import
	set BillingAddressLine2 = replace(BillingAddressLine2, '&apos;', '''')

	update @import
	set BillingAddressCity = replace(BillingAddressCity, '&amp;', '&')

	update @import
	set BillingAddressCity = replace(BillingAddressCity, '&quot;', '"' )

	update @import
	set BillingAddressCity = replace(BillingAddressCity, '&lt;', '<')

	update @import
	set BillingAddressCity = replace(BillingAddressCity, '&gt;', '>')

	update @import
	set BillingAddressCity = replace(BillingAddressCity, '&apos;', '''')

	update @import
	set BillingAddressState = replace(BillingAddressState, '&amp;', '&')

	update @import
	set BillingAddressState = replace(BillingAddressState, '&quot;', '"' )

	update @import
	set BillingAddressState = replace(BillingAddressState, '&lt;', '<')

	update @import
	set BillingAddressState = replace(BillingAddressState, '&gt;', '>')

	update @import
	set BillingAddressState = replace(BillingAddressState, '&apos;', '''')

	update @import
	set BillingAddressZip = replace(BillingAddressZip, '&amp;', '&')

	update @import
	set BillingAddressZip = replace(BillingAddressZip, '&quot;', '"' )

	update @import
	set BillingAddressZip = replace(BillingAddressZip, '&lt;', '<')

	update @import
	set BillingAddressZip = replace(BillingAddressZip, '&gt;', '>')

	update @import
	set BillingAddressZip = replace(BillingAddressZip, '&apos;', '''')

	update @import
	set BillingAddressCompany = replace(BillingAddressCompany, '&amp;', '&')

	update @import
	set BillingAddressCompany = replace(BillingAddressCompany, '&quot;', '"' )

	update @import
	set BillingAddressCompany = replace(BillingAddressCompany, '&lt;', '<')

	update @import
	set BillingAddressCompany = replace(BillingAddressCompany, '&gt;', '>')

	update @import
	set BillingAddressCompany = replace(BillingAddressCompany, '&apos;', '''')

	update @import
	set PublicationName = replace(PublicationName, '&amp;', '&')

	update @import
	set PublicationName = replace(PublicationName, '&quot;', '"' )

	update @import
	set PublicationName = replace(PublicationName, '&lt;', '<')

	update @import
	set PublicationName = replace(PublicationName, '&gt;', '>')

	update @import
	set PublicationName = replace(PublicationName, '&apos;', '''')

	update @import
	set SubscriptionType = replace(SubscriptionType, '&amp;', '&')

	update @import
	set SubscriptionType = replace(SubscriptionType, '&quot;', '"' )

	update @import
	set SubscriptionType = replace(SubscriptionType, '&lt;', '<')

	update @import
	set SubscriptionType = replace(SubscriptionType, '&gt;', '>')

	update @import
	set SubscriptionType = replace(SubscriptionType, '&apos;', '''')

	update @import
	set AuditCategoryName = replace(AuditCategoryName, '&amp;', '&')

	update @import
	set AuditCategoryName = replace(AuditCategoryName, '&quot;', '"' )

	update @import
	set AuditCategoryName = replace(AuditCategoryName, '&lt;', '<')

	update @import
	set AuditCategoryName = replace(AuditCategoryName, '&gt;', '>')

	update @import
	set AuditCategoryName = replace(AuditCategoryName, '&apos;', '''')

	update @import
	set AuditCategoryCode = replace(AuditCategoryCode, '&amp;', '&')

	update @import
	set AuditCategoryCode = replace(AuditCategoryCode, '&quot;', '"' )

	update @import
	set AuditCategoryCode = replace(AuditCategoryCode, '&lt;', '<')

	update @import
	set AuditCategoryCode = replace(AuditCategoryCode, '&gt;', '>')

	update @import
	set AuditCategoryCode = replace(AuditCategoryCode, '&apos;', '''')

	update @import
	set AuditRequestTypeName = replace(AuditRequestTypeName, '&amp;', '&')

	update @import
	set AuditRequestTypeName = replace(AuditRequestTypeName, '&quot;', '"' )

	update @import
	set AuditRequestTypeName = replace(AuditRequestTypeName, '&lt;', '<')

	update @import
	set AuditRequestTypeName = replace(AuditRequestTypeName, '&gt;', '>')

	update @import
	set AuditRequestTypeName = replace(AuditRequestTypeName, '&apos;', '''')

	update @import
	set AuditRequestTypeCode = replace(AuditRequestTypeCode, '&amp;', '&')

	update @import
	set AuditRequestTypeCode = replace(AuditRequestTypeCode, '&quot;', '"' )

	update @import
	set AuditRequestTypeCode = replace(AuditRequestTypeCode, '&lt;', '<')

	update @import
	set AuditRequestTypeCode = replace(AuditRequestTypeCode, '&gt;', '>')

	update @import
	set AuditRequestTypeCode = replace(AuditRequestTypeCode, '&apos;', '''')

	update @import
	set BillingAddressCountry = replace(BillingAddressCountry, '&amp;', '&')

	update @import
	set BillingAddressCountry = replace(BillingAddressCountry, '&quot;', '"' )

	update @import
	set BillingAddressCountry = replace(BillingAddressCountry, '&lt;', '<')

	update @import
	set BillingAddressCountry = replace(BillingAddressCountry, '&gt;', '>')

	update @import
	set BillingAddressCountry = replace(BillingAddressCountry, '&apos;', '''')

	update @import
	set MailingAddressCountry = replace(MailingAddressCountry, '&amp;', '&')

	update @import
	set MailingAddressCountry = replace(MailingAddressCountry, '&quot;', '"' )

	update @import
	set MailingAddressCountry = replace(MailingAddressCountry, '&lt;', '<')

	update @import
	set MailingAddressCountry = replace(MailingAddressCountry, '&gt;', '>')

	update @import
	set MailingAddressCountry = replace(MailingAddressCountry, '&apos;', '''')


	--import Dimensions
	DECLARE @importDim TABLE    
	(  
		ImportDimensionId int default(0) NOT NULL,
		SystemSubscriberID int NULL,
		SubscriptionID int NULL,
		PublicationID int NULL,
		DateUpdated datetime NULL,
		IsMergedToUAD bit NOT NULL DEFAULT ('false'),
		DateMergedToUAD datetime NULL,
		DateCreated datetime NOT NULL DEFAULT (getdate()),
		account_id int 
	)

	insert into @importDim 
	(
		SystemSubscriberID,SubscriptionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,DateCreated,account_id
	)
	SELECT 
			SystemSubscriberID,SubscriptionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,DateCreated,account_id
	FROM OPENXML(@docHandle, N'/XML/ImportSubscriber/Dimensions')   
	WITH   
	(  
		ImportDimensionId int 'ImportDimensionId',
		SystemSubscriberID int 'SystemSubscriberID',
		SubscriptionID int 'SubscriptionID',
		PublicationID int 'PublicationID',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		IsMergedToUAD bit 'IsMergedToUAD',
		DateMergedToUAD datetime 'DateMergedToUAD',
		account_id int 'account_id'
	)  

	--import DimensionDetail
	DECLARE @importDimDetail TABLE    
	(  
		ImportDimensionId int not null,
		SystemSubscriberID int NULL,
		SubscriptionID int NULL,
		DimensionField varchar(500) null,
		DimensionValue varchar(500) null,
		PublicationID int not null
	)

	insert into @importDimDetail 
	(
		ImportDimensionId,
		SystemSubscriberID,
		SubscriptionID,
		DimensionField,
		DimensionValue,
		PublicationID
	)
	SELECT 
			ImportDimensionId,SystemSubscriberID,SubscriptionID,DimensionField,DimensionValue,PublicationID
	FROM OPENXML(@docHandle, N'/XML/ImportSubscriber/Dimensions/Dimensions/ImportDimensionDetail')   
	WITH   
	(  
		ImportDimensionId int 'ImportDimensionId',
		SystemSubscriberID int 'SystemSubscriberID',
		SubscriptionID int 'SubscriptionID',
		DimensionField varchar(500) 'DimensionField',
		DimensionValue varchar(500) 'DimensionValue',
		PublicationID int 'PublicationID'
	)  

	EXEC sp_xml_removedocument @docHandle  
	
	--Insert or Update to ImportSubscriber 
	update s
	set 
		s.IsLead = i.IsLead,
		s.RenewalCode_CustomerID = i.RenewalCode_CustomerID,
		s.SubscriberAccountFirstName = i.SubscriberAccountFirstName,
		s.SubscriberAccountLastName = i.SubscriberAccountLastName,
		s.SubscriberEmail = i.SubscriberEmail,
		s.SubscriberPhone = i.SubscriberPhone,
		s.SubscriberSource = i.SubscriberSource,
		s.SubscriptionGeniusMailingAddressID = i.SubscriptionGeniusMailingAddressID,
		s.MailingAddressFirstName = i.MailingAddressFirstName,
		s.MailingAddressLastName = i.MailingAddressLastName,
		s.MailingAddressTitle = i.MailingAddressTitle,
		s.MailingAddressLine1 = i.MailingAddressLine1,
		s.MailingAddressLine2 = i.MailingAddressLine2,
		s.MailingAddressCity = i.MailingAddressCity,
		s.MailingAddressState = i.MailingAddressState,
		s.MailingAddressZip = i.MailingAddressZip,
		s.MailingAddressCountry = i.MailingAddressCountry,
		s.MailingAddressCompany = i.MailingAddressCompany,
		s.SystemBillingAddressID = i.SystemBillingAddressID,
		s.BillingAddressFirstName = i.BillingAddressFirstName,
		s.BillingAddressLastName= i.BillingAddressLastName,
		s.BillingAddressLine1 = i.BillingAddressLine1,
		s.BillingAddressLine2 = i.BillingAddressLine2,
		s.BillingAddressCity = i.BillingAddressCity,
		s.BillingAddressState = i.BillingAddressState,
		s.BillingAddressZip= i.BillingAddressZip,
		s.BillingAddressCountry = i.BillingAddressCountry,
		s.BillingAddressCompany = i.BillingAddressCompany,
		s.PublicationName = i.PublicationName,
		s.IssuesLeft = i.IssuesLeft,
		s.UnearnedRevenue= i.UnearnedRevenue,
		s.Copies = i.Copies,
		s.SubscriptionCreatedDate = i.SubscriptionCreatedDate,
		s.SubscriptionRenewDate = i.SubscriptionRenewDate,
		s.SubscriptionExpireDate = i.SubscriptionExpireDate,
		s.SubscriptionLastQualifiedDate = i.SubscriptionLastQualifiedDate,
		s.SubscriptionType = i.SubscriptionType,
		s.AuditCategoryName = i.AuditCategoryName,
		s.AuditCategoryCode = i.AuditCategoryCode,
		s.AuditRequestTypeName = i.AuditRequestTypeName,
		s.AuditRequestTypeCode = i.AuditRequestTypeCode,
		s.TransactionID = i.TransactionID,
		s.PublicationID = i.PublicationID,
		s.DateUpdated = GETDATE(),
		s.IsMergedToUAD = 'false',
		s.DateMergedToUAD = null,
		s.account_id = i.account_id,
		s.ParentSubscriptionID = i.ParentSubscriptionID,
		s.IsSiteLicenseMaster = i.IsSiteLicenseMaster,
		s.IsSiteLicenseSeat = i.IsSiteLicenseSeat
	from ImportSubscriber s
	join @import i on i.SystemSubscriberID = s.SystemSubscriberID and i.SubscriptionID = s.SubscriptionID

	--ImportSubscriber - keys:  SystemSubscriberID, SubscriptionID
	insert into ImportSubscriber
           (SystemSubscriberID,IsLead,RenewalCode_CustomerID,SubscriberAccountFirstName,SubscriberAccountLastName,SubscriberEmail,SubscriberPhone,
		    SubscriberSource,SubscriptionGeniusMailingAddressID,MailingAddressFirstName,MailingAddressLastName,MailingAddressTitle,MailingAddressLine1,MailingAddressLine2,
			MailingAddressCity,MailingAddressState,MailingAddressZip,MailingAddressCountry,MailingAddressCompany,SystemBillingAddressID,BillingAddressFirstName,BillingAddressLastName,
			BillingAddressLine1,BillingAddressLine2,BillingAddressCity,BillingAddressState,BillingAddressZip,BillingAddressCountry,BillingAddressCompany,PublicationName,IssuesLeft,UnearnedRevenue,
			Copies,SubscriptionID,SubscriptionCreatedDate,SubscriptionRenewDate,SubscriptionExpireDate,SubscriptionLastQualifiedDate,SubscriptionType,
			AuditCategoryName,AuditCategoryCode,AuditRequestTypeName,AuditRequestTypeCode,TransactionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,account_id,ParentSubscriptionID,IsSiteLicenseMaster,IsSiteLicenseSeat)
     select 
			i.SystemSubscriberID,i.IsLead,i.RenewalCode_CustomerID,i.SubscriberAccountFirstName,i.SubscriberAccountLastName,i.SubscriberEmail,i.SubscriberPhone,
		    i.SubscriberSource,i.SubscriptionGeniusMailingAddressID,i.MailingAddressFirstName,i.MailingAddressLastName,i.MailingAddressTitle,i.MailingAddressLine1,i.MailingAddressLine2,
			i.MailingAddressCity,i.MailingAddressState,i.MailingAddressZip,i.MailingAddressCountry,i.MailingAddressCompany,i.SystemBillingAddressID,i.BillingAddressFirstName,i.BillingAddressLastName,
			i.BillingAddressLine1,i.BillingAddressLine2,i.BillingAddressCity,i.BillingAddressState,i.BillingAddressZip,i.BillingAddressCountry,i.BillingAddressCompany,i.PublicationName,i.IssuesLeft,i.UnearnedRevenue,
			i.Copies,i.SubscriptionID,i.SubscriptionCreatedDate,i.SubscriptionRenewDate,i.SubscriptionExpireDate,i.SubscriptionLastQualifiedDate,i.SubscriptionType,
			i.AuditCategoryName,i.AuditCategoryCode,i.AuditRequestTypeName,i.AuditRequestTypeCode,i.TransactionID,i.PublicationID,i.DateUpdated,i.IsMergedToUAD,i.DateMergedToUAD,i.account_id,i.ParentSubscriptionID,i.IsSiteLicenseMaster,i.IsSiteLicenseSeat
	from @import i
	left outer join ImportSubscriber s on i.SystemSubscriberID = s.SystemSubscriberID and i.SubscriptionID = s.SubscriptionID
	where s.SubscriptionID is null
	
	--Insert or Update ImportDimension 
	update d
	set 
		d.DateUpdated = GETDATE(),
		d.IsMergedToUAD = 'false',
		d.DateMergedToUAD = null
	from ImportDimension d
	join @importDim i on i.SystemSubscriberID = d.SystemSubscriberID and i.SubscriptionID = d.SubscriptionID and i.PublicationID = d.PublicationID

	--ImportDimension - keys:  SystemSubscriberID, SubscriptionID, PublicationID
	insert into ImportDimension
           (SystemSubscriberID,SubscriptionID,PublicationID,DateUpdated,IsMergedToUAD,DateMergedToUAD,DateCreated,account_id)
     select 
			i.SystemSubscriberID,i.SubscriptionID,i.PublicationID,i.DateUpdated,i.IsMergedToUAD,i.DateMergedToUAD,GETDATE(),i.account_id
	from @importDim i
	left outer join ImportDimension d on i.SystemSubscriberID = d.SystemSubscriberID and i.SubscriptionID = d.SubscriptionID and i.PublicationID = d.PublicationID
	where d.SubscriptionID is null

	--Insert or Update ImportDimensionDetail
	insert into ImportDimensionDetail
           (ImportDimensionId,SystemSubscriberID,SubscriptionID,PublicationID,DimensionValue,DimensionField)
     select 
			0, i.SystemSubscriberID,i.SubscriptionID,i.PublicationID,i.DimensionValue,i.DimensionField
	from @importDimDetail i
	
	update dd
	set dd.ImportDimensionId = d.ImportDimensionId
	from ImportDimensionDetail dd
	join ImportDimension d on dd.SystemSubscriberID = d.SystemSubscriberID and dd.SubscriptionID = d.SubscriptionID and dd.PublicationID = d.PublicationID
	where cast(d.DateCreated as date) = cast(GETDATE() as date)

END
go 