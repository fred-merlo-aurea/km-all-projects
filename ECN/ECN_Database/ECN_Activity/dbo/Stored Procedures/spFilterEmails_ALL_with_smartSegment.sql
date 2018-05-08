CREATE PROCEDURE [dbo].[spFilterEmails_ALL_with_smartSegment](  
 @GroupID int,  
 @CustomerID int,  
 @FilterID int,  
 @Filter varchar(8000),  
 @BlastID int,  
 @BlastID_and_BounceDomain varchar(250),  
 @ActionType varchar(10),  
 @refBlastID int,  
 @EmailID int = 0)  
AS  
BEGIN    
  --set @GroupID = 93815  
  --set @CustomerID = 1  
  --set @FilterID = 0;  
  --set @Filter = ''  
  --set @ActionType = ''  
  --set @BlastID = 433151   
  --set @BlastID_and_BounceDomain = ''  
  --set @refBlastID = 0  
   
  set NOCOUNT ON  
   
 if @EmailID is null  
  set @EmailID = 0  
   
  declare   
   @SqlString  varchar(8000),   
   @EmailString  varchar(8000),  
   @MailRoute varchar(255),  
   @Domain varchar(100) ,
   @DynamicTagExists bit = 0, 	
  		@DT_startTag varchar(25) = 'ECN.DynamicTag.',		
  		@DT_endTag varchar(25) = '.ECN.DynamicTag',
  		@DynamicTagColumns varchar(MAX) = '',
  		@layoutid int = 0,
  	@DynamicSlotExists bit = 0,
  	@selectslotstr varchar(8000) = ''
  
 set @SqlString = ''  
 set @EmailString  = ''  
     
 create table #tempA (EmailID int)  
  
 create table #g(  
  GID int,  
  GroupID int,  
  ShortName varchar(50),  
  LongName varchar(255),  
  SurveyID int,  
  DatafieldSetID int,  
  )  
  
  
 select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)) from ecn5_communicator..Blast where BlastID = @BlastID and StatusCode <> 'Deleted'
 select @MailRoute = m.MTAName from ecn5_communicator..MTA m join ecn5_communicator..MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain  
 if(@MailRoute is null)  
 Begin  
  select @MailRoute = m.MTAName from ecn5_communicator..MTA m join ecn5_communicator..MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'  
 End  
 --select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and DomainName = @Domain  
 --if(@MailRoute is null)  
 --Begin  
 -- select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and IsDefault = 'true'  
 --End   
 --select @MailRoute = ConfigValue from ecn5_accounts..CustomerConfig where CustomerID = @CustomerID and ConfigName = 'MailRoute'  
 if @MailRoute is null  
 Begin  
  set @MailRoute = ''  
 End  
  Select @layoutid = LayoutID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @BlastID and b.StatusCode <> 'Deleted'
	
  if exists (select top 1 filterID from ecn5_Communicator..contentFilter WITH (NOLOCK) where layoutID = @layoutID and IsDeleted = 0)
			set @DynamicSlotExists = 1
  
 insert into #g   
 select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID from ecn5_communicator..groupdatafields  
 where GroupDatafields.groupID = @GroupID and IsDeleted = 0
  
 if lower(@ActionType) = 'unopen'  
 Begin  
  insert into #tempA  
     select distinct EmailID from BlastActivityOpens el where BlastID = @refBlastID   
   
    set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '    
