CREATE PROCEDURE [dbo].[e_Email_Exists_ByID] 
	@EmailID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 EmailID FROM [Emails] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND EmailID = @EmailID) SELECT 1 ELSE SELECT 0
END