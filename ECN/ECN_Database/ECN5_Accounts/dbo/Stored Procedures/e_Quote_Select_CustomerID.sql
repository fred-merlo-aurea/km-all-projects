create PROCEDURE [dbo].[e_Quote_Select_CustomerID]
@CustomerID int
AS

SELECT * FROM Quote WHERE CustomerID = @CustomerID and IsDeleted=0
