USE [ecn5_communicator]
GO

/****** Object:  StoredProcedure [dbo].[sp_getBlastEmailsList_for_DynamicContentIncludes7DayReporting]    Script Date: 06/13/2011 07:47:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE PROC [dbo].[sp_getBlastEmailsList_for_DynamicContentIncludes7DayReporting] 
(
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID varchar(2000),
	@SupressionList varchar(2000),
	@OnlyCounts bit        
)
as
    

--select top 100 * from blasts where customerID = 125 order by blastID desc
--set @CustomerID = 2486
--set 	@BlastID = 142953
--set 	@GroupID = 19631
--set 	@FilterID = '0'
--set 	@BlastID_and_BounceDomain = ''
--set 	@ActionType = ''        
--set 	@refBlastID = ''
--set 	@SupressionList = ''
--set 	@OnlyCounts = 0

--set @CustomerID = 1
--set 	@BlastID = 307575
--set 	@GroupID = 46184
--set 	@FilterID = '0'
--set 	@BlastID_and_BounceDomain = ''
--set 	@ActionType = ''        
--set 	@refBlastID = ''
--set 	@SupressionList = ''
--set 	@OnlyCounts = 0

--set		@CustomerID = 1
--set 	@BlastID = 307575
--set 	@GroupID = 46184
--set 	@FilterID = '0'
--set 	@BlastID_and_BounceDomain = ''
--set 	@ActionType = ''        
--set 	@refBlastID = ''
--set 	@SupressionList = ''
--set 	@OnlyCounts = 0

BEGIN        
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),        
		@Col1 varchar(8000),        
		@Col2 varchar(8000),      
		@topcount varchar(10),
		@samplecount varchar(10),
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
		@IsMessageEnabledforThreshold bit 

	declare	
			@thresholdlimit int,
			@blasttime datetime
			

	set @DynamicFromName = ''
	set @DynamicFromEmail = ''	
	set @DynamicReplyToEmail = ''
	set @mailRoute = ''

	select @mailRoute = ConfigValue FROM ecn5_accounts..CustomerConfig where customerID = @CustomerID and configName = 'MailRoute'
	select @testblast = TestBlast, @blasttime = sendtime, @DynamicFromName = DynamicFromName, @DynamicFromEmail = DynamicFromEmail, @DynamicReplyToEmail = DynamicReplyToEmail from blasts where blastID = @BlastID
      
	--@thresholdlimit is the channel's threshold limit, 0 is unlimited
	select @basechannelID = c.basechannelID, @thresholdlimit = EmailThreshold from ecn5_accounts..customer c   join 
			ecn5_accounts..BaseChannel bc on bc.BaseChannelID = c.BaseChannelID
	where customerID = @CustomerID
	
	set @CurrDate = convert(varchar, getdate(), 112)

	if @customerID = 1214 
		set @topcount = '50000'      
	else      
		set @topcount = '100000'
	set @samplecount = @topcount   
   

 	set @SqlString = ''        
	set @EmailString  = ''        
	set @Col1  = ''        
	set @Col2  = ''        
	set @emailcolumns = ''
	set @Filter = ''
	set @IsMessageEnabledforThreshold = 0
	set @blastPriority = 0

	select @Filter = Whereclause from filters where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '

	set @filter = replace(@filter,'[emailaddress]','emailaddress')
	set @filter = replace(@filter,'emailaddress','emails.emailaddress')

        
 	declare @gdf table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int, Reason varchar(25)) 
 	CREATE UNIQUE CLUSTERED INDEX blacklist_ind on  #blacklist(EmailID) with ignore_dup_key
 	
 	create table #SampleSuppression (EmailID int) 
 	CREATE UNIQUE CLUSTERED INDEX SampleSuppression_ind on  #SampleSuppression(EmailID) with ignore_dup_key
 	
 	create table #ThresholdSuppression (EmailAddress varchar(100))  
 	CREATE UNIQUE CLUSTERED INDEX ThresholdSuppression_ind on  #ThresholdSuppression(EmailAddress) with ignore_dup_key
 	
 	if (@BlastID > 0 and @testblast <> 'Y')
 	Begin
 		
 		--@customerProductThreshold > 0 = Threshold feature enabled
 		SELECT 
			@customerProductThreshold = count(p.productname) 
		FROM 
				ecn5_accounts..CustomerProduct cp 
				JOIN ecn5_accounts..ProductDetails pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Products p ON pd.ProductID = p.ProductID 
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
				JOIN ecn5_accounts..ProductDetails pd ON cp.ProductDetailID = pd.ProductDetailID 
				JOIN ecn5_accounts..Products p ON pd.ProductID = p.ProductID 
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
				Blasts b
				JOIN Layouts l ON b.LayoutID = l.LayoutID
				JOIN MessageTypes mt ON l.MessageTypeID = mt.MessageTypeID
		WHERE	
				b.BlastID = @BlastID 
				
				
				
		--new logic from flowchart				
		IF @customerProductThreshold > 0 AND @thresholdlimit > 0
		BEGIN
			--Threshold feature enabled
			
			CREATE TABLE #sentemail (emailaddress VARCHAR(255), totalemail int default 1)		
			
 			CREATE UNIQUE CLUSTERED INDEX sentemail_ind on  #sentemail(emailaddress) with ignore_dup_key
			
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
						FROM	Blasts b   WITH (NOLOCK) 
							JOIN ecn5_accounts..Customer c   WITH (NOLOCK) ON b.CustomerID = c.customerID
							JOIN Layouts l   WITH (NOLOCK) on b.LayoutID = l.LayoutID
							JOIN MessageTypes mt   WITH (NOLOCK) on l.MessageTypeID = mt.MessageTypeID
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
						FROM	Blasts b WITH (NOLOCK) 
							JOIN ecn5_accounts..Customer c  WITH (NOLOCK) ON b.CustomerID = c.customerID
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
				FROM	Blasts b 
					JOIN ecn5_accounts..Customer c ON b.CustomerID = c.customerID
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
 	      
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	
 	create table #tempA (EmailID int)        

	insert into #blacklist        
	SELECT	
			eg.EmailID, 'Master Suppression' 
	FROM	
			EmailGroups eg   WITH (NOLOCK) JOIN 
			Groups g   WITH (NOLOCK) ON eg.groupID = g.groupID        
	WHERE 	
			g.CustomerID = @CustomerID AND 
			g.MasterSupression=1        

	if exists (select SampleBlastID from Sampleblasts where blastID = @BlastID)
	Begin
		select @samplecount = Amount from Sampleblasts where blastID = @BlastID

		insert into #SampleSuppression
		select emailID from EmailActivityLog   WITH (NOLOCK) 
		where (ActionTypeCode = 'send' OR ActionTypeCode = 'suppressed') and blastID = 
			(select blastID from sampleblasts where blastID <> @BlastID and sampleID in (select sampleID from sampleblasts where blastID = @BlastID))
	End


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
		
		select @emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @SupressionList = BlastSuppression, @layoutID = layoutID, @TestBlast = TestBlast from blasts where blastID = @BlastID

		if (@TestBlast <> 'Y' and lower(@ActionType) <> 'softbounce')        
		Begin   
			if (not ((@CustomerID = 1041 and @emailsubject like 'Return Your Textbooks%' and @EmailFrom = 'textbookdept@nebook.com') or @CustomerID= 2808 or @CustomerID=3090 or @CustomerID=3137))
			Begin	
				insert into #blacklist        
 				SELECT  distinct EmailID, '7 Day' 
 				FROM  EmailActivityLog WITH (NOLOCK)       
				WHERE         
					BlastID in (SELECT BlastID FROM Blasts   WITH (NOLOCK) WHERE LayoutID = @LayoutID) AND         
					ActionTypeCode = 'send' AND         
					ActionDate>dateadd(d, -7 , getdate())        
			end
			
		End        
  			
		declare @contentlength int
		set @contentlength = 0
		
		select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join layouts l on c.contentID = l.ContentSlot1 where layoutID = @layoutID

		if @contentlength = 8000
		Begin
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			
			insert into @gdf         
			select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID     
		end
		else
		Begin		
			select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
			(
			 select distinct emailColumns.columnName 
			  from  blasts join         
				Layouts on Blasts.layoutID = Layouts.layoutID left outer join        
				Content on  Content.ContentID = Layouts.ContentSlot1 or         
				   Content.ContentID = Layouts.ContentSlot2 or         
				   Content.ContentID = Layouts.ContentSlot3 or         
				   Content.ContentID = Layouts.ContentSlot4 or         
				   Content.ContentID = Layouts.ContentSlot5 or         
				   Content.ContentID = Layouts.ContentSlot6 or         
				   Content.ContentID = Layouts.ContentSlot7 or         
				   Content.ContentID = Layouts.ContentSlot8 or         
				   Content.ContentID = Layouts.ContentSlot9 left outer join        
				Templates on Layouts.TemplateID  = Templates.TemplateID left outer join        
				contentFilters on contentFilters.contentID = content.contentID or contentFilters.layoutID = blasts.layoutID cross join
				( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			  where         
				Blasts.BlastID = @BlastID and 
				(
					CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentSource) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentText) > 0 or   
					CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentSource) > 0 or   
					CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentText) > 0 or  
					CHARINDEX(emailColumns.columnName, contentFilters.whereclause) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  Templates.TemplateSource) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  Templates.TemplateText) > 0 or
					CHARINDEX(emailColumns.columnName, @Filter) > 0 or
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  @emailsubject) > 0 
				)
			) emailColumns

	       
  			insert into @gdf         
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g join GroupDataFields gdf on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0      
  			union        
  			select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  			from  blasts join         
			Layouts on Blasts.layoutID = Layouts.layoutID left outer join        
			Content on  Content.ContentID = Layouts.ContentSlot1 or         
			   Content.ContentID = Layouts.ContentSlot2 or         
			   Content.ContentID = Layouts.ContentSlot3 or         
			   Content.ContentID = Layouts.ContentSlot4 or         
			   Content.ContentID = Layouts.ContentSlot5 or         
			   Content.ContentID = Layouts.ContentSlot6 or         
			   Content.ContentID = Layouts.ContentSlot7 or         
			   Content.ContentID = Layouts.ContentSlot8 or         
			   Content.ContentID = Layouts.ContentSlot9 left outer join        
			Templates on Layouts.TemplateID  = Templates.TemplateID left outer join        
			contentFilters on contentFilters.contentID = content.contentID or contentFilters.layoutID = blasts.layoutID  join        
			Groups on Groups.GroupID = blasts.groupID and Groups.CustomerID = Blasts.CustomerID join         
			GroupDataFields gdf on gdf.GroupID = Groups.GroupID         
		  where         
			Blasts.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and    
			(        
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentSource) > 0 or   
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentTExt) > 0 or  
				CHARINDEX('%%' +ShortName+ '%%', contentfilters.whereclause) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Templates.TemplateSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Templates.TemplateText) > 0  or
				CHARINDEX('%%' +ShortName+ '%%',  @emailsubject) > 0  or 
				CHARINDEX('%%' +ShortName+ '%%',  @DynamicFromName) > 0 or 
				CHARINDEX('%%' +ShortName+ '%%',  @DynamicFromEmail) > 0 or 
				CHARINDEX('%%' +ShortName+ '%%',  @DynamicReplyToEmail) > 0
			)   
 		end        
	end
	
	if Len(ltrim(rtrim(@SupressionList))) > 0
	Begin
		declare @suppressionGroup TABLE (GroupID int)

		insert into @suppressionGroup 
		select items from fn_Split(@SupressionList, ',')

		insert into #blacklist 
		select distinct emailID, 'Suppression List' from Emailgroups where groupID in (select groupID from @suppressionGroup)
	End

 	if lower(@ActionType) = 'unopen'        
 	Begin        
  		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     									  ') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	               
  	Begin        
    	exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''click'' ') 

         
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' +  @refBlastID  +         
        								') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@BlastID) +         
            								' and e.ActionValue IN (''soft'', ''softbounce'')) '        
  	End  
    else if lower(@ActionType) = 'open'        
 	Begin        
  		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode =''click'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +    
     									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end 
  	else if lower(@ActionType) = 'suppressed'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog Where BlastID in ( ' +  @refBlastID  + ') and ActionTypeCode = ''suppressed'' ) '       
  	End
  	    
  	if not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	Begin
		if @OnlyCounts = 0		 
		Begin			
			if not exists(select filterID from contentFilters where layoutID = @layoutID) OR @BlastID = 0
			Begin
						exec ( 'select top ' + @topcount + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
						' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, case when blist.emailid is not null then 1 else 0 end as InBlackList, ' +        
						'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
						'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
						' into #tmpEmailList1  from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +  
						' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID +         
						' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
						' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
						' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
						' and EmailGroups.GroupID = ' + @GroupID +        
						' and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
						' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID 
						
						insert into EmailActivityLog
						select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''7 day rule'', NULL, ''Y'' 
						from #tmpEmailList1 t join #blacklist blist on t.emailID = blist.emailID
						where InBlackList = 1 and blist.Reason = ''7 Day''
						
						insert into EmailActivityLog
						select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
						from #tmpEmailList1 t 
							join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
						where  InBlackList = 0
						
						select t.* 
						from #tmpEmailList1 t 
							left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
						where	t1.emailaddress is null and
								 InBlackList = 0 and
								t.emailID in (select distinct top ' + @samplecount + ' emailID from #tmpEmailList1) 
						order by t.emailID
						
						drop table #tmpEmailList1
						') 
			end
			else
			Begin
				select @selectslotstr = dbo.fn_DCSelectString(@layoutID) 

					exec ( @selectslotstr + ' into #tmpEmailList2 from ( select top ' +  @topcount + ' Emails.EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
								' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, case when blist.emailid is not null then 1 else 0 end as InBlackList, ' +        
								'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' +  @BlastID + ''' as ConversionTrkCDE, ' +        
								'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
								'  from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +    
								' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  +  @basechannelID  + 
								' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
								' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
								' where sample.emailID is NULL and Emails.CustomerID = ' +  @CustomerID  + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
								' and EmailGroups.GroupID = ' +  @GroupID  +        
								' and cms.emailaddress is null  and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
								' ' + @Filter + ' ' +  @EmailString + ') inn2 order by inn2.EmailID 
								

								insert into EmailActivityLog
								select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''7 day rule'', NULL, ''Y'' 
								from #tmpEmailList2 t join #blacklist blist on t.emailID = blist.emailID
								where InBlackList = 1 and blist.Reason = ''7 Day''
								
								insert into EmailActivityLog
								select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
								from #tmpEmailList2 t 
									join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								where  InBlackList = 0
								
								select t.* 
								from #tmpEmailList2 t 
									left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								where	t1.emailaddress is null and
										 InBlackList = 0 and
										t.emailID in (select distinct top ' + @samplecount + ' emailID from #tmpEmailList2) 
								order by t.emailID
								
								drop table #tmpEmailList2							
								
								')      

			end
		end
		else
		Begin
			
			exec ( 'select count(Emails.EmailID) ' + 
			' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
			' left outer join #ThresholdSuppression ts on emails.emailaddress = ts.emailaddress ' + 
			' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
			' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null and ts.emailaddress is null and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
			' ' + @Filter + ' ' +  @EmailString)  		

		end
		
 	End        
 	Else --if UDF's exists        
  	Begin  
	  insert into #E         
	  select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
	  from EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''
	        
	  select   	@Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),        
	    		@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')        
	  from   @gdf g        
         
	  set @Col1 = substring(@Col1, 0, len(@Col1))         
	  set @Col2 = substring(@Col2 , 0, len(@Col2))        

		if @OnlyCounts = 0 
		Begin
			
			-- not if dynamic blasts
			if not exists(select filterID from contentFilters where layoutID = @layoutID) OR @BlastID = 0 
			Begin
				exec ( ' 
				if exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0)
				Begin
					select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, case when blist.emailid is not null then 1 else 0 end as InBlackList, ' +      
					'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns + ' InnerTable2.* ' +      
					' into #tmpEmailList3 from Emails left outer join ' +       
						 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
							   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID left outer join #SampleSuppression sample on sample.emailID = e.emailID where bl.emailID is null and sample.emailID is NULL) as InnerTable1 Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
						 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
					' #blacklist blist on blist.emailID = Emails.emailID ' +   
					' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
					' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
					' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
					' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString +' order by emails.EmailID;

					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''7 day rule'', NULL, ''Y'' 
					from #tmpEmailList3 t join #blacklist blist on t.emailID = blist.emailID
					where InBlackList = 1 and blist.Reason = ''7 Day''
					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList3 t 
						join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where  InBlackList = 0
					
					select t.* 
					from #tmpEmailList3 t 
						left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null and
							 InBlackList = 0 and
							t.emailID in (select distinct top ' + @samplecount + ' emailID from #tmpEmailList3) 
					order by t.emailID
					
					drop table #tmpEmailList3
				end
				Else
				Begin
					select top ' + @topcount + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, case when blist.emailid is not null then 1 else 0 end as InBlackList, ' +      
					'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns + ' InnerTable2.* ' +      
					'  into #tmpEmailList4  from Emails left outer join ' +       
						 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
							   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID left outer join #SampleSuppression sample on sample.emailID = e.emailID where bl.emailID is null and sample.emailID is NULL) as InnerTable1 ' +         
						  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
						 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
					' #blacklist blist on blist.emailID = Emails.emailID ' + 
					' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
					' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
					' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
					' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString +' order by emails.EmailID;
					
					
					--select ''before suppression'', count(*) from #tmpEmailList4
					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''7 day rule'', NULL, ''Y'' 
					from #tmpEmailList4 t join #blacklist blist on t.emailID = blist.emailID
					where InBlackList = 1 and blist.Reason = ''7 Day''
					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList4 t 
						join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where  InBlackList = 0
					
					select t.* 
					from #tmpEmailList4 t 
						left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null and
							 InBlackList = 0 and
							t.emailID in (select distinct top ' + @samplecount + ' emailID from #tmpEmailList4) 
					order by t.emailID
					
					drop table #tmpEmailList4
				End
				')  
			end
			else
			Begin
				select @selectslotstr = dbo.fn_DCSelectString(@layoutID)
				 
				exec (@selectslotstr + ' into #tmpEmailList5 from ( select top ' + @topcount + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
				'''eid='' + Convert(varchar,Emails.EmailID) +''&bid= '+ @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, case when blist.emailid is not null then 1 else 0 end as InBlackList, ' +      
				'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress,  '''+ @CurrDate + ''' as CurrDate, ' +  @emailcolumns + ' InnerTable2.* ' +      
				'  from Emails  left outer join ' +       
					 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
						   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID left outer join #SampleSuppression sample on sample.emailID = e.emailID where bl.emailID is null and sample.emailID is NULL) as InnerTable1 ' +         
					  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
					 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
				' EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
				' #blacklist blist on blist.emailID = Emails.emailID ' +   
				' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
				' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
				' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
				' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString + ') inn2 order by inn2.EmailID 
				
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''7 day rule'', NULL, ''Y'' 
					from #tmpEmailList5 t join #blacklist blist on t.emailID = blist.emailID
					where InBlackList = 1 and blist.Reason = ''7 Day''
					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList5 t 
						join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where  InBlackList = 0
					
					select t.* 
					from #tmpEmailList5 t 
						left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null and
							 InBlackList = 0 and
							t.emailID in (select distinct top ' + @samplecount + ' emailID from #tmpEmailList5) 
					order by t.emailID
					
					drop table #tmpEmailList5			
				
				')  
			end
		end
		else
		Begin

			exec ( ' select  count(Emails.EmailID) ' +      
			' from Emails  left outer join ' +       
				 '(select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from  ' +         
					   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID left outer join #SampleSuppression sample on sample.emailID = e.emailID where bl.emailID is null and sample.emailID is NULL) as InnerTable1 ' +         
				  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ' +         
				 ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
			' EmailGroups  on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
			' #blacklist blist on blist.emailID = Emails.emailID left outer join ' +   
			' #ThresholdSuppression ts on emails.emailaddress = ts.emailaddress ' +       
			' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' left outer join #SampleSuppression sample on sample.emailID = Emails.emailID ' + 
			' where sample.emailID is NULL and Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and ts.emailaddress is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString )  
			

		end      
  	END        

	drop table #tempA        
	drop table #E        
	drop table #blacklist
	drop table #ThresholdSuppression 
	drop table #SampleSuppression   

END









GO


