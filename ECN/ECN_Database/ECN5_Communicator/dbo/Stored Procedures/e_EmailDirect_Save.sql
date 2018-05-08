CREATE PROCEDURE [dbo].[e_EmailDirect_Save]
	@EmailDirectID int = null,
	@CustomerID int,
	@Source varchar(500),
	@Process varchar(500),
	@Status varchar(20),
	@SendTime datetime,
	@EmailAddress varchar(500),
	@FromEmailAddress varchar(500),
	@FromName varchar(500),
	@ReplyEmailAddress varchar(500),
	@Content varchar(MAX),
	@CreatedUserID int = null,
	@UpdatedUserID int = null,
	@EmailSubject varchar(500)
AS
	if(@EmailDirectID is null)
	BEGIN
		INSERT INTO EmailDirect(CustomerID, Source, Process, Status, SendTime, EmailAddress, FromEmailAddress, FromName, ReplyEmailAddress, Content, CreatedUserID,CreatedDate,EmailSubject)
		VALUES(@CustomerID, @Source, @Process,@Status, @SendTime, @EmailAddress, @FromEmailAddress, @FromName, @ReplyEmailAddress, @Content, @CreatedUserID, GETDATE(),@EmailSubject)
		Select @@IDENTITY;
	END
	else
	BEGIN
		Update EmailDirect
		set CustomerID = @CustomerID, Source = @Source, Process = @Process, Status = @Status, SendTime = @SendTime, EmailAddress = @EmailAddress, FromEmailAddress = @FromEmailAddress, FromName = @FromName, ReplyEmailAddress = @ReplyEmailAddress, Content = @Content, UpdatedDate = GETDATE(), UpdatedUserID = @UpdatedUserID,EmailSubject = @EmailSubject
		WHERE EmailDirectID = @EmailDirectID
	END
