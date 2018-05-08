CREATE  PROCEDURE [dbo].[MovedToActivity_sp_downloadBlastEmails](
	@blastID varchar(10), 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100))
AS
BEGIN
	INSERT INTO ECN_ACTIVITY..OldProcsInUse (ProcName, DateUsed) VALUES ('sp_downloadBlastEmails', GETDATE()) 	
	--set @blastID = 433151
	--set @ReportType = 'send'
	--set @FilterType = ''
	--set @ISP = ''

	SET NOCOUNT ON

	declare   
		@CustomerID int,  
		@GroupID int,  
		@SqlString  varchar(8000),
		@ReportColumns varchar(500)

	select @CustomerID = CustomerID, @GroupID = GroupID from [BLAST] where BlastID = @BlastID  	  

	set @SqlString = ''

	create table #tempA 
	(  
		EmailID int,  
		ActionDate datetime,
		ActionValue varchar(255),
		ActionNotes  varchar(255) 
	)
  

	if @ReportType = 'send'
	Begin
		insert into #tempA  
		SELECT  eal.EmailID, eal.ActionDate, '', ''
		FROM	EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID  
		WHERE	BlastID=@blastID	AND ActionTypeCode in ('send','testsend') and e.emailaddress like '%' + @ISP 

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	end
	else if @ReportType = 'open'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT  eal.EmailID, eal.ActionDate, eal.ActionValue, ''
			FROM	EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID  
			WHERE	BlastID=@blastID	AND ActionTypeCode='open' and e.emailaddress like '%' + @ISP 
		else
			insert into #tempA  
			SELECT	eal.EmailID, max(eal.ActionDate), max(eal.ActionValue), ''
			FROM	EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID  
			WHERE	BlastID=@blastID	AND ActionTypeCode='open' and e.emailaddress like '%' + @ISP 
			group by eal.EmailID

		set @ReportColumns = ' #tempA.ActionDate as OpenTime, #tempA.ActionValue as Info '
	End
	else if @ReportType = 'noopen'
	Begin
			insert into #tempA  
			SELECT	eal.EmailID, eal.actiondate, '', '' 
			FROM	EmailActivityLog eal 
			WHERE	BlastID=@blastID	AND ActionTypeCode in ('send','testsend')
			and 
				EmailID not in (SELECT EmailID FROM	EmailActivityLog WHERE	BlastID=@blastID AND ActionTypeCode in ('open','click'))

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'click'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT	eal.EmailID, eal.ActionDate, eal.ActionValue, ''
			FROM	EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID  
			WHERE	BlastID=@blastID	AND ActionTypeCode='click' and e.emailaddress like '%' + @ISP 
		else
			insert into #tempA  
			SELECT	eal.EmailID, max(eal.ActionDate),  eal.ActionValue, ''
			FROM	EmailActivityLog eal JOIN Emails e ON eal.EMailID=e.EMailID  
			WHERE	BlastID=@blastID	AND ActionTypeCode='click' and e.emailaddress like '%' + @ISP 
			group by	eal.EmailID , eal.ActionValue

		set @ReportColumns = ' #tempA.ActionDate as ClickTime, #tempA.ActionValue as Link '
	End
	else if @ReportType = 'noclick'
	Begin
			insert into #tempA  
			SELECT	eal.EmailID, eal.actiondate, '', '' 
			FROM	EmailActivityLog eal 
			WHERE	BlastID=@blastID	AND ActionTypeCode in ('send','testsend')
			and 
				EmailID not in (SELECT EmailID FROM	EmailActivityLog WHERE	BlastID=@blastID AND ActionTypeCode in ('click'))

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'bounce'
	Begin
		insert into #tempA  
		SELECT	eal.EmailID, eal.ActionDate, eal.ActionValue, eal.ActionNotes 
		FROM	EmailActivityLog eal join Emails e on eal.EMailID=e.EMailID 
		WHERE	e.emailaddress like '%' + @ISP and
				eal.eaid in 
				(
					select 	max(EAID) as EAID
					FROM	EmailActivityLog 
					WHERE	BlastID = @blastID AND ActionTypeCode='bounce' 
							AND ActionValue= (case when len(ltrim(rtrim(@FilterType))) > 0 then @FilterType else ActionValue end)
					group by emailID
				)	
	
		set @ReportColumns = ' #tempA.ActionDate as BounceTime, #tempA.ActionValue as BounceType, #tempA.ActionNotes as BounceSignature '
	end
	else if @ReportType = 'unsubscribe'
	Begin
		insert into #tempA  
		SELECT  eal.EmailID, eal.ActionDate, eal.ActionValue, eal.ActionNotes
		FROM   
				EmailActivityLog eal join emails e on eal.emailid = e.emailID  
		where	eal.BlastID=@blastID AND eal.ActionTypeCode=@FilterType   and  
				e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, #tempA.ActionNotes as Reason '
		
	end
	else if @ReportType = 'resend'
	Begin
		insert into #tempA  
		SELECT  eal.EmailID, eal.ActionDate, eal.ActionValue, ''
		FROM   
				EmailActivityLog eal join emails e on eal.emailid = e.emailID  
		where	eal.BlastID=@blastID AND eal.ActionTypeCode='resend'   and  
				e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as ResentTime, #tempA.ActionValue as Action '
		
	end

 	declare @g table(GID int, ShortName varchar(50))        
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier)        

	insert into @g   
	select GroupDatafieldsID, ShortName from groupdatafields where GroupDatafields.groupID = @GroupID        

	if @groupID = 0
	Begin
		exec( 'select EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' DateAdded, DateUpdated ' +  
		' from  Emails join #tempA on #tempA.EmailID = Emails.EmailID '+  
		' where Emails.CustomerID = ' + @CustomerID + 
		' order by #tempA.ActionDate desc, emailaddress')  

	end
	else if not exists(select * from @g)
	Begin  
		exec( 'select EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' +  
		' from  Emails join EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
		' join #tempA on #tempA.EmailID = Emails.EmailID '+  
		' where Emails.CustomerID = ' + @CustomerID + 
		' and EmailGroups.GroupID = ' + @GroupID +  
		' order by #tempA.ActionDate desc, emailaddress')  
	End  
	else
	Begin
		insert into #E   
		select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID 
		from EmailDataValues join @g g on g.GID = EmailDataValues.GroupDataFieldsID join #tempA on #tempA.EmailID = EmailDataValues.EmailID 

		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						from	EmailDataValues edv  join  
								Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join 
								#tempA on #tempA.EmailID = edv.EmailID 
						where 
								gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID is null
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
					from	EmailDataValues edv  join  
							Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join 
								#tempA on #tempA.EmailID = edv.EmailID 
					where 
							gdf.GroupID = ' + convert(varchar(15), @GroupID) + ' and datafieldsetID > 0
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End
		
		exec ('select EmailAddress, ' + @ReportColumns + ' , Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from Emails 
					join ' +       
			' EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +    
			 ' join #tempA on #tempA.EmailID = Emails.EmailID '+    
			' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID +
			' order by #tempA.ActionDate desc, emailaddress')  


	end

	 drop table #tempA  
	 drop table #E  

	SET NOCOUNT OFF

END
