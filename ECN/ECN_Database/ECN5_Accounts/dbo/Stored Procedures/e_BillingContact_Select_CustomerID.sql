create PROCEDURE [dbo].[e_BillingContact_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM BillingContact WHERE CustomerID = @CustomerID and IsDeleted=0
