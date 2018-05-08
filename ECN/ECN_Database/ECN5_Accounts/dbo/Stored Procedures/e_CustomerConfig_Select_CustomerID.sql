create PROCEDURE [dbo].[e_CustomerConfig_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM CustomerConfig WHERE CustomerID = @CustomerID and IsDeleted=0
