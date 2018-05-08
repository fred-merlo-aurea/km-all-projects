CREATE PROCEDURE [dbo].[e_Subscription_Search_Params]
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
@PublisherID int = 0,
@PublicationID int,
@clientDisplayName varchar(100)
AS
BEGIN

	SET NOCOUNT ON

	CREATE table #Indiv  
	(
		SubscriptionID [int] NOT NULL,
		FName varchar(100) null,
		LName varchar(100) null,
		Title varchar(100) null,
		Company varchar(100) null,
		Address varchar(255) null,
		MailStop varchar(255) null,
		City varchar(100) null,
		State varchar(50) null,
		County varchar(100) null,
		Country varchar(100) null,
		CountryID int null,
		RegionID int null,
		Phone varchar(100) null,
		Fax varchar(100) null,
		Email varchar(100) null,
		Zip varchar(10) null,
		IsActive bit null,
		IsLocked bit null,
		LockDate datetime null,
		LockDateRelease datetime null,
		LockedByUserID int null,
		SubscriptionStatusID int null,
		Sequence int null,
		PublicationID int null,
		PublicationCode varchar(50),
		ClientName varchar(100)		
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
				set @WhereSqlStmt = ' FName like ''' + @fName + '%'''
			end
		if LEN(@lName) > 0
			begin
				if LEN(@WhereSqlStmt) > 0
					begin
						set @WhereSqlStmt += ' and LName like ''' + @lName + '%'''
					end
				else
					begin
						set @WhereSqlStmt = ' LName like ''' + @lName + '%'''
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
						SET @WhereSqlStmt += ' and Address like ''' + @Add1 + '%'''
					END
				ELSE
					BEGIN
						SET @WhereSqlStmt = ' Address like ''' + @Add1 + '%'''
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
					SET @WhereSqlStmt += ' and State = '''+@RegionCode+''''		
				ELSE
					SET @WhereSqlStmt = 'State = '''+@RegionCode+''''		
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
							SET @WhereSqlStmt += ' and Zip = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''		
						ELSE
							SET @WhereSqlStmt = 'Zip = '''+@Zip5+''' and Plus4 = '''+@Zip4+''''	
					END
				ELSE
					BEGIN
						IF LEN(@WhereSqlStmt) > 0
							SET @WhereSqlStmt += ' and Zip like '''+@Zip5+'%'''		
						ELSE
							SET @WhereSqlStmt = 'Zip like '''+@Zip5+'%'''	
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
					SET @WhereSqlStmt += ' and (Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'		
				ELSE
					SET @WhereSqlStmt = '(Phone like '''+@Phone+'%'' OR Mobile like '''+@Phone+'%'')'
			END		
		IF @SequenceID > 0
			BEGIN		
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ss.Sequence = '''+cast(@SequenceID as varchar(25))+''''		
				ELSE
					SET @WhereSqlStmt = 'ss.Sequence = '''+cast(@SequenceID as varchar(25))+''''
				
			END
		IF LEN(@AccountID) > 0 
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''		
				ELSE
					SET @WhereSqlStmt = 'ss.AccountNumber = '''+cast(@AccountID as varchar(25))+''''
				
			END
		
		--IF @PublisherID > 0
		--BEGIN
		--	IF LEN(@WhereSqlStmt) > 0
		--		SET @WhereSqlStmt += ' and ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''		
		--	ELSE
		--		SET @WhereSqlStmt = 'ss.PublisherID = '''+cast(@PublisherID as varchar(25))+''''
		--END
		
		IF @PublicationID > 0
			BEGIN
				IF LEN(@WhereSqlStmt) > 0
					SET @WhereSqlStmt += ' and ps.PubID = '''+cast(@PublicationID as varchar(25))+''''		
				ELSE
					SET @WhereSqlStmt = 'ps.PubID = '''+cast(@PublicationID as varchar(25))+''''
			END
		
		--SET @SqlStmt = 'Insert into #Indiv
		--	Select top 100 s.*,ss.PublisherID,ss.PublicationID ,'''' as PublisherName,'''' as PublicationCode
		--	FROM Subscriber s WITH (NOLOCK) INNER JOIN Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID
		--	WHERE ' + @WhereSqlStmt			
			
		SET @SqlStmt = 'Insert into #Indiv
				select top 100 ss.SubscriptionID,FName,LName,Title,Company,[Address],MailStop,City,[State],County,Country,CountryID,RegionID,Phone,Fax,ps.Email,Zip,
				ss.IsActive,IsLocked,LockDate,LockDateRelease,LockedByUserID,SubscriptionStatusID,ss.Sequence, p.PubID,p.PubCode,@clientDisplayName as ClientName
				from Subscriptions ss with(nolock) join PubSubscriptions ps with(nolock) on ss.SubscriptionID = ps.SubscriptionID 	
												   join Pubs p with(nolock) on ps.PubID = p.PubID	
				where  ' + @WhereSqlStmt				
		
		--PRINT(@SqlStmt)
		Exec(@SqlStmt)

	SELECT TOP 100 SubscriptionID,FName,LName,Title,Company,[Address],MailStop,City,[State],County,Country,CountryID,RegionID,Phone,Fax,Email,Zip,
		IsActive,IsLocked,LockDate,LockDateRelease,LockedByUserID,SubscriptionStatusID,Sequence,PublicationID as ProductID,PublicationCode as ProductCode,ClientName
	FROM #Indiv 
	GROUP BY SubscriptionID,FName,LName,Title,Company,[Address],MailStop,City,[State],County,Country,CountryID,RegionID,Phone,Fax,Email,Zip,
		IsActive,IsLocked,LockDate,LockDateRelease,LockedByUserID,SubscriptionStatusID,Sequence,PublicationID,PublicationCode,ClientName
	Order By ClientName,PublicationCode,FName,LName,[Company],[Title],[Address],[City],[State],[Zip],[Email],[Phone] DESC

	drop table #Indiv
	
END