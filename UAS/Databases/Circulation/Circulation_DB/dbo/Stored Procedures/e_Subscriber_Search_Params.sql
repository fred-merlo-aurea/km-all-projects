CREATE PROCEDURE [dbo].[e_Subscriber_Search_Params]
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
@AccountID varchar(50),
@PublisherID int,
@PublicationID int

AS

	CREATE table #Indiv  
	(
		[SubscriberID] [int] NOT NULL,
		[ExternalKeyID] [int] NULL,
		[FirstName] [varchar](50) NULL,
		[LastName] [varchar](50) NULL,
		[Company] [varchar](100) NULL,
		[Title] [varchar](255) NULL,
		Occupation [varchar](255) NULL,
		[AddressTypeID] [int] NULL,
		[Address1] [varchar](100) NULL,
		[Address2] [varchar](100) NULL,
		[Address3] [varchar](100) NULL,
		[City] [varchar](50) NULL,
		RegionCode [varchar](50) NULL,
		RegionID [int] NULL,
		[ZipCode] [varchar](10) NULL,
		[Plus4] [varchar](10) NULL,
		[CarrierRoute] [varchar](10) NULL,
		[County] [varchar](50) NULL,
		[Country] [varchar](50) NULL,
		[CountryID] [int] NULL,
		Latitude decimal(18,15) null,
		Longitude decimal(18,15) null,
		[IsAddressValidated] [bit] NOT NULL,
		[AddressValidationDate] [datetime] NULL,
		[AddressValidationSource] [varchar](50) NULL,
		[AddressValidationMessage] [varchar](max) NULL,
		[Email] [varchar](255) NULL,
		[Phone] [varchar](50) NULL,
		[Fax] [varchar](50) NULL,
		[Mobile] [varchar](50) NULL,
		[Website] [varchar](255) NULL,
		Birthdate date NULL,
		Age int NULL,
		Income [varchar](50) NULL,
		Gender [varchar](50) NULL,
		[DateCreated] [datetime] NOT NULL,
		[DateUpdated] [datetime] NULL,
		[CreatedByUserID] [int] NOT NULL,
		[UpdatedByUserID] [int] NULL,
		tmpSubscriptionID int null,
		[IsLocked] bit NOT NULL,
		LockedByUserID int NULL,
		LockDate datetime NULL,
		LockDateRelease datetime NULL,
		PhoneExt [varchar](25) NULL,
		IsInActiveWaveMailing bit,
		AddressTypeCodeId int null,
		AddressLastUpdatedDate datetime null,
		AddressUpdatedSourceTypeCodeId int null,
		WaveMailingID int NULL,
		Igrp_No uniqueidentifier,
		SFRecordIdentifier uniqueidentifier,
		PublisherID int null,
		PublicationID int null,
		PublisherName varchar(256),
		PublicationCode varchar(50)
	)

		declare @FirstName varchar(50)
		declare @LastName varchar(50)
		declare @WhereSqlStmt varchar(max)
		declare @WhereLeftSqlStmt varchar(max)
		declare @SqlStmt varchar(max)

		
		--IF LEN(@FullName) > 0
		--BEGIN
		--	IF(@nameCount) <= 2
		--	BEGIN
		--		IF PATINDEX('% %',@FullName) > 0
		--		BEGIN
		--			SET @FirstName = SUBSTRING(@FullName, 0, PATINDEX('% %',@FullName))	
					
		--			DECLARE @FirstNameLength int = LEN(@FirstName)
					
		--			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LEFT(FirstName,'+CAST(@FirstNameLength as VARCHAR(256))+'),'''+@FirstName+''') > 75'
		--			SET @WhereLeftSqlStmt = 'Left(FirstName,1) = Left('''+@FirstName+''',1)'
					
		--			SET @LastName = REPLACE(@FullName, @FirstName + ' ','')
					
		--			IF LEN(@LastName) > 0
		--			DECLARE @LastNameLength int = LEN(@LastName)
		--			BEGIN
		--				SET @WhereSqlStmt += ' and dbo.fn_Levenshtein(LEFT(LastName,'+CAST(@LastNameLength as VARCHAR(256))+'),'''+@LastName+''') > 75'
		--				SET @WhereLeftSqlStmt += ' and Left(LastName,1) = Left('''+@LastName+''',1)'
		--			END
		--		END
		--		ELSE IF PATINDEX('% %',@FullName) = 0
		--		BEGIN
		--			SET @WhereSqlStmt = '(dbo.fn_Levenshtein(FirstName,'''+@FullName+''') > 75 or dbo.fn_Levenshtein(LastName,'''+@FullName+''') > 75)'
		--			SET @WhereLeftSqlStmt = '(Left(FirstName,1) = Left('''+@FullName+''',1) or Left(LastName,1) = Left('''+@FullName+''',1))'
		--		END
		--	END
		--	ELSE IF(@nameCount) >= 3
		--	BEGIN
		--		SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LEFT(FirstName + '' '' + lastname,'+CAST((select LEN(@FullName)) as VARCHAR(256))+'),'''+@FullName+''') > 75'
		--	END
		--END

		--IF LEN(@FullName) > 0
		--BEGIN
		--		IF PATINDEX('% %',@FullName) = 0
		--		BEGIN
		--			SET @WhereSqlStmt = '(dbo.fn_Levenshtein(FirstName,'''+@FullName+''') > 75 or dbo.fn_Levenshtein(LastName,'''+@FullName+''') > 75)'
		--			SET @WhereLeftSqlStmt = '(Left(FirstName,1) = Left('''+@FullName+''',1) or Left(LastName,1) = Left('''+@FullName+''',1))'
		--		END
		--		ELSE
		--		BEGIN
		--			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(REPLACE(LEFT(FirstName + lastname,'+CAST((select LEN(@FullName)) as VARCHAR(256))+'),'' '',''''),'''+@FullName+''') > 80 '
		--			SET @WhereLeftSqlStmt = '(LEFT(FirstName + LastName,1) = LEFT('''+@FullName+''',1))'
		--		END
		--END
		
		if LEN(@fname) > 0
		begin
			set @WhereSqlStmt = ' firstName like ''' + @fName + '%'''
		end
		
		if LEN(@lName) > 0
		begin
			if LEN(@WhereSqlStmt) > 0
			begin
				set @WhereSqlStmt += ' and lastName like ''' + @lName + '%'''
			end
			else
			begin
				set @WhereSqlStmt = ' lastName like ''' + @lName + '%'''
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
				SET @WhereSqlStmt += ' and Email like ''' + @Email + '%'''
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = ' Email like ''' + @Email + '%'''
			END
		END
		
		IF LEN(@Phone) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and (Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'		
			ELSE
				SET @WhereSqlStmt = '(Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'
		END	
				
		IF @SequenceID > 0
		BEGIN		
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.SequenceID = '''+cast(@SequenceID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.SequenceID = '''+cast(@SequenceID as varchar(25))+''''
				
		END
		
		IF LEN(@AccountID) > 0 
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''
				
		END
		
		IF @PublisherID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''
		END
		
		IF @PublicationID > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
				SET @WhereSqlStmt += ' and ss.PublicationID = '''+cast(@PublicationID as varchar(25))+''''		
			ELSE
				SET @WhereSqlStmt = 'ss.PublicationID = '''+cast(@PublicationID as varchar(25))+''''
		END
		
		
		--IF LEN(@WhereLeftSqlStmt) > 0
		--BEGIN
		--	SET @SqlStmt = 'Insert into #Indiv
		--					Select top 100 s.*,ss.PublisherID,ss.PublicationId,p.PublisherName,pp.PublicationCode
		--					FROM Subscriber s WITH (NOLOCK) INNER JOIN Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID
		--													INNER JOIN Publisher p with(nolock) on ss.PublisherId = p.PublisherId
		--													INNER JOIN Publication pp with(nolock) on ss.PublicationID = pp.PublicationId
		--					WHERE  ('+@WhereLeftSqlStmt+') and ('+ @WhereSqlStmt+')'
		--END
		--ELSE
		--BEGIN
		SET @SqlStmt = 'Insert into #Indiv
			Select top 100 s.*,ss.PublisherID,ss.PublicationID ,'''' as PublisherName,'''' as PublicationCode
			FROM Subscriber s WITH (NOLOCK) INNER JOIN Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID
			WHERE ' + @WhereSqlStmt				
		--END
		
		--PRINT(@SqlStmt)
		Exec(@SqlStmt)

	SELECT TOP 100 [SubscriberID],[ExternalKeyID],[FirstName],[LastName],[Company],[Title],Occupation,[AddressTypeID],[Address1],[Address2],[Address3],[City],RegionCode,RegionID,[ZipCode],
		[Plus4],[CarrierRoute],[County],[Country],[CountryID],Latitude,Longitude,[IsAddressValidated],[AddressValidationDate],[AddressValidationSource],[AddressValidationMessage],
		[Email],[Phone],[Fax],[Mobile],[Website],Birthdate,Age,Income,Gender,[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,PublisherID,PublicationID,
		PublisherName,PublicationCode, WaveMailingID, IsInActiveWaveMailing
	FROM #Indiv 
	GROUP BY [SubscriberID],[ExternalKeyID],[FirstName],[LastName],[Company],[Title],Occupation,[AddressTypeID],[Address1],[Address2],[Address3],[City],RegionCode,RegionID,[ZipCode],
		[Plus4],[CarrierRoute],[County],[Country],[CountryID],Latitude,Longitude,[IsAddressValidated],[AddressValidationDate],[AddressValidationSource],[AddressValidationMessage],
		[Email],[Phone],[Fax],[Mobile],[Website],Birthdate,Age,Income,Gender,[DateCreated],[DateUpdated],[CreatedByUserID],[UpdatedByUserID],tmpSubscriptionID,IsLocked,LockedByUserID,LockDate,LockDateRelease,PhoneExt,PublisherID,PublicationID,
		PublisherName,PublicationCode, WaveMailingID, IsInActiveWaveMailing
	Order By PublisherName,PublicationCode,[FirstName],[LastName],[Company],[Title],[Address1],[City],RegionCode,[ZipCode],[Email],[Phone] DESC

	drop table #Indiv