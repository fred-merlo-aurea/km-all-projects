CREATE PROCEDURE [dbo].[e_Group_Exists_ByID] 
	@GroupID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 GroupID FROM [Groups] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND GroupID = @GroupID) SELECT 1 ELSE SELECT 0
END