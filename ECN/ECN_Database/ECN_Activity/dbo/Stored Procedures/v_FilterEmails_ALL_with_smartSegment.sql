﻿
CREATE PROCEDURE [dbo].[v_FilterEmails_ALL_with_smartSegment](  
 @GroupID int,  
 @CustomerID int,  
 @FilterID int,  
 @Filter varchar(MAX),  
 @BlastID int,  
 @BlastID_and_BounceDomain varchar(250),  
 @ActionType varchar(10),  
 @refBlastID varchar(2000),  
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
   
   @EmailString  varchar(MAX),  
   @MailRoute varchar(255),  
   @Domain varchar(100)  
  
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
  
    create table #RefBlastIDs(refBlastID nvarchar(100))
  
  insert into #RefBlastIDs(refBlastID)
  SELECT * FROM fn_Split(@refBlastID, ',')
  
 select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)) from ecn5_communicator..Blast with (nolock) where BlastID = @BlastID and StatusCode <> 'Deleted'
 select @MailRoute = m.MTAName from ecn5_communicator..MTA m with (nolock) join ecn5_communicator..MTACustomer mc with (nolock) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain  
 if(@MailRoute is null)  
 Begin  
  select @MailRoute = m.MTAName from ecn5_communicator..MTA m with (nolock) join ecn5_communicator..MTACustomer mc with (nolock) on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'  
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
  
 insert into #g   
 select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID from ecn5_communicator..groupdatafields with (nolock)  
 where GroupDatafields.groupID = @GroupID and IsDeleted = 0
  
 if lower(@ActionType) = 'unopen'  
 Begin  
  insert into #tempA  
     select distinct EmailID from BlastActivityOpens el with (nolock) where BlastID in (SELECT CONVERT(int, refBlastID) FROM #RefBlastIDs)  
   
    set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E with (nolock) Where BlastID in (SELECT Convert(int, refBlastID) from #RefBlastIDs) and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '    
end   
  
 -- Add all the other conditions below  
  else if lower(@ActionType) = 'unclick'  
  Begin  
    insert into #tempA  
     select distinct EmailID from BlastActivityClicks el where BlastID in (SELECT CONVERT(int, refBlastID) FROM #RefBlastIDs)  
   
    set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E with (nolock) Where BlastID in (SELECT Convert(int, refBlastID) from #RefBlastIDs) and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '  
  End  
  else if lower(@ActionType) = 'sent'
	BEGIN
		Set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END
	else if lower(@ActionType) = 'not sent'
	BEGIN
		Set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END
   
  if not exists (select GroupDatafieldsID from ecn5_communicator..GroupDataFields with (nolock) JOIN ecn5_communicator..Groups with (nolock) on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID and GroupDataFields.IsDeleted = 0)  or not exists(select * from #g)  
  Begin  
    exec ( 'select emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
      ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
    ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
    ' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +  
    '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +  
  '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ''' +  
    @MailRoute+''' as MailRoute ' +  
    ', 1 as IsMTAPriority ' +
    ' from  ecn5_communicator..Emails with (nolock) join ecn5_communicator..EmailGroups eg with (nolock) on eg.EmailID = Emails.EmailID ' +   
    ' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
    ' and eg.GroupID = ' + @GroupID +  
    ' and ( emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
    ' ' + @Filter + ' ' +  @EmailString + ' order by emails.EmailID ')  
 End  
 Else/* if UDF's exists*/  
  Begin  
    DECLARE @StandAloneUDFs VARCHAR(MAX)  
  SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields with (nolock) WHERE GroupID = @GroupID and IsDeleted = 0 AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'  
  DECLARE @TransactionalUDFs VARCHAR(MAX)  
  SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields with (nolock) WHERE GroupID = @GroupID AND IsDeleted = 0 and DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'         
    
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
      from ecn5_communicator..EmailDataValues edv with (nolock) join    
        ecn5_communicator..Groupdatafields gdf with (nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  
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
     from ecn5_communicator..EmailDataValues edv with (nolock) join    
       ecn5_communicator..Groupdatafields gdf with (nolock) on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  
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
  
  
  
  exec (' select emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
         ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
         ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
         ' Convert(varchar(1000),Notes) as Notes, Emails.Password, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +  
         '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ''' +  
         @MailRoute+''' as MailRoute, ' +  
         ' 1 as IsMTAPriority, ' +
         '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' + @sColumns + @tColumns +    
            ' from ecn5_communicator..Emails emails with (nolock) join  ' +   
    ' ecn5_communicator..EmailGroups eg with (nolock) on eg.EmailID = emails.EmailID ' + @standAloneQuery + @TransactionalQuery +   
 ' where emails.CustomerID = ' + @CustomerID + ' and emails.EmailAddress like ''%@%.%''' +  
    ' and (emails.emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' +   
    ' and eg.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString )--+ ' order by Emails.EmailID '  
     
 END  
  
 drop table #tempA  
 drop table #G  
 drop table #RefBlastIDs
  
END