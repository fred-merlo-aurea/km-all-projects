CREATE proc dbo.sp_SaveEmailGroup
(
	@GroupID int,
	@EmailID int,
	@FormatTypeCode varchar(50)	,
	@SubscribeTypeCode varchar(50)	
)
as
Begin
	declare @emailgroupID int;
	set @emailgroupID = 0;
	
	declare @smsEnabled int;
	set @smsEnabled = 0;

	select @emailgroupID = emailgroupID from [EmailGroups] where EmailID = @EmailID AND GroupID = @GroupID
	select @smsEnabled = ISNULL(smsenabled,2) from [EmailGroups] where EmailID = @EmailID AND GroupID = @GroupID


	if @emailgroupID = 0
	Begin
		INSERT INTO [EmailGroups] ( EmailID, GroupID, FormatTypeCode, SubscribeTypeCode , CreatedOn, SMSEnabled) VALUES 
			( @EmailID, @GroupID, @FormatTypeCode, @SubscribeTypeCode, GETDATE(), 'True')
	End
	Else
	Begin
		if(@smsEnabled=1)
			BEGIN
				UPDATE [EmailGroups] 
				SET		FormatTypeCode = @FormatTypeCode , 
						SubscribeTypeCode = @SubscribeTypeCode, 
						LastChanged=getdate()
				WHERE	EmailID =@EmailID AND 
						GroupID = @GroupID
			END
		ELSE if(@smsEnabled=2)
			BEGIN
				UPDATE [EmailGroups] 
				SET		FormatTypeCode = @FormatTypeCode , 
						SubscribeTypeCode = @SubscribeTypeCode, 
						LastChanged=getdate(),
						SMSEnabled=1
				WHERE	EmailID =@EmailID AND 
						GroupID = @GroupID
			END
		ELSE if(@smsEnabled=0)
			BEGIN
				UPDATE [EmailGroups] 
				SET		FormatTypeCode = @FormatTypeCode , 
						SubscribeTypeCode = @SubscribeTypeCode, 
						LastChanged=getdate()
				WHERE	EmailID =@EmailID AND 
						GroupID = @GroupID
			END
	End

End