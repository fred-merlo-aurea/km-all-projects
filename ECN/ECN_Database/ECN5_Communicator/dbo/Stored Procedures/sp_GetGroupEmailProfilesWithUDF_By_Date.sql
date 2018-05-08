CREATE PROC sp_GetGroupEmailProfilesWithUDF_By_Date
(    
	@GroupID int,    
	@CustomerID int,    
	@SubscribeType varchar(100) ,
	@FromDate DATETIME,
	@ToDate DATETIME,
	@Recent BIT,
	@ProfileFilter varchar(100) = 'ProfilePlusAllUDFs',
	@Filter varchar(8000)  
)    
AS    

SET NOCOUNT ON  

BEGIN  

	DECLARE	@FromDate_C VARCHAR(30),
			@ToDate_C	VARCHAR(30),
			@SelectSQL varchar(8000),
			@FromSQL varchar(8000),
			@WhereSQL varchar(8000),
			@OrderSQL varchar(8000),
			@StandAloneUDFs VARCHAR(3000) ='',
			@TransactionalUDFs VARCHAR(2000) ='',
			@sColumns varchar(200) ='',
			@tColumns varchar(200) ='',
			@standAloneQuery varchar(4000) ='',
			@TransactionalQuery varchar(4000) =''

	SET @FromDate_C = convert(varchar(20),CAST(@FromDate as date))
	SET @ToDate_C   = convert(varchar(20),CAST(@ToDate as date))  --CONVERT(VARCHAR(23),@ToDate + '23:59:59' ,121)  

	SET @SelectSQL = '
	SELECT DISTINCT
		e.EmailID, 
		EmailAddress, 
		Title, 
		FirstName, 
		LastName, 
		FullName, 
		Company, 
		Occupation, 
		Address, 
		Address2, 
		City, 
		State, 
		Zip, 
		Country, 
		Voice, 
		Mobile, 
		Fax, 
		Website, 
		Age, 
		Income, 
		Gender,   
		User1, 
		User2, 
		User3, 
		User4, 
		User5, 
		User6, 
		Birthdate, 
		UserEvent1, 
		UserEvent1Date, 
		UserEvent2, 
		UserEvent2Date,   
		CONVERT(VARCHAR(1000),Notes) AS Notes, 
		password, 
		CreatedOn, 
		LastChanged, 
		GroupID, 
		FormatTypeCode, 
		SubscribeTypeCode  '

	SET @FromSQL = 	'
	FROM
			Emails e WITH (NOLOCK) 
			INNER JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID '

	SET @OrderSQL = '
	ORDER BY 
			e.EmailAddress' 

	SET @WhereSQL = 
	' 
	WHERE 
			e.CustomerID = ' + CONVERT(VARCHAR(10),@CustomerID) + ' 
			AND eg.SubscribeTypeCode IN ('+ @SubscribeType+')' + '
			AND eg.GroupID = ' + CONVERT(VARCHAR(10),@GroupID)  + ' 
			AND (cast(eg.CreatedOn as date) BETWEEN ''' + @FromDate_C + ''' AND ''' + @ToDate_C + '''
			OR cast(eg.LastChanged as date) BETWEEN  ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''')'

	IF @Recent = 1
	BEGIN
		SET @FromSQL = @FromSQL + '
			INNER JOIN ECN_Activity..BlastActivityOpens bao WITH (NOLOCK) ON bao.EmailID = eg.EmailID '   

		SET @WhereSQL = @WhereSQL+ ' 
			AND (cast(bao.OpenTime as date) BETWEEN ''' + @FromDate_C + ''' AND ''' + @ToDate_C + ''')'
	END

	IF @Filter IS NOT NULL 
	BEGIN 
		SET @WhereSQL = @WhereSQL + ' ' + @Filter 
	END


	IF EXISTS(SELECT GroupID FROM GroupDataFields WITH (NOLOCK) WHERE GroupDatafields.groupID = @GroupID) AND @ProfileFilter IN ('ProfilePlusStandalone','ProfilePlusAllUDFs')
	BEGIN  
		SELECT @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM GroupDataFields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID IS NULL AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH(''),root('MyString'),type ).value('/MyString[1]','varchar(max)'), 1, 2, '') + ']'
		SELECT @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM GroupDataFields  WITH (NOLOCK) WHERE GroupID = @GroupID AND DatafieldSetID > 0  AND IsDeleted = 0 ORDER BY '],[' + ShortName FOR XML PATH(''),root('MyString'),type ).value('/MyString[1]','varchar(max)'), 1, 2, '') + ']'

		IF LEN(@standaloneUDFs) > 0 --AND (@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAllUDFs')
		BEGIN
			SET @sColumns = ', 
			SAUDFs.* '

			SET @standAloneQuery= ' 
			LEFT OUTER JOIN			
					(
						SELECT *
						 FROM
						 (
							SELECT 
								edv.emailID as temp_EmailID,  
								gdf.ShortName, 
								edv.DataValue
							FROM
								EmailDataValues edv  WITH (NOLOCK) 
								INNER JOIN Groupdatafields gdf  WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							WHERE 
								gdf.GroupID = ' + CONVERT(VARCHAR(15), @GroupID) + ' 
								AND DataFieldSetID IS NULL  
								AND gdf.IsDeleted = 0
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) AS pvt 			
					) 
				SAUDFs on e.emailID = SAUDFs.temp_EmailID'
		END

		IF LEN(@TransactionalUDFs) > 0 AND @ProfileFilter = 'ProfilePlusAllUDFs'
		BEGIN
			 SET @tColumns = ', 
				TUDFs.* '

			 SET @TransactionalQuery= '
			   LEFT OUTER JOIN
					(
						SELECT *
						 FROM
						 (
							SELECT 
								edv.emailID as temp_EmailID1, 
								edv.entryID, 
								gdf.ShortName, 
								edv.DataValue
							FROM
								EmailDataValues edv WITH (NOLOCK)
								INNER JOIN Groupdatafields gdf WITH (NOLOCK) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							WHERE
								gdf.GroupID = ' + CONVERT(VARCHAR(15), @GroupID) + ' 
								AND DatafieldsetID > 0 
								AND gdf.IsDeleted = 0
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) AS pvt 			
					) 
				TUDFs on e.emailID = TUDFs.temp_EmailID1 '
		END
	
		SET @SelectSQL = @SelectSQL + @sColumns + @tColumns 
		SET @FromSQL = @FromSQL + @standAloneQuery + @TransactionalQuery 
		END

		EXEC (@SelectSQL + @FromSQL + @WhereSQL + @OrderSQL)	
		--PRINT (@SelectSQL + @FromSQL + @WhereSQL + @OrderSQL)	
END 
