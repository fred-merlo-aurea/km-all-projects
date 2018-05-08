CREATE PROC [dbo].[sp_getBlastEmailsList_for_DynamicContent] 
(
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID varchar(2000),
	@SupressionList varchar(2000),
	@OnlyCounts bit,
	@LogSuppressed bit = 1        
)
as

--declare @CustomerID int,        	
--	@BlastID int,
--	@GroupID int,        
--	@FilterID int,        
--	@BlastID_and_BounceDomain varchar(250),        
--	@ActionType varchar(10),        
--	@refBlastID varchar(2000),
--	@SupressionList varchar(2000),
--	@OnlyCounts bit,
--	@LogSuppressed bit
		
--set @CustomerID  =     	3053
--set @BlastID = 578456
--set @GroupID =    48310
--set @FilterID =        0
--set @BlastID_and_BounceDomain  = ''    
--set @ActionType  = ''      
--set @refBlastID  = ''
--set @SupressionList = ''
--set @OnlyCounts= 0
--set @LogSuppressed = 0

BEGIN        
	set NOCOUNT ON    

	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),        
		@Col1 varchar(8000),        
		@Col2 varchar(8000),      
		@topcount varchar(10),
		@emailcolumns varchar(2000),
		@emailsubject varchar(1000),
		@EmailFrom varchar(100),
		@Filter varchar(8000),
		@layoutID int,
		@TestBlast  varchar(5)  ,
		@selectslotstr varchar(8000),
		@BasechannelID int,
		@DynamicFromName	varchar(100),       
		@DynamicFromEmail	varchar(100),      
		@DynamicReplyToEmail  varchar(100),
		@CurrDate varchar(10),
		@mailRoute varchar(100),
		@customerproductthreshold int,
		@customerproductpriority int,
		@blastpriority int,
		@IsMessageEnabledforThreshold bit,
		@Domain varchar(100),
		@OverrideAmount int = null,
		@OverrideIsAmount int = null,
		@AmountAlreadySent int = null,
		@BlastType varchar(50),
		@CampaignItemID int,
		@IsMTAPriority bit = 0

	declare	
			@thresholdlimit int,
			@blasttime datetime
			

	set @DynamicFromName = ''
	set @DynamicFromEmail = ''	
	set @DynamicReplyToEmail = ''
	set @mailRoute = ''	
	
	select 
		@Domain = RIGHT(b.EmailFrom, LEN(b.EmailFrom) - CHARINDEX('@', b.EmailFrom)),@OverrideAmount = b.OverrideAmount, 
		@OverrideIsAmount = b.OverrideIsAmount, @testblast = b.TestBlast, @blasttime = b.sendtime, @DynamicFromName = b.DynamicFromName, 
		@DynamicFromEmail = b.DynamicFromEmail, @DynamicReplyToEmail = b.DynamicReplyToEmail, @BlastType = b.BlastType
	from 
		blast b with (nolock)
	where 
		b.blastID = @BlastID and b.StatusCode <> 'Deleted'
		
	--if EXISTS(SELECT * FROM BlastSingles where BlastID = @BlastID) or rtrim(ltrim(@TestBlast)) = 'Y'
	if rtrim(ltrim(@TestBlast)) = 'Y'
	Begin
		set @IsMTAPriority = 1
		select @CampaignItemID = CampaignItemID from CampaignItemTestBlast where blastid = @BlastID and IsDeleted = 0
	End
	else
	Begin
		select @CampaignItemID = CampaignItemID from CampaignItemBlast where blastid = @BlastID and IsDeleted = 0
	End
	 
	--@thresholdlimit is the channel's threshold limit, 0 is unlimited
	select @basechannelID = c.basechannelID, @thresholdlimit = EmailThreshold from ecn5_accounts..customer c   join 
			ecn5_accounts..BaseChannel bc on bc.BaseChannelID = c.BaseChannelID
	where customerID = @CustomerID and c.IsDeleted = 0 and bc.IsDeleted = 0
	
	set @CurrDate = convert(varchar, getdate(), 112)

	set @SqlString = ''        
	set @EmailString  = ''        
	set @Col1  = ''        
	set @Col2  = ''        
	set @emailcolumns = ''
	set @Filter = ''
	set @IsMessageEnabledforThreshold = 0
	set @blastPriority = 0

	select @Filter = Whereclause from filter where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '

	set @filter = replace(@filter,'[emailaddress]','emailaddress')
	set @filter = replace(@filter,'emailaddress','emails.emailaddress')

        
 	declare @gdf table(GID int, ShortName varchar(50))        

 	create table #AlreadySent (EmailID int, Which varchar(50))  
 	CREATE UNIQUE CLUSTERED INDEX AlreadySent_ind on #AlreadySent(EmailID, Which) with ignore_dup_key
 	
 	create table #ToSend (EmailID int)  
 	CREATE UNIQUE CLUSTERED INDEX ToSend_ind on #ToSend (EmailID) with ignore_dup_key
 	
 	create table #OtherSuppression (OrderID int, EmailID int, Reason varchar(50))  
 	CREATE UNIQUE CLUSTERED INDEX OtherSuppression_ind on #OtherSuppression(EmailID) with ignore_dup_key
 	
 	create table #ThresholdSuppression (EmailAddress varchar(100))  
 	CREATE UNIQUE CLUSTERED INDEX ThresholdSuppression_ind on #ThresholdSuppression(EmailAddress) with ignore_dup_key
 	 	
 	INSERT INTO #AlreadySent
	SELECT 
		EmailID, 'Current'
	FROM 
		EmailActivityLog WITH (NOLOCK) 
	WHERE 
		(ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') AND --??SUNIL - SHOULD NOT INCLUDE SUPPRESSED
		BlastID = @BlastID
 	
 	if (@BlastID > 0 and @testblast <> 'Y')
 	Begin
 		
 		--@customerProductThreshold > 0 = Threshold feature enabled
 		SELECT 
			@customerProductThreshold = count(p.productname) 
		FROM 
				ecn5_accounts..CustomerProduct cp 
				JOIN ecn5_accounts..ProductDetail pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Product p ON pd.ProductID = p.ProductID 
		WHERE 
				cp.CustomerID = @customerID AND 
				p.ProductName = 'ecn.communicator' AND 
				pd.productdetailName = 'SetMessageThresholds' AND 
				cp.Active = 'y' 
		
		--@customerProductPriority > 0 = Priority feature enabled		
		SELECT 
			@customerProductPriority = count(p.productname) 
		FROM 
				ecn5_accounts..CustomerProduct cp 
				JOIN ecn5_accounts..ProductDetail pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Product p ON pd.ProductID = p.ProductID 
		WHERE 
				cp.CustomerID = @customerID AND 
				p.ProductName = 'ecn.communicator' AND 
				pd.productdetailName = 'SetMessagePriority' AND 
				cp.Active = 'y' 
				
		--@IsMessageEnabledforThreshold - 1 = Message threshold enabled
		--@blastPriority - 1 = Message priority enabled
		SELECT	
				@IsMessageEnabledforThreshold = Threshold,
				@blastPriority = (case when mt.Priority = 1 then isnull(mt.SortOrder,0) else 0 end)
		FROM	
				Blast b
				JOIN Layout l ON b.LayoutID = l.LayoutID
				JOIN MessageType mt ON l.MessageTypeID = mt.MessageTypeID
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
				FROM	Blast b 
					JOIN ecn5_accounts..Customer c ON b.CustomerID = c.customerID
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
			left outer join ChannelNoThresholdList ntl  WITH (NOLOCK) on se.emailaddress = ntl.emailaddress and ntl.basechannelID = @basechannelID
			WHERE ntl.emailaddress is null and ntl.IsDeleted = 0 
			GROUP BY se.emailaddress      
			HAVING SUM(totalemail) >= @thresholdlimit	
			
			DROP TABLE #sentemail
			
		END				
			
	End	
	
	IF @BlastID <> 0
	BEGIN
		select @emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @SupressionList = BlastSuppression, @layoutID = layoutID, @TestBlast = TestBlast from blast where blastID = @BlastID and StatusCode <> 'Deleted'
	END
	
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
			@CustomerID <> 3137 and --Neebo Rental Program
			@CustomerID <> 1941 -- KM Master Lists.
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
	
	--3. List Suppression	
	if Len(ltrim(rtrim(@SupressionList))) > 0
	Begin
		INSERT INTO #OtherSuppression
		SELECT	
				3, eg.EmailID, 'List'
		FROM	
				EmailGroups eg WITH (NOLOCK) join
				(select items as groupID from fn_Split(@SupressionList, ',')) SL on eg.GroupID = SL.groupID
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
				JOIN ecn5_accounts..Customer c on e.CustomerID = c.CustomerID
				JOIN DomainSuppression ds WITH (NOLOCK) ON RIGHT(e.EmailAddress, LEN(e.EmailAddress) - CHARINDEX('@', e.EmailAddress)) = ds.Domain AND (e.CustomerID = ds.CustomerID OR c.BaseChannelID = ds.BaseChannelID)   
	WHERE 	
			eg.GroupID = @GroupID and c.IsDeleted = 0 and ds.IsDeleted = 0
	 	      
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	
 	create table #tempA (EmailID int)  

	if Lower(@BlastType) = 'ab'
	Begin
		insert into #AlreadySent
		select emailID, 'Other' 
		from emailactivitylog WITH (NOLOCK) 
		where (ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') and blastID = 
			(select blastID from CampaignItemBlast where blastID <> @BlastID and CampaignItemID = @CampaignItemID and IsDeleted = 0)
	End

 	if @BlastID = 0 
 	begin        
		if LEN(@Filter) > 0        
		Begin
			insert into @gdf         
			select distinct gdf.GroupDatafieldsID, gdf.ShortName 
			from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0         
		end	
 	end        
 	else        
 	begin  
 		
 	   
		select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
		if(@MailRoute is null or LEN(@MailRoute) < 1)
		Begin
			select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
		End
		
		if exists(select filterID from contentFilter where layoutID = @layoutID and IsDeleted = 0)   --@contentlength = 8000 or 
		Begin
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			
			insert into @gdf         
			select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID and IsDeleted = 0  
		end
		else
		Begin		
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(
			 select distinct emailColumns.columnName 
			  from  blast join         
				Layout on Blast.layoutID = Layout.layoutID left outer join        
				Content on  Content.ContentID = Layout.ContentSlot1 or         
				   Content.ContentID = Layout.ContentSlot2 or         
				   Content.ContentID = Layout.ContentSlot3 or         
				   Content.ContentID = Layout.ContentSlot4 or         
				   Content.ContentID = Layout.ContentSlot5 or         
				   Content.ContentID = Layout.ContentSlot6 or         
				   Content.ContentID = Layout.ContentSlot7 or         
				   Content.ContentID = Layout.ContentSlot8 or         
				   Content.ContentID = Layout.ContentSlot9 left outer join        
				Template on Layout.TemplateID  = Template.TemplateID left outer join        
				contentFilter on contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID cross join
				( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			  where         
				Blast.BlastID = @BlastID and 
				blast.StatusCode <> 'Deleted' and
				Layout.IsDeleted = 0 and
				Content.IsDeleted = 0 and
				Template.IsDeleted = 0 and
				contentFilter.IsDeleted = 0 and
				(
					PATINDEX('%' +emailColumns.columnName+ '%', Content.ContentSource) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%', Content.ContentText) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%', contentFilter.whereclause) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%',  Template.TemplateSource) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%',  Template.TemplateText) > 0 or
					PATINDEX('%' +emailColumns.columnName+ '%', @Filter) > 0 or
					PATINDEX('%' +emailColumns.columnName+ '%',  @emailsubject) > 0 
				)
			) emailColumns

	       
  			insert into @gdf         
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g join GroupDataFields gdf on gdf.GroupID = g.GroupID      
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0 and gdf.IsDeleted = 0     
  			union        
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  			from  blast join         
			Layout on Blast.layoutID = Layout.layoutID left outer join        
			Content on  Content.ContentID = Layout.ContentSlot1 or         
			   Content.ContentID = Layout.ContentSlot2 or         
			   Content.ContentID = Layout.ContentSlot3 or         
			   Content.ContentID = Layout.ContentSlot4 or         
			   Content.ContentID = Layout.ContentSlot5 or         
			   Content.ContentID = Layout.ContentSlot6 or         
			   Content.ContentID = Layout.ContentSlot7 or         
			   Content.ContentID = Layout.ContentSlot8 or         
			   Content.ContentID = Layout.ContentSlot9 left outer join        
			Template on Layout.TemplateID  = Template.TemplateID left outer join        
			contentFilter on contentFilter.contentID = content.contentID or contentFilter.layoutID = blast.layoutID  join        
			Groups on Groups.GroupID = blast.groupID and Groups.CustomerID = Blast.CustomerID join         
			GroupDataFields gdf on gdf.GroupID = Groups.GroupID         
		  where         
			Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and  			 
			blast.StatusCode <> 'Deleted' and
			Layout.IsDeleted = 0 and
			Content.IsDeleted = 0 and
			Template.IsDeleted = 0 and
			contentFilter.IsDeleted = 0 and 
			gdf.IsDeleted = 0 and
			(
			    PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
			    PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', Content.ContentSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', Content.ContentTExt) > 0 or   
				PATINDEX('%' +ShortName+ '%', contentfilter.whereclause) > 0 or   
				PATINDEX('%' +ShortName+ '%', Template.TemplateSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', Template.TemplateText) > 0  or
				PATINDEX('%' +ShortName+ '%',  @emailsubject) > 0  or 
				PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
				PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
				PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
			)   
 		end        
	end

  	if lower(@ActionType) = 'unopen'        
 	Begin        
  		exec('insert into #tempA select EmailID from ecn_activity.dbo.BlastActivityOpens where BlastID in (' + @refBlastID +') ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E Where BlastID in ( ' + @refBlastID +        
     									  ') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	               
  	Begin        
    	exec('insert into #tempA select EmailID from ecn_activity..BlastActivityClicks where BlastID in (' + @refBlastID +') ') 

         
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ecn_activity..BlastActivitySends E Where BlastID in ( ' +  @refBlastID  +         
        								') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab join ECN_ACTIVITY.dbo.BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID where BlastID = ' + convert(varchar(10),@BlastID) +   
      
            								' and BounceCode IN (''soft'', ''softbounce'')) '        
  	End  
    else if lower(@ActionType) = 'open'        
 	Begin        
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens  where BlastID in (' + @refBlastID +')) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks  where BlastID in (' + @refBlastID +')) '         
  	end 
  	else if lower(@ActionType) = 'suppressed'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab join ECN_ACTIVITY.dbo.SuppressedCodes bc on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @refBlastID +') and SupressedCode in (''Threshold'')) '       
  	End
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends bas where bas.BlastID in (' + @refBlastID + ') '
	END
  	else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends bas where bas.BlastID in (' + @refBlastID + ') '
	END
  	
  	DECLARE @sqlTopCount NVARCHAR(4000) 
  	DECLARE @CountToUse int
  	DECLARE @CountToSuppress int
  	DECLARE @CountSent int
  	DECLARE @CountAvailable int
  	
  	SELECT	@CountSent = COUNT(EmailID) FROM #AlreadySent
  	SET		@CountToUse = 0  	
	
	--no UDFs	--??sunil remove Email Parser and add fn_validateemailaddress function.
  	IF not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID and gdf.IsDeleted = 0) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	BEGIN
	
		--get all emails based on blast		
		EXEC ( 'insert into #ToSend  ' + 
		' select Emails.EmailID  from  Emails with (NOLOCK) join EmailGroups with (NOLOCK) on EmailGroups.EmailID = Emails.EmailID ' +         
		' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
		' and EmailGroups.GroupID = ' + @GroupID +        
		' and dbo.fn_ValidateEmailAddress(Emails.EmailAddress) = 1 ' +   
		' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID 
		')
		
		SELECT @CountAvailable = COUNT(EmailID) FROM #ToSend
		
		--if amount is % get actual number for @OverRideIsAmount
		IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) --??sunil remove Email Parser and add fn_validateemailaddress function.
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
			IF not exists(select filterID from contentFilter where layoutID = @layoutID and IsDeleted = 0) OR @BlastID = 0
			BEGIN		
				EXEC ( 'select top ' + @CountToUse + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
				' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
				'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
				'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +   
				', ' + @IsMTAPriority + ' as IsMTAPriority ' +     
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

					EXEC ( @selectslotstr + ' from ( select top ' +  @CountToUse + ' Emails.EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
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
			SELECT @CountToUse
		END  		
 	END 
 	--UDFs exists        
 	ELSE       
  	BEGIN
  		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			@StandAloneUDFIDs VARCHAR(1000),
			@TransactionalUDFIDs VARCHAR(1000),
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
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
				
		if LEN(@StandAloneUDFs) > 0
		Begin
			 set @StandAloneColumns = ' SAUDFs.* '
			 set @standAloneTempQuery = '
						SELECT * into #tempStandAlone
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
							from	EmailDataValues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
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
							from	EmailDataValues edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
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
			' and dbo.fn_ValidateEmailAddress(Emails.EmailAddress) = 1' +   
			' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID;' +
			@StandAloneDrop + ';' + 
			@TransactionalDrop + ';  ' 
			)
		

		SELECT @CountAvailable = COUNT(EmailID) FROM #ToSend

		--if amount is % get actual number for @OverRideIsAmount
		IF(@OverRideIsAmount is not null and @OverRideIsAmount = 0) --??sunil remove Email Parser and add fn_validateemailaddress function.
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
			IF not exists(select filterID from contentFilter where layoutID = @layoutID and IsDeleted = 0) OR @BlastID = 0
			BEGIN		
				if (@BlastID = 582516)
				Begin
					exec ( ' 
						select top 100000 Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
						'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
						'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + 
						@IsMTAPriority + ' as IsMTAPriority, ' + @emailcolumns + 'emails.user1 ' + 
						'  into #tmpEmailList4  from #ToSend ts ' +
						' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +
						' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID '  +
						' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
						' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
						' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null ' + @Filter + ' order by emails.EmailID; 
						
						select t.* from #tmpEmailList4 t 
						where	t.emailID in (select distinct top 100000 emailID from #tmpEmailList4 t4) 
						order by t.emailID
						
						drop table #tmpEmailList4
					')				
				End
				Else
				Begin
					--print '1'	
					--exec ( ' 
								
					--	IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0)
					--	BEGIN ' + 
					--		@StandAloneTempQuery + ';' + 
					--		@TransactionalTempQuery + ';' +
					--		' select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					--		'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
					--		'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns +   
					--		@StandAloneColumns + @TransactionalColumns +      
					--		' into #tmpEmailList3  from #ToSend ts ' +  
					--		' join Emails with (NOLOCK) on ts.EmailID = Emails.EmailID ' +  
					--		' join EmailGroups with (NOLOCK) on Emails.EmailID = EmailGroups.EmailID ' +@StandAloneQuery + @TransactionalQuery + 
					--		' left outer join #AlreadySent al on Emails.EmailID = al.EmailID ' +
					--		' left outer join #OtherSuppression ot on Emails.EmailID = ot.EmailID ' +
					--		' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and al.EmailID is null and ot.EmailID is null order by emails.EmailID;
							
					--		select t.* from #tmpEmailList3 t 
					--		where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList3 t3) 
					--		order by t.emailID
							
					--		drop table #tmpEmailList3
					--	END;' +
						
					--@StandAloneDrop + ';' + 
					--@TransactionalDrop + ';' )				
					exec (
					@StandAloneTempQuery + ';' + 
					@TransactionalTempQuery + ';' +
					' IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0 and IsDeleted = 0 )
					BEGIN ' + 
					'select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
						'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
						'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + 
						@IsMTAPriority + ' as IsMTAPriority, ' + @emailcolumns +   
						@StandAloneColumns + @TransactionalColumns +     
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
						@IsMTAPriority + ' as IsMTAPriority, ' + @emailcolumns +
						@StandAloneColumns + @TransactionalColumns +   
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
					--print '2'
				End		
				
			END
			--dynamic content
			ELSE
			BEGIN
			
				SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
				--top ' +  @CountToUse + '
				EXEC ( 			
					@StandAloneTempQuery + ';' + 
					@TransactionalTempQuery + ';' +
					' ' +
				@selectslotstr + ' into #tmpEmailList5 from ( select  Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
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
			SELECT @CountToUse
		END  
  	END              

	drop table #tempA        
	drop table #E        
	drop table #OtherSuppression
	drop table #ToSend
	drop table #AlreadySent
	drop table #ThresholdSuppression    


END