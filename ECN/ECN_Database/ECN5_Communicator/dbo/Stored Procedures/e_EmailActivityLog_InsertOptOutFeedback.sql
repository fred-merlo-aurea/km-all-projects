CREATE PROC [dbo].[e_EmailActivityLog_InsertOptOutFeedback]
(
	@BlastID int,
	@Groups varchar (4000), 
	@EmailID int,
	@Reason varchar(100) = ''
)
AS
		 
BEGIN
	
	SET NOCOUNT ON
	
	CREATE TABLE #e  
	(
		groupID	int,
		emailID int,
		blastID int
	)
	
	CREATE INDEX idx_temp_EmailBlast on #e (EmailId,BlastId)
	CREATE INDEX idx_temp_EmailGroup on #e (EmailId,GroupId)


	if len(RTRIM(ltrim(@Reason))) > 0
	begin
		set @Reason = '. Reason: ' + RTRIM(ltrim(@Reason))
	end
	else
	begin
		set @Reason = ''
	end
	
	declare @dateChanged datetime = GetDate()
	
	insert into #e 
	select *, 
		@EmailID, 
		@BlastID 
	from 
		dbo.fn_split(@Groups,',')
	/* delete if already exists */ -- added by Sunil on 07/05/2015
	--Delete e
	--from #E e join ECN_ACTIVITY..BlastActivityBounces b on e.EmailID = b.EmailID and e.BlastID = b.BlastID
	
	--Delete e
	--from #E e join EmailActivityLog eal on e.EmailID = eal.EmailID and e.BlastID = eal.BlastID and ActionTypeCode ='Unsubscribe'
	--update emailgroups
	update 
		emailgroups 
	set 
		SubscribeTypeCode  = 'U', 
		LastChanged = @dateChanged
	from 	
		emailgroups eg with (nolock)
		join #e e1 on e1.emailID = eg.emailID and e1.groupID = eg.GroupID
	where
		eg.SubscribeTypeCode <> 'M'
	
	--insert into emailgroups
	insert into EmailGroups (
		EmailID, 
		e1.GroupID, 
		FormatTypeCode, 
		SubscribeTypeCode, 
		CreatedOn, 
		LastChanged)
	select distinct 
		e1.EmailID, 
		e1.groupID, 
		'html', 
		'U', 
		@dateChanged, 
		@dateChanged 
	from 
		#e e1 
		left outer join emailgroups eg on e1.emailID = eg.emailID and e1.groupID = eg.groupID-- and eg.SubscribeTypeCode <> 'M'
	where 
		eg.emailgroupID is null and 
		e1.groupID is not null

	---- insert emailActivityLog if not exists
	insert into Emailactivitylog (
		EmailID, 
		BlastID, 
		ActionTypeCode, 
		ActionDate, 
		ActionValue, 
		ActionNotes, 
		Processed) 
	select distinct 
		e1.EmailID, 
		e1.BlastID, 
		'subscribe', 
		@dateChanged, 
		'U', 
		'UNSUBSCRIBED THRU BLAST: ' + Convert(varchar(19),@BlastID) + ' FOR GROUP: ' + CONVERT(varchar(255), e1.groupID) + @Reason, 
		'n'  
	from 	
		#e e1 
End