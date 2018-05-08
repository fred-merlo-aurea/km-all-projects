CREATE PROCEDURE [dbo].[e_Customer_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM Customer WITH(NOLOCK) WHERE CustomerID = @CustomerID  and IsDeleted=0
