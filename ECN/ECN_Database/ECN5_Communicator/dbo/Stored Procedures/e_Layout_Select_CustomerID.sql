CREATE PROCEDURE [dbo].[e_Layout_Select_CustomerID]   
@CustomerID int = NULL
AS
	SELECT * FROM Layout WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0