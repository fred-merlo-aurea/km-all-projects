--2014-10-24 MK Added  WITH (NOLOCK) hints

CREATE proc [dbo].[e_EmailActivityLog_InsertSpamFeedback] 
(
	@ebIDs 	varchar(8000), 
	@ActionTypeCode varchar(100),
	@ActionNotes varchar(355),
	@SubscribeTypeCode varchar(1),
	@source varchar(200) = 'e_EmailActivityLog_InsertSpamFeedback' 
)
AS
		 
BEGIN

	--testing queries
	--select * from Emails WITH(NOLOCK) where EmailID = 218801477
	--select * from Emails WITH(NOLOCK) where EmailAddress = 'jmkmtest1@gmail.com' and CustomerID = 2806
	--select e.EmailAddress, e.CustomerID, eg.* from EmailGroups eg WITH(NOLOCK) join Emails e WITH(NOLOCK) on eg.EmailID = e.EmailID where EmailAddress = 'jmkmtest1@gmail.com' and CustomerID in (2806, 2807)
	--select * from Groups where GroupID = 32487
	--select * from Blast where BlastID = 800339
	--delete Emails where EmailID = 218806856
	--delete Emailgroups where EmailID = 218806856
	
	SET NOCOUNT ON

	DECLARE @E  TABLE 
	(
		emailID int,
		blastID	int,
		customerID	int,
		mastergroupID	int,
		emailAddress varchar(255)	
	)

	--insert emailid and blast id into temp table
	insert into @e (
		emailID, 
		blastID)
	select 
		substring(Items, 1, charindex('|', items)-1), 
		substring(Items, charindex('|', Items)+1, len(Items)) 
	from 
		dbo.fn_split(@ebIDs,',')
	
	--delete if already exists 
	--Delete e
	--from @e e join ECN_ACTIVITY..BlastActivityBounces b on e.EmailID = b.EmailID and e.BlastID = b.BlastID
	
	--Delete e
	--from @e e join EmailActivityLog eal on e.EmailID = eal.EmailID and e.BlastID = eal.BlastID and ActionTypeCode = @ActionTypeCode
	--new for finding the MSCustomerID-------------------------------------------------
	--set the MSCustomerID from the customer or the base channel or from the email, in that order
	update @e 
	set 
		customerID = case when c.MSCustomerID is not null then c.MSCustomerID when bc.MSCustomerID is not null then bc.MSCustomerID else e.CustomerID end, 
		emailAddress = e.EmailAddress
	from 
		@e e1 
		join emails e with (nolock) on e1.emailID = e.emailID
		join ecn5_accounts..Customer c with(nolock) on e.customerID = c.CustomerID
		join ecn5_accounts..Basechannel bc with(nolock) on c.BaseChannelID = bc.BaseChannelID	
	
	--set the MSEmailID from the MSCustomerID, if it doesn't exist...add it	
	--if the email address doesn't exist for the MSCustomerID add it
	insert into Emails(
		EmailAddress, 
		CustomerID, 
		DateAdded, 
		BounceScore,
		SoftBounceScore)
	select 
		e1.EmailAddress, 
		e1.CustomerID, 
		GETDATE(), 
		0,
		0
	from
		@e e1
		left outer join Emails e  WITH (NOLOCK) on e1.EmailAddress = e.EmailAddress and e1.CustomerID = e.CustomerID
	where
		e.EmailID is null
	-- update the emailid for the MSCustomerID
	update @e
	set 
		EmailID = e.EmailID
	from 
		@e e1
		join Emails e with (nolock) on e1.emailAddress = e.EmailAddress and e1.customerID = e.CustomerID	

	--update the mastergroupid for the MSCustomerID
	update @e 
	set 
		mastergroupID = g.groupID	
	from 
		@e e1 
		join groups g  WITH (NOLOCK) on e1.customerID = g.customerID  and g.MasterSupression = 1
	--select * from @e	
	
	declare @dateChanged datetime = GetDate()

	-- update emailgroups for existing records
	-- for Mastersuppersiongroup  = 'S'
	-- for other groups that are not already unsubscribed = 'M'
	-- for the others don't change
	update emailgroups 
	set 
		SubscribeTypeCode = case when e1.mastergroupID = eg.groupID then @SubscribeTypeCode when (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then 'M' else eg.SubscribeTypeCode end, 
		LastChanged = case when (e1.mastergroupID = eg.groupID) or (e1.mastergroupID <> eg.GroupID and eg.SubscribeTypeCode <> 'U') then @dateChanged else eg.LastChanged end, LastChangedSource = @source
	from 	
		emailgroups eg with (nolock)
		join @e e1 on e1.emailID = eg.emailID
	
	-- insert emailgroups if master suppression not exists
	insert into EmailGroups (
		EmailID, 
		GroupID, 
		FormatTypeCode, 
		SubscribeTypeCode, 
		CreatedOn, 
		LastChanged,
		CreatedSource)
	select 
		distinct e1.EmailID, 
		e1.mastergroupID, 
		'html', 
		@SubscribeTypeCode, 
		@dateChanged, 
		@dateChanged,
		@source
	from 
		@e e1 
		left outer join emailgroups eg with (nolock) on e1.emailID = eg.emailID and e1.mastergroupID = eg.groupID
	where 
		eg.emailgroupID is null and 
		e1.mastergroupID is not null

	-- insert unsubscribe emailActivityLog if not exists
	insert into Emailactivitylog (
		EmailID, 
		BlastID, 
		ActionTypeCode, 
		ActionDate, 
		ActionValue, 
		ActionNotes, 
		Processed) 
	select 	
		distinct e1.EmailID, 
		e1.BlastID, 
		@ActionTypeCode, 
		@dateChanged, 
		'U', 
		@ActionNotes, 
		'n'  
	from 	
		@e e1 
End