CREATE PROCEDURE [dbo].[spKMPSSubscriptionData]
 (
  @GroupID int,
  @CustomerID int,
  @Filter varchar(8000),
  @BlastID int,
  @BlastID_and_BounceDomain varchar(250),
  @ActionType varchar(10),
  @refBlastID int
 )
AS
BEGIN
 set NOCOUNT ON
 
 declare
  @ColumnSet1A varchar(8000),
  @ColumnSet1B varchar(8000),
  @ColumnSet1 varchar(8000),
  @ColumnSet2 varchar(8000),
  @SqlString  varchar(8000), 
  @SqlString1  varchar(8000), 
  @SqlString2  varchar(8000), 
  @EmailString  varchar(8000)
 
  create table #tempA (EmailID int)

  set @ColumnSet1A = ''
  set @ColumnSet1B = ''
  set @ColumnSet1 = ''
  set @ColumnSet2 = ''
  set @SqlString = ''
  set @SqlString1 = ''
  set @SqlString2 = ''
  set @EmailString  = ''

 if lower(@ActionType) = 'unopen'
 Begin
 
  insert into #tempA
    select distinct EmailID from BlastActivityOpens baop where BlastID = @refBlastID             
 
  set @EmailString = @EmailString + ' and ecn5_communicator..Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 end 
 -- Add all the other conditions below
 else if lower(@ActionType) = 'unclick'
 Begin
  insert into #tempA
    select distinct EmailID from BlastActivityClicks bacl where BlastID = @refBlastID      
 
  set @EmailString = @EmailString + ' and ecn5_communicator..Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 End
 else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from BlastActivitySends with(nolock) WHERE BlastID in (' + @refBlastID + ')) '
	END
else if lower(@ActionType) = 'not sent'
BEGIN
	set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from BlastActivitySends with(nolock) WHERE BlastID in (' + @refBlastID + ')) '
END
 
 if not exists (select * from ecn5_communicator..GroupDataFields JOIN ecn5_communicator..Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID) 
 Begin
  set @SqlString ='select Emails.EmailID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
       ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
       ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
       ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
       '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid='+convert(varchar(10),@BlastID)+''' as ConversionTrkCDE, ' +
       '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' +
       ' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = ecn5_communicator..Emails.EmailID ' + 
       ' where ecn5_communicator..Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and ecn5_communicator..Emails.EmailAddress like ''%@%.%'' ' +
       ' and ecn5_communicator..EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) + ' ' + @Filter + ' ' + @EmailString + 
       ' order by ecn5_communicator..Emails.EmailID '
 
  print(@SqlString)
  exec(@SqlString)
 
 End
 Else
 Begin
  /* if UDF's exists*/
  --select  @ColumnSet1  = @ColumnSet1 + coalesce('max([' + ShortName  + ']) as ' + ShortName  + ',',''),
  select  @ColumnSet1A  = @ColumnSet1A + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
   @ColumnSet2 = @ColumnSet2 + coalesce('case when ecn5_communicator..GroupDatafields.groupDataFieldsID = ' + convert(varchar(10),GroupDatafields.groupDataFieldsID) + ' then ecn5_communicator..EmailDataValues.DataValue else null end as [' + ShortName  + '],', '')
  from ecn5_communicator..GroupDatafields
  where
  ecn5_communicator..GroupDatafields.groupID = @GroupID
 
   set @ColumnSet1A = ' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + substring(@ColumnSet1A,0,len(@ColumnSet1A)) + ' from (  '
   set @ColumnSet1B = 
   ' select ecn5_communicator..Emails.EmailID, ecn5_communicator..EmailDataValues.EntryID, ' + substring(@ColumnSet2,0,len(@ColumnSet2))
   + ' from ecn5_communicator..Emails join '+
   + ' ecn5_communicator..EmailDataValues on ecn5_communicator..Emails.EmailID = ecn5_communicator..EmailDataValues.EmailID join '+
   + ' ecn5_communicator..GroupDataFields on ecn5_communicator..EmailDataValues.groupDataFieldsID = ecn5_communicator..GroupDataFields.groupDataFieldsID '
   + ' where ecn5_communicator..Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' ) as InnerTable1 '
   + ' Group by InnerTable1.EmailID, InnerTable1.EntryID'
 
   set @SqlString1 =
   ' select ecn5_communicator..Emails.EmailID, '''+convert(varchar(10),@BlastID)+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
       ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
        ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
        ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
        '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid='+convert(varchar(10),@BlastID)+''' as ConversionTrkCDE, ' +
   '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, InnerTable2.* ' +
   ' from ecn5_communicator..Emails left outer join  ('

   set @SqlString2 = 
   ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +
   ' join ecn5_communicator..EmailGroups on ecn5_communicator..EmailGroups.EmailID = Emails.EmailID ' +
   ' where ecn5_communicator..Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and ecn5_communicator..Emails.EmailAddress like ''%@%.%'' ' +
   ' and ecn5_communicator..EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) --+
--   ' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID '

-- made the above modification 'cos the final sql was exceeding the 8000 char limit on the @SqlString variable length. 
-- and Added the last part the the exec & the print functions.. - ashok 09-20-2005
--   print (@SqlString)
--   exec (@SqlString)
--   ' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID '
 
   print(@ColumnSet1A);
   print (@ColumnSet1B);
   print (@SqlString1+ @ColumnSet1A + @ColumnSet1B + @SqlString2+' '  + @Filter + ' ' +  @EmailString + ' order by ecn5_communicator..Emails.EmailID ')
   exec (@SqlString1+ @ColumnSet1A + @ColumnSet1B + @SqlString2+' '  + @Filter + ' ' +  @EmailString + ' order by ecn5_communicator..Emails.EmailID ')

 END
 
 drop table #tempA
 
END
