CREATE PROCEDURE [dbo].[spDownloadBlastEmails]
	@blastID varchar(10), 
	@ReportType  varchar(25),
	@FilterType varchar(25), 
	@ISP varchar(100),
	@StartDate datetime = null,
	@EndDate datetime = null,
	@ProfileFilter varchar(25) = 'ProfilePlusStandalone',
	@OnlyUnique bit = 0

AS
BEGIN 	
	SET NOCOUNT ON
	if @StartDate is not null and @EndDate is not null
	BEGIN
		set @StartDate = @StartDate + '00:00:00 '  
		 set @EndDate = @EndDate + '23:59:59'
	END

	--set @blastID = 456550
	--set @ReportType = 'send'
	--set @FilterType = ''
	--set @ISP = ''

	declare   
		@CustomerID int,  
		@GroupID int,  
		@SqlString  varchar(MAX),
		@ReportColumns varchar(MAX)

	select @CustomerID = CustomerID, @GroupID = ISNULL(GroupID,0) from ecn5_communicator..Blast where BlastID = @BlastID

	if @GroupID = 0
	BEGIN
		SET @GroupID = null
		declare @refBlastID int,										
				@layoutPlanID int,
				@IsFormTrig bit,
				@campItemID int,
				@index int = 0
		
			
			SET @refBlastID = @BlastID
			while @groupID is null and @index < 10
			BEGIN								
				SET  @layoutPlanID = null
				SET  @IsFormTrig = null
				SET @campItemID = null
			
			
				select @IsFormTrig = case when EventType in('abandon','submit') then 1 else 0 end, @campItemID = CampaignItemID , @layoutPlanID = LayoutPlanID
				from ECN5_COMMUNICATOR..LayoutPlans lp with(nolock)
				where lp.BlastID = @refBlastID and ISNULL(lp.IsDeleted,0) = 0 and lp.Status = 'y'
				
				
				if @layoutPlanID is not null and @campItemID is not null
				BEGIN
					--must be a layout plan
					select @refBlastID = BlastID 
					from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) 
					where cib.CampaignItemID = @campItemID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE IF @layoutPlanID is not null and @campItemID is null
				BEGIN
					select @layoutPlanID = MIN(LayoutPlanID), @refBlastID = MIN(bs.refblastID)
					from ECN5_COMMUNICATOR..BlastSingles bs with(nolock) 
					where bs.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				ELSE
				BEGIN		
					--must be a trigger plan
					select @refBlastID = tp.RefTriggerID 
					from ECN5_COMMUNICATOR..TriggerPlans tp with(nolock)
					where tp.BlastID = @refBlastID and ISNULL(tp.IsDeleted,0) = 0 and tp.Status = 'y'
			
					select @campItemID = CampaignItemID
					from ECN5_COMMUNICATOR..CampaignItemBlast cib with(nolock) 
					where cib.BlastID = @refBlastID
			
					select @groupID = GroupID from ECN5_COMMUNICATOR..Blast b with(nolock) where b.BlastID = @refBlastID
				END
				SET @index = @index + 1
			
			END
					
	END			

									

	set @SqlString = ''

	create table #tempA 
	(  
		EmailID int,  
		ActionDate datetime,
		ActionValue varchar(2048),
		ActionNotes  varchar(355) 
	)
     declare @SeedListIDs table(pKey int identity(1,1),EmailID int)
      
      insert into @SeedListIDs(EmailID)
      Select eg.EmailID
      from ECN5_COMMUNICATOR..EmailGroups eg with(nolock)
      join ECN5_COMMUNICATOR..Groups g with(nolock) on eg.groupid = g.GroupID
	  left outer join ECN5_COMMUNICATOR..EmailGroups eg2 with(nolock) on eg.EmailID = eg2.EmailID and eg2.GroupID = @GroupID
      where g.CustomerID = @CustomerID and isnull(g.IsSeedList,0) = 1

	CREATE INDEX IX_1 on #tempA (EmailID) -- added by Sunil on 7/11/2014 - performance issues
  
	if @ReportType = 'send'
	Begin
		insert into #tempA  
		SELECT  e.EmailID, bas.SendTime as 'ActionDate', 'send' as 'ActionValue' , ''
		FROM	BlastActivitySends bas with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bas.EmailID = e.EmailID
		left outer join @SeedListIDs eg on bas.EmailID = eg.EmailID 
		WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and eg.pKey is null

		set @ReportColumns = ' #tempA.ActionDate as SendTime '
	end
	else if @ReportType = 'open'
	Begin
		if @FilterType = 'all'
			insert into #tempA  
			SELECT  e.EmailID, baop.OpenTime as 'ActionDate', baop.BrowserInfo as 'ActionValue', ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON e.EmailID = baop.EmailID
			left outer join @SeedListIDs eg on baop.EmailID = eg.EmailID 
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and eg.pKey is null
		else
			insert into #tempA  
			SELECT	e.EmailID, max(baop.OpenTime), max(baop.BrowserInfo), ''
			FROM	BlastActivityOpens baop with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON baop.EmailID = e.EmailID
			left outer join @SeedListIDs eg on baop.EmailID = eg.EmailID 
			WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and eg.pKey is null
			group by e.EmailID

		set @ReportColumns = ' #tempA.ActionDate as OpenTime, #tempA.ActionValue as Info '
	End
	else if @ReportType = 'noopen'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, bas.SendTime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock) --left join BlastActivityOpens baop with(nolock) on bas.EmailID = baop.EmailID	
			left outer join @SeedListIDs eg on bas.EmailID = eg.EmailID	 
			WHERE   bas.BlastID=@blastID and bas.EmailID not in (select EmailID from BlastActivityOpens where BlastID = @blastID) and eg.pKey is null
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
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID 
				WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and bacl.ClickTime between @StartDate and @EndDate and eg.pKey is null
			end
			else
				insert into #tempA  
				SELECT	e.EmailID, bacl.ClickTime as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID 
				WHERE	BlastID=@blastID and e.emailaddress like '%' + @ISP and eg.pKey is null
		end
		else
			if @StartDate is not null and @EndDate is not null
				insert into #tempA  
				SELECT	e.EmailID, max(bacl.ClickTime) as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID 
				WHERE	BlastID=@blastID AND e.emailaddress like '%' + @ISP and bacl.ClickTime between @StartDate and @EndDate and eg.pKey is null
				group by e.EmailID , bacl.URL
			else
				insert into #tempA  
				SELECT	e.EmailID, max(bacl.ClickTime) as 'ActionDate', bacl.URL as 'ActionValue', ''
				FROM	BlastActivityClicks bacl with(nolock) JOIN ecn5_communicator..Emails e with(nolock) ON bacl.EmailID = e.EmailID
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID 
				WHERE	BlastID=@blastID AND e.emailaddress like '%' + @ISP and eg.pKey is null
				group by e.EmailID , bacl.URL

		set @ReportColumns = ' #tempA.ActionDate as ClickTime, #tempA.ActionValue as Link '
	End
	ELSE IF @ReportType = 'uniqueclicks'
	BEGIN
		IF @StartDate IS NOT NULL AND @EndDate IS NOT NULL
			INSERT INTO #tempA  
			SELECT	distinct				
				e.EmailID, 
				max(bacl.ClickTime) AS 'ActionDate', 
				'uniqueclicks' AS 'ActionValue', 
				''
			FROM
				BlastActivityClicks bacl WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID 
			WHERE	bacl.BlastID = @blastID AND
				(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP) 
				and bacl.ClickTime BETWEEN @StartDate AND @EndDate and eg.pKey is null
			GROUP BY 
				bacl.blastID, 
				
				e.EmailID
				
		ELSE
			INSERT INTO #tempA  
			SELECT	distinct				
				e.EmailID, 
				max(bacl.ClickTime) AS 'ActionDate', 
				'uniqueclicks' AS 'ActionValue', 
				''
			FROM
				BlastActivityClicks bacl WITH(NOLOCK) 
				JOIN ecn5_communicator..Emails e WITH(NOLOCK) ON bacl.EmailID = e.EmailID 
				left outer join @SeedListIDs eg on bacl.EmailID = eg.EmailID
			WHERE bacl.BlastID = @blastID AND
				(LEN(@ISP) = 0 OR e.emailaddress like '%' + @ISP)  and eg.pKey is null
			GROUP BY
				bacl.blastID, 
				
				e.EmailID 
		
		set @ReportColumns = ' #tempA.ActionDate as ClickTime '
	END
	else if @ReportType = 'noclick'
	Begin
			insert into #tempA  
			SELECT	bas.EmailID, bas.SendTime as 'ActionDate', '', '' 
			FROM	BlastActivitySends bas with(nolock)-- left join BlastActivityClicks bacl with(nolock) on bas.EmailID = bacl.EmailID	
			left outer join @SeedListIDs eg on bas.EmailID = eg.EmailID	 	
			WHERE	bas.BlastID = @blastID and bas.EmailID not in (select EmailID from BlastActivityClicks where BlastID = @blastID) and eg.pKey is null
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
				left outer join @SeedListIDs eg on babo.EmailID = eg.EmailID 
		WHERE	e.emailaddress like '%' + @ISP and eg.pKey is null and 
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
	if(@FilterType = 'all')
	begin
		IF @OnlyUnique = 0
		BEGIN
			insert into #tempA  
			SELECT  e.EmailID, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue', baus.Comments as 'ActionNotes'
			FROM   
					BlastActivityUnSubscribes baus with(nolock)
					join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID  
					join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
					left outer join @SeedListIDs eg on baus.EmailID = eg.EmailID 
			where	baus.BlastID = @blastID
					and e.emailaddress like '%' + @ISP  and eg.pKey is null
		END 
		
		ELSE IF @OnlyUnique = 1
		BEGIN
			insert into #tempA  
			SELECT tbl.EmailID, tbl.UnsubscribeTime AS ActionDate, tbl.UnsubscribeCode AS ActionValue, tbl.Comments AS ActionNotes
			FROM ( 
			SELECT
			e.EmailID, 
			baus.UnsubscribeTime, 
			usc.UnsubscribeCode, 
			baus.Comments,
			ROW_NUMBER() OVER (PARTITION BY e.emailID ORDER BY baus.UnsubscribeTime DESC) as num

			FROM   
					BlastActivityUnSubscribes baus with(nolock)
					join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID
					left outer join @SeedListIDs eg on baus.EmailID = eg.EmailID  
					join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
			where	baus.BlastID = @blastID
					and e.emailaddress like '%' + @ISP   and eg.pKey is null
					and bauS.Comments like '%FOR GROUP: ' + CONVERT(varchar(25),@GroupID) + '%'
				) tbl
			WHERE num = 1
			ORDER BY EmailID, ActionNotes, ActionDate		
			
		END   
		
			set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, REPLACE(REPLACE(#tempA.ActionNotes, CHAR(13), '' ''), CHAR(10), '' '') as Reason '
	end
	else
	begin
	if @OnlyUnique = 0
		begin
			insert into #tempA  
			SELECT  e.EmailID, baus.UnsubscribeTime as 'ActionDate', usc.UnsubscribeCode as 'ActionValue', baus.Comments as 'ActionNotes'
			FROM   
					BlastActivityUnSubscribes baus with(nolock)
					join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID  
					join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
					left outer join @SeedListIDs eg on baus.EmailID = eg.EmailID 
			where	baus.BlastID = @blastID AND usc.UnsubscribeCode = @FilterType  
					and e.emailaddress like '%' + @ISP     and eg.pKey is null 
		end	
		else if @OnlyUnique = 1
		BEGIN
			insert into #tempA  
			SELECT baus.EmailID, MIN(baus.UnsubscribeTime) AS ActionDate, usc.UnsubscribeCode AS ActionValue, MIN(baus.Comments) AS ActionNotes
			
			FROM   
					BlastActivityUnSubscribes baus with(nolock)
					join ecn5_communicator..emails e with(nolock) on e.EmailID = baus.emailID  
					join UnsubscribeCodes usc with(nolock) on usc.UnsubscribeCodeID = baus.UnsubscribeCodeID
					left outer join @SeedListIDs eg on baus.EmailID = eg.EmailID 
			where	baus.BlastID = @blastID
					and e.emailaddress like '%' + @ISP  and eg.pKey is null
					and usc.UnsubscribeCode = @FilterType
			GROUP BY baus.EmailID, usc.UnsubscribeCode
			ORDER BY baus.EmailID, ActionNotes, ActionDate		
			
		END   
		set @ReportColumns = ' #tempA.ActionDate as UnsubscribeTime, #tempA.ActionValue as SubscriptionChange, REPLACE(REPLACE(#tempA.ActionNotes, CHAR(13), '' ''), CHAR(10), '' '') as Reason '
		end 
	end
	else if @ReportType = 'resend'
	Begin
		insert into #tempA  
		SELECT  e.EmailID, bars.ResendTime as 'ActionDate', 'resend' as ActionValue, ''
		FROM   
				BlastActivityResends bars with(nolock) join ecn5_communicator..emails e with(nolock) on e.EmailID = bars.emailID 
				left outer join @SeedListIDs eg on bars.EmailID = eg.EmailID 
		where	bars.BlastID = @blastID and e.emailaddress like '%' + @ISP  and eg.pKey is null

		set @ReportColumns = ' #tempA.ActionDate as ResentTime, #tempA.ActionValue as Action '		
	end
	else if @ReportType = 'suppressed'
	Begin
		if(@FilterType = 'ALL')
		begin
			insert into #tempA  
			SELECT  distinct bas.EmailID, ISNULL(bas.SuppressedTime, b.SendTime) as ActionDate, '', sc.SupressedCode AS 'ActionNotes'
			FROM	
			ecn5_communicator..Blast b 
			JOIN BlastActivitySuppressed bas on b.BlastID = bas.BlastID
			JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EmailID 
			left outer join @SeedListIDs eg on bas.EmailID = eg.EmailID 
			JOIN SuppressedCodes sc WITH(NOLOCK) on sc.SuppressedCodeID = bas.SuppressedCodeID
			--join ECN5_COMMUNICATOR..EmailActivityLog eal on bas.BlastID = eal.BlastID  -- commented by Sunil on 7/11/2014 - get the time from blastactivitysuppressed; if NULL get blast.sendtime.
			WHERE	b.BlastID=@blastID and e.emailaddress like '%' + @ISP  and eg.pKey is null
		end
		else
		begin
			insert into #tempA  
			SELECT  distinct bas.EmailID, ISNULL(bas.SuppressedTime, b.SendTime) as 'ActionDate', '', sc.SupressedCode AS 'ActionNotes'
			FROM	ecn5_communicator..Blast b 
					JOIN BlastActivitySuppressed bas on b.BlastID = bas.BlastID
					JOIN ecn5_communicator..Emails e ON bas.EmailID = e.EMailID 
					left outer join @SeedListIDs eg on bas.EmailID = eg.EmailID
					join SuppressedCodes sc on sc.SuppressedCodeID = bas.SuppressedCodeID  
					--join ECN5_COMMUNICATOR..EmailActivityLog eal on bas.BlastID = eal.BlastID -- commented by Sunil on 7/11/2014 - get the time from blastactivitysuppressed; if NULL get blast.sendtime.
			WHERE	b.BlastID=@blastID and sc.SupressedCode = @FilterType and e.emailaddress like '%' + @ISP  and eg.pKey is null
			--select * from #tempA
		end
		set @ReportColumns = ' #tempA.ActionDate as SendTime, #tempA.ActionNotes AS SuppressedReason '
	End
	else if @ReportType = 'forward'
	begin
		insert into #tempA
		select e.EmailID, bar.ReferTime as 'ActionDate','refer' as ActionValue,''
		from
			ecn_Activity..BlastActivityRefer bar with(nolock)
			join ecn5_communicator..Emails e with(nolock) on e.EmailID = bar.EmailID
			left outer join @SeedListIDs eg on bar.EmailID = eg.EmailID 
		where
			bar.BlastID = @blastID and e.EmailAddress like '%' + @ISP and eg.pKey is null
		set @ReportColumns = ' #tempA.ActionDate as ForwardTime '
	end
			
	
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
		' from  ecn5_Communicator..Emails join #tempA on #tempA.EmailID = Emails.EmailID '+  
		' where ecn5_Communicator..Emails.CustomerID = ' + @CustomerID + 
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
		if(@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAll')
		BEGIN
		insert into #E   
		select EmailDataValues.EmailID, EmailDataValues.GroupDataFieldsID, DataValue, EntryID, datafieldsetID, ShortName 
		from ecn5_communicator..EmailDataValues join @g g on g.GID = EmailDataValues.GroupDataFieldsID join #tempA on #tempA.EmailID = EmailDataValues.EmailID 
		
		DECLARE @StandAloneUDFs VARCHAR(MAX)
		if(@ProfileFilter = 'ProfilePlusStandalone' or @ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @StandAloneUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID  and IsDeleted=0  AND DatafieldSetID is null ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'
		END
		DECLARE @TransactionalUDFs VARCHAR(MAX)
		if(@ProfileFilter = 'ProfilePlusAll')
		BEGIN
		SELECT  @TransactionalUDFs = STUFF(( SELECT DISTINCT '],[' + ShortName FROM ecn5_communicator..groupdatafields WHERE GroupID = @GroupID  and IsDeleted=0  AND DatafieldSetID > 0 ORDER BY '],[' + ShortName FOR XML PATH('') ), 1, 2, '') + ']'       
		END
		declare @sColumns varchar(MAX),
				@tColumns varchar(MAX),
				@standAloneQuery varchar(MAX),
				@TransactionalQuery varchar(MAX)
				
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

		END
		
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
GO

