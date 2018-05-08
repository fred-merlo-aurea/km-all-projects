CREATE PROCEDURE [dbo].[v_Blast_GetHTMLPreview](
 @BlastID int,    
 @EmailID int)

	--set @BlastID = 304606
	--set @EmailID = 106792516
    
AS
BEGIN  
  
  set NOCOUNT ON    
     
  declare     
   @SqlString  varchar(MAX),     
   @EmailString  varchar(MAX),   
   @groupID int,    
   @CustomerID int ,
   @Filter varchar(MAX),
		@layoutID int,
		@DynamicTagExists bit = 0, 	
  		@DT_startTag varchar(25) = 'ECN.DynamicTag.',		
  		@DT_endTag varchar(25) = '.ECN.DynamicTag',
  		@DynamicTagColumns varchar(MAX) = '',
		@selectslotstr varchar(MAX) = '',
		@DynamicSlotExists bit = 0

   
 set @SqlString = ''    
 set @EmailString  = ''    
 set @Filter = ''
 --remove Filter condtion -- not working in public profile bcoze the UDF value changed after blast has been sent -- 06/16/2010

	--select @Filter = Whereclause from filter where FilterID = (select filterID from blast where blastID = @blastID and StatusCode <> 'Deleted') and IsDeleted = 0


 select  @CustomerID = CustomerID,    
    @GroupID = GroupID,
	@layoutID = LayoutID
 from blast    
 where BlastID = @BlastID and StatusCode <> 'Deleted'
       
 create table #g(    
  GID int,   
  GroupID int,   
  ShortName varchar(50)  
  )    
    
	if exists (select top 1 filterID from contentFilter WITH (NOLOCK) where layoutID = @layoutID and IsDeleted = 0)
			set @DynamicSlotExists = 1

	 declare @allContent table (contentID int)
	 declare @dynamicTags table (DynamicTagID int, Tag varchar(255), contentID int)

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


	declare @contentlength int
	set @contentlength = 0
	

	select @contentlength = DATALENGTH(convert(varchar(8000),contentsource))  from Content c join layout l on c.contentID = l.ContentSlot1 where layoutID = @layoutID and c.IsDeleted = 0 and l.IsDeleted = 0

    if @contentlength = 8000 or @DynamicTagExists = 1 or @DynamicSlotExists = 1
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
	if (@DynamicSlotExists = 1)
	BEGIN
	  SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
		exec( @selectslotstr + @DynamicTagColumns + ' into #tmpEmailList5  from ( select [Emails].EmailID, ' +@BlastID  + ' as BlastID, EmailAddress, [Emails].CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2,     
      City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender,     
    User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date,     
    Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, [EmailGroups].groupID, FormatTypeCode, SubscribeTypeCode,     
    ''eid='' + LTRIM(STR([Emails].EmailID))+ ''&bid=' + @BlastID +''' as ConversionTrkCDE  
	 , bl.EmailFrom as EmailFrom, bl.LayoutID as LayoutID, bl.SendTime as SendTime, bl.FinishTime as FinishTime
    from  [Emails] with(nolock)
	join [EmailGroups] with(nolock) on [EmailGroups].EmailID = [Emails].EmailID     
	join [Blast] bl with(nolock) on (bl.BlastID =  ' +@BlastID + ')

    where     
      [Emails].EmailID = ' + @EmailID + ' and     
      [Emails].CustomerID = ' + @CustomerID + ' and    
	  [Emails].EmailAddress like ''%@%.%'' and         [EmailGroups].GroupID = ' + @GroupID + ') inn2 
	         
			select t.* from #tmpEmailList5 t 
			where t.emailID in (select distinct emailID from #tmpEmailList5 t5) 
			order by t.emailID
	        
			drop table #tmpEmailList5;   
			')
      END
      ELSE
      BEGIN
		  exec('select  [Emails].EmailID, ' +@BlastID  + ' as BlastID, EmailAddress, [Emails].CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2,     
		  City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender,     
		User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date,     
		Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, [EmailGroups].groupID, FormatTypeCode, SubscribeTypeCode,     
		''eid='' + LTRIM(STR([Emails].EmailID))+ ''&bid=' + @BlastID +''' as ConversionTrkCDE  
		 , bl.EmailFrom as EmailFrom, bl.LayoutID as LayoutID, bl.SendTime as SendTime, bl.FinishTime as FinishTime ' +@DynamicTagColumns + ' 
		from  [Emails] with(nolock)
		join [EmailGroups] with(nolock) on [EmailGroups].EmailID = [Emails].EmailID     
		join [Blast] bl with(nolock) on (bl.BlastID =  ' +@BlastID + ')

		where     
		  [Emails].EmailID = ' + @EmailID + ' and     
		  [Emails].CustomerID = ' + @CustomerID + ' and    
		  [Emails].EmailAddress like ''%@%.%'' and         [EmailGroups].GroupID = ' + @GroupID )
      END
        
 End    
 Else --if UDF's exists    
  Begin    
		DECLARE @StandAloneUDFs VARCHAR(MAX),
			@TransactionalUDFs VARCHAR(MAX),
			@StandAloneUDFIDs VARCHAR(MAX),
			@TransactionalUDFIDs VARCHAR(MAX)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 and g.IsDeleted = 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
		declare @sColumns varchar(MAX),
				@tColumns varchar(MAX),
				@standAloneQuery varchar(MAX),
				@TransactionalQuery varchar(MAX)
				
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
     
	 if(@DynamicSlotExists = 1)
	 BEGIN
	 SELECT @selectslotstr = dbo.fn_DCSelectString(@layoutID) 
	 exec ( @selectslotstr + @DynamicTagColumns + 'into #tmpEmailList5 from ( select [Emails].EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, [Emails].CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +    
			 ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +    
			 ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +    
			 ' Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, [EmailGroups].GroupID, FormatTypeCode, SubscribeTypeCode, ' +    
			 '''eid='' + LTRIM(STR([Emails].EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE '  +
			 ' , bl.EmailFrom as EmailFrom, bl.LayoutID as LayoutID, bl.SendTime as SendTime, bl.FinishTime as FinishTime ' 
			 + @sColumns + @tColumns +  
			' from [Emails]	' + 
			' join [Blast] bl (NOLOCK) on (bl.BlastID = '+ @BlastID +') ' +    
			' join [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + @standAloneQuery + @TransactionalQuery +   
		' where  [Emails].EmailID = ' + @EmailID + ' and [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%''' +    
		' and [EmailGroups].GroupID = ' + @GroupID + ' ' + @Filter + ' ) inn2 order by inn2.EmailID
                        
                        select t.* from #tmpEmailList5 t 
                        where t.emailID in (select distinct emailID from #tmpEmailList5 t5) 
                        order by t.emailID
                        
                        drop table #tmpEmailList5;                      
                        
                        ')  
	 END
	 ELSE
	 BEGIN
    
	  exec (' select [Emails].EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, [Emails].CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +    
			 ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +    
			 ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +    
			 ' Convert(varchar,Notes) as Notes, [Emails].DateAdded, [Emails].DateUpdated, CreatedOn, LastChanged, [EmailGroups].GroupID, FormatTypeCode, SubscribeTypeCode, ' +    
			 '''eid='' + LTRIM(STR([Emails].EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE '  +
			 ' , bl.EmailFrom as EmailFrom, bl.LayoutID as LayoutID, bl.SendTime as SendTime, bl.FinishTime as FinishTime ' 
			 + @sColumns + @tColumns + @DynamicTagColumns + 
			' from [Emails]	' + 
			' join [Blast] bl (NOLOCK) on (bl.BlastID = '+ @BlastID +') ' +    
			' join [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + @standAloneQuery + @TransactionalQuery +   
		' where  [Emails].EmailID = ' + @EmailID + ' and [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%''' +    
		' and [EmailGroups].GroupID = ' + @GroupID + ' ' + @Filter + ' order by [Emails].EmailID ')    
	END
 END    
    
 drop table #G    
   
 END