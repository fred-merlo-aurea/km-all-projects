CREATE PROCEDURE dbo.getCircDataForADMS
(
	@ClientId INT,
	@clientName Varchar(50),
	@GroupIDs VARCHAR(MAX),
	@FullExport Bit = 0,
	@StartDate	Date = '',
	@EndDate	Date = ''

)
AS
SET NOCOUNT ON 

BEGIN
	DECLARE 
		@DiffDate DATE = DATEADD(DAY,-1,GETDATE()),
		@StandAloneUDFs VARCHAR(MAX),
		@TransactionalUDFs VARCHAR(MAX),
		@sColumns varchar(MAX),
		@tColumns varchar(MAX),
		@standAloneQuery varchar(MAX),
		@TransactionalQuery varchar(MAX)
					
	SET @clientName = upper(@clientName)
	SET @StandAloneUDFs = ''
	SET @TransactionalUDFs = ''
	SET @sColumns = ''
	SET @tColumns = ''
	SET @standAloneQuery = ''
	SET @TransactionalQuery = ''
			
			SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName 
			FROM groupdatafields 
			WHERE  
				ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
				and ShortName not in (select ShortName from GroupDatafields where Isdeleted =0 AND GroupID in (select items from dbo.fn_Split(@GroupIDs, ',')) and DatafieldSetID > 0 ) 
				and GroupID in (select items from dbo.fn_Split(@GroupIDs, ',')) 
				AND DatafieldSetID is null 
				AND IsDeleted = 0
			ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
				
			SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName 
			FROM groupdatafields 
			WHERE  
				ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
				and GroupID in (select items from dbo.fn_Split(@GroupIDs, ',')) 
				AND DatafieldSetID > 0 
				AND SHORTNAME  != 'TOPICS'
				AND IsDeleted = 0
			ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

					
			if LEN(@standaloneUDFs) > 0
			Begin
				set @sColumns = ', SAUDFs.* '
				set @standAloneQuery= ' left outer join			
					(
						SELECT *
						 FROM
						 ( 
							SELECT gdf.groupID as SUDF_GID, edv.emailID as tmp_EmailID,  gdf.ShortName, REPLACE(DataValue,CHAR(9),CHAR(32)) AS DataValue
							from	EmailDataValues edv with (NOLOCK)  join  
									Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
							where 
									gdf.GroupID in (' + @GroupIDs + ')  and datafieldsetID is null 
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
					) 
					SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID  and emailgroups.groupID = SAUDFs.SUDF_GID '
			End

			if LEN(@TransactionalUDFs) > 0
			Begin

				set @tColumns = ', TUDFs.* '
				set @TransactionalQuery= '  left outer join
				(
					SELECT *
					 FROM
					 (
						SELECT gdf.groupID As TUDF_GID, edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, REPLACE(DataValue,CHAR(9),CHAR(32)) AS DataValue
						from	EmailDataValues edv with (NOLOCK)  join  
								Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID in (' + @GroupIDs + ')  and datafieldsetID > 0
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
				) 
				TUDFs on Emails.emailID = TUDFs.tmp_EmailID1  and emailgroups.groupID = TUDFs.TUDF_GID '
			End

			BEGIN TRY
					exec ('drop table ECN_Temp..MAF_' + @ClientName + '_Circ');
			END TRY
			BEGIN CATCH
			END CATCH; 


--remove tabs here		
IF @FullExport = 1 
BEGIN 
EXEC (' 
	select ' 
		+ @ClientID + ' as Client_ID, ''' 
		+ @clientName + ''' as ClientName, 
		Emails.EmailID, 
		REPLACE(EmailAddress,CHAR(9),CHAR(32)) AS EmailAddress,
		REPLACE(Title,CHAR(9),CHAR(32)) AS Title, 
		REPLACE(FirstName,CHAR(9),CHAR(32)) AS FirstName, 
		REPLACE(LastName,CHAR(9),CHAR(32)) AS LastName, 
		REPLACE(FullName,CHAR(9),CHAR(32)) AS FullName, 
		REPLACE(Company,CHAR(9),CHAR(32)) AS Company, 
		REPLACE(Occupation,CHAR(9),CHAR(32)) AS Occupation, 
		REPLACE(Address,CHAR(9),CHAR(32)) AS Address, 
		REPLACE(Address2,CHAR(9),CHAR(32)) AS Address2, 
		REPLACE(City,CHAR(9),CHAR(32)) AS City, 
		REPLACE(State,CHAR(9),CHAR(32)) AS State, 
		REPLACE(Zip,CHAR(9),CHAR(32)) AS Zip, 
		REPLACE(Country,CHAR(9),CHAR(32)) AS Country, 
		REPLACE(Voice,CHAR(9),CHAR(32)) AS Voice, 
		REPLACE(Mobile,CHAR(9),CHAR(32)) AS Mobile, 
		REPLACE(Fax,CHAR(9),CHAR(32)) AS Fax, 
		REPLACE(Website,CHAR(9),CHAR(32)) AS Website, 
		REPLACE(Age,CHAR(9),CHAR(32)) AS Age, 
		REPLACE(Income,CHAR(9),CHAR(32)) AS Income, 
		REPLACE(Gender,CHAR(9),CHAR(32)) AS Gender, 
		REPLACE(User1,CHAR(9),CHAR(32)) AS User1, 
		REPLACE(User2,CHAR(9),CHAR(32)) AS User2, 
		REPLACE(User3,CHAR(9),CHAR(32)) AS User3, 
		REPLACE(User4,CHAR(9),CHAR(32)) AS User4, 
		REPLACE(User5,CHAR(9),CHAR(32)) AS User5, 
		REPLACE(User6,CHAR(9),CHAR(32)) AS User6, 
		REPLACE(Birthdate,CHAR(9),CHAR(32)) AS Birthdate, 
		REPLACE(UserEvent1,CHAR(9),CHAR(32)) AS UserEvent1, 
		REPLACE(UserEvent1Date,CHAR(9),CHAR(32)) AS UserEvent1Date, 
		REPLACE(UserEvent2,CHAR(9),CHAR(32)) AS UserEvent2, 
		REPLACE(UserEvent2Date,CHAR(9),CHAR(32)) AS UserEvent2Date, 
		REPLACE(Convert(varchar,Notes),CHAR(9),CHAR(32)) AS Notes, 
		CreatedOn, 
		LastChanged = CASE WHEN LastChanged IS NULL THEN CreatedOn ELSE LastChanged END,
		FormatTypeCode, 
		SubscribeTypeCode, 
		Emailgroups.GroupId ' 
		+ @sColumns 
		+ @tColumns +  
		' into ECN_Temp..MAF_' + @ClientName + '_Circ ' +
		' from 
			Emails with (NOLOCK) 
			join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' 
			+ @standAloneQuery 
			+ @TransactionalQuery +  
		' where 
			EmailGroups.GroupID IN (' + @GroupIDs + ')')
END

IF @FullExport = 0 AND @StartDate = ''
BEGIN
EXEC (' 
	select ' 
		+ @ClientID + ' as Client_ID, ''' 
		+ @clientName + ''' as ClientName, 
		Emails.EmailID, 
		REPLACE(EmailAddress,CHAR(9),CHAR(32)) AS EmailAddress,
		REPLACE(Title,CHAR(9),CHAR(32)) AS Title, 
		REPLACE(FirstName,CHAR(9),CHAR(32)) AS FirstName, 
		REPLACE(LastName,CHAR(9),CHAR(32)) AS LastName, 
		REPLACE(FullName,CHAR(9),CHAR(32)) AS FullName, 
		REPLACE(Company,CHAR(9),CHAR(32)) AS Company, 
		REPLACE(Occupation,CHAR(9),CHAR(32)) AS Occupation, 
		REPLACE(Address,CHAR(9),CHAR(32)) AS Address, 
		REPLACE(Address2,CHAR(9),CHAR(32)) AS Address2, 
		REPLACE(City,CHAR(9),CHAR(32)) AS City, 
		REPLACE(State,CHAR(9),CHAR(32)) AS State, 
		REPLACE(Zip,CHAR(9),CHAR(32)) AS Zip, 
		REPLACE(Country,CHAR(9),CHAR(32)) AS Country, 
		REPLACE(Voice,CHAR(9),CHAR(32)) AS Voice, 
		REPLACE(Mobile,CHAR(9),CHAR(32)) AS Mobile, 
		REPLACE(Fax,CHAR(9),CHAR(32)) AS Fax, 
		REPLACE(Website,CHAR(9),CHAR(32)) AS Website, 
		REPLACE(Age,CHAR(9),CHAR(32)) AS Age, 
		REPLACE(Income,CHAR(9),CHAR(32)) AS Income, 
		REPLACE(Gender,CHAR(9),CHAR(32)) AS Gender, 
		REPLACE(User1,CHAR(9),CHAR(32)) AS User1, 
		REPLACE(User2,CHAR(9),CHAR(32)) AS User2, 
		REPLACE(User3,CHAR(9),CHAR(32)) AS User3, 
		REPLACE(User4,CHAR(9),CHAR(32)) AS User4, 
		REPLACE(User5,CHAR(9),CHAR(32)) AS User5, 
		REPLACE(User6,CHAR(9),CHAR(32)) AS User6, 
		REPLACE(Birthdate,CHAR(9),CHAR(32)) AS Birthdate, 
		REPLACE(UserEvent1,CHAR(9),CHAR(32)) AS UserEvent1, 
		REPLACE(UserEvent1Date,CHAR(9),CHAR(32)) AS UserEvent1Date, 
		REPLACE(UserEvent2,CHAR(9),CHAR(32)) AS UserEvent2, 
		REPLACE(UserEvent2Date,CHAR(9),CHAR(32)) AS UserEvent2Date, 
		REPLACE(Convert(varchar,Notes),CHAR(9),CHAR(32)) AS Notes, 
		CreatedOn, 
		LastChanged = CASE WHEN LastChanged IS NULL THEN CreatedOn ELSE LastChanged END,
		FormatTypeCode, 
		SubscribeTypeCode, 
		Emailgroups.GroupId ' 
		+ @sColumns 
		+ @tColumns +  
		' into ECN_Temp..MAF_' + @ClientName + '_Circ ' +
		' from 
			Emails with (NOLOCK) 
			join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' 
			+ @standAloneQuery 
			+ @TransactionalQuery +  
		' where 
			EmailGroups.GroupID IN (' + @GroupIDs + ') 
			AND (CONVERT(DATE,CreatedOn) = CONVERT(VARCHAR(10),'''+ @DiffDate +''',120 )
			OR CONVERT(DATE,LastChanged) = CONVERT(VARCHAR(10),'''+ @DiffDate +''',120 ))' )
END

IF @FullExport = 0 AND @startdate != ''
BEGIN
EXEC (' 
	select ' 
		+ @ClientID + ' as Client_ID, ''' 
		+ @clientName + ''' as ClientName, 
		Emails.EmailID, 
		REPLACE(EmailAddress,CHAR(9),CHAR(32)) AS EmailAddress,
		REPLACE(Title,CHAR(9),CHAR(32)) AS Title, 
		REPLACE(FirstName,CHAR(9),CHAR(32)) AS FirstName, 
		REPLACE(LastName,CHAR(9),CHAR(32)) AS LastName, 
		REPLACE(FullName,CHAR(9),CHAR(32)) AS FullName, 
		REPLACE(Company,CHAR(9),CHAR(32)) AS Company, 
		REPLACE(Occupation,CHAR(9),CHAR(32)) AS Occupation, 
		REPLACE(Address,CHAR(9),CHAR(32)) AS Address, 
		REPLACE(Address2,CHAR(9),CHAR(32)) AS Address2, 
		REPLACE(City,CHAR(9),CHAR(32)) AS City, 
		REPLACE(State,CHAR(9),CHAR(32)) AS State, 
		REPLACE(Zip,CHAR(9),CHAR(32)) AS Zip, 
		REPLACE(Country,CHAR(9),CHAR(32)) AS Country, 
		REPLACE(Voice,CHAR(9),CHAR(32)) AS Voice, 
		REPLACE(Mobile,CHAR(9),CHAR(32)) AS Mobile, 
		REPLACE(Fax,CHAR(9),CHAR(32)) AS Fax, 
		REPLACE(Website,CHAR(9),CHAR(32)) AS Website, 
		REPLACE(Age,CHAR(9),CHAR(32)) AS Age, 
		REPLACE(Income,CHAR(9),CHAR(32)) AS Income, 
		REPLACE(Gender,CHAR(9),CHAR(32)) AS Gender, 
		REPLACE(User1,CHAR(9),CHAR(32)) AS User1, 
		REPLACE(User2,CHAR(9),CHAR(32)) AS User2, 
		REPLACE(User3,CHAR(9),CHAR(32)) AS User3, 
		REPLACE(User4,CHAR(9),CHAR(32)) AS User4, 
		REPLACE(User5,CHAR(9),CHAR(32)) AS User5, 
		REPLACE(User6,CHAR(9),CHAR(32)) AS User6, 
		REPLACE(Birthdate,CHAR(9),CHAR(32)) AS Birthdate, 
		REPLACE(UserEvent1,CHAR(9),CHAR(32)) AS UserEvent1, 
		REPLACE(UserEvent1Date,CHAR(9),CHAR(32)) AS UserEvent1Date, 
		REPLACE(UserEvent2,CHAR(9),CHAR(32)) AS UserEvent2, 
		REPLACE(UserEvent2Date,CHAR(9),CHAR(32)) AS UserEvent2Date, 
		REPLACE(Convert(varchar,Notes),CHAR(9),CHAR(32)) AS Notes, 
		CreatedOn, 
		LastChanged = CASE WHEN LastChanged IS NULL THEN CreatedOn ELSE LastChanged END,
		FormatTypeCode, 
		SubscribeTypeCode, 
		Emailgroups.GroupId ' 
		+ @sColumns 
		+ @tColumns +  
		' into ECN_Temp..MAF_' + @ClientName + '_Circ ' +
		' from 
			Emails with (NOLOCK) 
			join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' 
			+ @standAloneQuery 
			+ @TransactionalQuery +  
		' where 
			EmailGroups.GroupID IN (' + @GroupIDs + ') 
			AND (
				CONVERT(DATE,CreatedOn)      BETWEEN CONVERT(VARCHAR(10),'''+ @StartDate +''',120 ) AND CONVERT(VARCHAR(10),'''+ @EndDate +''',120 )
				OR CONVERT(DATE,LastChanged) BETWEEN CONVERT(VARCHAR(10),'''+ @StartDate +''',120 ) AND CONVERT(VARCHAR(10),'''+ @EndDate +''',120 ))' 
				)
END
END
GO

