-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'AIN', @GroupID = 327154, @StartDate = '4/1/2016', @EndDate = '4/5/2016', @FullExport = 0
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'AIN', @GroupID = 327154, @FullExport = 0
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'AIN', @GroupID = 327154, @FullExport = 1
--Select * from ecn_temp..MAF_AIN_Circ


CREATE PROCEDURE [dbo].[job_getCircDataForADMS_by_GroupID]
(
	@ClientName	varchar(50),
	@GroupID	int,
	@FullExport Bit = 0,
	@StartDate	Date = '',
	@EndDate	Date = ''

)
AS
SET NOCOUNT ON 

BEGIN

	declare @ClientId INT

	DECLARE 
		@DiffDate DATE = DATEADD(DAY,-1,GETDATE()),
		@StandAloneUDFs VARCHAR(MAX),
		@TransactionalUDFs VARCHAR(MAX),
		@sColumns varchar(MAX),
		@tColumns varchar(MAX),
		@standAloneQuery varchar(MAX),
		@TransactionalQuery varchar(MAX),
		@SQL varchar(MAX)
					
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
				and ShortName not in (select ShortName from GroupDatafields where Isdeleted =0 AND GroupID = @GroupID and DatafieldSetID > 0 ) 
				and GroupID = @GroupID
				AND DatafieldSetID is null 
				AND IsDeleted = 0
				AND ShortName not in
				(
					'Batch',
					'Cat',
					'ForZip',
					'Histbatch',
					'Job',
					'Par3C',
					'PaymentStatus',
					'PubCode',
					'Qdate',
					'QSource',
					'TransactionType',
					'Xact',
					'XactDate'
				)
				
			ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
				
			SELECT  
				@TransactionalUDFs = 
					STUFF(( SELECT DISTINCT '],[' + ShortName 
					FROM groupdatafields 
					WHERE  
						ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
						and GroupID = @GroupID 
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
									gdf.GroupID = ' + CONVERT(VARCHAR(10), @GroupID) + ' and datafieldsetID is null 
									and
									gdf.shortname not in 
									(
										''Batch'',
										''Cat'',
										''ForZip'',
										''Histbatch'',
										''Job'',
										''Par3C'',
										''PaymentStatus'',
										''PubCode'',
										''Qdate'',
										''QSource'',
										''TransactionType'',
										''Xact'',
										''XactDate''
									)
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
								gdf.GroupID = ' + CONVERT(VARCHAR(10), @GroupID) + ' and datafieldsetID > 0
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

SET @SQL = '
	select 
		Emails.EmailID, 
		REPLACE(EmailAddress,CHAR(9),CHAR(32)) AS EmailAddress,
		REPLACE(Title,CHAR(9),CHAR(32)) AS Title, 
		REPLACE(FirstName,CHAR(9),CHAR(32)) AS FirstName, 
		REPLACE(LastName,CHAR(9),CHAR(32)) AS LastName, 
		REPLACE(Company,CHAR(9),CHAR(32)) AS Company, 
		REPLACE(Address,CHAR(9),CHAR(32)) AS Address, 
		REPLACE(Address2,CHAR(9),CHAR(32)) AS Address2, 
		REPLACE(City,CHAR(9),CHAR(32)) AS City, 
		REPLACE(State,CHAR(9),CHAR(32)) AS State, 
		REPLACE(Zip,CHAR(9),CHAR(32)) AS Zip, 
		REPLACE(Country,CHAR(9),CHAR(32)) AS Country, 
		REPLACE(Voice,CHAR(9),CHAR(32)) AS Phone, 
		REPLACE(Mobile,CHAR(9),CHAR(32)) AS Mobile, 
		REPLACE(Fax,CHAR(9),CHAR(32)) AS Fax, 
		REPLACE(Website,CHAR(9),CHAR(32)) AS Website, 
		REPLACE(Age,CHAR(9),CHAR(32)) AS Age, 
		REPLACE(Income,CHAR(9),CHAR(32)) AS Income, 
		REPLACE(Gender,CHAR(9),CHAR(32)) AS Gender, 
		QualificationDate = convert(varchar(100), CASE WHEN LastChanged IS NULL THEN CreatedOn ELSE LastChanged END, 23),
		SubscribeTypeCode, 
		Emailgroups.GroupId ' 
		+ @sColumns 
		+ @tColumns +  
		' into ECN_Temp..MAF_' + @ClientName + '_Circ ' +
		' from 
			Emails with (NOLOCK) 
			join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' 
			+ @standAloneQuery 
			+ @TransactionalQuery
			
--remove tabs here		
IF @FullExport = 1 
BEGIN 
	SET @SQL = @SQL + ' 
		where 
			EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID)
END

IF @FullExport = 0 AND @StartDate = ''
BEGIN
	SET @SQL = @SQL + ' 
		where 
			EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID) + ' 
			AND (CONVERT(DATE,CreatedOn) = ''' + CONVERT(VARCHAR(10),@DiffDate,120 ) + '''
			OR CONVERT(DATE,LastChanged) = ''' + CONVERT(VARCHAR(10),@DiffDate,120 ) + ''')
			AND IsNull(LastChangedSource,'''')  <> ''SSIS.JobSyncUADCircDataToECN''  ' 
END

IF @FullExport = 0 AND @startdate != ''
BEGIN
	SET @SQL = @SQL + ' 
		where 
			EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID) + '
			 AND (
				CONVERT(DATE,CreatedOn)      BETWEEN ''' + CONVERT(VARCHAR(10),@StartDate,120 ) + ''' AND ''' + CONVERT(VARCHAR(10),@EndDate,120 ) + '''
				OR CONVERT(DATE,LastChanged) BETWEEN ''' + CONVERT(VARCHAR(10),@StartDate,120 ) + ''' AND ''' + CONVERT(VARCHAR(10),@EndDate,120 ) + ''')
				--AND IsNull(LastChangedSource,'''')  <> ''SSIS.JobSyncUADCircDataToECN''  ' 
END

EXEC (@SQL)

--print @SQL
--print @DiffDate
--print '*Scol*' + @sColumns
--print '*Tcol*' + @tColumns
--print '*SA*' + @standAloneQuery
--print '*TR*' + @TransactionalQuery
--print @GroupID


BEGIN TRY
	BEGIN TRY
	EXEC (' 
		UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +
			' SET SUBSRC = CASE WHEN ISNULL(promocode, '''') <> '''' THEN promocode  WHEN ISNULL(Demo39, '''') <> '''' THEN DEMO39  ELSE SUBSRC END' 		
		 )
	END TRY
	BEGIN CATCH
		BEGIN TRY
			EXEC (' 
				UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +
					' SET SUBSRC = CASE WHEN ISNULL(Demo39, '''') <> '''' THEN DEMO39  ELSE SUBSRC END' 		
				 )
		 END TRY
		 BEGIN CATCH
			-- group may be missing SUBSRC and Demo39 columns
		END CATCH
	END CATCH
END TRY
BEGIN CATCH
END CATCH
  -- Do not catch error,  some groups do not have a PUBSRC column
END