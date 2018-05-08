CREATE PROCEDURE [dbo].[v_Blast_GetBlastEmailsListForDynamicContent_Filters]
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID varchar(MAX),        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(MAX),        
	@SupressionList varchar(MAX),
	@OnlyCounts bit,
	@LogSuppressed bit = 1        
AS
	
BEGIN        
	set NOCOUNT ON    

	declare         
		@SqlString  varchar(8000) = '',         
		@EmailString  varchar(8000) = '',        
		@Col1 varchar(8000) = '',        
		@Col2 varchar(8000) = '',      
		@topcount varchar(10) = '',
		@emailcolumns varchar(2000) = '',
		@emailsubject varchar(1000) = '',
		@EmailFrom varchar(100) = '',
		@Filter varchar(max) = '',
		@layoutID int = 0,
		@TestBlast  varchar(5)  = '' ,
		@selectslotstr varchar(8000) = '',
		@BasechannelID int = 0,
		@DynamicFromName	varchar(100) = '',       
		@DynamicFromEmail	varchar(100) = '',      
		@DynamicReplyToEmail  varchar(100) = '',
		@CurrDate varchar(10) = '',
		@mailRoute varchar(100) = '',
		@customerproductthreshold int,
		@customerproductpriority int,
		@blastpriority int = 0,
		@IsMessageEnabledforThreshold bit = 0,
		@Domain varchar(100) = '',
		@OverrideAmount int = null,
		@OverrideIsAmount int = null,
		@AmountAlreadySent int = null,
		@BlastType varchar(50) = '',
		@SampleID int = NULL,
		@IsMTAPriority bit = 0,
		@thresholdlimit int = 0,
		@blasttime datetime,
		@DynamicSlotExists bit = 0,
		@UDFExists bit = 0,
		@sqlTopCount NVARCHAR(4000), 
  		@CountToUse int,
  		@CountToSuppress int,
  		@CountSent int,
  		@CountAvailable int,
  		@WinningBlastID int = 0,
  		@DynamicTagExists bit = 0, 	
  		@DT_startTag varchar(25) = 'ECN.DynamicTag.',		
  		@DT_endTag varchar(25) = '.ECN.DynamicTag',
  		@DynamicTagColumns varchar(MAX) = ''
  		
	set @CurrDate = convert(varchar, getdate(), 112)

 	declare @gdf table(GID int, ShortName varchar(50))    
	
	declare @actionTypes table(SSID int, action varchar(50), refBlastIDs varchar(1000))
	
	    

 	create table #AlreadySent (EmailID int, Which varchar(50))  
 	CREATE UNIQUE CLUSTERED INDEX AlreadySent_ind on #AlreadySent(EmailID, Which) with ignore_dup_key
 	
 	create table #ToSend (EmailID int)  
 	CREATE UNIQUE CLUSTERED INDEX ToSend_ind on #ToSend (EmailID) with ignore_dup_key
 	
 	create table #OtherSuppression (OrderID int, EmailID int, Reason varchar(50))  
 	CREATE UNIQUE CLUSTERED INDEX OtherSuppression_ind on #OtherSuppression(EmailID) with ignore_dup_key
 	
 	create table #ThresholdSuppression (EmailAddress varchar(100))  
 	CREATE UNIQUE CLUSTERED INDEX ThresholdSuppression_ind on #ThresholdSuppression(EmailAddress) with ignore_dup_key

	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	

	declare @allContent table (contentID int)
	declare @dynamicTags table (DynamicTagID int, Tag varchar(20), contentID int)

 	create table #tempA (EmailID int)  
		
	IF @BlastID <> 0
	BEGIN
	
		select 
			@Domain = RIGHT(b.EmailFrom, LEN(b.EmailFrom) - CHARINDEX('@', b.EmailFrom)),@OverrideAmount = b.OverrideAmount, 
			@OverrideIsAmount = b.OverrideIsAmount, @testblast = b.TestBlast, @blasttime = b.sendtime, @DynamicFromName = b.DynamicFromName, 
			@DynamicFromEmail = b.DynamicFromEmail, @DynamicReplyToEmail = b.DynamicReplyToEmail, @BlastType = b.BlastType, @SampleID = SampleID,
			@emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @SupressionList = BlastSuppression, @layoutID = layoutID
		from 
			blast b with (nolock)
		where 
			b.blastID = @BlastID and b.StatusCode <> 'Deleted'
			
		if exists (select top 1 filterID from contentFilter WITH (NOLOCK) where layoutID = @layoutID and IsDeleted = 0)
			set @DynamicSlotExists = 1
			
		insert into @allContent
		select contentID
		from
		(
		select LayoutID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9 from Layout 
		where LayoutID  = @layoutID  and IsDeleted = 0
		) x
		unpivot (contentID for slot in (ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9)) as unpvt
		where contentID > 0
		union
		select ContentID from ContentFilter where LayoutID = @layoutID and IsDeleted = 0
		
		insert into @dynamicTags
		select distinct DynamicTagID, dt.Tag, dt.ContentID from Content c join @allContent ac on c.ContentID = ac.contentID cross join DynamicTag dt 
		Where c.CustomerID = @CustomerID and dt.CustomerID = @CustomerID and c.IsDeleted = 0 and isnull(dt.IsDeleted,0) = 0 and 
		(PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentSource) > 0 or   PATINDEX('%' + @DT_startTag +dt.Tag+ @DT_endTag + '%', c.ContentText) > 0 )		
		
		if exists (select top 1 contentID from @dynamicTags)
		Begin
			set @DynamicTagExists = 1						

			SELECT 
			   @DynamicTagColumns = STUFF( (SELECT ',' + CASe when dynamicwhere is null then convert(varchar(10), x.ContentID) else 'Case ' + dynamicwhere + ' else ' +  convert(varchar(10), x.ContentID) + ' END ' END + 
			   ' as [' + @DT_startTag + Tag + @DT_endTag +']'  from 
			(
			SELECT 
			  dt.*,
			  STUFF((
				SELECT ' When ' + WhereClause + ' then '  + convert(varchar(10), dtr1.ContentID)
				FROM @dynamicTags dt1 join DynamicTagRule dtr1 on dt1.DynamicTagID = dtr1.DynamicTagID join [rule] r1 on dtr1.RuleID = r1.RuleID
				WHERE  dt1.DynamicTagID = dt.DynamicTagID  and dtr1.IsDeleted = 0  and r1.IsDeleted = 0
				order by dt1.DynamicTagID, dtr1.Priority
				FOR XML PATH(''),TYPE).value('(./text())[1]','VARCHAR(MAX)')
			  ,1,0,'') AS dynamicwhere
			FROM @dynamicTags dt
			GROUP BY dt.DynamicTagID, dt.Tag, dt.contentID) x 
			 FOR XML PATH('')), 
			1, 0, '')
		
			select @DynamicTagColumns = replace(replace(@DynamicTagColumns, '&gt;', '>'), '&lt;', '<')
		End
		
	END
		
	if rtrim(ltrim(@TestBlast)) = 'Y'
	Begin
		set @IsMTAPriority = 1
	End
	 
	if exists (select Top 1 gdf.GroupDatafieldsID from GroupDataFields gdf WITH (NOLOCK) JOIN Groups g WITH (NOLOCK) on gdf.GroupID = g.GroupID where g.groupID = @GroupID and gdf.IsDeleted = 0)
		set @UDFExists = 1 
	 
	--@thresholdlimit is the channel's threshold limit, 0 is unlimited
	select @basechannelID = c.basechannelID, @thresholdlimit = EmailThreshold from ecn5_accounts..customer c WITH (NOLOCK)   join 
			ecn5_accounts..BaseChannel bc WITH (NOLOCK) on bc.BaseChannelID = c.BaseChannelID
	where customerID = @CustomerID and c.IsDeleted = 0 and bc.IsDeleted = 0
	

	select @Filter = t.WhereClause
		from 
		(
			SELECT ' and (' + Convert(varchar(MAX),Whereclause) + ') ' as WhereClause
			FROM filter WITH (NOLOCK) 
			where FilterID in (SELECT CONVERT(int,t.Items) 
								from dbo.fn_Split(@FilterID,',') t)
		) t

	
	--wgh: trimming where clause
	set @Filter = RTRIM(ltrim(@Filter))

	if @Filter <> ''
	begin
		--set @filter = ' and (' + @filter + ') '
		set @Filter= REPLACE(@Filter, 'CONVERT(VARCHAR,', 'CONVERT(VARCHAR(255),');
	end

	set @Filter = replace(@Filter,'[emailaddress]','emailaddress')
	set @Filter = replace(@Filter,'emailaddress','emails.emailaddress')

 	if (@BlastID > 0 and @testblast <> 'Y')
 	Begin
 		INSERT INTO #AlreadySent
		SELECT 
			EmailID, 'Current'
		FROM 
			EmailActivityLog WITH (NOLOCK) 
		WHERE 
			(ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') AND --??SUNIL - SHOULD NOT INCLUDE SUPPRESSED
			BlastID = @BlastID
			
 		--@customerProductThreshold > 0 = Threshold feature enabled
 		SELECT 
			@customerProductThreshold = count(p.productname) 
		FROM 
				ecn5_accounts..CustomerProduct cp WITH (NOLOCK) 
				JOIN ecn5_accounts..ProductDetail pd WITH (NOLOCK) ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Product p WITH (NOLOCK) ON pd.ProductID = p.ProductID 
		WHERE 
				cp.CustomerID = @customerID AND 
				p.ProductName = 'ecn.communicator' AND 
				pd.productdetailName = 'SetMessageThresholds' AND 
				cp.Active = 'y' and
				cp.IsDeleted = 0
		
		--@customerProductPriority > 0 = Priority feature enabled		
		SELECT 
			@customerProductPriority = count(p.productname) 
		FROM 
				ecn5_accounts..CustomerProduct cp WITH (NOLOCK) 
				JOIN ecn5_accounts..ProductDetail pd WITH (NOLOCK) ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Product p WITH (NOLOCK) ON pd.ProductID = p.ProductID 
		WHERE 
				cp.CustomerID = @customerID AND 
				p.ProductName = 'ecn.communicator' AND 
				pd.productdetailName = 'SetMessagePriority' AND 
				cp.Active = 'y' and
				cp.IsDeleted = 0 
				
		--@IsMessageEnabledforThreshold - 1 = Message threshold enabled
		--@blastPriority - 1 = Message priority enabled
		SELECT	
				@IsMessageEnabledforThreshold = Threshold,
				@blastPriority = (case when mt.Priority = 1 then isnull(mt.SortOrder,0) else 0 end)
		FROM	
				Blast b WITH (NOLOCK)
				JOIN Layout l WITH (NOLOCK) ON b.LayoutID = l.LayoutID
				JOIN MessageType mt WITH (NOLOCK) ON l.MessageTypeID = mt.MessageTypeID
		WHERE	
				b.BlastID = @BlastID and
				b.StatusCode <> 'Deleted' and
				l.IsDeleted = 0 and
				mt.IsDeleted = 0
				
		--new logic from flowchart				
		IF @customerProductThreshold > 0 AND @thresholdlimit > 0
		BEGIN
			--Threshold feature enabled
			
			CREATE TABLE #sentemail (emailaddress VARCHAR(255), totalemail int default 1)		
			
 			-- wgh - thresh was not working, think it's because we limit insert of this to 1 per email - CREATE UNIQUE CLUSTERED INDEX sentemail_ind on  #sentemail(emailaddress) with ignore_dup_key
			
			DECLARE @cursorBlastID int
 					
 			IF @customerProductPriority > 0
			BEGIN
				--Priority feature enabled
				IF @IsMessageEnabledforThreshold > 0
				BEGIN
					--Message threshold enabled						
					insert into #sentEmail
					exec spGetThresholdSuppressedEmails @BlastID, 'N'
					
					IF @blastPriority > 0
					BEGIN
						--Message priority enabled so do threshold & Priority
						DECLARE db_cursor CURSOR FOR			
						SELECT  b.BlastID 
						FROM	Blast b   WITH (NOLOCK) 
							JOIN ecn5_accounts..Customer c   WITH (NOLOCK) ON b.CustomerID = c.customerID
							JOIN Layout l   WITH (NOLOCK) on b.LayoutID = l.LayoutID
							JOIN MessageType mt   WITH (NOLOCK) on l.MessageTypeID = mt.MessageTypeID
						WHERE 
							c.BaseChannelID = @basechannelID AND
							b.TestBlast = 'N' AND
							b.StatusCode <> 'Deleted' AND
							c.IsDeleted = 0 AND
							l.IsDeleted = 0 AND
							mt.IsDeleted = 0 AND
							mt.Priority = 1 AND
							(
								(b.StatusCode='active' and b.blastID <> @blastID) or
								(b.StatusCode='pending' AND mt.SortOrder < @blastPriority and b.SendTime < (SELECT CONVERT(VARCHAR(10),GETDATE(),111) + ' 23:59:59')) or
								(b.StatusCode='pending' AND b.sendtime < @blasttime AND mt.SortOrder = @blastPriority) or
								(b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID AND mt.SortOrder = @blastPriority) 
							)
										
						OPEN db_cursor   
						FETCH NEXT FROM db_cursor INTO @cursorBlastID

						WHILE @@FETCH_STATUS = 0   
						BEGIN   
							   insert into #sentEmail (emailaddress)
							   EXEC spGetBlastEmailListWithNoSuppression @cursorBlastID

								--select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

							   FETCH NEXT FROM db_cursor INTO @cursorBlastID
						END 
						CLOSE db_cursor   
						DEALLOCATE db_cursor
					END
					ELSE
					BEGIN
						--Message priority NOT enabled so do Threshold ONLY
						DECLARE db_cursor CURSOR FOR			
						SELECT  b.BlastID 
						FROM	Blast b WITH (NOLOCK) 
							JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
						WHERE 
							c.BaseChannelID = @basechannelID AND
							b.TestBlast = 'N' AND
							b.StatusCode <> 'Deleted' AND
							c.IsDeleted = 0 AND
							(
								(b.StatusCode='active'  and b.blastID <> @blastID) or
								(b.StatusCode='pending' AND b.sendtime < @blasttime) or
								(b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID) 
							)
										
						OPEN db_cursor   
						FETCH NEXT FROM db_cursor INTO @cursorBlastID

						WHILE @@FETCH_STATUS = 0   
						BEGIN   
							   insert into #sentEmail (emailaddress)
							   EXEC spGetBlastEmailListWithNoSuppression @cursorBlastID

								--select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

							   FETCH NEXT FROM db_cursor INTO @cursorBlastID
						END 
						CLOSE db_cursor   
						DEALLOCATE db_cursor 
					END				
				END			
			END
			ELSE
			BEGIN
				--Priority feature NOT enabled so do Threshold ONLY
				insert into #sentEmail
				exec spGetThresholdSuppressedEmails @BlastID, 'Y'
			
				DECLARE db_cursor CURSOR FOR			
				SELECT  b.BlastID 
				FROM	Blast b WITH (NOLOCK) 
					JOIN ecn5_accounts..Customer c WITH (NOLOCK) ON b.CustomerID = c.customerID
				WHERE 
					c.BaseChannelID = @basechannelID AND
					b.TestBlast = 'N' AND
					b.StatusCode <> 'Deleted' AND
					c.IsDeleted = 0 AND 
					(
						(b.StatusCode='active'  and b.blastID <> @blastID) or
						(b.StatusCode='pending' AND b.sendtime < @blasttime) or
						(b.StatusCode='pending' AND b.sendtime = @blasttime and b.BlastID < @BlastID) 
					)
								
				OPEN db_cursor   
				FETCH NEXT FROM db_cursor INTO @cursorBlastID

				WHILE @@FETCH_STATUS = 0   
				BEGIN   
					   insert into #sentEmail (emailaddress)
					   EXEC spGetBlastEmailListWithNoSuppression @cursorBlastID

						--select 'Active/Pending Email suppressed : ', @cursorBlastID, getdate(), @@ROWCOUNT 

					   FETCH NEXT FROM db_cursor INTO @cursorBlastID
				END 
				CLOSE db_cursor   
				DEALLOCATE db_cursor 
			
			END	
			
			INSERT INTO #ThresholdSuppression
			SELECT  se.emailaddress
			FROM #sentemail se 
			left outer join ChannelNoThresholdList ntl  WITH (NOLOCK) on se.emailaddress = ntl.emailaddress and ntl.basechannelID = @basechannelID and ntl.IsDeleted = 0
			WHERE ntl.emailaddress is null 
			GROUP BY se.emailaddress      
			HAVING SUM(totalemail) >= @thresholdlimit	
			
			DROP TABLE #sentemail
			
		END				
			
	End	

	-- insert threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
	--1. Threshold Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			1, e.EmailID, 'Threshold'
	FROM	
			Emails e WITH (NOLOCK)
				JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
				JOIN #ThresholdSuppression ts WITH (NOLOCK) ON e.EmailAddress = ts.EmailAddress 
	WHERE 	
			eg.GroupID = @GroupID
		
	--2. 7 Day Suppression	
	IF (@TestBlast <> 'Y' AND lower(@ActionType) <> 'softbounce')        
	Begin   
		IF	@groupID <> 61640 and -- CBR Rental
			@groupID <> 35894 and -- NEEBO Rental Transactions
			@groupID <> 126097 and -- NEEBO Closed Wareshouse  Group
			@groupID <> 126098 and -- NEEBO Closed  inMarket  Group
			@CustomerID <> 2808 and --Apogee Retail
			@CustomerID <> 3090 and -- Apogee Retail NY, LLC
			@CustomerID <> 3137-- and --Neebo Rental Program
			--@CustomerID <> 1941 -- KM Master Lists. -- removed KM Master Lists per Raisa on 12/30/2013
		BEGIN
			INSERT INTO #OtherSuppression
			SELECT	
					2, eal.EmailID, '7Day'
			FROM	
					EmailActivityLog eal WITH (NOLOCK)
						JOIN Blast B WITH (NOLOCK) ON eal.BlastID = B.BlastID
			WHERE 	
					LayoutID = @LayoutID AND 
					SendTime > dateadd(d, -7 , @blasttime) AND
					ActionTypeCode = 'send' and
					B.StatusCode <> 'Deleted'
		END
	END
	DECLARE @SuppressionGroups table(GroupID int, SSID int, RefBlastIDs varchar(500), FilterID int)
	--3. List Suppression	
	if Len(ltrim(rtrim(@SupressionList))) > 0
	Begin
		--Sample XML for @SuppressionGroups
		--<SuppFilters>
		--	<SupppressionGroup id="1">
		--		<ssID id="1">
		--			<refBlastIDs>1,2,4</refBlastIDs>
		--		</ssID>
		--		<FilterIDs>11,22,33</FilterIDs>
		--	</SuppressionGroup>
		--	<SuppressionGroup id="2">
		--		<ssID id="2">
		--			<refBlastIDs>1</refBlastIDs>
		--		</ssID>
		--		<ssID id="4">
		--			<refBlastIDs>5</refBlastIDs>
		--		</ssID>
		--		<FilterIDs>44,55</FilterIDs>
		--	</SuppressionGroup>
		--</SuppFilters
		DECLARE @docHandleSupp int
		EXEC sp_xml_preparedocument @docHandleSupp OUTPUT, @SupressionList  
		INSERT INTO @SuppressionGroups(GroupID, SSID, RefBlastIDs, FilterID)
		SELECT SuppGroupID, SSID, Refblasts, null
		FROM OPENXML(@docHandleSupp, N'/SuppFilters/SuppressionGroup/ssID')
		WITH
		(
			SuppGroupID int '../@id',SSID int '@id', Refblasts varchar(500) 'refBlastIDs'
		)

		INSERT INTO @SuppressionGroups(GroupID, SSID, RefBlastIDs, FilterID)
		SELECT SuppGroupID, null, null, FilterIDs
		FROM OPENXML(@docHandleSupp, N'/SuppFilters/SuppressionGroup/FilterIDs')
		WITH
		(
			SuppGroupID int '../@id',FilterIDs varchar(500) 'FilterIDs'
		)
		exec sp_xml_removedocument @docHandleSupp

		Declare @tempSSNames table(SmartSegmentID int, SmartSegmentName varchar(50))
		INSERT INTO @tempSSNames
		SELECT SmartSegmentID, SmartSegmentName
		FROM SmartSegment

		Declare @SuppFilter varchar(MAX)
		Declare @CurrentGroupID int
		Declare @LastGroupID int = -1
		Declare @SSID int
		Declare @REfBlastIDs varchar(500)
		Declare @FilterIDs varchar(500)
		DECLARE @FilterExist bit = 0
		DECLARE @SuppGdf table(GID int, ShortName varchar(50))   
		DECLARE suppCursor cursor
		FOR
		SELECT GroupID, SSID, RefBlastIDs, FilterID 
		FROM @SuppressionGroups
		ORDER BY GroupID asc
		OPEN suppCursor
		FETCH NEXT FROM suppCursor
		INTO @CurrentGroupID, @SSID, @REfBlastIDs, @FilterIDs
		WHILE @@FETCH_STATUS = 0
		BEGIN
		
			if(@LastGroupID = @CurrentGroupID)
			BEGIN
				--Same as last group so keep adding to the filter sql
				if(@SSID is not null and LEN(ISNULL(@REfBlastIDs, '')) > 0)
					if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'unopen')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + @REfBlastIDs +        
     									  ') and not exists (select EmailID from ecn_activity..BlastActivityOpens bao1 with(nolock) where BlastID in (' + @REfBlastIDs + ')) '
					else if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'unclick')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  @REfBlastIDs  +         
        								') and not exists (select EmailID from ecn_activity..BlastActivityClicks WITH (NOLOCK) where BlastID in (' + @REfBlastIDs +')  ) '
					else if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'open')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK)  where BlastID in (' + @REfBlastIDs +')) '
					else if exists(SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'click')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks WITH (NOLOCK)  where BlastID in (' + @REfBlastIDs +')) '
					else if exists(SELECT Top 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'suppressed')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @REfBlastIDs +') and SupressedCode in (''Threshold'')) '
					else if exists(SELECT TOp 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'sent')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @REfBlastIDs + ')) '
					else if exists(SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'not sent')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @REfBlastIDs + ')) '
				else if (@FilterIDs is not null and LEN(@FilterIDs) > 0)
					SET @FilterExist = 1
					select @SuppFilter = @SuppFilter + t.WhereClause
					from 
					(
						SELECT ' and (' + CONVERT(varchar(MAX),Whereclause) + ') ' as WhereClause
						FROM filter WITH (NOLOCK) 
						where FilterID in (SELECT CONVERT(int,t.Items) 
											from dbo.fn_Split(@FilterIDs,',') t)
					) t
					insert into @SuppGdf         
					select distinct gdf.GroupDatafieldsID, gdf.ShortName 
					from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
					where  g.groupID = @CurrentGroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @SuppFilter) > 0 and gdf.IsDeleted = 0  
			END
			ELSE if(@LastGroupID = -1)
			BEGIN
				--Starting first group
				SET @LastGroupID = @CurrentGroupID
				SET @SuppFilter = ''
				SET @FilterExist = 0
				if(@SSID is not null and LEN(ISNULL(@REfBlastIDs,'')) > 0)
				BEGIN
					if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'unopen')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + @REfBlastIDs +        
     									  ') and not exists (select EmailID from ecn_activity..BlastActivityOpens bao1 with(nolock) where BlastID in (' + @REfBlastIDs + ')) '
					else if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'unclick')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  @REfBlastIDs  +         
        								') and not exists (select EmailID from ecn_activity..BlastActivityClicks WITH (NOLOCK) where BlastID in (' + @REfBlastIDs +')  ) '
					else if exists (SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'open')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK)  where BlastID in (' + @REfBlastIDs +')) '
					else if exists(SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'click')
						Set @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks WITH (NOLOCK)  where BlastID in (' + @REfBlastIDs +')) '
					else if exists(SELECT Top 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'suppressed')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @REfBlastIDs +') and SupressedCode in (''Threshold'')) '
					else if exists(SELECT TOp 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'sent')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @REfBlastIDs + ')) '
					else if exists(SELECT TOP 1 * from @tempSSNames where SmartSegmentID = @SSID and SmartSegmentName = 'not sent')
						SET @SuppFilter = @SuppFilter + ' and Emails.EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @REfBlastIDs + ')) '
				END
				else if (@FilterIDs is not null and LEN(@FilterIDs) > 0)
					SET @FilterExist = 1
					select @SuppFilter = @SuppFilter + t.WhereClause
					from 
					(
						SELECT ' and (' + CONVERT(varchar(MAX),Whereclause) + ') ' as WhereClause
						FROM filter WITH (NOLOCK) 
						where FilterID in (SELECT CONVERT(int,t.Items) 
											from dbo.fn_Split(@FilterIDs,',') t)
					) t
					insert into @SuppGdf         
					select distinct gdf.GroupDatafieldsID, gdf.ShortName 
					from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
					where  g.groupID = @CurrentGroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @SuppFilter) > 0 and gdf.IsDeleted = 0   
			END
			ELSE
			BEGIN
				--Starting new group so execute the filter sql to insert emails from the previous group and clear everything out to start the next group
				-- any reference to the group id to run on should be last group id because at this point were done with the last groupid and the current group id
				-- has yet to be processed
				if(LEN(@SuppFilter) > 0 and @FilterExist = 1)
				BEGIN
					 --There is a UDF filter so we have to get all the udfs for the current group, build the sql and run the filters
					 --Need to have matt look at this as it might cause some problems
					  
					--Have to get UDFs
					DECLARE @SuppStandAloneUDFs VARCHAR(2000),
						@SuppTransactionalUDFs VARCHAR(2000),
						--@StandAloneUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
						--@TransactionalUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
						@SuppStandAloneUDFIDs VARCHAR(2000),
						@SuppTransactionalUDFIDs VARCHAR(2000),
						@SuppStandAloneColumns VARCHAR(200),
						@SuppTransactionalColumns VARCHAR(200),
						@SuppStandAloneQuery VARCHAR(4000),
						@SuppTransactionalQuery VARCHAR(4000),
						@SuppStandAloneTempQuery VARCHAR(4000),
						@SuppTransactionalTempQuery VARCHAR(4000),
						@SuppStandAloneDrop VARCHAR(500),
						@SuppTransactionalDrop VARCHAR(500)
					SET @SuppStandAloneUDFs = ''
					SET @SuppTransactionalUDFs = ''
					SET @SuppStandAloneUDFIDs = ''
					SET @SuppTransactionalUDFIDs  = ''
					SET @SuppStandAloneColumns  = ''
					SET @SuppTransactionalColumns = ''
					SET @SuppStandAloneQuery  = ''
					SET @SuppTransactionalQuery  = ''
					SET @SuppstandAloneTempQuery  = ''
					SET @SuppTransactionalTempQuery  = ''		
					SET @SuppStandAloneDrop  = ''
					SET @SuppTransactionalDrop  = ''
					
					SELECT  @SuppStandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @SuppGdf tg on g.GroupDatafieldsID = tg.GID 
					WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

					SELECT  @SuppStandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @SuppGdf tg on g.GroupDatafieldsID = tg.GID 
					WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

					SELECT  @SuppTransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @SuppGdf tg on g.GroupDatafieldsID = tg.GID 
					WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

					SELECT  @SuppTransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @SuppGdf tg on g.GroupDatafieldsID = tg.GID 
					WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
					
					if LEN(@SuppStandAloneUDFs) > 0
					Begin
						 set @SuppStandAloneColumns = ' SAUDFs.* '
						 set @SuppstandAloneTempQuery = '
									SELECT * into #tempSuppStandAlone
									 FROM
									 (
										SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
										from	EmailDataValues edv WITH (NOLOCK) join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
										where 
												gdf.groupdatafieldsID in (' + @SuppStandAloneUDFIDs + ') and gdf.IsDeleted = 0 
									 ) u
									 PIVOT
									 (
									 MAX (DataValue)
									 FOR ShortName in (' + @SuppStandAloneUDFs + ')) as pvt;
									 
									 CREATE INDEX IDX_tempSuppStandAlone_EmailID ON #tempSuppStandAlone(tmp_EmailID);'
						 
						set @SuppStandAloneQuery = ' left outer join #tempSuppStandAlone SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'			 
						 
						set @SuppStandAloneDrop  = 'drop table #tempSuppStandAlone'
					End

					if LEN(@SuppTransactionalUDFs) > 0
					Begin
						if LEN(@SuppStandAloneColumns) > 0
						Begin
							set @SuppTransactionalColumns = ', TUDFs.* '
						end
						Else
						Begin
							set @SuppTransactionalColumns = ' TUDFs.* '
						End
						set @SuppTransactionalTempQuery =  '  
									SELECT * into #tempSuppTransactional
									 FROM
									 (
										SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
										from	EmailDataValues edv WITH (NOLOCK)  join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
										where 
												gdf.groupdatafieldsID in (' + @SuppTransactionalUDFIDs + ') and gdf.IsDeleted = 0 
									 ) u
									 PIVOT
									 (
									 MAX (DataValue)
									 FOR ShortName in (' + @SuppTransactionalUDFs + ')) as pvt;
									 
									 CREATE INDEX IDX_tempSuppTransactional_EmailID ON #tempSuppTransactional(tmp_EmailID1);'			
						
						set @SuppTransactionalQuery = '  left outer join #tempSuppTransactional TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
						
						set @SuppTransactionalDrop  = 'drop table #tempSuppTransactional'
					End
					
					EXEC ( 
						@SuppStandAloneTempQuery + ';' + 
						@SuppTransactionalTempQuery + ';' +
							'insert into #OtherSuppression  ' + 
							' select 3, Emails.EmailID, ''List''  from  Emails with (NOLOCK) ' +@SuppStandAloneQuery + @SuppTransactionalQuery + 
							' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
							' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
							' and EmailGroups.GroupID = ' + @LastGroupID +        
							' ' + @SuppFilter + ' order by Emails.EmailID;' +
						@SuppStandAloneDrop + ';' + 
						@SuppTransactionalDrop + ';  ' 
						)
					SET @SuppFilter = ''
					SET @FilterExist = 0
				END
				ELSE
				BEGIN
					--No UDF filters so maybe only SS or nothing, just do the group suppression
					exec ('INSERT INTO #OtherSuppression
					SELECT 3, eg.EmailID, ''List''
					FROM 
						Emails with(nolock)
						JOIN EmailGroups eg with(nolock) on Emails.EmailID = eg.EmailID
						WHERE eg.GroupID = ' + @LastGroupID + ' ' + @SuppFilter)
					SET @SuppFilter = ''
					SET @FilterExist = 0
				END
				--Now updating the last group id
				SET @LastGroupID = @CurrentGroupID
			END

		FETCH NEXT FROM suppCursor
		INTO  @CurrentGroupID, @SSID, @REfBlastIDs, @FilterIDs
		END
		CLOSE suppCursor
		DEALLOCATE suppCursor

		--INSERT INTO #OtherSuppression
		--SELECT	
		--		3, eg.EmailID, 'List'
		--FROM	
		--		EmailGroups eg WITH (NOLOCK) join
		--		(select items as groupID from fn_Split(@SupressionList, ',')) SL on eg.GroupID = SL.groupID
	End
	
	--4. Group Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			4, eg.EmailID, 'Group'
	FROM	
			EmailGroups eg WITH (NOLOCK)
				JOIN GROUPS g WITH (NOLOCK) ON eg.GroupID = g.GroupID
	WHERE 	
			g.CustomerID = @CustomerID AND 
			g.MasterSupression=1
			
	--5. Channel Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			5, e.EmailID, 'Channel'
	FROM	
			Emails e WITH (NOLOCK)
				JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
				JOIN ChannelMasterSuppressionList cms WITH (NOLOCK) ON e.EmailAddress = cms.EmailAddress
	WHERE 	
			eg.GroupID = @GroupID AND
			cms.BaseChannelID = @BasechannelID and
			cms.IsDeleted = 0
	
	--6. Domain Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			6, e.EmailID, 'Domain'
	FROM	
			Emails e WITH (NOLOCK)
				JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
				JOIN ecn5_accounts..Customer c WITH (NOLOCK) on e.CustomerID = c.CustomerID
				JOIN DomainSuppression ds WITH (NOLOCK) ON RIGHT(e.EmailAddress, LEN(e.EmailAddress) - CHARINDEX('@', e.EmailAddress)) = ds.Domain AND (e.CustomerID = ds.CustomerID OR c.BaseChannelID = ds.BaseChannelID)   
	WHERE 	
			eg.GroupID = @GroupID and c.IsDeleted = 0 and ds.IsDeleted = 0

	--7. Global Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			5, e.EmailID, 'Global'
	FROM	
			Emails e WITH (NOLOCK)
				JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
				JOIN GlobalMasterSuppressionList gms WITH (NOLOCK) ON e.EmailAddress = gms.EmailAddress
	WHERE 	
			eg.GroupID = @GroupID AND
			gms.IsDeleted = 0

	--8. Invalid Email
	INSERT INTO #OtherSuppression
	SELECT	
			1, e.EmailID, 'InvalidEmail'
	FROM	
			Emails e WITH (NOLOCK)
				JOIN EmailGroups eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
	WHERE 	
			eg.GroupID = @GroupID AND
			dbo.fn_ValidateEmailAddress(Emailaddress) = 0

	--if sample, suppress the sends from the other sample
	if @BlastType = 'Sample'
	Begin
		insert into #AlreadySent
		select emailID, 'Other' 
		from emailactivitylog WITH (NOLOCK) 
		where (ActionTypeCode = 'Send' OR ActionTypeCode = 'Suppressed') and blastID = 
			(select blastID from Blast WITH (NOLOCK) where blastID <> @BlastID and SampleID = @SampleID and BlastType = 'Sample' and StatusCode <> 'Deleted')
	End
	
	--if champion, suppress the sends from the winning blast id
	--insert logic here to handle sending champion to losing A/B emails
	if @BlastType = 'Champion'
	Begin

		select @WinningBlastID = WinningBlastID from [Sample] WITH (NOLOCK) where SampleID = @SampleID and IsDeleted = 0
		
		insert into #AlreadySent
		select emailID, 'Other' 
		from emailactivitylog WITH (NOLOCK) 
		where (ActionTypeCode = 'Send' OR ActionTypeCode = 'Suppressed') and blastID = @WinningBlastID
	End

 	if @BlastID = 0 
 	begin       
		if LEN(@Filter) > 0 and  @UDFExists = 1        
		Begin
			insert into @gdf         
			select distinct gdf.GroupDatafieldsID, gdf.ShortName 
			from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0         
		end	
 	end        
 	else        
 	begin  
		select @MailRoute = m.MTAName from MTA m WITH (NOLOCK) join MTACustomer mc WITH (NOLOCK) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
		if(@MailRoute is null or LEN(@MailRoute) < 1)
		Begin
			select @MailRoute = m.MTAName from MTA m WITH (NOLOCK) join MTACustomer mc WITH (NOLOCK) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
		End
				
		if @DynamicSlotExists = 1 or @DynamicTagExists = 1
		Begin
		
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			
			if @UDFExists = 1   
				insert into @gdf         
				select GroupDatafieldsID, ShortName from groupdatafields WITH (NOLOCK) where GroupDatafields.groupID = @GroupID and IsDeleted = 0  
		end
		else
		Begin		
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(
			 select distinct emailColumns.columnName 
			  from  blast WITH (NOLOCK) join         
				Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
				Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
				   Content.ContentID = Layout.ContentSlot2 or         
				   Content.ContentID = Layout.ContentSlot3 or         
				   Content.ContentID = Layout.ContentSlot4 or         
				   Content.ContentID = Layout.ContentSlot5 or         
				   Content.ContentID = Layout.ContentSlot6 or         
				   Content.ContentID = Layout.ContentSlot7 or         
				   Content.ContentID = Layout.ContentSlot8 or         
				   Content.ContentID = Layout.ContentSlot9 left outer join        
				Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
				contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and ContentFilter.IsDeleted = 0 cross join
				( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			  where         
				Blast.BlastID = @BlastID and 
				blast.StatusCode <> 'Deleted' and
				Layout.IsDeleted = 0 and
				Content.IsDeleted = 0 and
				(
					PATINDEX('%[$][$]' +emailColumns.columnName+ '[$][$]%', Content.ContentSource) > 0 or   
					PATINDEX('%[$][$]' +emailColumns.columnName+ '[$][$]%', Content.ContentText) > 0 or   
					PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%', Content.ContentSource) > 0 or   
					PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%', Content.ContentText) > 0 or   
					PATINDEX('%[#][#]' +emailColumns.columnName+ '[#][#]%', Content.ContentSource) > 0 or   
					PATINDEX('%[#][#]' +emailColumns.columnName+ '[#][#]%', Content.ContentTExt) > 0 or   					
					PATINDEX('%' +emailColumns.columnName+ '%', contentFilter.whereclause) > 0 or   
					PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  Template.TemplateSource) > 0 or   
					PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  Template.TemplateText) > 0 or
					PATINDEX('%' +emailColumns.columnName+ '%', @Filter) > 0 or
					PATINDEX('%[%][%]' +emailColumns.columnName+ '[%][%]%',  @emailsubject) > 0 
				)
			) emailColumns

  			if(LEN(@Filter) > 0)
			begin
  				insert into @gdf         
  				select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
				where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0     
  				union        
  				select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  				from  blast WITH (NOLOCK) join         
				Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
				Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
				   Content.ContentID = Layout.ContentSlot2 or         
				   Content.ContentID = Layout.ContentSlot3 or         
				   Content.ContentID = Layout.ContentSlot4 or         
				   Content.ContentID = Layout.ContentSlot5 or         
				   Content.ContentID = Layout.ContentSlot6 or         
				   Content.ContentID = Layout.ContentSlot7 or         
				   Content.ContentID = Layout.ContentSlot8 or         
				   Content.ContentID = Layout.ContentSlot9 left outer join        
				Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
				contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and contentFilter.IsDeleted = 0  join        
				Groups WITH (NOLOCK) on Groups.GroupID = blast.groupID and Groups.CustomerID = Blast.CustomerID join         
				GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = Groups.GroupID         
			  where         
				Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and  			 
				blast.StatusCode <> 'Deleted' and
				Layout.IsDeleted = 0 and
				Content.IsDeleted = 0 and
				gdf.IsDeleted = 0 and
				(
					PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
					PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
					PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentSource) > 0 or   
					PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentTExt) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentSource) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentTExt) > 0 or   
					PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentSource) > 0 or   
					PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentTExt) > 0 or   					
					PATINDEX('%' +ShortName+ '%', contentfilter.whereclause) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateSource) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateText) > 0  or
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%',  @emailsubject) > 0  or 
					PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
					PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
					PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
				)   
			end
			else
			begin
  				insert into @gdf         
  		--		select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g WITH (NOLOCK) join GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = g.GroupID      
				--where  g.groupID = @GroupID and g.customerID = @CustomerID and gdf.IsDeleted = 0     
  		--		union        
  				select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  				from  blast WITH (NOLOCK) join         
				Layout WITH (NOLOCK) on Blast.layoutID = Layout.layoutID left outer join        
				Content WITH (NOLOCK) on  Content.ContentID = Layout.ContentSlot1 or         
				   Content.ContentID = Layout.ContentSlot2 or         
				   Content.ContentID = Layout.ContentSlot3 or         
				   Content.ContentID = Layout.ContentSlot4 or         
				   Content.ContentID = Layout.ContentSlot5 or         
				   Content.ContentID = Layout.ContentSlot6 or         
				   Content.ContentID = Layout.ContentSlot7 or         
				   Content.ContentID = Layout.ContentSlot8 or         
				   Content.ContentID = Layout.ContentSlot9 left outer join        
				Template WITH (NOLOCK) on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join        
				contentFilter WITH (NOLOCK) on (contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID) and contentFilter.IsDeleted = 0  join        
				Groups WITH (NOLOCK) on Groups.GroupID = blast.groupID and Groups.CustomerID = Blast.CustomerID join         
				GroupDataFields gdf WITH (NOLOCK) on gdf.GroupID = Groups.GroupID         
			  where         
				Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and  			 
				blast.StatusCode <> 'Deleted' and
				Layout.IsDeleted = 0 and
				Content.IsDeleted = 0 and
				gdf.IsDeleted = 0 and
				(
					PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
					PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
					PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentSource) > 0 or   
					PATINDEX('%[$][$]' +ShortName+ '[$][$]%', Content.ContentTExt) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentSource) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Content.ContentTExt) > 0 or  
					PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentSource) > 0 or   
					PATINDEX('%[#][#]' +ShortName+ '[#][#]%', Content.ContentTExt) > 0 or   					 
					PATINDEX('%' +ShortName+ '%', contentfilter.whereclause) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateSource) > 0 or   
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%', Template.TemplateText) > 0  or
					PATINDEX('%[%][%]' +ShortName+ '[%][%]%',  @emailsubject) > 0  or 
					PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
					PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
					PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
				)   
			end
		
 		end        
	end
	--JWelter adding this for multiple SS support 03042015
	--Sample XML for SmartSegments
	--<SmartSegments>
	--	<ssID id="1">
	--		<refBlastIDs>1,2,3</refBlastIDs>
	--	</ssID>
	--	<ssID id="2">
	--		<refBlastIDs>1,2,4</refBlastIDs>
	--	</ssID>
	--</SmartSegments>
	declare @docHandle int
	EXEC sp_xml_preparedocument @docHandle OUTPUT, @ActionType  
	INSERT INTO @actionTypes(SSID, action,refBlastIDs)
	SELECT SS, '',RefBlasts
	FROM OPENXML(@docHandle, N'/SmartSegments')   
	WITH   
	(  
		SS int 'ssID', RefBlasts varchar(1000) 'refBlastIDs'
	) 
	exec sp_xml_removedocument @docHandle

	update @actionTypes
	set action = (Select SmartSegmentName from SmartSegment where SmartSegmentID = SSID)
	--JWelter changing the way it looks for SS
	DECLARE @refBlastID varchar(1000)
  	if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'unopen'    )    
 	Begin        
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'unopen'
  		exec('insert into #tempA select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK) where BlastID in (' + @refBlastID +') ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' + @refBlastID +        
     									  ') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	if  exists(select Top 1 * from @actionTypes a where lower(a.action) =  'unclick')  	               
  	Begin        
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'unclick'
    	exec('insert into #tempA select EmailID from ecn_activity..BlastActivityClicks WITH (NOLOCK) where BlastID in (' + @refBlastID +') ') 

         
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E WITH (NOLOCK) Where BlastID in ( ' +  @refBlastID  +         
        								') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'softbounce'  )             
  	Begin        
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'softbounce'
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.BounceCodes bc WITH (NOLOCK) on bab.BounceCodeID = bc.BounceCodeID where BlastID = ' + convert(varchar(10),@BlastID) +   
      
            								' and BounceCode IN (''soft'', ''softbounce'')) '        
  	End  
    if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'open')
 	Begin        
			Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'open'
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens WITH (NOLOCK)  where BlastID in (' + @refBlastID +')) '        
  	end    
  	if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'click')        
 	Begin     
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'click'   
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks WITH (NOLOCK)  where BlastID in (' + @refBlastID +')) '         
  	end 
  	if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'suppressed')               
  	Begin        
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'suppressed'  
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab WITH (NOLOCK) join ECN_ACTIVITY.dbo.SuppressedCodes bc WITH (NOLOCK) on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @refBlastID +') and SupressedCode in (''Threshold'')) '       
  	End
	if exists(select Top 1 * from @actionTypes a where lower(a.action) = 'sent')
	BEGIN
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'sent'  
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @refBlastID + ')) '
	END
	if  exists(select Top 1 * from @actionTypes a where lower(a.action) = 'not sent')
	BEGIN
		Set @refBlastID = ''
		Select @refBlastID = refBlastIDs from @actionTypes where action = 'not sent'  
		set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends WITH(NOLOCK) WHERE BlastID in (' + @refBlastID + ')) '
	END

  	
  	SELECT	@CountSent = COUNT(EmailID) FROM #AlreadySent
  	SET		@CountToUse = 0  	

	
	--no UDFs	
  	IF @UDFExists = 0 or not exists(select top 1 GID from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	BEGIN
	
		--get all emails based on blast		
		EXEC ( 'insert into #ToSend  ' + 
		' select Emails.EmailID  from  Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
		' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
		' and EmailGroups.GroupID = ' + @GroupID +        
		' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID 
		')
		
		SELECT @CountAvailable = COUNT(EmailID) FROM #ToSend
		
		--if amount is % get actual number for @OverRideIsAmount
		IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) 
		BEGIN
			SET @OverrideAmount = (@CountAvailable * @OverrideAmount) / 100
			IF @OverrideAmount = 0
			BEGIN
				SET @OverrideAmount = 1
			END
		END
		
		--use @OverrideAmount or @CountAvailable
		IF @OverrideAmount > 0
		BEGIN
			SET @CountToUse = @OverrideAmount
		END
		ELSE
		BEGIN
			IF @CountAvailable - @CountSent < 0
			BEGIN
				SET @CountToUse = 0
			END
			ELSE
			BEGIN
				SET @CountToUse = @CountAvailable - @CountSent
			END
		END

		--get full records
		IF @OnlyCounts = 0	
		BEGIN
			
			--log suppressed
			SELECT 
				@CountToSuppress = @CountToUse - COUNT(ts.EmailID)
			FROM 
				#ToSend ts
					LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
					LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
			WHERE 
				os.EmailID IS NULL AND
				al.EmailID IS NULL
			
			IF @CountToSuppress > 0
			BEGIN
				IF @LogSuppressed = 1
				BEGIN	
					-- order to pull ascending threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
					exec ( 'INSERT into EmailActivityLog '+
					' select TOP ' + @CountToSuppress + ' os.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), os.Reason, NULL, ''Y'' '+
					' from #ToSend ts '+
						' JOIN #OtherSuppression os ON ts.EmailID = os.EmailID '+
						' LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID '+
					' WHERE  '+
						' al.EmailID IS NULL '+					
					' order by os.OrderID ASC')
				END
			END
					
			--no dynamic content	
			IF @DynamicSlotExists = 0 OR @BlastID = 0
			BEGIN		
				EXEC ( 'select top ' + @CountToUse + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
				' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
				'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
				'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +   
				', ' + @IsMTAPriority + ' as IsMTAPriority ' +     
				@DynamicTagColumns + ' ' +
				' from  #ToSend ts ' +
						' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
						' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +
						' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
						' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
						' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null' +
						' ' + @Filter + ' order by Emails.EmailID')	
			END
			--dynamic content
			ELSE
			BEGIN
				SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 

					EXEC ( @selectslotstr + @DynamicTagColumns + ' ' + ' from ( select top ' +  @CountToUse + ' Emails.EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
					' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
					'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' +  @BlastID + ''' as ConversionTrkCDE, ' +        
					'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +  
					', ' + @IsMTAPriority + ' as IsMTAPriority ' +          
					' from #ToSend ts ' +
					' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
					' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +
					' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
					' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
					' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' ) inn2 order by inn2.EmailID')  
			END
		END
		--just get count
		ELSE
		BEGIN
			--SELECT @CountToUse	-- 7/17/2013 - WGH: Need to use suppression and already sent in count
			
			SELECT
				case when @CountToUse > COUNT(ts.EmailID) then COUNT(ts.EmailID) else @CountToUse end
			FROM 
				#ToSend ts
					LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
					LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
			WHERE 
				os.EmailID IS NULL AND
				al.EmailID IS NULL
		END  		
 	END 
 	--UDFs exists        
 	ELSE       
  	BEGIN
  		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			--@StandAloneUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
			--@TransactionalUDFIDs VARCHAR(1000), modified 7/10/2014 as we were exceeding 1000 characters
			@StandAloneUDFIDs VARCHAR(2000),
			@TransactionalUDFIDs VARCHAR(2000),
			@StandAloneColumns VARCHAR(200),
			@TransactionalColumns VARCHAR(200),
			@StandAloneQuery VARCHAR(4000),
			@TransactionalQuery VARCHAR(4000),
			@StandAloneTempQuery VARCHAR(4000),
			@TransactionalTempQuery VARCHAR(4000),
			@StandAloneDrop VARCHAR(500),
			@TransactionalDrop VARCHAR(500)
			
		SET @StandAloneUDFs = ''
		SET @TransactionalUDFs = ''
		SET @StandAloneUDFIDs = ''
		SET @TransactionalUDFIDs  = ''
		SET @StandAloneColumns  = ''
		SET @TransactionalColumns = ''
		SET @StandAloneQuery  = ''
		SET @TransactionalQuery  = ''
		SET @standAloneTempQuery  = ''
		SET @TransactionalTempQuery  = ''		
		SET @StandAloneDrop  = ''
		SET @TransactionalDrop  = ''
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g WITH (NOLOCK) join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
				
		if LEN(@StandAloneUDFs) > 0
		Begin
			 set @StandAloneColumns = ' SAUDFs.* '
			 set @standAloneTempQuery = '
						SELECT * into #tempStandAlone
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv WITH (NOLOCK) join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ') and gdf.IsDeleted = 0 
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt;
						 
						 CREATE INDEX IDX_tempStandAlone_EmailID ON #tempStandAlone(tmp_EmailID);'
			 
			set @StandAloneQuery = ' left outer join #tempStandAlone SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'			 
			 
			set @StandAloneDrop  = 'drop table #tempStandAlone'
		End

		if LEN(@TransactionalUDFs) > 0
		Begin
			if LEN(@StandAloneColumns) > 0
			Begin
				set @TransactionalColumns = ', TUDFs.* '
			end
			Else
			Begin
				set @TransactionalColumns = ' TUDFs.* '
			End
			set @TransactionalTempQuery =  '  
						SELECT * into #tempTransactional
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	EmailDataValues edv WITH (NOLOCK)  join groupdatafields gdf WITH (NOLOCK) on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ') and gdf.IsDeleted = 0 
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt;
						 
						 CREATE INDEX IDX_tempTransactional_EmailID ON #tempTransactional(tmp_EmailID1);'			
			
			set @TransactionalQuery = '  left outer join #tempTransactional TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
			
			set @TransactionalDrop  = 'drop table #tempTransactional'
		End
  	

		--get all emails based on blast
		EXEC ( 
			@StandAloneTempQuery + ';' + 
			@TransactionalTempQuery + ';' +
		'insert into #ToSend  ' + 
		' select Emails.EmailID  from  Emails with (NOLOCK) ' +@StandAloneQuery + @TransactionalQuery + 
			' join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID;' +
			@StandAloneDrop + ';' + 
			@TransactionalDrop + ';  ' 
			)
		

		SELECT @CountAvailable = COUNT(EmailID) FROM #ToSend

		--if amount is % get actual number for @OverRideIsAmount
		IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) 
		BEGIN
			SET @OverrideAmount = (@CountAvailable * @OverrideAmount) / 100
			IF @OverrideAmount = 0
			BEGIN
				SET @OverrideAmount = 1
			END
		END
		
		--use @OverrideAmount or @CountAvailable
		IF @OverrideAmount > 0
		BEGIN
			SET @CountToUse = @OverrideAmount
		END
		ELSE
		BEGIN
			IF @CountAvailable - @CountSent < 0
			BEGIN
				SET @CountToUse = 0
			END
			ELSE
			BEGIN
				SET @CountToUse = @CountAvailable - @CountSent
			END
		END

		--get full records
		IF @OnlyCounts = 0	
		BEGIN
			--log suppressed
			SELECT 
				@CountToSuppress = @CountToUse - COUNT(ts.EmailID)
			FROM 
				#ToSend ts
					LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
					LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
			WHERE 
				os.EmailID IS NULL AND
				al.EmailID IS NULL
			
			IF @CountToSuppress > 0
			BEGIN	
				IF @LogSuppressed = 1
				BEGIN
					-- order to pull ascending threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
					exec ( 'INSERT into EmailActivityLog '+
					' select TOP ' + @CountToSuppress + ' os.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), os.Reason, NULL, ''Y'' '+
					' from #ToSend ts '+
						' JOIN #OtherSuppression os ON ts.EmailID = os.EmailID '+
						' LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID '+
					' WHERE  '+
						' al.EmailID IS NULL '+					
					' order by os.OrderID ASC')
				END
			END
				
			--no dynamic content	
			IF @DynamicSlotExists = 0 OR @BlastID = 0
			BEGIN		
				exec (
					@StandAloneTempQuery + ';' + 
					@TransactionalTempQuery + ';' +
					' IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0 and IsDeleted = 0 )
					BEGIN ' + 
					'select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
						'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
						'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + 
						@IsMTAPriority + ' as IsMTAPriority, ' + 
						@emailcolumns + @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns +    
						' into #tmpEmailList3  from #ToSend ts ' +  
						' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +  
						' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
						' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
						' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
						' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and al.EmailID is null and ot.EmailID is null ' + @Filter + ' order by emails.EmailID;
						
						select t.* from #tmpEmailList3 t 
						where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList3 t3) 
						order by t.emailID
						
						drop table #tmpEmailList3
					END
					ELSE
					BEGIN ' + 
					'select top ' + @CountToUse + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
						'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
						'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' +
						@IsMTAPriority + ' as IsMTAPriority, ' + 
						@emailcolumns +  @StandAloneColumns + @TransactionalColumns + @DynamicTagColumns + 
						'  into #tmpEmailList4  from #ToSend ts ' +
						' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
						' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' + @StandAloneQuery + @TransactionalQuery + 
						' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
						' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
						' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' order by emails.EmailID; 
						
						select t.* from #tmpEmailList4 t 
						where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList4 t4) 
						order by t.emailID
						
						drop table #tmpEmailList4
					END;' +				
					@StandAloneDrop + ';' + 
					@TransactionalDrop + ';' )
			END
			--dynamic content
			ELSE
			BEGIN
			
				SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
				EXEC ( 			
					@StandAloneTempQuery + ';' + 
					@TransactionalTempQuery + ';' +
					' ' +
				@selectslotstr + @DynamicTagColumns + ' into #tmpEmailList5 from ( select  Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
				'''eid='' + Convert(varchar,Emails.EmailID) +''&bid= '+ @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
				'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress,  '''+ @CurrDate + ''' as CurrDate, ' +  
				@IsMTAPriority + ' as IsMTAPriority, ' + @emailcolumns +    
				@StandAloneColumns + @TransactionalColumns +       
				' from #ToSend ts ' +
				' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
				' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +@StandAloneQuery + @TransactionalQuery + 
				' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
				' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
				' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' ) inn2 order by inn2.EmailID
				
				select t.* from #tmpEmailList5 t 
				where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList5 t5) 
				order by t.emailID
				
				drop table #tmpEmailList5;				
				
				' +				
				@StandAloneDrop + ';' + 
				@TransactionalDrop + ';' )
			END
		END
		--just get count
		ELSE
		BEGIN
			--SELECT @CountToUse	-- 7/17/2013 - WGH: Need to use suppression and already sent in count
			
			SELECT
				case when @CountToUse > COUNT(ts.EmailID) then COUNT(ts.EmailID) else @CountToUse end
			FROM 
				#ToSend ts
					LEFT OUTER JOIN #OtherSuppression os ON ts.EmailID = os.EmailID
					LEFT OUTER JOIN #AlreadySent al ON ts.EmailID = al.EmailID
			WHERE 
				os.EmailID IS NULL AND
				al.EmailID IS NULL
		END  
  	END              

	drop table #tempA        
	drop table #E        
	drop table #OtherSuppression
	drop table #ToSend
	drop table #AlreadySent
	drop table #ThresholdSuppression    


END
