CREATE PROCEDURE [dbo].[e_CustomerNote_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM CustomerNote WHERE CustomerID = @CustomerID and IsDeleted=0
