CREATE PROCEDURE [dbo].[e_Role_Select_CustomerID]
@CustomerID int
AS
SELECT *
FROM 
	[Role] WITH(NOLOCK)
WHERE 
	CustomerID = @CustomerID AND
	IsDeleted = 0
