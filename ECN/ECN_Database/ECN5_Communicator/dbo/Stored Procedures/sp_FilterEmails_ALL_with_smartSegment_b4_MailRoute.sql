--Sp_helptext sp_FilterEmails_ALL_with_smartSegment 2850, 1053, 0, 'AND ( emails.EmailID in ( SELECT EmailID FROM EmailGroups WHERE GroupID = 2850 AND (CreatedOn between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'' or LastChanged between ''03/11/2008 00:00:00'' and ''03/11/2008 23:59:59'') ))', 0,'', '',0

CREATE proc [dbo].[sp_FilterEmails_ALL_with_smartSegment_b4_MailRoute] (
	@GroupID int,
	@CustomerID int,
	@FilterID int,
	@Filter varchar(8000),
	@BlastID int,
	@BlastID_and_BounceDomain varchar(250),
	@ActionType varchar(10),
	@refBlastID int)
as

BEGIN
 	set NOCOUNT ON
 
 	declare 
	  @SqlString  varchar(8000), 
	  @EmailString  varchar(8000),
	  @Col1 varchar(8000),
	  @Col2 varchar(8000)

	set @SqlString = ''
	set @EmailString  = ''
	set @Col1  = ''
	set @Col2  = ''
			
	create table #tempA (EmailID int)

	create table #g(
		GID int,
		GroupID	int,
		ShortName	varchar(50),
		LongName	varchar(255),
		SurveyID	int,
		DatafieldSetID	int,
		)

	create table #E(
			EmailDataValuesID	int,
			EmailID	int,
			GroupDatafieldsID	int,
			DataValue	varchar(500),
			ModifiedDate	datetime,
			SurveyGridID	int,
			EntryID	uniqueidentifier
		)


	insert into #g 
	select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID from groupdatafields
	where GroupDatafields.groupID = @GroupID

	if lower(@ActionType) = 'unopen'
	Begin
		insert into #tempA
    	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'open' 
 
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) ' 	
end 

	-- Add all the other conditions below
 	else if lower(@ActionType) = 'unclick'
 	Begin
  		insert into #tempA
    	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'click' 
 
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 	End
 
 	if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID)  or not exists(select * from #g)
 	Begin
  		exec (	'select Emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
			  	' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
				'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
				'''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' +
				' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + 
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%''' +
				' and EmailGroups.GroupID = ' + @GroupID +
				' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')
	End
	Else/* if UDF's exists*/
 	Begin
  		
		select  @Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
				@Col2 = @Col2 + coalesce('Case when G.GID=' + convert(varchar(10),#g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')
		from 
				#g where GroupID = @GroupID
 
		set @Col1 = substring(@Col1, 0, len(@Col1)) 
		set @Col2 = substring(@Col2 , 0, len(@Col2))

		insert into #E 
		select EmailDataValues.* 
		from EmailDataValues join #G on #G.GID = EmailDataValues.GroupDataFieldsID


		exec (' select Emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
						   ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
						   ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
						   ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
						   '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
						   '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, InnerTable2.* ' +
   						   ' from Emails left outer join  (' + 
				' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ( ' + 
				' select Emails.EmailID, E.EntryID, ' + @Col2 +
			    ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join ' + 
			    ' EmailDataValues E on Emails.EmailID = E.EmailID join ' + 
			    ' #g G on E.groupDataFieldsID = G.GID AND G.GroupID = Groups.GroupID ' + 
			    ' where Groups.groupID = ' + @GroupID + ' and Groups.CustomerID = ' + @CustomerID + ') as InnerTable1 ' + 
			    ' Group by InnerTable1.EmailID, InnerTable1.EntryID' + 
				') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +
				' join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%''' +
				' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString )--+ ' order by Emails.EmailID '
			
 END

	drop table #tempA
	drop table #G
	drop table #E
END
