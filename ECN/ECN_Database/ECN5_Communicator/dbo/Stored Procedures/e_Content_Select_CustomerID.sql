create PROCEDURE [dbo].[e_Content_Select_CustomerID]   
@CustomerID int
AS
	SELECT * FROM Content WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0