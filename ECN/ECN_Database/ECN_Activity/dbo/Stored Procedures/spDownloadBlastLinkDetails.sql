CREATE PROCEDURE [dbo].[spDownloadBlastLinkDetails]
	@CampaignItemID int, 
	@CustomerID int,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@ProfileFilter varchar(25) = 'ProfilePlusStandalone',
	@OnlyUnique bit = 0,
	@LinkURL varchar(2048)
AS
BEGIN 	
	SET NOCOUNT ON
	if @StartDate is not null and @EndDate is not null
	BEGIN
		SET @StartDate = @StartDate + '00:00:00 '   
		SET @EndDate = @EndDate + '23:59:59'
	END
	CREATE TABLE #blasts 
	(
		blastID int,
		GroupID int, 
		SendTime Datetime
	unique clustered (blastID, GroupID)
	)

	DECLARE    
		@SqlString  varchar(8000)
		

	SET @SqlString = ''

	CREATE TABLE #tempA 
	(  
		BlastID int,
		GroupID int,
		EmailID int,  
		ActionDate datetime,
		ActionValue varchar(2048),
		ActionNotes  varchar(355)
	)
	
	INSERT INTO #blasts (BlastId,Groupid,SendTime)
		SELECT 
			blastID, 
			groupID,
			SendTime
		FROM 
			ecn5_communicator.dbo.Blast
		WHERE 
			BlastID IN
			(SELECT blastid FROM ecn5_communicator..CampaignItemBlast WHERE CampaignItemID = @CampaignItemID AND IsDeleted = 0) AND	CustomerID = @CustomerID AND StatusCode <> 'Deleted'
   
   if exists(select top 1 * from #Blasts where GroupID is null)
	BEGIN
		declare @currentBlastID int,
				 @refBlastID int,					
					@groupID int,
					@layoutPlanID int,
					@IsFormTrig bit,
					@campItemID int,
					@index int = 0
		declare mycursor cursor
		FOR
		Select BlastID, GroupID from #Blasts where GroupID is null
		OPEN mycursor
		FETCH NEXT FROM mycursor into @currentBlastID, @groupID
		WHILE @@FETCH_STATUS = 0
		BEGIN
			SET  @refBlastID = null
			SET  @layoutPlanID = null
			SET  @IsFormTrig = null
			SET @campItemID = null
			SET @refBlastID = @currentBlastID
			while @groupID is null and @index < 10
			BEGIN
				
				select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID , @layoutPlanID = LayoutPlanID
				from ECN5_COMMUNICATOR..LayoutPlans lp with(nolock)
				where lp.BlastID = @refBlastID and ISNULL(lp.IsDeleted,0) = 0 and lp.Status = 'y'
				
				
				if @layoutPlanID is not null and @campItemID is not null
				BEGIN
					--must be a layout plan
					select @refBlastID = BlastID 
					from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) 
					where cib.CampaignItemID = @campItemID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE IF @layoutPlanID is not null and @campItemID is null
				BEGIN
					select @layoutPlanID = MIN(LayoutPlanID), @refBlastID = MIN(bs.refblastID)
					from ECN5_COMMUNICATOR..BlastSingles bs with(nolock) 
					where bs.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE
				BEGIN		
					--must be a trigger plan
					select @refBlastID = tp.RefTriggerID 
					from ECN5_COMMUNICATOR..TriggerPlans tp with(nolock)
					where tp.BlastID = @refBlastID and ISNULL(tp.IsDeleted,0) = 0 and tp.Status = 'y'
			
					select @campItemID = CampaignItemID
					from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) 
					where cib.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				SET @index = @index + 1
			END
			
			SET @index = 0
			if @groupID is not null
			BEGIN
				update #Blasts
				set GroupID = @groupID
				where BlastID = @currentBlastID
				SET @groupID = null
			END
			FETCH NEXT FROM mycursor into @refBlastID, @groupID
		END		
		close mycursor 
		deallocate mycursor
	END			






		INSERT INTO #tempA  
				SELECT distinct     
					  bs.blastID, 
					  bs.groupID, 
					  e.EmailID, 
					  MAX(bacl.ClickTime) AS 'ActionDate', 
					  bacl.URL AS 'ActionValue', 
					  ''
				FROM
					  BlastActivityClicks bacl WITH(NOLOCK) 
					  join ECN5_COMMUNICATOR..BlastLinks bl with(nolock) on bacl.BlastID = bl.BlastID
					  join ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
					  JOIN #blasts bs ON bs.blastID = bacl.BlastID
				WHERE  bl.linkurl = @LinkURL
				group by bs.blastID, bs.GroupID, e.EmailID, bacl.URL


	
    DECLARE @g TABLE(GID int, ShortName varchar(50))        

	INSERT INTO @g   
	SELECT
		GroupDatafieldsID, 
		ShortName 
	FROM
		ecn5_communicator..groupdatafields gdf 
		JOIN #blasts b ON b.GroupID = gdf.GroupID
	WHERE
		gdf.IsDeleted = 0

	CREATE NONCLUSTERED INDEX idx_tempA_1 ON #tempA (EmailID, groupID)
	
	IF NOT EXISTS(SELECT TOP 1 * FROM @g)
	BEGIN  
		EXEC( 'SELECT  EmailAddress,Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' +  
		' FROM  ecn5_communicator.dbo.Emails WITH (NOLOCK) '+
		' JOIN ecn5_communicator.dbo.EmailGroups WITH (NOLOCK)  ON EmailGroups.EmailID = Emails.EmailID ' +   
		' JOIN #tempA ON #tempA.EmailID = Emails.EmailID AND emailgroups.groupID = #tempA.groupID'+  
		' ORDER BY #tempA.ActionDate desc, emailaddress')  
	END  
	ELSE
	BEGIN

		DECLARE @StandAloneUDFs VARCHAR(MAX)
		if(@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @StandAloneUDFs = STUFF((	SELECT 
												DISTINCT '],[' + ShortName 
											FROM 
												ecn5_communicator..groupdatafields gdf 
												JOIN #blasts g ON g.GroupID = gdf.GroupID 
											WHERE 
												IsDeleted = 0 
												AND DatafieldSetID IS NULL 
											ORDER BY 
												'],[' + ShortName 
											FOR XML PATH('') ), 1, 2, '') + ']'
		END

		DECLARE @TransactionalUDFs VARCHAR(MAX)
		IF(@ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @TransactionalUDFs = STUFF((SELECT 
												DISTINCT '],[' + ShortName 
											FROM 
												ecn5_communicator..groupdatafields gdf 
												JOIN #blasts g ON g.GroupID = gdf.GroupID 
											WHERE 
												IsDeleted = 0 
												AND DatafieldSetID > 0 
											ORDER BY 
												'],[' + ShortName 
											FOR XML PATH('') ), 1, 2, '') + ']'       
		END
		DECLARE @sColumns varchar(MAX),
				@tColumns varchar(MAX),
				@standAloneQuery varchar(MAX),
				@TransactionalQuery varchar(MAX)
				
		SET @sColumns = ''
		SET @tColumns= ''
		SET @standAloneQuery = ''
		SET @TransactionalQuery = ''		
				
		IF LEN(@standaloneUDFs) > 0
		BEGIN
			SET @sColumns = ', SAUDFs.* '
			SET @standAloneQuery= ' left outer JOIN			
				(
					SELECT *
					 FROM
					 (
						SELECT edv.emailID AS tmp_EmailID,  gdf.ShortName, edv.DataValue
						FROM	
							ecn5_communicator..EmailDataValues edv WITH (NOLOCK) 
							JOIN ecn5_communicator..Groupdatafields gdf WITH(nolock) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
							JOIN #tempA ON #tempA.EmailID = edv.EmailID 
							JOIN #blasts g ON g.GroupID = gdf.GroupID
						WHERE 
							datafieldsetID is null 
							AND gdf.IsDeleted = 0 
					 ) u
					 PIVOT
					 (
					 MAX (DataValue)
					 FOR ShortName in (' + @StandAloneUDFs + ')) AS pvt 			
				) 
				SAUDFs ON Emails.emailID = SAUDFs.tmp_EmailID'
		END
		
		IF LEN(@TransactionalUDFs) > 0
		BEGIN

			SET @tColumns = ', TUDFs.* '
			SET @TransactionalQuery= '  left outer JOIN
			(
				SELECT *
				 FROM
				 (
					SELECT edv.emailID AS tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
					FROM
						ecn5_communicator..EmailDataValues edv WITH (NOLOCK)
						JOIN ecn5_communicator..Groupdatafields gdf with(nolock) ON edv.GroupDatafieldsID = gdf.GroupDatafieldsID 
						JOIN #tempA ON #tempA.EmailID = edv.EmailID JOIN #blasts g ON g.GroupID = gdf.GroupID
					WHERE 
						datafieldsetID > 0 
						AND gdf.IsDeleted = 0 
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) AS pvt 			
			) 
			TUDFs ON Emails.emailID = TUDFs.tmp_EmailID1 '
		END
		
		EXEC ('SELECT EmailAddress,#tempA.blastID, #tempA.groupID,Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' FROM  ' +       
			' ecn5_communicator..Emails WITH (NOLOCK) JOIN '+    
			' ecn5_communicator..EmailGroups WITH (NOLOCK)  ON EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +    
			' JOIN #tempA ON #tempA.EmailID = Emails.EmailID AND emailgroups.groupID = #tempA.groupID'+  
			' ORDER BY #tempA.ActionDate desc, emailaddress')  


	END
	
	 DROP TABLE #tempA 
	 DROP TABLE #blasts 

	SET NOCOUNT OFF

END

