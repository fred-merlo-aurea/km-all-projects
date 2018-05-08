CREATE PROCEDURE [dbo].[e_CustomerTemplate_Select_CustomerID]   
@CustomerID int = NULL
AS
	SELECT * FROM CustomerTemplate WHERE CustomerID = @CustomerID and IsActive = 1 and IsDeleted = 0
