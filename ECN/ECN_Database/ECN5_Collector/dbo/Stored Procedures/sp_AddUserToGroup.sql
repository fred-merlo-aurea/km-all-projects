CREATE proc [dbo].[sp_AddUserToGroup]
(
	@surveyID int,
	@emailaddress varchar(100),
	@IPAddress varchar(100)
)
as
Begin
	declare @groupID int,
			@customerID int,
			@EmailID int

	set @EmailID = 0

	select @groupID = GroupID, @customerID = CustomerID from survey where SurveyID = @surveyID

	select @emailID = EmailID from ecn5_communicator..Emails where customerID =@customerID and emailaddress = @emailaddress

	if @emailID = 0
	Begin
		insert into ecn5_communicator..Emails (EmailAddress, CustomerID,Notes,  DateAdded)
		values (@emailaddress, @customerID,@IPAddress, getdate())

		set @EmailID = @@IDENTITY

		insert into ecn5_communicator..EmailGroups(EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn)
		values (@EmailID, @groupID, 'html', 'S', getdate())

	end
	else
	Begin
		if not exists (select emailgroupID from ecn5_communicator..emailgroups where emailID = @emailID and groupID = @groupID)
		Begin
			insert into ecn5_communicator..EmailGroups(EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn)
			values (@EmailID, @groupID, 'html', 'S', getdate())
		End
	end

	select @EmailID
End
