create PROCEDURE [dbo].[e_CustomerLicense_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM CustomerLicense WHERE CustomerID = @CustomerID and IsDeleted=0
