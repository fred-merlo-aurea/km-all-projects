﻿
CREATE PROCEDURE [dbo].[spDownloadBlastEmails_backup_07112014]
	@blastID varchar(10), 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100),
	@StartDate datetime = null,
	@EndDate datetime = null
AS
BEGIN 	
	SET NOCOUNT ON


set @StartDate = @StartDate + '00:00:00 '   
set @EndDate = @EndDate + '23:59:59'

	--set @blastID = 456550
	--set @ReportType = 'send'
	--set @FilterType = ''
	--set @ISP = ''

	declare   
		@CustomerID int,  
		@GroupID int,  
		@SqlString  varchar(8000),
		@ReportColumns varchar(500)

	select @CustomerID = CustomerID, @GroupID = GroupID from ecn5_communicator..Blast where BlastID = @BlastID

	set @SqlString = ''

	create table #tempA 
	(  
		EmailID int,  
		ActionDate datetime,
		ActionValue varchar(2048),
		ActionNotes  varchar(255) 
	)
  
	CREATE INDEX IX_1 on #tempA (EmailID) -- added by Sunil on 7/11/2014 - performance issues
  
	if @ReportType = 'send'
	Begin
		insert into #tempA  
		SELECT  e.EmailID, bas.SendTime as 'ActionDate', 'send' as 'ActionValue' , ''
		FROM	BlastActivitySends bas with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bas.EmailID = e.EmailID
		WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	end
	else if @ReportType = 'open'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT  e.EmailID, baop.OpenTime as 'ActionDate', baop.BrowserInfo as 'ActionValue', ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON e.EmailID = baop.EmailID
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP
		else
			insert into #tempA  
			SELECT	e.EmailID, max(baop.OpenTime), max(baop.BrowserInfo), ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON baop.EmailID = e.EmailID
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP 
			group by e.EmailID

		set @ReportColumns = ' #tempA.ActionDate as OpenTime, #tempA.ActionValue as Info '
	End
	else if @ReportType = 'noopen'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, '' as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock) --left join BlastActivityOpens baop with(nolock) on bas.EmailID = baop.EmailID			
			WHERE   bas.BlastID=@blastID and bas.EmailID not in (select EmailID from BlastActivityOpens where BlastID = @blastID)
			--WHERE   baop.EmailID is null and bas.BlastID=@blastID			
			
		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'click'
	Begin
		if @FilterType = 'all'
		begin
			if @StartDate is not null and @EndDate is not null
			begin
				insert into #tempA  
				SELECT	e.EmailID, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and bacl.ClickTime between @StartDate and @EndDate
			end
			else
				insert into #tempA  
				SELECT	e.EmailID, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP 
		end
		else
			if @StartDate is not null and @EndDate is not null
				insert into #tempA  
				SELECT	e.EmailID, max(bacl.ClickTime) as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				WHERE	BlastID=@blastID AND e.emailaddress like '%' + @ISP and bacl.ClickTime between @StartDate and @EndDate
				group by e.EmailID , bacl.URL
			else
				insert into #tempA  
				SELECT	e.EmailID, max(bacl.ClickTime) as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				WHERE	BlastID=@blastID AND e.emailaddress like '%' + @ISP 
				group by e.EmailID , bacl.URL

		set @ReportColumns = ' #tempA.ActionDate as ClickTime, #tempA.ActionValue as Link '
	End
	else if @ReportType = 'noclick'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, bas.SendTime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock)-- left join BlastActivityClicks bacl with(nolock) on bas.EmailID = bacl.EmailID			
			WHERE	bas.BlastID = @blastID and EmailID not in (select EmailID from BlastActivityClicks where BlastID = @blastID)
			--WHERE	bacl.EmailID is null and bas.BlastID = @blastID

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'bounce'
	Begin
		insert into #tempA  
		SELECT	e.EmailID, babo.BounceTime as 'ActionDate', bc.BounceCode as 'ActionValue', babo.BounceMessage as 'ActionNotes'
		FROM	BlastActivityBounces babo 
				join ecn5_communicator..Emails e on e.EmailID = babo.EmailID
				join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID
		WHERE	e.emailaddress like '%' + @ISP and
				babo.BounceID in 
				(
					select 	max(BounceID) as BounceID
					FROM	BlastActivityBounces babo with(nolock) join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID
					WHERE	BlastID = @blastID
							AND bc.BounceCode = (case when len(ltrim(rtrim(@FilterType))) > 0 then @FilterType else bc.BounceCode end) 
					group by babo.emailID
				)	
	
		set @ReportColumns = ' #tempA.ActionDate as BounceTime, #tempA.ActionValue as BounceType, #tempA.ActionNotes as BounceSignature '
	end
	else if @ReportType = 'unsubscribe'
	Begin
		insert into #tempA  
		SELECT  e.EmailID, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue', baus.Comments as 'ActionNotes'
		FROM   
				BlastActivityUnSubscribes baus with(nolock)
				join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID  
				join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
		where	baus.BlastID = @blastID AND usc.UnsubscribeCode = @FilterType  
				and e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, REPLACE(REPLACE(#tempA.ActionNotes, CHAR(13), '' ''), CHAR(10), '' '') as Reason '
		
	end
	else if @ReportType = 'resend'
	Begin
		insert into #tempA  
		SELECT  e.EmailID, bars.ResendTime as 'ActionDate', 'resend' as ActionValue, ''
		FROM   
				BlastActivityResends bars with(nolock) join ecn5_communicator..emails e with(nolock) on e.EmailID = bars.emailID 
		where	bars.BlastID = @blastID and e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as ResentTime, #tempA.ActionValue as Action '		
	end
	else if @ReportType = 'suppressed'
	Begin
		if(@FilterType = 'ALL')
		begin
			insert into #tempA  
			SELECT  distinct bas.EmailID, eal.ActionDate as ActionDate, '', ''
			FROM	
			BlastActivitySuppressed bas 
			JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EmailID 
			join ECN5_COMMUNICATOR..EmailActivityLog eal on bas.BlastID = eal.BlastID
			WHERE	bas.BlastID=@blastID and e.emailaddress like '%' + @ISP
		end
		else
		begin
			insert into #tempA  
			SELECT  distinct bas.EmailID, eal.ActionDate as 'ActionDate', '', ''
			FROM	BlastActivitySuppressed bas JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EMailID 
					join SuppressedCodes sc on sc.SuppressedCodeID = bas.SuppressedCodeID 
					join ECN5_COMMUNICATOR..EmailActivityLog eal on bas.BlastID = eal.BlastID
			WHERE	bas.BlastID=@blastID and sc.SupressedCode = @FilterType and e.emailaddress like '%' + @ISP 
			select * from #tempA
		end
		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
 	declare @g table(GID int, ShortName varchar(50), datafieldsetID int)  
 	      
	create table #E(EmailID int, GID int, DataValue varchar(500), EntryID uniqueidentifier, datafieldsetID int, ShortName varchar(50))
	CREATE NONCLUSTERED INDEX #E_ind on  #E(EmailID) 

	insert into @g   
	select GroupDatafieldsID, ShortName, DatafieldSetID from ecn5_communicator..groupdatafields where GroupDatafields.groupID = @GroupID and GroupDatafields.IsDeleted=0       

	
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
		' from  ecn5_communicator..Emails join ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
		' join #tempA on #tempA.EmailID = Emails.EmailID '+  
		' where Emails.CustomerID = ' + @CustomerID + 
		' and EmailGroups.GroupID = ' + @GroupID +  
		' order by #tempA.ActionDate desc, emailaddress')  
	End  
	else
	Begin
		insert into #E   
		select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID, datafieldsetID, ShortName 
		from ecn5_communicator..EmailDataValues join @g g on g.GID = EmailDataValues.GroupDataFieldsID join #tempA on #tempA.EmailID = EmailDataValues.EmailID 
		
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID  and IsDeleted=0  AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID  and IsDeleted=0  AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						SELECT EmailID as tmp_EmailID,  ShortName, DataValue
						from	#E
						where 
								datafieldsetID is null
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
					SELECT EmailID as tmp_EmailID1, EntryID, ShortName, DataValue
					from	#E 
					where 
							datafieldsetID > 0
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
			' from ecn5_communicator..Emails 
					join ' +       
			' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +    
			 ' join #tempA on #tempA.EmailID = Emails.EmailID '+    
			' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID +
			' order by #tempA.ActionDate desc, emailaddress')  


	end

	 drop table #tempA  
	 drop table #E  

	SET NOCOUNT OFF

END