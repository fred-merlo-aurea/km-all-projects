CREATE  PROCEDURE [dbo].[spFilteremails_with_smartsegment_with_blacklist](
	@GroupID int,        
	@CustomerID int,        
	@FilterID int,        
	@Filter varchar(8000),        
	@BlastID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID int)
AS
BEGIN	
	--set @GroupID = 52958
	--set @CustomerID = 3137
	--set @FilterID = 27919;
	--set @Filter = ''
	--set @ActionType = ''
	--set @BlastID = 357587 
	--set @BlastID_and_BounceDomain = ''
	--set @refBlastID = 0    
      
	set NOCOUNT ON        
        
	declare         
		@SqlString  varchar(8000),         
		@EmailString  varchar(8000),    
		@topcount varchar(10),
		@emailcolumns varchar(2000),
		@emailsubject varchar(1000)

      
	if @customerID = 1214 
		set @topcount = '50000'      
	else      
		set @topcount = '100000'   

 	set @SqlString = ''        
	set @EmailString  = ''      
	set @emailcolumns = ''
        
 	declare @g table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int)        
	create table #tempA (EmailID int)        

	insert into #blacklist        
	SELECT e.EmailID FROM ecn5_communicator..Emails e JOIN ecn5_communicator..EmailGroups eg ON e.emailID = eg.emailID JOIN ecn5_communicator..Groups g ON eg.groupID = g.groupID        
	WHERE 	g.CustomerID = @CustomerID AND g.MasterSupression=1        

 	if @BlastID = 0        
 	begin        
		insert into @g         
		select GroupDatafieldsID, ShortName from ecn5_communicator..groupdatafields where GroupDatafields.groupID = @GroupID        
 	end        
 	else        
 	begin        
        
		select @emailsubject = EmailSubject from ecn5_communicator..[BLAST] where blastID = @BlastID

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
				CHARINDEX(emailColumns.columnName, [CONTENTFILTER].whereclause) > 0 or   
				CHARINDEX('%%' +emailColumns.columnName+ '%%',  [TEMPLATE].TemplateSource) > 0 or   
				CHARINDEX('%%' +emailColumns.columnName+ '%%',  [TEMPLATE].TemplateText) > 0 or
				CHARINDEX(emailColumns.columnName, @Filter) > 0 or
				CHARINDEX('%%' +emailColumns.columnName+ '%%',  @emailsubject) > 0 
			)
		) emailColumns

       
  		insert into @g         
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
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [CONTENTFILTER].whereclause) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateText) > 0  or
			CHARINDEX('%%' +ShortName+ '%%',  @emailsubject) > 0    
	    )   
 	end        

 	if lower(@ActionType) = 'unopen'        
 	Begin        
  		insert into #tempA        
      	select distinct EmailID from BlastActivityOpens el where BlastID = @refBlastID       
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) +        
     									  ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	-- Add all the other conditions below               
  	Begin        
    	insert into #tempA        
      	select distinct EmailID from BlastActivityClicks el where BlastID = @refBlastID     
         
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) +         
            								' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from BlastActivityBounces E join BounceCodes bc on e.BounceCodeID = bc.BounceCodeID Where BlastID = ' + convert(varchar(10),@BlastID) +         
            								' and bc.BounceCode IN (''soft'', ''softbounce'')) '        
  	End  
	else if lower(@ActionType) = 'sent'
	BEGIN
		Set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END
	else if lower(@ActionType) = 'not sent'
	BEGIN
		Set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END
        
  	if not exists (select gdf.GroupDatafieldsID from ecn5_communicator..GroupDataFields gdf JOIN ecn5_communicator..Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @g)  
  	Begin        
	    exec ( 'select top ' + @topcount + ' Emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, ' + @emailcolumns + 
		' groupID, FormatTypeCode, SubscribeTypeCode,  emailgroups.CreatedOn, emailgroups.LastChanged, ' +        
	    '''eid='' + Convert(varchar,Emails.EmailID)+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +        
	    '''bounce_''+Convert(varchar,Emails.EmailID)+''-''+''' + @BlastID_and_BounceDomain + ''' as BounceAddress ' +        
	    ' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
	    ' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
	    ' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
	    ' and EmailGroups.GroupID = ' + @GroupID +        
	    ' and blist.emailID is null  and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
	    ' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')        
 	End        
 	Else --if UDF's exists        
  	Begin        
		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			@StandAloneUDFIDs VARCHAR(1000),
			@TransactionalUDFIDs VARCHAR(1000)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM ecn5_communicator..groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM ecn5_communicator..groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM ecn5_communicator..groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM ecn5_communicator..groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
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
							from	ecn5_communicator..EmailDataValues edv join ecn5_communicator..groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
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
			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	ecn5_communicator..EmailDataValues edv  join ecn5_communicator..groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
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

	  exec ( ' select top ' + @topcount + ' Emails.EmailID, ' + @BlastID + ' as BlastID, EmailAddress, Emails.CustomerID, GroupID, FormatTypeCode, SubscribeTypeCode, ' +      
	    '''eid='' + Convert(varchar,Emails.EmailID) +''&bid=' + @BlastID + ''' as ConversionTrkCDE, emailgroups.CreatedOn, emailgroups.LastChanged, ' +      
	    '''bounce_''+ Convert(varchar,Emails.EmailID) +''-''+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ' + @emailcolumns  + @sColumns + @tColumns +      
	    ' from ecn5_communicator..Emails 
			join ' +        
	    ' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +     
	    ' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
	    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
		' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString )  
      
  	END        

	drop table #tempA       
	drop table #blacklist        


END
