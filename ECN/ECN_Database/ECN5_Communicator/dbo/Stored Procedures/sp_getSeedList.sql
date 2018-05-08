CREATE proc [dbo].[sp_getSeedList]
(
	@CustomerID int,
	@BlastID int
)
as
Begin
	declare @basechannelID int,
			@SeedListGroupID int,
			@IsTestBlast varchar(10),
			@blastsendCount int,
			@Domain varchar(100),
			@MailRoute varchar(100),
			@GroupID int

	set @basechannelID = -1
	set @SeedListGroupID = -1
	set @MailRoute = ''
	set @Domain = ''
	set @GroupID = 0

	select @IsTestBlast = TestBlast, @blastsendCount = sendtotal from [BLAST] where blastID = @BlastID
	
	select @Domain = RIGHT(EmailFrom, LEN(EmailFrom) - CHARINDEX('@', EmailFrom)), @GroupID = GroupID from [BLAST] where BlastID = @BlastID
	select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and m.DomainName = @Domain
	if(@MailRoute is null or LEN(@MailRoute) < 1)
	Begin
		select @MailRoute = m.MTAName from MTA m join MTACustomer mc on m.MTAID = mc.MTAID where mc.CustomerID = @CustomerID and mc.IsDefault = 'true'
	End	

	--wgh: moved this from inside the if as it wasn't getting set for the else
	select @SeedListGroupID = groupID from groups where customerID = @customerID and IsSeedList = 1	

	if @CustomerID <> 3137 and @blastsendCount = 0 and @IsTestBlast = 'N' and (exists(select csfm.ClientServiceFeatureMapID 
																							from KMPlatform..ClientServiceFeatureMap csfm WITH(NOLOCK) 
																							JOIN KMPlatform..ServiceFeature sf with(nolock)  on csfm.ServiceFeatureID = sf.ServiceFeatureID
																							WHERE csfm.ClientID = @CustomerID and sf.SFCode = 'SeedList' and csfm.IsEnabled = 1)) -- or exists (select top 1 * from ChannelSeedList where basechannelID = @basechannelID)
	Begin
		select @basechannelID = basechannelID from [ECN5_ACCOUNTS].[DBO].[CUSTOMER]  where customerID = @customerID
		--wgh: moved outside of if
		--select @SeedListGroupID = groupID from groups where customerID = @customerID and IsSeedList = 1

		if exists (select top 1 * from ChannelSeedList where basechannelID = @basechannelID)
		Begin
			-- create seed list group if not exists.
			if @SeedListGroupID = -1
			Begin
				INSERT INTO Groups ( GroupName, GroupDescription, CustomerID, FolderID, OwnerTypeCode, PublicFolder, IsSeedList ) 
				VALUES ( 'Seed List Group', 'Seed List Group',@customerID,0, 'customer' ,0,1)

				set @SeedListGroupID = @@IDENTITY 
			End

			-- create emails from ChannelSeedlist to Emails if not exists
			insert into Emails (EmailAddress, customerID, DateAdded)
			select c.emailaddress, @customerID, getdate() from ChannelSeedList c left outer join emails e on c.emailaddress = e.emailaddress and e.customerID = @customerID
			where basechannelID = @basechannelID  and e.emailID is null

			-- create emails from ChannelSeedlist to Emailgroups if not exists
			if @SeedListGroupID > 0
			Begin
				insert into Emailgroups (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, CreatedOn,LastChanged)
				select e.emailID, @seedlistgroupID, 'html', 'S', getdate(), getdate() 
				from ChannelSeedList c join emails e on c.emailaddress = e.emailaddress left outer join
					 emailgroups eg on e.emailID = eg.emailID and eg.groupID = @SeedListGroupID
				where basechannelID = @basechannelID and e.customerID = @customerID and eg.emailgroupID is null
			End
		End

	End
	
	declare @RunTime datetime
	set @RunTime = GETDATE()
	insert into BlastSeedlistHistory(BlastID, EmailID, GroupID, RunTime)
	select	@BlastID, e.emailID, @SeedListGroupID, @RunTime
	from emailgroups eg join emails e on eg.emailID = e.emailID 
	where  e.customerID = @customerID and eg.groupID = @SeedListGroupID and eg.groupID > 0	

	select	e.emailID, 
			e.emailaddress, 
			@blastID as blastID,
			@MailRoute as mailRoute,
			@GroupID as groupID, 
			@customerID as CustomerID, 
			eg.formattypecode, 
			eg.subscribetypecode,
			'eid=' + convert(varchar,e.emailID) + '&bid_' + convert(varchar,@blastID)  as ConversionTrkCDE,
			'bounce_' + convert(varchar,e.emailID) + '-' as BounceAddress
	from emailgroups eg join emails e on eg.emailID = e.emailID 
	where  e.customerID = @customerID and eg.groupID = @SeedListGroupID and eg.groupID > 0 and eg.SubscribeTypeCode = 'S'
End