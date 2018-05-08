CREATE PROCEDURE [dbo].[CreateNewProfileWithEmailAddress] 
	@EmailAddress VARCHAR(250), @CustomerID INT, @GroupID INT
AS
BEGIN
	DECLARE @EmailID int
	SET @EmailID = 0
		
	SELECT @EmailID = EmailID from emails where EmailAddress = @EmailAddress and CustomerID = @CustomerID
		
	if @EmailID  =  0
	begin
		INSERT INTO Emails(EmailAddress,CustomerID) VALUES (@EmailAddress,@CustomerID)   
		SELECT @EmailID = SCOPE_IDENTITY() 	
    end         
        	
	IF @GroupID > 0
	BEGIN		
		INSERT INTO EmailGroups(EmailID,GroupID,FormatTypeCode,SubscribeTypeCode,CreatedOn,LastChanged)   
			VALUES (@EmailID, @GroupID, 'html', 'S', GETDATE(), null)      
	END   			 
	
END
