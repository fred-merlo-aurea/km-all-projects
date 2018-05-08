USE [ECN5_COMMUNICATOR]
GO

IF EXISTS (SELECT 1 FROM Sysobjects where name = 'getEnewsDataForADMS')
DROP Proc getEnewsDataForADMS
GO


/****** Object:  StoredProcedure [dbo].[getEnewsDataForADMS]    Script Date: 02/23/2016 12:32:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

---------------------------------
--Created MK 2014-11-14 
-- Procedure is called by SSIS package to export data to FTP for ADMS process
-- Removed TAB characters for Tab delimited export
-----------------------------------

CREATE PROCEDURE [dbo].[getEnewsDataForADMS]
(
	@clientName Varchar(50),
	@FullExport Bit = 0,
	@DaysOld int = -1,
	@EndDate DATE = NULL,
	@GroupID int = 0
)
as
Begin

	-- EXECUTE getEnewsDataForADMS 'CANON', 0, -103, '3/9/2016'
	-- EXECUTE getEnewsDataForADMS 'EHPublishing', 0, -103, '3/9/2016'
	-- EXECUTE getEnewsDataForADMS 'ICD', 0, -10, '8/19/2016'
	-- EXECUTE getEnewsDataForADMS 'Scranton', 0, -10, '8/19/2016'
	-- EXECUTE getEnewsDataForADMS 'TradePress', 0, -10, '8/19/2016'
	-- EXECUTE getEnewsDataForADMS 'NoriaCorp', 1, -1, '3/9/2016'
	-- EXECUTE getEnewsDataForADMS 'MacFadden', 0, -2, '7/21/2016'
	-- EXECUTE getEnewsDataForADMS 'tenMissions', 0, -20, '10/25/2016'
	-- EXECUTE getEnewsDataForADMS 'TradePress', 0, -1,  '8/6/2016'
	-- EXECUTE getEnewsDataForADMS 'NECA', 0, -10, '2/27/2017'
	-- Select * from ecn_temp.dbo.MAF_CANON_Enews where lastchanged > '3/10/2016'
	-- Select * from ecn_temp.dbo.MAF_MacFadden_Enews 
	-- Select * from ecn_temp.dbo.MAF_ICD_Enews 
	-- Select * from ecn_temp.dbo.MAF_NECA_Enews 
	-- Select * from ecn_temp.dbo.MAF_Tradepress_Enews where emailaddress = 'b.dahl@hubbell.com'
	-- select MIN(lastChanged), MAX(lastchanged) from ecn_temp.dbo.MAF_CANON_Enews
--select * from ecn_temp.dbo.MAF_tenmissions_enews where groupid = 408572 

	set NOCOUNT ON  
	
	IF @DaysOld > 0
	BEGIN
		Set @DaysOld = @DaysOld * -1
	END 	
 
	DECLARE @SQLStatement varchar(max)
	declare @ClientID int,
			@SubscribeTypeCodeQuery varchar(800) = ''
	DECLARE @StartDate DATE
	DECLARE @AddPubCode int
	
	
	SELECT @EndDate  = IsNull(@EndDate, GETDATE())
	SELECT @StartDate = DATEADD(DAY,@DaysOld,@EndDate)
	
	IF @StartDate = @EndDate
	BEGIN
		-- Only if @DaysOld = 0 THEN we want the current date,  Set EndDate to Next Day. 
		-- Otherwise, we just want to get everything from DaysOld to less than the current date.
		SET @EndDate = DATEADD(DAY,1,GETDATE())
	END 
	
	set @clientName = upper(@clientName)
	 
	create table #groups (grpID int Primary Key, Pubcode varchar(50), EnewsTransformGrouptoPubCode bit)

	select top 1 @ClientID = clientID from dbo.UAD_ENEWS_SYNC
	where client = @clientName
	IF @@ROWCOUNT = 0
	BEGIN
		print 'ERROR - Client is not setup in UAD_ENEWS_SYNC Table'
	END 

	-- if any of the rows for the client are set to transform, then PubCode will be added to the file
	select @AddPubCode = MAX(CONVERT(int,EnewsTransformGrouptoPubCode)) from dbo.UAD_ENEWS_SYNC
	where client = @clientName
	IF @@ROWCOUNT = 0
	BEGIN
		print 'ERROR - Client is not setup in UAD_ENEWS_SYNC Table'
	END 

	IF @GroupID = 0
	BEGIN	
		insert into #groups
			select distinct groupID, Pubcode, EnewsTransformGrouptoPubCode from dbo.UAD_ENEWS_SYNC
			where client = @clientName and IsActive = 1
	END
	ELSE
	BEGIN
		-- selecting just 1 group, make sure it is also configured in UAD_Enews... table
		insert into #groups
			select distinct groupID, Pubcode, EnewsTransformGrouptoPubCode from dbo.UAD_ENEWS_SYNC
			where groupID = @GroupID and client = @clientName and IsActive = 1
	END
	
	if (Select COUNT(*) from #groups) = 0
	BEGIN
		print 'ERROR - Client is not setup with groups in UAD_ENEWS_SYNC Table'
	END


	-- Per TFS ticket 44570, allow master Suppressed
	--if (@clientName = 'TRADEPRESS')
	--Begin
	--	set @SubscribeTypeCodeQuery = ' AND EmailGroups.SubscribeTypeCode <> ''M'''
	--End

	DECLARE @StandAloneUDFs VARCHAR(MAX)
	DECLARE @TransactionalUDFs VARCHAR(MAX)
	declare @sColumns varchar(MAX),
			@tColumns varchar(MAX),
			@standAloneQuery varchar(MAX),
			@TransactionalQuery varchar(MAX)
			
	set @StandAloneUDFs = ''
	set @TransactionalUDFs = ''
	set @sColumns = ''
	set @tColumns = ''
	set @standAloneQuery = ''
	set @TransactionalQuery = ''
	
	
	SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName 
	FROM groupdatafields 
	WHERE  
		ISNULL(groupdatafields.IsDeleted,0) = 0 and
		ShortName in 
	(select fieldname from ecn_temp..EnewsExportFields) and 
	ShortName not in (select ShortName from GroupDatafields where GroupID in (select grpID from #groups) and 
	DatafieldSetID > 0 ) and GroupID in (select grpID from #groups) AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'


	if (@clientName = 'UPI')
	Begin
		-- Allow SHORTNAME of TOPICS for UPI
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName 
		FROM groupdatafields 
		WHERE  
			ISNULL(groupdatafields.IsDeleted,0) = 0 and
			ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
			and GroupID in (select grpID from #groups)
			AND DatafieldSetID > 0 
		ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
	End
	Else
	Begin
		-- SKIP SHORTNAME of TOPICS for most Clients
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName 
		FROM groupdatafields 
		WHERE  
			ISNULL(groupdatafields.IsDeleted,0) = 0 and
			ShortName in (select fieldname from ecn_temp..EnewsExportFields) 
			and GroupID in (select grpID from #groups)
			AND DatafieldSetID > 0 
			AND SHORTNAME  != 'TOPICS'
		ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
	END
				
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
							ISNULL(gdf.IsDeleted,0) = 0 and gdf.GroupID in (select grpID from #groups)  and datafieldsetID is null
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
			) 
			SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID  and emailgroups.groupID = SAUDFs.SUDF_GID '
	End
-- Task 12634 commented out, then subsequently re-added
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
						ISNULL(gdf.IsDeleted,0) = 0 and gdf.GroupID in (select grpID from #groups)  and datafieldsetID > 0
			 ) u
			 PIVOT
			 (
			 MAX (DataValue)
			 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
		) 
		TUDFs on Emails.emailID = TUDFs.tmp_EmailID1  and emailgroups.groupID = TUDFs.TUDF_GID '
	End

	BEGIN TRY
			exec ('drop table ECN_Temp..MAF_' + @ClientName + '_Enews');
	END TRY
	BEGIN CATCH
	END CATCH; 

	SELECT @SQLStatement = 	
		'SELECT ' +
			CONVERT(varchar(20),@ClientID) + ' as Client_ID, ''' +
			@clientName + ''' as ClientName, 
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
			'
		if @clientName = 'ICD'
		Begin
			SELECT @SQLStatement = @SQLStatement + '
			REPLACE(Convert(varchar,Password),CHAR(9),CHAR(32)) AS Password, '
		End	
		SELECT @SQLStatement = @SQLStatement + '
			CreatedOn, 
			LastChanged, 
			FormatTypeCode, 
			SubscribeTypeCode, 
			CASE 
			WHEN Subscribetypecode = ''P'' THEN 7	--Pending ?
			WHEN Subscribetypecode = ''S'' THEN 1	--Subscribed / Active
			WHEN Subscribetypecode = ''U'' THEN 6	--Unsubscribed
			WHEN Subscribetypecode = ''M'' THEN 4	--MasterSuppressed
			WHEN Subscribetypecode = ''B'' THEN 3	--Bad /Invalid
			WHEN Subscribetypecode = ''D'' THEN 7	--Deleted / 
			END	 AS EmailStatusId,
			Emailgroups.GroupId 
			' 
		IF @AddPubCode = 1
		BEGIN
			SELECT @SQLStatement = @SQLStatement + ',' + 
			'CASE when IsNull(t.EnewsTransformGrouptoPubCode, 1) = 1 then REPLACE(Convert(varchar,IsNull(t.PubCode,'''')),CHAR(9),CHAR(32)) ELSE '''' END AS ECNPubCode'
		END
		SELECT @SQLStatement = @SQLStatement
			+ @sColumns 
			+ @tColumns +  
			' into ECN_Temp..MAF_' + @ClientName + '_Enews ' +
			' from 
				Emails with (NOLOCK) 
				join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID 
				join #groups t on t.grpID = EmailGroups.GroupID ' 
				+ @standAloneQuery 
				+ @TransactionalQuery
				
	IF @FullExport = 1 
	BEGIN 
		-- ADD in Where Clause
		SELECT @SQLStatement = @SQLStatement + ' WHERE 1=1 '
--				EmailGroups.GroupID IN (select grpID from #groups) ' 
				+ @SubscribeTypeCodeQuery
	END

	IF @FullExport = 0
	BEGIN
		-- ADD in DIFF Where Clause
		SELECT @SQLStatement = @SQLStatement + ' WHERE 1=1 '
--				EmailGroups.GroupID IN (select grpID from #groups) ' 
				+ @SubscribeTypeCodeQuery + '
				AND (
				( CONVERT(DATE,CreatedOn) >= ''' + CONVERT(VARCHAR(10),@StartDate,120 ) + '''  AND CONVERT(DATE,CreatedOn) < ''' + CONVERT(VARCHAR(10),@EndDate,120 )  + ''')
				OR 
				( CONVERT(DATE,LastChanged) >= ''' + CONVERT(VARCHAR(10),@StartDate,120 ) + ''' AND CONVERT(DATE,LastChanged) < ''' + CONVERT(VARCHAR(10), @EndDate,120 ) + '''))'
	END
--	Print @SQLStatement
	EXEC (@SQLStatement)
	EXEC (' 
		UPDATE ECN_Temp..MAF_' + @ClientName + '_Enews ' +
			' SET emailaddress = '''' '+  
			' where emailaddress like ''%kmpsgroup.com%'' '
		 )
END


GO


