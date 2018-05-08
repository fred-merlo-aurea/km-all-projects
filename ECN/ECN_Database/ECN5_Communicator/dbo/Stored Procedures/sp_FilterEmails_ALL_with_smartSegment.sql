CREATE  PROCEDURE [dbo].[sp_FilterEmails_ALL_with_smartSegment](
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
	
	declare @hostname varchar(400)
	set @hostname = HOST_NAME() + ' / App Name : ' + APP_NAME()

	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_FilterEmails_ALL_with_smartSegment ( host: ' + @hostname + ')', GETDATE()) 	
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
	  @Domain varchar(100)

	set @SqlString = ''
	set @EmailString  = ''
			
	create table #tempA (EmailID int)

	create table #g(
		GID int,
		GroupID	int,
		ShortName	varchar(50),
		LongName	varchar(255),
		SurveyID	int,
		DatafieldSetID	int,
		)


	select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)) from Blast where BlastID = @BlastID
	select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
	if(@MailRoute is null)
	Begin
		select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
	End
	--select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and DomainName = @Domain
	--if(@MailRoute is null)
	--Begin
	--	select @MailRoute = MTAName from CustomerMTA where CustomerID = @CustomerID and IsDefault = 'true'
	--End	
	--select @MailRoute = ConfigValue from [ECN5_ACCOUNTS].[DBO].CustomerConfig where CustomerID = @CustomerID and ConfigName = 'MailRoute'
	if @MailRoute is null
	Begin
		set @MailRoute = ''
	End

	insert into #g 
	select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID from groupdatafields
	where GroupDatafields.groupID = @GroupID and IsDeleted = 0

	if lower(@ActionType) = 'unopen'
	Begin
		insert into #tempA
    	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'open' 
 
  		set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) ' 	
end 

	-- Add all the other conditions below
 	else if lower(@ActionType) = 'unclick'
 	Begin
  		insert into #tempA
    	select distinct EmailID from EmailActivityLog el where BlastID = @refBlastID and ActionTypeCode = 'click' 
 
  		set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E Where BlastID = ' + convert(varchar(10),@refBlastID) + ' and ActionTypeCode = ''send'' and not exists (select EmailID from #tempA el where el.EmailID = E.emailID) ) '
 	End
	else if lower(@ActionType) = 'sent'
	BEGIN
		set @EmailString = @EmailString + ' and [Emails].EmailID in (select EmailID from EmailActivityLog E WHERE BlastID = ' + Convert(varchar(10), @refBlastID) + ' and ActionTypeCode = ''send'')'
	END
	else if lower(@ActionType) = 'not sent'
	BEGIN
		set @EmailString = @EmailString + ' and [Emails].EmailID not in (select EmailID from EmailActivityLog E WHERE BlastID = ' + Convert(varchar(10), @refBlastID) + ' and ActionTypeCode = ''send'')'
	END
 
 	if not exists (select GroupDatafieldsID from GroupDataFields JOIN Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID)  or not exists(select * from #g)
 	Begin
  		exec (	'select [Emails].EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
			  	' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
				'''eid='' + LTRIM(STR([Emails].EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
				'''bounce_''+LTRIM(STR([Emails].EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress, ''' +
				@MailRoute+''' as MailRoute ' +
				' from  [Emails] join [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + 
				' where [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%''' +
				' and [EmailGroups].GroupID = ' + @GroupID +
				' and ( [Emails].emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' + 
				' ' + @Filter + ' ' +  @EmailString + ' order by [Emails].EmailID ')
	End
	Else/* if UDF's exists*/
 	Begin
  		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID and IsDeleted = 0 AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID and IsDeleted = 0 AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						from	[EMAILDATAVALUES] edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null and gdf.IsDeleted = 0 
								and (edv.emailID = ' + convert(varchar(15), @EmailID) + ' or ' + convert(varchar(15), @EmailID) + ' = 0 )
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
					from	[EMAILDATAVALUES] edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0 and gdf.IsDeleted = 0 
							and (edv.emailID = ' + convert(varchar(15), @EmailID) + ' or ' + convert(varchar(15), @EmailID) + ' = 0 )
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
						   ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
						   '''eid='' + LTRIM(STR([Emails].EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ''' +
						   @MailRoute+''' as MailRoute, ' +
						   '''bounce_''+LTRIM(STR([Emails].EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' + @sColumns + @tColumns +  
   						   ' from [Emails] join  ' + 
				' [EmailGroups] on [EmailGroups].EmailID = [Emails].EmailID ' + @standAloneQuery + @TransactionalQuery + 
				' where [Emails].CustomerID = ' + @CustomerID + ' and [Emails].EmailAddress like ''%@%.%''' +
				' and ([Emails].emailID = ' + @EmailID + ' or ' + @EmailID + ' = 0 ) ' + 
				' and [EmailGroups].GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString )--+ ' order by [Emails].EmailID '
			
 END

	drop table #tempA
	drop table #G


END
