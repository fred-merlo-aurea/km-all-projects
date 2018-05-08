CREATE PROCEDURE [dbo].[sp_UpdateEmailAddress]
	 @NewEmailAddress varchar(100), 
	 @OldEmailAddress varchar(100),  	
	 @GroupID int	
AS
BEGIN
	DECLARE @EmailID INT
	
	set @EmailID = 0
	
	IF @OldEmailAddress <> @NewEmailAddress and LEN(@OldEmailAddress) > 0
	BEGIN
		IF not exists (SELECT e.EmailID FROM Emails e join EmailGroups eg ON e.EmailID = eg.EmailID	WHERE e.EmailAddress = @NewEmailAddress and eg.GroupID = @GroupID) 
		BEGIN	
			SELECT @EmailID = e.EmailID FROM Emails e join EmailGroups eg ON e.EmailID = eg.EmailID WHERE e.EmailAddress = @OldEmailAddress and eg.GroupID = @GroupID
			
			if @EmailID > 0
				UPDATE Emails SET EmailAddress = @NewEmailAddress WHERE EmailID = @EmailID			
		END
	END

END
