CREATE procedure [dbo].[spGetBlastEmailsList]
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

/*
declare @CustomerID int,        	
		@BlastID int,
		@GroupID int,        
		@FilterID int,        
		@BlastID_and_BounceDomain varchar(250),        
		@ActionType varchar(10),        
		@refBlastID varchar(2000),
		@SupressionList varchar(2000),
		@OnlyCounts bit     

set @CustomerID = 1041
set 	@BlastID = 228948
set 	@GroupID = 35894
set 	@FilterID = 19492
set 	@BlastID_and_BounceDomain = ''
set 	@ActionType = ''        
set 	@refBlastID = ''
set 	@SupressionList = ''
set 	@OnlyCounts = 0
*/
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
		@Filter varchar(8000),
		@BasechannelID int,
		@DynamicFromName	varchar(100),       
		@DynamicFromEmail	varchar(100),      
		@DynamicReplyToEmail  varchar(100)

	set @DynamicFromName = ''
	set @DynamicFromEmail = ''	
	set @DynamicReplyToEmail = ''
		
	select @DynamicFromName = DynamicFromName, @DynamicFromEmail = DynamicFromEmail, @DynamicReplyToEmail = DynamicReplyToEmail from ecn5_communicator..[BLAST] where blastID = @BlastID

	select @basechannelID = basechannelID from ecn5_accounts..[CUSTOMER] where customerID = @CustomerID
      
	if @BlastID = 96471 
		set @topcount = '2000'      
	else      
		set @topcount = '100000'   

 	set @SqlString = ''        
	set @EmailString  = ''        
	set @Col1  = ''        
	set @Col2  = ''        
	set @emailcolumns = ''
	set @Filter = ''

	select @Filter = Whereclause from ecn5_communicator..[FILTER] where FilterID = @FilterID

	if @filter <> ''
		set @filter = ' and (' + @filter + ') '

	set @filter = replace(@filter,'[emailaddress]','emailaddress')
	set @filter = replace(@filter,'emailaddress','emails.emailaddress')
        
 	declare @gdf table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int)    
	CREATE UNIQUE CLUSTERED INDEX blacklist_ind on  #blacklist(EmailID) with ignore_dup_key
    
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)   
	CREATE UNIQUE CLUSTERED INDEX E_ind on  #E(EmailID,EntryID,GID) with ignore_dup_key
	     
 	create table #tempA (EmailID int) 
	CREATE UNIQUE CLUSTERED INDEX tempA_ind on  #tempA(EmailID) with ignore_dup_key
	 
 	create table #tempB (EmailID int)      
	CREATE UNIQUE CLUSTERED INDEX tempB_ind on  #tempB(EmailID) with ignore_dup_key       

	insert into #blacklist        
	SELECT eg.EmailID FROM ecn5_communicator..EmailGroups eg JOIN ecn5_communicator..Groups g ON eg.groupID = g.groupID        
	WHERE 	g.CustomerID = @CustomerID AND g.MasterSupression=1        


 	if @BlastID = 0        
 	begin        
		--insert into @gdf         
		--select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID  
		  
		if LEN(@Filter) > 0        
		Begin
			insert into @gdf         
			select distinct gdf.GroupDatafieldsID, gdf.ShortName from ecn5_communicator..Groups g join ecn5_communicator..GroupDataFields gdf on gdf.GroupID = g.GroupID         
			where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0          
		end	
	
 	end        
 	else        
 	begin        
        
		select @emailsubject = EmailSubject, @SupressionList = BlastSuppression from ecn5_communicator..[BLAST] where blastID = @BlastID

  		declare @LayoutID int,        
     			@TestBlast  varchar(5)        
          
  		SELECT @LayoutID = LayoutID, @TestBlast = TestBlast FROM ecn5_communicator..[BLAST] WHERE BlastID = @BlastID        
   
  		if (@TestBlast <> 'Y' and lower(@ActionType) <> 'softbounce')        
  		Begin   
			insert into #blacklist        
     			SELECT  distinct EmailID FROM  ecn5_communicator..EmailActivityLog         
			WHERE         
				BlastID in (SELECT BlastID FROM ecn5_communicator..[BLAST] WHERE LayoutID = @LayoutID) AND         
				ActionTypeCode = 'send' AND         
				ActionDate>dateadd(d, -7 , getdate())        
  		End        

		select @emailcolumns = @emailcolumns + ' emails.' + columnname + ', ' from
		(
		 select distinct emailColumns.columnName 
		  from  ecn5_communicator..[BLAST] join         
		    ecn5_communicator..[LAYOUT] on [BLAST].layoutID = [LAYOUT].layoutID left outer join        
		    ecn5_communicator..Content on  Content.ContentID = [LAYOUT].ContentSlot1 or         
		       Content.ContentID = [LAYOUT].ContentSlot2 or         
		       Content.ContentID = [LAYOUT].ContentSlot3 or         
		       Content.ContentID = [LAYOUT].ContentSlot4 or         
		       Content.ContentID = [LAYOUT].ContentSlot5 or         
		       Content.ContentID = [LAYOUT].ContentSlot6 or         
		       Content.ContentID = [LAYOUT].ContentSlot7 or         
		       Content.ContentID = [LAYOUT].ContentSlot8 or         
		       Content.ContentID = [LAYOUT].ContentSlot9 left outer join        
		    ecn5_communicator..[TEMPLATE] on [LAYOUT].TemplateID  = [TEMPLATE].TemplateID left outer join        
		    ecn5_communicator..[CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID cross join
			( select convert(varchar(100),c.name) as columnName from ecn5_communicator..syscolumns c join ecn5_communicator..sysobjects s on c.id = s.id and s.name = 'emails' where c.name not in ('emailID','customerID','emailaddress')) emailColumns
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
  		select distinct gdf.GroupDatafieldsID, gdf.ShortName from ecn5_communicator..Groups g join ecn5_communicator..GroupDataFields gdf on gdf.GroupID = g.GroupID         
		where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0      
  		union        
  		select distinct gdf.GroupDatafieldsID, gdf.ShortName      
  		from  ecn5_communicator..[BLAST] join         
	    ecn5_communicator..[LAYOUT] on [BLAST].layoutID = [LAYOUT].layoutID left outer join        
	    ecn5_communicator..Content on  Content.ContentID = [LAYOUT].ContentSlot1 or         
	       Content.ContentID = [LAYOUT].ContentSlot2 or         
	       Content.ContentID = [LAYOUT].ContentSlot3 or         
	       Content.ContentID = [LAYOUT].ContentSlot4 or         
	       Content.ContentID = [LAYOUT].ContentSlot5 or         
	       Content.ContentID = [LAYOUT].ContentSlot6 or         
	       Content.ContentID = [LAYOUT].ContentSlot7 or         
	       Content.ContentID = [LAYOUT].ContentSlot8 or         
	       Content.ContentID = [LAYOUT].ContentSlot9 left outer join        
	    ecn5_communicator..[TEMPLATE] on [LAYOUT].TemplateID  = [TEMPLATE].TemplateID left outer join        
	    ecn5_communicator..[CONTENTFILTER] on [CONTENTFILTER].contentID = content.contentID join        
	    ecn5_communicator..Groups on Groups.GroupID = [BLAST].groupID and Groups.CustomerID = [BLAST].CustomerID join         
	    ecn5_communicator..GroupDataFields gdf on gdf.GroupID = Groups.GroupID         
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
			CHARINDEX('%%' +ShortName+ '%%',  @emailsubject) > 0 or 
			CHARINDEX('%%' +ShortName+ '%%',  @DynamicFromName) > 0 or 
			CHARINDEX('%%' +ShortName+ '%%',  @DynamicFromEmail) > 0 or 
			CHARINDEX('%%' +ShortName+ '%%',  @DynamicReplyToEmail) > 0
	    )   
 	end        

	if Len(ltrim(rtrim(@SupressionList))) > 0
	Begin
		declare @suppressionGroup TABLE (GroupID int)

		insert into @suppressionGroup 
		select items from ecn5_communicator..fn_Split(@SupressionList, ',')

		insert into #blacklist 
		select distinct emailID from ecn5_communicator..Emailgroups where groupID in (select groupID from @suppressionGroup)
	End

 	if lower(@ActionType) = 'unopen'        
 	Begin       

		exec('insert into #tempA select EmailID from BlastActivityOpens el where BlastID in (' + @refBlastID +') ') 
		exec('insert into #tempB select EmailID from BlastActivitySends el where BlastID in (' + @refBlastID +') ') 
		
		set @EmailString = @EmailString + ' and Emails.EmailID in (select b.EmailID from #tempB b left outer join #tempA a on b.emailID = a.emailID Where a.emailID is null ) ' 
         
    	--set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' + @refBlastID +        
     --									  ') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	               
  	Begin      

		exec('insert into #tempA select EmailID from BlastActivityClicks el where BlastID in (' + @refBlastID +') ')
		exec('insert into #tempB select EmailID from BlastActivitySends el where BlastID in (' + @refBlastID +') ') 
		
		set @EmailString = @EmailString + ' and Emails.EmailID in (select b.EmailID from #tempB b left outer join #tempA a on b.emailID = a.emailID Where a.emailID is null ) '  

         
   		--set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID in ( ' +  @refBlastID  +         
     --       								') and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from BlastActivityBounces E join BounceCodes bc on e.BounceCodeID = bc.BounceCodeID Where BlastID = ' + convert(varchar(10),@BlastID) +         
            								' and bc.BounceCode IN (''soft'', ''softbounce'')) '        
  	End 
  	-----------------------------------------------------------------------
  	else if lower(@ActionType) = 'open'        
 	Begin        
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivityOpens  where BlastID in (' + @refBlastID +')) '        
  	end    
  	else if lower(@ActionType) = 'click'        
 	Begin        
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivityClicks  where BlastID in (' + @refBlastID +')) '         
  	end 
  	----------------------------------------------------------------------- 
  	else if lower(@ActionType) = 'suppressed'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from BlastActivitySuppressed Where BlastID in ( ' +  @refBlastID  + ') ) '       
  	End  
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from BlastActivitySends with(nolock) WHERE BlastID in (' + @refBlastID + ')) '
	END
    else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from BlastActivitySends with(nolock) WHERE BlastID in (' + @refBlastID + ')) '
	END
        
  	if not exists (select gdf.GroupDatafieldsID from ecn5_communicator..GroupDataFields gdf JOIN ecn5_communicator..Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @gdf)  or (@OnlyCounts = 1 and len(@Filter) = 0)
  	Begin      

		if @OnlyCounts = 0 
		Begin	

			exec ( 'select top ' + @topcount + ' Emails.EmailID, '+@BlastID+' as BlastID, LTRIM(RTRIM(emails.EmailAddress)) AS EmailAddress, CustomerID, ' + @emailcolumns + 
			' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
			'''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
			'''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress ' +        
			' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +	
			' left outer join 	ecn5_communicator..ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null  and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +  
			' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')        
		end
		else
		Begin
			exec ( 'select count(Emails.EmailID) ' + 
			' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
			' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +    
			' left outer join 	ecn5_communicator..ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
			' and EmailGroups.GroupID = ' + @GroupID +        
			' and blist.emailID is null and cms.emailaddress is null and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +   
			' ' + @Filter + ' ' +  @EmailString)        
		
		end
		
 	End        
 	Else --if UDF's exists        
  	Begin        
	  insert into #E         
	  select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID
	  from ecn5_communicator..EmailDataValues join @gdf g on g.GID = EmailDataValues.GroupDataFieldsID  -- and isnull(datavalue,'') <> ''
	        
	  select   	@Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),        
	    		@Col2 = @Col2 + coalesce('Case when E.GID=' + convert(varchar(10),g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')        
	  from   @gdf g        
         
	  set @Col1 = substring(@Col1, 0, len(@Col1))         
	  set @Col2 = substring(@Col2 , 0, len(@Col2))        

		if @OnlyCounts = 0 
		Begin
			exec ( ' 

select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' into #tmpUDF from  ' +         
						   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID where bl.emailID is null) as InnerTable1 ' +         
					  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ;

CREATE NONCLUSTERED  INDEX tmpUDF_ind on #tmpUDF (tmp_EmailID)

select top ' + @topcount + ' Emails.EmailID , ' + @BlastID + ' as BlastID, LTRIM(RTRIM(emails.EmailAddress)) AS EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
			'''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
			'''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ' + @emailcolumns + ' InnerTable2.* ' +      
			' from ecn5_communicator..Emails left outer join #tmpUDF as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
			' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
			' #blacklist blist on blist.emailID = Emails.emailID ' +     
			' left outer join 	ecn5_communicator..ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' where Emails.CustomerID = ' + @CustomerID + ' and cms.emailaddress is null and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString +  ';  drop table #tmpUDF ')    
		end
		else
		Begin
			exec ( ' 

select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' into #tmpUDF from  ' +         
						   '(select e.EmailID, E.EntryID, ' + @Col2 + ' from #E E left outer join #blacklist bl on bl.emailID = e.emailID where bl.emailID is null) as InnerTable1 ' +         
					  ' Group by InnerTable1.EmailID, InnerTable1.EntryID ;
					  
CREATE NONCLUSTERED INDEX tmpUDF_ind on #tmpUDF (tmp_EmailID)

select  count(Emails.EmailID) ' +      
			' from ecn5_communicator..Emails left outer join #tmpUDF as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID join ' +        
			' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID left outer join ' +        
			' #blacklist blist on blist.emailID = Emails.emailID ' + 
			' left outer join 	ecn5_communicator..ChannelMasterSuppressionList cms on emails.emailaddress = cms.emailaddress and cms.basechannelID = '  + @basechannelID + 
			' where Emails.CustomerID = ' + @CustomerID + ' and cms.emailaddress is null and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
			' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString +  ';  drop table #tmpUDF ')    

		end      
  	END        

	drop table #tempA   
	drop table #tempB     
	drop table #E        
	drop table #blacklist        

END
