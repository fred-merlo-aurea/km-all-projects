CREATE PROCEDURE [dbo].[e_Campaign_Select_CustomerID]   
@CustomerID int
AS
	SELECT * FROM Campaign WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0