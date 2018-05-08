CREATE PROCEDURE [dbo].[sp_FilterEmails_new]
	(
		@GroupID int,
		@CustomerID int,
		@FilterID int,
		@Filter varchar(2500),
		@BlastID int,
		@BlastID_and_BounceDomain varchar(250)
	)
AS
BEGIN
	set NOCOUNT ON
	declare @ColumnSet1 varchar(8000),
		@ColumnSet2 varchar(8000),
		@SqlString  varchar(8000)
	set @ColumnSet1 = ''
	set @ColumnSet2 = ''
	set @SqlString = ''
	if not exists (select * from GroupDataFields JOIN Groups on 
GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID)
	Begin
		set @SqlString ='select Emails.EmailID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
		     ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
		     ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
		     ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
		     '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid='+convert(varchar(10),@BlastID)+''' as ConversionTrkCDE, ' +
		     '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' +
		     ' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
		     ' where Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
		     ' and EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) + ' ' + @Filter +
		     ' order by Emails.EmailID '
		print(@SqlString)
		exec(@SqlString)
	End
	Else
	Begin
		/* if UDF's exists*/
		--select  @ColumnSet1  = @ColumnSet1 + coalesce('max([' + ShortName  + ']) as ' + ShortName  + ',',''),
		select  @ColumnSet1  = @ColumnSet1 + coalesce('max([' + RTRIM(ShortName)  + ']) as ''' + RTRIM(ShortName)  + ''',',''),
			@ColumnSet2 = @ColumnSet2 + coalesce('case when GroupDatafields.groupDataFieldsID = ' + convert(varchar(10),GroupDatafields.groupDataFieldsID) + ' then EmailDataValues.DataValue else null end as [' + ShortName  + '],', '')
		from GroupDatafields
		where
		GroupDatafields.groupID = @GroupID
			set @ColumnSet1 = ' select InnerTable1.EmailID as tmp_EmailID, InnerTable1.EntryID, ' + substring(@ColumnSet1,0,len(@ColumnSet1)) + ' from (  ' +
			+ ' select Emails.EmailID, EmailDataValues.EntryID, ' + substring(@ColumnSet2,0,len(@ColumnSet2))
			+ ' from Groups join EmailGroups on Groups.groupID = EmailGroups.groupID join  Emails on Emails.EmailID = EmailGroups.EmailID join '+
			+ ' EmailDataValues on Emails.EmailID = EmailDataValues.EmailID join '+
			+ ' GroupDataFields on EmailDataValues.groupDataFieldsID = GroupDataFields.groupDataFieldsID AND GroupDataFields.GroupID = Groups.GroupID '
			+ ' where Groups.groupID = ' + convert(varchar(10),@GroupID) + ' and Groups.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and EmailGroups.SubscribeTypeCode = ''S'') as InnerTable1 '
			+ ' Group by InnerTable1.EmailID, InnerTable1.EntryID'
			set @SqlString =
			' select Emails.EmailID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
		    	' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
		     	' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
		     	' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
		     	'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid='+convert(varchar(10),@BlastID)+''' as ConversionTrkCDE, ' +
			'''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, InnerTable2.* ' +
			' from Emails left outer join  (' + @ColumnSet1 + ') as InnerTable2 on Emails.EmailID = InnerTable2.tmp_EmailID ' +
			' join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +
			' where Emails.CustomerID = ' + convert(varchar(10),@CustomerID) + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
			' and EmailGroups.GroupID = ' + convert(varchar(10),@GroupID) +
			' ' + @Filter + ' order by Emails.EmailID '
			print (@SqlString)
			exec (@SqlString)
	END
END
