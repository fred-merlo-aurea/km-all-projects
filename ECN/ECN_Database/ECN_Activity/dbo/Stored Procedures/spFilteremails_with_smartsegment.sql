CREATE  PROCEDURE [dbo].[spFilteremails_with_smartsegment](
	@GroupID int,
	@CustomerID int,
	@FilterID int,
	@Filter varchar(8000),
	@BlastID int,
	@BlastID_and_BounceDomain varchar(250),
	@ActionType varchar(10),
	@refBlastID int)
AS
BEGIN 	
	--set @GroupID = 52958
	--set @CustomerID = 3137
	--set @FilterID = 27919;
	--set @Filter = ''
	--set @ActionType = ''
	--set @BlastID = 357587 
	--set @BlastID_and_BounceDomain = ''
	--set @refBlastID = 0
	
 	set NOCOUNT ON
 
 	declare 
	  @SqlString  varchar(8000), 
	  @EmailString  varchar(8000)

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

	if @BlastID = 0
	begin
		insert into #g 
		select GroupDatafieldsID, GroupID, ShortName, LongName, SurveyID, DatafieldSetID from ecn5_communicator..groupdatafields
		where GroupDatafields.groupID = @GroupID
	end
	else
	begin
		insert into #g 
		select distinct gdf.GroupDatafieldsID, gdf.GroupID, gdf.ShortName, gdf.LongName, gdf.SurveyID, gdf.DatafieldSetID 
		from  ecn5_communicator..Groups g join   
		ecn5_communicator..GroupDataFields gdf on gdf.GroupID = g.GroupID   
		where  g.groupID = @GroupID and   
		g.customerID = @CustomerID and  
		@Filter like '%' + ShortName + '%' 
		union
		select 	distinct gdf.GroupDatafieldsID, gdf.GroupID, gdf.ShortName, gdf.LongName, gdf.SurveyID, gdf.DatafieldSetID 
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
				ecn5_communicator..GroupDataFields gdf on gdf.GroupID = Groups.GroupID 
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
		Set @EmailString = @EmailString + ' and Emails.EmailID in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END
	else if lower(@ActionType) = 'not sent'
	BEGIN
		Set @EmailString = @EmailString + ' and Emails.EmailID not in (SELECT EmailID From BlastActivitySends bas with(nolock) where bas.BlastID = ' + convert(varchar(10), @refBlastID) + ') '
	END

 	if not exists (select GroupDatafieldsID from ecn5_communicator..GroupDataFields JOIN ecn5_communicator..Groups on GroupDataFields.GroupID = Groups.GroupID where Groups.groupID = @GroupID) or not exists(select * from #g)
 	Begin
  		exec (	'select Emails.EmailID, '''+@BlastID+''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
			  	' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
				' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
				' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, groupID, FormatTypeCode, SubscribeTypeCode, ' +
				'''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
				'''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+''' as BounceAddress ' +
				' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + 
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
				' and EmailGroups.GroupID = ' + @GroupID +
				' ' + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')
	End
	Else --if UDF's exists
 	Begin
		DECLARE @StandAloneUDFs VARCHAR(2000),
			@TransactionalUDFs VARCHAR(2000),
			@StandAloneUDFIDs VARCHAR(1000),
			@TransactionalUDFIDs VARCHAR(1000)
			
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM ecn5_communicator..groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'

		SELECT  @StandAloneUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM ecn5_communicator..groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE g.DatafieldSetID is null ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '') 

		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + g.ShortName FROM ecn5_communicator..groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY '],[' + g.ShortName FOR XML PATH('') ), 1, 2, '') + ']'       

		SELECT  @TransactionalUDFIDs = STUFF(( SELECT DISTINCT ',' + convert(varchar(10),GroupDatafieldsID) FROM ecn5_communicator..groupdatafields g join #g tg on g.GroupDatafieldsID = tg.GID 
		WHERE isnull(g.DatafieldSetID,0) > 0 ORDER BY ',' + convert(varchar(10),GroupDatafieldsID) FOR XML PATH('') ), 1, 1, '')
			
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
							from	ecn5_communicator..EmailDataValues edv join ecn5_communicator..groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @StandAloneUDFIDs + ')
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
							from	ecn5_communicator..EmailDataValues edv  join ecn5_communicator..groupdatafields gdf on edv.groupdatafieldsID = gdf.groupdatafieldsID
							where 
									gdf.groupdatafieldsID in (' + @TransactionalUDFIDs + ')
						 ) u
						 PIVOT
						 (
						 MAX (DataValue)
						 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
					) 
				TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End


		exec (' select Emails.EmailID, ''' + @BlastID + ''' as BlastID, EmailAddress, CustomerID, Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +
						   ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +
						   ' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +
						   ' Convert(varchar,Notes) as Notes, CreatedOn, LastChanged, GroupID, FormatTypeCode, SubscribeTypeCode, ' +
						   '''eid='' + LTRIM(STR(Emails.EmailID))+''&bid=' + @BlastID + ''' as ConversionTrkCDE, ' +
						   '''bounce_''+LTRIM(STR(Emails.EmailID)+''-'')+'''+@BlastID_and_BounceDomain+'''' + @sColumns + @tColumns +  
   						   ' from ecn5_communicator..Emails 
								join ' +   
				' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +  
				' where Emails.CustomerID = ' + @CustomerID + ' and Emails.EmailAddress like ''%@%.%'' and EmailGroups.SubscribeTypeCode = ''S''' +
				' and EmailGroups.GroupID = ' + @GroupID + ' '  + @Filter + ' ' +  @EmailString + ' order by Emails.EmailID ')

 END

	drop table #tempA
	drop table #G
	
END
