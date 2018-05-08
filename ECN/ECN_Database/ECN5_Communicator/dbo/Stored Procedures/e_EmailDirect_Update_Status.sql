CREATE PROCEDURE [dbo].[e_EmailDirect_Update_Status]
	@EmailDirectID int,
	@Status varchar(20)
AS
	if(@Status = 'Active')
	BEGIN
		Update EmailDirect
		set StartTime = GETDATE(), Status = @Status
		where EmailDirectID = @EmailDirectID
	END
	ELSE IF (@Status = 'Sent')
	BEGIN
		Update EmailDirect
		set FinishTime = GETDATE(), Status = @Status
		WHERE EmailDirectID = @EmailDirectID
	END
	ELSE IF(@Status = 'Opened')
	BEGIN
		Update EmailDirect
		set OpenTime = GETDATE(), Status = @Status
		WHERE EmailDirectID = @EmailDirectID
	END
	ELSE
	BEGIN
		Update EmailDirect
		set Status = @Status
		where EmailDirectID = @EmailDirectID
	END