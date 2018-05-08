CREATE  PROCEDURE [dbo].[MovedToActivity_sp_filteremailscount_with_smartsegment_with_blacklist](
	@GroupID int,        
	@CustomerID int,        
	@FilterID int,        
	@Filter varchar(8000),        
	@BlastID int,        
	@BlastID_and_BounceDomain varchar(250),        
	@ActionType varchar(10),        
	@refBlastID int  )
AS
BEGIN 
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_filteremailscount_with_smartsegment_with_blacklist', GETDATE())	
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
		@EmailString  varchar(8000)

  	set @SqlString = ''        
	set @EmailString  = '' 
        
 	declare @g table(GID int, ShortName varchar(50))        

 	create table #blacklist (EmailID int)        
	create table #tempA (EmailID int)        

	insert into #blacklist        
	SELECT e.EmailID FROM Emails e JOIN EmailGroups eg ON e.emailID = eg.emailID JOIN Groups g ON eg.groupID = g.groupID        
	WHERE 	g.CustomerID = @CustomerID AND g.MasterSupression=1        

 	if @BlastID = 0        
 	begin        
		insert into @g         
		select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID        
 	end        
 	else        
 	begin        
        
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

  		insert into @g         
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
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [CONTENTFILTER].whereclause) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateSource) > 0 or   
			CHARINDEX('%%' +ShortName+ '%%', [TEMPLATE].TemplateText) > 0    
	    )   
 	end        

 	if lower(@ActionType) = 'unopen'        
 	Begin        
  		insert into #tempA        
      	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'open'         
         
    	set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) +        
     									  ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	end         
  	else if lower(@ActionType) = 'unclick'  	-- Add all the other conditions below               
  	Begin        
    	insert into #tempA        
      	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'click'         
         
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) +         
            								' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '        
  	End        
  	else if lower(@ActionType) = 'softbounce'               
  	Begin        
   		set @EmailString = @EmailString +	' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@BlastID) +         
            								' and e.ActionValue IN (''soft'', ''softbounce'')) '        
  	End  
        
  	if not exists (select gdf.GroupDatafieldsID from GroupDataFields gdf JOIN Groups on gdf.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from @g)  
  	Begin        
	    exec ( 'select count(Emails.EmailID) ' +        
	    ' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +         
	    ' left outer join #blacklist blist on blist.emailID = Emails.emailID ' +         
	    ' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +        
	    ' and EmailGroups.GroupID = ' + @GroupID +        
	    ' and blist.emailID is null  and  Emails.emailAddress not like ''@%.%''  and Emails.emailAddress not like ''%@%@%''' +         
	    ' ' + @Filter + ' ' +  @EmailString )        
 	End        
 	Else --if UDF's exists        
  	Begin        
        
		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			@StandAloneUDFIDs VARCHAR(1000),
			@TransactionalUDFIDs VARCHAR(1000)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join @g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)
				
		if LEN(@standaloneUDFs) > 0
		Begin
			set @sColumns = ', SAUDFs.* '
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

			set @tColumns = ', TUDFs.* '
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
		
	  exec ( ' select count(Emails.EmailID) ' +      
	    ' from Emails 
			join ' +         
	    ' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +        
	    ' left outer join #blacklist blist on blist.emailID = Emails.emailID ' + @standAloneQuery + @TransactionalQuery +       
	    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID + ' and Emails.EmailAddress like ''%@%.%''  and Emails.emailAddress not like ''%@%@%''  and  Emails.emailAddress not like ''@%.%'' ' +
		' and EmailGroups.SubscribeTypeCode = ''S'' and blist.emailID is null ' + @Filter + ' ' +  @EmailString )  
      
  	END        

	drop table #tempA       
	drop table #blacklist        

END
