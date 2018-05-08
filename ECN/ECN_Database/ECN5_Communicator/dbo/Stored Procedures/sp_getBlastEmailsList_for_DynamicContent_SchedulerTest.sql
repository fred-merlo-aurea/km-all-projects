CREATE PROC [dbo].[sp_getBlastEmailsList_for_DynamicContent_SchedulerTest] 
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
--	@OnlyCounts bit  
	
	
--set @CustomerID  =     	1
--set @BlastID = 448384
--set @GroupID =    97728    
--set @FilterID =        0
--set @BlastID_and_BounceDomain  = '448384@bounce2.com'    
--set @ActionType  = ''      
--set @refBlastID  = ''
--set @SupressionList = ''
--set @OnlyCounts= 0

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
		@AmountAlreadySent int = null

	declare	
			@thresholdlimit int,
			@blasttime datetime
			

	set @DynamicFromName = ''
	set @DynamicFromEmail = ''	
	set @DynamicReplyToEmail = ''
	set @mailRoute = ''

	select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)) from [BLAST] where BlastID = @BlastID
	
	select @OverrideAmount = OverrideAmount, @OverrideIsAmount = OverrideIsAmount, @testblast = TestBlast, @blasttime = sendtime, @DynamicFromName = DynamicFromName, @DynamicFromEmail = DynamicFromEmail, @DynamicReplyToEmail = DynamicReplyToEmail from blasts
 where blastID = @BlastID
	 
	--@thresholdlimit is the channel's threshold limit, 0 is unlimited
	select @basechannelID = c.basechannelID, @thresholdlimit = EmailThreshold from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c   join 
			[ECN5_ACCOUNTS].[DBO].[BASECHANNEL] bc on bc.BaseChannelID = c.BaseChannelID
	where customerID = @CustomerID
	
	set @CurrDate = convert(varchar, getdate(), 112)

	if @customerID = 1214 
		set @topcount = '50000'      
	else      
		set @topcount = '100000'   

 	set @SqlString = ''        
	set @EmailString  = ''        
	set @Col1  = ''        
	set @Col2  = ''        
	set @emailcolumns = ''
	set @Filter = ''
	set @IsMessageEnabledforThreshold = 0
	set @blastPriority = 0

	select @Filter = Whereclause from [FILTER] where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '

	set @filter = replace(@filter,'[emailaddress]','emailaddress')
	set @filter = replace(@filter,'emailaddress','[Emails].emailaddress')

        
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
				[ECN5_ACCOUNTS].[DBO].[CUSTOMERPRODUCT] cp 
				JOIN [ECN5_ACCOUNTS].[DBO].[PRODUCTDETAIL] pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN [ECN5_ACCOUNTS].[DBO].[PRODUCT] p ON pd.ProductID = p.ProductID 
		WHERE 
				cp.CustomerID = @customerID AND 
				p.ProductName = 'ecn.communicator' AND 
				pd.productdetailName = 'SetMessageThresholds' AND 
				cp.Active = 'y'
		
		--@customerProductPriority > 0 = Priority feature enabled		
		SELECT 
			@customerProductPriority = count(p.productname) 
		FROM 
				[ECN5_ACCOUNTS].[DBO].[CUSTOMERPRODUCT] cp 
				JOIN [ECN5_ACCOUNTS].[DBO].[PRODUCTDETAIL] pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN [ECN5_ACCOUNTS].[DBO].[PRODUCT] p ON pd.ProductID = p.ProductID 
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
				[BLAST] b
				JOIN [LAYOUT] l ON b.LayoutID = l.LayoutID
				JOIN [MESSAGETYPE] mt ON l.MessageTypeID = mt.MessageTypeID
		WHERE	
				b.BlastID = @BlastID 
				
				
				
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
						FROM	[BLAST] b   WITH (NOLOCK) 
							JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c   WITH (NOLOCK) ON b.CustomerID = c.customerID
							JOIN [LAYOUT] l   WITH (NOLOCK) on b.LayoutID = l.LayoutID
							JOIN [MESSAGETYPE] mt   WITH (NOLOCK) on l.MessageTypeID = mt.MessageTypeID
						WHERE 
							c.BaseChannelID = @basechannelID AND
							b.TestBlast = 'N' AND
							mt.Priority = 1 AND
							(
								(b.StatusCode='active' and b.blastID <> @blastID) or
								(b.StatusCode='pending' AND mt.SortOrder < @blastPriority) or
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
						FROM	[BLAST] b WITH (NOLOCK) 
							JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c  WITH (NOLOCK) ON b.CustomerID = c.customerID
						WHERE 
							c.BaseChannelID = @basechannelID AND
							b.TestBlast = 'N' AND
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
				FROM	[BLAST] b 
					JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c ON b.CustomerID = c.customerID
				WHERE 
					c.BaseChannelID = @basechannelID AND
					b.TestBlast = 'N' AND
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
			WHERE ntl.emailaddress is null
			GROUP BY se.emailaddress      
			HAVING SUM(totalemail) >= @thresholdlimit	
			
			DROP TABLE #sentemail
			
		END				
			
	End	
	
	IF @BlastID <> 0
	BEGIN
		select @emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @SupressionList = BlastSuppression, @layoutID = layoutID, @TestBlast = TestBlast from [BLAST] where blastID = @BlastID	
	END
	
	-- insert threshold, 7 day, list, mslist, cmslist, domain (upto counttoUse)
	--1. Threshold Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			1, e.EmailID, 'Threshold'
	FROM	
			[Emails] e WITH (NOLOCK)
				JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID
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
			@CustomerID <> 3137 --Neebo Rental Program
		BEGIN
			INSERT INTO #OtherSuppression
			SELECT	
					2, eal.EmailID, '7Day'
			FROM	
					EmailActivityLog eal WITH (NOLOCK)
						JOIN [BLAST] B WITH (NOLOCK) ON eal.BlastID = B.BlastID
			WHERE 	
					LayoutID = @LayoutID AND 
					SendTime > dateadd(d, -7 , @blasttime) AND
					ActionTypeCode = 'send'
		END
	END
	
	--3. List Suppression	
	if Len(ltrim(rtrim(@SupressionList))) > 0
	Begin
		INSERT INTO #OtherSuppression
		SELECT	
				3, eg.EmailID, 'List'
		FROM	
				[EmailGroups] eg WITH (NOLOCK) join
				(select items as groupID from fn_Split(@SupressionList, ',')) SL on eg.GroupID = SL.groupID
	End
	
	--4. Group Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			4, eg.EmailID, 'Group'
	FROM	
			[EmailGroups] eg WITH (NOLOCK)
				JOIN GROUPS g WITH (NOLOCK) ON eg.GroupID = g.GroupID
	WHERE 	
			g.CustomerID = @CustomerID AND 
			g.MasterSupression=1
			
	--5. Channel Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			5, e.EmailID, 'Channel'
	FROM	
			[Emails] e WITH (NOLOCK)
				JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
				JOIN ChannelMasterSuppressionList cms WITH (NOLOCK) ON e.EmailAddress = cms.EmailAddress
	WHERE 	
			eg.GroupID = @GroupID AND
			cms.BaseChannelID = @BasechannelID
	
	--6. Domain Suppression	
	INSERT INTO #OtherSuppression
	SELECT	
			6, e.EmailID, 'Domain'
	FROM	
			[Emails] e WITH (NOLOCK)
				JOIN [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID 
				JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c on e.CustomerID = c.CustomerID
				JOIN DomainSuppression ds WITH (NOLOCK) ON RIGHT(e.EmailAddress, LEN(e.EmailAddress) - CHARINDEX('@', e.EmailAddress)) = ds.Domain AND (e.CustomerID = ds.CustomerID OR c.BaseChannelID = ds.BaseChannelID)   
	WHERE 	
			eg.GroupID = @GroupID
	 	      
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	
 	create table #tempA (EmailID int)  

	--wgh testing
	--select * from #OtherSuppression
	--select @OverrideAmount
	--select @OverrideIsAmount
	if exists (select SampleBlastID from SampleBlasts where blastID = @BlastID)
	Begin
		select @OverrideAmount = Amount, @OverrideIsAmount = IsAmount from SampleBlasts where blastID = @BlastID
		--wgh testing
		--select @OverrideAmount
		--select @OverrideIsAmount
		
		insert into #AlreadySent
		select emailID, 'Other' 
		from emailactivitylog WITH (NOLOCK) 
		where (ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') and blastID = 
			(select blastID from SampleBlasts where blastID <> @BlastID and sampleID in (select sampleID from SampleBlasts where blastID = @BlastID))
	End
	
	--wgh testing
	--select * from #AlreadySent

 	if @BlastID = 0 
 	begin        
		if LEN(@Filter) > 0        
		Begin
			insert into @gdf         
			select distinct gdf.GroupDatafieldsID, gdf.ShortName 
			from Groups g   WITH (NOLOCK)  join GroupDataFields gdf   WITH (NOLOCK) on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0          
		end	
 	end        
 	else        
 	begin  
 		
 	   
		select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
		if(@MailRoute is null or LEN(@MailRoute) < 1)
		Begin
			select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
		End
		
		--declare @contentlength int
		--set @contentlength = 0
		
		--select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join [LAYOUT] l on c.contentID = l.ContentSlot1 where layoutID = @layoutID

		if exists(select filterID from [CONTENTFILTER] where layoutID = @layoutID)   --@contentlength = 8000 or 
		Begin
			select @emailcolumns = @emailcolumns + ' [Emails].' + columnname + ', ' from
			(select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'Emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			
			insert into @gdf         
			select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID     
		end
		else
		Begin		
			select @emailcolumns = @emailcolumns + ' [Emails].' + columnname + ', ' from
			(
			 select distinct emailColumns.columnName 
			  from  [BLAST] join         
				[LAYOUT] on [BLAST].layoutID = [LAYOUT].layoutID left outer join        
				Content on  Content.ContentID = [LAYOUT].ContentSlot1 or         
				   Content.ContentID = [LAYOUT].ContentSlot2 or         
				   Content.ContentID = [LAYOUT].ContentSlot3 or         
				   Content.ContentID = [LAYOUT].ContentSlot4 or         
				   Content.ContentID = [LAYOUT].ContentSlot5 or         
				   Content.ContentID = [LAYOUT].ContentSlot6 or         
				   Content.ContentID = [LAYOUT].ContentSlot7 or         
				   Content.ContentID = [LAYOUT].ContentSlot8 or         
				   Content.ContentID = [LAYOUT].ContentSlot9 left outer join        
				[TEMPLATE] on [LAYOUT].TemplateID  = [TEMPLATE].TemplateID left outer join        
				[CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID or [CONTENTFILTER].layoutID = [BLAST].layoutID cross join
				( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'Emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			  where         
				[BLAST].BlastID = @BlastID and 
				(
					PATINDEX('%' +emailColumns.columnName+ '%', Content.ContentSource) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%', Content.ContentText) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%', [CONTENTFILTER].whereclause) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%',  [TEMPLATE].TemplateSource) > 0 or   
					PATINDEX('%' +emailColumns.columnName+ '%',  [TEMPLATE].TemplateText) > 0 or
					PATINDEX('%' +emailColumns.columnName+ '%', @Filter) > 0 or
					PATINDEX('%' +emailColumns.columnName+ '%',  @emailsubject) > 0 
				)
			) emailColumns

	       
  			insert into @gdf         
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g join GroupDataFields gdf on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0      
  			union        
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  			from  [BLAST] join         
			[LAYOUT] on [BLAST].layoutID = [LAYOUT].layoutID left outer join        
			Content on  Content.ContentID = [LAYOUT].ContentSlot1 or         
			   Content.ContentID = [LAYOUT].ContentSlot2 or         
			   Content.ContentID = [LAYOUT].ContentSlot3 or         
			   Content.ContentID = [LAYOUT].ContentSlot4 or         
			   Content.ContentID = [LAYOUT].ContentSlot5 or         
			   Content.ContentID = [LAYOUT].ContentSlot6 or         
			   Content.ContentID = [LAYOUT].ContentSlot7 or         
			   Content.ContentID = [LAYOUT].ContentSlot8 or         
			   Content.ContentID = [LAYOUT].ContentSlot9 left outer join        
			[TEMPLATE] on [LAYOUT].TemplateID  = [TEMPLATE].TemplateID left outer join        
			[CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID or [CONTENTFILTER].layoutID = [BLAST].layoutID  join        
			Groups on Groups.GroupID = [BLAST].groupID and Groups.CustomerID = [BLAST].CustomerID join         
			GroupDataFields gdf on gdf.GroupID = Groups.GroupID         
		  where         
			[BLAST].BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and    
			(
			    PATINDEX('%sort="' +ShortName+ '"%', Content.ContentSource) > 0 or           
			    PATINDEX('%filter_field="' +ShortName+ '"%', Content.ContentSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', Content.ContentSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', Content.ContentTExt) > 0 or   
				PATINDEX('%' +ShortName+ '%', [CONTENTFILTER].whereclause) > 0 or   
				PATINDEX('%' +ShortName+ '%', [TEMPLATE].TemplateSource) > 0 or   
				PATINDEX('%' +ShortName+ '%', [TEMPLATE].TemplateText) > 0  or
				PATINDEX('%' +ShortName+ '%',  @emailsubject) > 0  or 
				PATINDEX('%' +ShortName+ '%',  @DynamicFromName) > 0 or 
				PATINDEX('%' +ShortName+ '%',  @DynamicFromEmail) > 0 or 
				PATINDEX('%' +ShortName+ '%',  @DynamicReplyToEmail) > 0
			)   
 		end        
	end
	
	--??sunil - change smartsegement filter to use ECN_ACTIVITY database.
	--if lower(@ActionType) = 'unopen'        
 --	Begin        
 -- 		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
 --   	set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
 --    									  ') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
 -- 	end         
 -- 	else if lower(@ActionType) = 'unclick'  	               
 -- 	Begin        
 --   	exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''click'' ') 

         
 --  		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' +  @refBlastID  +         
 --       								') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
 -- 	End        
 -- 	else if lower(@ActionType) = 'softbounce'               
 -- 	Begin        
 --  		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@BlastID) +         
 --           								' and e.ActionValue IN (''soft'', ''softbounce'')) '        
 -- 	End  
 --   else if lower(@ActionType) = 'open'        
 --	Begin        
 -- 		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
 --   	set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
 --    									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
 -- 	end    
 -- 	else if lower(@ActionType) = 'click'        
 --	Begin        
 -- 		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode =''click'' ') 
         
 --   	set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +    
 --    									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
 -- 	end 
 -- 	else if lower(@ActionType) = 'suppressed'               
 -- 	Begin        
 --  		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from EmailActivityLog Where BlastID in ( ' +  @refBlastID  + ') and ActionTypeCode = ''suppressed'' ) '       
 -- 	End
  	
  	if lower(@ActionType) = 'unopen'        
 	Begin        
  		exec('insert into #tempA select EmailID from ecn_activity.dbo.BlastActivityOpens where BlastID in (' + @refBlastID +') ') 
         
    	set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from ecn_activity..BlastActivitySends E Where BlastID in ( ' + @refBlastID +        
     									  ') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	               
  	Begin        
    	exec('insert into #tempA select EmailID from ecn_activity..BlastActivityClicks where BlastID in (' + @refBlastID +') ') 

         
   		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from ecn_activity..BlastActivitySends E Where BlastID in ( ' +  @refBlastID  +         
        								') and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivityBounces bab join ECN_ACTIVITY.dbo.BounceCodes bc on bab.BounceCodeID = bc.BounceCodeID where BlastID = ' + convert(varchar(10),@BlastID) +   
      
            								' and BounceCode IN (''soft'', ''softbounce'')) '        
  	End  
    else if lower(@ActionType) = 'open'        
 	Begin        
    	set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from ecn_activity.dbo.BlastActivityOpens  where BlastID in (' + @refBlastID +')) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from ecn_activity.dbo.BlastActivityClicks  where BlastID in (' + @refBlastID +')) '         
  	end 
  	else if lower(@ActionType) = 'suppressed'               
  	Begin        
   		set @EmailString = @EmailString +	' and [Emails].EmailID in (select EmailID from ECN_ACTIVITY.dbo.BlastActivitySuppressed bab join ECN_ACTIVITY.dbo.SuppressedCodes bc on bab.SuppressedCodeID = bc.SuppressedCodeID where BlastID in (' + @refBlastID +') and SupressedCode in (''Threshold'')) '       
  	End
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and [Emails].EmailID in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends where BlastID in (' + @refBlastID + ') '
	END
  	else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and [Emails].EmailID not in (SELECT EmailID from ECN_ACTIVITY.dbo.BlastActivitySends where BlastID in (' + @refBlastID + ') '
	END
  	
  	DECLARE @sqlTopCount NVARCHAR(4000) 
  	DECLARE @CountToUse int
  	DECLARE @CountToSuppress int
  	DECLARE @CountSent int
  	DECLARE @CountAvailable int
  	SELECT @CountSent = COUNT(EmailID) FROM #AlreadySent
  	
  	
	
	--no UDFs	--??sunil remove Email Parser and add fn_validateemailaddress function.
  	IF not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	BEGIN
  --		if @LogSuppressed = 0
		--begin
		--	print '1'
		--	print @Filter
		--	print '2'
		--	print @EmailString
		--	print '2.5'
		--	print ( 'insert into #ToSend  ' + 
		--	' select [Emails].EmailID  from  [Emails] with (NOLOCK) join [EmailGroups] with (NOLOCK) on [EmailGroups].EmailID = [Emails].EmailID ' +         
		--	' where [Emails].CustomerID = ' + convert(varchar,@CustomerID) + ' and [Emails].EmailAddress like ''%@%.%'' and [EmailGroups].SubscribeTypeCode = ''S''' +        
		--	' and [EmailGroups].GroupID = ' + convert(varchar,@GroupID) +        
		--	' and dbo.fn_ValidateEmailAddress([Emails].EmailAddress) = 1 ' +   
		--	' ' + @Filter + ' ' +  @EmailString + ' order by [Emails].EmailID 
		--	')
		--end			
		--get all [Emails] based on blast		
		EXEC ( 'insert into #ToSend  ' + 
		' select [Emails].EmailID  from  [Emails] with (NOLOCK) join [EmailGroups] with (NOLOCK) on [EmailGroups].EmailID = [Emails].EmailID ' +         
		' where [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%'' and [EmailGroups].SubscribeTypeCode = ''S''' +        
		' and [EmailGroups].GroupID = ' + @GroupID +        
		' and dbo.fn_ValidateEmailAddress([Emails].EmailAddress) = 1 ' +   
		' ' + @Filter + ' ' +  @EmailString + ' order by [Emails].EmailID 
		')
		--if @LogSuppressed = 0
		--begin
		--	print '3'
		--end
		--WGH testing
		--select * from #ToSend
		
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
		
		IF @OverrideAmount - @CountSent > @topcount
		BEGIN
			--use @topcount
			SET @CountToUse = @topcount
		END
		ELSE
		BEGIN
			--use @OverrideAmount or @CountAvailable
			IF @OverrideAmount > 0
			BEGIN
				SET @CountToUse = @OverrideAmount-- - @CountSent
			END
			ELSE
			BEGIN
				IF (@CountAvailable - @CountSent) > @topcount
				BEGIN
					SET @CountToUse = @topcount - @CountSent
				END
				ELSE
				BEGIN
					SET @CountToUse = @CountAvailable - @CountSent
				END
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
			
			--wgh testing
			--select @CountToSuppress
			--select @CountToUse
			
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
			IF not exists(select filterID from [CONTENTFILTER] where layoutID = @layoutID) OR @BlastID = 0
			BEGIN		
				EXEC ( 'select top ' + @CountToUse + ' [Emails].EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, [Emails].EmailAddress, CustomerID, ' + @emailcolumns + 
				' groupID, FormatTypeCode, SubscribeTypeCode,  [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +        
				'''eid='' + Convert(varchar,[Emails].EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
				'''bounce_''+Convert(varchar,[Emails].EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
				' from  #ToSend ts ' +
						' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +
						' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID ' +
						' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
						' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
						' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null')	
			END
			--dynamic content
			ELSE
			BEGIN
				SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 

					EXEC ( @selectslotstr + ' from ( select top ' +  @CountToUse + ' [Emails].EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  [Emails].EmailAddress, CustomerID, ' + @emailcolumns + 
					' groupID, FormatTypeCode, SubscribeTypeCode,  [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +        
					'''eid='' + Convert(varchar,[Emails].EmailID)+''&bid=' +  @BlastID + ''' as ConversionTrkCDE, ' +        
					'''bounce_''+Convert(varchar,[Emails].EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
					' from #ToSend ts ' +
					' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +
					' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID ' +
					' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
					' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
					' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null) inn2 order by inn2.EmailID')  
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
			@TransactionalUDFIDs VARCHAR(1000)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @gdf tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)
				
		if LEN(@standaloneUDFs) > 0
		Begin
			 set @sColumns = ' SAUDFs.* '
			 set @standAloneQuery= ' left outer join			
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue
							from	[EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ')
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
					) 
				SAUDFs on [Emails].emailID = SAUDFs.tmp_EmailID'
		End

		if LEN(@TransactionalUDFs) > 0
		Begin
			if LEN(@sColumns) > 0
			Begin
				set @tColumns = ', TUDFs.* '
			end
			Else
			Begin
				set @tColumns = ' TUDFs.* '
			End
			set @TransactionalQuery= '  left outer join
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	[EMAILDATAVALUES] edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ')
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on [Emails].emailID = TUDFs.tmp_EmailID1 '
		End
  	

		--get all [Emails] based on blast
		EXEC ( 'insert into #ToSend  ' + 
		' select [Emails].EmailID  from  [Emails] with (NOLOCK) ' +@standAloneQuery + @TransactionalQuery + 
			' join [EmailGroups] with (NOLOCK) on [EmailGroups].EmailID = [Emails].EmailID ' +         
			' where [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%'' and [EmailGroups].SubscribeTypeCode = ''S''' +        
			' and [EmailGroups].GroupID = ' + @GroupID +        
			' and dbo.fn_ValidateEmailAddress([Emails].EmailAddress) = 1' +   
			' ' + @Filter + ' ' +  @EmailString + ' order by [Emails].EmailID 
		') 
		
		--wgh testing
		--select * from #ToSend	

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

		IF @OverrideAmount - @CountSent > @topcount
		BEGIN
			--use @topcount
			SET @CountToUse = @topcount
		END
		ELSE
		BEGIN
			--use @OverrideAmount or @CountAvailable
			IF @OverrideAmount > 0
			BEGIN
				SET @CountToUse = @OverrideAmount-- - @CountSent
			END
			ELSE
			BEGIN
				IF (@CountAvailable - @CountSent) > @topcount
				BEGIN
					SET @CountToUse = @topcount - @CountSent
				END
				ELSE
				BEGIN
					SET @CountToUse = @CountAvailable - @CountSent
				END
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
			
			--wgh testing
			--select '1'
			
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
			
			--wgh testing
			--select @CountToSuppress
				
			--no dynamic content	
			IF not exists(select filterID from [CONTENTFILTER] where layoutID = @layoutID) OR @BlastID = 0
			BEGIN		
				if (@BlastID = 582516)
				Begin
				exec ( ' 
					select top 100000 [Emails].EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, [Emails].EmailAddress, [Emails].CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,[Emails].EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +      
					'''bounce_''+ Convert(varchar,[Emails].EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns + '[Emails].user1 ' + 
					'  into #tmpEmailList4  from #ToSend ts ' +
					' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +
					' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID '  +
					' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
					' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
					' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null order by [Emails].EmailID; 
					
					select t.* from #tmpEmailList4 t 
					where	t.emailID in (select distinct top 100000 emailID from #tmpEmailList4 t4) 
					order by t.emailID
					
					drop table #tmpEmailList4
				')				
				End
				Else
				Begin						
				exec ( ' 
				IF exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0)
				BEGIN
					select [Emails].EmailID,' + @BlastID + ' as BlastID, [Emails].EmailAddress, '''+@mailRoute+''' as mailRoute, [Emails].CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,[Emails].EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +      
					'''bounce_''+ Convert(varchar,[Emails].EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns +   
					@sColumns + @tColumns +      
					' into #tmpEmailList3  from #ToSend ts ' +  
					' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +  
					' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID ' +@standAloneQuery + @TransactionalQuery + 
					' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
					' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
					' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and al.EmailID is null and ot.EmailID is null order by [Emails].EmailID;
					
					select t.* from #tmpEmailList3 t 
					where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList3 t3) 
					order by t.emailID
					
					drop table #tmpEmailList3
				END
				ELSE
				BEGIN
					select top ' + @CountToUse + ' [Emails].EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, [Emails].EmailAddress, [Emails].CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,[Emails].EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +      
					'''bounce_''+ Convert(varchar,[Emails].EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns +
					@sColumns + @tColumns +   
					'  into #tmpEmailList4  from #ToSend ts ' +
					' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +
					' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID '  +@standAloneQuery + @TransactionalQuery + 
					' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
					' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
					' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null order by [Emails].EmailID; 
					
					select t.* from #tmpEmailList4 t 
					where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList4 t4) 
					order by t.emailID
					
					drop table #tmpEmailList4
				END
				')
				End		
				
			END
			--dynamic content
			ELSE
			BEGIN
				--wgh testing
				--select '3'
			
				SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
				--top ' +  @CountToUse + '
				EXEC ( @selectslotstr + ' into #tmpEmailList5 from ( select  [Emails].EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, [Emails].EmailAddress, [Emails].CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
				'''eid='' + Convert(varchar,[Emails].EmailID) +''&bid= '+ @BlastID + ''' as ConversionTrkCDE, [EmailGroups].CreatedOn, [EmailGroups].LastChanged, ' +      
				'''bounce_''+ Convert(varchar,[Emails].EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress,  '''+ @CurrDate + ''' as CurrDate, ' +  @emailcolumns +    
				@sColumns + @tColumns +       
				' from #ToSend ts ' +
				' join [Emails] with (NOLOCK) on ts.EmailID = [Emails].EmailID ' +
				' join [EmailGroups] with (NOLOCK) on [Emails].EmailID = [EmailGroups].EmailID ' +@standAloneQuery + @TransactionalQuery + 
				' left outer join #OtherSuppression ot on [Emails].EmailID = ot.EmailID ' +
				' left outer join #AlreadySent al on [Emails].EmailID = al.EmailID ' +
				' where [Emails].CustomerID = ' + @CustomerID + ' and [EmailGroups].GroupID = ' + @GroupID + ' and ot.EmailID is null and al.EmailID is null) inn2 order by inn2.EmailID
				
				select t.* from #tmpEmailList5 t 
				where	t.emailID in (select distinct top ' + @CountToUse + ' emailID from #tmpEmailList5 t5) 
				order by t.emailID
				
				drop table #tmpEmailList5				
				
				') 
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
