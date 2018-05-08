create proc [dbo].[sp_createFilterforSingleEmails] 
(
		@customerID int,
		@UserID int,
		@FirstName varchar(50),
		@LastName varchar(50),
		@Emailaddress varchar(100)
)
as

BEGIN
 	set NOCOUNT ON

	declare @GroupID int,
			@EmailID int,
			@FilterID int
	
	select @groupID = GroupID from Groups where CustomerID = @CustomerID and groupname = 'BlastSingleEmail'

	select @EmailID	= EmailID from Emails where EmailAddress = @Emailaddress and customerID = @customerID

	if Isnull(@EmailID,0) = 0
	Begin
		insert into Emails 
			(CustomerID, FirstName, LastName, Emailaddress, DateAdded, DateUpdated)
		values
			(@CustomerID, @FirstName, @LastName, @Emailaddress, getdate(), getdate())

		set @EmailID = @@identity
	End

	if not exists (select emailID from emailgroups where emailID = @EmailID and groupID = @GroupID)
	Begin
		insert into emailgroups 
			(EmailID, GroupID, formatTypeCode, SubscribeTypeCode, CreatedOn, LastChanged)	
		values
			(@EmailID, @GroupID, 'html', 'S', getdate(), getdate())
	End

	select @FilterID = filterID from [FILTER] where customerID = @customerID and GroupID = @GroupID and FilterName = @Emailaddress
	
	if Isnull(@FilterID,0) = 0
	Begin
		insert into [FILTER] (CustomerID, CreatedUserID, groupID, filterName, whereclause, dynamicwhere, CreatedDate)
		values
			(@CustomerID, @userID, @groupID, @emailaddress, 'Emails.EmailID = ' + convert(varchar,@EmailID), '', getdate())

		set @FilterID = @@identity
	end

	select @FilterID

end
