CREATE PROCEDURE [dbo].[e_LinkOwnerIndex_Select_All]   
@CustomerID int = NULL
AS
	SELECT * FROM LinkOwnerIndex WITH (NOLOCK) WHERE CustomerID = @CustomerID and IsDeleted = 0