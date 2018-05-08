CREATE PROCEDURE [dbo].[e_Email_Exists_ByEmailAddressAndCust]
	@EmailAddress varchar(100),
	@CustomerID int
AS
BEGIN     		
	
declare @SPEmailAddress varchar(100)
declare @SPCustomerID int

set @SPEmailAddress = @EmailAddress
set @SPCustomerID = @CustomerID

	IF EXISTS (SELECT TOP 1 EmailID FROM [Emails] WITH (NOLOCK) WHERE CustomerID = @SPCustomerID AND EmailAddress = @SPEmailAddress)
		BEGIN
			SELECT 1
		END
	ELSE 
		BEGIN
			 SELECT 0
		END
END