end   
  
 -- Add all the other conditions below  
  else if lower(@ActionType) = 'unclick'  
  Begin  
    insert into #tempA  
     select distinct EmailID from BlastActivityClicks el where BlastID = @refBlastID  
   
    set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '  
  End  
  else if lower(@ActionType) = 'sent'
  BEGIN
	Set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
  END
    else if lower(@ActionType) = 'not sent'
  BEGIN
	Set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
  END
   
   declare @allContent table (contentID int)
  declare @dynamicTags table (DynamicTagID int, Tag varchar(50), contentID int)
	
	insert into @allContent
		select contentID
		from
		(
		select LayoutID, ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9 from ECN5_COMMUNICATOR..Layout	where LayoutID  = @layoutID  and IsDeleted = 0
		) x
		unpivot (contentID for slot in (ContentSlot1, ContentSlot2, ContentSlot3, ContentSlot4, ContentSlot5, ContentSlot6, ContentSlot7, ContentSlot8, ContentSlot9)) as unpvt
		where contentID > 0
		union
		select ContentID from ECN5_COMMUNICATOR..ContentFilter where LayoutID = @layoutID and IsDeleted = 0



	insert into @dynamicTags
		select distinct DynamicTagID, dt.Tag, dt.ContentID from ECN5_Communicator..Content c join @allContent ac on c.ContentID = ac.contentID cross join ECN5_Communicator..DynamicTag dt 
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
				FROM @dynamicTags dt1 join ECN5_Communicator..DynamicTagRule dtr1 on dt1.DynamicTagID = dtr1.DynamicTagID join ECN5_Communicator..[rule] r1 on dtr1.RuleID = r1.RuleID
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
   
  if not exists (select GroupDatafieldsID from ecn5_communicator..GroupDataFields JOIN ecn5_communicator..Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID and GroupDataFields.IsDeleted = 0)  or not exists(select * from #g)  
  Begin  

	     if @DynamicSlotExists = 0
	     begin
			exec ( 'select emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +  
			'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +  
		  '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ''' +  
			@MailRoute+''' as MailRoute ' +  
			', 1 as IsMTAPriority ' + @DynamicTagColumns +  
			' from  ecn5_communicator..Emails  join ecn5_communicator..EmailGroups eg on eg.EmailID = Emails.EmailID ' +   
			' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
			' and eg.GroupID = ' + @GroupID +  
			' and ( emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
			' ' + @Filter + ' ' +  @EmailString + ' order by emails.EmailID ')  
		end
		else
		begin
		  SELECT @selectslotstr = ECN5_COMMUNICATOR.dbo.fn_DCSelectString_SINGLE(@layoutID) 
  
			exec ( @selectslotstr + ',emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +  
		'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +  
	  '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ''' +  
		@MailRoute+''' as MailRoute ' +  
		', 1 as IsMTAPriority '  + @DynamicTagColumns +  
		' from  ecn5_communicator..Emails  join ecn5_communicator..EmailGroups eg on eg.EmailID = Emails.EmailID ' +   
		' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
		' and eg.GroupID = ' + @GroupID +  
		' and ( emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
		' ' + @Filter + ' ' +  @EmailString + ' order by emails.EmailID ')  
    
    end
 End  
 Else/* if UDF's exists*/  
  Begin  
    DECLARE @StandAloneUDFs VARCHAR(max)  
  SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID and IsDeleted = 0 AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'  
  DECLARE @TransactionalUDFs VARCHAR(max)  
  SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND IsDeleted = 0 and DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'         
    
  declare @sColumns varchar(max),  
    @tColumns varchar(max),  
    @standAloneQuery varchar(max),  
    @TransactionalQuery varchar(max)  
      
  if LEN(@standaloneUDFs) > 0  
  Begin  
   set @sColumns = ', SAUDFs.* '  
   set @standAloneQuery= ' left outer join     
    (  
     SELECT *  
      FROM  
      (  
      SELECT edv.emailID as tmp_EmailID,  gdf.ShortName, edv.DataValue  
      from ecn5_communicator..EmailDataValues edv  join    
        ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  
      where   
        gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null  and gdf.IsDeleted = 0
        and (edv.emailID = ' + convert(varchar(15), @EmailID) + ' or ' + convert(varchar(15), @EmailID) + ' = 0 )  
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
     from ecn5_communicator..EmailDataValues edv  join    
       ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  
     where   
       gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0 and gdf.IsDeleted = 0
       and (edv.emailID = ' + convert(varchar(15), @EmailID) + ' or ' + convert(varchar(15), @EmailID) + ' = 0 )  
     ) u  
     PIVOT  
     (  
     MAX (DataValue)  
     FOR ShortName in (' + @TransactionalUDFs + ')) as pvt      
   )   
   TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '  
  End  
  

	IF @DynamicSlotExists = 0
	BEGIN
	  exec (' select emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			 ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			 ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			 ' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +  
			 '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ''' +  
			 @MailRoute+''' as MailRoute, ' +  
			 ' 1 as IsMTAPriority, ' +
			 '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' + @sColumns + @tColumns + @DynamicTagColumns +   
				' from ecn5_communicator..Emails emails join  ' +   
		' ecn5_communicator..EmailGroups eg on eg.EmailID = emails.EmailID ' + @standAloneQuery + @TransactionalQuery +   
	 ' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
		' and (emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
		' and eg.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString )--+ ' order by Emails.EmailID '  
     END
     ELSE
      BEGIN
		 SELECT @selectslotstr = ECN5_COMMUNICATOR.dbo.fn_DCSelectString_SINGLE(@layoutID) 
	     
		   exec (@selectslotstr + ' ,emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			 ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			 ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			 ' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +  
			 '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ''' +  
			 @MailRoute+''' as MailRoute, ' +  
			 ' 1 as IsMTAPriority, ' +
			 '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' + @sColumns + @tColumns + @DynamicTagColumns +
			 ' from ecn5_communicator..Emails emails join  ' +   
			 ' ecn5_communicator..EmailGroups eg on eg.EmailID = emails.EmailID ' + @standAloneQuery + @TransactionalQuery +   
			 ' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
			 ' and (emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
			 ' and eg.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString )--+ ' order by Emails.EmailID ' 
     END
 END  
  
 drop table #tempA  
 drop table #G  
  
  
END