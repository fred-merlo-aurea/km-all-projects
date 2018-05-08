CREATE PROCEDURE [dbo].[sp_HTMLPreview](
 @BlastID int,    
 @EmailID int)

	--set @BlastID = 304606
	--set @EmailID = 106792516
    
AS
BEGIN  
  
  set NOCOUNT ON    
     
  declare     
   @SqlString  varchar(8000),     
   @EmailString  varchar(8000),   
   @groupID int,    
   @CustomerID int ,
   @Filter varchar(8000),
		@layoutID int

   
 set @SqlString = ''    
 set @EmailString  = ''    
 set @Filter = ''
 --remove Filter condtion -- not working in public profile bcoze the UDF value changed after blast has been sent -- 06/16/2010

	select @Filter = Whereclause from filter where FilterID = (select filterID from blast where blastID = @blastID and StatusCode <> 'Deleted') and IsDeleted = 0

	if @filter <> ''
	Begin
		set @filter = ' and (' + @filter + ') '

		set @filter = replace(@filter,'[emailaddress]','emailaddress')
		set @filter = replace(@filter,'emailaddress','[Emails].emailaddress')
	end

 select  @CustomerID = CustomerID,    
    @GroupID = GroupID    
 from blast    
 where BlastID = @BlastID and StatusCode <> 'Deleted'
       
 create table #g(    
  GID int,   
  GroupID int,   
  ShortName varchar(50)  
  )    
    

    
	declare @contentlength int
	set @contentlength = 0
	
	select @layoutID = layoutID  from blast where blastID = @BlastID and StatusCode <> 'Deleted'
	select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join layout l on c.contentID = l.ContentSlot1 where layoutID = @layoutID and c.IsDeleted = 0 and l.IsDeleted = 0

    if @contentlength = 8000
		Begin
			insert into #g         
			select GroupDatafieldsID, @GroupID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID  and IsDeleted = 0   
		end
		else
		Begin
			 insert into #g     
			 select distinct gdf.GroupDatafieldsID, @GroupID, gdf.ShortName from Groups g join GroupDataFields gdf on gdf.GroupID = g.GroupID         
						where  g.groupID = @GroupID and g.customerID = @CustomerID and CHARINDEX(ShortName, @Filter) > 0   and gdf.IsDeleted = 0   
			 union  
			 select  distinct gdf.GroupDatafieldsID, @GroupID, gdf.ShortName
			 from  Blast join     
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
			   Template on Layout.TemplateID  = Template.TemplateID and Template.IsDeleted = 0 left outer join    
			   ContentFilter on ContentFilter.contentID = content.contentID and ContentFilter.IsDeleted = 0 join    
			   Groups on Groups.GroupID = Blast.groupID and Groups.CustomerID = Blast.CustomerID join     
			   GroupDataFields gdf on gdf.GroupID = Groups.GroupID     
			 where     
			   Blast.BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and 
			   Blast.StatusCode <> 'Deleted' and Layout.IsDeleted = 0 and Content.IsDeleted = 0 and gdf.IsDeleted = 0 and   
			   (   
			    CHARINDEX('sort="' +ShortName+ '"', Content.ContentSource) > 0 or           
			    CHARINDEX('filter_field="' +ShortName+ '"', Content.ContentSource) > 0 or  
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Content.ContentTExt) > 0 or   
				CHARINDEX('##' +ShortName+ '##', Content.ContentSource) > 0 or 
				CHARINDEX('##' +ShortName+ '##', Content.ContentTExt) > 0 or   	 
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentSource) > 0 or   
				CHARINDEX('$$' +ShortName+ '$$', Content.ContentTExt) > 0 or  
				CHARINDEX('%%' +ShortName+ '%%', ContentFilter.whereclause) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Template.TemplateSource) > 0 or   
				CHARINDEX('%%' +ShortName+ '%%', Template.TemplateText) > 0  
			   )    
		End	    
    
  if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID and GroupDataFields.IsDeleted = 0) or not exists(select * from #g)    
  Begin    
    select  [Emails].EmailID, @BlastID as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2,     
      City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender,     
    User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date,     
    Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode,     
    'eid=' + LTRIM(STR([Emails].EmailID))+ '&bid=' + convert(varchar(10),@BlastID)  as ConversionTrkCDE    
    from  [Emails] join [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID     
    where     
      [Emails].EmailID = @EmailID and     
      [Emails].CustomerID = @CustomerID and    
      [Emails].EmailAddress like '%@%.%' and     
      [EmailGroups].GroupID = @GroupID  
         
 End    
 Else --if UDF's exists    
  Begin    
		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			@StandAloneUDFIDs VARCHAR(2000),
			@TransactionalUDFIDs VARCHAR(2000)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
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
							from	[EMAILDATAVALUES] edv join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ') and gdf.IsDeleted = 0 and edv.emailID = ' + convert(varchar(10),@EmailID) + '
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

			set @tColumns = ', TUDFs.* '
			set @TransactionalQuery= '  left outer join
					(
						SELECT *
						 FROM
						 (
							SELECT edv.emailID as tmp_EmailID1, edv.entryID, gdf.ShortName, edv.DataValue
							from	[EMAILDATAVALUES] edv  join groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ') and gdf.IsDeleted = 0 and edv.emailID = ' + convert(varchar(10),@EmailID) + '
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on [Emails].emailID = TUDFs.tmp_EmailID1 '
		End
     
    
  exec (' select [Emails].EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +    
         ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +    
         ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +    
         ' Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +    
         '''eid='' + LTRIM(STR([Emails].EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE '  + @sColumns + @tColumns +   
        ' from [Emails] 
			join ' +    
    ' [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + @standAloneQuery + @TransactionalQuery +   
    ' where  [Emails].EmailID = ' + @EmailID + ' and [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%''' +    
    ' and [EmailGroups].GroupID = ' + @GroupID + ' ' + @Filter + ' order by [Emails].EmailID ')    

    
 END    
    
 drop table #G    
   
 END

