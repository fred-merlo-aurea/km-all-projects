CREATE PROCEDURE [dbo].[e_LandingPageAssign_Select_CustomerID] 
@CustomerID int
AS
	SELECT *
	FROM LandingPageAssign WITH (NOLOCK)
	WHERE CustomerID = @CustomerID