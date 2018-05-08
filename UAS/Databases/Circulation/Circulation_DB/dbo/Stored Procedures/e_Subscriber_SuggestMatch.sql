
CREATE PROCEDURE [dbo].[e_Subscriber_SuggestMatch] 
	@FirstName varchar(50) = '',
	@LastName varchar(50) = '',
	@Email varchar(50) = '',
	@PublisherID int = '',
	@PublicationID int = ''

AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @WhereSqlStmt varchar(max)
	DECLARE @WhereLeftSqlStmt varchar(max)
	DECLARE @SqlStmt varchar(max)

		IF LEN(@FirstName) > 0
		BEGIN
			SET @WhereSqlStmt = 'dbo.fn_Levenshtein(FirstName,'''+@FirstName+''') > 85'
			SET @WhereLeftSqlStmt = 'Left(FirstName,2) = Left('''+@FirstName+''',2)'
		END
		
		IF LEN(@LastName) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
			BEGIN
				SET @WhereSqlStmt += ' and dbo.fn_Levenshtein(LastName,'''+@LastName+''') > 85' 
				SET @WhereLeftSqlStmt += ' and Left(LastName,2) = Left('''+@LastName+''',2)'
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = 'dbo.fn_Levenshtein(LastName,'''+@LastName+''') > 85'
				SET @WhereLeftSqlStmt = 'Left(LastName,2) = Left('''+@LastName+''',2)'
			END
		END
		
		IF LEN(@Email) > 0
		BEGIN
			IF LEN(@WhereSqlStmt) > 0
			BEGIN
				SET @WhereSqlStmt += ' and Email = '''+@Email+'''' 
				--SET @WhereLeftSqlStmt += ' and Left(Email,2) = Left('''+@Email+''',2)'
			END
			ELSE
			BEGIN
				SET @WhereSqlStmt = 'Email = '''+@Email+'''' 
				--SET @WhereLeftSqlStmt = 'Left(Email,2) = Left('''+@Email+''',2)'
			END
		END
			
	-- If @WhereLeftSqlStmt is 0 then there was no statements generated for that variable, therefore the @Sqlstmt should be the ELSE statement
	IF LEN(@WhereLeftSqlStmt) > 0
	BEGIN
		SET @SqlStmt = 'Select top 25 s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County 
								,Phone,Mobile,Fax,Email,Website 
						FROM Subscriber s WITH (NOLOCK) inner join Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID 
						WHERE ('+@WhereLeftSqlStmt+') and ('+ @WhereSqlStmt+') 
						and ss.PublisherID = '+ cast(@PublisherID as varchar(10)) +' and SS.PublicationID = '+ cast(@PublicationID as varchar(10)) +' 
						GROUP BY s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County
						 ,Phone,Mobile,Fax,Email,Website'
	END
	ELSE
	BEGIN
		SET @SqlStmt = 'Select top 25 s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County 
							,Phone,Mobile,Fax,Email,Website 
					FROM Subscriber s WITH (NOLOCK) inner join Subscription ss WITH(NOLOCK) on s.SubscriberID = ss.SubscriberID 
					WHERE ('+ @WhereSqlStmt+') 
					and ss.PublisherID = '+ cast(@PublisherID as varchar(10)) +' and SS.PublicationID = '+ cast(@PublicationID as varchar(10)) +' 
					GROUP BY s.SubscriberID,FirstName,LastName,Title,Company,AddressTypeID,Address1,Address2,Address3,City,Country,RegionCode,ZipCode,County
					 ,Phone,Mobile,Fax,Email,Website'
	END
	

	EXEC(@SqlStmt)
	
END
