CREATE  PROCEDURE [dbo].[spGetBlastEmailsListUsingThresholdAndPriority](
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID varchar(2000),
	@SupressionList varchar(2000),
	@OnlyCounts bit  )
AS
BEGIN      

	--set		@CustomerID = 3137
	--set 	@BlastID = 357587
	--set 	@GroupID = 52958
	--set 	@FilterID = '0'
	--set 	@BlastID_and_BounceDomain = ''
	--set 	@ActionType = ''        
	--set 	@refBlastID = ''
	--set 	@SupressionList = ''
	--set 	@OnlyCounts = 0   

	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),    
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
		@blastpriority int  

	declare	
			@thresholdlimit int,
			@blasttime datetime,
			@Domain varchar(100)
			

	set @DynamicFromName = ''
	set @DynamicFromEmail = ''	
	set @DynamicReplyToEmail = ''
	set @mailRoute = ''

	select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)) from [BLAST] where BlastID = @BlastID
	select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and DomainName = @Domain
	if(@MailRoute is null)
	Begin
		select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and IsDefault = 'true'
	End
	--select @mailRoute = ConfigValue FROM [ECN5_ACCOUNTS].[DBO].CustomerConfig where customerID = @CustomerID and configName = 'MailRoute'
	select @testblast = TestBlast, @blasttime = sendtime, @DynamicFromName = DynamicFromName, @DynamicFromEmail = DynamicFromEmail, @DynamicReplyToEmail = DynamicReplyToEmail from [BLAST] where blastID = @BlastID
      
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
	set @emailcolumns = ''
	set @Filter = ''

	select @Filter = Whereclause from [FILTER] where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '

	set @filter = replace(@filter,'[emailaddress]','emailaddress')
	set @filter = replace(@filter,'emailaddress','emails.emailaddress')

        
 	declare @gdf table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int)  
 	CREATE UNIQUE CLUSTERED INDEX blacklist_ind on  #blacklist(EmailID) with ignore_dup_key
 	
 	create table #ThresholdSuppression (EmailAddress varchar(100))  
 	CREATE UNIQUE CLUSTERED INDEX ThresholdSuppression_ind on  #ThresholdSuppression(EmailAddress) with ignore_dup_key
 	
 	if (@BlastID > 0 and @testblast <> 'Y')
 	Begin
 		
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
				
		SELECT	
				@blastPriority = mt.SortOrder
		FROM	
				[BLAST] b
				JOIN [LAYOUT] l ON b.LayoutID = l.LayoutID
				JOIN [MESSAGETYPE] mt ON l.MessageTypeID = mt.MessageTypeID
		WHERE	
				b.BlastID = @BlastID AND
				mt.Priority = 1
				
		IF @customerproductthreshold > 0 AND @thresholdlimit IS NOT NULL AND @thresholdlimit > 0
			BEGIN
 			CREATE TABLE #sentemail (emailaddress VARCHAR(255), totalemail int default 1)
			 		
 			insert into #sentEmail
 			exec spGetThresholdSuppressedEmails @BlastID
	 		
	 		--select 'sentEmail suppressed',getdate(), COUNT(*) from #sentEmail
	 			 				
			DECLARE @cursorBlastID int			
			
	 		IF @blastpriority IS NULL OR @blastpriority = 0
	 		BEGIN
				DECLARE db_cursor CURSOR FOR			
				SELECT  b.BlastID 
				FROM	[BLAST] b 
					JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c ON b.CustomerID = c.customerID
				WHERE 
					c.BaseChannelID = @basechannelID AND
					b.TestBlast = 'N' AND
					(
						(b.StatusCode='active') or
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
			ELSE
			BEGIN
				DECLARE db_cursor CURSOR FOR			
				SELECT  b.BlastID 
				FROM	[BLAST] b 
					JOIN [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  c ON b.CustomerID = c.customerID
					JOIN [LAYOUT] l on b.LayoutID = l.LayoutID
					JOIN [MESSAGETYPE] mt on l.MessageTypeID = mt.MessageTypeID
				WHERE 
					c.BaseChannelID = @basechannelID AND
					b.TestBlast = 'N' AND
					mt.Priority = 1 AND
					(
						(b.StatusCode='active') or
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
			
			
	 		--select 'Active/Pending Email suppressed : ', getdate(),COUNT(*) from #sentEmail
			
			insert into #ThresholdSuppression
			SELECT  emailaddress --, SUM(totalemail)
			FROM #sentemail
			GROUP BY emailaddress	
			HAVING SUM(totalemail) >= @thresholdlimit -- get from [BASECHANNEL]
		
			--select 'ThresholdSuppression : ',getdate(), COUNT(*) from #ThresholdSuppression				
			
			DROP TABLE #sentemail
		END
		
	End	 	
 	      
 	create table #tempA (EmailID int)        

	insert into #blacklist        
	SELECT eg.EmailID FROM EmailGroups eg JOIN Groups g ON eg.groupID = g.groupID        
	WHERE 	g.CustomerID = @CustomerID AND g.MasterSupression=1        

	if exists (select SampleBlastID from SampleBlasts where blastID = @BlastID)
	Begin
		select @topcount = Amount from SampleBlasts where blastID = @BlastID

		insert into #blacklist
		select emailID from emailactivitylog where actiontypecode = 'send' and blastID = 
			(select blastID from SampleBlasts where blastID <> @BlastID and sampleID in (select sampleID from SampleBlasts where blastID = @BlastID))
	End


 	if @BlastID = 0 
 	begin        
		--insert into @gdf         
		--select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID        
		
		if LEN(@Filter) > 0        
		Begin
			insert into @gdf         
			select distinct gdf.GroupDatafieldsID, gdf.ShortName from Groups g join GroupDataFields gdf on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0          
		end	
 	end        
 	else        
 	begin        
		
		select @emailsubject = EmailSubject, @EmailFrom = EmailFrom,  @SupressionList = BlastSuppression, @layoutID = layoutID, @TestBlast = TestBlast from [BLAST] where blastID = @BlastID

		if (@TestBlast <> 'Y' and lower(@ActionType) <> 'softbounce')        
		Begin   
			if (not ((@CustomerID = 1041 and @emailsubject like 'Return Your Textbooks%' and @EmailFrom = 'textbookdept@nebook.com') or @CustomerID= 2808 or @CustomerID=3090))
			Begin	
				insert into #blacklist        
 					SELECT  distinct EmailID FROM  EmailActivityLog         
				WHERE         
					BlastID in (SELECT BlastID FROM [BLAST] WHERE LayoutID = @LayoutID) AND         
					ActionTypeCode = 'send' AND         
					ActionDate>dateadd(d, -7 , getdate())        
			end		
			--else
			--Begin
			--	insert into #blacklist        
 		--			SELECT  distinct EmailID FROM  EmailActivityLog         
			--	WHERE         
			--		BlastID in (SELECT BlastID FROM [BLAST] WHERE LayoutID = @LayoutID) AND         
			--		ActionTypeCode = 'send' AND         
			--		Convert(varchar(10), ActionDate,101)= Convert(varchar(10), getdate(),101)
			--End
			
		End        
  			
		declare @contentlength int
		set @contentlength = 0
		
		select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join [LAYOUT] l on c.contentID = l.ContentSlot1 where layoutID = @layoutID

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
				( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
			  where         
				[BLAST].BlastID = @BlastID and 
				(
					CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentSource) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentText) > 0 or   
					CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentSource) > 0 or   
					CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentText) > 0 or  
					CHARINDEX(emailColumns.columnName, [CONTENTFILTER].whereclause) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  [TEMPLATE].TemplateSource) > 0 or   
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  [TEMPLATE].TemplateText) > 0 or
					CHARINDEX(emailColumns.columnName, @Filter) > 0 or
					CHARINDEX('%%' +emailColumns.columnName+ '%%',  @emailsubject) > 0 
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
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentSource) > 0 or   
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentTExt) > 0 or  
				CHARINDEX('%%' +ShortName+ '%%', [CONTENTFILTER].whereclause) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateText) > 0  or
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
		select distinct emailID from Emailgroups where groupID in (select groupID from @suppressionGroup)
	End

 	if lower(@ActionType) = 'unopen'        
 	Begin        
  		--insert into #tempA        
      	--select distinct EmailID from EmailActivityLog el where BlastID in (select items from dbo.fn_split(@refBlastID,',')) and ActionTypeCode = 'open'         

		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     									  ') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	               
  	Begin        
    	--insert into #tempA        
      	--select distinct EmailID from EmailActivityLog el where BlastID in (select items from dbo.fn_split(@refBlastID,',')) and ActionTypeCode = 'click'         

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
  		--insert into #tempA        
      	--select distinct EmailID from EmailActivityLog el where BlastID in (select items from dbo.fn_split(@refBlastID,',')) and ActionTypeCode = 'open'         

		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode = ''open'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		--insert into #tempA        
      	--select distinct EmailID from EmailActivityLog el where BlastID in (select items from dbo.fn_split(@refBlastID,',')) and ActionTypeCode = 'open'         

		exec('insert into #tempA select EmailID from EmailActivityLog el where BlastID in (' + @refBlastID +') and ActionTypeCode =''click'' ') 
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     									  ') and ActionTypeCode = ''send'' and exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end 
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from EmailActivityLog WHERE BlastID in (' + @refBlastID + ') and ActionTypeCode = ''send'') '
	END	
  	else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from EmailActivityLog WHERE BlastID in (' +@refBlastID + ') and ActionTypeCode = ''send'') '
	END	
  	    
  	if not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	Begin
		if @OnlyCounts = 0		 
		Begin			
			if not exists(select filterID from [CONTENTFILTER] where layoutID = @layoutID) OR @BlastID = 0
			Begin
						exec ( 'select top ' + @topcount + ' Emails.EmailID,'''+@BlastID+''' as BlastID,'''+@mailRoute+''' as mailRoute, Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
						' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
						'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
						'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
						' into #tmpEmailList1  from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
						' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
						' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
						' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
						' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
						' and EmailGroups.GroupID = ' + @GroupID +        
						' and blist.emailID is null and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
						' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID 
						
								insert into EmailActivityLog
								select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
								from #tmpEmailList1 t join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								
								select * from #tmpEmailList1 t left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								where	t1.emailaddress is null and
										t.emailID in (select distinct top ' + @topcount + ' emailID from #tmpEmailList1) 
								order by t.emailID

								drop table #tmpEmailList1
						') 	
			end
			else
			Begin
				select @selectslotstr = dbo.fn_DCSelectString(@layoutID) 

					exec ( @selectslotstr + ' into #tmpEmailList2 from ( select top ' +  @topcount + ' Emails.EmailID,'''+ @BlastID +''' as BlastID, '''+@mailRoute+''' as mailRoute,  Emails.EmailAddress, CustomerID, ' + @emailcolumns + 
								' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
								'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' +  @BlastID + ''' as ConversionTrkCDE, ' +        
								'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate ' +        
								'  from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +    
								' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  +  @basechannelID  + 
								' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' +
								' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
								' where Emails.CustomerID = ' +  @CustomerID  + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
								' and EmailGroups.GroupID = ' +  @GroupID  +        
								' and blist.emailID is null and cms.emailaddress is null  and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
								' ' + @Filter + ' ' +  @EmailString + ') inn2 order by inn2.EmailID 
								

								insert into EmailActivityLog
								select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
								from #tmpEmailList2 t join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								
								select * from #tmpEmailList2 t left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
								where	t1.emailaddress is null and
										t.emailID in (select distinct top ' + @topcount + ' emailID from #tmpEmailList2) 
								order by t.emailID

								drop table #tmpEmailList2								
								
								')      

			end
		end
		else
		Begin
			--select 'start', getdate()
  	     
			exec ( 'select count(Emails.EmailID) ' + 
			' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + 
			' left outer join #ThresholdSuppression ts on emails.emailaddress = ts.emailaddress ' + 
			' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null and ts.emailaddress is null and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
			' ' + @Filter + ' ' +  @EmailString)    
		
			--select 'end', getdate()

		end
		
 	End        
 	Else --if UDF's exists        
  	Begin  
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
							from	EmailDataValues edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ')
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @StandAloneUDFs + ')) as pvt 			
					) 
				SAUDFs on Emails.emailID = SAUDFs.tmp_EmailID'
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
							from	EmailDataValues edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ')
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End   

		if @OnlyCounts = 0 
		Begin
			
			-- not if dynamic blasts
			if not exists(select filterID from [CONTENTFILTER] where layoutID = @layoutID) OR @BlastID = 0 
			Begin
				print '1'
				exec ( ' 
				if exists (select top 1 groupdatafieldsID from groupdatafields where groupID = ' + @GroupID + ' and isnull(datafieldsetID,0) > 0)
				Begin
					select Emails.EmailID,' + @BlastID + ' as BlastID, Emails.EmailAddress, '''+@mailRoute+''' as mailRoute, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
					'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns + @sColumns + @tColumns +     
					' into #tmpEmailList3 from Emails join ' +       
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID ' +        
					' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + @standAloneQuery + @TransactionalQuery +  
					' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
					' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
					' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
					' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString +' order by emails.EmailID;

					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList3 t join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					
					select * from #tmpEmailList3 t left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null and
							t.emailID in (select distinct top ' + @topcount + ' emailID from #tmpEmailList3 t3) 
					order by t.emailID

					drop table #tmpEmailList3
				end
				Else
				Begin
					select top ' + @topcount + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
					'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
					'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, '''+ @CurrDate + ''' as CurrDate, ' + @emailcolumns + @sColumns + @tColumns +  
					'  into #tmpEmailList4  from Emails join ' +       
					' EmailGroups  on EmailGroups.EmailID = Emails.EmailID ' +        
					' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + @standAloneQuery + @TransactionalQuery +  
					' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
					' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
					' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
					' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString +' order by emails.EmailID;
										
					select ''before suppression'', count(*) from #tmpEmailList4
					
					insert into EmailActivityLog
					select distinct t.EmailID, ' + @BlastID + ', ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList4 t join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					
					select * from #tmpEmailList4 t left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null 
					order by t.emailID

					drop table #tmpEmailList4
				End
				')  
			end
			else
			Begin
				select @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
				
				exec (@selectslotstr + ' into #tmpEmailList5 from ( select top ' + @topcount + ' Emails.EmailID,' + @BlastID + ' as BlastID, '''+@mailRoute+''' as mailRoute, Emails.EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
				'''eid='' + Convert(varchar,Emails.EmailID) +''&bid= '+ @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
				'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress,  '''+ @CurrDate + ''' as CurrDate, ' +  @emailcolumns + @sColumns + @tColumns + 
				'  from Emails join ' +       
				' EmailGroups  on EmailGroups.EmailID = Emails.EmailID ' +        
				' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + @standAloneQuery + @TransactionalQuery +  
				' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
				' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
				' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
				' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString + ') inn2 order by inn2.EmailID 
				
					insert into EmailActivityLog
					select distinct t.EmailID,' + @BlastID + ',  ''suppressed'', GETDATE(), ''threshold'', NULL, ''Y'' 
					from #tmpEmailList5 t join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					
					select * from #tmpEmailList5 t left outer join #ThresholdSuppression t1 on t.emailaddress = t1.emailaddress
					where	t1.emailaddress is null 
					order by t.emailID

					drop table #tmpEmailList5				
				
				')  
			end
		end
		else
		Begin

			exec ( ' select  count(Emails.EmailID) ' +      
			' from Emails  join ' +       
			' EmailGroups  on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID left outer join ' +   
			' #ThresholdSuppression ts on emails.emailaddress = ts.emailaddress ' +       
			' left outer join 	ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' left outer join 	GlobalMasterSuppressionList gms on emails.emailaddress = gms.emailaddress ' + 
			' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null and ts.emailaddress is null and cms.emailaddress is null ' + @Filter + ' ' +  @EmailString )  
			

		end      
  	END        

	drop table #tempA        
	drop table #blacklist
	drop table #ThresholdSuppression    

END
