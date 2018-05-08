CREATE PROCEDURE [dbo].[e_Individual_Search]
@Search nvarchar(4000),
@SearchFields nvarchar(4000) = '',
@OrderBy nvarchar(2000) = ''
AS 
BEGIN

	SET NOCOUNT ON

	DECLARE @FindWord nvarchar(4000)
	Declare @individual nvarchar(100) = null

	CREATE table #Indiv  
	(
		[IndividualID] [int] NOT NULL,
		[FirstName] [nvarchar](50) NULL,
		[LastName] [nvarchar](50) NULL,
		[Company] [nvarchar](100) NULL,
		[Title] [nvarchar](255) NULL,
		[AddressTypeID] [int] NULL,
		[Address1] [nvarchar](100) NULL,
		[Address2] [nvarchar](100) NULL,
		[City] [nvarchar](50) NULL,
		[State] [nchar](50) NULL,
		[StateID] [int] NULL,
		[ZipCode] [nchar](10) NULL,
		[Plus4] [nchar](10) NULL,
		[CarrierRoute] [varchar](10) NULL,
		[County] [nvarchar](50) NULL,
		[Country] [nvarchar](50) NULL,
		[CountryID] [int] NULL,
		[IsAddressValidated] [bit] NOT NULL,
		[AddressValidationDate] [datetime] NULL,
		[AddressValidationSource] [nvarchar](50) NULL,
		[AddressValidationMessage] [nvarchar](max) NULL,
		[Email] [nvarchar](255) NULL,
		[Phone] [nchar](50) NULL,
		[Fax] [nchar](50) NULL,
		[Mobile] [nchar](50) NULL,
		[Website] [nvarchar](255) NULL,
		[DateCreated] [datetime] NOT NULL,
		[DateUpdated] [datetime] NULL,
		[CreatedByUserID] [int] NOT NULL,
		[UpdatedByUserID] [int] NULL
	)

	IF LEN(@SearchFields) = 0
		BEGIN
			WHILE LEN(@Search) > 0
				BEGIN
					IF PATINDEX('% %',@Search) > 0
						BEGIN
							SET @individual = SUBSTRING(@Search, 0, PATINDEX('% %',@Search))
							SET @FindWord = '"' +@individual + '*"'
							-- do query here
							Insert Into #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(FirstName,@FindWord)) OR
									(CONTAINS(LastName,@FindWord)) OR
									(CONTAINS(Company,@FindWord)) OR
									(CONTAINS(Title,@FindWord)) OR
									(CONTAINS(Address1,@FindWord)) OR
									(CONTAINS(City,@FindWord)) OR
									(CONTAINS(State,@FindWord)) OR
									(CONTAINS(ZipCode,@FindWord)) OR
									(CONTAINS(Email,@FindWord)) OR
									(CONTAINS(Phone,@FindWord)) OR
									(CONTAINS(Mobile,@FindWord)) 
				
							----------------------
							SET @Search = REPLACE(@Search, @individual + ' ','')
						END
					ELSE
						BEGIN
							SET @individual = @Search
							SET @FindWord = '"' +@individual + '*"'
							-- do query here
							Insert Into #Indiv
							SELECT *
							FROM Individual WITH (NOLOCK)
							WHERE (CONTAINS(FirstName,@FindWord)) OR
									(CONTAINS(LastName,@FindWord)) OR
									(CONTAINS(Company,@FindWord)) OR
									(CONTAINS(Title,@FindWord)) OR
									(CONTAINS(Address1,@FindWord)) OR
									(CONTAINS(City,@FindWord)) OR
									(CONTAINS(State,@FindWord)) OR
									(CONTAINS(ZipCode,@FindWord)) OR
									(CONTAINS(Email,@FindWord)) OR
									(CONTAINS(Phone,@FindWord)) OR
									(CONTAINS(Mobile,@FindWord)) 
				
							----------------------
							SET @Search = NULL
						END
				END
		END
	ELSE
		BEGIN
			DECLARE @Column nvarchar(50)
			DECLARE @OriginalSearch nvarchar(4000) = @Search
		
			DECLARE c Cursor
			FOR 
				SELECT Items FROM dbo.fn_Split(@SearchFields,',')
			OPEN c
			FETCH NEXT FROM c INTO @Column
			WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @Search = @OriginalSearch
					WHILE LEN(@Search) > 0
						BEGIN
							IF PATINDEX('% %',@Search) > 0
								BEGIN
									SET @individual = SUBSTRING(@Search, 0, PATINDEX('% %',@Search))
									SET @FindWord = '"' +@individual + '*"'

									IF @Column = 'FirstName'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(FirstName,@FindWord))
									END
					
									IF @Column = 'LastName'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(LastName,@FindWord))
									END
					
									IF @Column = 'Company'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(Company,@FindWord))
									END
					
									IF @Column = 'Title'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(Title,@FindWord))
									END
					
									IF @Column = 'Address1'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(Address1,@FindWord))
									END
					
									IF @Column = 'City'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(City,@FindWord))
									END
					
									IF @Column = 'State'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(State,@FindWord))
									END
					
									IF @Column = 'ZipCode'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(ZipCode,@FindWord))
									END
					
									IF @Column = 'Email'
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(Email,@FindWord))
									END
					
									--IF @Column = 'Phone'
									--BEGIN
									--	INSERT INTO #Indiv
									--		SELECT *
									--		FROM Individual WITH (NOLOCK)
									--		WHERE (CONTAINS(Phone,@FindWord))
									--END
					
									IF (@Column = 'Mobile') OR (@Column = 'Phone')
									BEGIN
										INSERT INTO #Indiv
											SELECT *
											FROM Individual WITH (NOLOCK)
											WHERE (CONTAINS(Mobile,@FindWord)) OR (CONTAINS(Phone,@FindWord))
									END
										
									----------------------
									SET @Search = REPLACE(@Search, @individual + ' ','')
								END
							ELSE
								BEGIN
									SET @individual = @Search
									SET @FindWord = '"' +@individual + '*"'
									-- do query here
									IF @Column = 'FirstName'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(FirstName,@FindWord))
										END
									IF @Column = 'LastName'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(LastName,@FindWord))
										END
									IF @Column = 'Company'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(Company,@FindWord))
										END
									IF @Column = 'Title'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(Title,@FindWord))
										END
									IF @Column = 'Address1'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(Address1,@FindWord))
										END
									IF @Column = 'City'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(City,@FindWord))
										END
									IF @Column = 'State'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(State,@FindWord))
										END
					
									IF @Column = 'ZipCode'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(ZipCode,@FindWord))
										END
					
									IF @Column = 'Email'
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(Email,@FindWord))
										END
					
									--IF @Column = 'Phone'
									--BEGIN
									--	INSERT INTO #Indiv
									--		SELECT *
									--		FROM Individual WITH (NOLOCK)
									--		WHERE (CONTAINS(Phone,@FindWord))
									--END
					
									IF (@Column = 'Mobile') OR (@Column = 'Phone')
										BEGIN
											INSERT INTO #Indiv
												SELECT *
												FROM Individual WITH (NOLOCK)
												WHERE (CONTAINS(Mobile,@FindWord)) OR (CONTAINS(Phone,@FindWord))
										END
					
									----------------------
									SET @Search = NULL
								END
						END
			
					FETCH NEXT FROM c INTO @Column
				END
			CLOSE c
			DEALLOCATE c
		END

	IF LEN(@OrderBy) > 0
		BEGIN
			DECLARE @SqlCode nvarchar(500) = 'SELECT * FROM #Indiv ORDER BY ' + @OrderBy
			EXEC(@SqlCode)
		END
	ELSE
		BEGIN
			SELECT * FROM #Indiv
		END
	DROP TABLE #Indiv

END