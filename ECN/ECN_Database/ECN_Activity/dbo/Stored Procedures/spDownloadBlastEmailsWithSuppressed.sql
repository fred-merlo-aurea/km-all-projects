CREATE PROCEDURE [dbo].[spDownloadBlastEmailsWithSuppressed](
	@blastID varchar(10), 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100))
AS
BEGIN
	SET NOCOUNT ON

	declare
		@CustomerID int,  
		@GroupID int,  
		@SqlString  varchar(8000),
		@ReportColumns varchar(500)

	select @CustomerID = CustomerID, @GroupID = GroupID from ecn5_communicator..[BLAST] where BlastID = @BlastID

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
		SELECT  bas.EmailID, bas.SendTime as 'ActionDate', '', ''
		FROM	BlastActivitySends bas with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bas.EmailID = e.EMailID
		WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	end
	else if @ReportType = 'suppressed'
	Begin
		if(@FilterType = 'ALL')
		begin
			insert into #tempA  
			SELECT  bas.EmailID, '' as ActionDate, '', ''
			FROM	BlastActivitySuppressed bas JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EmailID 
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP
		end
		else
		begin
			insert into #tempA  
			SELECT  bas.EmailID, '' as 'ActionDate', '', ''
			FROM	BlastActivitySuppressed bas JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EMailID 
					join SuppressedCodes sc on sc.SuppressedCodeID = bas.SuppressedCodeID 
			WHERE	BlastID=@blastID and sc.SupressedCode = @FilterType and e.emailaddress like '%' + @ISP 
			
		end

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'open'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT  baop.EmailID, baop.OpenTime as 'ActionDate', baop.BrowserInfo as 'ActionValue', ''
			FROM	BlastActivityOpens baop JOIN ecn5_communicator..Emails e ON e.EmailID = baop.EmailID  
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP
		else
			insert into #tempA  
			SELECT	baop.EmailID, max(baop.OpenTime), max(baop.BrowserInfo), ''
			FROM	BlastActivityOpens baop JOIN ecn5_communicator..Emails e ON baop.EMailID=e.EMailID  
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP 
			group by baop.EmailID

		set @ReportColumns = ' #tempA.ActionDate as OpenTime, #tempA.ActionValue as Info '
	End
	else if @ReportType = 'noopen'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, bas.SendTime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas 
					left join BlastActivityClicks bacl on bacl.BlastID = bas.BlastID and bas.EmailID = bacl.EmailID
					left join BlastActivityOpens baop on baop.BlastID = bas.BlastID and baop.EmailID = bas.EmailID					
			WHERE	bas.BlastID = @blastID and bacl.ClickID is null and baop.OpenID is null

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'click'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT	bacl.EmailID, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue', ''
			FROM	BlastActivityClicks bacl JOIN ecn5_communicator..Emails e ON bacl.EmailID = e.EMailID
			WHERE	bacl.BlastID = @blastID and e.emailaddress like '%' + @ISP
		else
			insert into #tempA  
			SELECT bacl.EmailID, max(bacl.ClickTime), bacl.URL as 'ActionValue', ''
			FROM BlastActivityClicks bacl JOIN ecn5_communicator..Emails e ON bacl.EmailID = e.EmailID
			WHERE BlastID = @blastID and e.emailaddress like '%' + @ISP 
			group by bacl.EmailID, bacl.URL

		set @ReportColumns = ' #tempA.ActionDate as ClickTime, #tempA.ActionValue as Link '
	End
	else if @ReportType = 'noclick'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, bas.SendTime as 'actiondate', '', '' 
			FROM	BlastActivitySends bas left join BlastActivityClicks bacl on bas.BlastID = bacl.BlastID and bacl.EmailID = bas.EmailID
			WHERE   bas.BlastID=@blastID and bacl.ClickID is null
						
		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'bounce'
	Begin
		insert into #tempA  
		SELECT	babo.EmailID, babo.BounceTime as 'ActionDate', bc.BounceCode as 'ActionValue', babo.BounceMessage as 'ActionNotes' 
		FROM	BlastActivityBounces babo 
				join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID
				join ecn5_communicator..Emails e on babo.EmailID = e.EmailID
		WHERE	e.emailaddress like '%' + @ISP and
				babo.eaid in 
				(
					select 	max(EAID) as EAID
					FROM	BlastActivityBounces join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID
					WHERE	BlastID = @blastID
							AND bc.BounceCode = (case when len(ltrim(rtrim(@FilterType))) > 0 then @FilterType else bc.BounceCode end)
					group by emailID
				)	
	
		set @ReportColumns = ' #tempA.ActionDate as BounceTime, #tempA.ActionValue as BounceType, #tempA.ActionNotes as BounceSignature '
	end
	else if @ReportType = 'unsubscribe'
	Begin
		insert into #tempA  
		SELECT  baus.EmailID, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue', baus.Comments as 'ActionNotes'
		FROM   
				BlastActivityUnSubscribes baus 
				join UnsubscribeCodes usc on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
				join ecn5_communicator..emails e on baus.emailid = e.emailID  
		where	baus.BlastID = @blastID and  
				e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, #tempA.ActionNotes as Reason '
		
	end
	else if @ReportType = 'resend'
	Begin
		insert into #tempA  
		SELECT  bars.EmailID, bars.ResendTime as 'ActionDate', '' as ActionValue, '' -- ? field for actionvalue for resend
		FROM   
				BlastActivityResends bars join ecn5_communicator..emails e on bars.EmailID = e.emailID				
		where	bars.BlastID = @blastID and  
				e.emailaddress like '%' + @ISP     

		set @ReportColumns = ' #tempA.ActionDate as ResentTime, #tempA.ActionValue as Action '
	end

 	declare @g table(GID int, ShortName varchar(50))
	insert into @g   
	select GroupDatafieldsID, ShortName from ecn5_communicator..groupdatafields where GroupDatafields.groupID = @GroupID        

	if @groupID = 0
	Begin
		exec( 'select EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' DateAdded, DateUpdated ' +  
		' from  ecn5_communicator..Emails join #tempA on #tempA.EmailID = Emails.EmailID '+  
		' where ecn5_communicator..Emails.CustomerID = ' + @CustomerID + 
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
		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
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
						from	ecn5_communicator..EmailDataValues edv  join  
								ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join 
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
					from	ecn5_communicator..EmailDataValues edv  join  
							ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID  join 
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
			' from ecn5_communicator..Emails 
				join ' +    
			' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +     
			 ' join #tempA on #tempA.EmailID = Emails.EmailID '+    
			' where Emails.CustomerID = ' + @CustomerID + ' and EmailGroups.GroupID = ' + @GroupID +
			' order by #tempA.ActionDate desc, emailaddress')  


	end

	 drop table #tempA 

	SET NOCOUNT OFF


END
