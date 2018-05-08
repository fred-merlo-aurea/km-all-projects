CREATE PROCEDURE [dbo].[spDownloadBlastGroupDetails](
	@ID varchar(10), 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100) 	
	)
AS
BEGIN 	
	SET NOCOUNT ON
	
	Create table #blasts (blastID int, GroupID int, unique clustered (blastID, GroupID))

	declare    
		@SqlString  varchar(8000),
		@ReportColumns varchar(500)

	set @SqlString = ''

	create table #tempA 
	(  
		BlastID int,
		GroupID int,
		EmailID int,  
		ActionDate datetime,
		ActionValue varchar(255),
		ActionNotes  varchar(255)
	)
	
	insert into #blasts
	select blastID, groupID from ecn5_communicator.dbo.[BLAST]
	Where BlastID in 
		(select items from	 ecn5_communicator.dbo.fn_Split((select blastIDs from ecn5_communicator..blastgrouping where blastgroupID = @ID), ','))
	
  
	if @ReportType = 'send'
	Begin
		insert into #tempA  
		SELECT  bs.blastID, bs.groupID, e.EmailID, bas.SendTime as 'ActionDate', 'send' as 'ActionValue' , ''
		FROM	BlastActivitySends bas with(nolock) JOIN 
				ecn5_communicator..Emails e with(nolock) ON bas.EmailID = e.EmailID join
				#blasts bs on bs.blastID = bas.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	end
	else if @ReportType = 'open'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT  bs.blastID, bs.groupID, e.EmailID, baop.OpenTime as 'ActionDate', baop.BrowserInfo as 'ActionValue', ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON e.EmailID = baop.EmailID join
				#blasts bs on bs.blastID = baop.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)
		else
			insert into #tempA  
			SELECT	bs.blastID, bs.groupID, e.EmailID, max(baop.OpenTime), max(baop.BrowserInfo), ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON baop.EmailID = e.EmailID join
					#blasts bs on bs.blastID = baop.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)
		group by bs.blastID, bs.groupID, e.EmailID

		set @ReportColumns = ' #tempA.ActionDate as OpenTime, #tempA.ActionValue as Info '
	End
	else if @ReportType = 'noopen'
	Begin
			insert into #tempA  
			SELECT	bs.blastID, bs.groupID, bas.EmailID, sendtime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock)  join
					#blasts bs on bs.blastID = bas.BlastID	
			WHERE  bas.EmailID not in (select EmailID from BlastActivityOpens bao  join #blasts bs on bs.blastID = bao.BlastID)
			
		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'click'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT	bs.blastID, bs.groupID, e.EmailID, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue', ''
			FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID join
				#blasts bs on bs.blastID = bacl.BlastID
			WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)
		else
			insert into #tempA  
			SELECT	bs.blastID, bs.groupID, e.EmailID, max(bacl.ClickTime) as 'ActionDate', bacl.URL as 'ActionValue', ''
			FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID join
					#blasts bs on bs.blastID = bacl.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)
			group by bs.blastID, bs.groupID, e.EmailID , bacl.URL

		set @ReportColumns = ' #tempA.ActionDate as ClickTime, #tempA.ActionValue as Link '
	End
	else if @ReportType = 'noclick'
	Begin
			insert into #tempA  
			SELECT	bs.blastID, bs.groupID, bas.EmailID, bas.SendTime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock)  join
					#blasts bs on bs.blastID = bas.BlastID
			WHERE	EmailID not in (select EmailID from BlastActivityClicks bac  join #blasts bs on bs.blastID = bac.BlastID)

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	End
	else if @ReportType = 'bounce'
	Begin
		insert into #tempA  
		SELECT	inn.blastID, inn.groupID, e.EmailID, babo.BounceTime as 'ActionDate', bc.BounceCode as 'ActionValue', babo.BounceMessage as 'ActionNotes'
		FROM	BlastActivityBounces babo 
				join ecn5_communicator..Emails e on e.EmailID = babo.EmailID
				join BounceCodes bc on bc.BounceCodeID = babo.BounceCodeID join
				(
					select 	bs.blastID, bs.groupID,max(BounceID) as BounceID
					FROM	BlastActivityBounces babo1 with(nolock)   join #blasts bs on bs.blastID = babo1.BlastID join BounceCodes bc1 on bc1.BounceCodeID = babo1.BounceCodeID
					WHERE	(len(ltrim(rtrim(@FilterType))) = 0 or  bc1.BounceCode = @FilterType) 
					group by bs.blastID, bs.groupID,babo1.emailID
				) inn on inn.BounceID = babo.BounceID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP) 		
	
		set @ReportColumns = ' #tempA.ActionDate as BounceTime, #tempA.ActionValue as BounceType, #tempA.ActionNotes as BounceSignature '
	end
	else if @ReportType = 'unsubscribe'
	Begin
		insert into #tempA  
		SELECT  bs.blastID, bs.groupID, e.EmailID, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue', baus.Comments as 'ActionNotes'
		FROM   
				BlastActivityUnSubscribes baus with(nolock)
				join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID  
				join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID join
					#blasts bs on bs.blastID = baus.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP) AND usc.UnsubscribeCode = @FilterType 

		set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, #tempA.ActionNotes as Reason '
		
	end
	else if @ReportType = 'resend'
	Begin
		insert into #tempA  
		SELECT  bs.blastID, bs.groupID, e.EmailID, bars.ResendTime as 'ActionDate', 'resend' as ActionValue, ''
		FROM   
				BlastActivityResends bars with(nolock) join ecn5_communicator..emails e with(nolock) on e.EmailID = bars.emailID  join
				#blasts bs on bs.blastID = bars.BlastID
		WHERE	(len(@ISP) = 0 or e.emailaddress like '%' + @ISP)

		set @ReportColumns = ' #tempA.ActionDate as ResentTime, #tempA.ActionValue as Action '		
	end

 	declare @g table(GID int, ShortName varchar(50))        
	insert into @g   
	select	GroupDatafieldsID, ShortName 
	from	ecn5_communicator..groupdatafields gdf join
			#blasts b on b.GroupID = gdf.GroupID

	--if @groupID = 0
	--Begin
	--	exec( 'select EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
	--	  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
	--	' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
	--	' DateAdded, DateUpdated ' +  
	--	' from  Emails join #tempA on #tempA.EmailID = Emails.EmailID '+  
	--	' where Emails.CustomerID = ' + @CustomerID + 
	--	' order by #tempA.ActionDate desc, emailaddress')  

	--end
	
	CREATE NONCLUSTERED INDEX idx_tempA_1 ON #tempA (EmailID, groupID)
	
	if not exists(select top 1 * from @g)
	Begin  
		exec( 'select EmailAddress, ' + @ReportColumns + ', Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
		  ' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
		' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
		' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' +  
		' from  ecn5_communicator.dbo.Emails '+
		' join ecn5_communicator.dbo.EmailGroups on EmailGroups.EmailID = Emails.EmailID ' +   
		' join #tempA on #tempA.EmailID = Emails.EmailID and emailgroups.groupID = #tempA.groupID'+  
		' order by #tempA.ActionDate desc, emailaddress')  
	End  
	else
	Begin

		DECLARE @StandAloneUDFs VARCHAR(2000)
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields gdf join #blasts g on g.GroupID = gdf.GroupID where DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		DECLARE @TransactionalUDFs VARCHAR(2000)
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields gdf join #blasts g on g.GroupID = gdf.GroupID where DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		
		declare @sColumns varchar(200),
				@tColumns varchar(200),
				@standAloneQuery varchar(4000),
				@TransactionalQuery varchar(4000)
				
		set @sColumns = ''
		set @tColumns= ''
		set @standAloneQuery = ''
		set @TransactionalQuery = ''		
				
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
								#tempA on #tempA.EmailID = edv.EmailID join #blasts g on g.GroupID = gdf.GroupID
						where datafieldsetID is null
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
							ecn5_communicator..Groupdatafields gdf on edv.GroupDatafieldsID = gdf.GroupDatafieldsID join
								#tempA on #tempA.EmailID = edv.EmailID join #blasts g on g.GroupID = gdf.GroupID
					where datafieldsetID > 0
				 ) u
				 PIVOT
				 (
				 MAX (DataValue)
				 FOR ShortName in (' + @TransactionalUDFs + ')) as pvt 			
			) 
			TUDFs on Emails.emailID = TUDFs.tmp_EmailID1 '
		End
		
		exec ('select EmailAddress, #tempA.blastID, #tempA.groupID, ' + @ReportColumns + ' , Title, FirstName, LastName, FullName, Company, Occupation, Address, Address2, ' +  
			' City, State, Zip, Country, Voice, Mobile, Fax, Website, Age, Income, Gender, ' +  
			' User1, User2, User3, User4, User5, User6, Birthdate, UserEvent1, UserEvent1Date, UserEvent2, UserEvent2Date, ' +  
			' CreatedOn, LastChanged, FormatTypeCode, SubscribeTypeCode ' + @sColumns + @tColumns +  
			' from  ' +       
			' ecn5_communicator..Emails join '+    
			' ecn5_communicator..EmailGroups on EmailGroups.EmailID = Emails.EmailID ' + @standAloneQuery + @TransactionalQuery +    
			' join #tempA on #tempA.EmailID = Emails.EmailID and emailgroups.groupID = #tempA.groupID'+  
			' order by #tempA.ActionDate desc, emailaddress')  


	end

	 drop table #tempA 
	 drop table #blasts 

	SET NOCOUNT OFF

END
