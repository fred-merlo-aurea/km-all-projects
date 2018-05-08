------------------------------------------------------
-- 2014-01-28 MK Changed EmailDataValueId to BIGINT
--
--
--
------------------------------------------------------

CREATE proc [dbo].[sp_GetGroupEmailProfilesEmailField] (  
 @GroupID int,  
 @CustomerID int,  
 @Filter varchar(8000),  
 @SubscribeType varchar(100)  
)  
as  
  
BEGIN  
  set NOCOUNT ON  
   
  declare   
   @SqlString  varchar(8000),   
   @Col1 varchar(8000),  
   @Col2 varchar(8000)  
  
 set @SqlString = ''  
 set @Col1  = ''  
 set @Col2  = ''  
     
 create table #tempA (EmailID int)  
  
 create table #g(  
  GID int,  
  GroupID int,  
  ShortName varchar(50),  
  LongName varchar(255),  
  SurveyID int,  
  DatafieldSetID int,  
  )  
  
 create table #E(  
   EmailDataValuesID BIGINT,--int,  
   EmailID int,  
   GroupDatafieldsID int,  
   DataValue varchar(500),  
   ModifiedDate datetime,  
   SurveyGridID int,  
   EntryID uniqueidentifier  
  )  
  
 insert into #g   
 select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID
 from groupdatafields  
 where GroupDatafields.groupID = @GroupID  
    
 if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from #g)  
  Begin  
  print  ( @SubscribeType)  
    exec ( 'select EmailAddress '+  
    ' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'+  
    ' and EmailGroups.GroupID = ' + @GroupID +  
    ' ' + @Filter + '  order by Emails.EmailID ')  
 End  
 Else --if UDF's exists  
  Begin  
  
  insert into #E   
  select EmailDataValues.*   
  from EmailDataValues join #G on #G.GID = EmailDataValues.GroupDataFieldsID  
  
  --select * from #E  
  
  select  @Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),  
    @Col2 = @Col2 + coalesce('Case when G.GID=' + convert(varchar(10),#g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')  
  from   
    #g where GroupID = @GroupID  
   
  set @Col1 = substring(@Col1, 0, len(@Col1))   
  set @Col2 = substring(@Col2 , 0, len(@Col2))  
  
  exec (' select EmailAddress' +  
            ' from Emails left outer join  (' +   
    ' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ( ' +   
    ' select Emails.EmailID, E.EntryID, ' + @Col2 +  
       ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join ' +   
       ' #E E on Emails.EmailID = E.EmailID join ' +   
       ' #g G on E.groupDataFieldsID = G.GID AND G.GroupID = Groups.GroupID ' +   
       ' where Groups.groupID = ' + @GroupID + ' and Groups.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+') ) as InnerTable1 ' +   
       ' Group by InnerTable1.EmailID, InnerTable1.EntryID' +   
    ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +  
    ' join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +  
    ' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode IN ('+@SubscribeType+')'+  
    ' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' order by Emails.EmailID ')  
END  
 drop table #tempA  
 drop table #G  
 drop table #E  
END