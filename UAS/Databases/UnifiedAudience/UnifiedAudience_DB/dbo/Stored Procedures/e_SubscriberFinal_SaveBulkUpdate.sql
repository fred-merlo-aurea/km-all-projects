CREATE PROCEDURE [e_SubscriberFinal_SaveBulkUpdate]
@xml xml
AS
BEGIN
	SET NOCOUNT ON  	

	DECLARE @docHandle int

    declare @insertcount int
    
	DECLARE @import TABLE    
	(  
		tmpImportID int IDENTITY(1,1), 
		SubscriberFinalID int,
		[STRecordIdentifier] [uniqueidentifier] NOT NULL,[SourceFileID] [int] NULL,[PubCode] [varchar](100) NULL,[Sequence] [int] NOT NULL,
		[FName] [varchar](100) NULL,[LName] [varchar](100) NULL,[Title] [varchar](100) NULL,[Company] [varchar](100) NULL,[Address] [varchar](255) NULL,[MailStop] [varchar](255) NULL,
		[City] [varchar](50) NULL,[State] [varchar](50) NULL,[Zip] [varchar](10) NULL,[Plus4] [varchar](4) NULL,[ForZip] [varchar](50) NULL,[County] [varchar](20) NULL,
		[Country] [varchar](100) NULL,[CountryID] [int] NULL,[Phone] [varchar](100) NULL,[PhoneExists] [bit] NULL,[Fax] [varchar](100) NULL,[FaxExists] [bit] NULL,
		[Email] [varchar](100) NULL,[EmailExists] [bit] NULL,[CategoryID] [int] NULL,[TransactionID] [int] NULL,[TransactionDate] [smalldatetime] NULL,[QDate] [datetime] NULL,
		[QSourceID] [int] NULL,[RegCode] [varchar](5) NULL,[Verified] [varchar](100) NULL,[SubSrc] [varchar](8) NULL,[OrigsSrc] [varchar](8) NULL,[Par3C] [varchar](10) NULL,
		[Source] [varchar](50) NULL,[Priority] [varchar](4) NULL,
		[IGrp_No] [uniqueidentifier] NULL,[IGrp_Cnt] [int] NULL,[CGrp_No] [uniqueidentifier] NULL,[CGrp_Cnt] [int] NULL,[StatList] [bit] NULL,[Sic] [varchar](8) NULL,[SicCode] [varchar](20) NULL,
		[Gender] [varchar](1024) NULL,[IGrp_Rank] [varchar](2) NULL,[CGrp_Rank] [varchar](2) NULL,[Address3] [varchar](255) NULL,[Home_Work_Address] [varchar](10) NULL,
		[PubIDs] [varchar](2000) NULL,[Demo7] [varchar](1) NULL,[IsExcluded] [bit] NULL,[Mobile] [varchar](30) NULL,[Latitude] [decimal](18, 15) NULL,[Longitude] [decimal](18, 15) NULL,
		[IsLatLonValid] [bit] NULL,[LatLonMsg] [nvarchar](500) NULL,[Score] [int] NULL,
		[EmailStatusID] [int] NULL,[StatusUpdatedDate] [datetime] NULL,[StatusUpdatedReason] [nvarchar](200) NULL,[IsMailable] [bit] NULL,[Ignore] [bit] NOT NULL,[IsDQMProcessFinished] [bit] NOT NULL,
		[DQMProcessDate] [datetime] NULL,[IsUpdatedInLive] [bit] NOT NULL,[UpdateInLiveDate] [datetime] NULL,SFRecordIdentifier [uniqueidentifier] NOT NULL,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,ProcessCode varchar(50) NOT NULL,ImportRowNumber int NULL,IsActive BIT NULL,
		ExternalKeyId int null,AccountNumber varchar(50) null,EmailID int null,
		Copies int null,GraceIssues int null,IsComp bit null,IsPaid bit null,IsSubscribed bit null,Occupation varchar(50) null,SubscriptionStatusID int null,SubsrcID int null,Website varchar(255) null,
		MailPermission bit null,FaxPermission bit null,PhonePermission bit null,OtherProductsPermission bit null,ThirdPartyPermission bit null,EmailRenewPermission bit null,TextPermission bit null,
		[SubGenSubscriberID] INT NULL, [SubGenSubscriptionID] INT NULL, [SubGenPublicationID] INT NULL, [SubGenMailingAddressId] INT NULL, [SubGenBillingAddressId]	INT NULL, [IssuesLeft] INT NULL,[UnearnedReveue] MONEY NULL,
		[SubGenIsLead] BIT NULL, [SubGenRenewalCode] VARCHAR(50) NULL, [SubGenSubscriptionRenewDate] DATE NULL, [SubGenSubscriptionExpireDate] DATE NULL, [SubGenSubscriptionLastQualifiedDate] DATE NULL
	)  

	--CREATE INDEX EA_1 on #import (SubscriberFinalID)
	
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @xml  

	-- IMPORT FROM XML TO TEMP TABLE
	insert into @import 
	(
		 SubscriberFinalID,STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive,ExternalKeyId,AccountNumber,EmailID,
		 Copies,GraceIssues,IsComp,IsPaid,IsSubscribed,Occupation,SubscriptionStatusID,SubsrcID,Website,MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,
		 EmailRenewPermission,TextPermission,SubGenSubscriberID,SubGenSubscriptionID,SubGenPublicationID,SubGenMailingAddressId,SubGenBillingAddressId,IssuesLeft,UnearnedReveue,
		 SubGenIsLead,SubGenRenewalCode,SubGenSubscriptionRenewDate,SubGenSubscriptionExpireDate,SubGenSubscriptionLastQualifiedDate
	)  
	
	SELECT 
		 SubscriberFinalID,STRecordIdentifier,SourceFileID,PubCode,Sequence,FName,LName,Title,Company,Address,MailStop,City,State,Zip,Plus4,ForZip,County,Country,CountryID,Phone,PhoneExists,
		 Fax,FaxExists,Email,EmailExists,CategoryID,TransactionID,TransactionDate,QDate,QSourceID,RegCode,Verified,SubSrc,OrigsSrc,Par3C,Source,
		 Priority,IGrp_No,IGrp_Cnt,CGrp_No,CGrp_Cnt,StatList,Sic,SicCode,Gender,IGrp_Rank,CGrp_Rank,Address3,Home_Work_Address,PubIDs,Demo7,IsExcluded,Mobile,Latitude,Longitude,IsLatLonValid,
		 LatLonMsg,Score,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,IsMailable,Ignore,IsDQMProcessFinished,DQMProcessDate,IsUpdatedInLive,
		 UpdateInLiveDate,SFRecordIdentifier,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,ProcessCode,ImportRowNumber,IsActive,ExternalKeyId,AccountNumber,EmailID,
		 Copies,GraceIssues,IsComp,IsPaid,IsSubscribed,Occupation,SubscriptionStatusID,SubsrcID,Website,MailPermission,FaxPermission,PhonePermission,OtherProductsPermission,ThirdPartyPermission,
		 EmailRenewPermission,TextPermission,SubGenSubscriberID,SubGenSubscriptionID,SubGenPublicationID,SubGenMailingAddressId,SubGenBillingAddressId,IssuesLeft,UnearnedReveue,
		 SubGenIsLead,SubGenRenewalCode,SubGenSubscriptionRenewDate,SubGenSubscriptionExpireDate,SubGenSubscriptionLastQualifiedDate
	FROM OPENXML(@docHandle, N'/XML/SubscriberFinal')   
	WITH   
	(  
		SubscriberFinalID int 'SubscriberFinalID',
		STRecordIdentifier uniqueidentifier 'STRecordIdentifier',
		SourceFileID int 'SourceFileID',
		PubCode varchar(100) 'PubCode',
		Sequence int 'Sequence',
		FName varchar(100) 'FName',
		LName varchar(100) 'LName',
		Title varchar(100) 'Title',
		Company varchar(100) 'Company',
		Address varchar(255) 'Address',
		MailStop varchar(255) 'MailStop',
		City varchar(50) 'City',
		State varchar(50) 'State',
		Zip varchar(10) 'Zip',
		Plus4 varchar(4) 'Plus4',
		ForZip varchar(50) 'ForZip',
		County varchar(20) 'County',
		Country varchar(100) 'Country',
		CountryID int 'CountryID',
		Phone varchar(100) 'Phone',
		PhoneExists bit 'PhoneExists',
		Fax varchar(100) 'Fax',
		FaxExists bit 'FaxExists',
		Email varchar(100) 'Email',
		EmailExists bit 'EmailExists',
		CategoryID int 'CategoryID',
		TransactionID int 'TransactionID',
		TransactionDate smalldatetime 'TransactionDate',
		QDate datetime 'QDate',
		QSourceID int 'QSourceID',
		RegCode varchar(5) 'RegCode',
		Verified varchar(100) 'Verified',
		SubSrc varchar(8) 'SubSrc',
		OrigsSrc varchar(8) 'OrigsSrc',
		Par3C varchar(10) 'Par3C',
		Source varchar(50) 'Source',
		Priority varchar(4) 'Priority',
		IGrp_No uniqueidentifier 'IGrp_No',
		IGrp_Cnt int 'IGrp_Cnt',
		CGrp_No uniqueidentifier 'CGrp_No',
		CGrp_Cnt int 'CGrp_Cnt',
		StatList bit 'StatList',
		Sic varchar(8) 'Sic',
		SicCode varchar(20) 'SicCode',
		Gender varchar(1024) 'Gender',
		IGrp_Rank varchar(2) 'IGrp_Rank',
		CGrp_Rank varchar(2) 'CGrp_Rank',
		Address3 varchar(255) 'Address3',
		Home_Work_Address varchar(10) 'Home_Work_Address',
		PubIDs varchar(2000) 'PubIDs',
		Demo7 varchar(1) 'Demo7',
		IsExcluded bit 'IsExcluded',
		Mobile varchar(30) 'Mobile',
		Latitude decimal(18, 15) 'Latitude',
		Longitude decimal(18, 15) 'Longitude',
		IsLatLonValid bit 'IsLatLonValid',
		LatLonMsg nvarchar(500) 'LatLonMsg',
		Score int 'Score',
		EmailStatusID int 'EmailStatusID',
		StatusUpdatedDate datetime 'StatusUpdatedDate',
		StatusUpdatedReason nvarchar(200) 'StatusUpdatedReason',
		IsMailable bit 'IsMailable',
		Ignore bit 'Ignore',
		IsDQMProcessFinished bit 'IsDQMProcessFinished',
		DQMProcessDate datetime 'DQMProcessDate',
		IsUpdatedInLive bit 'IsUpdatedInLive',
		UpdateInLiveDate datetime 'UpdateInLiveDate',
		SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		ProcessCode varchar(50) 'ProcessCode',
		ImportRowNumber int 'ImportRowNumber',
		IsActive BIT 'IsActive',
		ExternalKeyId int 'ExternalKeyId',
		AccountNumber varchar(50) 'AccountNumber',
		EmailID int 'EmailID',
		Copies int 'Copies',
		GraceIssues int 'GraceIssues',
		IsComp bit 'IsComp',
		IsPaid bit 'IsPaid',
		IsSubscribed bit 'IsSubscribed',
		Occupation varchar(50) 'Occupation',
		SubscriptionStatusID int 'SubscriptionStatusID',
		SubsrcID int 'SubscrcID',
		Website varchar(255) 'Website',
		MailPermission bit 'MailPermission',
		FaxPermission bit 'FaxPermission',
		PhonePermission bit 'PhonePermission',
		OtherProductsPermission bit 'OtherProductsPermission',
		ThirdPartyPermission bit 'ThirdPartyPermission',
		EmailRenewPermission bit 'EmailRenewPermission',
		TextPermission bit 'TextPermission',
		SubGenSubscriberID int 'SubGenSubscriberID',
		SubGenSubscriptionID int 'SubGenSubscriptionID',
		SubGenPublicationID int 'SubGenPublicationID',
		SubGenMailingAddressId int 'SubGenMailingAddressId',
		SubGenBillingAddressId int 'SubGenBillingAddressId',
		IssuesLeft int 'IssuesLeft',
		UnearnedReveue money 'UnearnedReveue',
		SubGenIsLead bit 'SubGenIsLead',
		SubGenRenewalCode varchar(50) 'SubGenRenewalCode',
		SubGenSubscriptionRenewDate date 'SubGenSubscriptionRenewDate',
		SubGenSubscriptionExpireDate date 'SubGenSubscriptionExpireDate',
		SubGenSubscriptionLastQualifiedDate date 'SubGenSubscriptionLastQualifiedDate'
	)  
	
	
	EXEC sp_xml_removedocument @docHandle    
	
	--------------------SubscriberDemographicFinal
	DECLARE @sdfDocHandle int

    declare @sdfInsertcount int
    
	DECLARE @sdfImport TABLE    
	(  
		SDFinalID int,PubID int, SFRecordIdentifier uniqueidentifier, MAFField varchar(255), Value varchar(max), NotExists bit,
		[DateCreated] [datetime] NULL,[DateUpdated] [datetime] NULL,[CreatedByUserID] [int] NULL,[UpdatedByUserID] int NULL,
		SubscriberOriginalRecordIdentifier [uniqueidentifier]
	)  

	EXEC sp_xml_preparedocument @sdfDocHandle OUTPUT, @xml
	
	insert into @sdfImport 
	(
		 SDFinalID,PubID,SFRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	)  
	
	SELECT SDFinalID,PubID,SFRecordIdentifier,MAFField,Value,NotExists
		 ,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,SubscriberOriginalRecordIdentifier
	FROM OPENXML(@sdfDocHandle, N'/XML/SubscriberFinal/DemographicFinalList/SubscriberDemographicFinal')  
	WITH   
	(
		SDFinalID int 'SDFinalID',  
		PubID int 'PubID',
		SFRecordIdentifier uniqueidentifier 'SFRecordIdentifier',
		MAFField varchar(255) 'MAFField',
		Value varchar(max) 'Value',
		NotExists bit 'NotExists',
		DateCreated datetime 'DateCreated',
		DateUpdated datetime 'DateUpdated',
		CreatedByUserID int 'CreatedByUserID',
		UpdatedByUserID int 'UpdatedByUserID',
		SubscriberOriginalRecordIdentifier uniqueidentifier 'SubscriberOriginalRecordIdentifier' 
	)  
	
	
	EXEC sp_xml_removedocument @sdfDocHandle
	
	
	
	UPDATE SubscriberFinal
	SET STRecordIdentifier = i.STRecordIdentifier,
		SourceFileID = i.SourceFileID,
		PubCode = i.PubCode,
		Sequence = i.Sequence,
		FName = i.FName,
		LName = i.LName,
		Title = i.Title,
		Company = i.Company,
		Address = i.Address,
		MailStop = i.MailStop,
		City = i.City,
		State = i.State,
		Zip = i.Zip,
		Plus4 = i.Plus4,
		ForZip = i.ForZip,
		County = i.County,
		Country = i.Country,
		CountryID = i.CountryID,
		Phone = i.Phone,
		--PhoneExists = i.PhoneExists,
		Fax = i.Fax,
		--FaxExists = i.FaxExists,
		Email = i.Email,
		--EmailExists = i.EmailExists,
		CategoryID = i.CategoryID,
		TransactionID = i.TransactionID,
		TransactionDate = i.TransactionDate,
		QDate = i.QDate,
		QSourceID = i.QSourceID,
		RegCode = i.RegCode,
		Verified = i.Verified,
		SubSrc = i.SubSrc,
		OrigsSrc = i.OrigsSrc,
		Par3C = i.Par3C,		
		Source = i.Source,
		Priority = i.Priority,
		IGrp_No = i.IGrp_No,
		IGrp_Cnt = i.IGrp_Cnt,
		CGrp_No = i.CGrp_No,
		CGrp_Cnt = i.CGrp_Cnt,
		StatList = i.StatList,
		Sic = i.Sic,
		SicCode = i.SicCode,
		Gender = i.Gender,
		IGrp_Rank = i.IGrp_Rank,
		CGrp_Rank = i.CGrp_Rank,
		Address3 = i.Address3,
		Home_Work_Address = i.Home_Work_Address,
		--PubIDs = i.PubIDs,
		Demo7 = i.Demo7,
		---IsExcluded = i.IsExcluded,
		Mobile = i.Mobile,
		Latitude = i.Latitude,
		Longitude = i.Longitude,
		IsLatLonValid = i.IsLatLonValid,
		LatLonMsg = i.LatLonMsg,
		--Score = i.Score,
		EmailStatusID = i.EmailStatusID,
		--StatusUpdatedDate = i.StatusUpdatedDate,
		--StatusUpdatedReason = i.StatusUpdatedReason,
		IsMailable = i.IsMailable,
		Ignore = i.Ignore,
		IsDQMProcessFinished = i.IsDQMProcessFinished,
		DQMProcessDate = i.DQMProcessDate,
		IsUpdatedInLive = i.IsUpdatedInLive,
		UpdateInLiveDate = i.UpdateInLiveDate,
		SFRecordIdentifier = i.SFRecordIdentifier,
		DateUpdated = GETDATE(),
		UpdatedByUserID = 1,
		ImportRowNumber = i.ImportRowNumber,
		ProcessCode = i.ProcessCode,
		IsActive = i.IsActive,
		ExternalKeyId = i.ExternalKeyId,
		AccountNumber = i.AccountNumber,
		EmailID = i.EmailID,
		Copies = i.Copies,
		GraceIssues = i.GraceIssues,
		IsComp = i.IsComp,
		IsPaid = i.IsPaid,
		IsSubscribed = i.IsSubscribed,
		Occupation = i.Occupation,
		SubscriptionStatusID = i.SubscriptionStatusID,
		SubsrcID = i.SubsrcID,
		Website = i.Website,		
		MailPermission = i.MailPermission,
		FaxPermission = i.FaxPermission,
		PhonePermission = i.PhonePermission,
		OtherProductsPermission = i.OtherProductsPermission,
		ThirdPartyPermission = i.ThirdPartyPermission,
		EmailRenewPermission = i.EmailRenewPermission,
		TextPermission = i.TextPermission,
		SubGenSubscriberID = i.SubGenSubscriberID,
		SubGenSubscriptionID = i.SubGenSubscriptionID,
		SubGenPublicationID = i.SubGenPublicationID,
		SubGenMailingAddressId = i.SubGenMailingAddressId,
		SubGenBillingAddressId = i.SubGenBillingAddressId,
		IssuesLeft = i.IssuesLeft,
		UnearnedReveue = i.UnearnedReveue,
		SubGenIsLead = i.SubGenIsLead,
		SubGenRenewalCode = i.SubGenRenewalCode,
		SubGenSubscriptionRenewDate = i.SubGenSubscriptionRenewDate,
		SubGenSubscriptionExpireDate = i.SubGenSubscriptionExpireDate,
		SubGenSubscriptionLastQualifiedDate = i.SubGenSubscriptionLastQualifiedDate
	FROM @import i
	WHERE SubscriberFinal.SubscriberFinalID = i.SubscriberFinalID


	UPDATE SubscriberDemographicFinal
	SET PubID = i.PubID,
		MAFField = i.MAFField,
		Value = i.Value,
		NotExists = i.NotExists,
		DateCreated = i.DateCreated,
		DateUpdated = i.DateUpdated,
		CreatedByUserID = i.CreatedByUserID,
		UpdatedByUserID = i.UpdatedByUserID
	FROM @sdfImport i
	WHERE i.SDFinalID = SubscriberDemographicFinal.SDFinalID
	
	--DROP table #import
	--DROP table #sdfImport

	UPDATE SubscriberFinal
	SET TransactionID = 10
	WHERE TransactionID = 0
	
	UPDATE SubscriberFinal
	SET CategoryID = 10
	WHERE CategoryID = 0

END
GO