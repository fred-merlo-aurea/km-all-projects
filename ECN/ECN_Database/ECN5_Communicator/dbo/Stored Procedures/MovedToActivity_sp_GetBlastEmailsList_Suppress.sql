﻿CREATE  PROCEDURE [dbo].[MovedToActivity_sp_GetBlastEmailsList_Suppress]( 
	@CustomerID int,        	
	@BlastID int,
	@GroupID int,        
	@FilterID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID varchar(2000),
	@SupressionList varchar(2000),
	@OnlyCounts bit   )     
AS
BEGIN
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_GetBlastEmailsList_Suppress', GETDATE())
	--set @GroupID = 49195
	--set @CustomerID = 1
	--set @FilterID = 30954
	--set @BlastID = 448175
	--set @BlastID_and_BounceDomain = '';
	--set @ActionType = ''
	--set @refBlastID = '0'
	--set @OnlyCounts = 0
	--set @SupressionList = ''
	
	
     
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),   
		@topcount varchar(10),
		@emailcolumns varchar(2000),
		@emailsubject varchar(1000),
		@Filter varchar(8000),
		@BasechannelID int

	select @basechannelID = basechannelID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where customerID = @CustomerID
      
	if @BlastID = 96471 
		set @topcount = '2000'      
	else      
		set @topcount = '100000'   

 	set @SqlString = ''        
	set @EmailString  = ''      
	set @emailcolumns = ''
	set @Filter = ''

	select @Filter = Whereclause from [FILTER] where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '
        
 	declare @gdf table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int)        
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        
 	create table #tempA (EmailID int)        

	insert into #blacklist        
	SELECT eg.EmailID FROM EmailGroups eg JOIN Groups g ON eg.groupID = g.groupID        
	WHERE 	g.CustomerID = @CustomerID AND g.MasterSupression=1        


 	if @BlastID = 0        
 	begin        
		insert into @gdf         
		select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID        
 	end        
 	else        
 	begin        
        
		select @emailsubject = EmailSubject, @SupressionList = BlastSuppression from [BLAST] where blastID = @BlastID

  		declare @LayoutID int,        
     			@TestBlast  varchar(5)        
          
  		SELECT @LayoutID = LayoutID, @TestBlast = TestBlast FROM [BLAST] WHERE BlastID = @BlastID        
   
  		if (@TestBlast <> 'Y' and lower(@ActionType) <> 'softbounce')        
  		Begin   
			insert into #blacklist        
     			SELECT  distinct EmailID FROM  EmailActivityLog         
			WHERE         
				BlastID in (SELECT BlastID FROM [BLAST] WHERE LayoutID = @LayoutID) AND         
				ActionTypeCode = 'send' AND         
				ActionDate>dateadd(d, -7 , getdate())        
  		End        

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
		    [CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID cross join
			( select convert(varchar(100),c.name) as columnName from syscolumns c join sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
		  where         
		    [BLAST].BlastID = @BlastID and 
			(
				CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentSource) > 0 or   
				CHARINDEX('%%' +emailColumns.columnName+ '%%', Content.ContentText) > 0 or   
				CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentSource) > 0 or   
				CHARINDEX('$$' +emailColumns.columnName+ '$$', Content.ContentText) > 0 or  
				CHARINDEX('##' +emailColumns.columnName+ '##', Content.ContentSource) > 0 or   
				CHARINDEX('##' +emailColumns.columnName+ '##', Content.ContentText) > 0 or  
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
	    [CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID join        
	    Groups on Groups.GroupID = [BLAST].groupID and Groups.CustomerID = [BLAST].CustomerID join         
	    GroupDataFields gdf on gdf.GroupID = Groups.GroupID         
	  where         
	    [BLAST].BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and    
	    (     
			CHARINDEX('sort="' +ShortName+ '"', Content.ContentSource) > 0 or           
			CHARINDEX('filter_field="' +ShortName+ '"', Content.ContentSource) > 0 or     
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
			CHARINDEX('$$' +ShortName+ '$$', Content.ContentSource) > 0 or   
			CHARINDEX('$$' +ShortName+ '$$', Content.ContentTExt) > 0 or  
			CHARINDEX('##' +ShortName+ '##', Content.ContentSource) > 0 or   
			CHARINDEX('##' +ShortName+ '##', Content.ContentText) > 0 or 
			CHARINDEX('%%' +ShortName+ '%%', [CONTENTFILTER].whereclause) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateText) > 0  or
			CHARINDEX('%%' +ShortName+ '%%',  @emailsubject) > 0    
	    )   
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
      
	if (@basechannelID 	= 16 and @onlyCounts = 0 and @BlastID > 0)
	Begin
		if not exists (select top 1 * from BlastSuppressedEmails where blastID = @blastID)
		Begin
			insert into BlastSuppressedEmails 
			select @blastID, b.EmailID, getdate() 
				FROM #blacklist b JOIN EmailGroups eg ON eg.EmailID = b.EmailID 
				WHERE eg.GroupID = @GroupID 
		end
	End
  
  	if not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	Begin      

		if @OnlyCounts = 0 
		Begin	
			exec ( 'select top ' + @topcount + ' Emails.EmailID, '''+@BlastID+''' as BlastID, LTRIM(RTRIM(EmailAddress)) AS EmailAddress, CustomerID, ' + @emailcolumns + 
			' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
			'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
			'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress ' +        
			' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null  and Emails.emailaddress not in (select emailaddress from ChannelMasterSuppressionList where basechannelID = ' + @basechannelID + ') and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' + 

 
       
			' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')        
		end
		else
		Begin
			--select 'start', getdate()

			exec ( 'select count(Emails.EmailID) ' + 
			' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null and Emails.emailaddress not in (select emailaddress from ChannelMasterSuppressionList where basechannelID = ' + @basechannelID + ') and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +   


      
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
			print ( ' select top ' + convert(varchar(10),@topcount) + ' Emails.EmailID, ' + convert(varchar(10),@BlastID) + ' as BlastID, LTRIM(RTRIM(EmailAddress)) AS EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
			'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + convert(varchar(10),@BlastID) + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
			'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ' + @emailcolumns + @sColumns + @tColumns +      
			' from Emails join ' +       
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +     
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +     
			' where Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and Emails.emailaddress not in (select emailaddress from ChannelMasterSuppressionList where basechannelID = ' + convert(varchar(10),@basechannelID) + ') and EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) + ' and Emails.EmailAddress like 

''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString ) 
			exec ( ' select top ' + @topcount + ' Emails.EmailID, ' + @BlastID + ' as BlastID, LTRIM(RTRIM(EmailAddress)) AS EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
			'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
			'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ' + @emailcolumns + @sColumns + @tColumns +      
			' from Emails join ' +       
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +     
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +     
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.emailaddress not in (select emailaddress from ChannelMasterSuppressionList where basechannelID = ' + @basechannelID + ') and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like 

''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString )  
		end
		else
		Begin
			exec ( ' select  count(Emails.EmailID) ' +      
			' from Emails join ' +       
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +        
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.emailaddress not in (select emailaddress from ChannelMasterSuppressionList where basechannelID = ' + @basechannelID + ') and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like 

''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString )  

		end      
  	END        

	drop table #tempA      
	drop table #blacklist        
	drop table #E
END
