CREATE PROCEDURE [dbo].[e_Email_UpdateEmailAddress]
	 @NewEmailAddress varchar(100), 
	 @OldEmailAddress varchar(100),  	
	 @Source varchar(100),
	 @GroupID int,
	 @CustomerID int
AS
BEGIN
	DECLARE @EmailID INT
	DECLARE @NewEmailID INT
	
	set @EmailID = 0
	set @NewEmailID = 0
	
	IF @OldEmailAddress <> @NewEmailAddress and LEN(@OldEmailAddress) > 0
	BEGIN
		IF not exists (SELECT e.EmailID FROM [Emails] e WITH (NOLOCK) WHERE e.EmailAddress = @NewEmailAddress and e.CustomerID = @CustomerID) 
		BEGIN	
			SELECT @EmailID = e.EmailID FROM [Emails] e WITH (NOLOCK) join [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID WHERE e.EmailAddress = @OldEmailAddress and eg.GroupID = @GroupID
			
			if @EmailID > 0
			BEGIN
				UPDATE [Emails] SET EmailAddress = @NewEmailAddress , DateUpdated = GETDATE() WHERE EmailID = @EmailID			
				UPDATE [EmailGroups] set LastChangedSource = @Source, LastChanged = GETDATE() WHERE EmailID = @EmailID and GroupID = @GroupID
			END
		END
		ELSE
		BEGIN
			SELECT @EmailID = e.EmailID FROM [Emails] e WITH (NOLOCK) join [EmailGroups] eg WITH (NOLOCK) ON e.EmailID = eg.EmailID WHERE e.EmailAddress = @OldEmailAddress and eg.GroupID = @GroupID
			SELECT @NewEmailID = e.EmailID FROM [Emails] e WITH (NOLOCK) WHERE e.EmailAddress = @NewEmailAddress and e.CustomerID = @CustomerID

			if @EmailID > 0 and @NewEmailID > 0
			BEGIN				
				UPDATE [EmailGroups] set LastChangedSource = @Source, LastChanged = GETDATE(), SubscribeTypeCode = 'U' WHERE EmailID = @EmailID and GroupID = @GroupID
				INSERT INTO [EmailGroups] (EmailID, GroupID, FormatTypeCode, SubscribeTypeCode, SMSEnabled, CreatedOn, CreatedSource) VALUES (@NewEmailID, @GroupID, 'html', 'S', 'True', GETDATE(), @Source)
			END
		END
	END
END