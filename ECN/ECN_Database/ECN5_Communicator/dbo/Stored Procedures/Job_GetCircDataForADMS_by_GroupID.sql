--USE [ECN5_COMMUNICATOR]
--GO

--IF EXISTS (SELECT 1 FROM Sysobjects where name = 'job_getCircDataForADMS_by_GroupID')
--DROP Proc job_getCircDataForADMS_by_GroupID
--GO

/****** Object:  StoredProcedure [dbo].[getCircDataForADMS_Updated]    Script Date: 02/03/2016 11:30:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'PentaVision', @GroupID = 305485, @StartDate = '1/1/2010', @EndDate = '5/21/2016', @FullExport = 0, @DiffExport = 1, @DaysOld = -1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'PentaVision', @GroupID = 305485, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'AIN', @GroupID = 327154, @FullExport = 0, @DaysOld = 0
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'AIN', @GroupID = 327154, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'FRANCE', @GroupID = 183883, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'FRANCE', @GroupID = 183883, @DiffExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'FRANCE', @GroupID = 183883, @DiffExport = 1, @DaysOld = -2
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MacFadden', @GroupID = 365507, @FullExport = 0, @DaysOld = -1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MacFadden', @GroupID = 374224, @FullExport = 0, @DaysOld = -7
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MacFadden', @GroupID = 376253, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MacFadden', @GroupID = 376584, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MacFadden', @GroupID = 376681, @FullExport = 0, @DaysOld = -91
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'SourceMedia', @GroupID = 383528, @StartDate = '7/14/2016', @EndDate = '7/14/2016 14:11', @FullExport = 0
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'SourceMedia', @GroupID = 383529, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'MTG', @GroupID = 188805, @FullExport = 0, @DaysOld = -42
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'ICD', @GroupID = 353961, @FullExport = 0, @DaysOld = -2
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'Meister', @GroupID = 185433, @FullExport = 0, @DaysOld = -1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'Pennwell', @GroupID = 418728, @FullExport = 1
-- Execute Job_GetCircDataForADMS_by_GroupID @ClientName = 'Pennwell', @GroupID = 418280, @StartDate = '12/31/2016', @EndDate = '3/1/2017 13:30', @FullExport = 0, @DiffExport = 0, @DaysOld = -60

--Select * from ecn_temp..MAF_AIN_Circ where emailID = 361048341
--Select * from ecn_temp..MAF_France_Circ 
--Select * from ecn_temp..MAF_PentaVision_Circ
--Select * from ecn_temp..MAF_Pennwell_Circ
--Select * from ecn_temp..MAF_France_Circ
--Select * from ecn_temp..MAF_MacFadden_Circ
--Select * from ecn_temp..MAF_MTG_Circ
--Select * from ecn_temp..MAF_CEX_Circ
--Select * from ecn_temp..MAF_ICD_Circ
--Select * from ecn_temp..MAF_Meister_Circ where Convert(Date,qualificationDate) > Convert(Date,'9/14/2016')
--Select * from ecn_temp..MAF_SourceMedia_Circ
--Select * from ecn_temp..MAF_PentaVision_Circ where Convert(Date, qualificationDate) < '4/22/2016' and business = 30 order by qualificationDate



CREATE PROCEDURE [dbo].[job_getCircDataForADMS_by_GroupID]
(
	@ClientName	varchar(50),
	@GroupID	int,
	@FullExport Bit = 0,
	@StartDate	Datetime = '',
	@EndDate	Datetime = '',
	@DiffExport	Bit = 0,
	@DaysOld	int = -1

)
AS
SET NOCOUNT ON 

BEGIN

	declare @ClientId INT

	DECLARE 
		@DiffDate DATE = DATEADD(DAY,@DaysOld,GETDATE()),
		@StandAloneUDFs VARCHAR(MAX),
		@TransactionalUDFs VARCHAR(MAX),
		@sColumns varchar(MAX),
		@tColumns varchar(MAX),
		@standAloneQuery varchar(MAX),
		@TransactionalQuery varchar(MAX),
		@SQL varchar(MAX),
		@Plus4Exists int,
		@ForZipExists int,
		@MexStateExists int
					
	SET @clientName = upper(@clientName)
	SET @StandAloneUDFs = ''
	SET @TransactionalUDFs = ''
	SET @sColumns = ''
	SET @tColumns = ''
	SET @standAloneQuery = ''
	SET @TransactionalQuery = ''

	IF @FullExport = 1
	BEGIN
		-- Set DiffExport =0, cannot be set to both full and diff.  Diff was just defaulted and Full overrides.
		SET @DiffExport = 0
	END

	SELECT  @Plus4Exists = Count(*) 
	FROM	groupdatafields 
	WHERE  
		ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
		and ShortName not in (select ShortName from GroupDatafields where Isdeleted =0 AND GroupID = @GroupID and DatafieldSetID > 0 ) 
		and GroupID = @GroupID
		AND DatafieldSetID is null 
		AND IsDeleted = 0
		AND ShortName = 'Plus4'

	SELECT  @ForZipExists = Count(*) 
	FROM	groupdatafields 
	WHERE  
		ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
		and ShortName not in (select ShortName from GroupDatafields where Isdeleted =0 AND GroupID = @GroupID and DatafieldSetID > 0 ) 
		and GroupID = @GroupID
		AND DatafieldSetID is null 
		AND IsDeleted = 0
		AND ShortName = 'ForZip'

	SELECT  @MexStateExists = Count(*) 
	FROM	groupdatafields 
	WHERE  
		ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
		and ShortName not in (select ShortName from GroupDatafields where Isdeleted =0 AND GroupID = @GroupID and DatafieldSetID > 0 ) 
		and GroupID = @GroupID
		AND DatafieldSetID is null 
		AND IsDeleted = 0
		AND ShortName = 'MEX_STATE'
			
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
				'Histbatch',
				'Job',
				'MAF_job',
				'MAF_Promocode',
				'Par3C',
				'PubCode',
				'Qdate',
				'QSource',
				'TransactionType',
				'Xact',
				'XactDate',
				'Gender'				-- added, this cause a ambiguous column error with the defined column 'gender'
			)
			
		ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
				
		SELECT  
			@TransactionalUDFs = 
				STUFF(( SELECT DISTINCT '],[' + ShortName 
				FROM groupdatafields 
				WHERE  
					ShortName in (select fieldname from ecn_temp..EnewsExportFields UNION Select 't_term') 
					and GroupID = @GroupID 
					AND DatafieldSetID > 0 
					AND SHORTNAME  != 'TOPICS'
					AND IsDeleted = 0
				ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

					
		if LEN(@standaloneUDFs) > 0
		Begin
			set @sColumns = ', 
				SAUDFs.* '
			set @standAloneQuery= ' 
				left outer join			
				(
					SELECT *
					 FROM
					 ( 
						SELECT 
							gdf.groupID as SUDF_GID, edv.emailID as tmp_EmailID,  gdf.ShortName, REPLACE(DataValue,CHAR(9),CHAR(32)) AS DataValue
						from	EmailDataValues edv with (NOLOCK)  
						join 	Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + CONVERT(VARCHAR(10), @GroupID) + ' and datafieldsetID is null 
								and
								gdf.shortname not in 
								(
									''Batch'',
									''Cat'',
									''Histbatch'',
									''Job'',
									''MAF_job'',
									''MAF_Promocode'',
									''Par3C'',
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
			set @tColumns = ', 
			TUDFs.* '
			set @TransactionalQuery= '
			left outer join
			(
				SELECT *
				 FROM
				 (
					SELECT 
						gdf.groupID As TUDF_GID, edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, REPLACE(DataValue,CHAR(9),CHAR(32)) AS DataValue
					from	EmailDataValues edv with (NOLOCK)  
					join 	Groupdatafields gdf with (NOLOCK) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
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
			REPLACE(CASE WHEN CharIndex(''kmpsgroup.com'', EmailAddress, 1) > 0 THEN '''' ELSE EmailAddress END ,CHAR(9),CHAR(32)) AS EmailAddress,
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
			+ @tColumns + ' 
			into ECN_Temp..MAF_' + @ClientName + '_Circ 
			from 
				Emails with (NOLOCK) 
				join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID 
				left join ECN_Temp..ECNUADResync with (NOLOCK) on ECNUADResync.EmailID = emailGroups.EmailID and ECNUADResync.GroupID = EmailGroups.GroupID and ECNUADResync.Processed = 0' 
				+ @standAloneQuery 
				+ @TransactionalQuery
				
	--remove tabs here	
	IF @FullExport = 1 
	BEGIN 
		SET @SQL = @SQL + ' 
			where 
				EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID) + '
				AND EmailGroups.SubscribeTypeCode <> ''M'' '
	END

	IF @FullExport = 0 AND @DiffExport = 0 -- and @startdate != ''
	BEGIN
		IF @EndDate = ''
		BEGIN
			-- Default to current Date
			SET @EndDate = GETDATE()
		END
		IF @StartDate = ''
		BEGIN
			-- Default to Daysold back from end date
			SET @StartDate = DATEADD(dd,@DaysOld, Convert(Date,@EndDate))
		END
		-- Get all rows that are greater than or equal to the starting date but less than the End Date (do not include those from the current date)
		
		SET @SQL = @SQL + ' 
			where 
				EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID) + ' 
				AND EmailGroups.SubscribeTypeCode <> ''M''
				AND (IsNull(SUBSCRIPTION,''N'') LIKE ''Y%'' OR (IsNull(SUBSCRIPTION,''N'') = ''N'' AND IsNull(SUBSCRIBERID, 0) <> 0) OR PAYMENTSTATUS = ''PAID'')
				AND (ISNULL(SUBSRC, '''') = ''NNWEB'' OR IsNull(t_TransactionID, '''') <> '''')
				AND ( (ISNULL(PAIDORFREE,'''') NOT IN ('''',''PENDING'',''NONQUALIFIED'') AND ISNULL(PAYMENTSTATUS, '''') <> ''PENDING'')  OR PAYMENTSTATUS = ''PAID'')
				AND ( (CreatedOn >= ''' + CONVERT(VARCHAR(30),@StartDate,120 ) + ''' AND CreatedOn < ''' + CONVERT(VARCHAR(30),@EndDate,120 ) + ''')
				OR (LastChanged >= ''' + CONVERT(VARCHAR(30),@StartDate,120 ) + ''' AND LastChanged < ''' + CONVERT(VARCHAR(30),@EndDate,120 ) + ''') 
				OR (ECNUADResync.EmailID is NOT NULL)  ) '
	END


	IF @FullExport = 0 AND @DiffExport = 1 --@StartDate = ''
	BEGIN
		IF @EndDate = ''
		BEGIN
			SET @EndDate = GETDATE()
		END
		
		SET @SQL = @SQL + ' 
			where 
				EmailGroups.GroupID = ' + Convert(varchar(10),@GroupID) + ' 
				AND EmailGroups.SubscribeTypeCode <> ''M''
				AND (IsNull(SUBSCRIPTION,''N'') LIKE ''Y%'' OR (IsNull(SUBSCRIPTION,''N'') = ''N'' AND IsNull(SUBSCRIBERID, 0) <> 0) OR PAYMENTSTATUS = ''PAID'')
				AND (ISNULL(SUBSRC, '''') = ''NNWEB'' OR IsNull(t_TransactionID, '''') <> '''')
				AND ( (ISNULL(PAIDORFREE,'''') NOT IN ('''',''PENDING'',''NONQUALIFIED'') AND ISNULL(PAYMENTSTATUS, '''') <> ''PENDING'')  OR PAYMENTSTATUS = ''PAID'')
				AND ( (CONVERT(DATE,CreatedOn) >= ''' + CONVERT(VARCHAR(10),@DiffDate,120 ) + ''' AND CONVERT(DATE,CreatedOn) < ''' + CONVERT(VARCHAR(10),CONVERT(DATE,@EndDate) ,120 ) + ''')
				OR (CONVERT(DATE,LastChanged) >= ''' + CONVERT(VARCHAR(10),@DiffDate,120 ) + ''' AND CONVERT(DATE,LastChanged) < ''' + CONVERT(VARCHAR(30),CONVERT(DATE,@EndDate),120 ) + ''')
				OR (ECNUADResync.EmailID is NOT NULL)  ) '
	END

	EXEC (@SQL)


	UPDATE ecn_Temp..ECNUADResync
		SET Processed = 1, ProcessedDate = GETDATE()
	WHERE GroupID = @GroupID and processed = 0

--print @SQL
--print @DiffDate
--print '*Scol*' + @sColumns
--print '*Tcol*' + @tColumns
--print '*SA*' + @standAloneQuery
--print '*TR*' + @TransactionalQuery
--print @GroupID

-- remove column because it has not been mapped.  It was just used to retrieve the correct records.
--EXEC ('alter table  ECN_Temp..MAF_' + @ClientName + '_Circ ' + 'DROP COLUMN PAYMENTSTATUS')

	IF @Plus4Exists > 0
	BEGIN
	--	Select zip, plus4,SUBSTRING(Zip, LEN(Zip) - 3, 4),* from ecn_Temp..MAF_TRADEPRESS_Circ where LEN(zip) > 5
		
		
		-- Split zipcode into zip code and plus4
		EXEC ('
			UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +  
				' SET PLUS4 = CASE WHEN LEN(ZIP) >= 9 AND (Country like ''United States%'' OR ISNULL(Country,'''') = '''') THEN SUBSTRING(Zip, LEN(Zip) - 3, 4) ELSE Plus4 END'
			 )
		EXEC ('
			UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' + 
				' SET Zip = CASE WHEN LEN(ZIP) > 5 AND (Country like ''United States%'' OR ISNULL(Country,'''') = '''') THEN SUBSTRING(Zip, 1, 5) ELSE Zip END'
			 )
	END

	IF @ForZipExists > 0
	BEGIN
		--UPDATE zipcode from ForZip if Zip is not empty
		EXEC ('
			UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +  
				' SET ZIP = CASE When IsNull(Zip, ''0'') = ''0'' THEN ForZip ELSE Zip END
			 ')
		 -- Remove Column FORZIP, it has not a mapped column for import
		EXEC ('alter table  ECN_Temp..MAF_' + @ClientName + '_Circ ' + 'DROP COLUMN FORZIP')
	END


	IF @MexStateExists > 0
	BEGIN
		-- Clear out the state field when country is mexico.  States are held in the MEX_State column and transformed by the engine process 
		EXEC ('
			UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +  
				' SET State = '''' where COUNTRY like ''MEXICO%'' and IsNull(state, '''') <> '''' and ISNULL(Mex_State, '''') <> ''''
			 ')

		-- Clear out the MEX_STATE field when country is not mexico.  States are held in the State column.
		EXEC ('
			UPDATE ECN_Temp..MAF_' + @ClientName + '_Circ ' +  
				' SET Mex_State = '''' where COUNTRY NOT LIKE ''MEXICO%''
			 ')
	END


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
GO


