CREATE PROCEDURE [dbo].[e_ProductSubscription_Search_Params]
@fName varchar(100) = '',
@lName varchar(100) = '',
@Company varchar(100) = '',
@Title varchar(255) = '',
@Add1 varchar(100) = '',
@City varchar(50) = '',
@RegionCode varchar(50) = '',
@Zip varchar(50) = '',
@Country varchar(50) = '',
@Email varchar(255) = '',
@Phone varchar(50) = '',
@SequenceID int,
@AccountID varchar(50)='',
@PublisherID int = 0,
@PublicationID int,
@ClientDisplayName varchar(100),
@SubscriptionID int = 0

AS
BEGIN

	set nocount on

	CREATE table #Indiv  
	(
		[PubSubscriptionID]   INT NULL,
		[SubscriptionID]      INT           NULL,
		[PubID]               INT           NULL,
		[Demo7]               VARCHAR (1)   NULL,
		[QualificationDate]   DATE          NULL,
		[PubQSourceID]        INT           NULL,
		[PubCategoryID]       INT           NULL,
		[PubTransactionID]    INT           NULL,
		[EmailStatusID]       INT           NULL,
		[StatusUpdatedDate]   DATETIME      NULL,
		[StatusUpdatedReason] VARCHAR (200) NULL,
		[Email]               VARCHAR (100) NULL,
		[DateCreated]        DATETIME    NULL,
		[DateUpdated]        DATETIME         NULL,
		[CreatedByUserID]    INT              NULL,
		[UpdatedByUserID]    INT              NULL,
		[IsComp]			 BIT			  NULL DEFAULT 0,
		[SubscriptionStatusID] INT			  NULL DEFAULT 1,
		[AccountNumber]			varchar(50) 	NULL,
		[AddRemoveID]			INT			NULL DEFAULT 0,
		[Copies]				INT			NULL DEFAULT 1,
		[GraceIssues]			INT			NULL DEFAULT 0,
		[IMBSEQ]				VARCHAR(256) NULL,
		[IsActive]				BIT NULL,
		[IsPaid]				BIT NULL,
		[IsSubscribed]			BIT NULL,
		[MemberGroup]			VARCHAR(256) NULL,
		[OnBehalfOf]			VARCHAR(256) null,
		[OrigsSrc]				VARCHAR(256) null,
		[Par3CID]				INT null,
		[SequenceID]			INT NULL,
		[Status]				VARCHAR(50) NULL,
		[SubscriberSourceCode]		VARCHAR(100) NULL,
		[SubSrcID]				INT NULL,
		[Verify]				varchar(100) null,
		[ExternalKeyID]            INT              NULL,
		[FirstName]                VARCHAR (50)     NULL,
		[LastName]                 VARCHAR (50)     NULL,
		[Company]                  VARCHAR (100)    NULL,
		[Title]                    VARCHAR (255)    NULL,
		[Occupation]               VARCHAR (50)     NULL,
		[AddressTypeID]            INT              NULL,
		[Address1]                 VARCHAR (256)    NULL,
		[Address2]                 VARCHAR (256)    NULL,
		[Address3]                 VARCHAR (256)    NULL,
		[City]                     VARCHAR (50)     NULL,
		[RegionCode]               VARCHAR (50)     NULL,
		[RegionID]                 INT              NULL,
		[ZipCode]                  VARCHAR (50)     NULL,
		[Plus4]                    VARCHAR (10)        NULL,
		[CarrierRoute]             VARCHAR (10)     NULL,
		[County]                   VARCHAR (50)     NULL,
		[Country]                  VARCHAR (50)     NULL,
		[CountryID]                INT              NULL,
		[Latitude]                 DECIMAL (18, 15) NULL,
		[Longitude]                DECIMAL (18, 15) NULL,
		[IsAddressValidated]       BIT              NULL,
		[AddressValidationDate]    DATETIME         NULL,
		[AddressValidationSource]  VARCHAR (50)     NULL,
		[AddressValidationMessage] VARCHAR (MAX)    NULL,
		[Phone]                    VARCHAR (100)     NULL,
		[Fax]                      VARCHAR (100)     NULL,
		[Mobile]                   VARCHAR (100)     NULL,
		[Website]                  VARCHAR (255)    NULL,
		[Birthdate]                DATE             NULL,
		[Age]                      INT              NULL,
		[Income]                   VARCHAR (50)     NULL,
		[Gender]                   VARCHAR (50)     NULL,
		[tmpSubscriptionID]        INT              NULL,
		[IsLocked]                 BIT              DEFAULT ((0)) NULL,
		[LockedByUserID]		   INT				NULL,
		[LockDate]				   DATETIME			NULL,
		[LockDateRelease]		   DATETIME         NULL,
		[PhoneExt]				   VARCHAR(25)	    NULL, 
		[IsInActiveWaveMailing]	   BIT				DEFAULT ((0)) NULL,
		AddressTypeCodeId int null,
		AddressLastUpdatedDate datetime null,
		AddressUpdatedSourceTypeCodeId int null,
		[WaveMailingID] INT NULL, 
		[IGrp_No] UNIQUEIDENTIFIER NULL, 
		[SFRecordIdentifier] UNIQUEIDENTIFIER NULL,
		[ReqFlag] int NULL,
		[PubTransactionDate] datetime NULL,
		[EmailID] int NULL,
		[SubGenSubscriberID]	   INT				DEFAULT 0 NULL,
		[MailPermission] BIT NULL, 
		[FaxPermission] BIT NULL, 
		[PhonePermission] BIT NULL, 
		[OtherProductsPermission] BIT NULL, 
		[ThirdPartyPermission] BIT NULL, 
		[EmailRenewPermission] BIT NULL, 
		[TextPermission] BIT NULL, 
		[SubGenSubscriptionID] INT NULL DEFAULT 0, 
		[SubGenPublicationID] INT NULL DEFAULT 0, 
		[SubGenMailingAddressId] INT NULL DEFAULT 0, 
		[SubGenBillingAddressId] INT NULL DEFAULT 0, 
		[IssuesLeft] INT NULL DEFAULT 0, 
		[UnearnedReveue] MONEY NULL DEFAULT 0, 
		[SubGenIsLead] BIT NULL,
		[SubGenRenewalCode] VARCHAR(50) NULL,
		[SubGenSubscriptionRenewDate] DATE NULL,
		[SubGenSubscriptionExpireDate] DATE NULL,
		[SubGenSubscriptionLastQualifiedDate] DATE NULL,
		PubCode varchar(50) null,
		PubName varchar(100) null,
		PubTypeDisplayName varchar(250) null,
		ClientName varchar(100) null
	)

		declare @FirstName varchar(50)
		declare @LastName varchar(50)
		declare @WhereSqlStmt varchar(max)
		declare @WhereLeftSqlStmt varchar(max)
		declare @SqlStmt varchar(max)
		if LEN(@fname) > 0
			begin
				set @WhereSqlStmt = ' FirstName like ''' + @fName + '%'''
			end
		
		if LEN(@lName) > 0
			begin
				if LEN(@WhereSqlStmt) > 0
					begin
						set @WhereSqlStmt += ' and LastName like ''' + @lName + '%'''
					end
				else
					begin
						set @WhereSqlStmt = ' LastName like ''' + @lName + '%'''
					end
			end

		IF LEN(@Company) > 0
			BEGIN

				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and Company like ''' + @Company + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' Company like ''' + @Company + '%'''
					END
			END

		IF LEN(@Title) > 0
			BEGIN

				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and Title like ''' + @Title + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' Title like ''' + @Title + '%'''
					END
			END
		
		IF LEN(@Add1) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and Address1 like ''' + @Add1 + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' Address1 like ''' + @Add1 + '%'''
					END
			END
		
		IF LEN(@City) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and City like ''' + @City + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' City like ''' + @City + '%'''
					END
			END
		
		IF LEN(@RegionCode) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and RegionCode = '''+@RegionCode+''''		
				ELSE
					SET @WhereSqlStmt = 'RegionCode = '''+@RegionCode+''''		
			END	

		IF LEN(@Zip) > 0
			BEGIN
				DECLARE @Zip5 varchar(25)
				DECLARE @Zip4 varchar(25)
		
				IF LEN(@Zip) = 10
					BEGIN
						SET @Zip5 = SUBSTRING(@Zip,1,5)
						SET @Zip4 = SUBSTRING(@Zip,7,4)
					END
				ELSE IF LEN(@Zip) = 7
					BEGIN
						SET @Zip5 = SUBSTRING(@Zip,1,3)
						SET @Zip4 = SUBSTRING(@Zip,5,3)
					END
				ELSE
					BEGIN
						SET @Zip5 = @Zip
					END
			
				IF LEN(@Zip4) > 0
					BEGIN
						IF LEN(@WhereSqlStmt) > 0
							SET @WhereSqlStmt += ' and ZipCode = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''		
						ELSE
							SET @WhereSqlStmt = 'ZipCode = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''	
					END
				ELSE
					BEGIN
						IF LEN(@WhereSqlStmt) > 0
							SET @WhereSqlStmt += ' and ZipCode like '''+@Zip5+'%'''		
						ELSE
							SET @WhereSqlStmt = 'ZipCode like '''+@Zip5+'%'''	
					END	
			END
		
		IF LEN(@Country) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and Country like ''' + @Country + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' Country like ''' + @Country + '%'''
					END
			END
		
		IF LEN(@Email) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0			
					BEGIN
						SET @WhereSqlStmt += ' and ps.Email like ''' + @Email + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' ps.Email like ''' + @Email + '%'''
					END
			END
		
		IF LEN(@Phone) > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and (replace(Phone,''-'','''') like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'		
				ELSE
					SET @WhereSqlStmt = '(replace(Phone,''-'','''') like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'
			END	
				
		IF @SequenceID > 0
			BEGIN		
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ps.SequenceID = '''+cast(@SequenceID as varchar(25))+''''		
				ELSE
					SET @WhereSqlStmt = 'ps.SequenceID = '''+cast(@SequenceID as varchar(25))+''''
				
			END
		
		IF LEN(@AccountID) > 0 
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ps.AccountNumber = '''+cast(@AccountID as varchar(25))+''''		
				ELSE
					SET @WhereSqlStmt = 'ps.AccountNumber = '''+cast(@AccountID as varchar(25))+''''
				
			END
		
		--IF @PublisherID > 0
		--BEGIN
		--	IF LEN(@WhereSqlStmt) > 0
		--		SET @WhereSqlStmt += ' and ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''		
		--	ELSE
		--		SET @WhereSqlStmt = 'ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''
		--END
		
		IF @SubscriptionID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ps.SubscriptionID = '''+cast(@SubscriptionID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ps.SubscriptionID = '''+cast(@SubscriptionID as varchar(25))+''''
		END

		IF @PublicationID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ps.PubID = '''+cast(@PublicationID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ps.PubID = '''+cast(@PublicationID as varchar(25))+''''
		END
	
		SET @SqlStmt = ' Insert into #Indiv
					select top 100 
					ps.[PubSubscriptionID]  	,		
					ps.[SubscriptionID]     	,		
					ps.[PubID]              	,		
					ps.[Demo7]              	,		
					ps.[QualificationDate]  	,		
					ps.[PubQSourceID]       	,		
					ps.[PubCategoryID]      	,		
					ps.[PubTransactionID]   	,		
					ps.[EmailStatusID]      	,		
					ps.[StatusUpdatedDate]		,	
					ps.[StatusUpdatedReason]	,	
					ps.[Email]					,	
					ps.[DateCreated]			,	
					ps.[DateUpdated]			,	
					ps.[CreatedByUserID]		,	
					ps.[UpdatedByUserID]		,	
					ps.[IsComp]				,	
					ps.[SubscriptionStatusID]	,	
					ps.[AccountNumber]			,	
					ps.[AddRemoveID]			,	
					ps.[Copies]				,	
					ps.[GraceIssues]			,	
					ps.[IMBSEQ]				,	
					ps.[IsActive]				,	
					ps.[IsPaid]				,	
					ps.[IsSubscribed]			,	
					ps.[MemberGroup]			,	
					ps.[OnBehalfOf]			,	
					ps.[OrigsSrc]				,	
					ps.[Par3CID]				,	
					ps.[SequenceID]			,	
					ps.[Status]				,	
					ps.[SubscriberSourceCode]	,	
					ps.[SubSrcID]				,	
					ps.[Verify]				,	
					ps.[ExternalKeyID]      	,	
					ps.[FirstName]          	,	
					ps.[LastName]           	,	    
					ps.[Company]            	,	    
					ps.[Title]              	,	    
					ps.[Occupation]         	,	    
					ps.[AddressTypeID]      	,	    
					ps.[Address1]           	,	    
					ps.[Address2]           	,	    
					ps.[Address3]           	,	    
					ps.[City]               	,	    
					ps.[RegionCode]         	,	    
					ps.[RegionID]           	,	    
					ps.[ZipCode]            	,	    
					ps.[Plus4]              	,	    
					ps.[CarrierRoute]       	,	    
					ps.[County]             	,	    
					ps.[Country]            	,	    
					ps.[CountryID]          	,	    
					ps.[Latitude]           	,	    
					ps.[Longitude]          	,	    
					ps.[IsAddressValidated] 	,	    
					ps.[AddressValidationDate]  , 		
					ps.[AddressValidationSource] ,		
					ps.[AddressValidationMessage],		
					ps.[Phone]                   ,		
					ps.[Fax]                     ,		
					ps.[Mobile]                  ,		
					ps.[Website]                 ,		
					ps.[Birthdate]               ,		
					ps.[Age]                     ,		
					ps.[Income]                  ,		
					ps.[Gender]                  ,		
					ps.[tmpSubscriptionID]       ,		
					ps.[IsLocked]                ,		
					ps.[LockedByUserID]		  ,		
					ps.[LockDate]				  ,		
					ps.[LockDateRelease]		  ,		
					ps.[PhoneExt]				  ,		
					ps.[IsInActiveWaveMailing]	  ,		
					ps.AddressTypeCodeId		,		
					ps.AddressLastUpdatedDate	,		
					ps.AddressUpdatedSourceTypeCodeId	,
					ps.[WaveMailingID]		,			
					ps.[IGrp_No]			,			
					ps.[SFRecordIdentifier]		,	
					ps.[ReqFlag]					,	
					ps.[PubTransactionDate]		,	
					ps.[EmailID]					,	
					ps.[SubGenSubscriberID]		,		
					ps.[MailPermission]			,		
					ps.[FaxPermission]				,		
					ps.[PhonePermission]			,		
					ps.[OtherProductsPermission]	,		
					ps.[ThirdPartyPermission]		,		
					ps.[EmailRenewPermission]		,		
					ps.[TextPermission]			,		
					ps.[SubGenSubscriptionID]		,		
					ps.[SubGenPublicationID]		,		
					ps.[SubGenMailingAddressId]	,		
					ps.[SubGenBillingAddressId]	,		
					ps.[IssuesLeft]				,		
					ps.[UnearnedReveue]			,		
					ps.[SubGenIsLead]				,		
					ps.[SubGenRenewalCode]			,		
					ps.[SubGenSubscriptionRenewDate],		
					ps.[SubGenSubscriptionExpireDate],		
					ps.[SubGenSubscriptionLastQualifiedDate], 
					P.PubCode,
					P.PubName,
					pt.PubTypeDisplayName,
					''' + @ClientDisplayName + ''' as ClientName
					FROM PubSubscriptions ps with(nolock)
					JOIN Pubs p With(NoLock)ON p.PubID = ps.PubID   
					JOIN PubTypes pt With(NoLock) ON pt.PubTypeID = p.PubTypeID 
					where p.IsCirc = 1 and ' + @WhereSqlStmt				

		--PRINT(@SqlStmt)
		Exec(@SqlStmt)

	--Task-47827: UAS Web - The Users need to be able to see the First name/Last name concatenated together on the Search screen details even if the first name is blank or null and only Last name exists.
	SELECT TOP 100 i.*, RTRIM(LTRIM(ISNULL(i.FirstName, '') + ' ' + ISNULL(i.LastName, ''))) as 'FullName',
			case when i.CountryID = 1 and ISNULL(i.ZipCode,'')!='' AND ISNULL(i.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + '-' + i.Plus4 
				  when i.CountryID = 2 and ISNULL(i.ZipCode,'')!='' AND ISNULL(i.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + ' ' + i.Plus4 
				  else RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(i.Plus4,''))) end as 'FullZip',
			isnull(cty.PhonePrefix,1) as 'PhoneCode',
			RTRIM(LTRIM(ISNULL(i.[Address1], ''))) + ', ' + RTRIM(LTRIM(ISNULL(i.Address2,''))) + ', ' + RTRIM(LTRIM(ISNULL(i.Address3,''))) + ', ' + RTRIM(LTRIM(ISNULL(i.City, ''))) + ', ' + RTRIM(LTRIM(ISNULL(i.RegionCode,''))) + ', ' + 
			(case when i.CountryID = 1 and ISNULL(i.ZipCode,'')!='' AND ISNULL(i.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + '-' + i.Plus4 
				  when i.CountryID = 2 and ISNULL(i.ZipCode,'')!='' AND ISNULL(i.Plus4,'')!='' THEN RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + ' ' + i.Plus4 
				  else RTRIM(LTRIM(ISNULL(i.ZipCode, ''))) + RTRIM(LTRIM(ISNULL(i.Plus4,''))) end)+ ', ' + 
			RTRIM(LTRIM(ISNULL(i.Country, ''))) as 'FullAddress'
	FROM #Indiv i
	LEFT JOIN UAD_Lookup..Country cty with(nolock) on i.CountryID = cty.CountryID
	GROUP BY 
	PubSubscriptionID,SubscriptionID,PubID,Demo7,QualificationDate,PubQSourceID,PubCategoryID,PubTransactionID,EmailStatusID,StatusUpdatedDate,StatusUpdatedReason,Email,DateCreated,DateUpdated,CreatedByUserID,UpdatedByUserID,
	IsComp,SubscriptionStatusID,AccountNumber,AddRemoveID,Copies,GraceIssues,IMBSEQ,IsActive,IsPaid,IsSubscribed,MemberGroup,OnBehalfOf,OrigsSrc,Par3CID,SequenceID,Status,SubscriberSourceCode,SubSrcID,Verify,
	ExternalKeyID,FirstName,LastName,Company,Title,Occupation,AddressTypeID,Address1,Address2,Address3,City,RegionCode,RegionID,ZipCode,Plus4,CarrierRoute,County,Country,i.CountryID,Latitude,Longitude,IsAddressValidated,
	AddressValidationDate,AddressValidationSource,AddressValidationMessage,Phone,Fax,Mobile,Website,Birthdate,Age,Income,Gender,tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,IsInActiveWaveMailing, SubGenSubscriberID,
	MailPermission, FaxPermission, PhonePermission, OtherProductsPermission, EmailRenewPermission, ThirdPartyPermission, TextPermission,
	SubGenSubscriptionID, SubGenPublicationID, SubGenMailingAddressId, SubGenBillingAddressId, IssuesLeft, UnearnedReveue, 
	SubGenIsLead, SubGenRenewalCode, SubGenSubscriptionRenewDate, SubGenSubscriptionExpireDate, SubGenSubscriptionLastQualifiedDate,
	AddressTypeCodeId,AddressLastUpdatedDate,AddressUpdatedSourceTypeCodeId,WaveMailingID,IGrp_No,SFRecordIdentifier,ReqFlag,PubCode,PubName,PubTypeDisplayName,ClientName,cty.PhonePrefix,PubTransactionDate,EmailID
	Order By i.ClientName,i.PubCode,i.FirstName,i.LastName,i.Company,i.Title,i.Address1,i.City,i.RegionCode,i.ZipCode,i.Email,i.Phone DESC

	drop table #Indiv

END