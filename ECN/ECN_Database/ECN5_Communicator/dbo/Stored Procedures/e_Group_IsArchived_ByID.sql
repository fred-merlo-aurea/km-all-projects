CREATE PROCEDURE [dbo].[e_Group_IsArchived_ByID] 
	@GroupID int,
	@CustomerID int
AS     
BEGIN     		
	IF EXISTS (SELECT TOP 1 GroupID FROM [Groups] WITH (NOLOCK) WHERE CustomerID = @CustomerID AND GroupID = @GroupID AND Archived =1) SELECT 1 ELSE SELECT 0
END
