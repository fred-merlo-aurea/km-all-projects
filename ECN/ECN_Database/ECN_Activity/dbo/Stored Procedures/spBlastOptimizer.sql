CREATE proc [dbo].[spBlastOptimizer] (
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


	if @BlastID = 0
	begin
		insert into #g 
		select * from ecn5_communicator..groupdatafields
		where GroupDatafields.groupID = @GroupID
	end
	else
	begin
		insert into #g 
		select distinct GroupDataFields.*
		from 	ecn5_communicator..Groups join 
				ecn5_communicator..GroupDataFields on GroupDataFields.GroupID = Groups.GroupID 
		where 	Groups.groupID = @GroupID and 
				Groups.customerID = @CustomerID and
			  	@Filter like '%' + ShortName + '%'
		union
		select 	distinct GroupDataFields.* 
		from 	ecn5_communicator..[BLAST] join 
				ecn5_communicator..[LAYOUT] on [BLAST].layoutID = [LAYOUT].layoutID left outer join
				ecn5_communicator..Content on 	Content.ContentID = [LAYOUT].ContentSlot1 or 
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
				ecn5_communicator..GroupDataFields on GroupDataFields.GroupID = Groups.GroupID 
		where 
				[BLAST].BlastID = @BlastID and Groups.GroupID = @GroupID and Groups.CustomerID = @CustomerID and
				(
					Content.ContentSource like '%' + ShortName + '%' or 
					Content.ContentTExt  like '%' + ShortName + '%' or 
					[CONTENTFILTER].whereclause like '%' + ShortName + '%' or
					[TEMPLATE].TemplateSource like '%' + ShortName + '%'  or 
					[TEMPLATE].TemplateText like '%' + ShortName + '%'
				)
	end


	--select * from #g


	if lower(@ActionType) = 'unopen'
	Begin
		insert into #tempA
    	select distinct EmailID from BlastActivityOpens bao where BlastID = @refBlastID 
 
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 	end 

	-- Add all the other conditions below
 	else if lower(@ActionType) = 'unclick'
 	Begin
  		insert into #tempA
    	select distinct EmailID from BlastActivityClicks bac where BlastID = @refBlastID
 
  		set @EmailString = @EmailString + ' and Emails.EmailID in (select EmailID from BlastActivitySends E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 	End
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID from BlastActivitySends WITH(NOLOCK) WHERE BlastID = ' + CONVERT(varchar(10), @refBlastID) + ') '
	END
	else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID from BlastActivitySends WITH(NOLOCK) WHERE BlastID = ' + CONVERT(varchar(10), @refBlastID) + ') '
	END

 	if not exists (select * from ecn5_communicator..GroupDataFields JOIN ecn5_communicator..Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from #g)
 	Begin
  		exec (	'select Emails.EmailID,  EmailAddress ' + 
				--' Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2 ' +
			  	--' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
				--' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
				--' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
				--'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
				--'''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' +
				' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + 
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
				' and EmailGroups.GroupID = ' + @GroupID +
				' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')
	End
	Else --if UDF's exists
 	Begin

		insert into #E 
		select EmailDataValues.* 
		from ecn5_communicator..EmailDataValues join #G on #G.GID = EmailDataValues.GroupDataFieldsID

		--select * from #E

		select  @Col1 = @Col1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
				@Col2 = @Col2 + coalesce('Case when G.GID=' + convert(varchar(10),#g.GID) + ' then E.DataValue end [' + RTRIM(ShortName)  + '],', '')
		from 
				#g where GroupID = @GroupID
 
		set @Col1 = substring(@Col1, 0, len(@Col1)) 
		set @Col2 = substring(@Col2 , 0, len(@Col2))
/*
		exec (	' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ( ' + 
			' select Emails.EmailID, E.EntryID, ' + @Col2 +
			' from ecn5_communicator..Groups join ecn5_communicator..EmailGroups on Groups.groupID = EmailGroups.groupID join  ecn5_communicator..Emails on Emails.EmailID = EmailGroups.EmailID join ' + 
			' #E E on Emails.EmailID = E.EmailID join ' + 
			' #g G on E.groupDataFieldsID = G.GID AND G.GroupID = Groups.GroupID ' + 
			' where Groups.groupID = ' + @GroupID + ' and Groups.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode = ''S'') as InnerTable1 ' + 
			' Group by InnerTable1.EmailID, InnerTable1.EntryID')
*/

		exec (' select Emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
						   ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
						   ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
						   ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
						   '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
						   '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, InnerTable2.* ' +
   						   ' from ecn5_communicator..Emails left outer join  (' + 
				' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + @Col1 + ' from ( ' + 
				' select Emails.EmailID, E.EntryID, ' + @Col2 +
			    ' from ecn5_communicator..Groups join ecn5_communicator..EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join ' + 
			    ' #E E on Emails.EmailID = E.EmailID join ' + 
			    ' #g G on E.groupDataFieldsID = G.GID AND G.GroupID = Groups.GroupID ' + 
			    ' where Groups.groupID = ' + @GroupID + ' and Groups.CustomerID = ' + @CustomerID + ' and EmailGroups.SubscribeTypeCode = ''S'') as InnerTable1 ' + 
			    ' Group by InnerTable1.EmailID, InnerTable1.EntryID' + 
				') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +
				' join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
				' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')

 END

	drop table #tempA
	drop table #G
	drop table #E
END
