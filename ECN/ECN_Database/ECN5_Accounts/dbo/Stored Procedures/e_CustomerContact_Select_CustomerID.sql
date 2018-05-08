CREATE PROCEDURE [dbo].[e_CustomerContact_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM CustomerContact WHERE CustomerID = @CustomerID  and IsDeleted=0
