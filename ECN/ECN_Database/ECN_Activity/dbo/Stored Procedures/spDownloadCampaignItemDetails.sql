CREATE PROCEDURE [dbo].[spDownloadCampaignItemDetails]
	@CampaignItemID int, 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100) ,
	@CustomerID int,
	@StartDate datetime = null,
	@EndDate datetime = null,
	@ProfileFilter varchar(25) = 'ProfilePlusStandalone',
	@OnlyUnique bit = 0
	
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
		@SqlString  varchar(8000),
		@ReportColumns varchar(500)

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
   
	IF @ReportType = 'send'
	BEGIN
		INSERT INTO #tempA  
		SELECT  
			bs.blastID, 
			bs.groupID, 
			e.EmailID, 
			bas.SendTime AS 'ActionDate', 
			'send' AS 'ActionValue' , 
			''
		FROM
			BlastActivitySends bas WITH(NOLOCK) 
			JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bas.EmailID = e.EmailID 
			JOIN #blasts bs ON bs.blastID = bas.BlastID
		WHERE
			(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP)

		SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END
	ELSE IF @ReportType = 'open'
	BEGIN
		IF @FilterType = 'all'
			INSERT INTO #tempA  
			SELECT  
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				baop.OpenTime AS 'ActionDate', 
				baop.BrowserInfo AS 'ActionValue', 
				''
			FROM
				BlastActivityOpens baop WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON e.EmailID = baop.EmailID 
				JOIN #blasts bs ON bs.blastID = baop.BlastID
			WHERE
				(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP)

		ELSE
			INSERT INTO #tempA  
			SELECT	
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				MAX(baop.OpenTime), 
				MAX(baop.BrowserInfo), 
				''
			FROM
				BlastActivityOpens baop WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON baop.EmailID = e.EmailID 
				JOIN #blasts bs ON bs.blastID = baop.BlastID
			WHERE
				(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP)
			GROUP BY
				bs.blastID, 
				bs.groupID, 
				e.EmailID

		SET @ReportColumns = ' #tempA.ActionDate AS OpenTime, #tempA.ActionValue AS Info '
	END
	ELSE IF @ReportType = 'noopen'
	BEGIN
			INSERT INTO #tempA  
			SELECT	
				bs.blastID, 
				bs.groupID, 
				bas.EmailID, 
				bas.sendtime AS 'ActionDate', 
				'', 
				'' 
			FROM
				BlastActivitySends bas WITH(NOLOCK)  
				JOIN #blasts bs ON bs.blastID = bas.BlastID	
			WHERE
				bas.EmailID NOT IN (SELECT EmailID FROM BlastActivityOpens bao JOIN #blasts bs ON bs.blastID = bao.BlastID)
			
		SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END
	ELSE IF @ReportType = 'click'
	BEGIN
		IF @FilterType = 'all'
			IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
				INSERT INTO #tempA  
				SELECT	
					bs.blastID, 
					bs.groupID, 
					e.EmailID, 
					bacl.ClickTime AS 'ActionDate', 
					bacl.URL AS 'ActionValue', 
					''
				FROM
					BlastActivityClicks bacl WITH(NOLOCK) 
					JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
					JOIN #blasts bs ON bs.blastID = bacl.BlastID
				WHERE
					(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP) 
					AND bacl.ClickTime BETWEEN @StartDate AND @EndDate
			ELSE
				INSERT INTO #tempA  
				SELECT	
					bs.blastID, 
					bs.groupID, 
					e.EmailID, 
					bacl.ClickTime AS 'ActionDate', 
					bacl.URL AS 'ActionValue', 
					''
				FROM
					BlastActivityClicks bacl WITH(NOLOCK) 
					JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
					JOIN #blasts bs ON bs.blastID = bacl.BlastID
				WHERE
					(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP)
		ELSE
			IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
				INSERT INTO #tempA  
				SELECT	
					bs.blastID, 
					bs.groupID, 
					e.EmailID, 
					max(bacl.ClickTime) AS 'ActionDate', 
					bacl.URL AS 'ActionValue', 
					''
				FROM
					BlastActivityClicks bacl WITH(NOLOCK) 
					JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
					JOIN #blasts bs ON bs.blastID = bacl.BlastID
				WHERE	
					(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
					and bacl.ClickTime BETWEEN @StartDate AND @EndDate
				GROUP BY 
					bs.blastID, 
					bs.groupID, 
					e.EmailID , 
					bacl.URL
			ELSE
				INSERT INTO #tempA  
				SELECT	
					bs.blastID, 
					bs.groupID, 
					e.EmailID, 
					max(bacl.ClickTime) AS 'ActionDate', 
					bacl.URL AS 'ActionValue', 
					''
				FROM
					BlastActivityClicks bacl WITH(NOLOCK) 
					JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
					JOIN #blasts bs ON bs.blastID = bacl.BlastID
				WHERE
					(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP)
				GROUP BY
					bs.blastID, 
					bs.groupID, 
					e.EmailID , 
					bacl.URL
		
		SET @ReportColumns = ' #tempA.ActionDate AS ClickTime, #tempA.ActionValue AS Link '
	END
	ELSE IF @ReportType = 'uniqueclicks'
	BEGIN
		IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
			INSERT INTO #tempA  
			SELECT	distinct
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				max(bacl.ClickTime) AS 'ActionDate', 
				'' AS 'ActionValue', 
				''
			FROM
				BlastActivityClicks bacl WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
				JOIN #blasts bs ON bs.blastID = bacl.BlastID
			WHERE	
				(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
				and bacl.ClickTime BETWEEN @StartDate AND @EndDate
			GROUP BY 
				bs.blastID, 
				bs.groupID, 
				e.EmailID
				
		ELSE
			INSERT INTO #tempA  
			SELECT	distinct
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				max(bacl.ClickTime) AS 'ActionDate', 
				'' AS 'ActionValue', 
				''
			FROM
				BlastActivityClicks bacl WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
				JOIN #blasts bs ON bs.blastID = bacl.BlastID
			WHERE
				(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP)
			GROUP BY
				bs.blastID, 
				bs.groupID, 
				e.EmailID 
		
		SET @ReportColumns = ' #tempA.ActionDate AS ClickTime  '
	END
	ELSE IF @ReportType = 'noclick'
	BEGIN
			INSERT INTO #tempA  
			SELECT	
				bs.blastID, 
				bs.groupID, 
				bas.EmailID, 
				bas.SendTime AS 'ActionDate', 
				'', 
				'' 
			FROM
				BlastActivitySends bas WITH(NOLOCK)
				JOIN #blasts bs ON bs.blastID = bas.BlastID
			WHERE
				EmailID NOT IN (SELECT EmailID FROM BlastActivityClicks bac JOIN #blasts bs ON bs.blastID = bac.BlastID)

		SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END
	ELSE IF @ReportType = 'bounce'
	BEGIN
		INSERT INTO #tempA  
		SELECT
			inn.blastID, 
			inn.groupID, 
			e.EmailID, 
			babo.BounceTime AS 'ActionDate', 
			bc.BounceCode AS 'ActionValue', 
			babo.BounceMessage AS 'ActionNotes'
		FROM
			BlastActivityBounces babo 
			JOIN ecn5_communicator..Emails e WITH (NOLOCK) ON e.EmailID = babo.EmailID
			JOIN BounceCodes bc ON bc.BounceCodeID = babo.BounceCodeID 
			JOIN
				(
					SELECT 	
						bs.blastID, 
						bs.groupID,
						MAX(BounceID) AS BounceID
					FROM
						BlastActivityBounces babo1 WITH(NOLOCK) 
						JOIN #blasts bs ON bs.blastID = babo1.BlastID 
						JOIN BounceCodes bc1 ON bc1.BounceCodeID = babo1.BounceCodeID
					WHERE	
						(LEN(ltrim(rtrim(@FilterType))) = 0 OR  bc1.BounceCode = @FilterType) 
					GROUP BY 
						bs.blastID, 
						bs.groupID,
						babo1.emailID
				) inn ON inn.BounceID = babo.BounceID
		WHERE
			(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP) 		
	
		SET @ReportColumns = ' #tempA.ActionDate AS BounceTime, #tempA.ActionValue AS BounceType, #tempA.ActionNotes AS BounceSignature '
	END
	ELSE IF @ReportType = 'unsubscribe'
	BEGIN
		if @OnlyUnique = 0
			BEGIN 
			INSERT INTO #tempA  
			SELECT
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				baus.UnsubscribeTime AS 'ActionDate', 
				usc.UnsubscribeCode AS 'ActionValue', 
				baus.Comments AS 'ActionNotes'
			FROM   
				BlastActivityUnSubscribes baus WITH(NOLOCK)
				JOIN ecn5_communicator..emails e WITH(NOLOCK)  ON e.EmailID = baus.emailID  
				JOIN UnsubscribeCodes usc WITH(NOLOCK)  ON usc.UnsubscribeCodeID = baus.UnsubscribeCodeID 
				JOIN #blasts bs ON bs.blastID = baus.BlastID
			WHERE
				(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
				AND usc.UnsubscribeCode = @FilterType 
		END 
	ELSE IF @OnlyUnique= 1
			BEGIN 

				INSERT INTO #tempA  
				SELECT 
				bs.blastID, bs.groupID, e.EmailID, MIN(baus.UnsubscribeTime) as 'ActionDate', usc.UnsubscribeCode as 'ActionValue',MIN( baus.Comments) as 'ActionNotes'
				
					FROM   
						BlastActivityUnSubscribes baus WITH(NOLOCK)
						JOIN ecn5_communicator.dbo.emails e WITH(NOLOCK)  ON e.EmailID = baus.emailID  
						JOIN UnsubscribeCodes usc WITH(NOLOCK)  ON usc.UnsubscribeCodeID = baus.UnsubscribeCodeID 
						JOIN #blasts bs ON bs.blastID = baus.BlastID    
					WHERE
						(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
						AND usc.UnsubscribeCode = @FilterType 
					GROUP BY bs.blastID, bs.GroupID, e.emailID, usc.UnsubscribeCode
              
					
			ORDER BY EmailID, ActionNotes, ActionDate
				--SELECT
				--	bs.blastID, 
				--	bs.groupID, 
				--	e.EmailID, 
				--	max(baus.UnsubscribeTime) AS 'ActionDate', 
				--	usc.UnsubscribeCode AS 'ActionValue', 
				--	max(baus.Comments) AS 'ActionNotes'
				--FROM   
				--	BlastActivityUnSubscribes baus WITH(NOLOCK)
				--	JOIN ecn5_communicator..emails e WITH(NOLOCK)  ON e.EmailID = baus.emailID  
				--	JOIN UnsubscribeCodes usc WITH(NOLOCK)  ON usc.UnsubscribeCodeID = baus.UnsubscribeCodeID 
				--	JOIN #blasts bs ON bs.blastID = baus.BlastID
					
				--WHERE
				--	(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
				--	AND usc.UnsubscribeCode = @FilterType 
				--	and bauS.Comments like '%FOR GROUP: ' + CONVERT(varchar(25),bs.GroupID) + '%'
				--GROUP BY
				--	bs.blastID, bs.GroupID, e.EmailID, usc.UnsubscribeCode
			END 


		SET @ReportColumns = ' #tempA.ActionDate AS UnsubscribeTime, #tempA.ActionValue AS SubscriptionChange, #tempA.ActionNotes AS Reason  '


	END
	ELSE IF @ReportType = 'resend'
	BEGIN
		INSERT INTO #tempA  
		SELECT
			bs.blastID, 
			bs.groupID, 
			e.EmailID, 
			bars.ResendTime AS 'ActionDate', 
			'resend' AS ActionValue, 
			''
		FROM   
			BlastActivityResends bars WITH(NOLOCK) 
			JOIN ecn5_communicator..emails e WITH(NOLOCK) ON e.EmailID = bars.emailID  
			JOIN #blasts bs ON bs.blastID = bars.BlastID
		WHERE
			(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP)

		SET @ReportColumns = ' #tempA.ActionDate AS ResentTime, #tempA.ActionValue AS Action '		
	END
	ELSE IF @ReportType = 'suppressed'
	BEGIN
		IF(@FilterType = 'ALL')
		BEGIN
			INSERT INTO #tempA  
			SELECT DISTINCT 
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				ISNULL(bas.SuppressedTime,bs.Sendtime) AS ActionDate, --If SuppressedTime is unavailable, default to blast Sendtime
				'',
				sc.SupressedCode AS 'ActionNotes'
			FROM
				BlastActivitySuppressed bas WITH (NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bas.EmailID = e.EmailID 
				JOIN SuppressedCodes sc WITH(NOLOCK) on sc.SuppressedCodeID = bas.SuppressedCodeID
				JOIN #blasts bs ON bs.blastID = bas.BlastID
			WHERE	
				e.emailaddress LIKE '%' + @ISP 
				--sc.SupressedCode = @FilterType 
		END
		ELSE
		BEGIN
			INSERT INTO #tempA  
			SELECT DISTINCT 
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				ISNULL(bas.SuppressedTime,bs.Sendtime) AS ActionDate, --If SuppressedTime is unavailable, default to blast Sendtime
				'',
				sc.SupressedCode AS 'ActionNotes'
			FROM	
				BlastActivitySuppressed bas WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bas.EmailID = e.EMailID 
				JOIN SuppressedCodes sc ON sc.SuppressedCodeID = bas.SuppressedCodeID 
				JOIN #blasts bs ON bs.blastID = bas.BlastID
			WHERE	
				--sc.SupressedCode = @FilterType 
				e.emailaddress LIKE '%' + @ISP 
			
		END

		--SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
		SET @ReportColumns = ' #tempA.ActionDate AS SendTime, #tempA.ActionNotes AS SuppressedReason  '
	END
	ELSE IF @ReportType = 'delivered'
	BEGIN
		INSERT INTO #tempA  
			SELECT  
				bs.blastID, 
				bs.groupID, 
				e.EmailID, 
				bas.SendTime AS 'ActionDate', 
				'delivered' AS 'ActionValue' , 
				''
			FROM
				BlastActivitySends bas WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bas.EmailID = e.EmailID 
				JOIN #blasts bs ON bs.blastID = bas.BlastID
				left outer join BlastActivityBounces bab with(nolock) on bas.EmailID = bab.EmailID and bas.blastid = bab.blastid
			WHERE
				(LEN(@ISP) = 0 OR e.emailaddress LIKE '%' + @ISP) and bab.BounceID is null

			SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END
	ELSE IF @ReportType = 'notsent'
	BEGIN
		INSERT INTO #tempA  
			SELECT  
				b.blastID, 
				b.groupID, 
				eg.EmailID, 
				bas.SendTime AS 'ActionDate', 
				'notsent' AS 'ActionValue' , 
				''
			FROM ECN5_COMMUNICATOR..Blast b with(nolock) 
            join ECN5_COMMUNICATOR..EmailGroups eg WITH (NOLOCK) on b.GroupID =  eg.GroupID and eg.SubscribeTypeCode = 's'
            JOIN #blasts ids ON ids.blastID = b.blastID 
            left outer join BlastActivitySends bas with(nolock) on bas.blastid = b.blastid and eg.EmailID = bas.EmailID
            WHERE bas.SendID is null

			SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END
	ELSE IF @ReportType = 'open_noclick'
	BEGIN
		INSERT INTO #tempA  
			SELECT distinct 
				bas.blastID, 
				ids.groupID, 
				e.EmailID, 
				MAX(bas.OpenTime) AS 'ActionDate',
				'open_noclick' AS 'ActionValue' , 
				''
			FROM BlastActivityOpens bas WITH (NOLOCK) JOIN #blasts ids ON ids.blastID = bas.blastID 
			JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bas.EmailID = e.EmailID 
            left outer join BlastActivityClicks bac with(nolock) on bas.blastid = bac.blastid and bac.EmailID = bas.EmailID
            WHERE bac.ClickID is null
			GROUP BY bas.blastID,ids.groupID,e.EmailID
			SET @ReportColumns = ' #tempA.ActionDate AS SendTime '
	END

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
		EXEC( 'SELECT EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
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
		
		EXEC ('SELECT EmailAddress, #tempA.blastID, #tempA.groupID, ' + @ReportColumns + ' , Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
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
GO

GRANT EXEC ON spDownloadCampaignItemDetails TO ECN5
GO

