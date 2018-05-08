CREATE PROCEDURE [dbo].[e_Email_Exists_ByEmailAddress] 
	@EmailAddress varchar(100),
	@CustomerID int,
	@EmailID int
AS     
BEGIN     		
	
declare @NewEmailID int


	IF EXISTS (SELECT TOP 1 EmailID FROM [Emails] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND EmailAddress = @EmailAddress and EmailID<>@EmailID)
	BEGIN
		select @NewEmailID=q1.EmailID from 
		(SELECT TOP 1 EmailID FROM [Emails] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND EmailAddress = @EmailAddress and EmailID<>@EmailID) q1
		IF EXISTS (
					select GroupID from EmailGroups
					where EmailID=@NewEmailID OR EmailID=@EmailID
					group by GroupID
					having COUNT(GroupID)= 2
				  )
		BEGIN
			SELECT 1
		END
	ELSE
		BEGIN
			SELECT 0
		END
	END
	ELSE 
	BEGIN
		 SELECT 0
	END
END